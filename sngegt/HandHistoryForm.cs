/*
    This file is part of SNGEGT.

    SNGEGT is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 2 of the License, or
    (at your option) any later version.

    SNGEGT is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with SNGEGT.  If not, see <http://www.gnu.org/licenses/>.
*/



using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Collections;
using SNGEGT;
using System.IO;

namespace SNGEGT
{
	/// <summary>
	/// Description of Form1.
	/// </summary>
	public partial class GameSelectForm
	{

        private List<String> tablenames;
        private GameChangedNotifier notifier;
        private Hashtable GameInfos;
        
		public GameSelectForm(GameChangedNotifier notifier)
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
			tablenames = new List <String>(10);
			this.notifier = notifier;
			
		}
		
		
		void ButtonOKClick(object sender, System.EventArgs e)
		{
			
		}
		
		public void SelectGame(String filename, List <BlindInfo> aFulltiltBlinds)
		{
			HistoryReader reader = new HistoryReader();
            iFulltiltBlinds = aFulltiltBlinds;
            reader.fulltiltblinds = iFulltiltBlinds;
            
			GameInfos = reader.ReadHistory(filename);
			//Clear old names
			tablenames.Clear();
			IEnumerator enumerator = GameInfos.Keys.GetEnumerator();
			enumerator.Reset();
			//Add new names to list
			while (enumerator.MoveNext())
				tablenames.Add((String) enumerator.Current);
			
			//J‰rjestet‰‰n lista
			tablenames.Sort();
			
			//Update tablenames to list
			listBox1.BeginUpdate();
			listBox1.Items.Clear();
			for (int i = 0; i < tablenames.Count; i++)
			{
                
				TableData gamedata = (TableData) GameInfos[tablenames[i]];
                string action ="";
                if (gamedata.SelectedAction == OwnAction.ECall)
                    action = "{Call}";
                else if (gamedata.SelectedAction == OwnAction.EFold)
                    action = "{Fold}";
                else if (gamedata.SelectedAction == OwnAction.ERaise)
                    action = "{Raise}";
                else if (gamedata.SelectedAction == OwnAction.EAll)
                    action = "{All-in}";
                else
                    action = "{Check}";

                listBox1.Items.Add(tablenames[i] +" [" +NumberToCard(gamedata.handCards[0])
				                                 +"," +NumberToCard(gamedata.handCards[1]) +"] "
                                                 +action);
			}
			listBox1.EndUpdate();
			//listBox1.Items.AddRange(
            FileInfo info = new FileInfo(filename);
            String path = info.DirectoryName;
            fileSystemWatcher1.Path = path;
            fileSystemWatcher1.Filter = info.Name;
            fileSystemWatcher1.EnableRaisingEvents = checkBox1.Checked;
            
			    
		}
		
		private String NumberToCard(int CardNumber)
		{
			String lands = "hdcs";
			int landnumber = CardNumber/13;
			char land = lands[CardNumber/13];
			CardNumber -= landnumber*13;
			String values = "23456789TJQKA";
			String result = "";
			//Debug.WriteLine(CardNumber);
			//return "";
			result += values.ToCharArray()[CardNumber];
			result += land;
			return  result;
		
			
			
		}
		


        private void listBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listBox1.SelectedIndex < 0)
                return;

            notifier.GameChanged((TableData)GameInfos[tablenames[listBox1.SelectedIndex]]);
        }

        private void fileSystemWatcher1_Changed(object sender, System.IO.FileSystemEventArgs e)
        {
            SelectGame(e.FullPath, iFulltiltBlinds);
            if (listBox1.Items.Count > 0)
                listBox1.SelectedIndex = listBox1.Items.Count - 1;

        }



        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            fileSystemWatcher1.EnableRaisingEvents = checkBox1.Checked;
        }

        private void GameSelectForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                Hide();
                e.Cancel = true;
            }
        }

        private List<BlindInfo> iFulltiltBlinds;
	}
	
}
