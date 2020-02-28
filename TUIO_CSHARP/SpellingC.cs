using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUIO;
using Tuio_Demo;

namespace TUIO_DEMO
{
    public class SpellingC
    {
        public List<string> Words = new List<string>();
        public List<string> LevelWords = new List<string>();
        public List<string> CorrectWords = new List<string>();
        public Dictionary<int, char> Letters = new Dictionary<int, char>(); 
        public User currentUser = new User();
        public Dictionary<int, char> levelLetters = new Dictionary<int, char>();
        public List<User> users = new List<User>();
        public SpellingC()
            {
            Load_Letters();
            Load_Words();
            CreateUser("Adham", 30);
            CreateUser("Mazen", 31);
            GetUser(30);
            }
        void CreateUser(string name, int id)
        {
            User ur = new User();
            ur.Name = name;
            ur.ID = id;
            users.Add(ur);
        }
        public void GetUser(int id)
        {
            for (int i = 0; i < users.Count; i++)
            {
                if (users[i].ID == id)
                {
                    currentUser = users[i];
                    break;
                }
            }
        }

       List<Symbolcs> SortList(List<Symbolcs> symbols)
        {
            List<Symbolcs> symb = new List<Symbolcs>();
            int current = -1;
            for (int i = 0; i < symbols.Count; i++)
            {
                int min = 99999;
         
                int pos =0 ;
                for (int k = 0; k < symbols.Count; k++)
                {
                    if(symbols[k].Position.X < min && symbols[k].Position.X > current)
                    {
                        min = symbols[k].Position.X;
                        pos = k;
                    }
                }
                current = min;
               
                symb.Add(symbols[pos]);
            }
            return symb;
        }
        string MinMaxDist(List<Symbolcs> symbols)
        {
            List<Symbolcs> Symbols = SortList(symbols);
            string Word = "";
            if (Symbols.Count > 0)
            {
                if (TranslateID(Symbols[0].ID) != ' ')
                {
                    Word += TranslateID(Symbols[0].ID);
                }
                int min = 999999;
                int max = 0;
                int first = Symbols[0].Position.Y;
              
                for (int i = 1; i < Symbols.Count; i++)
                {

                    if (Symbols[i].Position.Y <= first + 3)
                    {
                        if (Symbols[i].Position.X > Symbols[i - 1].Position.X + 25)
                        {
                            if (TranslateID(Symbols[i].ID) != ' ')
                            {
                                Word += TranslateID(Symbols[i].ID);
                            }
                        }
                    }
                    else
                    if ( Symbols[i].Position.Y >= first - 3)
                    {
                        if (Symbols[i].Position.X > Symbols[i - 1].Position.X + 25)
                        {
                            if (TranslateID(Symbols[i].ID) != ' ')
                            {
                                Word += TranslateID(Symbols[i].ID);
                            }
                        }
                    }

                }
            }
            return Word;
        }
        

     public   void CollectWord(List<Symbolcs> cursorList)
        {


            string word = "";
         word =   MinMaxDist(cursorList);
            checkWord(word);
        }  
        char TranslateID(int id)
        {
            if(Letters.ContainsKey(id))
            {
                return Letters[id];
            }
         
            return ' ';
        }
        void Load_Words()
        {
            Populate(ref Words, "file.txt");
            Words.Add("gh");
        }
        void Load_Letters(List<char> letters)
        {
            for (int i = 0; i < letters.Count; i++)
            {
                levelLetters.Add(i, letters[i]);
           
            }
        }
        public void Populate(ref List<string> str, string path)
        {

            var fileStream = new FileStream(path, FileMode.Open);
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
            {
                string text = null;
                while (streamReader.Peek() != -1)
                {
                    text = streamReader.ReadLine();
                    string[] words = text.Split(new Char[] { '\\', ',', '/', '.', ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (string n in words)
                    {
                        if (n.Length > 1)
                            str.Add(n);

                    }

                }
            }
            }
      public  string  Get_User(int id)
        {
            for(int i = 0; i < users.Count; i++)
            {
                if(users[i].ID == id)
                {
                    return users[i].Name;
                }
            }
            return " ";
        }
        public string GetLetter(int id)
        {
            if(Letters.ContainsKey(id))
            {
                return Letters[id].ToString();
            }
            else
            {
                return " ";
            }
        }
        void Load_Letters()
        {
            char l = 'a';
            for(int i =0; i < 26; i++)
            {
                Letters.Add(i,l);
                l++;

            }
        }
        void checkWord(string Word)
        {
            if (currentUser != null)
            {
                if (Words.Contains(Word) && !currentUser.LevelWords.Contains(Word))
                {
                    currentUser.LevelWords.Add(Word);
                    currentUser.levelPoints += 10;
                }
            }
        }


    }
}
