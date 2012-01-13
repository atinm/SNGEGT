using System;
using System.Collections.Generic;
using System.Text;

namespace HeadsUp_Trainer
{
    class Recognize
    {
        private readonly int NOTHING       = 0;
        private readonly int ONEPAIR       = 1;
        private readonly int TWOPAIRS      = 2;
        private readonly int TRIPS         = 3;
        private readonly int STRAIGHT      = 4;
        private readonly int FLUSH         = 5;
        private readonly int FULLHOUSE     = 6;
        private readonly int QUADS         = 7;
        private readonly int STRAIGHTFLUSH = 8;


        // nopein lajittelu 7 alkion kokoiselle taulukolle
        private void insertSort(int length, int[] a)
        {
            int i, j, value;

            for (i = 1; i < length; ++i)
            {
                value = a[i];
                for (j = i - 1; j >= 0 && a[j] < value; --j)
                    a[j + 1] = a[j];
                a[j + 1] = value;
            }
        }


        // Ei mitään
        private bool nothing(int[] values, int[] result)
        {
            int i;
            result[0] = NOTHING;
            for (i = 0; i < 5; i++)
                result[i + 1] = values[i];
            return true;
        }


        // Yksi pari.
        private bool onePair(int[] values, int[] result)
        {
            int pairNumber = 0, i = 0, counter = 2;
            for (i = 0; i < 6; i++)
            {
                if (values[i] == values[i + 1] && values[i] != 0)
                {
                    pairNumber = values[i];
                    break;
                }
            }
            if (pairNumber != 0)
            {
                result[0] = ONEPAIR;
                result[1] = pairNumber;
                for (i = 0; i < 7; i++)
                {
                    if (values[i] != pairNumber)
                    {
                        result[counter] = values[i];
                        counter++;
                        if (counter == 5)
                            break;
                    }
                }
                return true;
            }
            return false;
        }


        // Kaksi paria
        private bool twoPairs(int[] values, int[] result)
        {
            int firstPairNumber = 0, secondPairNumber = 0, i = 0;
            for (i = 0; i < 6; i++)
            {
                if (values[i] == values[i + 1] && values[i] != 0)
                {
                    firstPairNumber = values[i];
                    break;
                }
            }

            // Ei ole yhtään paria tai pari oli taulukon kahdessa viimeisimmässä alkiossa
            if (firstPairNumber == 0 || i == 6)
                return false;

            for (i = i + 1; i < 6; i++)
            {
                if (values[i] == values[i + 1])
                {
                    secondPairNumber = values[i];
                    break;
                }
            }

            if (firstPairNumber != 0 && secondPairNumber != 0)
            {
                result[0] = TWOPAIRS;
                result[1] = firstPairNumber;
                result[2] = secondPairNumber;
                for (i = 0; i < 7; i++)
                {
                    if (values[i] != firstPairNumber && values[i] != secondPairNumber)
                    {
                        result[3] = values[i];
                        break;
                    }
                }
                return true;
            }
            return false;
        }


        // Kolmoset.
        private bool trips(int[] values, int[] result)
        {
            int tripsNumber = 0, i = 0, counter = 2;
            for (i = 0; i < 5; i++)
            {
                if (values[i] == values[i + 2] && values[i] != 0)
                {
                    tripsNumber = values[i];
                    break;
                }
            }
            if (tripsNumber != 0)
            {
                result[0] = TRIPS;
                result[1] = tripsNumber;
                for (i = 0; i < 7; i++)
                {
                    if (values[i] != tripsNumber)
                    {
                        result[counter] = values[i];
                        counter++;
                        if (counter == 4)
                            break;
                    }
                }
                return true;
            }
            return false;
        }


        // Suora.
        private bool straight(int[] values, int[] result)
        {
            int straightNumber = 0, i = 0, counter = 0;
            int[] valuesUnique ={ values[0], 0, 0, 0, 0, 0, 0, 0 };

            for (i = 1; i < 7; i++)
            {
                if (values[i] != valuesUnique[counter])
                {
                    valuesUnique[counter + 1] = values[i];
                    counter++;
                }
            }

            if (values[0] == 14)
                valuesUnique[counter + 1] = 1;

            for (i = 0; i < 4; i++)
            {
                if (valuesUnique[i] - valuesUnique[i + 4] == 4 && valuesUnique[i + 4] != 0)
                {
                    straightNumber = valuesUnique[i];
                    break;
                }
            }

            if (straightNumber != 0)
            {
                result[0] = STRAIGHT;
                result[1] = straightNumber;
                return true;
            }
            return false;
        }


        // Väri.
        private bool flush(int[] numbers, int[] result)
        {
            int suit = 99, i = 0, counter = 1;
            int[] suits = { 0, 0, 0, 0 };

            for (i = 0; i < 7; i++)
                if (numbers[i] != 99)
                    suits[numbers[i] / 13]++;

            for (i = 0; i < 4; i++)
            {
                if (suits[i] >= 5)
                {
                    suit = i;
                    break;
                }
            }

            if (suit != 99)
            {
                result[0] = FLUSH;
                for (i = 0; i < 7; i++)
                {
                    if (numbers[i] != 99 && numbers[i] / 13 == suit)
                    {
                        result[counter] = (numbers[i] % 13) + 2;
                        counter++;
                    }
                    if (counter == 6)
                        return true;
                }
            }

            return false;
        }


        // Täyskäsi.
        private bool fullHouse(int[] values, int[] result)
        {
            int tripsNumber = 0, pairNumber = 0, i;
            for (i = 0; i < 5; i++)
            {
                if (values[i] == values[i + 2] && values[i] != 0)
                {
                    tripsNumber = values[i];
                    break;
                }
            }

            if (tripsNumber == 0)
                return false;

            for (i = 0; i < 6; i++)
            {
                if (values[i] == values[i + 1] && values[i] != tripsNumber && values[i] != 0)
                {
                    pairNumber = values[i];
                    break;
                }
            }

            if (tripsNumber != 0 && pairNumber != 0)
            {
                result[0] = FULLHOUSE;
                result[1] = tripsNumber;
                result[2] = pairNumber;
                return true;
            }

            return false;
        }


        // Neloset
        private bool quads(int[] values, int[] result)
        {
            int quadsNumber = 0, i = 0;
            for (i = 0; i < 4; i++)
            {
                if (values[i] == values[i + 3] && values[i] != 0)
                {
                    quadsNumber = values[i];
                    break;
                }
            }

            if (quadsNumber != 0)
            {
                result[0] = QUADS;
                result[1] = quadsNumber;
                for (i = 0; i < 7; i++)
                {
                    if (values[i] != quadsNumber)
                    {
                        result[2] = values[i];
                        break;
                    }
                }
                return true;
            }

            return false;
        }


        // Värisuora.
        private bool straighFlush(int[] numbers, int[] result)
        {
            int straightNumber = 0, suit = 99, i = 0, counter = 0;
            int[] suits = { 0, 0, 0, 0 };
            int[] valuesUnique = { 0, 0, 0, 0, 0, 0, 0, 0 };

            // Lasketaan maiden lukumaarat
            for (i = 0; i < 7; i++)
            {
                if (numbers[i] != 99)
                    suits[numbers[i] / 13]++;
            }

            // Valitaan maa, jota on vähintään viisi kappaletta
            for (i = 0; i < 4; i++)
            {
                if (suits[i] >= 5)
                {
                    suit = i;
                    break;
                }
            }

            // Lopetetaan heti, jos ei viittä samaa maata.
            if (suit == 99)
                return false;

            // Laitetaan samaa maata olevat kortit taulukkoon
            for (i = 0; i < 7; i++)
            {
                if (numbers[i] != 99 && numbers[i] / 13 == suit)
                {
                    valuesUnique[counter] = numbers[i];
                    counter++;
                }
            }

            // Lisätään 1, jos arvoista löytyy ässä.
            if ((valuesUnique[0] % 13) + 2 == 14)
                valuesUnique[counter] = valuesUnique[0] - 13; //vai counter + 1 !!

            // Löytyy suora, jos viiden alkion etäisyys toisistaan on tasan neljä.
            for (i = 0; i < 4; i++)
            {
                if (valuesUnique[i] - valuesUnique[i + 4] == 4 && valuesUnique[i + 4] != 0)
                {
                    straightNumber = valuesUnique[i];
                    break;
                }
            }

            if (straightNumber != 0)
            {
                result[0] = STRAIGHTFLUSH;
                result[1] = straightNumber % 13 + 2;
                return true;
            }

            return false;
        }


        // n = pöytäkorttien lukumäärä
        public int handValue(int[] Hand, int[] Board, int n, int[] result)
        {
            int i;
            int[] values = { 0, 0, 0, 0, 0, 0, 0 };
            int[] numbers = { 99, 99, 99, 99, 99, 99, 99 };

            for (i = 0; i < 5; i++)
                result[i] = 0;

            values[0] = Hand[0] % 13 + 2;
            values[1] = Hand[1] % 13 + 2;
            numbers[0] = Hand[0];
            numbers[1] = Hand[1];

            for (i = 0; i < n; i++)
            {
                values[i + 2] = Board[i] % 13 + 2;
                numbers[i + 2] = Board[i];
            }

            insertSort(7, values);
            insertSort(7, numbers);

            if (straighFlush(numbers, result))
                i = 0;
            else if (quads(values, result))
                i = 0;
            else if (fullHouse(values, result))
                i = 0;
            else if (flush(numbers, result))
                i = 0;
            else if (straight(values, result))
                i = 0;
            else if (trips(values, result))
                i = 0;
            else if (twoPairs(values, result))
                i = 0;
            else if (onePair(values, result))
                i = 0;
            else
                nothing(values, result);

            return result[0] * 759375 + result[1] * 50625 + result[2] * 3375 + result[3] * 225 + result[4] * 15 + result[5];
        }
    }
}
