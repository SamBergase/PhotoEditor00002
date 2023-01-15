using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    public enum CurrencyUnits
    {
        UndefCurrencyUnit, GBP, USD, MNT, EUR, SEK
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
    public enum ContactType
    {
        UndefContactType, Phone, Email, Website, Facebook, Twitter, LinkedIn, Instagram, Snapchat
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
    public enum HairColorType
    {
        UndefHairColor, Black, DarkBrown, Brown, LightBrown, DarkChestnut, Chestnut, LightChestnut,
        DarkRed, Red, LightRed, DarkGray, Gray, LightGray, Green, Blue, Pink, Lilac, Orange, MixedColor
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
    // TODO - The "RelationType" should be fetched from the user profile data.
    public enum RelationType
    {
        UndefRelationType, Mother, Father, Sister, Brother, Son, Daughter, Single, Child, Friend, Girlfriend, Boyfriend, Engagee, 
        WorkFriend, Associate, Spouse, Partner, Fiancee, Married, Divorced, Widow, Lover, Master, Slave, Bull, cuckold, Widower
    }
    public enum EyeColorType
    {
        UndefEyeColor, Black, Brown, Blue, Green, Gray, Yellow
    }
    public enum MarkingType
    {
        UndefMarking, Scar, Freckles, Birthmark, Tattoo, Piercing
    }
    // TODO - The "EventType" should be fetched from the user profile data.
    public enum EventType
    {
        UndefEvent, Party, PhotoSession, Picnic
    }
    // TODO - The "RoleType" should be fetched from the user profile data.
    public enum RoleType
    {
        UndefRole, Attender, Organizer, Responsible, Producer, Acting, Slave, Master, Cuckold, Bull, Boss
    }
    // TODO - The "MotifType" should be fetched from the user profile data.
    public enum MotifType
    {
        UndefMotif, Nature, Portrait, Architecture, Proof, Family, Vacation, Vehicles, Erotic, Pornography
    }
    public enum ClassificationType
    {
        UndefLevel, Unlassified, Limited, Confidential, Secret, QualifSecret
    }
    public enum NameType
    {
        UndefNameType, Birth, Taken, Married, Alias, Nickname
    }
    public enum GeogrDir
    {
        UndefGeoDir, North, East, South, West
    }
    // TODO - The "ContextType" should be fetched from the user profile data.
    public enum ContextType
    {
        UndefContext, Portrait, Architecture, Nature, Transportations, Garden, Erotic, Porn, HardCorePorn
    }
    #endregion
    #region struct-definitions
    public struct userNames
    {
        public NameType nameType;
        public string Surname;
        public string Midname;
        public string Famname;
    }
    public struct userContacts
    {
        public ContactType contactType;
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
        public DateTime brthDate;
        public string brthSecurityCode;
        public GenderType brthGender;
    }
    public struct userComplexion
    {
        public EtnicityType etnicType;
        public int redChannelValue;
        public int greenChannelValue;
        public int blueChannelValue;
        public DateTime validDate;
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
        public DateTime validDate;
    }
    public struct userLengthData
    {
        public float value;
        public DistUnits unit;
        public DateTime validDate;
    }
    public struct userWeightData
    {
        public float value;
        public WeightUnits unit;
        public DateTime validDate;
    }
    public struct userChestData
    {
        public BreastType type;
        public float circumference;
        public DistUnits units;
        public BreastSizeType sizeType;
        public DateTime validDate;
    }
    public struct userFaceData
    {
        public float eyeWidth;
        public float cheekboneWidth;
        public float chinWidth;
        public float mouthWidth;
        public float height;
        public DistUnits units;
        public DateTime validDate;
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
        public DateTime boughtDate;
        public DateTime salesDate;
        public float boughtValue;
        public float salesValue;
        public CurrencyUnits currency;
    }
    public struct userHairColorData
    {
        public HairColorType colorTag;
        public HairTextureType textureTag;
        public HairLengthType lengthTag;
        public DateTime validDate;
    }
    public struct userMarkingData
    {
        public MarkingType markTag;
        public string placement;
        public string motif;
        public DateTime validDate;
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
        public DateTime started;
        public DateTime ended;
    }
    public struct userAttendedEventData
    {
        public string eventID;
        public EventType type;
        public DateTime started;
        public DateTime ended;
    }
    public struct userRelatedImages
    {
        public string imagePathName;
        public ContextType contentClassification;
        public ClassificationType classificationLevel;
    }
    #endregion
    class ActorClass
    {
        private string userId;

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

        public RelationType anvNuvarandeRelation;

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

        public string nationality;

        public int noOfDataSet = 0;
        public void loadActor(string userID)
        {
            // TODO - The actor data storage should be changed
            // string rootPath = "C:\\Users\\sambe\\source\\repos\\PhotoEditor00002\\PhotoEditor00002";
            string rootPath = "C:\\Users\\esbberg\\source\\repos\\PhotoEditor00002\\PhotoEditor00002";
            string filename = rootPath + "\\ActorData\\ActorData_" + userID + ".acf";
            if (System.IO.File.Exists(filename))
            {
                foreach (string line in System.IO.File.ReadLines(filename))
                {
                    if (line != "-1")
                    {
                        int dp0, dp1, dp2, dp3, dp4, dp5, dp6, dp7, dp8, dp9;
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
                                            string cttt = dataInfo.Substring(0, dp0);
                                            if (cttt == "Email")
                                                anvKontakter[noOfContacts].contactType = ContactType.Email;
                                            else if (cttt == "Phone")
                                                anvKontakter[noOfContacts].contactType = ContactType.Phone;
                                            else if (cttt == "Website")
                                                anvKontakter[noOfContacts].contactType = ContactType.Website;
                                            else if (cttt == "Facebook")
                                                anvKontakter[noOfContacts].contactType = ContactType.Facebook;
                                            else if (cttt == "Twitter")
                                                anvKontakter[noOfContacts].contactType = ContactType.Twitter;
                                            else if (cttt == "LinkedIn")
                                                anvKontakter[noOfContacts].contactType = ContactType.LinkedIn;
                                            else if (cttt == "Instagram")
                                                anvKontakter[noOfContacts].contactType = ContactType.Instagram;
                                            else if (cttt == "Snapchat")
                                                anvKontakter[noOfContacts].contactType = ContactType.Snapchat;
                                            else
                                                anvKontakter[noOfContacts].contactType = ContactType.UndefContactType;
                                            noOfDataSet++;
                                            anvKontakter[noOfContacts].contactPath = dataInfo.Substring(dp0 + 2, dataInfo.Length - dp0 - 2);
                                            noOfDataSet++;
                                        }
                                        else
                                        {
                                            anvKontakter[noOfContacts].contactType = ContactType.UndefContactType;
                                            noOfDataSet++;
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
                                                anvFodlseData.brthStreetNumber = Int32.Parse(tmpString);
                                                noOfDataSet++;
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
                                                                    if ((tmpString != "Unknown") && (tmpString != "Undefined"))
                                                                    {
                                                                        int sdp1 = tmpString.IndexOf("-");
                                                                        string yearString = tmpString.Substring(0, sdp1);
                                                                        anvFodlseData.brthDate.AddYears(Int32.Parse(yearString));
                                                                        tmpString = tmpString.Substring(sdp1 + 1, tmpString.Length - sdp1 - 1);
                                                                        int sdp2 = tmpString.IndexOf("-");
                                                                        string monthString = tmpString.Substring(0, sdp2);
                                                                        anvFodlseData.brthDate.AddMonths(Int32.Parse(monthString));
                                                                        tmpString = tmpString.Substring(sdp2 + 1, tmpString.Length - sdp2 - 1);
                                                                        anvFodlseData.brthDate.AddDays(Int32.Parse(tmpString));
                                                                        noOfDataSet++;
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
                                                                        // Gender
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
                                                anvHudfarg[noOfComplexions].redChannelValue = Int32.Parse(dataInfo.Substring(0, dp1));
                                                noOfDataSet++;
                                                dataInfo = dataInfo.Substring(dp1 + 2, dataInfo.Length - dp1 - 2);
                                                dp2 = dataInfo.IndexOf(";");
                                                if ((dp2 > 0) && (dp2 < dataInfo.Length - 2))
                                                {
                                                    // G-value; B-value; Valid-Date
                                                    anvHudfarg[noOfComplexions].greenChannelValue = Int32.Parse(dataInfo.Substring(0, dp2));
                                                    noOfDataSet++;
                                                    dataInfo = dataInfo.Substring(dp2 + 2, dataInfo.Length - dp2 - 2);
                                                    dp3 = dataInfo.IndexOf(";");
                                                    if ((dp3 > 0) && (dp3 < dataInfo.Length - 2))
                                                    {
                                                        // B-value; Valid-Date
                                                        anvHudfarg[noOfComplexions].blueChannelValue = Int32.Parse(dataInfo.Substring(0, dp3));
                                                        noOfDataSet++;
                                                        dataInfo = dataInfo.Substring(dp3 + 2, dataInfo.Length - dp3 - 2);
                                                        // Valid-Date [YYYY-MM-DD|YY-MM-DD|YYYY-MM|YY-MM|YYYY|MM]
                                                        dp5 = dataInfo.IndexOf("-");
                                                        if ((dp5 > 0) && (dp5 < dataInfo.Length - 1))
                                                        {
                                                            // Valid-Date [YYYY-MM-DD|YY-MM-DD|YYYY-MM|YY-MM]
                                                            string yrval = dataInfo.Substring(0, dp5);
                                                            anvHudfarg[noOfComplexions].validDate.AddYears(Int32.Parse(yrval));
                                                            noOfDataSet++;
                                                            dataInfo = dataInfo.Substring(dp5 + 1, dataInfo.Length - dp5 - 1);
                                                            dp6 = dataInfo.IndexOf("-");
                                                            if ((dp6 > 0) && (dp6 < dataInfo.Length - 1))
                                                            {
                                                                // Valid-Date [MM-DD]
                                                                string mntval = dataInfo.Substring(0, dp6);
                                                                anvHudfarg[noOfComplexions].validDate.AddMonths(Int32.Parse(mntval));
                                                                noOfDataSet++;
                                                                string dayval = dataInfo.Substring(dp6 + 1, dataInfo.Length - dp6 - 1);
                                                                anvHudfarg[noOfComplexions].validDate.AddDays(Int32.Parse(dayval));
                                                                noOfDataSet++;
                                                            }
                                                            else
                                                            {
                                                                // Valid-Date [MM|MM]
                                                                string mntval = dataInfo.Substring(0, dp6);
                                                                anvHudfarg[noOfComplexions].validDate.AddMonths(Int32.Parse(mntval));
                                                                noOfDataSet++;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            // Valid-Date [YYYY|YY]
                                                            string yrval = dataInfo.Substring(0, dp5);
                                                            anvHudfarg[noOfComplexions].validDate.AddYears(Int32.Parse(yrval));
                                                            noOfDataSet++;
                                                        }
                                                    }
                                                }
                                            }
                                        }
//                                        anvHudfarg
                                    }
                                } break;
                            case "RelStats":
                                {
                                    // RelStats : Status
                                    if ((dataInfo == "Single") || (dataInfo == "single"))
                                        anvNuvarandeRelation = RelationType.Single;
                                    else if ((dataInfo == "Girlfriend") || (dataInfo == "girlfriend"))
                                        anvNuvarandeRelation = RelationType.Boyfriend;
                                    else if ((dataInfo == "Engagee") || (dataInfo == "engagee"))
                                        anvNuvarandeRelation = RelationType.Engagee;
                                    else if ((dataInfo == "Spouse") || (dataInfo == "spouse"))
                                        anvNuvarandeRelation = RelationType.Spouse;
                                    else if ((dataInfo == "Partner") || (dataInfo == "partner"))
                                        anvNuvarandeRelation = RelationType.Partner;
                                    else if ((dataInfo == "Fiancee") || (dataInfo == "fiancee"))
                                        anvNuvarandeRelation = RelationType.Fiancee;
                                    else if ((dataInfo == "Married") || (dataInfo == "married"))
                                        anvNuvarandeRelation = RelationType.Married;
                                    else if ((dataInfo == "Divorced") || (dataInfo == "divorced"))
                                        anvNuvarandeRelation = RelationType.Divorced;
                                    else if ((dataInfo == "Widow") || (dataInfo == "widow"))
                                        anvNuvarandeRelation = RelationType.Widow;
                                    else if ((dataInfo == "Widower") || (dataInfo == "widower"))
                                        anvNuvarandeRelation = RelationType.Widower;
                                    else
                                        anvNuvarandeRelation = RelationType.UndefRelationType;
                                    noOfDataSet++;
                                } break;
                            case "Gender  ":
                                {
                                    // Gender   : GenderType "type"; float "length"; float "circumference"; DistUnits "usedUnits"; GenderAppearanceType "appearance"; GenderBhaviourType "behaviour"; string "presentation"; DateTime "validDate"
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
                                            if ((dp1 > 0) && (dp1 < dataInfo.Length - 2))
                                            {
                                                // float "length"; float "circumference"; DistUnits "usedUnits"; GenderAppearanceType "appearance"; GenderBhaviourType "behaviour"; string "presentation"; DateTime "validDate"
                                                anvKonsInfo[noOfGenderInfo].length = Int32.Parse(dataInfo.Substring(0, dp1));
                                                noOfDataSet++;
                                                dataInfo = dataInfo.Substring(dp1 + 2, dataInfo.Length - dp1 - 2);
                                                dp2 = dataInfo.IndexOf(";");
                                                if ((dp2 > 0) && (dp2 < dataInfo.Length - 2))
                                                {
                                                    // float "circumference"; DistUnits "usedUnits"; GenderAppearanceType "appearance"; GenderBhaviourType "behaviour"; string "presentation"; DateTime "validDate"
                                                    anvKonsInfo[noOfGenderInfo].circumference = Int32.Parse(dataInfo.Substring(0, dp2));
                                                    noOfDataSet++;
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
                                                                    anvKonsInfo[noOfGenderInfo].presentation = dataInfo.Substring(0, dp6);
                                                                    noOfDataSet++;
                                                                    dataInfo = dataInfo.Substring(dp6 + 2, dataInfo.Length - dp6 - 2);
                                                                    // DateTime "validDate" [YYYY-MM-DD|YY-MM-DD|YYYY-MM|YY-MM|YYYY|YY]
                                                                    dp7 = dataInfo.IndexOf("-");
                                                                    if ((dp7 > 0) || (dp7 < dataInfo.Length - 1))
                                                                    {
                                                                        // DateTime "validDate" [YYYY-MM-DD|YY-MM-DD|YYYY-MM|YY-MM]
                                                                        anvKonsInfo[noOfGenderInfo].validDate.AddYears(Int32.Parse(dataInfo.Substring(0, dp7)));
                                                                        noOfDataSet++;
                                                                        dataInfo = dataInfo.Substring(dp7 + 1, dataInfo.Length - dp7 - 1);
                                                                        dp8 = dataInfo.IndexOf("-");
                                                                        if ((dp8 > 0) || (dp8 < dataInfo.Length - 1))
                                                                        {
                                                                            // DateTime "validDate" [MM-DD]
                                                                            anvKonsInfo[noOfGenderInfo].validDate.AddMonths(Int32.Parse(dataInfo.Substring(0, dp8)));
                                                                            noOfDataSet++;
                                                                            anvKonsInfo[noOfGenderInfo].validDate.AddDays(Int32.Parse(dataInfo.Substring(dp8 + 1, dataInfo.Length - dp8 - 1)));
                                                                            noOfDataSet++;
                                                                        }
                                                                        else
                                                                        {
                                                                            // DateTime "validDate" [MM]
                                                                            anvKonsInfo[noOfGenderInfo].validDate.AddMonths(Int32.Parse(dataInfo));
                                                                            noOfDataSet++;
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        // DateTime "validDate" [YYYY|YY]
                                                                        anvKonsInfo[noOfGenderInfo].validDate.AddYears(Int32.Parse(dataInfo));
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
                                } break;
                            case "Length  ":
                                {
                                    // TODO - Fix Actor length handling
                                    // Length   : Value; Unit; Valid-Date
                                } break;
                            case "Weight  ":
                                {
                                    // TODO - Fix Actor weight handling
                                    // Weight   : Value; unit; Valid-Date
                                } break;
                            case "Boobs   ":
                                {
                                    // TODO - Fix Actor chest handling
                                    // Boobs    : Type-tag; Circumf; Unit; Size-tag; Valid-Date
                                } break;
                            case "FaceData":
                                {
                                    // TODO - Fix Actor face-data handling
                                    // FaceData : Eye-width; Cheekbone-Width; Chin-Width; Mouth-Width; Height; Unit; Valid-Date
                                } break;
                            case "Residnce":
                                {
                                    // TODO - Fix Actor residence handling
                                    // Residnce : Streetname; number; additive; City; Area; Zipcode; Country; Purchase; In-Date; Out-Date; Sale; Currency
                                } break;
                            case "HairColr":
                                {
                                    // TODO - Fix Actor hair color handling
                                    // HairColr : Color-tag; Texture-tag; Length-tag; Valid-Date
                                } break;
                            case "MarkData":
                                {
                                    // TODO - Fix Actor marking data handling.
                                    // MarkData : type-tag; placement; Motif; Valid-Date
                                } break;
                            case "Ocupatn ":
                                {
                                    // TODO - Fix Actor occupation data handling.
                                    // Ocupatn  : Title; Company; Streetname; Number; State; Areaname; Zipcode; Country; Start-Date; End-Date
                                } break;
                            case "Attended":
                                {
                                    // TODO - Fix Actor attended events handling.
                                    // Attended : Event ID; Event-Type-tag; Start-Date-Time; End-Date-Time;
                                } break;
                            case "ReltdImg":
                                {
                                    // TODO - Fix Actor related images handling.
                                    // ReltdImg : Path-Name-ext; Type-Of-Content-Tag; Security-Level
                                } break;
                            case "National":
                                {
                                    // TODO - Fix Actor nationality handling.
                                    // National : Nationality-name
                                } break;
                            default:
                                // A unused value
                                break;
                        }
                    }
                }
            }
        }
    }
}
