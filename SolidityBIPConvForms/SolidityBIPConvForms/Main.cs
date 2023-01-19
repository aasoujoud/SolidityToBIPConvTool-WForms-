using SolidityBIPConvForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SolidityBIPConv
{
    public class Main
    {
        public List<string> ContractNameList = new List<string>();
        public BIPtemplate bIPFinishedTemplate = new BIPtemplate();
        public TextBox OutPutBox;

        public void MainMethod(String FileString, TextBox ReceivedOutputBox)
        {
            OutPutBox = ReceivedOutputBox;
            string Text = FileString;
            //OutputBox.Text = Text;
            // ReadAllText automatically closes file after reading it
            Text = ClearComments(Text); // clear comments
            BeginBIP();
            SeparateContracts(Text);
            EndBIP();
        }


        public string ClearComments(string text)
        {
            string text1;
            string firstTag = "/*";
            string secondTag = "*/";
            string Tag1 = "//";
            string Tag2 = Environment.NewLine;
            Regex regex = new Regex(string.Format("{0}(.*?){1}", Regex.Escape(firstTag), Regex.Escape(secondTag)), RegexOptions.Singleline); // removes comments accross multiple lines
            Regex regex1 = new Regex(string.Format("{0}(.*?){1}", Regex.Escape(Tag1), Regex.Escape(Tag2)), RegexOptions.RightToLeft); // removes comments accross one line 
            text1 = regex1.Replace(text, Environment.NewLine);
            return regex.Replace(text1, Environment.NewLine);
        }

        public void BeginBIP()
        {
            OutPutBox.AppendText("model Globalmodel" + Environment.NewLine + // WRITE TO FILE ////////////////////
                                   "port type ePort" + Environment.NewLine +
                                   "connector type SINGLE(syncPort p)" + Environment.NewLine +
                                               " define p" + Environment.NewLine +
                               "end" + Environment.NewLine);

        }
        public void SeparateContracts(string text)
        {
            string ContractName = "";
            text = text.Substring(text.IndexOf("contract") + "contract".Length); // Trims everything before word "contract" found.
            String[] Contracts = Regex.Split(text, "contract"); // Splits contracts by contract name

            foreach (string contract in Contracts)
            {
                foreach (char s in contract)
                {
                    if (s == '{')
                    {
                        break;
                    }
                    ContractName += s;

                }
                ContractName = ContractName.Replace(" ", ""); // removing space between
                ContractNameList.Add(ContractName);
                SeparateFunction(contract, ContractName);
                ContractName = ""; // resets contract name
            }

        }

        public void SeparateFunction(string text, string ContractName)
        {
            List<SimpliedFunctions> function = new List<SimpliedFunctions>(); // list of functions
            String[] separateFunctions = text.Replace("function", "#function").Replace("fallback", "#fallback").Replace("receive", "#receive").Split('#'); // Each string in the array of strings contains a whole function
            List<string> list = new List<string>(separateFunctions); // converting Array of strings to list 
            List<String> States;
            string initial_state = "initial to invoke";


            list.RemoveAt(0); // removing contract name and variables



            foreach (string s in list)
            {
                SimpliedFunctions func = new SimpliedFunctions(); // intializing a new class object 
                func.SetAllfunction(s); // adding separated functions to each class object
                function.Add(func); // Add functions to a list of functions 
            }

            bIPFinishedTemplate.SetContractName(ContractName + "1");

            for (int i = 0; i < function.Count; i++)
            {
                function[i].SetCurrentTemplate(bIPFinishedTemplate);  // Using the same template for all classes, this template will be sent over to all functions.
                function[i].SeparatefunctionHeader();
                function[i].InterpertHeader();
                function[i].SeparateBody();
                function[i].InterpertBody();
                bIPFinishedTemplate.AddLineToTransition();
                bIPFinishedTemplate.AddLineToPorts();
                bIPFinishedTemplate.AddLineToConnector();
                bIPFinishedTemplate.TransitionGenerator();
            }
            OutPutBox.AppendText("atomic type " + ContractName + Environment.NewLine); // Write To File Function ///////////////////////////////////////////////
                                                                                       // define ports

            foreach (string s in bIPFinishedTemplate.PortsList)
            {
                OutPutBox.AppendText(s + Environment.NewLine);

            }

            // define states
            States = bIPFinishedTemplate.States.Distinct().ToList<string>();
            string JoinedStates = string.Join(",", States);
            JoinedStates = " place " + JoinedStates;
            OutPutBox.AppendText(JoinedStates + Environment.NewLine); // Write To File Function ///////////////////////////////////////////////
            OutPutBox.AppendText(initial_state + Environment.NewLine); // Write To File Function ///////////////////////////////////////////////
                                                                       // set initial states


            // transition BIP

            foreach (string s in bIPFinishedTemplate.TransitionList)
            {
                OutPutBox.AppendText(s + Environment.NewLine);  // Write To File Function ///////////////////////////////////////////////

            }


            OutPutBox.AppendText("end" + Environment.NewLine);  // Write To File Function ///////////////////////////////////////////////
                                                                //connectors
            bIPFinishedTemplate.ResetLists();
        }

        public void EndBIP()
        {
            OutPutBox.AppendText("compound type globModel" + Environment.NewLine); // Write To File Function ///////////////////////////////////////////////
            foreach (string s in ContractNameList)
            {
                OutPutBox.AppendText("component " + s + " " + s + "1" + Environment.NewLine); // Write To File Function ///////////////////////////////////////////////
            }



            foreach (string s in bIPFinishedTemplate.ConnectorList) // Write To File Function ///////////////////////////////////////////////
            {
                OutPutBox.AppendText(s + Environment.NewLine); // Write To File Function ///////////////////////////////////////////////
            }


            //endBIP 
            OutPutBox.AppendText("end " + Environment.NewLine +    // Write To File Function ///////////////////////////////////////////////
               "component globModel Root " + Environment.NewLine +
               "end " + Environment.NewLine);
        }
    }
}
