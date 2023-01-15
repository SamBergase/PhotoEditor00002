using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoEditor00001
{
    public enum distanceUnits
    {
        undefDistUnit, points, millimeter, centimeter, decimeter, meter, kilometer, inch, feet, yard
    }
    public enum weightUnits
    {
        undefWeightUnit, gram, kilogram, tonnes, stones, pound
    }
    public enum currencyUnits
    {
        undefCurrencyUnit, gbp, usd, mnt, eur, sek
    }
    public enum nameType
    {
        UndefNameType, Birth, Taken, Married, Alias, Nickname
    }

    class ActorDataClass
    {
        private string userIdentity = "";
        int noOfNames = 0;
        public nameType[] nTyp { get; set; }
        public string[] surn { get; set; }
        public string[] middn { get; set; }
        public string[] famn { get; set; }

        public ActorDataClass (string uid)
        {
            this.userIdentity = uid;
            // string actorDataFilename = System.IO.Directory.GetCurrentDirectory() + uid + ".acf";
            string actorDataFilename = "C:\\Users\\esbberg\\source\\repos\\PhotoEditor00001\\PhotoEditor00001\\ActorData\\ActorData_" + uid + ".acf";
            if (System.IO.File.Exists(actorDataFilename))
            {
                // User data file exists, read the data
                foreach (string line in System.IO.File.ReadLines(actorDataFilename))
                {
                    string adTag = line.Substring(0, 8);
                    string adData = line.Substring(11, line.Length - 11);
                    switch (adTag)
                    {
                        case "UserID  ":
                            {
                                this.userIdentity = adData;
                                    // Return fail.
                            } break;
                        case "UserName":
                            {
                                string tempNameType = "";
                                string tempSurname = "";
                                string tempMidname = "";
                                string tempFamname = "";

                                int dp0 = adData.IndexOf(";");
                                if ((dp0 > 0) && (dp0 < adData.Length - 2))
                                {
                                    tempNameType = adData.Substring(dp0 + 2, adData.Length - dp0 - 2);
                                    adData = adData.Substring(0, dp0);
                                }
                                int dp1 = adData.IndexOf(" ");
                                if ((dp1 > 0) && (dp1 < adData.Length - 2))
                                {
                                    // More than one name, first interpreted as surname
                                    tempSurname = adData.Substring(0, dp1);
                                    adData = adData.Substring(dp1 + 1, adData.Length - dp1 - 1);
                                    int dp2 = adData.IndexOf(" ");
                                    if ((dp2 > 0) && (dp2 < adData.Length - 2))
                                    {
                                        // Have a middlename too
                                        tempMidname = adData.Substring(0, dp2);
                                        adData = adData.Substring(dp2 + 1, adData.Length - dp2 - 1);
                                        tempFamname = adData;
                                    }
                                    else
                                    {
                                        // Have only Familyname
                                        tempFamname = adData;
                                    }
                                }
                                else
                                {
                                    // Merely a name, enterpreted as surname
                                    tempSurname = adData;
                                }

                                if (tempNameType == "Alias")
                                    this.nTyp.SetValue(nameType.Alias, this.noOfNames);
                                else if (tempNameType == "Birth")
                                    this.nTyp.SetValue(nameType.Birth, this.noOfNames);
                                else if (tempNameType == "Married")
                                    this.nTyp.SetValue(nameType.Birth, this.noOfNames);
                                else if (tempNameType == "Nick")
                                    this.nTyp.SetValue(nameType.Nickname, this.noOfNames);
                                else if (tempNameType == "Taken")
                                    this.nTyp.SetValue(nameType.Taken, this.noOfNames);
                                else
                                    this.nTyp.SetValue(nameType.UndefNameType, this.noOfNames);

                                this.surn.SetValue(tempSurname, this.noOfNames);
                                this.middn.SetValue(tempMidname, this.noOfNames);
                                this.famn.SetValue(tempFamname, this.noOfNames);

                                this.noOfNames++;
                            } break;
                        default:
                            { 

                            } break;
                    }
                }
            }
            else
            {
                // New user data file, create standard file.
            }
        }
    }
}
