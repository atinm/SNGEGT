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
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using System.Threading;
using System.Text.RegularExpressions;

namespace SNGEGT
{
    public class Award: IComparable
    {
        public Award()
        {
            wins = new List<Double>(6);
        }
        public string name;
        public List <double> wins;
        public int playercount;

        public override string ToString()
        {
            return name;
        }

        #region IComparable Members

        public int CompareTo(object obj)
        {
            return name.CompareTo(obj);
        }

        #endregion
    }
    public class BlindSaver
    {
        public BlindSaver()
        {
            initialize();
        }

        public void initialize()
        {
            name = "";
            blinds = new List<BlindInfo>(10);
            awards = new List<Award>(5);

        }

        

        public void Save(XmlWriter writer)
        {

            writer.WriteStartElement("GameStructure");
            
            writer.WriteStartElement("Levels");
            for (int i = 0; i < blinds.Count; i++)
            {
                writer.WriteStartElement("Level");
                writer.WriteString(blinds[i].ToString());
                writer.WriteEndElement();
            }
            writer.WriteEndElement(); //levels
            

            writer.WriteStartElement("Awards");
            for (int i = 0; i < awards.Count; i++)
            {
                writer.WriteStartElement("AwardStruct");
                writer.WriteAttributeString("name", awards[i].name);
                writer.WriteAttributeString("playercount", Convert.ToString(awards[i].playercount));
                for (int j = 0; j < awards[i].wins.Count; j++)
                {
                    writer.WriteStartElement("Award");
                    writer.WriteString(Convert.ToString(awards[i].wins[j]));
                    writer.WriteEndElement();
                }
                writer.WriteEndElement(); //AwardStruct
            }
            writer.WriteEndElement(); //Awards
            writer.WriteEndElement(); //GameStruct
            

        }

        public bool Load(XmlNode node)
        {
            if (node.Name != "GameStructure")
                return false;

           
            XmlNode ChildNode, ChildNode2;
            XmlNode ParentNode = node.FirstChild;
          
            
            while (ParentNode != null)
            {
                if (ParentNode.Name == "Levels")
                {
                    ChildNode = ParentNode.FirstChild;
                    while (ChildNode != null)
                    {
                      
                        if ((ChildNode.Name == "Level") && Regex.Match(ChildNode.InnerText, @"^\d+/\d+(\+\d+)?$").Success)
                            blinds.Add(new BlindInfo(ChildNode.InnerText));

                        ChildNode = ChildNode.NextSibling;

                    }
                }

                if (ParentNode.Name == "Awards")
                {
                    ChildNode = ParentNode.FirstChild;
                    while(ChildNode != null)
                    {
                        if ((ChildNode.Name == "AwardStruct"))
                        {
                            Award aw = new Award();
                            aw.name = ChildNode.Attributes["name"].Value;
                            aw.playercount = Convert.ToInt32(ChildNode.Attributes["playercount"].Value);
                            ChildNode2 = ChildNode.FirstChild;
                            while (ChildNode2 != null)
                            {
                                if (ChildNode2.Name == "Award" && Regex.Match(ChildNode2.InnerText, @"^\d+([\.,]\d+)?$").Success)
                                {
                                    String[] muuta = new String[] { ".", "," };
                                    String text = ChildNode2.InnerText;
                                    for (int i = 0; i < muuta.Length; i++)
                                        text = text.Replace(muuta[i], System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator);

                                    aw.wins.Add(Convert.ToDouble(text));
                                }
                                ChildNode2 = ChildNode2.NextSibling;
                            }
                            aw.wins.Sort();
                            aw.wins.Reverse();
                            awards.Add(aw);

                        }

                        ChildNode = ChildNode.NextSibling;
                    }
                }
                    
                ParentNode = ParentNode.NextSibling;
            }

           
         
            return isok();

            
        }
        public bool isok()
        {
            return name != "" && blinds.Count > 0 && awards.Count > 0;
        }





        public List<BlindInfo> blinds;
        public List<Award> awards;
        public string name;
    }
}
