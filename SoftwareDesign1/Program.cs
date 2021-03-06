﻿using System;

using System.Collections.Generic;

using System.Linq;

using System.Text;

using System.Threading.Tasks;

using System.IO;

using System.Text.RegularExpressions;



namespace SoftwareDevelopment1

{
    class Tree
    {
        public ID value;
        public Tree left;
        public Tree right;
        public void Insert(ID value)
        {
            if (this.value == null)
                this.value = value;
            else
            {
                if (this.value.Hashcode.CompareTo(value.Hashcode) == 1)
                {
                    if (left == null)
                    {
                        this.left = new Tree();
                    }
                    left.Insert(value);
                }
                else if (this.value.Hashcode.CompareTo(value.Hashcode) == -1)
                {
                    if (right == null)
                        this.right = new Tree();
                    right.Insert(value);
                }
            }
        }
    }
    abstract class ID

    {

        public string type;
        public string name;
        public int hashcode;

        public string Type

        {

            get { return type; }

            set { type = value; }

        }
        public string Name

        {

            get { return name; }

            set { name = value; }

        }
        public int Hashcode

        {

            get { return hashcode; }

            set { hashcode = value; }

        }
        public ID(string text, string n)

        {

            if (text == "int")

                Type = "int_type";

            if (text == "float")

                Type = "float_type";

            if (text == "bool")

                Type = "bool_type";

            if (text == "char")

                Type = "char_type";

            if (text == "string")

                Type = "string_type";

            if (text == "class")

                Type = "class_type";
            Name = n;
            Hashcode = n.GetHashCode();
        }

        public override string ToString()

        {

            return this.Type;

        }

    }

    class CLASSES : ID

    {


        public string use;



        public string Use

        {

            get { return use; }

            set { use = value; }

        }



        public CLASSES(string name, string type) : base(type, name)

        {

            Name = name;

            Hashcode = name.GetHashCode();

            Use = "CLASSES";

        }

        public override string ToString()

        {

            return this.Name + " " + this.Hashcode + " " + this.Use + " " + base.ToString() + " " ;

        }

    }

    class CONSTS : ID

    {


        public string use;


        public object val;


        public string Use

        {

            get { return use; }

            set { use = value; }

        }



        public object Val

        {

            get { return val; }

            set { val = value; }

        }

        public CONSTS(string name, string val, string type) : base(type, name)

        {

            Name = name;

            Hashcode = name.GetHashCode();

            Use = "CONSTS";

            Val = val;

        }

        public override string ToString()

        {

            return this.Name + " " + this.Val + " " + this.Hashcode + " " + this.Use + " " + base.ToString() ;

        }

    }

    class VARS : ID

    {

        public string use;



        public string Use

        {

            get { return use; }

            set { use = value; }

        }



        public VARS(string name, string type) : base(type, name)

        {

            Name = name;

            Hashcode = name.GetHashCode();

            Use = "VARS";

        }

        public override string ToString()

        {

            return this.Name + " " + this.Hashcode + " " + this.Use +  " " + base.ToString();

        }

    }

    class METHODS : ID

    {

        public string use;

        public Dictionary<string,string> list;



        public string Use

        {

            get { return use; }

            set { use = value; }

        }


        public Dictionary<string, string> List

        {

            get { return list; }

            set { list = value; }

        }

        public METHODS(string name, string type, Dictionary<string, string> l) : base(type, name)

        {

            Name = name;

            Hashcode = name.GetHashCode();

            Use = "METHODS";

            List = l;

        }

        public override string ToString()

        {

            return this.Name + " " + this.Hashcode + " " + this.Use + " " + this.ListString() + " " + base.ToString();

        }

        public string ListString()

        {

            string line = "";

            foreach (KeyValuePair<string,string> x in this.List)

            {

                line = line + x.Key + " "+x.Value+ " | ";

            }

            return line;

        }

    }

    class Program

    {
        static void ForVar(Tree t,string txt)
        {
            Regex reg1 = new Regex(@"(int|float|bool|char|string)");

            Match m = reg1.Match(txt);

            txt = reg1.Replace(txt, " ");

            reg1 = new Regex(@"\;");

            txt = reg1.Replace(txt.Trim(' '), " ");

            ID v = new VARS(txt.Trim(' '), m.Value);
            t.Insert(v);
        }
        static void ForClass(Tree t,string txt)
        {
            txt = txt.Replace("class", " ");

            txt = txt.Replace(";", " ");

            ID cl = new CLASSES(txt.Trim(' '), "class");

            t.Insert(cl);
        }
        static void ForMeth(Tree t,string txt)
        {
            Regex reg1 = new Regex(@"(\((((ref|out)[\s]){0,1}(int|float|bool|char|string){1}([\s]){1}([\w]+[\d]*)\,*[\s]*)+\))|(\(\))");

            Match m = reg1.Match(txt);

            txt = reg1.Replace(txt, " ");

            reg1 = new Regex(@"(int|float|bool|char|string)");

            Match m1 = reg1.Match(txt);

            txt = reg1.Replace(txt.Trim(' '), " ");

            reg1 = new Regex(@"\;");

            txt = reg1.Replace(txt.Trim(' '), " ");

            reg1 = new Regex(@",");

            string[] par = reg1.Split(m.Value.Trim(' '));

            reg1 = new Regex(@"[\s]{1}");

            for (int j = 0; j < par.Length; j++)

            {

                par[j] = par[j].Replace(")", " ");

                par[j] = par[j].Replace("(", " ");

                par[j] = par[j].Trim(' ');

                par[j] = reg1.Replace(par[j], "_");

            }

            Dictionary<string, string> par2=new Dictionary<string, string>();
            for(int k = 0; k < par.Length; k++)
            {
                if (par[k].Contains("ref")==true)
                {
                    par2.Add(par[k], "param_ref");
                }
                else if (par[k].Contains("out") == true)
                {
                    par2.Add(par[k], "param_out");
                }
                else if (par[k] == "")
                {
                    par2.Add(par[k], "");
                }
                else
                {
                    par2.Add(par[k], "param_val");
                }
            }

            reg1 = new Regex(@"(\((((ref|out)[\s]){0,1}(int|float|bool|char|string){1}([\s]){1}([\w]+[\d]*)\,*[\s]*)+\))|(\(\))");

            txt = reg1.Replace(txt, " ");

            ID meth = new METHODS(txt.Trim(' '), m1.Value, par2);

            t.Insert(meth);
        }
        static void ForConst(Tree t, string txt)
        {
            txt = txt.Trim(' ');

            Regex reg = new Regex(@"(int|float|bool|char|string)");

            Match type = reg.Match(txt);

            txt = reg.Replace(txt, " ");

            reg = new Regex(@"const");

            txt = reg.Replace(txt, " ");

            txt = txt.Replace("=", " ");

            txt = txt.Replace(";", " ");

            reg = new Regex(@"[\w]+[\d]*");

            Match name = reg.Match(txt);

            txt = txt.Replace(name.Value, " ");

            txt = txt.Trim(' ');

            reg = new Regex(@"([\d]+)|([\w]+[\d]*)");

            Match val = reg.Match(txt);

            ID c = new CONSTS(name.Value, val.Value, type.Value);

            t.Insert(c);
        }

        static void Main(string[] args)

        {
            Regex formeth = new Regex(@"[\s|\w]*((\([\s|\w|\,]+\))|(\(\)))\;");

            Regex forclass = new Regex(@"class[\s]{1}[\w]+[\d]*\;");

            Regex forvar = new Regex(@"(int|float|bool|char|string)[\s]{1}[\w]+[\d]*\;");

            Regex forconst = new Regex(@"const[\s](int|float|bool|char|string)[\s][\w]+[\d]*[\s]*\=[\s]*\-*\'*(([\d]+(\.[\d]+)*)|([\w]+[\d]*))\'*\;");

            FileStream f = new FileStream("input.txt", FileMode.OpenOrCreate);

            StreamReader r = new StreamReader(f);

            int count = 0;

            while (r.ReadLine() != null)

            {

                count++;

            }

            r.Close();

            f.Close();

            FileStream f1 = new FileStream("input.txt", FileMode.OpenOrCreate);

            StreamReader r1 = new StreamReader(f1);

            Tree t = new Tree();
            for (int i = 0; i < count; i++)

            {

                string txt = r1.ReadLine();

                if (forvar.IsMatch(txt) == true)

                {

                    ForVar(t, txt);

                }

                else if (forclass.IsMatch(txt) == true)

                {

                    ForClass(t,txt);

                }

                else if (formeth.IsMatch(txt) == true)

                {

                    ForMeth( t, txt);

                }

                else if (forconst.IsMatch(txt) == true)

                {

                    ForConst(t, txt);

                }

            }

            r1.Close();
            f1.Close();
            Console.ReadKey();
        }

    }

}
