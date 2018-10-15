using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrandCircusWeek1Capstone
{
    using System.Text.RegularExpressions;//I'll be using RegEx for some string qualification

    class Program
    {
        static void Main(string[] args)
        {
            /* Pig Latin
             * Translate from English to Pig Latin
             * Prompt user for a string
             * Translate to Pig Latin and display on console
             * Ask to continue
             */

            Console.WriteLine(PigLatin("Hello!") + "Welcome to the Pig Latin generator!\n" +
                "We take whatever words or phrases you give and translate it \n" +
                "into fun and easy-to-read Pig Latin!\n");


            bool goAgain = true;
            //the bool above and loop below allow the user to continue indefinitely
            do
            {
                Console.Write("Enter the word or phrase you want translated: ");
                string phrase = Console.ReadLine();
                if (Regex.IsMatch(phrase, @"\S"))//only proceed if there is an entry
                {
                    Console.WriteLine(PigLatin(phrase));
                }
                else Console.WriteLine("Please make sure to actually enter a word or phrase before hitting \"Enter\"");

                goAgain = KeepGoing(goAgain);//returns true/false depending on input; see method below 
                
            } while (goAgain);
        }

        static string PigLatin(string phrase)
        {
            string vowels = "aeiou"; //Pig Latin splits on the first vowel
            string pigLatinWord = "";

            string[] splitWords = phrase.Split(' '); //first split the string into separate words

            foreach (var word in splitWords) //applying Pig Latin to the string word by word
            {
                string lowerWord = word.ToLower();
                bool foundVowel = false; //to confirm vowel is found and stop loop once that occurs
                

                if (Regex.IsMatch(lowerWord, @"-+")) //if there's one - in the word, split by that 
                {
                    pigLatinWord += SplitBy(lowerWord, '-');
                    foundVowel = true;
                }

                else if (Regex.IsMatch(lowerWord, @"/+")) //if there's one / in the word, split by that 
                {
                    pigLatinWord += SplitBy(lowerWord, '/');
                    foundVowel = true;
                }

                else if (Regex.IsMatch(lowerWord, @"\d+") || (Regex.IsMatch(lowerWord, @"\w[^\w\s]\w") && !Regex.IsMatch(lowerWord, @"[']")))                
                    //if the word has digits or non-word symbols inside it (but not a '), leave it alone
                {
                    pigLatinWord += lowerWord + " ";
                    foundVowel = true;
                }
                
                else
                { 
                    do
                    {//loops until a vowel is found
                        foreach (var v in vowels)
                        {//if begins with vowel, need to add "way" instead of anything else
                            if ((lowerWord.First() == v) && !foundVowel)
                                
                            {
                                pigLatinWord += (lowerWord + "way");
                                pigLatinWord = CheckPunctuation(pigLatinWord);
                                foundVowel = true;
                                break;
                            }

                        }

                        if (!foundVowel)
                        {//only proceeds translating if the first letter isn't a vowel 
                         //and stops once the first vowel is found
                            foreach (char letter in lowerWord)
                            {//separates on first vowel found in string

                                if (vowels.Contains(letter))
                                {
                                    int vowelIndex = lowerWord.IndexOf(letter);
                                    string latinWord = letter + lowerWord.Substring(vowelIndex + 1) + lowerWord.Substring(0, vowelIndex) + "ay";
                                    //flip word around at first vowel and add "-ay" to last syllable
                                    pigLatinWord += latinWord; //append Pig Latinized word to whole sentence
                                    pigLatinWord = CheckPunctuation(pigLatinWord);
                                    foundVowel = true; //stop this loop once the program finds a vowel
                                    break;
                                }
                            }
                            if (!foundVowel)
                            {
                                pigLatinWord += lowerWord + " ";
                                foundVowel = true;
                                //if no vowel in word, leave as is and move on to next word
                            }
                        }                        
                    } while (!foundVowel);
                }
            }
            
            pigLatinWord = pigLatinWord.First().ToString().ToUpper() + pigLatinWord.Substring(1); //capitalizes first letter
            
            return pigLatinWord;
        }

        static bool KeepGoing(bool continuer)
        {//method to check if user wants to continue
            bool correctInput = true; //makes sure user puts in a variation of "yes" or "no"
            do
            {
                Console.Write("\nWould you like to translate again? (y/n) ");
                string confirm = Console.ReadLine().ToLower();
                if (confirm == "n" || confirm == "no" || confirm == "onay")//added possible Pig Latin versions of "yes" and "no"
                {
                    Console.WriteLine(PigLatin("Thanks for playing!"));
                    continuer = false;
                    correctInput = true;
                    Console.ReadKey();
                }
                else if (confirm == "y" || confirm == "yes" || confirm == "esyay")
                {
                    Console.WriteLine(PigLatin("Okay!"));
                    continuer = true;
                    correctInput = true;
                }
                else
                {
                    Console.WriteLine(PigLatin("Sorry,") +  " I didn't understand.");
                    correctInput = false;
                }                
            } while (!correctInput);
            return continuer;            
        }

        static string CheckPunctuation(string messyString)
        {//looks for punctuation in a string, and sticks it on the end of the word
            string correctString = "";

            foreach (string messyWord in messyString.Split(' '))
            {
                string correctWord = messyWord; //if there's no punctuation in the word leave as-is
                foreach (char messyLetter in messyWord)
                {
                    if (".,!?:;".Contains(messyLetter))//common punctuation marks that go on the end of a word
                    
                    {//searches for punctuation in string above, sticks it on the end of the word
                        var punctIndex = messyWord.IndexOf(messyLetter);
                        correctWord = messyWord.Substring(0, punctIndex) + messyWord.Substring(punctIndex + 1) + messyLetter;                        
                    }   
                }
                correctString += correctWord + " ";
            }
            return correctString;
        }

        static string SplitBy(string splitString,char splitChar)
        {//splits by specified character, then removes extra whitespace, then recombines with char
            string[] hyphenSplit = splitString.Split(splitChar);
            string splitWord = Regex.Replace(PigLatin(hyphenSplit[0]), @"\s", "");
            return (splitWord + splitChar + PigLatin(hyphenSplit[1])).ToLower();
        }
    }
}
