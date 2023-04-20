using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Principal;
using System.Globalization;

namespace PhotoEditor00002
{
    #region enum-definitions
    public enum DistUnits
    {
        UndefDistUnit, Points, Millimeter, Centimeter, Decimeter, Meter, Inch, Feet, Yard
    }
    public enum WeightUnits
    {
        UndefWeightUnit, Gram, KiloGram, Tonnes, Stones, Pound
    }
    public enum GenderType
    {
        UndefGender, Male, Female
    }
    public enum GenderAppearanceType
    {
        UndefGenderAppearance, Barbie, Curtains, Horseshoe, Puffy, Tulip, I, II, III, IV, V
    }
    public enum GenderBhaviourType
    {
        UndeBehaviourType, Sloppy, Stretchy, Tight, Stiffer, Grower
    }
    public enum BreastSizeType
    {
        UndefBreastSize, AA, A, B, C, D, E, F, Flat, Medium, Bulgy, Oversize
    }
    public enum BreastType
    {
        UndefBreastType, NaturalBreasts, SiliconeBreasts
    }
    public enum EtnicityType
    {
        UndefEtnicity, Negro, Asian, Indian, Cocation, Mulatto, Mexican, Innuit
    }
    public enum HairTextureType
    {
        UndefHairTexture, Straight, Wavy, Curly, Coily
    }
    public enum HairLengthType
    {
        UndefHairLength, Short, Neck, Shoulder, MidBack, Waist, Ass, Long
    }
    public enum BodyType
    {
        UndefBodyType, Athhletic, Slim, Spiny, Plump, Fat
    }
    public enum MarkingType
    {
        UndefMarking, Scar, Freckles, Birthmark, Tattoo, Piercing
    }
    public enum ClassificationType
    {
        UndefLevel, Unclassified, Limited, Confidential, Secret, QualifSecret
    }
    public enum NameType
    {
        UndefNameType, Birth, Taken, Married, Alias, Nickname
    }
    public enum GeogrDir
    {
        UndefGeoDir, North, East, South, West
    }
    #endregion
    #region struct-definitions
    public struct defaultStruct
    {
        public string tag;
        public string description;
        public int level;
        public double value; // Value to Dollar
    }
    public struct myDateTime
    {
        public int year;
        public int month;
        public int day;
    }
    public struct userNames
    {
        public NameType nameType;
        public string Surname;
        public string Midname;
        public string Famname;
    }
    public struct userContacts
    {
        public defaultStruct type;
        public string contactPath;
    }
    public struct userBirthData
    {
        public string brthStreet;
        public int brthStreetNumber;
        public string brthStreetNumberAddon;
        public string brthCityname;
        public string brthAreaname;
        public string brthZipcode;
        public string brthCountryname;
        public myDateTime brthDate;
        public string brthSecurityCode;
        public GenderType brthGender;
        public float latVal;
        public GeogrDir latDir;
        public float lonVal;
        public GeogrDir lonDir;
    }
    public struct userComplexion
    {
        public EtnicityType etnicType;
        public int redChannelValue;
        public int greenChannelValue;
        public int blueChannelValue;
        public myDateTime validDate;
    }
    public struct userGenderData
    {
        public GenderType type;
        public float length;
        public float circumference;
        public DistUnits usedUnits;
        public GenderAppearanceType appearance;
        public GenderBhaviourType behaviour;
        public string presentation;
        public myDateTime validDate;
    }
    public struct userLengthData
    {
        //public float value;
        public float value;
        public DistUnits unit;
        public myDateTime validDate;
    }
    public struct userWeightData
    {
        public float value;
        public WeightUnits unit;
        public myDateTime validDate;
    }
    public struct userChestData
    {
        public BreastType type;
        public float circumference;
        public DistUnits units;
        public BreastSizeType sizeType;
        public myDateTime validDate;
    }
    public struct userFaceData
    {
        public float eyeWidth;
        public float cheekboneWidth;
        public float chinWidth;
        public float mouthWidth;
        public float height;
        public DistUnits units;
        public myDateTime validDate;
    }
    public struct userResidenceData
    {
        public string streetname;
        public int number;
        public string additive;
        public string city;
        public string area;
        public int zipcode;
        public string country;
        public myDateTime boughtDate;
        public myDateTime salesDate;
        public float boughtValue;
        public float salesValue;
        public defaultStruct usedCurrency;
    }
    public struct userHairColorData
    {
        public defaultStruct hairColor;
        public HairTextureType textureTag;
        public HairLengthType lengthTag;
        public myDateTime validDate;
    }
    public struct userEyeData
    {
        public string colorTag;
        public string formTag;
        public myDateTime validDate;
    }
    public struct userMarkingData
    {
        public MarkingType markTag;
        public string placement;
        public string motif;
        public myDateTime validDate;
    }
    public struct userOccupationData
    {
        public string title;
        public string company;
        public string streetname;
        public int number;
        public string statename;
        public string areaname;
        public int zipcode;
        public string country;
        public myDateTime started;
        public myDateTime ended;
    }
    public struct userAttendedEventData
    {
        public string eventID;
        public defaultStruct eventType;
        public string eventCategory;
        public defaultStruct role; // Hantera denna
        public myDateTime started;
        public myDateTime ended;
    }
    public struct userRelatedImages
    {
        public string imagePathName;
        public defaultStruct imageContext;
        public ClassificationType classificationLevel;
    }
    #endregion
    class ActorClass
    {
        #region variables
        private string userId;

        public const int maxNoOfEventCategories = 250;
        public int noOfEventCategories = 0;
        public defaultStruct[] eventCategories = new defaultStruct[maxNoOfEventCategories];

        public const int maxNoOfContentCategories = 250;
        public int noOfContentCategories = 0;
        public defaultStruct[] contentCategories = new defaultStruct[maxNoOfContentCategories];

        public const int maxNoOfContextCategories = 250;
        public int noOfContextCategories = 0;
        public defaultStruct[] contextCategories = new defaultStruct[maxNoOfContextCategories];

        public const int maxNoOfRelationCategories = 250;
        public int noOfRelationCategories = 0;
        public defaultStruct[] relationCategories = new defaultStruct[maxNoOfRelationCategories];

        public const int maxNoOfCurrencyCategories = 250;
        public int noOfCurrencyCategories = 0;
        public defaultStruct[] currencyCategory = new defaultStruct[maxNoOfCurrencyCategories];

        public const int maxNoOfContactCategories = 125;
        public int noOfContactCategories = 0;
        public defaultStruct[] contactCategory = new defaultStruct[maxNoOfContactCategories];

        public const int maxNoOfHairColorCategories = 64;
        public int noOfHairColorCategories = 0;
        public defaultStruct[] hairColorCategory = new defaultStruct[maxNoOfHairColorCategories];

        public const int maxNoOfRoleCategories = 128;
        public int noOfRoleCategories = 0;
        public defaultStruct[] roleCategories = new defaultStruct[maxNoOfRoleCategories];

        public const int maxNoOfNames = 8;
        public int noOfNames = 0;
        public userNames[] anvNamn = new userNames[maxNoOfNames];

        public const int maxNoOfContacts = 32;
        public int noOfContacts = 0;
        public userContacts[] anvKontakter = new userContacts[maxNoOfContacts];

        public userBirthData anvFodlseData;

        public const int maxNoOfComplexions = 8;
        public int noOfComplexions = 0;
        public userComplexion[] anvHudfarg = new userComplexion[maxNoOfComplexions];

        public defaultStruct anvAktuellRelation;

        public const int maxNoOfGenderInfo = 32;
        public int noOfGenderInfo = 0;
        public userGenderData[] anvKonsInfo = new userGenderData[maxNoOfGenderInfo];

        public const int maxNoOfLengthData = 64;
        public int noOfLengthData = 0;
        public userLengthData[] anvLangdData = new userLengthData[maxNoOfLengthData];

        public const int maxNoOfWeightData = 128;
        public int noOfWeightData = 0;
        public userWeightData[] anvViktData = new userWeightData[maxNoOfWeightData];

        public const int maxNoOfChestData = 32;
        public int noOfChestData = 0;
        public userChestData[] anvBrostData = new userChestData[maxNoOfChestData];

        public const int maxNoOfFaceData = 8;
        public int noOfFaceData = 0;
        public userFaceData[] anvAnsiktsData = new userFaceData[maxNoOfFaceData];

        public const int maxNoOfResidences = 32;
        public int noOfResidences = 0;
        public userResidenceData[] anvBostader = new userResidenceData[maxNoOfResidences];

        public const int maxNoOfHairData = 64;
        public int noOfHairData = 0;
        public userHairColorData[] anvHaar = new userHairColorData[maxNoOfHairData];

        public const int maxNoOfEyeColorData = 8;
        public int noOfEyeColorData = 0;
        public userEyeData[] anvEyes = new userEyeData[maxNoOfEyeColorData];

        public const int maxNoOfMarkingData = 16;
        public int noOfMarkingData = 0;
        public userMarkingData[] anvMarkningar = new userMarkingData[maxNoOfMarkingData];

        public const int maxNoOfOccupationData = 128;
        public int noOfOccupationData = 0;
        public userOccupationData[] anvAnstallningar = new userOccupationData[maxNoOfMarkingData];

        public const int maxNoOfAttendedEventData = 256;
        public int noOfAttendedEventData = 0;
        public userAttendedEventData[] anvTillstallningar = new userAttendedEventData[maxNoOfAttendedEventData];

        public const int maxNoOfRelatedImagesData = 1024;
        public int noOfRelatedImagesData = 0;
        public userRelatedImages[] anvRelBilder = new userRelatedImages[maxNoOfRelatedImagesData];

        public const int maxNoOfNationalityData = 10;
        public int noOfNationalityData = 0;
        public string[] nationality = new string[maxNoOfNationalityData];

        string scu = WindowsIdentity.GetCurrent().Name;
        string sProgPath = "source\\repos\\PhotoEditor00002\\PhotoEditor00002";

        public int noOfDataSet = 0;
        #endregion
        public bool addEventCategory(string tag, string description, string level)
        {
            bool retVal = false;
            if (noOfEventCategories < maxNoOfEventCategories)
            {
                eventCategories[noOfEventCategories].tag = tag;
                eventCategories[noOfEventCategories].description = description;
                if (level == "Open")
                    eventCategories[noOfEventCategories].level = 1;
                else if (level == "Limited")
                    eventCategories[noOfEventCategories].level = 2;
                else if (level == "Medium")
                    eventCategories[noOfEventCategories].level = 3;
                else if (level == "Relative")
                    eventCategories[noOfEventCategories].level = 4;
                else if (level == "Secret")
                    eventCategories[noOfEventCategories].level = 5;
                else if (level == "QualifSecret")
                    eventCategories[noOfEventCategories].level = 6;
                else
                    eventCategories[noOfEventCategories].level = 0;
                noOfEventCategories++;
                retVal = true;
            }
            return retVal;
        }
        public bool addContentCategory(string tag, string description, int level)
        {
            bool retVal = false;
            if (noOfContentCategories < maxNoOfContentCategories)
            {
                contentCategories[noOfContentCategories].tag = tag;
                contentCategories[noOfContentCategories].description = description;
                contentCategories[noOfContentCategories].level = level;
                noOfContentCategories++;
                retVal = true;
            }
            return retVal;
        }
        public bool addContextCategory(string tag, string desc, string level)
        {
            bool retVal = false;
            if (noOfContextCategories < maxNoOfContextCategories)
            {
                contextCategories[noOfContextCategories].tag = tag;
                contextCategories[noOfContextCategories].description = desc;
                //Undefined, Open, Limited, Medium, Relative, Secret, QualifSecret
                if (level == "Open")
                    contextCategories[noOfContextCategories].level = 1;
                else if (level == "Limited")
                    contextCategories[noOfContextCategories].level = 2;
                else if (level == "Medium")
                    contextCategories[noOfContextCategories].level = 3;
                else if (level == "Relative")
                    contextCategories[noOfContextCategories].level = 4;
                else if (level == "Secret")
                    contextCategories[noOfContextCategories].level = 5;
                else if (level == "QualifSecret")
                    contextCategories[noOfContextCategories].level = 6;
                else
                    contextCategories[noOfContextCategories].level = 0;
                noOfContextCategories++;
                retVal = true;
            }
            return retVal;
        }
        public bool addRelationCategory(string tag, string desc, string level)
        {
            bool retVal = false;
            if (noOfRelationCategories < maxNoOfRelationCategories)
            {
                relationCategories[noOfRelationCategories].tag = tag;
                relationCategories[noOfRelationCategories].description = desc;
                if (level == "Open")
                    relationCategories[noOfRelationCategories].level = 1;
                else if (level == "Limited")
                    relationCategories[noOfRelationCategories].level = 2;
                else if (level == "Medium")
                    relationCategories[noOfRelationCategories].level = 3;
                else if (level == "Relative")
                    relationCategories[noOfRelationCategories].level = 4;
                else if (level == "Secret")
                    relationCategories[noOfRelationCategories].level = 5;
                else if (level == "QualifSecret")
                    relationCategories[noOfRelationCategories].level = 6;
                else
                    relationCategories[noOfRelationCategories].level = 0;
                noOfRelationCategories++;
                retVal = true;
            }
            return retVal;
        }
        public bool addCurrencyCategory(string tag, string desc, string level, double value)
        {
            bool retVal = true;
            if (noOfCurrencyCategories < maxNoOfCurrencyCategories)
            {
                currencyCategory[noOfCurrencyCategories].tag = tag;
                currencyCategory[noOfCurrencyCategories].description = desc;
                if (level == "Open")
                    currencyCategory[noOfCurrencyCategories].level = 1;
                else if (level == "Limited")
                    currencyCategory[noOfCurrencyCategories].level = 2;
                else if (level == "Medium")
                    currencyCategory[noOfCurrencyCategories].level = 3;
                else if (level == "Relative")
                    currencyCategory[noOfCurrencyCategories].level = 4;
                else if (level == "Secret")
                    currencyCategory[noOfCurrencyCategories].level = 5;
                else if (level == "QualifSecret")
                    currencyCategory[noOfCurrencyCategories].level = 6;
                else
                    currencyCategory[noOfCurrencyCategories].level = 0;
                currencyCategory[noOfCurrencyCategories].value = value;
                noOfCurrencyCategories++;
                retVal = true;
            }
            return retVal;
        }
        public bool addContactCategory(string tag, string desc, string level)
        {
            bool retVal = false;
            if (noOfContactCategories < maxNoOfContactCategories)
            {
                contactCategory[noOfContactCategories].tag = tag;
                contactCategory[noOfContactCategories].description = desc; // Egently the path or number to make contact.
                if (level == "Open")
                    contactCategory[noOfContactCategories].level = 1;
                else if (level == "Limited")
                    contactCategory[noOfContactCategories].level = 2;
                else if (level == "Medium")
                    contactCategory[noOfContactCategories].level = 3;
                else if (level == "Relative")
                    contactCategory[noOfContactCategories].level = 4;
                else if (level == "Secret")
                    contactCategory[noOfContactCategories].level = 5;
                else if (level == "QualifSecret")
                    contactCategory[noOfContactCategories].level = 6;
                else
                    contactCategory[noOfContactCategories].level = 0;
                noOfContactCategories++;
                retVal = true;
            }
            return retVal;
        }
        public bool addHairColorCategory(string tag, string desc, string level)
        {
            bool retVal = false;
            if (noOfHairColorCategories < maxNoOfHairColorCategories)
            {
                hairColorCategory[noOfHairColorCategories].tag = tag;
                hairColorCategory[noOfHairColorCategories].description = desc;
                if (level == "Open")
                    hairColorCategory[noOfHairColorCategories].level = 1;
                else if (level == "Limited")
                    hairColorCategory[noOfHairColorCategories].level = 2;
                else if (level == "Medium")
                    hairColorCategory[noOfHairColorCategories].level = 3;
                else if (level == "Relative")
                    hairColorCategory[noOfHairColorCategories].level = 4;
                else if (level == "Secret")
                    hairColorCategory[noOfHairColorCategories].level = 5;
                else if (level == "QualifSecret")
                    hairColorCategory[noOfHairColorCategories].level = 6;
                else
                    hairColorCategory[noOfHairColorCategories].level = 0;
                noOfHairColorCategories++;
                retVal = true;
            }
            return retVal;
        }
        public bool addRoleCategory(string tag, string desc, string level)
        {
            bool retVal = false;
            if (noOfRoleCategories < maxNoOfRoleCategories)
            {
                roleCategories[noOfRoleCategories].tag = tag;
                roleCategories[noOfRoleCategories].description = desc;
                if (level == "Open")
                    roleCategories[noOfRoleCategories].level = 1;
                else if (level == "Limited")
                    roleCategories[noOfRoleCategories].level = 2;
                else if (level == "Medium")
                    roleCategories[noOfRoleCategories].level = 3;
                else if (level == "Relative")
                    roleCategories[noOfRoleCategories].level = 4;
                else if (level == "Secret")
                    roleCategories[noOfRoleCategories].level = 5;
                else if (level == "QualifSecret")
                    roleCategories[noOfRoleCategories].level = 6;
                else
                    roleCategories[noOfRoleCategories].level = 0;
                noOfRoleCategories++;
                retVal = true;
            }
            return retVal;
        }
        public void loadActor(string userID, string storagePath)
        {
            string filename = "";
            if ((storagePath != "") && (System.IO.Directory.Exists(storagePath)) &&
                (System.IO.File.Exists(storagePath + "\\ActorData_" + userID + ".acf")))
            {
                filename = storagePath + "\\ActorData_" + userID + ".acf";
            }
            else
            {
                int tnr = scu.IndexOf("\\");
                if ((tnr > 0) && (tnr < scu.Length - 1))
                    scu = scu.Substring(tnr + 1, scu.Length - tnr - 1);
                string rootPath = "C:\\Users\\" + scu + "\\" + sProgPath;
                filename = rootPath + "\\ActorData\\ActorData_" + userID + ".acf";
            }
            if (System.IO.File.Exists(filename))
            {
                foreach (string line in System.IO.File.ReadLines(filename))
                {
                    if (line != "-1")
                    {
                        int dp0, dp1, dp2, dp3, dp4, dp5, dp6, dp7, dp8, dp9, dp10, dp11, dp12;
                        string dataTag = line.Substring(0, 8);
                        string dataInfo = line.Substring(11, line.Length - 11);
                        switch (dataTag)
                        {
                            case "UserId ":
                                {
                                    this.userId = dataInfo;
                                    noOfDataSet++;
                                } break;
                            case "UserID ":
                                {
                                    this.userId = dataInfo;
                                    noOfDataSet++;
                                } break;
                            case "UserName":
                                {
                                    if (noOfNames < maxNoOfNames)
                                    {
                                        // format <surname>[ <midname(s)>][ <familyname>][; <nametype>]
                                        dp0 = dataInfo.IndexOf(";");
                                        if ((dp0 > 0) && (dp0 < dataInfo.Length - 2))
                                        {
                                            // We have name(s) and nametype
                                            string namntyp = dataInfo.Substring(dp0 + 2, dataInfo.Length - dp0 - 2);
                                            if (namntyp == "Birth")
                                                anvNamn[noOfNames].nameType = NameType.Birth;
                                            else if (namntyp == "Taken")
                                                anvNamn[noOfNames].nameType = NameType.Taken;
                                            else if (namntyp == "Married")
                                                anvNamn[noOfNames].nameType = NameType.Married;
                                            else if (namntyp == "Alias")
                                                anvNamn[noOfNames].nameType = NameType.Alias;
                                            else if ((namntyp == "Nick") || (namntyp == "Nickname") || (namntyp == "nick") || (namntyp == "nickname"))
                                                anvNamn[noOfNames].nameType = NameType.Nickname;
                                            else
                                                anvNamn[noOfNames].nameType = NameType.UndefNameType;
                                            noOfDataSet++;
                                            dataInfo = dataInfo.Substring(0, dp0);
                                            dp1 = dataInfo.IndexOf(" ");
                                            if ((dp1 > 0) && (dp1 < dataInfo.Length - 2))
                                            {
                                                // Have at least a surname and a familyname
                                                anvNamn[noOfNames].Surname = dataInfo.Substring(0, dp1);
                                                noOfDataSet++;
                                                dataInfo = dataInfo.Substring(dp1 + 1, dataInfo.Length - dp1 - 1);
                                                dp2 = dataInfo.IndexOf(" ");
                                                if ((dp2 > 0) && (dp2 < dataInfo.Length - 2))
                                                {
                                                    // Have middle-names
                                                    anvNamn[noOfNames].Midname = dataInfo.Substring(0, dp2);
                                                    noOfDataSet++;
                                                    anvNamn[noOfNames].Famname = dataInfo.Substring(dp2 + 1, dataInfo.Length - dp2 - 1);
                                                    noOfDataSet++;
                                                }
                                                else
                                                {
                                                    // Have only familyname
                                                    anvNamn[noOfNames].Famname = dataInfo;
                                                    noOfDataSet++;
                                                }
                                            }
                                            else
                                            {
                                                // Have only a single name
                                                anvNamn[noOfNames].Surname = dataInfo;
                                                noOfDataSet++;
                                            }
                                        }
                                        else
                                        {
                                            // We only have name(s)
                                            anvNamn[noOfNames].Surname = dataInfo;
                                            noOfDataSet++;
                                            anvNamn[noOfNames].nameType = NameType.UndefNameType;
                                            noOfDataSet++;
                                        }
                                        this.noOfNames++;
                                    }
                                } break;
                            case "Contact ":
                                {
                                    if (noOfContacts < maxNoOfContacts)
                                    {
                                        // Contact  : Type of Contact; contact path
                                        dp0 = dataInfo.IndexOf(";");
                                        if ((dp0 > 0) && (dp0 < dataInfo.Length - 2))
                                        {
                                            // We have Contact type and contact path
                                            bool foundCategory = false;
                                            string cttt = dataInfo.Substring(0, dp0);
                                            for (int i = 0; i < noOfContactCategories; i++)
                                            {
                                                if (contactCategory[i].tag == cttt)
                                                {
                                                    foundCategory = true;
                                                    anvKontakter[noOfContacts].type = contactCategory[i];
                                                    anvKontakter[noOfContacts].contactPath = dataInfo.Substring(dp0 + 2, dataInfo.Length - dp0 - 2);
                                                }
                                            }
                                            if (!(foundCategory))
                                            {
                                                if (addContactCategory(cttt, "", "Undefined"))
                                                {
                                                    anvKontakter[noOfContacts].type.tag = cttt;
                                                    anvKontakter[noOfContacts].contactPath = dataInfo.Substring(dp0 + 2, dataInfo.Length - dp0 - 2);
                                                    anvKontakter[noOfContacts].type.level = 0;
                                                }
                                            }
                                            noOfDataSet++;
                                        }
                                        else
                                        {
                                            anvKontakter[noOfContacts].contactPath = dataInfo.ToString();
                                            noOfDataSet++;
                                        }
                                        this.noOfContacts++;
                                    }
                                } break;
                            case "BrthData":
                                {
                                    // BrthData : number; additive; City; Area; Zipcode; Country; Date; Security code; Gender
                                    string tmpString = "";
                                    dp0 = dataInfo.IndexOf(";");
                                    if ((dp0 > 0) && (dp0 < dataInfo.Length - 2))
                                    {
                                        // Streetname; number; additive; City; Area; Zipcode; Country; Date; Security code; Gender
                                        tmpString = dataInfo.Substring(0, dp0);
                                        if ((tmpString != "Unknown") && (tmpString != "Undefined"))
                                        {
                                            anvFodlseData.brthStreet = tmpString;
                                            noOfDataSet++;
                                        }
                                        dataInfo = dataInfo.Substring(dp0 + 2, dataInfo.Length - dp0 - 2);
                                        dp1 = dataInfo.IndexOf(";");
                                        if ((dp1 > 0) && (dp1 < dataInfo.Length - 2))
                                        {
                                            // number; additive; City; Area; Zipcode; Country; Date; Security code; Gender
                                            tmpString = dataInfo.Substring(0, dp1);
                                            if ((tmpString != "Unknown") && (tmpString != "Undefined"))
                                            {
                                                int resval;
                                                if (int.TryParse(tmpString, out resval))
                                                {
                                                    anvFodlseData.brthStreetNumber = resval;
                                                    noOfDataSet++;
                                                }
                                            }
                                            dataInfo = dataInfo.Substring(dp1 + 2, dataInfo.Length - dp1 - 2);
                                            dp2 = dataInfo.IndexOf(";");
                                            if ((dp2 > 0) && (dp2 < dataInfo.Length - 2))
                                            {
                                                // additive; City; Area; Zipcode; Country; Date; Security code; Gender
                                                tmpString = dataInfo.Substring(0, dp2);
                                                if ((tmpString != "Unknown") && (tmpString != "Undefined"))
                                                {
                                                    anvFodlseData.brthStreetNumberAddon = tmpString;
                                                    noOfDataSet++;
                                                }
                                                dataInfo = dataInfo.Substring(dp2 + 2, dataInfo.Length - dp2 - 2);
                                                dp3 = dataInfo.IndexOf(";");
                                                if ((dp3 > 0) && (dp3 < dataInfo.Length - 2))
                                                {
                                                    // City; Area; Zipcode; Country; Date; Security code; Gender
                                                    tmpString = dataInfo.Substring(0, dp3);
                                                    if ((tmpString != "Unknown") && (tmpString != "Undefined"))
                                                    {
                                                        anvFodlseData.brthCityname = tmpString;
                                                        noOfDataSet++;
                                                    }
                                                    dataInfo = dataInfo.Substring(dp3 + 2, dataInfo.Length - dp3 - 2);
                                                    dp4 = dataInfo.IndexOf(";");
                                                    if ((dp4 > 0) && (dp4 < dataInfo.Length - 2))
                                                    {
                                                        // Area; Zipcode; Country; Date; Security code; Gender
                                                        tmpString = dataInfo.Substring(0, dp4);
                                                        if ((tmpString != "Unknown") && (tmpString != "Undefined"))
                                                        {
                                                            anvFodlseData.brthAreaname = tmpString;
                                                            noOfDataSet++;
                                                        }
                                                        dataInfo = dataInfo.Substring(dp4 + 2, dataInfo.Length - dp4 - 2);
                                                        dp5 = dataInfo.IndexOf(";");
                                                        if ((dp5 > 0) && (dp5 < dataInfo.Length - 2))
                                                        {
                                                            // Zipcode; Country; Date; Security code; Gender
                                                            tmpString = dataInfo.Substring(0, dp5);
                                                            if ((tmpString != "Unknown") && (tmpString != "Undefined"))
                                                            {
                                                                anvFodlseData.brthZipcode = tmpString;
                                                                noOfDataSet++;
                                                            }
                                                            dataInfo = dataInfo.Substring(dp5 + 2, dataInfo.Length - dp5 - 2);
                                                            dp6 = dataInfo.IndexOf(";");
                                                            if ((dp6 > 0) && (dp6 < dataInfo.Length - 2))
                                                            {
                                                                // Country; Date; Security code; Gender
                                                                tmpString = dataInfo.Substring(0, dp6);
                                                                if ((tmpString != "Unknown") && (tmpString != "Undefined"))
                                                                {
                                                                    anvFodlseData.brthCountryname = tmpString;
                                                                    noOfDataSet++;
                                                                }
                                                                dataInfo = dataInfo.Substring(dp6 + 2, dataInfo.Length - dp6 - 2);
                                                                dp7 = dataInfo.IndexOf(";");
                                                                if ((dp7 > 0) && (dp7 < dataInfo.Length - 2))
                                                                {
                                                                    // Date; Security code; Gender
                                                                    tmpString = dataInfo.Substring(0, dp7);
                                                                    int resval;
                                                                    if ((tmpString != "Unknown") && (tmpString != "Undefined"))
                                                                    {
                                                                        int sdp1 = tmpString.IndexOf("-");
                                                                        string yearString = tmpString.Substring(0, sdp1);
                                                                        if (int.TryParse(yearString, out resval))
                                                                        {
                                                                            anvFodlseData.brthDate.year = resval;
                                                                            noOfDataSet++;
                                                                        }
                                                                        tmpString = tmpString.Substring(sdp1 + 1, tmpString.Length - sdp1 - 1);
                                                                        int sdp2 = tmpString.IndexOf("-");
                                                                        string monthString = tmpString.Substring(0, sdp2);
                                                                        if (int.TryParse(monthString, out resval))
                                                                        {
                                                                            anvFodlseData.brthDate.month = resval;
                                                                            noOfDataSet++;
                                                                        }
                                                                        tmpString = tmpString.Substring(sdp2 + 1, tmpString.Length - sdp2 - 1);
                                                                        if (int.TryParse(tmpString, out resval))
                                                                        {
                                                                            anvFodlseData.brthDate.day = resval;
                                                                            noOfDataSet++;
                                                                        }
                                                                    }
                                                                    dataInfo = dataInfo.Substring(dp7 + 2, dataInfo.Length - dp7 - 2);
                                                                    dp8 = dataInfo.IndexOf(";");
                                                                    if ((dp8 > 0) && (dp8 < dataInfo.Length - 2))
                                                                    {
                                                                        // Security code; Gender
                                                                        tmpString = dataInfo.Substring(0, dp8);
                                                                        if ((tmpString != "Unknown") && (tmpString != "Undefined"))
                                                                        {
                                                                            anvFodlseData.brthSecurityCode = tmpString;
                                                                            noOfDataSet++;
                                                                        }
                                                                        dataInfo = dataInfo.Substring(dp8 + 2, dataInfo.Length - dp8 - 2);
                                                                        // Gender; lat value (DDMMSSss) latDirection (H); lonValue (DDDMMSSss); lonDirection (H)
                                                                        dp9 = dataInfo.IndexOf(";");
                                                                        if ((dp9 > 0) && (dp9 < dataInfo.Length - 2))
                                                                        {
                                                                            tmpString = dataInfo.Substring(0, dp9);
                                                                            if (tmpString == "Male")
                                                                                anvFodlseData.brthGender = GenderType.Male;
                                                                            else if (tmpString == "Female")
                                                                                anvFodlseData.brthGender = GenderType.Female;
                                                                            else
                                                                                anvFodlseData.brthGender = GenderType.UndefGender;
                                                                            noOfDataSet++;
                                                                            dataInfo = dataInfo.Substring(dp9 + 2, dataInfo.Length - dp9 - 2);
                                                                            // lat value (DDMMSSss) latDirection (H); lonValue (DDDMMSSss); lonDirection (H)
                                                                            dp10 = dataInfo.IndexOf(";");
                                                                            if ((dp10 > 0) && (dp10 < dataInfo.Length - 2))
                                                                            {
                                                                                tmpString = dataInfo.Substring(0, dp10);
                                                                                float fTmp;
                                                                                if (float.TryParse(tmpString, NumberStyles.Any, CultureInfo.InvariantCulture, out fTmp))
                                                                                    anvFodlseData.latVal = fTmp;
                                                                                noOfDataSet++;
                                                                                dataInfo = dataInfo.Substring(dp10 + 2, dataInfo.Length - dp10 - 2);
                                                                                // latDirection (H); lonValue (DDDMMSSss); lonDirection (H)
                                                                                dp11 = dataInfo.IndexOf(";");
                                                                                if ((dp11 > 0) && (dp11 < dataInfo.Length - 2))
                                                                                {
                                                                                    tmpString = dataInfo.Substring(0, dp11);
                                                                                    if ((tmpString == "North") || (tmpString == "north") || (tmpString == "N") || (tmpString == "n"))
                                                                                        anvFodlseData.latDir = GeogrDir.North;
                                                                                    else if ((tmpString == "South") || (tmpString == "south") || (tmpString == "S") || (tmpString == "s"))
                                                                                        anvFodlseData.latDir = GeogrDir.South;
                                                                                    else
                                                                                        anvFodlseData.latDir = GeogrDir.UndefGeoDir;
                                                                                    noOfDataSet++;
                                                                                    dataInfo = dataInfo.Substring(dp11 + 2, dataInfo.Length - dp11 - 2);
                                                                                    // lonValue (DDDMMSSss); lonDirection (H)
                                                                                    dp12 = dataInfo.IndexOf(";");
                                                                                    if ((dp12 > 0) && (dp12 < dataInfo.Length - 2))
                                                                                    {
                                                                                        tmpString = dataInfo.Substring(0, dp12);
                                                                                        if (float.TryParse(tmpString, NumberStyles.Any, CultureInfo.InvariantCulture, out fTmp))
                                                                                            anvFodlseData.lonVal = fTmp;
                                                                                        noOfDataSet++;
                                                                                        dataInfo = dataInfo.Substring(dp12 + 2, dataInfo.Length - dp12 - 2);
                                                                                        // lonDirection (H)
                                                                                        if ((dataInfo == "East") || (dataInfo == "east") || (dataInfo == "E") || (dataInfo == "e"))
                                                                                            anvFodlseData.lonDir = GeogrDir.East;
                                                                                        else if ((dataInfo == "West") || (dataInfo == "west") || (dataInfo == "W") || (dataInfo == "w"))
                                                                                            anvFodlseData.lonDir = GeogrDir.West;
                                                                                        else
                                                                                            anvFodlseData.lonDir = GeogrDir.UndefGeoDir;
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                        else
                                                                        {
                                                                            if ((dataInfo.Length > 0) && (dataInfo != " "))
                                                                            {
                                                                                if (dataInfo == "Male")
                                                                                    anvFodlseData.brthGender = GenderType.Male;
                                                                                else if (dataInfo == "Female")
                                                                                    anvFodlseData.brthGender = GenderType.Female;
                                                                                else
                                                                                    anvFodlseData.brthGender = GenderType.UndefGender;
                                                                                noOfDataSet++;
                                                                            }
                                                                            else
                                                                            {
                                                                                anvFodlseData.brthGender = GenderType.UndefGender;
                                                                                noOfDataSet++;
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }

                                    }
                                } break;
                            case "SkinTone":
                                {
                                    // SkinTone : Complexion; R-value; G-value; B-value; Valid-Date
                                    if (noOfComplexions < maxNoOfComplexions)
                                    {
                                        dp0 = dataInfo.IndexOf(";");
                                        if ((dp0 > 0) && (dp0 < dataInfo.Length - 2))
                                        {
                                            // Complexion; R-value; G-value; B-value; Valid-Date
                                            string cplx = dataInfo.Substring(0, dp0);
                                            if (cplx == "Negro")
                                                anvHudfarg[noOfComplexions].etnicType = EtnicityType.Negro;
                                            else if (cplx == "Asian")
                                                anvHudfarg[noOfComplexions].etnicType = EtnicityType.Asian;
                                            else if (cplx == "Indian")
                                                anvHudfarg[noOfComplexions].etnicType = EtnicityType.Indian;
                                            else if (cplx == "Cocation")
                                                anvHudfarg[noOfComplexions].etnicType = EtnicityType.Cocation;
                                            else if (cplx == "Mulatto")
                                                anvHudfarg[noOfComplexions].etnicType = EtnicityType.Mulatto;
                                            else if (cplx == "Innuit")
                                                anvHudfarg[noOfComplexions].etnicType = EtnicityType.Innuit;
                                            else
                                                anvHudfarg[noOfComplexions].etnicType = EtnicityType.UndefEtnicity;
                                            noOfDataSet++;
                                            dataInfo = dataInfo.Substring(dp0 + 2, dataInfo.Length - dp0 - 2);
                                            dp1 = dataInfo.IndexOf(";");
                                            if ((dp1 > 0) && (dp1 < dataInfo.Length - 2))
                                            {
                                                // R-value; G-value; B-value; Valid-Date
                                                int resval;
                                                if (int.TryParse(dataInfo.Substring(0, dp1), out resval))
                                                {
                                                    anvHudfarg[noOfComplexions].redChannelValue = resval;
                                                    noOfDataSet++;
                                                }
                                                dataInfo = dataInfo.Substring(dp1 + 2, dataInfo.Length - dp1 - 2);
                                                dp2 = dataInfo.IndexOf(";");
                                                if ((dp2 > 0) && (dp2 < dataInfo.Length - 2))
                                                {
                                                    // G-value; B-value; Valid-Date
                                                    if (int.TryParse(dataInfo.Substring(0, dp2), out resval))
                                                    {
                                                        anvHudfarg[noOfComplexions].greenChannelValue = resval;
                                                        noOfDataSet++;
                                                    }
                                                    dataInfo = dataInfo.Substring(dp2 + 2, dataInfo.Length - dp2 - 2);
                                                    dp3 = dataInfo.IndexOf(";");
                                                    if ((dp3 > 0) && (dp3 < dataInfo.Length - 2))
                                                    {
                                                        // B-value; Valid-Date
                                                        if (int.TryParse(dataInfo.Substring(0, dp3), out resval))
                                                        {
                                                            anvHudfarg[noOfComplexions].blueChannelValue = resval;
                                                            noOfDataSet++;
                                                        }
                                                        dataInfo = dataInfo.Substring(dp3 + 2, dataInfo.Length - dp3 - 2);
                                                        // Valid-Date [YYYY-MM-DD|YY-MM-DD|YYYY-MM|YY-MM|YYYY|MM]
                                                        dp5 = dataInfo.IndexOf("-");
                                                        if ((dp5 > 0) && (dp5 < dataInfo.Length - 1))
                                                        {
                                                            // Valid-Date [YYYY-MM-DD|YY-MM-DD|YYYY-MM|YY-MM]
                                                            string yrval = dataInfo.Substring(0, dp5);
                                                            if (int.TryParse(yrval, out resval))
                                                            {
                                                                anvHudfarg[noOfComplexions].validDate.year = resval;
                                                                noOfDataSet++;
                                                            }
                                                            dataInfo = dataInfo.Substring(dp5 + 1, dataInfo.Length - dp5 - 1);
                                                            dp6 = dataInfo.IndexOf("-");
                                                            if ((dp6 > 0) && (dp6 < dataInfo.Length - 1))
                                                            {
                                                                // Valid-Date [MM-DD]
                                                                string mntval = dataInfo.Substring(0, dp6);
                                                                if (int.TryParse(mntval, out resval))
                                                                {
                                                                    anvHudfarg[noOfComplexions].validDate.month = resval;
                                                                    noOfDataSet++;
                                                                }
                                                                string dayval = dataInfo.Substring(dp6 + 1, dataInfo.Length - dp6 - 1);
                                                                if (int.TryParse(dayval, out resval))
                                                                {
                                                                    anvHudfarg[noOfComplexions].validDate.day = resval;
                                                                    noOfDataSet++;
                                                                }
                                                            }
                                                            else
                                                            {
                                                                // Valid-Date [MM|MM]
                                                                string mntval = dataInfo.Substring(0, dp6);
                                                                if (int.TryParse(mntval, out resval))
                                                                {
                                                                    anvHudfarg[noOfComplexions].validDate.month = resval;
                                                                    noOfDataSet++;
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            // Valid-Date [YYYY|YY]
                                                            string yrval = dataInfo.Substring(0, dp5);
                                                            if (int.TryParse(yrval, out resval))
                                                            {
                                                                anvHudfarg[noOfComplexions].validDate.year = resval;
                                                                noOfDataSet++;
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        noOfComplexions++;
                                    }
                                } break;
                            case "RelStats":
                                {
                                    bool foundCategory = false;
                                    for (int i = 0; i < noOfRelationCategories; i++)
                                    {
                                        if (relationCategories[i].tag == dataInfo)
                                        {
                                            foundCategory = true;
                                            anvAktuellRelation = relationCategories[i];
                                        }
                                    }
                                    if (!(foundCategory))
                                    {
                                        anvAktuellRelation.tag = dataInfo;
                                        anvAktuellRelation.level = 0;
                                        anvAktuellRelation.description = "";
                                        relationCategories[noOfRelationCategories].tag = dataInfo;
                                    }
                                    noOfDataSet++;
                                } break;
                            case "Gender  ":
                                {
                                    // Gender   : GenderType "type"; float "length"; float "circumference"; DistUnits "usedUnits"; GenderAppearanceType "appearance"; GenderBhaviourType "behaviour"; string "presentation"; myDateTime "validDate"
                                    if (noOfGenderInfo < maxNoOfGenderInfo)
                                    {
                                        dp0 = dataInfo.IndexOf(";");
                                        if ((dp0 > 0) && (dp0 < dataInfo.Length - 2))
                                        {
                                            // GenderType "type"; float "length"; float "circumference"; DistUnits "usedUnits"; GenderAppearanceType "appearance"; GenderBhaviourType "behaviour"; string "presentation"; DateTime "validDate"
                                            string gdrtyp = dataInfo.Substring(0, dp0);
                                            if ((gdrtyp == "Male") || (gdrtyp == "male"))
                                                anvKonsInfo[noOfGenderInfo].type = GenderType.Male;
                                            else if ((gdrtyp == "Female") || (gdrtyp == "female"))
                                                anvKonsInfo[noOfGenderInfo].type = GenderType.Female;
                                            else
                                                anvKonsInfo[noOfGenderInfo].type = GenderType.UndefGender;
                                            noOfDataSet++;
                                            dataInfo = dataInfo.Substring(dp0 + 2, dataInfo.Length - dp0 - 2);
                                            dp1 = dataInfo.IndexOf(";");
                                            int resval;
                                            if ((dp1 > 0) && (dp1 < dataInfo.Length - 2))
                                            {
                                                // float "length"; float "circumference"; DistUnits "usedUnits"; GenderAppearanceType "appearance"; GenderBhaviourType "behaviour"; string "presentation"; DateTime "validDate"
                                                string sfl = dataInfo.Substring(0, dp1);
                                                float fresval;
                                                if (float.TryParse(sfl, NumberStyles.Any, CultureInfo.InvariantCulture, out fresval))
                                                {
                                                    anvKonsInfo[noOfGenderInfo].length = fresval;
                                                    noOfDataSet++;
                                                }
                                                dataInfo = dataInfo.Substring(dp1 + 2, dataInfo.Length - dp1 - 2);
                                                dp2 = dataInfo.IndexOf(";");
                                                if ((dp2 > 0) && (dp2 < dataInfo.Length - 2))
                                                {
                                                    // float "circumference"; DistUnits "usedUnits"; GenderAppearanceType "appearance"; GenderBhaviourType "behaviour"; string "presentation"; DateTime "validDate"
                                                    string sfc = dataInfo.Substring(0, dp2);
                                                    if (float.TryParse(sfc, NumberStyles.Any, CultureInfo.InvariantCulture, out fresval))
                                                    {
                                                        anvKonsInfo[noOfGenderInfo].circumference = fresval;
                                                        noOfDataSet++;
                                                    }
                                                    dataInfo = dataInfo.Substring(dp2 + 2, dataInfo.Length - dp2 - 2);
                                                    dp3 = dataInfo.IndexOf(";");
                                                    if ((dp3 > 0) && (dp3 < dataInfo.Length - 2))
                                                    {
                                                        // DistUnits "usedUnits"; GenderAppearanceType "appearance"; GenderBhaviourType "behaviour"; string "presentation"; DateTime "validDate"
                                                        string disu = dataInfo.Substring(0, dp3);
                                                        if ((disu == "Points") || (disu == "points"))
                                                            anvKonsInfo[noOfGenderInfo].usedUnits = DistUnits.Points;
                                                        else if ((disu == "Millimeter") || (disu == "millimeter"))
                                                            anvKonsInfo[noOfGenderInfo].usedUnits = DistUnits.Millimeter;
                                                        else if ((disu == "Centimeter") || (disu == "centimeter"))
                                                            anvKonsInfo[noOfGenderInfo].usedUnits = DistUnits.Centimeter;
                                                        else if ((disu == "Decimeter") || (disu == "decimeter"))
                                                            anvKonsInfo[noOfGenderInfo].usedUnits = DistUnits.Decimeter;
                                                        else if ((disu == "Meter") || (disu == "meter"))
                                                            anvKonsInfo[noOfGenderInfo].usedUnits = DistUnits.Meter;
                                                        else if ((disu == "Inch") || (disu == "inch"))
                                                            anvKonsInfo[noOfGenderInfo].usedUnits = DistUnits.Inch;
                                                        else if ((disu == "Feet") || (disu == "feet"))
                                                            anvKonsInfo[noOfGenderInfo].usedUnits = DistUnits.Feet;
                                                        else if ((disu == "Yard") || (disu == "yard"))
                                                            anvKonsInfo[noOfGenderInfo].usedUnits = DistUnits.Yard;
                                                        else
                                                            anvKonsInfo[noOfGenderInfo].usedUnits = DistUnits.UndefDistUnit;
                                                        noOfDataSet++;
                                                        dataInfo = dataInfo.Substring(dp3 + 2, dataInfo.Length - dp3 - 2);
                                                        dp4 = dataInfo.IndexOf(";");
                                                        if ((dp4 > 0) && (dp4 < dataInfo.Length - 2))
                                                        {
                                                            // GenderAppearanceType "appearance"; GenderBhaviourType "behaviour"; string "presentation"; DateTime "validDate"
                                                            string gat = dataInfo.Substring(0, dp4);
                                                            if ((gat == "Barbie") || (gat == "barbie"))
                                                                anvKonsInfo[noOfGenderInfo].appearance = GenderAppearanceType.Barbie;
                                                            else if ((gat == "Curtains") || (gat == "curtains"))
                                                                anvKonsInfo[noOfGenderInfo].appearance = GenderAppearanceType.Curtains;
                                                            else if ((gat == "Horseshoe") || (gat == "horseshoe"))
                                                                anvKonsInfo[noOfGenderInfo].appearance = GenderAppearanceType.Horseshoe;
                                                            else if ((gat == "Puffy") || (gat == "puffy"))
                                                                anvKonsInfo[noOfGenderInfo].appearance = GenderAppearanceType.Puffy;
                                                            else if ((gat == "Tulip") || (gat == "tulip"))
                                                                anvKonsInfo[noOfGenderInfo].appearance = GenderAppearanceType.Tulip;
                                                            else if ((gat == "I") || (gat == "1"))
                                                                anvKonsInfo[noOfGenderInfo].appearance = GenderAppearanceType.I;
                                                            else if ((gat == "II") || (gat == "2"))
                                                                anvKonsInfo[noOfGenderInfo].appearance = GenderAppearanceType.II;
                                                            else if ((gat == "III") || (gat == "3"))
                                                                anvKonsInfo[noOfGenderInfo].appearance = GenderAppearanceType.III;
                                                            else if ((gat == "IV") || (gat == "4"))
                                                                anvKonsInfo[noOfGenderInfo].appearance = GenderAppearanceType.IV;
                                                            else if ((gat == "V") || (gat == "5"))
                                                                anvKonsInfo[noOfGenderInfo].appearance = GenderAppearanceType.V;
                                                            else
                                                                anvKonsInfo[noOfGenderInfo].appearance = GenderAppearanceType.UndefGenderAppearance;
                                                            noOfDataSet++;
                                                            dataInfo = dataInfo.Substring(dp4 + 2, dataInfo.Length - dp4 - 2);
                                                            dp5 = dataInfo.IndexOf(";");
                                                            if ((dp5 > 0) && (dp5 < dataInfo.Length - 2))
                                                            {
                                                                // GenderBhaviourType "behaviour"; string "presentation"; DateTime "validDate"
                                                                string gbt = dataInfo.Substring(0, dp5);
                                                                if ((gbt == "Sloppy") || (gbt == "sloppy"))
                                                                    anvKonsInfo[noOfGenderInfo].behaviour = GenderBhaviourType.Sloppy;
                                                                else if ((gbt == "Stretchy") || (gbt == "stretchy"))
                                                                    anvKonsInfo[noOfGenderInfo].behaviour = GenderBhaviourType.Stretchy;
                                                                else if ((gbt == "Tight") || (gbt == "tight"))
                                                                    anvKonsInfo[noOfGenderInfo].behaviour = GenderBhaviourType.Tight;
                                                                else if ((gbt == "Stiffer") || (gbt == "stiffer"))
                                                                    anvKonsInfo[noOfGenderInfo].behaviour = GenderBhaviourType.Stiffer;
                                                                else if ((gbt == "Grower") || (gbt == "grower"))
                                                                    anvKonsInfo[noOfGenderInfo].behaviour = GenderBhaviourType.Grower;
                                                                else
                                                                    anvKonsInfo[noOfGenderInfo].behaviour = GenderBhaviourType.UndeBehaviourType;
                                                                noOfDataSet++;
                                                                dataInfo = dataInfo.Substring(dp5 + 2, dataInfo.Length - dp5 - 2);
                                                                dp6 = dataInfo.IndexOf(";");
                                                                if ((dp6 > 0) && (dp6 < dataInfo.Length - 2))
                                                                {
                                                                    // string "presentation"; DateTime "validDate"
                                                                    string spres = dataInfo.Substring(0, dp6);
                                                                    if ((spres != "Undefined") && (spres != "Undefined") && (spres != ""))
                                                                    {
                                                                        anvKonsInfo[noOfGenderInfo].presentation = dataInfo.Substring(0, dp6);
                                                                        noOfDataSet++;
                                                                    }
                                                                    dataInfo = dataInfo.Substring(dp6 + 2, dataInfo.Length - dp6 - 2);
                                                                    // DateTime "validDate" [YYYY-MM-DD|YY-MM-DD|YYYY-MM|YY-MM|YYYY|YY]
                                                                    dp7 = dataInfo.IndexOf("-");
                                                                    if ((dp7 > 0) && (dp7 < dataInfo.Length - 1))
                                                                    {
                                                                        // DateTime "validDate" [YYYY-MM-DD|YY-MM-DD|YYYY-MM|YY-MM]
                                                                        if (int.TryParse(dataInfo.Substring(0, dp7), out resval))
                                                                        {
                                                                            anvKonsInfo[noOfGenderInfo].validDate.year = resval;
                                                                            noOfDataSet++;
                                                                        }
                                                                        dataInfo = dataInfo.Substring(dp7 + 1, dataInfo.Length - dp7 - 1);
                                                                        dp8 = dataInfo.IndexOf("-");
                                                                        if ((dp8 > 0) && (dp8 < dataInfo.Length - 1))
                                                                        {
                                                                            // DateTime "validDate" [MM-DD]
                                                                            string svdmon = dataInfo.Substring(0, dp8);
                                                                            if (int.TryParse(svdmon, out resval))
                                                                            {
                                                                                anvKonsInfo[noOfGenderInfo].validDate.month = resval;
                                                                                noOfDataSet++;
                                                                            }
                                                                            string svdday = dataInfo.Substring(dp8 + 1, dataInfo.Length - dp8 - 1);
                                                                            if (int.TryParse(svdday, out resval))
                                                                            {
                                                                                anvKonsInfo[noOfGenderInfo].validDate.day = resval;
                                                                                noOfDataSet++;
                                                                            }
                                                                        }
                                                                        else
                                                                        {
                                                                            // DateTime "validDate" [MM]
                                                                            if (int.TryParse(dataInfo, out resval))
                                                                            {
                                                                                anvKonsInfo[noOfGenderInfo].validDate.month = resval;
                                                                                noOfDataSet++;
                                                                            }
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        // DateTime "validDate" [YYYY|YY]
                                                                        if (int.TryParse(dataInfo, out resval))
                                                                        {
                                                                            anvKonsInfo[noOfGenderInfo].validDate.year = resval;
                                                                            noOfDataSet++;
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        noOfGenderInfo++;
                                    }
                                } break;
                            case "Length  ":
                                {
                                    // Length   : Value; Unit; Valid-Date
                                    if (noOfLengthData < maxNoOfLengthData)
                                    {
                                        dp0 = dataInfo.IndexOf(";");
                                        if ((dp0 > 0) && (dp0 < dataInfo.Length - 2))
                                        {
                                            string sVal = dataInfo.Substring(0, dp0);
                                            float fVal;
                                            if (float.TryParse(sVal, NumberStyles.Any, CultureInfo.InvariantCulture, out fVal))
                                            {
                                                anvLangdData[noOfLengthData].value = fVal;
                                                noOfDataSet++;
                                            }
                                            dataInfo = dataInfo.Substring(dp0 + 2, dataInfo.Length - dp0 - 2);
                                            dp1 = dataInfo.IndexOf(";");
                                            if ((dp1 > 0) && (dp1 < dataInfo.Length - 2))
                                            {
                                                string sUnit = dataInfo.Substring(0, dp1);
                                                if ((sUnit == "Points") || (sUnit == "points"))
                                                    anvLangdData[noOfLengthData].unit = DistUnits.Points;
                                                else if ((sUnit == "Millimeter") || (sUnit == "millimeter"))
                                                    anvLangdData[noOfLengthData].unit = DistUnits.Millimeter;
                                                else if ((sUnit == "Centimeter") || (sUnit == "centimeter"))
                                                    anvLangdData[noOfLengthData].unit = DistUnits.Centimeter;
                                                else if ((sUnit == "Decimeter") || (sUnit == "decimeter"))
                                                    anvLangdData[noOfLengthData].unit = DistUnits.Decimeter;
                                                else if ((sUnit == "Meter") || (sUnit == "meter"))
                                                    anvLangdData[noOfLengthData].unit = DistUnits.Meter;
                                                else if ((sUnit == "Inch") || (sUnit == "inch"))
                                                    anvLangdData[noOfLengthData].unit = DistUnits.Inch;
                                                else if ((sUnit == "Feet") || (sUnit == "feet"))
                                                    anvLangdData[noOfLengthData].unit = DistUnits.Feet;
                                                else if ((sUnit == "Yard") || (sUnit == "yard"))
                                                    anvLangdData[noOfLengthData].unit = DistUnits.Yard;
                                                else
                                                    anvLangdData[noOfLengthData].unit = DistUnits.UndefDistUnit;
                                                noOfDataSet++;
                                                dataInfo = dataInfo.Substring(dp1 + 2, dataInfo.Length - dp1 - 2);
                                                dp2 = dataInfo.IndexOf("-");
                                                if ((dp2 > 0) && (dp2 < dataInfo.Length - 1))
                                                {
                                                    string svay = dataInfo.Substring(0, dp2);
                                                    int resval;
                                                    if (int.TryParse(svay, out resval))
                                                    {
                                                        anvLangdData[noOfLengthData].validDate.year = resval;
                                                        noOfDataSet++;
                                                    }
                                                    dataInfo = dataInfo.Substring(dp2 + 1, dataInfo.Length - dp2 - 1);
                                                    dp3 = dataInfo.IndexOf("-");
                                                    if ((dp3 > 0) && (dp3 < dataInfo.Length - 1))
                                                    {
                                                        string svam = dataInfo.Substring(0, dp3);
                                                        if (int.TryParse(svam, out resval))
                                                        {
                                                            anvLangdData[noOfLengthData].validDate.month = resval;
                                                            noOfDataSet++;
                                                        }
                                                        dataInfo = dataInfo.Substring(dp3 + 1, dataInfo.Length - dp3 - 1);
                                                        if (int.TryParse(dataInfo, out resval))
                                                        {
                                                            anvLangdData[noOfLengthData].validDate.day = resval;
                                                            noOfDataSet++;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        // we only have month
                                                        if (int.TryParse(dataInfo, out resval))
                                                        {
                                                            anvLangdData[noOfLengthData].validDate.month = resval;
                                                            noOfDataSet++;
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    // We only have a year
                                                    int resval;
                                                    if (int.TryParse(dataInfo, out resval))
                                                    {
                                                        anvLangdData[noOfLengthData].validDate.year = resval;
                                                        noOfDataSet++;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                // We only have unit
                                                if ((dataInfo == "Points") || (dataInfo == "points"))
                                                    anvLangdData[noOfLengthData].unit = DistUnits.Points;
                                                else if ((dataInfo == "Millimeter") || (dataInfo == "millimeter"))
                                                    anvLangdData[noOfLengthData].unit = DistUnits.Millimeter;
                                                else if ((dataInfo == "Centimeter") || (dataInfo == "centimeter"))
                                                    anvLangdData[noOfLengthData].unit = DistUnits.Centimeter;
                                                else if ((dataInfo == "Decimeter") || (dataInfo == "decimeter"))
                                                    anvLangdData[noOfLengthData].unit = DistUnits.Decimeter;
                                                else if ((dataInfo == "Meter") || (dataInfo == "meter"))
                                                    anvLangdData[noOfLengthData].unit = DistUnits.Meter;
                                                else if ((dataInfo == "Inch") || (dataInfo == "inch"))
                                                    anvLangdData[noOfLengthData].unit = DistUnits.Inch;
                                                else if ((dataInfo == "Feet") || (dataInfo == "feet"))
                                                    anvLangdData[noOfLengthData].unit = DistUnits.Feet;
                                                else if ((dataInfo == "Yard") || (dataInfo == "yard"))
                                                    anvLangdData[noOfLengthData].unit = DistUnits.Yard;
                                                else
                                                    anvLangdData[noOfLengthData].unit = DistUnits.UndefDistUnit;
                                            }
                                        }
                                        else
                                        {
                                            // We only have Value, assuming meter.
                                            float dblVal;
                                            if (float.TryParse(dataInfo, out dblVal))
                                            {
                                                anvLangdData[noOfLengthData].value = dblVal;
                                                anvLangdData[noOfLengthData].unit = DistUnits.Meter;
                                                noOfDataSet++;
                                            }
                                        }
                                        noOfLengthData++;
                                    }
                                } break;
                            case "Weight  ":
                                {
                                    // Weight   : Value; unit; Valid-Date
                                    if (noOfWeightData < maxNoOfWeightData)
                                    {
                                        dp0 = dataInfo.IndexOf(";");
                                        if ((dp0 > 0) && (dp0 < dataInfo.Length - 2))
                                        {
                                            string sVal = dataInfo.Substring(0, dp0);
                                            float fVal;
                                            if (float.TryParse(sVal, NumberStyles.Any, CultureInfo.InvariantCulture, out fVal))
                                            {
                                                anvViktData[noOfWeightData].value = fVal;
                                                noOfDataSet++;
                                            }
                                            dataInfo = dataInfo.Substring(dp0 + 2, dataInfo.Length - dp0 - 2);
                                            dp1 = dataInfo.IndexOf(";");
                                            if ((dp1 > 0) && (dp1 < dataInfo.Length - 2))
                                            {
                                                string sUnit = dataInfo.Substring(0, dp1);
                                                if ((sUnit == "Gram") || (sUnit == "gram"))
                                                    anvViktData[noOfWeightData].unit = WeightUnits.Gram;
                                                else if ((sUnit == "Kilogram") || (sUnit == "KiloGram") || (sUnit == "kilogram"))
                                                    anvViktData[noOfWeightData].unit = WeightUnits.KiloGram;
                                                else if ((sUnit == "Tonnes") || (sUnit == "tonnes"))
                                                    anvViktData[noOfWeightData].unit = WeightUnits.Tonnes;
                                                else if ((sUnit == "Pound")|| (sUnit == "pound"))
                                                    anvViktData[noOfWeightData].unit = WeightUnits.Pound;
                                                else if ((sUnit == "Stones") == (sUnit == "stones"))
                                                    anvViktData[noOfWeightData].unit = WeightUnits.Stones;
                                                else
                                                    anvViktData[noOfWeightData].unit = WeightUnits.UndefWeightUnit;
                                                dataInfo = dataInfo.Substring(dp1 + 2, dataInfo.Length - dp1 - 2);
                                                dp2 = dataInfo.IndexOf("-");
                                                if ((dp2 > 0) && (dp2 < dataInfo.Length - 1))
                                                {
                                                    string svay = dataInfo.Substring(0, dp2);
                                                    int resval;
                                                    if (int.TryParse(svay, out resval))
                                                    {
                                                        anvViktData[noOfWeightData].validDate.year = resval;
                                                        noOfDataSet++;
                                                    }
                                                    dataInfo = dataInfo.Substring(dp2 + 1, dataInfo.Length - dp2 - 1);
                                                    dp3 = dataInfo.IndexOf("-");
                                                    if ((dp3 > 0) && (dp3 < dataInfo.Length - 1))
                                                    {
                                                        string svam = dataInfo.Substring(0, dp3);
                                                        if (int.TryParse(svam, out resval))
                                                        {
                                                            anvViktData[noOfWeightData].validDate.month = resval;
                                                            noOfDataSet++;
                                                        }
                                                        dataInfo = dataInfo.Substring(dp3 + 1, dataInfo.Length - dp3 - 1);
                                                        if (int.TryParse(dataInfo, out resval))
                                                        {
                                                            anvViktData[noOfWeightData].validDate.day = resval;
                                                            noOfDataSet++;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        // we only have month
                                                        if (int.TryParse(dataInfo, out resval))
                                                        {
                                                            anvViktData[noOfWeightData].validDate.month = resval;
                                                            noOfDataSet++;
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    // We only have a year
                                                    int resval;
                                                    if (int.TryParse(dataInfo, out resval))
                                                    {
                                                        anvViktData[noOfWeightData].validDate.year = resval;
                                                        noOfDataSet++;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                // We only have unit
                                                if ((dataInfo == "Gram") || (dataInfo == "gram"))
                                                    anvViktData[noOfWeightData].unit = WeightUnits.Gram;
                                                else if ((dataInfo == "Kilogram") || (dataInfo == "KiloGram") || (dataInfo == "kilogram"))
                                                    anvViktData[noOfWeightData].unit = WeightUnits.KiloGram;
                                                else if ((dataInfo == "Tonnes") || (dataInfo == "tonnes"))
                                                    anvViktData[noOfWeightData].unit = WeightUnits.Tonnes;
                                                else if ((dataInfo == "Pound") || (dataInfo == "pound"))
                                                    anvViktData[noOfWeightData].unit = WeightUnits.Pound;
                                                else if ((dataInfo == "Stones") || (dataInfo == "stones"))
                                                    anvViktData[noOfWeightData].unit = WeightUnits.Stones;
                                                else
                                                    anvViktData[noOfWeightData].unit = WeightUnits.UndefWeightUnit;
                                            }
                                        }
                                        else
                                        {
                                            // We only have Value, assuming meter.
                                            float fVal;
                                            if (float.TryParse(dataInfo, NumberStyles.Any, CultureInfo.InvariantCulture, out fVal))
                                            {
                                                anvViktData[noOfWeightData].value = fVal;
                                                anvViktData[noOfWeightData].unit = WeightUnits.KiloGram;
                                                noOfDataSet++;
                                            }
                                        }
                                        noOfWeightData++;
                                    }
                                } break;
                            case "Boobs   ":
                                {
                                    // Boobs    : Type-tag; Circumf; Unit; Size-tag; Valid-Date
                                    if (noOfChestData < maxNoOfChestData)
                                    {
                                        dp0 = dataInfo.IndexOf(";");
                                        if ((dp0 > 0) && (dp0 < dataInfo.Length - 2))
                                        {
                                            // Type-tag; Circumf; Unit; Size-tag; Valid-Date
                                            string sChTp = dataInfo.Substring(0, dp0);
                                            if (sChTp == "Natural")
                                                anvBrostData[noOfChestData].type = BreastType.NaturalBreasts;
                                            else if ((sChTp == "Silicone") || (sChTp == "Fake") || (sChTp == "Plastic"))
                                                anvBrostData[noOfChestData].type = BreastType.SiliconeBreasts;
                                            else
                                                anvBrostData[noOfChestData].type = BreastType.UndefBreastType;
                                            noOfDataSet++;
                                            dataInfo = dataInfo.Substring(dp0 + 2, dataInfo.Length - dp0 - 2);
                                            dp1 = dataInfo.IndexOf(";");
                                            if ((dp1 > 0) && (dp1 < dataInfo.Length - 2))
                                            {
                                                // Circumf; Unit; Size-tag; Valid-Date
                                                string sChVal = dataInfo.Substring(0, dp1);
                                                float fVal;
                                                if (float.TryParse(sChVal, NumberStyles.Any, CultureInfo.InvariantCulture, out fVal))
                                                {
                                                    anvBrostData[noOfChestData].circumference = fVal;
                                                    noOfDataSet++;
                                                }
                                                dataInfo = dataInfo.Substring(dp1 + 2, dataInfo.Length - dp1 - 2);
                                                dp2 = dataInfo.IndexOf(";");
                                                if ((dp2 > 0) && (dp2 < dataInfo.Length - 2))
                                                {
                                                    // Unit; Size-tag; Valid-Date
                                                    string sChUnt = dataInfo.Substring(0, dp2);
                                                    if ((sChUnt == "Centimeter") || (sChUnt == "centimeter"))
                                                        anvBrostData[noOfChestData].units = DistUnits.Centimeter;
                                                    else if ((sChUnt == "Decimeter") || (sChUnt == "decimeter"))
                                                        anvBrostData[noOfChestData].units = DistUnits.Decimeter;
                                                    else if ((sChUnt == "Inch") || (sChUnt == "inch") || (sChUnt == "Inches") || (sChUnt == "inches"))
                                                        anvBrostData[noOfChestData].units = DistUnits.Inch;
                                                    else if ((sChUnt == "Feet") || (sChUnt == "feet"))
                                                        anvBrostData[noOfChestData].units = DistUnits.Feet;
                                                    else
                                                        anvBrostData[noOfChestData].units = DistUnits.UndefDistUnit;
                                                    dataInfo = dataInfo.Substring(dp2 + 2, dataInfo.Length - dp2 - 2);
                                                    dp3 = dataInfo.IndexOf(";");
                                                    if ((dp3 > 0) && (dp3 < dataInfo.Length - 2))
                                                    {
                                                        // Size-tag; Valid-Date
                                                        string chSzTag = dataInfo.Substring(0, dp3);
                                                        if ((chSzTag == "AA") || (chSzTag == "aa"))
                                                            anvBrostData[noOfChestData].sizeType = BreastSizeType.AA;
                                                        else if ((chSzTag == "A") || (chSzTag == "a"))
                                                            anvBrostData[noOfChestData].sizeType = BreastSizeType.A;
                                                        else if ((chSzTag == "B") || (chSzTag == "b"))
                                                            anvBrostData[noOfChestData].sizeType = BreastSizeType.B;
                                                        else if ((chSzTag == "Bulgy") || (chSzTag == "bulgy"))
                                                            anvBrostData[noOfChestData].sizeType = BreastSizeType.Bulgy;
                                                        else if ((chSzTag == "C") || (chSzTag == "c"))
                                                            anvBrostData[noOfChestData].sizeType = BreastSizeType.C;
                                                        else if ((chSzTag == "D") || (chSzTag == "d"))
                                                            anvBrostData[noOfChestData].sizeType = BreastSizeType.D;
                                                        else if ((chSzTag == "E") || (chSzTag == "e"))
                                                            anvBrostData[noOfChestData].sizeType = BreastSizeType.E;
                                                        else if ((chSzTag == "F") || (chSzTag == "f"))
                                                            anvBrostData[noOfChestData].sizeType = BreastSizeType.F;
                                                        else if ((chSzTag == "Flat") || (chSzTag == "flat"))
                                                            anvBrostData[noOfChestData].sizeType = BreastSizeType.Flat;
                                                        else if ((chSzTag == "Normal") || (chSzTag == "normal") || (chSzTag == "Medium") || (chSzTag == "medium"))
                                                            anvBrostData[noOfChestData].sizeType = BreastSizeType.Medium;
                                                        else if ((chSzTag == "OverSize") || (chSzTag == "Oversize") || (chSzTag == "oversize"))
                                                            anvBrostData[noOfChestData].sizeType = BreastSizeType.Oversize;
                                                        else
                                                            anvBrostData[noOfChestData].sizeType = BreastSizeType.UndefBreastSize;
                                                        dataInfo = dataInfo.Substring(dp3 + 2, dataInfo.Length - dp3 - 2);
                                                        dp4 = dataInfo.IndexOf("-");
                                                        if ((dp4 > 0) && (dp4 < dataInfo.Length - 1))
                                                        {
                                                            // Valid-Date [YYYY-MM-DD|YY-MM-DD]
                                                            string svay = dataInfo.Substring(0, dp4);
                                                            int resval;
                                                            if (int.TryParse(svay, out resval))
                                                            {
                                                                anvBrostData[noOfChestData].validDate.year = resval;
                                                                noOfDataSet++;
                                                            }
                                                            dataInfo = dataInfo.Substring(dp4 + 1, dataInfo.Length - dp4 - 1);
                                                            dp5 = dataInfo.IndexOf("-");
                                                            if ((dp5 > 0) && (dp5 < dataInfo.Length - 1))
                                                            {
                                                                string svam = dataInfo.Substring(0, dp5);
                                                                if (int.TryParse(svam, out resval))
                                                                {
                                                                    anvBrostData[noOfChestData].validDate.month = resval;
                                                                    noOfDataSet++;
                                                                }
                                                                dataInfo = dataInfo.Substring(dp5 + 1, dataInfo.Length - dp5 - 1);
                                                                if (int.TryParse(dataInfo, out resval))
                                                                {
                                                                    anvBrostData[noOfChestData].validDate.day = resval;
                                                                    noOfDataSet++;
                                                                }
                                                            }
                                                            else
                                                            {
                                                                if (int.TryParse(dataInfo, out resval))
                                                                {
                                                                    anvBrostData[noOfChestData].validDate.month = resval;
                                                                    noOfDataSet++;
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            int resval;
                                                            if (int.TryParse(dataInfo, out resval))
                                                            {
                                                                anvBrostData[noOfChestData].validDate.year = resval;
                                                                noOfDataSet++;
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        noOfChestData++;
                                    }
                                } break;
                            case "FaceData":
                                {
                                    // FaceData : Eye-width; Cheekbone-Width; Chin-Width; Mouth-Width; Height; Unit; Valid-Date
                                    if (noOfFaceData < maxNoOfFaceData)
                                    {
                                        dp0 = dataInfo.IndexOf(";");
                                        if ((dp0 > 0) && (dp0 < dataInfo.Length - 2))
                                        {
                                            // Eye-width; Cheekbone-Width; Chin-Width; Mouth-Width; Height; Unit; Valid-Date
                                            string sEyeWdt = dataInfo.Substring(0, dp0);
                                            float fVal0;
                                            if (float.TryParse(sEyeWdt, NumberStyles.Any, CultureInfo.InvariantCulture, out fVal0))
                                            {
                                                anvAnsiktsData[noOfFaceData].eyeWidth = fVal0;
                                                noOfDataSet++;
                                            }
                                            dataInfo = dataInfo.Substring(dp0 + 2, dataInfo.Length - dp0 - 2);
                                            dp1 = dataInfo.IndexOf(";");
                                            if ((dp1 > 0) && (dp1 < dataInfo.Length - 2))
                                            {
                                                // Cheekbone-Width; Chin-Width; Mouth-Width; Height; Unit; Valid-Date
                                                string scbw = dataInfo.Substring(0, dp1);
                                                float fVal1;
                                                if (float.TryParse(scbw, NumberStyles.Any, CultureInfo.InvariantCulture, out fVal1))
                                                {
                                                    anvAnsiktsData[noOfFaceData].cheekboneWidth = fVal1;
                                                    noOfDataSet++;
                                                }
                                                dataInfo = dataInfo.Substring(dp1 + 2, dataInfo.Length - dp1 - 2);
                                                dp2 = dataInfo.IndexOf(";");
                                                if ((dp2 > 0) && (dp2 < dataInfo.Length - 2))
                                                {
                                                    // Chin-Width; Mouth-Width; Height; Unit; Valid-Date
                                                    string scw = dataInfo.Substring(0, dp2);
                                                    float fVal2;
                                                    if (float.TryParse(scw, NumberStyles.Any, CultureInfo.InvariantCulture, out fVal2))
                                                    {
                                                        anvAnsiktsData[noOfFaceData].chinWidth = fVal2;
                                                        noOfDataSet++;
                                                    }
                                                    dataInfo = dataInfo.Substring(dp2 + 2, dataInfo.Length - dp2 - 2);
                                                    dp3 = dataInfo.IndexOf(";");
                                                    if ((dp3 > 0) && (dp3 < dataInfo.Length - 2))
                                                    {
                                                        // Mouth-Width; Height; Unit; Valid-Date
                                                        string smw = dataInfo.Substring(0, dp3);
                                                        float fVal3;
                                                        if (float.TryParse(smw, NumberStyles.Any, CultureInfo.InvariantCulture, out fVal3))
                                                        {
                                                            anvAnsiktsData[noOfFaceData].mouthWidth = fVal3;
                                                            noOfDataSet++;
                                                        }
                                                        dataInfo = dataInfo.Substring(dp3 + 2, dataInfo.Length - dp3 - 2);
                                                        dp4 = dataInfo.IndexOf(";");
                                                        if ((dp4 > 0) && (dp4 < dataInfo.Length - 2))
                                                        {
                                                            // Height; Unit; Valid-Date
                                                            string sh = dataInfo.Substring(0, dp4);
                                                            float fVal4;
                                                            if (float.TryParse(sh, NumberStyles.Any, CultureInfo.InvariantCulture, out fVal4))
                                                            {
                                                                anvAnsiktsData[noOfFaceData].height = fVal4;
                                                                noOfDataSet++;
                                                            }
                                                            dataInfo = dataInfo.Substring(dp4 + 2, dataInfo.Length - dp4 - 2);
                                                            dp5 = dataInfo.IndexOf(";");
                                                            if ((dp5 > 0) && (dp5 < dataInfo.Length - 2))
                                                            {
                                                                // Unit; Valid-Date
                                                                string sunt = dataInfo.Substring(0, dp5);
                                                                if ((sunt == "Poits") || (sunt == "points"))
                                                                    anvAnsiktsData[noOfFaceData].units = DistUnits.Points;
                                                                else if ((sunt == "Millimeter") || (sunt == "millimeter"))
                                                                    anvAnsiktsData[noOfFaceData].units = DistUnits.Millimeter;
                                                                else if ((sunt == "Centimeter") || (sunt == "centimeter"))
                                                                    anvAnsiktsData[noOfFaceData].units = DistUnits.Centimeter;
                                                                else if ((sunt == "Decimeter") || (sunt == "decimeter"))
                                                                    anvAnsiktsData[noOfFaceData].units = DistUnits.Decimeter;
                                                                else if ((sunt == "Meter") || (sunt == "meter"))
                                                                    anvAnsiktsData[noOfFaceData].units = DistUnits.Meter;
                                                                else if ((sunt == "Inch") || (sunt == "inch") || (sunt == "Inches") || (sunt == "inches"))
                                                                    anvAnsiktsData[noOfFaceData].units = DistUnits.Inch;
                                                                else if ((sunt == "Feet") || (sunt == "feet") || (sunt == "Foot") || (sunt == "foot"))
                                                                    anvAnsiktsData[noOfFaceData].units = DistUnits.Feet;
                                                                else
                                                                    anvAnsiktsData[noOfFaceData].units = DistUnits.UndefDistUnit;
                                                                dataInfo = dataInfo.Substring(dp5 + 2, dataInfo.Length - dp5 - 2);
                                                                dp6 = dataInfo.IndexOf("-");
                                                                int resval;
                                                                if ((dp6 > 0) && (dp6 < dataInfo.Length - 1))
                                                                {
                                                                    // Valid-Date [YYYY-MM-DD|YY-MM-DD]
                                                                    string sad = dataInfo.Substring(0, dp6);
                                                                    if (int.TryParse(sad, out resval))
                                                                    {
                                                                        anvAnsiktsData[noOfFaceData].validDate.year = resval;
                                                                        noOfDataSet++;
                                                                    }
                                                                    dataInfo = dataInfo.Substring(dp6 + 1, dataInfo.Length - dp6 - 1);
                                                                    dp7 = dataInfo.IndexOf("-");
                                                                    if ((dp7 > 0) && (dp7 < dataInfo.Length - 1))
                                                                    {
                                                                        // ValidDate [MM-DD]
                                                                        string smon = dataInfo.Substring(0, dp7);
                                                                        if (int.TryParse(smon, out resval))
                                                                        {
                                                                            anvAnsiktsData[noOfFaceData].validDate.month = resval;
                                                                            noOfDataSet++;
                                                                        }
                                                                        string sday = dataInfo.Substring(dp7 + 1, dataInfo.Length - dp7 - 1);
                                                                        if (int.TryParse(sday, out resval))
                                                                        {
                                                                            anvAnsiktsData[noOfFaceData].validDate.day = resval;
                                                                            noOfDataSet++;
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        if (int.TryParse(dataInfo, out resval))
                                                                        {
                                                                            anvAnsiktsData[noOfFaceData].validDate.month = resval;
                                                                            noOfDataSet++;
                                                                        }
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    // ValidDate [YYYY|YY]
                                                                    if (int.TryParse(dataInfo, out resval))
                                                                    {
                                                                        anvAnsiktsData[noOfFaceData].validDate.year = resval;
                                                                        noOfDataSet++;
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        noOfFaceData++;
                                    }
                                } break;
                            case "Residnce":
                                {
                                    // Residnce : Streetname; number; additive; City; Area; Zipcode; Country; Purchase; In-Date; Out-Date; Sale; Currency
                                    if (noOfResidences < maxNoOfResidences)
                                    {
                                        dp0 = dataInfo.IndexOf(";");
                                        if ((dp0 > 0) && (dp0 < dataInfo.Length - 2))
                                        {
                                            // Streetname; number; additive; City; Area; Zipcode; Country; Purchase; In-Date; Out-Date; Sale; Currency
                                            string ssnm = dataInfo.Substring(0, dp0);
                                            if ((ssnm != "Undefined") && (ssnm != "Unknown") && (ssnm != ""))
                                                anvBostader[noOfResidences].streetname = ssnm;
                                            dataInfo = dataInfo.Substring(dp0 + 2, dataInfo.Length - dp0 - 2);
                                            dp1 = dataInfo.IndexOf(";");
                                            int resdata;
                                            if ((dp1 > 0) && (dp1 < dataInfo.Length - 2))
                                            {
                                                // number; additive; City; Area; Zipcode; Country; Purchase; In-Date; Out-Date; Sale; Currency
                                                string snum = dataInfo.Substring(0, dp1);
                                                if (int.TryParse(snum, out resdata))
                                                {
                                                    anvBostader[noOfResidences].number = resdata;
                                                    noOfDataSet++;
                                                }
                                                dataInfo = dataInfo.Substring(dp1 + 2, dataInfo.Length - dp1 - 2);
                                                dp2 = dataInfo.IndexOf(";");
                                                if ((dp2 > 0) && (dp2 < dataInfo.Length - 1))
                                                {
                                                    // additive; City; Area; Zipcode; Country; Purchase; In-Date; Out-Date; Sale; Currency
                                                    string snoad = dataInfo.Substring(0, dp2);
                                                    if ((snoad != "Undefined") && (snoad != "Unknown") && (snoad != ""))
                                                        anvBostader[noOfResidences].additive = snoad;
                                                    dataInfo = dataInfo.Substring(dp2 + 2, dataInfo.Length - dp2 - 2);
                                                    dp3 = dataInfo.IndexOf(";");
                                                    if ((dp3 > 0) && (dp3 < dataInfo.Length - 1))
                                                    {
                                                        // City; Area; Zipcode; Country; Purchase; In-Date; Out-Date; Sale; Currency
                                                        string scty = dataInfo.Substring(0, dp3);
                                                        if ((scty != "Undefined") && (scty != "Unknown") && (scty != ""))
                                                            anvBostader[noOfResidences].city = scty;
                                                        dataInfo = dataInfo.Substring(dp3 + 2, dataInfo.Length - dp3 - 2);
                                                        dp4 = dataInfo.IndexOf(";");
                                                        if ((dp4 > 0) && (dp4 < dataInfo.Length - 1))
                                                        {
                                                            // Area; Zipcode; Country; Purchase; In-Date; Out-Date; Sale; Currency
                                                            string sarea = dataInfo.Substring(0, dp4);
                                                            if ((sarea != "Undefined") && (sarea != "Unknown") && (sarea != ""))
                                                                anvBostader[noOfResidences].area = sarea;
                                                            dataInfo = dataInfo.Substring(dp4 + 2, dataInfo.Length - dp4 - 2);
                                                            dp5 = dataInfo.IndexOf(";");
                                                            if ((dp5 > 0) && (dp5 < dataInfo.Length - 1))
                                                            {
                                                                // Zipcode; Country; Purchase; In-Date; Out-Date; Sale; Currency
                                                                string szc = dataInfo.Substring(0, dp5);
                                                                if (int.TryParse(szc, out resdata))
                                                                {
                                                                    anvBostader[noOfResidences].zipcode = resdata;
                                                                    noOfDataSet++;
                                                                }
                                                                dataInfo = dataInfo.Substring(dp5 + 2, dataInfo.Length - dp5 - 2);
                                                                dp6 = dataInfo.IndexOf(";");
                                                                if ((dp6 > 0) && (dp6 < dataInfo.Length - 1))
                                                                {
                                                                    // Country; Purchase; In-Date; Out-Date; Sale; Currency
                                                                    string sctry = dataInfo.Substring(0, dp6);
                                                                    if ((sctry != "Undefined") && (sctry != "Unknown") && (sctry != ""))
                                                                        anvBostader[noOfResidences].country = sctry;
                                                                    dataInfo = dataInfo.Substring(dp6 + 2, dataInfo.Length - dp6 - 2);
                                                                    dp7 = dataInfo.IndexOf(";");
                                                                    if ((dp7 > 0) && (dp7 < dataInfo.Length - 1))
                                                                    {
                                                                        // Purchase; In-Date; Out-Date; Sale; Currency
                                                                        string spch = dataInfo.Substring(0, dp7);
                                                                        if (int.TryParse(spch, out resdata))
                                                                        {
                                                                            anvBostader[noOfResidences].boughtValue = resdata;
                                                                            noOfDataSet++;
                                                                        }
                                                                        dataInfo = dataInfo.Substring(dp7 + 2, dataInfo.Length - dp7 - 2);
                                                                        dp8 = dataInfo.IndexOf(";");
                                                                        if ((dp8 > 0) && (dp8 < dataInfo.Length - 1))
                                                                        {
                                                                            // In-Date; Out-Date; Sale; Currency
                                                                            string sindat = dataInfo.Substring(0, dp8);
                                                                            int dp81 = sindat.IndexOf("-");
                                                                            int resval;
                                                                            if ((dp81 > 0) && (dp81 < sindat.Length - 1))
                                                                            {
                                                                                // InDate [YYYY-MM-DD|YY-MM-DD]
                                                                                string sinyr = sindat.Substring(0, dp81);
                                                                                if (int.TryParse(sinyr, out resval))
                                                                                {
                                                                                    anvBostader[noOfResidences].boughtDate.year = resval;
                                                                                    noOfDataSet++;
                                                                                }
                                                                                sindat = sindat.Substring(dp81 + 1, sindat.Length - dp81 - 1);
                                                                                int dp82 = sindat.IndexOf("-");
                                                                                if ((dp82 > 0) && (dp82 < sindat.Length - 1))
                                                                                {
                                                                                    // InDate [MM-DD|MM-DD]
                                                                                    string sinmon = sindat.Substring(0, dp82);
                                                                                    if (int.TryParse(sinmon, out resval))
                                                                                    {
                                                                                        anvBostader[noOfResidences].boughtDate.month = resval;
                                                                                        noOfDataSet++;
                                                                                    }
                                                                                    sindat = sindat.Substring(dp82 + 1, sindat.Length - dp82 - 1);
                                                                                    if (int.TryParse(sindat, out resval))
                                                                                    {
                                                                                        anvBostader[noOfResidences].boughtDate.day = resval;
                                                                                        noOfDataSet++;
                                                                                    }
                                                                                }
                                                                                else
                                                                                {
                                                                                    if (int.TryParse(sindat, out resval))
                                                                                    {
                                                                                        anvBostader[noOfResidences].boughtDate.month = resval;
                                                                                        noOfDataSet++;
                                                                                    }
                                                                                }
                                                                            }
                                                                            else
                                                                            {
                                                                                // InDate [YYYY|YY]
                                                                                if (int.TryParse(sindat, out resval))
                                                                                {
                                                                                    anvBostader[noOfResidences].boughtDate.year = resval;
                                                                                    noOfDataSet++;
                                                                                }
                                                                            }
                                                                            dataInfo = dataInfo.Substring(dp8 + 2, dataInfo.Length - dp8 - 2);
                                                                            dp9 = dataInfo.IndexOf(";");
                                                                            if ((dp9 > 0) && (dp9 < dataInfo.Length - 1))
                                                                            {
                                                                                // Out-Date; Sale; Currency
                                                                                string soutdat = dataInfo.Substring(0, dp9);
                                                                                int dp91 = soutdat.IndexOf("-");
                                                                                if ((dp91 > 0) && (dp91 < soutdat.Length - 1))
                                                                                {
                                                                                    // OutDate [YYYY-MM-DD|YY-MM-DD]
                                                                                    string soutyr = soutdat.Substring(0, dp91);
                                                                                    if (int.TryParse(soutyr, out resval))
                                                                                    {
                                                                                        anvBostader[noOfResidences].salesDate.year = resval;
                                                                                        noOfDataSet++;
                                                                                    }
                                                                                    soutdat = soutdat.Substring(dp91 + 1, soutdat.Length - dp91 - 1);
                                                                                    int dp92 = soutdat.IndexOf("-");
                                                                                    if ((dp92 > 0) && (dp92 < soutdat.Length - 1))
                                                                                    {
                                                                                        // OutDate [MM-DD]
                                                                                        string soutmon = soutdat.Substring(0, dp92);
                                                                                        if (int.TryParse(soutmon, out resval))
                                                                                        {
                                                                                            anvBostader[noOfResidences].salesDate.month = resval;
                                                                                            noOfDataSet++;
                                                                                        }
                                                                                        soutdat = soutdat.Substring(dp92 + 1, soutdat.Length - dp92 - 1);
                                                                                        if (int.TryParse(soutdat, out resval))
                                                                                        {
                                                                                            anvBostader[noOfResidences].salesDate.day = resval;
                                                                                            noOfDataSet++;
                                                                                        }
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        // OutDate [MM]
                                                                                        if (int.TryParse(soutdat, out resval))
                                                                                        {
                                                                                            anvBostader[noOfResidences].salesDate.month = resval;
                                                                                            noOfDataSet++;
                                                                                        }
                                                                                    }
                                                                                }
                                                                                else
                                                                                {
                                                                                    // OutDate [YYYY|YY]
                                                                                    if (int.TryParse(soutdat, out resval))
                                                                                    {
                                                                                        anvBostader[noOfResidences].salesDate.year = resval;
                                                                                        noOfDataSet++;
                                                                                    }
                                                                                }
                                                                                dataInfo = dataInfo.Substring(dp9 + 2, dataInfo.Length - dp9 - 2);
                                                                                dp10 = dataInfo.IndexOf(";");
                                                                                if ((dp10 > 0) && (dp10 < dataInfo.Length - 1))
                                                                                {
                                                                                    // Sale; Currency
                                                                                    string scurr = dataInfo.Substring(0, dp10);
                                                                                    float fVal;
                                                                                    if (float.TryParse(scurr, NumberStyles.Any, CultureInfo.InvariantCulture, out fVal))
                                                                                    {
                                                                                        anvBostader[noOfResidences].salesValue = fVal;
                                                                                        noOfDataSet++;
                                                                                    }
                                                                                    dataInfo = dataInfo.Substring(dp10 + 2, dataInfo.Length - dp10 - 2);
                                                                                    // Currency
                                                                                    bool foundCategory = false;
                                                                                    for (int i = 0; i < maxNoOfCurrencyCategories; i++)
                                                                                    {
                                                                                        if (dataInfo == currencyCategory[i].tag)
                                                                                        {
                                                                                            foundCategory = true;
                                                                                            anvBostader[noOfResidences].usedCurrency = currencyCategory[i];
                                                                                        }
                                                                                    }
                                                                                    if (!(foundCategory))
                                                                                    {
                                                                                        currencyCategory[noOfCurrencyCategories].tag = dataInfo;
                                                                                        currencyCategory[noOfCurrencyCategories].description = "";
                                                                                        currencyCategory[noOfCurrencyCategories].level = 0;
                                                                                        currencyCategory[noOfCurrencyCategories].value = 0.0;
                                                                                        anvBostader[noOfResidences].usedCurrency = currencyCategory[noOfCurrencyCategories];
                                                                                        noOfCurrencyCategories++;
                                                                                    }
                                                                                    noOfDataSet++;
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        noOfResidences++;
                                    }
                                } break;
                            case "HairColr":
                                {
                                    // HairColr : Color-tag; Texture-tag; Length-tag; Valid-Date
                                    if (noOfHairData < maxNoOfHairData)
                                    {
                                        // Color-tag; Texture-tag; Length-tag; Valid-Date
                                        dp0 = dataInfo.IndexOf(";");
                                        if ((dp0 > 0) && (dp0 < dataInfo.Length - 2))
                                        {
                                            // Color-tag; Texture-tag; Length-tag; Valid-Date
                                            bool foundCategory = false;
                                            string clrtg = dataInfo.Substring(0, dp0);
                                            for (int i = 0; i < noOfHairColorCategories; i++)
                                            {
                                                if (clrtg == hairColorCategory[i].tag)
                                                {
                                                    foundCategory = true;
                                                    anvHaar[noOfHairData].hairColor = hairColorCategory[i];
                                                }
                                            }
                                            if ((!(foundCategory)) && (noOfHairColorCategories < maxNoOfHairColorCategories))
                                            {
                                                anvHaar[noOfHairData].hairColor.tag = clrtg;
                                                anvHaar[noOfHairData].hairColor.description = "";
                                                anvHaar[noOfHairData].hairColor.level = 0;
                                                addHairColorCategory(clrtg, "", "Undefined");
                                            }
                                            dataInfo = dataInfo.Substring(dp0 + 2, dataInfo.Length - dp0 - 2);
                                            dp1 = dataInfo.IndexOf(";");
                                            string hrtxt = dataInfo.Substring(0, dp1);
                                            // Texture-tag; Length-tag; Valid-Date
                                            if ((hrtxt == "Straight") || (hrtxt == "straight"))
                                                anvHaar[noOfHairData].textureTag = HairTextureType.Straight;
                                            else if ((hrtxt == "Wavy") || (hrtxt == "wavy"))
                                                anvHaar[noOfHairData].textureTag = HairTextureType.Wavy;
                                            else if ((hrtxt == "Curly") || (hrtxt == "curly"))
                                                anvHaar[noOfHairData].textureTag = HairTextureType.Curly;
                                            else if ((hrtxt == "Coily") || (hrtxt == "coily"))
                                                anvHaar[noOfHairData].textureTag = HairTextureType.Coily;
                                            else
                                                anvHaar[noOfHairData].textureTag = HairTextureType.UndefHairTexture;
                                            dataInfo = dataInfo.Substring(dp1 + 2, dataInfo.Length - dp1 - 2);
                                            dp2 = dataInfo.IndexOf(";");
                                            string hrlnt = dataInfo.Substring(0, dp2);
                                            // Length-tag; Valid-Date
                                            if ((hrlnt == "Short") || (hrlnt == "short"))
                                                anvHaar[noOfHairData].lengthTag = HairLengthType.Short;
                                            else if ((hrlnt == "Neck") || (hrlnt == "neck"))
                                                anvHaar[noOfHairData].lengthTag = HairLengthType.Neck;
                                            else if ((hrlnt == "Shoulder") || (hrlnt == "shoulder"))
                                                anvHaar[noOfHairData].lengthTag = HairLengthType.Shoulder;
                                            else if ((hrlnt == "Mid Back") || (hrlnt == "mid back"))
                                                anvHaar[noOfHairData].lengthTag = HairLengthType.MidBack;
                                            else if ((hrlnt == "Waist") || (hrlnt == "waist"))
                                                anvHaar[noOfHairData].lengthTag = HairLengthType.Waist;
                                            else if ((hrlnt == "Ass") || (hrlnt == "ass"))
                                                anvHaar[noOfHairData].lengthTag = HairLengthType.Ass;
                                            else if ((hrlnt == "Long") || (hrlnt == "long"))
                                                anvHaar[noOfHairData].lengthTag = HairLengthType.Long;
                                            else
                                                anvHaar[noOfHairData].lengthTag = HairLengthType.UndefHairLength;
                                            dataInfo = dataInfo.Substring(dp2 + 2, dataInfo.Length - dp2 - 2);
                                            // Valid-Date
                                            if ((dataInfo != "Undefined") && (dataInfo != "Unknown"))
                                            {
                                                dp3 = dataInfo.IndexOf("-");
                                                if ((dp3 > 0) && (dp3 < dataInfo.Length - 1))
                                                {
                                                    if (dataInfo.Length > 9)
                                                    {
                                                        // Format YYYY-MM-DD
                                                        string yrval = dataInfo.Substring(0, 4);
                                                        string mtval = dataInfo.Substring(5, 2);
                                                        string dyval = dataInfo.Substring(8, 2);
                                                        anvHaar[noOfHairData].validDate.year = int.Parse(yrval);
                                                        anvHaar[noOfHairData].validDate.month = int.Parse(mtval);
                                                        anvHaar[noOfHairData].validDate.day = int.Parse(dyval);
                                                    }
                                                    else if (dataInfo.Length > 7)
                                                    {
                                                        // Format YY-MM-DD
                                                        string yrval = dataInfo.Substring(0, 2);
                                                        string mtval = dataInfo.Substring(3, 2);
                                                        string dyval = dataInfo.Substring(6, 2);
                                                        anvHaar[noOfHairData].validDate.year = int.Parse(yrval);
                                                        anvHaar[noOfHairData].validDate.month = int.Parse(mtval);
                                                        anvHaar[noOfHairData].validDate.day = int.Parse(dyval);
                                                    }
                                                    else
                                                    {
                                                        // Format YY-MM
                                                        string yrval = dataInfo.Substring(0, 2);
                                                        string mtval = dataInfo.Substring(3, 2);
                                                        anvHaar[noOfHairData].validDate.year = int.Parse(yrval);
                                                        anvHaar[noOfHairData].validDate.month = int.Parse(mtval);
                                                    }
                                                }
                                                else
                                                {
                                                    if (dataInfo.Length > 7)
                                                    {
                                                        // Format YYYYMMDD
                                                        string yrval = dataInfo.Substring(0, 4);
                                                        string mtval = dataInfo.Substring(4, 2);
                                                        string dyval = dataInfo.Substring(6, 2);
                                                        anvHaar[noOfHairData].validDate.year = int.Parse(yrval);
                                                        anvHaar[noOfHairData].validDate.month = int.Parse(mtval);
                                                        anvHaar[noOfHairData].validDate.day = int.Parse(dyval);
                                                    }
                                                    else if (dataInfo.Length > 5)
                                                    {
                                                        // Format YYMMDD
                                                        string yrval = dataInfo.Substring(0, 2);
                                                        string mtval = dataInfo.Substring(2, 2);
                                                        string dyval = dataInfo.Substring(4, 2);
                                                        anvHaar[noOfHairData].validDate.year = int.Parse(yrval);
                                                        anvHaar[noOfHairData].validDate.month = int.Parse(mtval);
                                                        anvHaar[noOfHairData].validDate.day = int.Parse(dyval);
                                                    }
                                                    else
                                                    {
                                                        // Format YYYY
                                                        string yrval = dataInfo.Substring(0, 4);
                                                        anvHaar[noOfHairData].validDate.year = int.Parse(yrval);
                                                    }
                                                }
                                            }
                                        }
                                        noOfHairData++;
                                    }
                                } break;
                            case "EyeColor":
                                {
                                    // EyeColor : Color-tag; Form-tag; Valid-Date
                                    if (noOfEyeColorData < maxNoOfEyeColorData)
                                    {
                                        dp0 = dataInfo.IndexOf(";");
                                        if ((dp0 > 0) && (dp0 < dataInfo.Length))
                                        {
                                            string colorTag = dataInfo.Substring(0, dp0);
                                            if ((colorTag != "Undefined") && (colorTag != "Unknown") && (colorTag != "undefined") && (colorTag != "unknown"))
                                                anvEyes[noOfEyeColorData].colorTag = colorTag;
                                            dataInfo = dataInfo.Substring(dp0 + 2, dataInfo.Length - dp0 - 2);
                                            dp1 = dataInfo.IndexOf(";");
                                            if ((dp1 > 0) && (dp1 < dataInfo.Length))
                                            {
                                                string formTag = dataInfo.Substring(0, dp1);
                                                if ((formTag != "Undefined") && (formTag != "Unknown") && (formTag != "undefined") && (formTag != "unknown"))
                                                    anvEyes[noOfEyeColorData].formTag = formTag;
                                                dataInfo = dataInfo.Substring(dp1 + 2, dataInfo.Length - dp1 - 2);
                                                dp2 = dataInfo.IndexOf("-");
                                                if ((dp2 > 0) && (dp2 < dataInfo.Length))
                                                {
                                                    // We have YY[YY]-MM[-DD]
                                                    if (dataInfo.Length > 9)
                                                    {
                                                        // Format YYYY-MM-DD
                                                        string yrval = dataInfo.Substring(0, 4);
                                                        string mtval = dataInfo.Substring(5, 2);
                                                        string dyval = dataInfo.Substring(8, 2);
                                                        anvEyes[noOfEyeColorData].validDate.year = int.Parse(yrval);
                                                        anvEyes[noOfEyeColorData].validDate.month = int.Parse(mtval);
                                                        anvEyes[noOfEyeColorData].validDate.day = int.Parse(dyval);
                                                    }
                                                    else if (dataInfo.Length > 7)
                                                    {
                                                        // Format YY-MM-DD
                                                        string yrval = dataInfo.Substring(0, 2);
                                                        string mtval = dataInfo.Substring(3, 2);
                                                        string dyval = dataInfo.Substring(6, 2);
                                                        anvEyes[noOfEyeColorData].validDate.year = int.Parse(yrval) + 1900;
                                                        anvEyes[noOfEyeColorData].validDate.month = int.Parse(mtval);
                                                        anvEyes[noOfEyeColorData].validDate.day = int.Parse(dyval);
                                                    }
                                                    else if (dataInfo.Length > 4)
                                                    {
                                                        // Format YY-MM
                                                        string yrval = dataInfo.Substring(0, 2);
                                                        string mtval = dataInfo.Substring(3, 2);
                                                        anvEyes[noOfEyeColorData].validDate.year = int.Parse(yrval) + 1900;
                                                        anvEyes[noOfEyeColorData].validDate.month = int.Parse(mtval);
                                                    }
                                                    else
                                                    {
                                                        // Format YYYY
                                                        anvEyes[noOfEyeColorData].validDate.year = int.Parse(dataInfo);
                                                    }
                                                }
                                                else
                                                {
                                                    if (dataInfo.Length > 7)
                                                    {
                                                        // Format YYYYMMDD
                                                        string yrval = dataInfo.Substring(0, 4);
                                                        string mtval = dataInfo.Substring(4, 2);
                                                        string dyval = dataInfo.Substring(6, 2);
                                                        anvEyes[noOfEyeColorData].validDate.year = int.Parse(yrval);
                                                        anvEyes[noOfEyeColorData].validDate.month = int.Parse(mtval);
                                                        anvEyes[noOfEyeColorData].validDate.day = int.Parse(dyval);
                                                    }
                                                    else if (dataInfo.Length > 5)
                                                    {
                                                        // Format YYMMDD
                                                        string yrval = dataInfo.Substring(0, 2);
                                                        string mtval = dataInfo.Substring(2, 2);
                                                        string dyval = dataInfo.Substring(4, 2);
                                                        anvEyes[noOfEyeColorData].validDate.year = int.Parse(yrval) + 1900;
                                                        anvEyes[noOfEyeColorData].validDate.month = int.Parse(mtval);
                                                        anvEyes[noOfEyeColorData].validDate.day = int.Parse(dyval);
                                                    }
                                                    else
                                                    {
                                                        // Format YYYY
                                                        anvEyes[noOfEyeColorData].validDate.year = int.Parse(dataInfo);
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            anvEyes[noOfEyeColorData].colorTag = dataInfo;
                                        }
                                        noOfEyeColorData++;
                                    }
                                } break;
                            case "MarkData":
                                {
                                    // MarkData : type-tag; placement; Motif; Valid-Date
                                    if (noOfMarkingData < maxNoOfMarkingData)
                                    {
                                        // type-tag; placement; Motif; Valid-Date
                                        dp0 = dataInfo.IndexOf(";");
                                        if ((dp0 > 0) && (dp0 < dataInfo.Length))
                                        {
                                            string mrktpe = dataInfo.Substring(0, dp0);
                                            if ((mrktpe == "Scar") || (mrktpe == "scar"))
                                                anvMarkningar[noOfMarkingData].markTag = MarkingType.Scar;
                                            else if ((mrktpe == "Freckles") || (mrktpe == "freckles"))
                                                anvMarkningar[noOfMarkingData].markTag = MarkingType.Freckles;
                                            else if ((mrktpe == "Birthmark") || (mrktpe == "birthmark"))
                                                anvMarkningar[noOfMarkingData].markTag = MarkingType.Birthmark;
                                            else if ((mrktpe == "Tattoo") || (mrktpe == "tattoo"))
                                                anvMarkningar[noOfMarkingData].markTag = MarkingType.Tattoo;
                                            else if ((mrktpe == "Piercing") || (mrktpe == "piercing"))
                                                anvMarkningar[noOfMarkingData].markTag = MarkingType.Piercing;
                                            else
                                                anvMarkningar[noOfMarkingData].markTag = MarkingType.UndefMarking;
                                            dataInfo = dataInfo.Substring(dp0 + 2, dataInfo.Length - dp0 - 2);
                                            dp1 = dataInfo.IndexOf(";");
                                            if ((dp1 > 0) && (dp1 < dataInfo.Length - 2))
                                            {
                                                string plcmt = dataInfo.Substring(0, dp1);
                                                // placement; Motif; Valid-Date
                                                if ((plcmt != "Undefined") && (plcmt != "Unknown"))
                                                    anvMarkningar[noOfMarkingData].placement = plcmt;
                                                dataInfo = dataInfo.Substring(dp1 + 2, dataInfo.Length - dp1 - 2);
                                                dp2 = dataInfo.IndexOf(";");
                                                if ((dp2 > 0) && (dp2 < dataInfo.Length - 2))
                                                {
                                                    string motf = dataInfo.Substring(0, dp2);
                                                    // Motif; Valid-Date
                                                    if ((motf != "Undefined") && (motf != "Unknown"))
                                                        anvMarkningar[noOfMarkingData].motif = motf;
                                                    dataInfo = dataInfo.Substring(dp2 + 2, dataInfo.Length - dp2 - 2);
                                                    // Valid-Date
                                                    dp3 = dataInfo.IndexOf("-");
                                                    if ((dp3 > 0) && (dp3 < dataInfo.Length - 1))
                                                    {
                                                        if (dataInfo.Length > 9)
                                                        {
                                                            // Format YYYY-MM-DD
                                                            string yrval = dataInfo.Substring(0, 4);
                                                            string mtval = dataInfo.Substring(5, 2);
                                                            string dyval = dataInfo.Substring(8, 2);
                                                            anvMarkningar[noOfMarkingData].validDate.year = int.Parse(yrval);
                                                            anvMarkningar[noOfMarkingData].validDate.month = int.Parse(mtval);
                                                            anvMarkningar[noOfMarkingData].validDate.day = int.Parse(dyval);
                                                        }
                                                        else if (dataInfo.Length > 7)
                                                        {
                                                            // Format YY-MM-DD
                                                            string yrval = dataInfo.Substring(0, 2);
                                                            string mtval = dataInfo.Substring(3, 2);
                                                            string dyval = dataInfo.Substring(6, 2);
                                                            anvMarkningar[noOfMarkingData].validDate.year = int.Parse(yrval);
                                                            anvMarkningar[noOfMarkingData].validDate.month = int.Parse(mtval);
                                                            anvMarkningar[noOfMarkingData].validDate.day = int.Parse(dyval);
                                                        }
                                                        else
                                                        {
                                                            // Format YY-MM
                                                            string yrval = dataInfo.Substring(0, 2);
                                                            string mtval = dataInfo.Substring(3, 2);
                                                            anvMarkningar[noOfMarkingData].validDate.year = int.Parse(yrval);
                                                            anvMarkningar[noOfMarkingData].validDate.month = int.Parse(mtval);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (dataInfo.Length > 7)
                                                        {
                                                            // Format YYYYMMDD
                                                            string yrval = dataInfo.Substring(0, 4);
                                                            string mtval = dataInfo.Substring(4, 2);
                                                            string dyval = dataInfo.Substring(6, 2);
                                                            anvMarkningar[noOfMarkingData].validDate.year = int.Parse(yrval);
                                                            anvMarkningar[noOfMarkingData].validDate.month = int.Parse(mtval);
                                                            anvMarkningar[noOfMarkingData].validDate.day = int.Parse(dyval);
                                                        }
                                                        else if (dataInfo.Length > 5)
                                                        {
                                                            // Format YYMMDD
                                                            string yrval = dataInfo.Substring(0, 2);
                                                            string mtval = dataInfo.Substring(2, 2);
                                                            string dyval = dataInfo.Substring(4, 2);
                                                            anvMarkningar[noOfMarkingData].validDate.year = int.Parse(yrval);
                                                            anvMarkningar[noOfMarkingData].validDate.month = int.Parse(mtval);
                                                            anvMarkningar[noOfMarkingData].validDate.day = int.Parse(dyval);
                                                        }
                                                        else
                                                        {
                                                            // Format YYYY
                                                            string yrval = dataInfo.Substring(0, 4);
                                                            anvMarkningar[noOfMarkingData].validDate.year = int.Parse(yrval);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        noOfMarkingData++;
                                    }
                                } break;
                            case "Ocupatn ":
                                {
                                    // Ocupatn  : Title; Company; Streetname; Number; State; Areaname; Zipcode; Country; Start-Date; End-Date
                                    if (noOfOccupationData < maxNoOfOccupationData)
                                    {
                                        dp0 = dataInfo.IndexOf(";");
                                        if ((dp0 > 0) && (dp0 < dataInfo.Length - 2))
                                        {
                                            // Title; Company; Streetname; Number; State; Areaname; Zipcode; Country; Start-Date; End-Date
                                            string ttl = dataInfo.Substring(0, dp0);
                                            if ((ttl != "Unknown") && (ttl != "Undefined"))
                                                anvAnstallningar[noOfOccupationData].title = ttl;
                                            dataInfo = dataInfo.Substring(dp0 + 2, dataInfo.Length - dp0 - 2);
                                            dp1 = dataInfo.IndexOf(";");
                                            if ((dp1 > 0) && (dp1 < dataInfo.Length - 2))
                                            {
                                                // Company; Streetname; Number; State; Areaname; Zipcode; Country; Start-Date; End-Date
                                                string cpny = dataInfo.Substring(0, dp1);
                                                if ((cpny != "Unknown") && (cpny != "Undefined"))
                                                    anvAnstallningar[noOfOccupationData].company = cpny;
                                                dataInfo = dataInfo.Substring(dp1 + 2, dataInfo.Length - dp1 - 2);
                                                dp2 = dataInfo.IndexOf(";");
                                                if ((dp2 > 0) && (dp2 < dataInfo.Length - 2))
                                                {
                                                    // Streetname; Number; State; Areaname; Zipcode; Country; Start-Date; End-Date
                                                    string strtnm = dataInfo.Substring(0, dp2);
                                                    if ((strtnm != "Undefined") && (strtnm != "Unknown"))
                                                        anvAnstallningar[noOfOccupationData].streetname = strtnm;
                                                    dataInfo = dataInfo.Substring(dp2 + 2, dataInfo.Length - dp2 - 2);
                                                    dp3 = dataInfo.IndexOf(";");
                                                    if ((dp3 > 0) && (dp3 < dataInfo.Length - 2))
                                                    {
                                                        // Number; State; Areaname; Zipcode; Country; Start-Date; End-Date
                                                        string sno = dataInfo.Substring(0, dp3);
                                                        if ((sno != "Undefined") && (sno != "Unknown"))
                                                            anvAnstallningar[noOfOccupationData].number = int.Parse(sno);
                                                        dataInfo = dataInfo.Substring(dp3 + 2, dataInfo.Length - dp3 - 2);
                                                        dp4 = dataInfo.IndexOf(";");
                                                        if ((dp4 > 0) && (dp4 < dataInfo.Length - 2))
                                                        {
                                                            // State; Areaname; Zipcode; Country; Start-Date; End-Date
                                                            string ste = dataInfo.Substring(0, dp4);
                                                            if ((ste != "Undefined") && (ste != "Unknown"))
                                                                anvAnstallningar[noOfOccupationData].statename = ste;
                                                            dataInfo = dataInfo.Substring(dp4 + 2, dataInfo.Length - dp4 - 2);
                                                            dp5 = dataInfo.IndexOf(";");
                                                            if ((dp5 > 0) && (dp5 < dataInfo.Length - 2))
                                                            {
                                                                // Areaname; Zipcode; Country; Start-Date; End-Date
                                                                string arn = dataInfo.Substring(0, dp5);
                                                                if ((arn != "Undefined") && (arn != "Unknown"))
                                                                    anvAnstallningar[noOfOccupationData].areaname = arn;
                                                                dataInfo = dataInfo.Substring(dp5 + 2, dataInfo.Length - dp5 - 2);
                                                                dp6 = dataInfo.IndexOf(";");
                                                                if ((dp6 > 0) && (dp6 < dataInfo.Length - 2))
                                                                {
                                                                    // Zipcode; Country; Start-Date; End-Date
                                                                    string zc = dataInfo.Substring(0, dp6);
                                                                    if ((zc != "Undefined") && (zc != "Unknown"))
                                                                        anvAnstallningar[noOfOccupationData].zipcode = int.Parse(zc);
                                                                    dataInfo = dataInfo.Substring(dp6 + 2, dataInfo.Length - dp6 - 2);
                                                                    dp7 = dataInfo.IndexOf(";");
                                                                    if ((dp7 > 0) && (dp7 < dataInfo.Length - 2))
                                                                    {
                                                                        // Country; Start-Date; End-Date
                                                                        string ctry = dataInfo.Substring(0, dp7);
                                                                        if ((ctry != "Undefined") && (ctry != "Unknown"))
                                                                            anvAnstallningar[noOfOccupationData].country = ctry;
                                                                        dataInfo = dataInfo.Substring(dp7 + 2, dataInfo.Length - dp7 - 2);
                                                                        dp8 = dataInfo.IndexOf(";");
                                                                        if ((dp8 > 0) && (dp8 < dataInfo.Length - 2))
                                                                        {
                                                                            // Start-Date; End-Date
                                                                            string stdt = dataInfo.Substring(0, dp8);
                                                                            int dp81 = stdt.IndexOf("-");
                                                                            if ((dp81 > 0) && (dp81 < stdt.Length - 1))
                                                                            {
                                                                                if (stdt.Length > 9)
                                                                                {
                                                                                    // Format YYYY-MM-DD
                                                                                    string yrval = stdt.Substring(0, 4);
                                                                                    string mtval = stdt.Substring(5, 2);
                                                                                    string dyval = stdt.Substring(8, 2);
                                                                                    anvAnstallningar[noOfOccupationData].started.year = int.Parse(yrval);
                                                                                    anvAnstallningar[noOfOccupationData].started.month = int.Parse(mtval);
                                                                                    anvAnstallningar[noOfOccupationData].started.day = int.Parse(dyval);
                                                                                }
                                                                                else if (stdt.Length > 7)
                                                                                {
                                                                                    // Format YY-MM-DD
                                                                                    string yrval = stdt.Substring(0, 2);
                                                                                    string mtval = stdt.Substring(3, 2);
                                                                                    string dyval = stdt.Substring(6, 2);
                                                                                    anvAnstallningar[noOfOccupationData].started.year = int.Parse(yrval);
                                                                                    anvAnstallningar[noOfOccupationData].started.month = int.Parse(mtval);
                                                                                    anvAnstallningar[noOfOccupationData].started.day = int.Parse(dyval);
                                                                                }
                                                                                else
                                                                                {
                                                                                    // Format YY-MM
                                                                                    string yrval = stdt.Substring(0, 2);
                                                                                    string mtval = stdt.Substring(3, 2);
                                                                                    anvAnstallningar[noOfOccupationData].started.year = int.Parse(yrval);
                                                                                    anvAnstallningar[noOfOccupationData].started.month = int.Parse(mtval);
                                                                                }
                                                                            }
                                                                            else
                                                                            {
                                                                                if (stdt.Length > 7)
                                                                                {
                                                                                    // Format YYYYMMDD
                                                                                    string yrval = stdt.Substring(0, 4);
                                                                                    string mtval = stdt.Substring(4, 2);
                                                                                    string dyval = stdt.Substring(6, 2);
                                                                                    anvAnstallningar[noOfOccupationData].started.year = int.Parse(yrval);
                                                                                    anvAnstallningar[noOfOccupationData].started.month = int.Parse(mtval);
                                                                                    anvAnstallningar[noOfOccupationData].started.day = int.Parse(dyval);
                                                                                }
                                                                                else if (stdt.Length > 5)
                                                                                {
                                                                                    // Format YYMMDD
                                                                                    string yrval = stdt.Substring(0, 2);
                                                                                    string mtval = stdt.Substring(2, 2);
                                                                                    string dyval = stdt.Substring(4, 2);
                                                                                    anvAnstallningar[noOfOccupationData].started.year = int.Parse(yrval);
                                                                                    anvAnstallningar[noOfOccupationData].started.month = int.Parse(mtval);
                                                                                    anvAnstallningar[noOfOccupationData].started.day = int.Parse(dyval);
                                                                                }
                                                                                else
                                                                                {
                                                                                    // Format YYYY
                                                                                    string yrval = stdt.Substring(0, 4);
                                                                                    anvAnstallningar[noOfOccupationData].started.year = int.Parse(yrval);
                                                                                }
                                                                            }
                                                                            dataInfo = dataInfo.Substring(dp8 + 2, dataInfo.Length - dp8 - 2);
                                                                            // End-Date
                                                                            dp9 = dataInfo.IndexOf("-");
                                                                            if ((dp9 > 0) && (dp9 < stdt.Length - 1))
                                                                            {
                                                                                if (dataInfo.Length > 9)
                                                                                {
                                                                                    // Format YYYY-MM-DD
                                                                                    string yrval = dataInfo.Substring(0, 4);
                                                                                    string mtval = dataInfo.Substring(5, 2);
                                                                                    string dyval = dataInfo.Substring(8, 2);
                                                                                    anvAnstallningar[noOfOccupationData].ended.year = int.Parse(yrval);
                                                                                    anvAnstallningar[noOfOccupationData].ended.month = int.Parse(mtval);
                                                                                    anvAnstallningar[noOfOccupationData].ended.day = int.Parse(dyval);
                                                                                }
                                                                                else if (dataInfo.Length > 7)
                                                                                {
                                                                                    // Format YY-MM-DD
                                                                                    string yrval = dataInfo.Substring(0, 2);
                                                                                    string mtval = dataInfo.Substring(3, 2);
                                                                                    string dyval = dataInfo.Substring(6, 2);
                                                                                    anvAnstallningar[noOfOccupationData].ended.year = int.Parse(yrval);
                                                                                    anvAnstallningar[noOfOccupationData].ended.month = int.Parse(mtval);
                                                                                    anvAnstallningar[noOfOccupationData].ended.day = int.Parse(dyval);
                                                                                }
                                                                                else
                                                                                {
                                                                                    // Format YY-MM
                                                                                    string yrval = dataInfo.Substring(0, 2);
                                                                                    string mtval = dataInfo.Substring(3, 2);
                                                                                    anvAnstallningar[noOfOccupationData].ended.year = int.Parse(yrval);
                                                                                    anvAnstallningar[noOfOccupationData].ended.month = int.Parse(mtval);
                                                                                }
                                                                            }
                                                                            else
                                                                            {
                                                                                if (dataInfo.Length > 7)
                                                                                {
                                                                                    // Format YYYYMMDD
                                                                                    string yrval = dataInfo.Substring(0, 4);
                                                                                    string mtval = dataInfo.Substring(4, 2);
                                                                                    string dyval = dataInfo.Substring(6, 2);
                                                                                    anvAnstallningar[noOfOccupationData].ended.year = int.Parse(yrval);
                                                                                    anvAnstallningar[noOfOccupationData].ended.month = int.Parse(mtval);
                                                                                    anvAnstallningar[noOfOccupationData].ended.day = int.Parse(dyval);
                                                                                }
                                                                                else if (dataInfo.Length > 5)
                                                                                {
                                                                                    // Format YYMMDD
                                                                                    string yrval = dataInfo.Substring(0, 2);
                                                                                    string mtval = dataInfo.Substring(2, 2);
                                                                                    string dyval = dataInfo.Substring(4, 2);
                                                                                    anvAnstallningar[noOfOccupationData].ended.year = int.Parse(yrval);
                                                                                    anvAnstallningar[noOfOccupationData].ended.month = int.Parse(mtval);
                                                                                    anvAnstallningar[noOfOccupationData].ended.day = int.Parse(dyval);
                                                                                }
                                                                                else
                                                                                {
                                                                                    // Format YYYY
                                                                                    string yrval = dataInfo.Substring(0, 4);
                                                                                    anvAnstallningar[noOfOccupationData].ended.year = int.Parse(yrval);
                                                                                }
                                                                            }
                                                                        }
                                                                        else
                                                                        {
                                                                            // Start-Date; End-Date
                                                                            string stdt = dataInfo.Substring(0, dp8);
                                                                            int dp81 = stdt.IndexOf("-");
                                                                            if ((dp81 > 0) && (dp81 < stdt.Length - 1))
                                                                            {
                                                                                if (stdt.Length > 9)
                                                                                {
                                                                                    // Format YYYY-MM-DD
                                                                                    string yrval = stdt.Substring(0, 4);
                                                                                    string mtval = stdt.Substring(5, 2);
                                                                                    string dyval = stdt.Substring(8, 2);
                                                                                    anvAnstallningar[noOfOccupationData].started.year = int.Parse(yrval);
                                                                                    anvAnstallningar[noOfOccupationData].started.month = int.Parse(mtval);
                                                                                    anvAnstallningar[noOfOccupationData].started.day = int.Parse(dyval);
                                                                                }
                                                                                else if (dataInfo.Length > 7)
                                                                                {
                                                                                    // Format YY-MM-DD
                                                                                    string yrval = stdt.Substring(0, 2);
                                                                                    string mtval = stdt.Substring(3, 2);
                                                                                    string dyval = stdt.Substring(6, 2);
                                                                                    anvAnstallningar[noOfOccupationData].started.year = int.Parse(yrval);
                                                                                    anvAnstallningar[noOfOccupationData].started.month = int.Parse(mtval);
                                                                                    anvAnstallningar[noOfOccupationData].started.day = int.Parse(dyval);
                                                                                }
                                                                                else
                                                                                {
                                                                                    // Format YY-MM
                                                                                    string yrval = stdt.Substring(0, 2);
                                                                                    string mtval = stdt.Substring(3, 2);
                                                                                    anvAnstallningar[noOfOccupationData].started.year = int.Parse(yrval);
                                                                                    anvAnstallningar[noOfOccupationData].started.month = int.Parse(mtval);
                                                                                }
                                                                            }
                                                                            else
                                                                            {
                                                                                if (dataInfo.Length > 7)
                                                                                {
                                                                                    // Format YYYYMMDD
                                                                                    string yrval = stdt.Substring(0, 4);
                                                                                    string mtval = stdt.Substring(4, 2);
                                                                                    string dyval = stdt.Substring(6, 2);
                                                                                    anvAnstallningar[noOfOccupationData].started.year = int.Parse(yrval);
                                                                                    anvAnstallningar[noOfOccupationData].started.month = int.Parse(mtval);
                                                                                    anvAnstallningar[noOfOccupationData].started.day = int.Parse(dyval);
                                                                                }
                                                                                else if (dataInfo.Length > 5)
                                                                                {
                                                                                    // Format YYMMDD
                                                                                    string yrval = stdt.Substring(0, 2);
                                                                                    string mtval = stdt.Substring(2, 2);
                                                                                    string dyval = stdt.Substring(4, 2);
                                                                                    anvAnstallningar[noOfOccupationData].started.year = int.Parse(yrval);
                                                                                    anvAnstallningar[noOfOccupationData].started.month = int.Parse(mtval);
                                                                                    anvAnstallningar[noOfOccupationData].started.day = int.Parse(dyval);
                                                                                }
                                                                                else
                                                                                {
                                                                                    // Format YYYY
                                                                                    string yrval = stdt.Substring(0, 4);
                                                                                    anvAnstallningar[noOfOccupationData].started.year = int.Parse(yrval);
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        noOfOccupationData++;
                                    }
                                } break;
                            case "Attended":
                                {
                                    // Attended : Event ID; Event-Type-tag; Start-Date-Time; End-Date-Time; Role
                                    if (noOfAttendedEventData < maxNoOfAttendedEventData)
                                    {
                                        dp0 = dataInfo.IndexOf(";");
                                        if ((dp0 > 0) && (dp0 < dataInfo.Length - 2))
                                        {
                                            // Event ID; Event-Type-tag; Start-Date-Time; End-Date-Time; Role
                                            string evid = dataInfo.Substring(0, dp0);
                                            if ((evid != "Undefined") && (evid != "Unknown"))
                                                anvTillstallningar[noOfAttendedEventData].eventID = evid;
                                            dataInfo = dataInfo.Substring(dp0 + 2, dataInfo.Length - dp0 - 2);
                                            dp1 = dataInfo.IndexOf(";");
                                            if ((dp1 > 0) && (dp1 < dataInfo.Length - 2))
                                            {
                                                // Event-Type-tag; Start-Date-Time; End-Date-Time; Role
                                                string evtp = dataInfo.Substring(0, dp1);
                                                bool foundCategory = false;
                                                for (int i = 0; i < noOfEventCategories; i++)
                                                {
                                                    if (evtp == eventCategories[i].tag)
                                                    {
                                                        anvTillstallningar[noOfAttendedEventData].eventType = eventCategories[i];
                                                        foundCategory = true;
                                                    }
                                                }
                                                if (!(foundCategory))
                                                {
                                                    addEventCategory(evtp, "No description", "Undefined");
                                                    anvTillstallningar[noOfAttendedEventData].eventType.tag = evtp;
                                                    anvTillstallningar[noOfAttendedEventData].eventType.description = "";
                                                    anvTillstallningar[noOfAttendedEventData].eventType.level = 0;
                                                }
                                                dataInfo = dataInfo.Substring(dp1 + 2, dataInfo.Length - dp1 - 2);
                                                dp2 = dataInfo.IndexOf(";");
                                                if ((dp2 > 0) && (dp2 < dataInfo.Length - 2))
                                                {
                                                    // Start-Date-Time; End-Date-Time; Role
                                                    string stdt = dataInfo.Substring(0, dp2);
                                                    int dp21 = stdt.IndexOf("-");
                                                    if ((dp21 > 0) && (dp21 < stdt.Length - 1))
                                                    {
                                                        if (stdt.Length > 9)
                                                        {
                                                            // Format YYYY-MM-DD
                                                            string yrval = stdt.Substring(0, 4);
                                                            string mtval = stdt.Substring(5, 2);
                                                            string dyval = stdt.Substring(8, 2);
                                                            anvTillstallningar[noOfAttendedEventData].started.year = int.Parse(yrval);
                                                            anvTillstallningar[noOfAttendedEventData].started.month = int.Parse(mtval);
                                                            anvTillstallningar[noOfAttendedEventData].started.day = int.Parse(dyval);
                                                        }
                                                        else if (stdt.Length > 7)
                                                        {
                                                            // Format YY-MM-DD
                                                            string yrval = stdt.Substring(0, 2);
                                                            string mtval = stdt.Substring(3, 2);
                                                            string dyval = stdt.Substring(6, 2);
                                                            anvTillstallningar[noOfAttendedEventData].started.year = int.Parse(yrval);
                                                            anvTillstallningar[noOfAttendedEventData].started.month = int.Parse(mtval);
                                                            anvTillstallningar[noOfAttendedEventData].started.day = int.Parse(dyval);
                                                        }
                                                        else
                                                        {
                                                            // Format YY-MM
                                                            string yrval = stdt.Substring(0, 2);
                                                            string mtval = stdt.Substring(3, 2);
                                                            anvTillstallningar[noOfAttendedEventData].started.year = int.Parse(yrval);
                                                            anvTillstallningar[noOfAttendedEventData].started.month = int.Parse(mtval);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (stdt.Length > 7)
                                                        {
                                                            // Format YYYYMMDD
                                                            string yrval = stdt.Substring(0, 4);
                                                            string mtval = stdt.Substring(4, 2);
                                                            string dyval = stdt.Substring(6, 2);
                                                            anvTillstallningar[noOfAttendedEventData].started.year = int.Parse(yrval);
                                                            anvTillstallningar[noOfAttendedEventData].started.month = int.Parse(mtval);
                                                            anvTillstallningar[noOfAttendedEventData].started.day = int.Parse(dyval);
                                                        }
                                                        else if (stdt.Length > 5)
                                                        {
                                                            // Format YYMMDD
                                                            string yrval = stdt.Substring(0, 2);
                                                            string mtval = stdt.Substring(2, 2);
                                                            string dyval = stdt.Substring(4, 2);
                                                            anvTillstallningar[noOfAttendedEventData].started.year = int.Parse(yrval);
                                                            anvTillstallningar[noOfAttendedEventData].started.month = int.Parse(mtval);
                                                            anvTillstallningar[noOfAttendedEventData].started.day = int.Parse(dyval);
                                                        }
                                                        else
                                                        {
                                                            // Format YYYY
                                                            string yrval = stdt.Substring(0, 4);
                                                            anvTillstallningar[noOfAttendedEventData].started.year = int.Parse(yrval);
                                                        }
                                                    }
                                                    dataInfo = dataInfo.Substring(dp2 + 2, dataInfo.Length - dp2 - 2);
                                                    // End-Date;  Role
                                                    int dpp = dataInfo.IndexOf(";");
                                                    if ((dpp > 0) && (dpp < dataInfo.Length - 1))
                                                    {
                                                        string stds = dataInfo.Substring(0, dpp);
                                                        dp9 = stds.IndexOf("-");
                                                        if ((dp9 > 0) && (dp9 < stds.Length - 1))
                                                        {
                                                            string yrval = stds.Substring(0, dp9);
                                                            stds = stds.Substring(dp9 + 1, stds.Length - dp9 - 1);
                                                            dp9 = stds.IndexOf("-");
                                                            if ((dp9 > 0) && (dp9 < stds.Length - 1))
                                                            {
                                                                string mtval = stds.Substring(0, dp9);
                                                                stds = stds.Substring(dp9 + 1, stds.Length - dp9 - 1);
                                                                anvTillstallningar[noOfAttendedEventData].ended.year = int.Parse(yrval);
                                                                anvTillstallningar[noOfAttendedEventData].ended.month = int.Parse(mtval);
                                                                anvTillstallningar[noOfAttendedEventData].ended.day = int.Parse(stds);
                                                            }
                                                            else
                                                            {
                                                                anvTillstallningar[noOfAttendedEventData].ended.year = int.Parse(yrval);
                                                                anvTillstallningar[noOfAttendedEventData].ended.month = int.Parse(stds);
                                                            }
                                                        }
                                                        else if (stds.Length == 4)
                                                            anvTillstallningar[noOfAttendedEventData].ended.year = int.Parse(stds);
                                                        else if (stds.Length == 2)
                                                            anvTillstallningar[noOfAttendedEventData].ended.year = int.Parse(stds) + 1900;
                                                        else if (stds.Length > 7)
                                                        {
                                                            // Format YYYYMMDD
                                                            string yrval = stds.Substring(0, 4);
                                                            string mtval = stds.Substring(4, 2);
                                                            string dyval = stds.Substring(6, 2);
                                                            anvTillstallningar[noOfAttendedEventData].ended.year = int.Parse(yrval);
                                                            anvTillstallningar[noOfAttendedEventData].ended.month = int.Parse(mtval);
                                                            anvTillstallningar[noOfAttendedEventData].ended.day = int.Parse(dyval);
                                                        }
                                                        else if (stds.Length > 5)
                                                        {
                                                            // Format YYMMDD
                                                            string yrval = stds.Substring(0, 2);
                                                            string mtval = stds.Substring(2, 2);
                                                            string dyval = stds.Substring(4, 2);
                                                            anvTillstallningar[noOfAttendedEventData].ended.year = int.Parse(yrval);
                                                            anvTillstallningar[noOfAttendedEventData].ended.month = int.Parse(mtval);
                                                            anvTillstallningar[noOfAttendedEventData].ended.day = int.Parse(dyval);
                                                        }
                                                        else
                                                        {
                                                            // Format YYYY
                                                            string yrval = stds.Substring(0, 4);
                                                            anvTillstallningar[noOfAttendedEventData].ended.year = int.Parse(yrval);
                                                        }
                                                        dataInfo = dataInfo.Substring(dpp + 2, dataInfo.Length - dpp - 2);
                                                        bool foundRoleCategory = false;
                                                        for (int i = 0; i < noOfRoleCategories; i++)
                                                        {
                                                            if (roleCategories[i].tag == dataInfo)
                                                            {
                                                                foundRoleCategory = true;
                                                                anvTillstallningar[noOfAttendedEventData].role = roleCategories[i];
                                                            }
                                                        }
                                                        if ((!(foundRoleCategory)) && (noOfRoleCategories < maxNoOfRoleCategories))
                                                        {
                                                            anvTillstallningar[noOfAttendedEventData].role.tag = dataInfo;
                                                            anvTillstallningar[noOfAttendedEventData].role.description = "";
                                                            anvTillstallningar[noOfAttendedEventData].role.level = 0;
                                                            addRoleCategory(dataInfo, "", "Undefined");
                                                        }
                                                    }
                                                    else
                                                    {
                                                        dp9 = dataInfo.IndexOf("-");
                                                        if ((dp9 > 0) && (dp9 < dataInfo.Length - 1))
                                                        {
                                                            if (dataInfo.Length > 9)
                                                            {
                                                                // Format YYYY-MM-DD
                                                                string yrval = dataInfo.Substring(0, 4);
                                                                string mtval = dataInfo.Substring(5, 2);
                                                                string dyval = dataInfo.Substring(8, 2);
                                                                anvTillstallningar[noOfAttendedEventData].ended.year = int.Parse(yrval);
                                                                anvTillstallningar[noOfAttendedEventData].ended.month = int.Parse(mtval);
                                                                anvTillstallningar[noOfAttendedEventData].ended.day = int.Parse(dyval);
                                                            }
                                                            else if (dataInfo.Length > 7)
                                                            {
                                                                // Format YY-MM-DD
                                                                string yrval = dataInfo.Substring(0, 2);
                                                                string mtval = dataInfo.Substring(3, 2);
                                                                string dyval = dataInfo.Substring(6, 2);
                                                                anvTillstallningar[noOfAttendedEventData].ended.year = int.Parse(yrval);
                                                                anvTillstallningar[noOfAttendedEventData].ended.month = int.Parse(mtval);
                                                                anvTillstallningar[noOfAttendedEventData].ended.day = int.Parse(dyval);
                                                            }
                                                            else
                                                            {
                                                                // Format YY-MM
                                                                string yrval = dataInfo.Substring(0, 2);
                                                                string mtval = dataInfo.Substring(3, 2);
                                                                anvTillstallningar[noOfAttendedEventData].ended.year = int.Parse(yrval);
                                                                anvTillstallningar[noOfAttendedEventData].ended.month = int.Parse(mtval);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            if (dataInfo.Length > 7)
                                                            {
                                                                // Format YYYYMMDD
                                                                string yrval = dataInfo.Substring(0, 4);
                                                                string mtval = dataInfo.Substring(4, 2);
                                                                string dyval = dataInfo.Substring(6, 2);
                                                                anvTillstallningar[noOfAttendedEventData].ended.year = int.Parse(yrval);
                                                                anvTillstallningar[noOfAttendedEventData].ended.month = int.Parse(mtval);
                                                                anvTillstallningar[noOfAttendedEventData].ended.day = int.Parse(dyval);
                                                            }
                                                            else if (dataInfo.Length > 5)
                                                            {
                                                                // Format YYMMDD
                                                                string yrval = dataInfo.Substring(0, 2);
                                                                string mtval = dataInfo.Substring(2, 2);
                                                                string dyval = dataInfo.Substring(4, 2);
                                                                anvTillstallningar[noOfAttendedEventData].ended.year = int.Parse(yrval);
                                                                anvTillstallningar[noOfAttendedEventData].ended.month = int.Parse(mtval);
                                                                anvTillstallningar[noOfAttendedEventData].ended.day = int.Parse(dyval);
                                                            }
                                                            else
                                                            {
                                                                // Format YYYY
                                                                string yrval = dataInfo.Substring(0, 4);
                                                                anvTillstallningar[noOfAttendedEventData].ended.year = int.Parse(yrval);
                                                            }
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    // Start-Date-Time;
                                                    dp9 = dataInfo.IndexOf("-");
                                                    if ((dp9 > 0) && (dp9 < dataInfo.Length - 1))
                                                    {
                                                        if (dataInfo.Length > 9)
                                                        {
                                                            // Format YYYY-MM-DD
                                                            string yrval = dataInfo.Substring(0, 4);
                                                            string mtval = dataInfo.Substring(5, 2);
                                                            string dyval = dataInfo.Substring(8, 2);
                                                            anvTillstallningar[noOfAttendedEventData].started.year = int.Parse(yrval);
                                                            anvTillstallningar[noOfAttendedEventData].started.month = int.Parse(mtval);
                                                            anvTillstallningar[noOfAttendedEventData].started.day = int.Parse(dyval);
                                                        }
                                                        else if (dataInfo.Length > 7)
                                                        {
                                                            // Format YY-MM-DD
                                                            string yrval = dataInfo.Substring(0, 2);
                                                            string mtval = dataInfo.Substring(3, 2);
                                                            string dyval = dataInfo.Substring(6, 2);
                                                            anvTillstallningar[noOfAttendedEventData].started.year = int.Parse(yrval);
                                                            anvTillstallningar[noOfAttendedEventData].started.month = int.Parse(mtval);
                                                            anvTillstallningar[noOfAttendedEventData].started.day = int.Parse(dyval);
                                                        }
                                                        else
                                                        {
                                                            // Format YY-MM
                                                            string yrval = dataInfo.Substring(0, 2);
                                                            string mtval = dataInfo.Substring(3, 2);
                                                            anvTillstallningar[noOfAttendedEventData].started.year = int.Parse(yrval);
                                                            anvTillstallningar[noOfAttendedEventData].started.month = int.Parse(mtval);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (dataInfo.Length > 7)
                                                        {
                                                            // Format YYYYMMDD
                                                            string yrval = dataInfo.Substring(0, 4);
                                                            string mtval = dataInfo.Substring(4, 2);
                                                            string dyval = dataInfo.Substring(6, 2);
                                                            anvTillstallningar[noOfAttendedEventData].started.year = int.Parse(yrval);
                                                            anvTillstallningar[noOfAttendedEventData].started.month = int.Parse(mtval);
                                                            anvTillstallningar[noOfAttendedEventData].started.day = int.Parse(dyval);
                                                        }
                                                        else if (dataInfo.Length > 5)
                                                        {
                                                            // Format YYMMDD
                                                            string yrval = dataInfo.Substring(0, 2);
                                                            string mtval = dataInfo.Substring(2, 2);
                                                            string dyval = dataInfo.Substring(4, 2);
                                                            anvTillstallningar[noOfAttendedEventData].started.year = int.Parse(yrval);
                                                            anvTillstallningar[noOfAttendedEventData].started.month = int.Parse(mtval);
                                                            anvTillstallningar[noOfAttendedEventData].started.day = int.Parse(dyval);
                                                        }
                                                        else
                                                        {
                                                            // Format YYYY
                                                            string yrval = dataInfo.Substring(0, 4);
                                                            anvTillstallningar[noOfAttendedEventData].started.year = int.Parse(yrval);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        noOfAttendedEventData++;
                                    }
                                } break;
                            case "ReltdImg":
                                {
                                    // ReltdImg : Path-Name-ext; Type-Of-Content-Tag; Security-Level
                                    if (noOfRelatedImagesData < maxNoOfRelatedImagesData)
                                    {
                                        // Path-Name-ext; Type-Of-Content-Tag; Security-Level
                                        dp0 = dataInfo.IndexOf(";");
                                        if ((dp0 > 0) && (dp0 < dataInfo.Length - 2))
                                        {
                                            string pne = dataInfo.Substring(0, dp0);
                                            if ((pne != "Unknown") && (pne != "Undefined"))
                                                anvRelBilder[noOfRelatedImagesData].imagePathName = pne;
                                            dataInfo = dataInfo.Substring(dp0 + 2, dataInfo.Length - dp0 - 2);
                                            dp1 = dataInfo.IndexOf(";");
                                            if ((dp1 > 0) && (dp1 < dataInfo.Length - 2))
                                            {
                                                // Type-Of-Content-Tag; Security-Level
                                                string toct = dataInfo.Substring(0, dp1);
                                                bool foundCategory = false;
                                                for (int i = 0; i < noOfContextCategories; i++)
                                                {
                                                    if (contextCategories[i].tag == toct)
                                                    {
                                                        anvRelBilder[noOfRelatedImagesData].imageContext = contextCategories[i];
                                                        foundCategory = true;
                                                    }
                                                }
                                                if (!(foundCategory))
                                                {
                                                    addContextCategory(toct, "No desciption", "Undefined");
                                                    anvRelBilder[noOfRelatedImagesData].imageContext.tag = toct;
                                                }
                                                dataInfo = dataInfo.Substring(dp1 + 2, dataInfo.Length - dp1 - 2);
                                                // Security-Level
                                                if ((dataInfo == "Unclassified") || (dataInfo == "unclassified"))
                                                    anvRelBilder[noOfRelatedImagesData].classificationLevel = ClassificationType.Unclassified;
                                                else if ((dataInfo == "Limited") || (dataInfo == "limited"))
                                                    anvRelBilder[noOfRelatedImagesData].classificationLevel = ClassificationType.Limited;
                                                else if ((dataInfo == "Confidential") || (dataInfo == "confidential"))
                                                    anvRelBilder[noOfRelatedImagesData].classificationLevel = ClassificationType.Confidential;
                                                else if ((dataInfo == "Secret") || (dataInfo == "secret"))
                                                    anvRelBilder[noOfRelatedImagesData].classificationLevel = ClassificationType.Secret;
                                                else if ((dataInfo == "Qualified Secret") || (dataInfo == "qualified secret") ||
                                                         (dataInfo == "Qualified") || (dataInfo == "qualified"))
                                                    anvRelBilder[noOfRelatedImagesData].classificationLevel = ClassificationType.QualifSecret;
                                                else
                                                    anvRelBilder[noOfRelatedImagesData].classificationLevel = ClassificationType.UndefLevel;
                                            }
                                        }
                                        noOfRelatedImagesData++;
                                    }
                                } break;
                            case "National":
                                {
                                    // National : Nationality-name
                                    if (noOfNationalityData < maxNoOfNationalityData)
                                    {
                                        nationality[noOfNationalityData] = dataInfo;
                                        noOfNationalityData++;
                                    }
                                } break;
                            default:
                                // A unused value
                                break;
                        }
                    }
                }
            }
        }
        public bool saveActorData(string userID, string storagePath)
        {
            bool retVal = false;
            string filename = "";
            if ((storagePath != "") && (System.IO.Directory.Exists(storagePath)) && 
                (System.IO.File.Exists(storagePath + "\\ActorData_" + userID + ".acf")))
            {
                filename = storagePath + "\\ActorData_" + userID + ".acf";
            }
            else
            {
                int tnr = scu.IndexOf("\\");
                if ((tnr > 0) && (tnr < scu.Length - 1))
                    scu = scu.Substring(tnr + 1, scu.Length - tnr - 1);
                string rootPath = "C:\\Users\\" + scu + "\\" + sProgPath;
                filename = rootPath + "\\ActorData\\ActorData_" + userID + ".acf";
            }
            using (System.IO.FileStream afs = System.IO.File.Create(filename))
            {
                using (System.IO.StreamWriter sw = new System.IO.StreamWriter(afs))
                {
                    var line = "UserId   : " + this.userId; // + Environment.NewLine;
                    sw.WriteLine(line);
                    for (int i = 0; i < this.getNoOfUserNames(); i++)
                    {
                        line = "UserName : " + this.getUserSurName(i) + " " + this.getUserMidName(i) + " " + this.getUserFamName(i) + "; " + this.getUserNameTag(i);
                        sw.WriteLine(line);
                    }
                    for (int i = 0; i < this.getNoOfUserContacts(); i++)
                    {
                        line = "Contact  : " + this.getUserContactType(i) + "; " + this.getUserContactPath(i);
                        sw.WriteLine(line);
                    }
                    line = "BrthData : " + this.getUserBirthStreet();
                    line = line + "; " + this.getUserBirthStreetNumber();
                    line = line + "; " + this.getUserBirthStreetNumberAddon();
                    line = line + "; " + this.getUserBirthCityname();
                    line = line + "; " + this.getUserBirthAreaname();
                    line = line + "; " + this.getUserBirthZipcode();
                    line = line + "; " + this.getUserBirthCountry();
                    line = line + "; " + this.getUserBirthDate();
                    line = line + "; " + this.getUserBirthSocNo();
                    line = line + "; " + this.getUserBirthGender();
                    line = line + "; " + this.getUserBirthLatitude();
                    line = line + "; " + this.getUserBirthLongitude();
                    sw.WriteLine(line);
                    for (int i = 0; i < getNumberOfSkinTones(); i++)
                    {
                        line = "SkinTone : " + getUserSkinToneTag(i) + "; " + getUserSkinToneRedChannel(i) + "; " + getUserSkinToneGreenChannel(i) + "; " + getUserSkinToneBlueChannel(i) + "; " + getUserSkinToneValidDate(i);
                        sw.WriteLine(line);
                    }
                    line = "RelStats + " + anvAktuellRelation.tag + "; " + anvAktuellRelation.description + "; " + anvAktuellRelation.level.ToString();
                    sw.WriteLine(line);
                    for (int i = 0; i < getNumberOfGenderData(); i++)
                    {
                        line = "Gender   : " + getUserGenderType(i);
                        line = line + "; " + getUserGenderLength(i);
                        line = line + "; " + getUserGenderCircumf(i);
                        line = line + "; " + getUserGenderUnit(i);
                        line = line + "; " + getUserGenderAppearance(i);
                        line = line + "; " + getUserGenderBehaviour(i);
                        line = line + "; " + getUserGenderPres(i);
                        line = line + "; " + getUserGenderInfoValidDate(i);
                        sw.WriteLine(line);
                    }
                    for (int i = 0; i < getNumberOfWeightData(); i++)
                    {
                        // Weight   : Value; unit; Valid-Date
                        line = "Weight   : " + getUserWeightVal(i) + "; " + getUserWeightUnit(i) + "; " + getUserWeightInfoValidDate(i);
                        sw.WriteLine(line);
                    }
                    for (int i = 0; i < getNumberOfChestData(); i++)
                    {
                        // Boobs    : Type-tag; Circumf; Unit; Size-tag; Valid-Date
                        line = "Boobs    : " + getUserChestType(i);
                        line = line + "; " + getUserChestCircumfVal(i);
                        line = line + "; " + getUserChestCircumfUnit(i);
                        line = line + "; " + getUserChestSizeType(i);
                        line = line + "; " + getUserChestInfoValidDate(i);
                        sw.WriteLine(line);
                    }
                    for (int i = 0; i < getNumberOfFaceData(); i++)
                    {
                        // FaceData : Eye-width; Cheekbone-Width; Chin-Width; Mouth-Width; Height; Unit; Valid-Date
                        line = "FaceData : " + getFaceEyeWidth(i);
                        line = line + getFaceCheekboneWidth(i);
                        line = line + getFaceChinWidth(i);
                        line = line + getFaceMouthWidth(i);
                        line = line + getFaceHeight(i);
                        line = line + getFaceUsedUnit(i);
                        line = line + getFaceInfoValidDate(i);
                        sw.WriteLine(line);
                    }
                    for (int i = 0; i < getNoOfResicenceData(); i++)
                    {
                        // Residnce : Streetname; number; additive; City; Area; Zipcode; Country; Purchase; In-Date; Out-Date; Sale; Currency
                        line = "Residnce : " + getActorResidStrtNme(i);
                        line = line + "; " + getActorResidStrtNo(i);
                        line = line + "; " + getActorResidStrtNoAdd(i);
                        line = line + "; " + getActorResidCity(i);
                        line = line + "; " + getActorResidZipcode(i);
                        line = line + "; " + getActorResidCountry(i);
                        line = line + "; " + getActorResidBoughtVal(i);
                        line = line + "; " + getActorResidBought(i);
                        line = line + "; " + getActorResidSold(i);
                        line = line + "; " + getActorResidSalesVal(i);
                        line = line + "; " + getActorResidCurrency(i);
                        sw.WriteLine(line);
                    }
                    for (int i = 0; i < getNumberOfHairData(); i++)
                    {
                        // HairColr : Color-tag; Texture-tag; Length-tag; Valid-Date
                        line = "HairColr : " + getUserHairColor(i);
                        line = line + "; " + getUserHairTexture(i);
                        line = line + "; " + getUserHairLength(i);
                        line = line + "; " + getUserHairValidDate(i);
                        sw.WriteLine(line);
                    }
                    for (int i = 0; i < getNumberOfEyeData(); i++)
                    {
                        // EyeColor : Color-tag; Form-tag; Valid-Date
                        line = "EyeColor : " + getUserEyeColorTag(i) + "; " + getUserEyeFormTag(i) + "; " + getUserEyeDataValidDate(i);
                        sw.WriteLine(line);
                    }
                    for (int i = 0; i < getNoOfMarkingsData(); i++)
                    {
                        // MarkData : type-tag; placement; Motif; Valid-Date
                        line = "MarkData : " + getActorMarkingType(i) + "; " + getActorMarkingPlace(i) + "; " + getActorMarkingMotif(i) + "; " + getActorMarkingValidDate(i);
                        sw.WriteLine(line);
                    }
                    for (int i = 0; i < getNoOfOccupationsData(); i++)
                    {
                        // Ocupatn  : Title; Company; Streetname; Number; State; Areaname; Zipcode; Country; Start-Date; End-Date
                        line = "Ocupatn  : " + getActorOccupationTitle(i);
                        line = line + "; " + getActorOccupationCompany(i);
                        line = line + "; " + getActorOccupationStrtNme(i);
                        line = line + "; " + getActorOccupationStrtNum(i);
                        line = line + "; " + getActorOccupationStatename(i);
                        line = line + "; " + getActorOccupationAreaname(i);
                        line = line + "; " + getActorOccupationZipcode(i);
                        line = line + "; " + getActorOccupationCountry(i);
                        line = line + "; " + getActorOccupationStarted(i);
                        if (getActorOccupationEnded(i) != "")
                            line = line + "; " + getActorOccupationEnded(i);
                        sw.WriteLine(line);
                    }
                    for (int i = 0; i < getNoOfAttendedEventsData(); i++)
                    {
                        // Attended : Event ID; Event-Type-tag; Start-Date-Time; End-Date-Time; Role
                        line = "Attended : " + getActorAttendedEventID(i);
                        line = line + "; " + getActorAttendedEventType(i);
                        line = line + "; " + getActorAttendedEventStarted(i);
                        line = line + "; " + getActorAttendedEventEnded(i);
                        line = line + "; " + getActorAttendedEventRoleTag(i);
                        sw.WriteLine(line);
                    }
                    for (int i = 0; i < getNoOfRelatedImages(); i++)
                    {
                        // ReltdImg : Path-Name-ext; Type-Of-Content-Tag; Security-Level
                        line = "ReltdImg : " + getActorRelatedImagePath(i) + "; " + getActorRelatedImageContent(i) + "; " + getActorRelatedImageClass(i);
                        sw.WriteLine(line);
                    }
                    for (int i = 0; i < getNoOfNationalityData(); i++)
                    {
                        // National : Nationality-name
                        line = "National : " + getNationalityData(i);
                        sw.WriteLine(line);
                    }

                    // Last line of this "using"-clause.
                    sw.Close();
                }
                afs.Close();
            }
            return retVal;
        }
        public int getNoOfUserNames() { return noOfNames; }
        public string getUserNameTag(int nr) { return anvNamn[nr].nameType.ToString(); }
        public string getUserSurName(int nr) { return anvNamn[nr].Surname; }
        public string getUserMidName(int nr) { return anvNamn[nr].Midname; }
        public string getUserFamName(int nr) { return anvNamn[nr].Famname; }
        public bool addUserName(string type, string surn, string midn, string famn)
        {
            if (noOfNames < maxNoOfNames)
            {
                noOfNames++;
                if ((type == "Birth") || (type == "birth") || (type == "born"))
                    anvNamn[noOfNames].nameType = NameType.Birth;
                else if ((type == "Taken") || (type == "taken"))
                    anvNamn[noOfNames].nameType = NameType.Taken;
                else if ((type == "Married") || (type == "married") || (type == "Husbands") || (type == "husbands"))
                    anvNamn[noOfNames].nameType = NameType.Married;
                else if ((type == "Alias") || (type == "alias"))
                    anvNamn[noOfNames].nameType = NameType.Alias;
                else if ((type == "Nick") || (type == "nick") || (type == "Nickname") || (type == "nickname"))
                    anvNamn[noOfNames].nameType = NameType.Nickname;
                else
                    anvNamn[noOfNames].nameType = NameType.UndefNameType;
                anvNamn[noOfNames].Surname = surn;
                anvNamn[noOfNames].Midname = midn;
                anvNamn[noOfNames].Famname = famn;
                return true;
            }
            else
                return false;
        }
        public int getNoOfUserContacts() { return noOfContacts; }
        public string getUserContactType(int nr) { return anvKontakter[nr].type.tag; }
        public string getUserContactPath(int nr) { return anvKontakter[nr].contactPath; }
        public bool addUserContact(string type, string path)
        {
            if (noOfContacts < maxNoOfContacts)
            {
                bool foundCategory = false;
                for (int i = 0; i < noOfContactCategories; i++)
                {
                    if (type == contactCategory[i].tag)
                    {
                        foundCategory = true;
                        anvKontakter[noOfContacts].type = contactCategory[i];
                        anvKontakter[noOfContacts].contactPath = path;
                        noOfContacts++;
                        return true;
                    }
                }
                if ((!(foundCategory)) && (noOfContactCategories < maxNoOfContactCategories))
                {
                    addContactCategory(type, "", "Undefined");
                    anvKontakter[noOfContacts].type.tag = type;
                    anvKontakter[noOfContacts].type.description = "";
                    anvKontakter[noOfContacts].type.level = 0;
                    anvKontakter[noOfContacts].contactPath = path;
                    noOfContacts++;
                    return true;
                }
                else
                    return false;
            }
            else
                return false;
        }
        public string getUserBirthStreet() { return anvFodlseData.brthStreet; }
        public string getUserBirthStreetNumber() { return anvFodlseData.brthStreetNumber.ToString(); }
        public string getUserBirthStreetNumberAddon() { return anvFodlseData.brthStreetNumberAddon; }
        public string getUserBirthCityname() { return anvFodlseData.brthCityname; }
        public string getUserBirthAreaname() { return anvFodlseData.brthAreaname; }
        public string getUserBirthZipcode() { return anvFodlseData.brthZipcode; }
        public string getUserBirthCountry() { return anvFodlseData.brthCountryname; }
        public string getUserBirthDate() { return anvFodlseData.brthDate.year.ToString() + "-" + anvFodlseData.brthDate.month.ToString() + "-" + anvFodlseData.brthDate.day.ToString(); }
        public string getUserBirthSocNo() { return anvFodlseData.brthSecurityCode; }
        public void setUserBirthSocNo(string indata) { anvFodlseData.brthSecurityCode = indata; }
        public string getUserBirthGender() { return anvFodlseData.brthGender.ToString(); }
        public string getUserBirthLatitude() { return anvFodlseData.latVal.ToString() + " " + anvFodlseData.latDir.ToString(); }
        public void setUserBirthLatitude(string indata)
        {
            // Format DD[ |.]MM[ |.]SS.ss [N|n|North|north]/[S|s|South|south]
            int dp = indata.IndexOf(" ");
            if ((dp > 0) && (dp < indata.Length - 1))
            {
                // Have at least one space
                // Format: DD[ MM[ SS[.ss]]][ N|N...
                if (dp > 8)
                {
                    // Format YYMMSS.ss [N|n|North|north]/[S|s|South|south]
                    string degstr = indata.Substring(0, 2);
                    int degint = int.Parse(degstr);
                    string minstr = indata.Substring(2, 2);
                    int minint = int.Parse(minstr);
                    string secstr = indata.Substring(4, 2);
                    int secint = int.Parse(secstr);
                    string semisecstr = indata.Substring(7, 2);
                    int semisecint = int.Parse(semisecstr);
                    anvFodlseData.latVal = (degint * 10000) + (minint * 100) + (secint * 1) + (semisecint / 100);
                    string dirstr = indata.Substring(dp + 1, indata.Length - dp - 1);
                    if ((dirstr == "N") || (dirstr == "n") || (dirstr == "North") || (dirstr == "north"))
                        anvFodlseData.latDir = GeogrDir.North;
                    else
                        anvFodlseData.latDir = GeogrDir.South;
                }
                else if (dp > 5)
                {
                    string tststr1 = indata.Substring(dp + 1, indata.Length - dp - 1);
                    int dp1 = tststr1.IndexOf(" ");
                    if ((dp1 > 0) && (dp1 < tststr1.Length))
                    {
                        // Format YYMMSS ss [N|n|North|north]/[S|s|South|south]
                        string degstr = indata.Substring(0, 2);
                        int degint = int.Parse(degstr);
                        string minstr = indata.Substring(2, 2);
                        int minint = int.Parse(minstr);
                        string secstr = indata.Substring(4, 2);
                        int secint = int.Parse(secstr);
                        string semisecstr = indata.Substring(8, 2);
                        int semisecint = int.Parse(semisecstr);
                        anvFodlseData.latVal = (degint * 10000) + (minint * 100) + (secint * 1) + (semisecint / 100);
                        string dirstr = tststr1.Substring(dp1 + 1, tststr1.Length - dp1 - 1);
                        if ((dirstr == "N") || (dirstr == "n") || (dirstr == "North") || (dirstr == "north"))
                            anvFodlseData.latDir = GeogrDir.North;
                        else
                            anvFodlseData.latDir = GeogrDir.South;
                    }
                    else
                    {
                        // Format YYMMSS [N|n|North|north]/[S|s|South|south]
                        string degstr = indata.Substring(0, 2);
                        int degint = int.Parse(degstr);
                        string minstr = indata.Substring(2, 2);
                        int minint = int.Parse(minstr);
                        string secstr = indata.Substring(4, 2);
                        int secint = int.Parse(secstr);
                        anvFodlseData.latVal = (degint * 10000) + (minint * 100) + (secint * 1);
                        string dirstr = indata.Substring(dp + 1, indata.Length - dp - 1);
                        if ((dirstr == "N") || (dirstr == "n") || (dirstr == "North") || (dirstr == "north"))
                            anvFodlseData.latDir = GeogrDir.North;
                        else
                            anvFodlseData.latDir = GeogrDir.South;
                    }
                }
                else if (dp > 3)
                {
                    // Format YYMM [N|n|North|north]/[S|s|South|south]
                    string degstr = indata.Substring(0, 2);
                    int degint = int.Parse(degstr);
                    string minstr = indata.Substring(2, 2);
                    int minint = int.Parse(minstr);
                    anvFodlseData.latVal = (degint * 10000) + (minint * 100);
                    string dirstr = indata.Substring(dp + 1, indata.Length - dp - 1);
                    if ((dirstr == "N") || (dirstr == "n") || (dirstr == "North") || (dirstr == "north"))
                        anvFodlseData.latDir = GeogrDir.North;
                    else
                        anvFodlseData.latDir = GeogrDir.South;
                }
                else if (dp > 1)
                {
                    // Format YY[ MM][ SS][ ss] [N|n|North|north]/[S|s|South|south]
                    // Format YY [N|n|North|north]/[S|s|South|south]
                    string teststr1 = indata.Substring(dp + 1, indata.Length - dp - 1);
                    int dp1 = teststr1.IndexOf(" ");
                    if ((dp1 > 0) && (dp1 < teststr1.Length - 1))
                    {
                        // Format YY[ MM][ SS][ ss] [N|n|North|north]/[S|s|South|south]
                    }
                    else
                    {
                        // Format YY [N|n|North|north]/[S|s|South|south]
                        string degstr = indata.Substring(0, 2);
                        int degint = int.Parse(degstr);
                        anvFodlseData.latVal = (degint * 10000);
                        string dirstr = indata.Substring(dp + 1, indata.Length - dp - 1);
                        if ((dirstr == "N") || (dirstr == "n") || (dirstr == "North") || (dirstr == "north"))
                            anvFodlseData.latDir = GeogrDir.North;
                        else
                            anvFodlseData.latDir = GeogrDir.South;
                    }
                }
            }
            else
            {
                // Format DD[.]MM[.]SS.ss[N|n|North|north]/[S|s|South|south]
                dp = indata.IndexOf(".");
                if ((dp > 0) && (dp < indata.Length - 1))
                {
                    int degrint = int.Parse(indata.Substring(0, dp));
                    indata = indata.Substring(dp + 1, indata.Length - dp - 1);
                    dp = indata.IndexOf(".");
                    if ((dp > 0) && (dp < indata.Length))
                    {
                        // Possibly minutes next
                        int minint = int.Parse(indata.Substring(0, dp));
                        indata = indata.Substring(dp + 1, indata.Length - dp - 1);
                        dp = indata.IndexOf(".");
                        if ((dp > 0) && (dp < indata.Length))
                        {
                            // Possibly seconds next
                            int secint = int.Parse(indata.Substring(0, dp));
                            indata = indata.Substring(dp + 1, indata.Length - dp - 1);
                            dp = indata.IndexOf(".");
                            if ((dp > 0) && (dp < indata.Length))
                            {
                                // Possibly semi-seconds next
                                int semisecint = int.Parse(indata.Substring(0, dp));
                                indata = indata.Substring(dp + 1, indata.Length - dp - 1);
                                anvFodlseData.latVal = (degrint * 10000) + (minint * 100) + (secint * 1) + (semisecint / 100);
                                if ((indata == "N") || (indata == "n") || (indata == "North") || (indata == "north"))
                                    anvFodlseData.latDir = GeogrDir.North;
                                else
                                    anvFodlseData.latDir = GeogrDir.South;
                            }
                            else
                            {
                                // Only degrees, minutes, seconds, and direction
                                anvFodlseData.latVal = (degrint * 10000) + (minint * 100) + (secint * 1);// + (semisecint / 100);
                                if ((indata == "N") || (indata == "n") || (indata == "North") || (indata == "north"))
                                    anvFodlseData.latDir = GeogrDir.North;
                                else
                                    anvFodlseData.latDir = GeogrDir.South;
                            }
                        }
                        else
                        {
                            // Only degrees, minutes, and direction
                            anvFodlseData.latVal = (degrint * 10000) + (minint * 100);// + (secint * 1) + (semisecint / 100);
                            if ((indata == "N") || (indata == "n") || (indata == "North") || (indata == "north"))
                                anvFodlseData.latDir = GeogrDir.North;
                            else
                                anvFodlseData.latDir = GeogrDir.South;
                        }
                    }
                    else
                    {
                        // Possibly direction
                        anvFodlseData.latVal = (degrint * 10000);// + (minint * 100) + (secint * 1) + (semisecint / 100);
                        if ((indata == "N") || (indata == "n") || (indata == "North") || (indata == "north"))
                            anvFodlseData.latDir = GeogrDir.North;
                        else
                            anvFodlseData.latDir = GeogrDir.South;
                    }
                }
            }
        }
        public string getUserBirthLongitude() { return anvFodlseData.lonVal.ToString() + " " + anvFodlseData.lonDir.ToString(); }
        public void setUserBirthLongitude(string indata)
        {
            // Format can be:
            //     DDD.MM.SS.ss [East|east|E|e]|[West|west|W|w]
            //     DDD MM SS ss [East|east|E|e]|[West|west|W|w]
            int dp = indata.IndexOf(".");
            if ((dp > 0) && (dp < indata.Length))
            {
                // Format: DDD.MM.SS.ss [East|east|E|e]|[West|west|W|w]
                int degpart = int.Parse(indata.Substring(0, dp));
                indata = indata.Substring(dp + 1, indata.Length - dp - 1);
                dp = indata.IndexOf(".");
                if ((dp > 0) && (dp < indata.Length))
                {
                    // Format: MM.SS.ss [East|east|E|e]|[West|west|W|w]
                    int minpart = int.Parse(indata.Substring(0, dp));
                    indata = indata.Substring(dp + 1, indata.Length - dp - 1);
                    dp = indata.IndexOf(".");
                    if ((dp > 0) && (dp < indata.Length))
                    {
                        // Format: SS.ss [East|east|E|e]|[West|west|W|w]
                        int secpart = int.Parse(indata.Substring(0, dp));
                        indata = indata.Substring(dp + 1, indata.Length - dp - 1);
                        dp = indata.IndexOf(" ");
                        if ((dp > 0) && (dp < indata.Length))
                        {
                            // Format: ss [East|east|E|e]|[West|west|W|w]
                            int semisecpart = int.Parse(indata.Substring(0, dp));
                            indata = indata.Substring(dp + 1, indata.Length - dp - 1);
                            anvFodlseData.lonVal = (degpart * 10000) + (minpart * 100) + secpart + (semisecpart / 100);
                            if ((indata == "E") || (indata == "e") || (indata == "East") || (indata == "east"))
                                anvFodlseData.lonDir = GeogrDir.East;
                            else
                                anvFodlseData.lonDir = GeogrDir.West;
                        }
                        else
                        {
                            // Format: DDD.MM.SS [East|east|E|e]|[West|west|W|w]
                            anvFodlseData.lonVal = (degpart * 10000) + (minpart * 100) + secpart;
                            if ((indata == "E") || (indata == "e") || (indata == "East") || (indata == "east"))
                                anvFodlseData.lonDir = GeogrDir.East;
                            else
                                anvFodlseData.lonDir = GeogrDir.West;
                        }
                    }
                    else
                    {
                        // Format: DDD.MM [East|east|E|e]|[West|west|W|w]
                        anvFodlseData.lonVal = (degpart * 10000) + (minpart * 100);
                        if ((indata == "E") || (indata == "e") || (indata == "East") || (indata == "east"))
                            anvFodlseData.lonDir = GeogrDir.East;
                        else
                            anvFodlseData.lonDir = GeogrDir.West;
                    }
                }
                else
                {
                    // Format DDD [East|east|E|e]|[West|west|W|w]
                    anvFodlseData.lonVal = (degpart * 10000);
                    if ((indata == "E") || (indata == "e") || (indata == "East") || (indata == "east"))
                        anvFodlseData.lonDir = GeogrDir.East;
                    else
                        anvFodlseData.lonDir = GeogrDir.West;
                }
            }
            else
            {
                // Format: DDD MM SS ss [East|east|E|e]|[West|west|W|w]
                dp = indata.IndexOf(" ");
                if ((dp > 0) && (dp < indata.Length))
                {
                    int degpart = int.Parse(indata.Substring(0, dp));
                    indata = indata.Substring(dp + 1, indata.Length - dp - 1);
                    // Format: MM SS ss [East|east|E|e]|[West|west|W|w]
                    dp = indata.IndexOf(" ");
                    if ((dp > 0) && (dp < indata.Length))
                    {
                        int minpart = int.Parse(indata.Substring(0, dp));
                        indata = indata.Substring(dp + 1, indata.Length - dp - 1);
                        // Format: SS ss [East|east|E|e]|[West|west|W|w]
                        dp = indata.IndexOf(" ");
                        if ((dp > 0) && (dp < indata.Length))
                        {
                            int secpart = int.Parse(indata.Substring(0, dp));
                            indata = indata.Substring(dp + 1, indata.Length - dp - 1);
                            // Format: ss [East|east|E|e]|[West|west|W|w]
                            dp = indata.IndexOf(" ");
                            if ((dp > 0) && (dp < indata.Length))
                            {
                                int semisecpart = int.Parse(indata.Substring(0, dp));
                                indata = indata.Substring(dp + 1, indata.Length - dp - 1);
                                // Format: [East|east|E|e]|[West|west|W|w]
                                anvFodlseData.lonVal = (degpart * 10000) + (minpart * 100) + secpart + (semisecpart / 100);
                                if ((indata == "E") || (indata == "e") || (indata == "East") || (indata == "east"))
                                    anvFodlseData.lonDir = GeogrDir.East;
                                else
                                    anvFodlseData.lonDir = GeogrDir.West;
                            }
                            else
                            {
                                anvFodlseData.lonVal = (degpart * 10000) + (minpart * 100) + secpart;
                                if ((indata == "E") || (indata == "e") || (indata == "East") || (indata == "east"))
                                    anvFodlseData.lonDir = GeogrDir.East;
                                else
                                    anvFodlseData.lonDir = GeogrDir.West;
                            }
                        }
                        else
                        {
                            anvFodlseData.lonVal = (degpart * 10000) + (minpart * 100);
                            if ((indata == "E") || (indata == "e") || (indata == "East") || (indata == "east"))
                                anvFodlseData.lonDir = GeogrDir.East;
                            else
                                anvFodlseData.lonDir = GeogrDir.West;
                        }
                    }
                    else
                    {
                        // Format: DDD [East|east|E|e]|[West|west|W|w]
                        anvFodlseData.lonVal = (degpart * 10000);
                        if ((indata == "E") || (indata == "e") || (indata == "East") || (indata == "east"))
                            anvFodlseData.lonDir = GeogrDir.East;
                        else
                            anvFodlseData.lonDir = GeogrDir.West;
                    }
                }
            }
        }
        public void setUserBirthStreet(string instr) { anvFodlseData.brthStreet = instr; }
        public bool setUserBirthStreetNumber(string instr)
        {
            int intvarde = 0;
            if (int.TryParse(instr, out intvarde))
            {
                anvFodlseData.brthStreetNumber = intvarde;
                return true;
            }
            else
                return false;
        }
        public void setUserBirthStreetNumberAddon(string instr) { anvFodlseData.brthStreetNumberAddon = instr; }
        public void setUserBirthCityname(string instr) { anvFodlseData.brthCityname = instr; }
        public void setUserBirthAreaname(string instr) { anvFodlseData.brthAreaname = instr; }
        public void setUserBirthZipcode(string instr) { anvFodlseData.brthZipcode = instr; }
        public void setUserBirthCountry(string instr) { anvFodlseData.brthCountryname = instr; }
        public bool setUserBirthDate(string instr)
        {
            bool retVal = true;
            int dp0 = instr.IndexOf("-");
            if ((dp0 > 0) && (dp0 < instr.Length - 1))
            {
                // Have "-" in indata
                // Possible formats: [YYYY-MM-DD(10)|YY-MM-DD(8)|YYYY-MM(7)|YY-MM(5)]
                string yrstr = instr.Substring(0, dp0);
                instr = instr.Substring(dp0 + 1, instr.Length - dp0 - 1);
                dp0 = instr.IndexOf("-");
                if ((dp0 > 0) && (dp0 < instr.Length - 1))
                {
                    string mtstr = instr.Substring(0, dp0);
                    instr = instr.Substring(dp0 + 1, instr.Length - dp0 - 1);
                    dp0 = instr.IndexOf("-");
                    if ((dp0 > 0) && (dp0 < instr.Length - 1))
                    {
                        string dystr = instr.Substring(0, dp0);
                        anvFodlseData.brthDate.day = int.Parse(dystr);
                    }
                    else
                    {
                        anvFodlseData.brthDate.day = int.Parse(instr);
                    }
                    anvFodlseData.brthDate.month = int.Parse(mtstr);
                }
                anvFodlseData.brthDate.year = int.Parse(yrstr);
            }
            else
            {
                // Don't have "-" in indata
                if (instr.Length > 7)
                {
                    // Format YYYYMMDD
                    string yrstr = instr.Substring(0, 4);
                    string mtstr = instr.Substring(4, 2);
                    string dystr = instr.Substring(6, 2);
                    anvFodlseData.brthDate.year = int.Parse(yrstr);
                    anvFodlseData.brthDate.month = int.Parse(mtstr);
                    anvFodlseData.brthDate.day = int.Parse(dystr);
                }
                else if (instr.Length > 5)
                {
                    // Format YYMMDD
                    string yrstr = instr.Substring(0, 2);
                    string mtstr = instr.Substring(2, 2);
                    string dystr = instr.Substring(4, 2);
                    anvFodlseData.brthDate.year = int.Parse(yrstr) + 1900;
                    anvFodlseData.brthDate.month = int.Parse(mtstr);
                    anvFodlseData.brthDate.day = int.Parse(dystr);
                }
            }
            return retVal;
        }
        // --- Face data ---
        public int getNumberOfFaceData() { return noOfFaceData; }
        public string getFaceEyeWidth(int nr) { return anvAnsiktsData[nr].eyeWidth.ToString(); }
        public string getFaceCheekboneWidth(int nr) { return anvAnsiktsData[nr].cheekboneWidth.ToString(); }
        public string getFaceChinWidth(int nr) { return anvAnsiktsData[nr].chinWidth.ToString(); }
        // Height; Unit; Valid-Date
        public string getFaceMouthWidth(int nr) { return anvAnsiktsData[nr].mouthWidth.ToString(); }
        public string getFaceHeight(int nr) { return anvAnsiktsData[nr].height.ToString(); }
        public string getFaceUsedUnit(int nr) { return anvAnsiktsData[nr].units.ToString(); }
        public string getFaceInfoValidDate(int nr) { return anvAnsiktsData[nr].validDate.year.ToString() + "-" + anvAnsiktsData[nr].validDate.month.ToString() + "-" + anvAnsiktsData[nr].validDate.day.ToString(); }
        // --- Skin tones ---
        public int getNumberOfSkinTones() { return noOfComplexions; }
        public string getUserSkinToneValidDate(int nr) { return anvHudfarg[nr].validDate.year.ToString() + "-" + anvHudfarg[nr].validDate.month.ToString() + "-" + anvHudfarg[nr].validDate.day.ToString(); }
        public string getUserSkinToneTag(int nr)
        {
            if (anvHudfarg[nr].etnicType == EtnicityType.Asian)
                return "Asian";
            else if (anvHudfarg[nr].etnicType == EtnicityType.Cocation)
                return "Cocation";
            else if (anvHudfarg[nr].etnicType == EtnicityType.Indian)
                return "Indian";
            else if (anvHudfarg[nr].etnicType == EtnicityType.Innuit)
                return "Innuit";
            else if (anvHudfarg[nr].etnicType == EtnicityType.Mexican)
                return "Mexican";
            else if (anvHudfarg[nr].etnicType == EtnicityType.Mulatto)
                return "Mulatto";
            else if (anvHudfarg[nr].etnicType == EtnicityType.Negro)
                return "Negro";
            else
                return "Unknown";
        }
        public string getUserSkinToneRedChannel(int nr) { return anvHudfarg[nr].redChannelValue.ToString(); }
        public string getUserSkinToneGreenChannel(int nr) { return anvHudfarg[nr].greenChannelValue.ToString(); }
        public string getUserSkinToneBlueChannel(int nr) { return anvHudfarg[nr].blueChannelValue.ToString(); }
        public bool setUserSkinToneTag(string hf)
        {
            if ((hf == "Asian") || (hf == "asian"))
                anvHudfarg[noOfComplexions].etnicType = EtnicityType.Asian;
            else if ((hf == "Negro") || (hf == "negro") || (hf == "African") || (hf == "african"))
                anvHudfarg[noOfComplexions].etnicType = EtnicityType.Negro;
            else if ((hf == "Indian") || (hf == "indian"))
                anvHudfarg[noOfComplexions].etnicType = EtnicityType.Indian;
            else if ((hf == "Cocation") || (hf == "cocation") || (hf == "White") || (hf == "white"))
                anvHudfarg[noOfComplexions].etnicType = EtnicityType.Cocation;
            else if ((hf == "Mulatto") || (hf == "mulatto") || (hf == "Mixed") || (hf == "mixed"))
                anvHudfarg[noOfComplexions].etnicType = EtnicityType.Mulatto;
            else if ((hf == "Mexican") || (hf == "mexican") || (hf == "SouthAmerican") || (hf == "southamerican"))
                anvHudfarg[noOfComplexions].etnicType = EtnicityType.Mexican;
            else if ((hf == "Innuit") || (hf == "innuit"))
                anvHudfarg[noOfComplexions].etnicType = EtnicityType.Innuit;
            else
                anvHudfarg[noOfComplexions].etnicType = EtnicityType.UndefEtnicity;
            return true;
        }
        // --- Eye Color Data ---
        public int getNumberOfEyeData() { return noOfEyeColorData; }
        public string getUserEyeColorTag(int nr) { return anvEyes[nr].colorTag; }
        public string getUserEyeFormTag(int nr) { return anvEyes[nr].formTag; }
        public string getUserEyeDataValidDate(int nr) { return anvEyes[nr].validDate.year.ToString() + "-" + anvEyes[nr].validDate.month.ToString() + "-" + anvEyes[nr].validDate.day.ToString(); }
        public bool setUserEyeData(string clr, string frm, string dte)
        {
            bool retVal = false;
            if (noOfEyeColorData < maxNoOfEyeColorData)
            {
                anvEyes[noOfEyeColorData].colorTag = clr;
                anvEyes[noOfEyeColorData].formTag = frm;
                int dp0 = dte.IndexOf("-");
                if ((dp0 > 0) && (dp0 < dte.Length))
                {
                    if (dte.Length > 9)
                    {
                        // Format YYYY-MM-DD
                        string syr = dte.Substring(0, 4);
                        string smt = dte.Substring(5, 2);
                        string sdy = dte.Substring(8, 2);
                        anvEyes[noOfEyeColorData].validDate.year = int.Parse(syr);
                        anvEyes[noOfEyeColorData].validDate.month = int.Parse(smt);
                        anvEyes[noOfEyeColorData].validDate.day = int.Parse(sdy);
                        noOfEyeColorData++;
                        retVal = true;
                    }
                    else if (dte.Length > 7)
                    {
                        // Format YY-MM-DD
                        string syr = dte.Substring(0, 2);
                        string smt = dte.Substring(3, 2);
                        string sdy = dte.Substring(6, 2);
                        anvEyes[noOfEyeColorData].validDate.year = int.Parse(syr) + 1900;
                        anvEyes[noOfEyeColorData].validDate.month = int.Parse(smt);
                        anvEyes[noOfEyeColorData].validDate.day = int.Parse(sdy);
                        noOfEyeColorData++;
                        retVal = true;
                    }
                    else if (dte.Length > 6)
                    {
                        // Format YYYY-MM
                        string syr = dte.Substring(0, 4);
                        string smt = dte.Substring(5, 2);
                        anvEyes[noOfEyeColorData].validDate.year = int.Parse(syr);
                        anvEyes[noOfEyeColorData].validDate.month = int.Parse(smt);
                        noOfEyeColorData++;
                        retVal = true;
                    }
                    else if (dte.Length > 4)
                    {
                        // Format YY-MM
                        string syr = dte.Substring(0, 2);
                        string smt = dte.Substring(3, 2);
                        anvEyes[noOfEyeColorData].validDate.year = int.Parse(syr) + 1900;
                        anvEyes[noOfEyeColorData].validDate.month = int.Parse(smt);
                        noOfEyeColorData++;
                        retVal = true;
                    }
                }
                else
                {
                    if (dte.Length > 7)
                    {
                        // Format YYYYMMDD
                        string syr = dte.Substring(0, 4);
                        string smt = dte.Substring(4, 2);
                        string sdy = dte.Substring(6, 2);
                        anvEyes[noOfEyeColorData].validDate.year = int.Parse(syr);
                        anvEyes[noOfEyeColorData].validDate.month = int.Parse(smt);
                        anvEyes[noOfEyeColorData].validDate.day = int.Parse(sdy);
                        noOfEyeColorData++;
                        retVal = true;
                    }
                    else if (dte.Length > 5)
                    {
                        // Format YYMMDD
                        string syr = dte.Substring(0, 2);
                        string smt = dte.Substring(2, 2);
                        string sdy = dte.Substring(4, 2);
                        anvEyes[noOfEyeColorData].validDate.year = int.Parse(syr) + 1900;
                        anvEyes[noOfEyeColorData].validDate.month = int.Parse(smt);
                        anvEyes[noOfEyeColorData].validDate.day = int.Parse(sdy);
                        noOfEyeColorData++;
                        retVal = true;
                    }
                    else if (dte.Length > 3)
                    {
                        // Format YYYY
                        string syr = dte.Substring(0, 4);
                        anvEyes[noOfEyeColorData].validDate.year = int.Parse(syr);
                        noOfEyeColorData++;
                        retVal = true;
                    }
                }
            }
            return retVal;
        }
        // --- Gender Data ---
        public int getNumberOfGenderData() { return noOfGenderInfo; }
        public string getUserGenderType(int nr) { return anvKonsInfo[nr].type.ToString(); }
        public string getUserGenderLength(int nr) { return anvKonsInfo[nr].length.ToString() + " " + anvKonsInfo[nr].usedUnits.ToString(); }
        public string getUserGenderCircumf(int nr) { return anvKonsInfo[nr].circumference.ToString() + " " + anvKonsInfo[nr].usedUnits.ToString(); }
        public string getUserGenderUnit(int nr) { return anvKonsInfo[nr].usedUnits.ToString(); }
        public string getUserGenderAppearance(int nr) { return anvKonsInfo[nr].appearance.ToString(); }
        public string getUserGenderBehaviour(int nr) { return anvKonsInfo[nr].behaviour.ToString(); }
        public string getUserGenderPres(int nr) { return anvKonsInfo[nr].presentation; }
        public string getUserGenderInfoValidDate(int nr) 
        { 
            if ((anvKonsInfo[nr].validDate.month > 9) && (anvKonsInfo[nr].validDate.day > 9))
                return anvKonsInfo[nr].validDate.year.ToString() + "-" + anvKonsInfo[nr].validDate.month.ToString() + "-" + anvKonsInfo[nr].validDate.day.ToString(); 
            else if (anvKonsInfo[nr].validDate.month > 9)
                return anvKonsInfo[nr].validDate.year.ToString() + "-" + anvKonsInfo[nr].validDate.month.ToString() + "-0" + anvKonsInfo[nr].validDate.day.ToString();
            else if (anvKonsInfo[nr].validDate.day > 9)
                return anvKonsInfo[nr].validDate.year.ToString() + "-0" + anvKonsInfo[nr].validDate.month.ToString() + "-" + anvKonsInfo[nr].validDate.day.ToString();
            else
                return anvKonsInfo[nr].validDate.year.ToString() + "-0" + anvKonsInfo[nr].validDate.month.ToString() + "-0" + anvKonsInfo[nr].validDate.day.ToString();
        }
        // --- Length Data ---
        public int getNumberOfLengthData() { return noOfLengthData; }
        public string getUserLengthValue(int nr) { return anvLangdData[nr].value.ToString() + " " + anvLangdData[nr].unit.ToString(); }
        public string getUserLengthInfoValidDate(int nr) 
        { 
            if ((anvLangdData[nr].validDate.month > 9) && (anvLangdData[nr].validDate.day > 9))
                return anvLangdData[nr].validDate.year.ToString() + "-" + anvLangdData[nr].validDate.month.ToString() + "-" + anvLangdData[nr].validDate.day.ToString(); 
            else if (anvLangdData[nr].validDate.month > 9)
                return anvLangdData[nr].validDate.year.ToString() + "-" + anvLangdData[nr].validDate.month.ToString() + "-0" + anvLangdData[nr].validDate.day.ToString();
            else if (anvLangdData[nr].validDate.day > 9)
                return anvLangdData[nr].validDate.year.ToString() + "-0" + anvLangdData[nr].validDate.month.ToString() + "-" + anvLangdData[nr].validDate.day.ToString();
            else
                return anvLangdData[nr].validDate.year.ToString() + "-0" + anvLangdData[nr].validDate.month.ToString() + "-0" + anvLangdData[nr].validDate.day.ToString();
        }
        // --- Weight Data ---
        public int getNumberOfWeightData() { return noOfWeightData; }
        public string getUserWeightValue(int nr) { return anvViktData[nr].value.ToString() + " " + anvViktData[nr].unit.ToString(); }
        public string getUserWeightVal(int nr) { return anvViktData[nr].value.ToString(); }
        public string getUserWeightUnit(int nr) { return anvViktData[nr].unit.ToString(); }
        public string getUserWeightInfoValidDate(int nr) 
        { 
            if ((anvViktData[nr].validDate.month > 9) && (anvViktData[nr].validDate.day > 9))
                return anvViktData[nr].validDate.year.ToString() + "-" + anvViktData[nr].validDate.month.ToString() + "-" + anvViktData[nr].validDate.day.ToString(); 
            else if (anvViktData[nr].validDate.month > 9)
                return anvViktData[nr].validDate.year.ToString() + "-" + anvViktData[nr].validDate.month.ToString() + "-0" + anvViktData[nr].validDate.day.ToString();
            else if (anvViktData[nr].validDate.day > 9)
                return anvViktData[nr].validDate.year.ToString() + "-0" + anvViktData[nr].validDate.month.ToString() + "-" + anvViktData[nr].validDate.day.ToString();
            else
                return anvViktData[nr].validDate.year.ToString() + "-0" + anvViktData[nr].validDate.month.ToString() + "-0" + anvViktData[nr].validDate.day.ToString();
        }
        // --- Chest Data ---
        public int getNumberOfChestData() { return noOfChestData; }
        public string getUserChestType(int nr) { return anvBrostData[nr].type.ToString(); }
        public string getUserChestCircumfValue(int nr) { return anvBrostData[nr].circumference.ToString() + " " + anvBrostData[nr].units.ToString(); }
        public string getUserChestCircumfVal(int nr) { return anvBrostData[nr].circumference.ToString(); }
        public string getUserChestCircumfUnit(int nr) { return anvBrostData[nr].units.ToString(); }
        public string getUserChestSizeType(int nr) { return anvBrostData[nr].sizeType.ToString(); }
        public string getUserChestInfoValidDate(int nr) 
        { 
            if ((anvBrostData[nr].validDate.month > 9) && (anvBrostData[nr].validDate.day > 9))
                return anvBrostData[nr].validDate.year.ToString() + "-" + anvBrostData[nr].validDate.month.ToString() + "-" + anvBrostData[nr].validDate.day.ToString(); 
            else if (anvBrostData[nr].validDate.month > 9)
                return anvBrostData[nr].validDate.year.ToString() + "-" + anvBrostData[nr].validDate.month.ToString() + "-0" + anvBrostData[nr].validDate.day.ToString();
            else if (anvBrostData[nr].validDate.day > 9)
                return anvBrostData[nr].validDate.year.ToString() + "-0" + anvBrostData[nr].validDate.month.ToString() + "-" + anvBrostData[nr].validDate.day.ToString();
            else
                return anvBrostData[nr].validDate.year.ToString() + "-0" + anvBrostData[nr].validDate.month.ToString() + "-0" + anvBrostData[nr].validDate.day.ToString();
        }
        // --- Hair Data ---
        public int getNumberOfHairData() { return noOfHairData; }
        public string getUserHairColor(int nr) { return anvHaar[nr].hairColor.tag; }
        public string getUserHairTexture(int nr) { return anvHaar[nr].textureTag.ToString(); }
        public string getUserHairLength(int nr) { return anvHaar[nr].lengthTag.ToString(); }
        public string getUserHairValidDate(int nr) 
        { 
            if ((anvHaar[nr].validDate.month > 9) && (anvHaar[nr].validDate.day > 9))
                return anvHaar[nr].validDate.year.ToString() + "-" + anvHaar[nr].validDate.month.ToString() + "-" + anvHaar[nr].validDate.day.ToString(); 
            else if (anvHaar[nr].validDate.month > 9)
                return anvHaar[nr].validDate.year.ToString() + "-" + anvHaar[nr].validDate.month.ToString() + "-0" + anvHaar[nr].validDate.day.ToString();
            else if (anvHaar[nr].validDate.day > 9)
                return anvHaar[nr].validDate.year.ToString() + "-0" + anvHaar[nr].validDate.month.ToString() + "-" + anvHaar[nr].validDate.day.ToString();
            else
                return anvHaar[nr].validDate.year.ToString() + "-0" + anvHaar[nr].validDate.month.ToString() + "-0" + anvHaar[nr].validDate.day.ToString();
        }
        // --- Markings Data ---
        public int getNoOfMarkingsData() { return noOfMarkingData; }
        public string getActorMarkingType(int nr) { return anvMarkningar[nr].markTag.ToString(); }
        public string getActorMarkingPlace(int nr)
        {
            if (anvMarkningar[nr].placement != null)
                return anvMarkningar[nr].placement.ToString();
            else
                return "";
        }
        public string getActorMarkingMotif(int nr) { return anvMarkningar[nr].motif.ToString(); }
        public string getActorMarkingValidDate(int nr) 
        { 
            if ((anvMarkningar[nr].validDate.month > 9) && (anvMarkningar[nr].validDate.day > 9))
                return anvMarkningar[nr].validDate.year.ToString() + "-" + anvMarkningar[nr].validDate.month.ToString() + "-" + anvMarkningar[nr].validDate.day.ToString(); 
            else if (anvMarkningar[nr].validDate.month > 9)
                return anvMarkningar[nr].validDate.year.ToString() + "-" + anvMarkningar[nr].validDate.month.ToString() + "-0" + anvMarkningar[nr].validDate.day.ToString();
            else if (anvMarkningar[nr].validDate.day > 9)
                return anvMarkningar[nr].validDate.year.ToString() + "-0" + anvMarkningar[nr].validDate.month.ToString() + "-" + anvMarkningar[nr].validDate.day.ToString();
            else
                return anvMarkningar[nr].validDate.year.ToString() + "-0" + anvMarkningar[nr].validDate.month.ToString() + "-0" + anvMarkningar[nr].validDate.day.ToString();
        }
        // --- Occupation Data ---
        public int getNoOfOccupationsData() { return noOfOccupationData; }
        public string getActorOccupationTitle(int nr) { return anvAnstallningar[nr].title; }
        public string getActorOccupationCompany(int nr) { return anvAnstallningar[nr].company; }
        public string getActorOccupationStreetname(int nr) { return anvAnstallningar[nr].streetname + " " + anvAnstallningar[nr].number.ToString(); }
        public string getActorOccupationStrtNme(int nr) { return anvAnstallningar[nr].streetname; }
        public string getActorOccupationStrtNum(int nr) { return anvAnstallningar[nr].number.ToString(); }
        public string getActorOccupationStatename(int nr) { return anvAnstallningar[nr].statename; }
        public string getActorOccupationAreaname(int nr) { return anvAnstallningar[nr].areaname; }
        public string getActorOccupationZipcode(int nr) { return anvAnstallningar[nr].zipcode.ToString(); }
        public string getActorOccupationCountry(int nr) { return anvAnstallningar[nr].country; }
        public string getActorOccupationStarted(int nr)
        {
            if (anvAnstallningar[nr].started.year > 0)
            {
                if ((anvAnstallningar[nr].started.month > 9) && (anvAnstallningar[nr].started.day > 9))
                    return anvAnstallningar[nr].started.year.ToString() + "-" + anvAnstallningar[nr].started.month.ToString() + "-" + anvAnstallningar[nr].started.day.ToString();
                else if (anvAnstallningar[nr].started.month > 9)
                    return anvAnstallningar[nr].started.year.ToString() + "-" + anvAnstallningar[nr].started.month.ToString() + "-0" + anvAnstallningar[nr].started.day.ToString();
                else if (anvAnstallningar[nr].started.day > 9)
                    return anvAnstallningar[nr].started.year.ToString() + "-0" + anvAnstallningar[nr].started.month.ToString() + "-" + anvAnstallningar[nr].started.day.ToString();
                else if ((anvAnstallningar[nr].started.day > 0) && (anvAnstallningar[nr].started.month > 0))
                    return anvAnstallningar[nr].started.year.ToString() + "-0" + anvAnstallningar[nr].started.month.ToString() + "-0" + anvAnstallningar[nr].started.day.ToString();
                else if (anvAnstallningar[nr].started.month > 0)
                    return anvAnstallningar[nr].started.year.ToString() + "-0" + anvAnstallningar[nr].started.month.ToString();
                else
                    return anvAnstallningar[nr].started.year.ToString();
            }
            else
                return "";
        }
        public string getActorOccupationEnded(int nr) 
        {
            if (anvAnstallningar[nr].ended.year > 0)
            {
                if ((anvAnstallningar[nr].ended.month > 9) && (anvAnstallningar[nr].ended.day > 9))
                    return anvAnstallningar[nr].ended.year.ToString() + "-" + anvAnstallningar[nr].ended.month.ToString() + "-" + anvAnstallningar[nr].ended.day.ToString();
                else if (anvAnstallningar[nr].ended.month > 9)
                    return anvAnstallningar[nr].ended.year.ToString() + "-" + anvAnstallningar[nr].ended.month.ToString() + "-0" + anvAnstallningar[nr].ended.day.ToString();
                else if (anvAnstallningar[nr].ended.day > 9)
                    return anvAnstallningar[nr].ended.year.ToString() + "-0" + anvAnstallningar[nr].ended.month.ToString() + "-" + anvAnstallningar[nr].ended.day.ToString();
                else if ((anvAnstallningar[nr].ended.month > 0) && (anvAnstallningar[nr].ended.month > 0))
                    return anvAnstallningar[nr].ended.year.ToString() + "-0" + anvAnstallningar[nr].ended.month.ToString() + "-0" + anvAnstallningar[nr].ended.day.ToString();
                else if (anvAnstallningar[nr].ended.month > 0)
                    return anvAnstallningar[nr].ended.year.ToString() + "-0" + anvAnstallningar[nr].ended.month.ToString();
                else
                    return anvAnstallningar[nr].ended.year.ToString();

            }
            else
                return "";
        }
        public bool setActorOccupationData(string title, string company, string street, string state, string area, string zipcode, string country, string start, string end)
        {
            bool retVal = false;
            if (noOfOccupationData < maxNoOfOccupationData)
            {
                anvAnstallningar[noOfOccupationData].title = title;
                anvAnstallningar[noOfOccupationData].company = company;
                int dp0 = street.IndexOf(" ");
                if ((dp0 > 0) && (dp0 < street.Length))
                {
                    string strt = street.Substring(0, dp0);
                    anvAnstallningar[noOfOccupationData].streetname = strt;
                    string nmbr = street.Substring(dp0 + 1, street.Length - dp0 - 1);
                    anvAnstallningar[noOfOccupationData].number = int.Parse(nmbr);
                }
                else
                    anvAnstallningar[noOfOccupationData].streetname = street;
                anvAnstallningar[noOfOccupationData].statename = state;
                anvAnstallningar[noOfOccupationData].zipcode = int.Parse(zipcode);
                anvAnstallningar[noOfOccupationData].country = country;
                dp0 = start.IndexOf("-");
                if ((dp0 > 0) && (dp0 < start.Length))
                {
                    if (start.Length > 9)
                    {
                        // Format YYYY-MM-DD
                        string syr = start.Substring(0, 4);
                        string smt = start.Substring(5, 2);
                        string sdy = start.Substring(8, 2);
                        anvAnstallningar[noOfOccupationData].started.year = int.Parse(syr);
                        anvAnstallningar[noOfOccupationData].started.month = int.Parse(smt);
                        anvAnstallningar[noOfOccupationData].started.day = int.Parse(sdy);
                        retVal = true;
                    }
                    else if (start.Length > 7)
                    {
                        // Format YY-MM-DD
                        string syr = start.Substring(0, 2);
                        string smt = start.Substring(3, 2);
                        string sdy = start.Substring(6, 2);
                        anvAnstallningar[noOfOccupationData].started.year = int.Parse(syr) + 1900;
                        anvAnstallningar[noOfOccupationData].started.month = int.Parse(smt);
                        anvAnstallningar[noOfOccupationData].started.day = int.Parse(sdy);
                        retVal = true;
                    }
                    else if (start.Length > 4)
                    {
                        // Format YY-MM
                        string syr = start.Substring(0, 2);
                        string smt = start.Substring(3, 2);
                        anvAnstallningar[noOfOccupationData].started.year = int.Parse(syr) + 1900;
                        anvAnstallningar[noOfOccupationData].started.month = int.Parse(smt);
                        retVal = true;
                    }
                }
                else
                {
                    if (start.Length > 7)
                    {
                        // Format YYYYMMDD
                        string syr = start.Substring(0, 4);
                        string smt = start.Substring(4, 2);
                        string sdy = start.Substring(6, 2);
                        anvAnstallningar[noOfOccupationData].started.year = int.Parse(syr);
                        anvAnstallningar[noOfOccupationData].started.month = int.Parse(smt);
                        anvAnstallningar[noOfOccupationData].started.day = int.Parse(sdy);
                        retVal = true;
                    }
                    else if (start.Length > 5)
                    {
                        // Format YYMMDD
                        string syr = start.Substring(0, 2);
                        string smt = start.Substring(2, 2);
                        string sdy = start.Substring(4, 2);
                        anvAnstallningar[noOfOccupationData].started.year = int.Parse(syr) + 1900;
                        anvAnstallningar[noOfOccupationData].started.month = int.Parse(smt);
                        anvAnstallningar[noOfOccupationData].started.day = int.Parse(sdy);
                        retVal = true;
                    }
                    else if (start.Length > 3)
                    {
                        // Format YYYY
                        string syr = start.Substring(0, 4);
                        anvAnstallningar[noOfOccupationData].started.year = int.Parse(syr);
                        retVal = true;
                    }
                }
                dp0 = end.IndexOf("-");
                if ((dp0 > 0) && (dp0 < end.Length))
                {
                    if (end.Length > 9)
                    {
                        // Format YYYY-MM-DD
                        string syr = end.Substring(0, 4);
                        string smt = end.Substring(5, 2);
                        string sdy = end.Substring(8, 2);
                        anvAnstallningar[noOfOccupationData].ended.year = int.Parse(syr);
                        anvAnstallningar[noOfOccupationData].ended.month = int.Parse(smt);
                        anvAnstallningar[noOfOccupationData].ended.day = int.Parse(sdy);
                        retVal = true;
                    }
                    else if (end.Length > 7)
                    {
                        // Format YY-MM-DD
                        string syr = end.Substring(0, 2);
                        string smt = end.Substring(3, 2);
                        string sdy = end.Substring(6, 2);
                        anvAnstallningar[noOfOccupationData].ended.year = int.Parse(syr) + 1900;
                        anvAnstallningar[noOfOccupationData].ended.month = int.Parse(smt);
                        anvAnstallningar[noOfOccupationData].ended.day = int.Parse(sdy);
                        retVal = true;
                    }
                    else if (end.Length > 4)
                    {
                        // Format YY-MM
                        string syr = start.Substring(0, 2);
                        string smt = start.Substring(3, 2);
                        anvAnstallningar[noOfOccupationData].ended.year = int.Parse(syr) + 1900;
                        anvAnstallningar[noOfOccupationData].ended.month = int.Parse(smt);
                        retVal = true;
                    }
                }
                else
                {
                    if (end.Length > 7)
                    {
                        // Format YYYYMMDD
                        string syr = end.Substring(0, 4);
                        string smt = end.Substring(4, 2);
                        string sdy = end.Substring(6, 2);
                        anvAnstallningar[noOfOccupationData].ended.year = int.Parse(syr);
                        anvAnstallningar[noOfOccupationData].ended.month = int.Parse(smt);
                        anvAnstallningar[noOfOccupationData].ended.day = int.Parse(sdy);
                        retVal = true;
                    }
                    else if (end.Length > 5)
                    {
                        // Format YYMMDD
                        string syr = end.Substring(0, 2);
                        string smt = end.Substring(2, 2);
                        string sdy = end.Substring(4, 2);
                        anvAnstallningar[noOfOccupationData].ended.year = int.Parse(syr) + 1900;
                        anvAnstallningar[noOfOccupationData].ended.month = int.Parse(smt);
                        anvAnstallningar[noOfOccupationData].ended.day = int.Parse(sdy);
                        retVal = true;
                    }
                    else if (end.Length > 3)
                    {
                        // Format YYYY
                        string syr = end.Substring(0, 4);
                        anvAnstallningar[noOfOccupationData].ended.year = int.Parse(syr);
                        retVal = true;
                    }
                }
                noOfOccupationData++;
                retVal = true;
            }
            return retVal;
        }
        // --- Residence Data ---
        public int getNoOfResicenceData() { return noOfResidences; }
        public string getActorResidStreetname(int nr) { return anvBostader[nr].streetname + " " + anvBostader[nr].number.ToString() + anvBostader[nr].additive; }
        public string getActorResidStrtNme(int nr) { return anvBostader[nr].streetname; }
        public string getActorResidStrtNo(int nr) { return anvBostader[nr].number.ToString(); }
        public string getActorResidStrtNoAdd(int nr) { return anvBostader[nr].additive; }
        public string getActorResidCity(int nr) { return anvBostader[nr].city; }
        public string getActorResidArea(int nr) { return anvBostader[nr].area; }
        public string getActorResidZipcode(int nr) { return anvBostader[nr].zipcode.ToString(); }
        public string getActorResidCountry(int nr) { return anvBostader[nr].country; }
        public string getActorResidBought(int nr) 
        {
            if (anvBostader[nr].boughtDate.year > 0)
            {
                if ((anvBostader[nr].boughtDate.month > 9) && (anvBostader[nr].boughtDate.day > 9))
                    return anvBostader[nr].boughtDate.year.ToString() + "-" + anvBostader[nr].boughtDate.month.ToString() + "-" + anvBostader[nr].boughtDate.day.ToString();
                else if (anvBostader[nr].boughtDate.month > 9)
                    return anvBostader[nr].boughtDate.year.ToString() + "-" + anvBostader[nr].boughtDate.month.ToString() + "-0" + anvBostader[nr].boughtDate.day.ToString();
                else if (anvBostader[nr].boughtDate.day > 9)
                    return anvBostader[nr].boughtDate.year.ToString() + "-0" + anvBostader[nr].boughtDate.month.ToString() + "-" + anvBostader[nr].boughtDate.day.ToString();
                else if ((anvBostader[nr].boughtDate.month > 0) && (anvBostader[nr].boughtDate.day > 0))
                    return anvBostader[nr].boughtDate.year.ToString() + "-0" + anvBostader[nr].boughtDate.month.ToString() + "-0" + anvBostader[nr].boughtDate.day.ToString();
                else if (anvBostader[nr].boughtDate.month > 0)
                    return anvBostader[nr].boughtDate.year.ToString() + "-0" + anvBostader[nr].boughtDate.month.ToString();
                else
                    return anvBostader[nr].boughtDate.year.ToString();
            }
            else
                return "";
        }
        public string getActorResidSold(int nr) 
        { 
            if ((anvBostader[nr].salesDate.month > 9) && (anvBostader[nr].salesDate.day > 9))
                return anvBostader[nr].salesDate.year.ToString() + "-" + anvBostader[nr].salesDate.month.ToString() + "-" + anvBostader[nr].salesDate.day.ToString();
            else if (anvBostader[nr].salesDate.month > 9)
                return anvBostader[nr].salesDate.year.ToString() + "-" + anvBostader[nr].salesDate.month.ToString() + "-0" + anvBostader[nr].salesDate.day.ToString();
            else if (anvBostader[nr].salesDate.day > 9)
                return anvBostader[nr].salesDate.year.ToString() + "-0" + anvBostader[nr].salesDate.month.ToString() + "-" + anvBostader[nr].salesDate.day.ToString();
            else if ((anvBostader[nr].salesDate.month > 0) && (anvBostader[nr].salesDate.day > 0))
                return anvBostader[nr].salesDate.year.ToString() + "-0" + anvBostader[nr].salesDate.month.ToString() + "-0" + anvBostader[nr].salesDate.day.ToString();
            else if (anvBostader[nr].salesDate.month > 0)
                return anvBostader[nr].salesDate.year.ToString() + "-0" + anvBostader[nr].salesDate.month.ToString();
            else
                return anvBostader[nr].salesDate.year.ToString();
        }
        public string getActorResidBoughtValue(int nr) { return anvBostader[nr].boughtValue.ToString() + " " + anvBostader[nr].usedCurrency.tag; }
        public string getActorResidBoughtVal(int nr) { return anvBostader[nr].boughtValue.ToString(); }
        public string getActorResidSalesValue(int nr) { return anvBostader[nr].salesValue.ToString() + " " + anvBostader[nr].usedCurrency.tag; }
        public string getActorResidSalesVal(int nr) { return anvBostader[nr].salesValue.ToString(); }
        public string getActorResidCurrency(int nr) { return anvBostader[nr].usedCurrency.tag; }
        // --- Attended Events Data ---
        public int getNoOfAttendedEventsData() { return noOfAttendedEventData; }
        public string getActorAttendedEventID(int nr) { return anvTillstallningar[nr].eventID; }
        public string getActorAttendedEventType(int nr) { return anvTillstallningar[nr].eventCategory; }//anvTillstallningar[nr].type.ToString(); }
        public string getActorAttendedEventStarted(int nr) 
        { 
            if ((anvTillstallningar[nr].started.month > 9) && (anvTillstallningar[nr].started.day > 9))
                return anvTillstallningar[nr].started.year.ToString() + "-" + anvTillstallningar[nr].started.month.ToString() + "-" + anvTillstallningar[nr].started.day.ToString(); 
            else if (anvTillstallningar[nr].started.day > 9)
                return anvTillstallningar[nr].started.year.ToString() + "-0" + anvTillstallningar[nr].started.month.ToString() + "-" + anvTillstallningar[nr].started.day.ToString();
            else if (anvTillstallningar[nr].started.month > 9)
                return anvTillstallningar[nr].started.year.ToString() + "-" + anvTillstallningar[nr].started.month.ToString() + "-0" + anvTillstallningar[nr].started.day.ToString();
            else
                return anvTillstallningar[nr].started.year.ToString() + "-0" + anvTillstallningar[nr].started.month.ToString() + "-0" + anvTillstallningar[nr].started.day.ToString();
        }
        public string getActorAttendedEventEnded(int nr)
        { 
            if ((anvTillstallningar[nr].ended.month > 9) && (anvTillstallningar[nr].ended.day > 9))
                return anvTillstallningar[nr].ended.year.ToString() + "-" + anvTillstallningar[nr].ended.month.ToString() + "-" + anvTillstallningar[nr].ended.day.ToString(); 
            else if (anvTillstallningar[nr].ended.day > 9)
                return anvTillstallningar[nr].ended.year.ToString() + "-0" + anvTillstallningar[nr].ended.month.ToString() + "-" + anvTillstallningar[nr].ended.day.ToString();
            else if (anvTillstallningar[nr].ended.month > 9)
                return anvTillstallningar[nr].ended.year.ToString() + "-" + anvTillstallningar[nr].ended.month.ToString() + "-0" + anvTillstallningar[nr].ended.day.ToString();
            else
                return anvTillstallningar[nr].ended.year.ToString() + "-0" + anvTillstallningar[nr].ended.month.ToString() + "-0" + anvTillstallningar[nr].ended.day.ToString();
        }
        public string getActorAttendedEventRoleTag(int nr) { return anvTillstallningar[nr].role.tag; }
        public string getActorAttendedEventRoleDescription(int nr) { return anvTillstallningar[nr].role.description; }
        public int getActorAttendedEventRoleLevel(int nr) { return anvTillstallningar[nr].role.level; }
        public bool setActorAttendedEventData(string id, string typ, string start, string ended, string roleTag)
        {
            bool retVal = false;
            if (noOfAttendedEventData < maxNoOfAttendedEventData)
            {
                anvTillstallningar[noOfAttendedEventData].eventID = id;
                bool foundCategory = false;
                for (int i = 0; i < noOfEventCategories; i++)
                {
                    if (typ == eventCategories[i].tag)
                    {
                        anvTillstallningar[noOfAttendedEventData].eventCategory = eventCategories[i].tag;
                        foundCategory = true;
                    }
                }
                if (!(foundCategory))
                {
                    addEventCategory(typ, "No description", "Undefined");
                    anvTillstallningar[noOfAttendedEventData].eventType.tag = typ;
                }
                int dp0 = start.IndexOf("-");
                if ((dp0 > 0) && (dp0 < start.Length))
                {
                    // We have "-" delimeters
                    if (start.Length > 9)
                    {
                        // Format YYYY-MM-DD
                        string yrstr = start.Substring(0, 4);
                        string mntstr = start.Substring(5, 2);
                        string daystr = start.Substring(8, 2);
                        anvTillstallningar[noOfAttendedEventData].started.year = int.Parse(yrstr);
                        anvTillstallningar[noOfAttendedEventData].started.month = int.Parse(mntstr);
                        anvTillstallningar[noOfAttendedEventData].started.day = int.Parse(daystr);
                    }
                    else if (start.Length > 7)
                    {
                        // Format YY-MM-DD
                        string yrstr = start.Substring(0, 2);
                        string mntstr = start.Substring(3, 2);
                        string daystr = start.Substring(6, 2);
                        anvTillstallningar[noOfAttendedEventData].started.year = int.Parse(yrstr) + 1900;
                        anvTillstallningar[noOfAttendedEventData].started.month = int.Parse(mntstr);
                        anvTillstallningar[noOfAttendedEventData].started.day = int.Parse(daystr);
                    }
                    else if (start.Length > 6)
                    {
                        // Format YYYY-MM
                        string yrstr = start.Substring(0, 4);
                        string mntstr = start.Substring(5, 2);
                        anvTillstallningar[noOfAttendedEventData].started.year = int.Parse(yrstr);
                        anvTillstallningar[noOfAttendedEventData].started.month = int.Parse(mntstr);
                    }
                }
                else
                {
                    // We don't have a delimiter
                    if (start.Length > 7)
                    {
                        // Format YYYYMMDD
                        string yrstr = start.Substring(0, 4);
                        string mntstr = start.Substring(4, 2);
                        string daystr = start.Substring(6, 2);
                        anvTillstallningar[noOfAttendedEventData].started.year = int.Parse(yrstr);
                        anvTillstallningar[noOfAttendedEventData].started.month = int.Parse(mntstr);
                        anvTillstallningar[noOfAttendedEventData].started.day = int.Parse(daystr);
                    }
                    else if (start.Length > 5)
                    {
                        // Format YYMMDD
                        string yrstr = start.Substring(0, 2);
                        string mntstr = start.Substring(2, 2);
                        string daystr = start.Substring(4, 2);
                        anvTillstallningar[noOfAttendedEventData].started.year = int.Parse(yrstr) + 1900;
                        anvTillstallningar[noOfAttendedEventData].started.month = int.Parse(mntstr);
                        anvTillstallningar[noOfAttendedEventData].started.day = int.Parse(daystr);
                    }
                    else if (start.Length > 3)
                    {
                        // Format YYYY
                        string yrstr = start.Substring(0, 4);
                        anvTillstallningar[noOfAttendedEventData].started.year = int.Parse(yrstr);
                    }
                }
                dp0 = ended.IndexOf("-");
                if ((dp0 > 0) && (dp0 < ended.Length))
                {
                    // We have "-" delimeters
                    if (ended.Length > 9)
                    {
                        // Format YYYY-MM-DD
                        string yrstr = ended.Substring(0, 4);
                        string mntstr = ended.Substring(5, 2);
                        string daystr = ended.Substring(8, 2);
                        anvTillstallningar[noOfAttendedEventData].ended.year = int.Parse(yrstr);
                        anvTillstallningar[noOfAttendedEventData].ended.month = int.Parse(mntstr);
                        anvTillstallningar[noOfAttendedEventData].ended.day = int.Parse(daystr);
                    }
                    else if (ended.Length > 7)
                    {
                        // Format YY-MM-DD
                        string yrstr = ended.Substring(0, 2);
                        string mntstr = ended.Substring(3, 2);
                        string daystr = ended.Substring(6, 2);
                        anvTillstallningar[noOfAttendedEventData].ended.year = int.Parse(yrstr) + 1900;
                        anvTillstallningar[noOfAttendedEventData].ended.month = int.Parse(mntstr);
                        anvTillstallningar[noOfAttendedEventData].ended.day = int.Parse(daystr);
                    }
                    else if (ended.Length > 6)
                    {
                        // Format YYYY-MM
                        string yrstr = ended.Substring(0, 4);
                        string mntstr = ended.Substring(5, 2);
                        anvTillstallningar[noOfAttendedEventData].ended.year = int.Parse(yrstr);
                        anvTillstallningar[noOfAttendedEventData].ended.month = int.Parse(mntstr);
                    }
                }
                else
                {
                    // We don't have a delimiter
                    if (ended.Length > 7)
                    {
                        // Format YYYYMMDD
                        string yrstr = ended.Substring(0, 4);
                        string mntstr = ended.Substring(4, 2);
                        string daystr = ended.Substring(6, 2);
                        anvTillstallningar[noOfAttendedEventData].ended.year = int.Parse(yrstr);
                        anvTillstallningar[noOfAttendedEventData].ended.month = int.Parse(mntstr);
                        anvTillstallningar[noOfAttendedEventData].ended.day = int.Parse(daystr);
                    }
                    else if (ended.Length > 5)
                    {
                        // Format YYMMDD
                        string yrstr = ended.Substring(0, 2);
                        string mntstr = ended.Substring(2, 2);
                        string daystr = ended.Substring(4, 2);
                        anvTillstallningar[noOfAttendedEventData].ended.year = int.Parse(yrstr) + 1900;
                        anvTillstallningar[noOfAttendedEventData].ended.month = int.Parse(mntstr);
                        anvTillstallningar[noOfAttendedEventData].ended.day = int.Parse(daystr);
                    }
                    else if (ended.Length > 3)
                    {
                        // Format YYYY
                        string yrstr = ended.Substring(0, 4);
                        anvTillstallningar[noOfAttendedEventData].ended.year = int.Parse(yrstr);
                    }
                }
                bool foundRoleCategory = false;
                for (int i = 0; i < noOfRoleCategories; i++)
                { 
                    if (roleTag == roleCategories[i].tag)
                    {
                        foundRoleCategory = true;
                        anvTillstallningar[noOfAttendedEventData].role = roleCategories[i];
                    }
                }
                if ((!(foundRoleCategory)) && (noOfRoleCategories < maxNoOfRoleCategories))
                {
                    anvTillstallningar[noOfAttendedEventData].role.tag = roleTag;
                    anvTillstallningar[noOfAttendedEventData].role.description = "";
                    anvTillstallningar[noOfAttendedEventData].role.level = 0;
                    addRoleCategory(roleTag, "", "Undefined");
                }
                retVal = true;
                noOfAttendedEventData++;
            }
            return retVal;
        }
        // --- Related Images Data ---
        public int getNoOfRelatedImages() { return noOfRelatedImagesData; }
        public string getActorRelatedImagePath(int nr) { return anvRelBilder[nr].imagePathName; }
        public string getActorRelatedImageContent(int nr) { return anvRelBilder[nr].imageContext.tag; }
        public string getActorRelatedImageClass(int nr) { return anvRelBilder[nr].classificationLevel.ToString(); }
        public int getActorRelatedImageClassValue(int nr)
        {
            //UndefLevel, Unclassified, Limited, Confidential, Secret, QualifSecret
            if (anvRelBilder[nr].classificationLevel == ClassificationType.Unclassified)
                return 1;
            else if (anvRelBilder[nr].classificationLevel == ClassificationType.Limited)
                return 2;
            else if (anvRelBilder[nr].classificationLevel == ClassificationType.Confidential)
                return 4;
            else if (anvRelBilder[nr].classificationLevel == ClassificationType.Secret)
                return 5;
            else if (anvRelBilder[nr].classificationLevel == ClassificationType.QualifSecret)
                return 6;
            else
                return 0;
        }
        public bool addActorRelatedImage(string impan, string imctxc, string imlvl)
        {
            bool retVal = false;
            if (noOfRelatedImagesData < maxNoOfRelatedImagesData)
            {
                anvRelBilder[noOfRelatedImagesData].imagePathName = impan;
                bool foundCategory = false;
                for (int i = 0; i < noOfContextCategories; i++)
                {
                    if (imctxc == contextCategories[i].tag)
                    {
                        anvRelBilder[noOfRelatedImagesData].imageContext = contextCategories[i];
                        foundCategory = true;
                    }
                }
                if (!(foundCategory))
                {
                    addContextCategory(imctxc, "No description", imlvl);
                    anvRelBilder[noOfRelatedImagesData].imageContext.tag = imctxc;
                }
                // UndefLevel, Unclassified, Limited, Confidential,     Secret, QualifSecret
                // Undefined,  Open,         Limited, Medium, Relative, Secret, QualifSecret
                if ((imlvl == "Unclassified") || (imlvl == "Open"))
                    anvRelBilder[noOfRelatedImagesData].classificationLevel = ClassificationType.Unclassified;
                else if (imlvl == "Limited")
                    anvRelBilder[noOfRelatedImagesData].classificationLevel = ClassificationType.Limited;
                else if ((imlvl == "Confidential") || (imlvl == "Medium") || (imlvl == "Relative"))
                    anvRelBilder[noOfRelatedImagesData].classificationLevel = ClassificationType.Confidential;
                else if (imlvl == "Secret")
                    anvRelBilder[noOfRelatedImagesData].classificationLevel = ClassificationType.Secret;
                else if (imlvl == "QualifSecret")
                    anvRelBilder[noOfRelatedImagesData].classificationLevel = ClassificationType.QualifSecret;
                else
                    anvRelBilder[noOfRelatedImagesData].classificationLevel = ClassificationType.UndefLevel;
                noOfRelatedImagesData++;
                retVal = true;
            }
            return retVal;
        }
        // --- Nationality ---
        public int getNoOfNationalityData() { return noOfNationalityData; }
        public string getNationalityData(int nr) { return nationality[nr]; }
        public bool setNationalityData(string indata)
        {
            bool retVal = false;
            if (noOfNationalityData < maxNoOfNationalityData)
            {
                nationality[noOfNationalityData] = indata;
                noOfNationalityData++;
                retVal = true;
            }
            return retVal;
        }
        // ---------------------------
    }
}
