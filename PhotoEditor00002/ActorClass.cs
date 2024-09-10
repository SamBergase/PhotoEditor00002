using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Principal;
using System.Globalization;
using System.IO;

namespace PhotoEditor00002
{
    #region enum-definitions
    public enum currEnum
    { 
        Undefined, Event, Occupation, Eyes, Residence, BrthDta, Face, Distance, Length, Weight, Gender, GenApp, GenBeh, BrstSz, BrstTp, Etnicity, HairText, HairLnt, BdyTp, MrkTp, Classification, NmeTp, GeoDir, RelImg, Nation
    }
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
        UndefHairTexture, Straight, Wavy, Curly, Coily, Frizzy
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
        public bool glasses;
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
        public string cityname;
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
    public struct userNationalities
    {
        public string nationTag;
        public string nationName;
        public int nationNumber;
    }
    #endregion
    class ActorClass
    {
        #region variables
        private string userId;
        public const int maxNoOfRootDirs = 15;
        public int noOfRootDirs = 0;
        private string[] userRootDir = new string[maxNoOfRootDirs];

        public string[] BreastSizeStrings = { "Undef", "AA", "A", "B", "C", "D", "E", "F", "Flat", "Medium", "Bulgy", "Oversize" };
        public string[] BreastTypeStrings = { "Undef", "Natural", "Silocone", "Saggy", "Puffy", "Nippy" };
        public string[] HairTextureStrings = { "Undef", "Straight", "Wavy", "Curly", "Coily", "Frizzy" };
        public string[] HairLengthStrings = { "Undef", "Short", "Neck", "Shoulder", "MidBack", "Waist", "Ass", "Long" };
        public string[] SecrecyLevelStrings = { "UndefLevel", "Unclassified", "Limited", "Confidential", "Secret", "QualifSecret" };
        public string[] MarkingsTypeStrings = { "UndefMarking", "Scar", "Freckles", "Birthmark", "Tattoo", "Piercing" };

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

        public const int maxNoOfNationalityCategories = 250;
        public int noOfNationalityCategories = 0;
        public defaultStruct[] nationalityCategories = new defaultStruct[maxNoOfNationalityCategories];

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

        public const int maxNoOfComplexionCategories = 128;
        public int noOfComplexionCategories = 0;
        public defaultStruct[] complexionCategory = new defaultStruct[maxNoOfComplexionCategories];

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

        public const int maxNoOfRelatedImagesData = 2048;
        public int noOfRelatedImagesData = 0;
        public userRelatedImages[] anvRelBilder = new userRelatedImages[maxNoOfRelatedImagesData];

        public const int maxNoOfNationalityData = 10;
        public int noOfNationalityData = 0;
        public userNationalities[] anvNationalities = new userNationalities[maxNoOfNationalityData];
//        public string[] nationality = new string[maxNoOfNationalityData];

        string scu = WindowsIdentity.GetCurrent().Name;
        string sProgPath = "source\\repos\\PhotoEditor00002\\PhotoEditor00002";

        public int noOfDataSet = 0;
        #endregion
        public bool checkEnumeration(string tag, int number, currEnum type)
        {
            bool retVal = false;
            switch (tag.ToLower())
            {
                case "birth":
                case "birthname":
                    {
                        if ((type == currEnum.NmeTp) && (number < maxNoOfNames))
                        {
                            anvNamn[number].nameType = NameType.Birth;
                            retVal = true;
                        }
                    } break;
                case "taken":
                    {
                        if ((type == currEnum.NmeTp) && (number < maxNoOfNames))
                        {
                            anvNamn[number].nameType = NameType.Taken;
                            retVal = true;
                        }
                    } break;
                case "married":
                    {
                        if ((type == currEnum.NmeTp) && (number < maxNoOfNames))
                        {
                            anvNamn[number].nameType = NameType.Married;
                            retVal = true;
                        }
                    } break;
                case "alias":
                    {
                        if ((type == currEnum.NmeTp) && (number < maxNoOfNames))
                        {
                            anvNamn[number].nameType = NameType.Alias;
                            retVal = true;
                        }
                    } break;
                case "nick":
                case "nickname":
                    {
                        if ((type == currEnum.NmeTp) && (number < maxNoOfNames))
                        {
                            anvNamn[number].nameType = NameType.Nickname;
                            retVal = true;
                        }
                    }
                    break;
                case "male":
                    {
                        if (type == currEnum.BrthDta)
                        {
                            anvFodlseData.brthGender = GenderType.Male;
                            retVal = true;
                        }
                        else if (type == currEnum.Gender)
                        {
                            if (number < maxNoOfGenderInfo)
                            {
                                anvKonsInfo[number].type = GenderType.Male;
                                retVal = true;
                            }
                        }
                    } break;
                case "female":
                    {
                        if (type == currEnum.BrthDta)
                        {
                            anvFodlseData.brthGender = GenderType.Female;
                            retVal = true;
                        }
                        else if (type == currEnum.Gender)
                        {
                            if (number < maxNoOfGenderInfo)
                            {
                                anvKonsInfo[number].type = GenderType.Female;
                                retVal = true;
                            }
                        }
                    }
                    break;
                case "n":
                case "north":
                    {
                        if (type == currEnum.BrthDta)
                        {
                            anvFodlseData.latDir = GeogrDir.North;
                            retVal = true;
                        }
                    } break;
                case "e":
                case "east":
                    {
                        if (type == currEnum.BrthDta)
                        {
                            if (type == currEnum.BrthDta)
                            {
                                anvFodlseData.lonDir = GeogrDir.East;
                                retVal = true;
                            }
                        }
                        else if (type == currEnum.BrstSz)
                        {
                            if (number < maxNoOfChestData)
                            {
                                anvBrostData[number].sizeType = BreastSizeType.E;
                                retVal = true;
                            }
                        }
                    } break;
                case "w":
                case "west":
                    {
                        if (type == currEnum.BrthDta)
                        {
                            anvFodlseData.lonDir = GeogrDir.West;
                            retVal = true;
                        }
                    } break;
                case "s":
                case "south":
                    {
                        if (type == currEnum.BrthDta)
                        {
                            anvFodlseData.latDir = GeogrDir.South;
                            retVal = true;
                        }
                    } break;
                case "negro":
                    {
                        if (number < maxNoOfComplexions)
                        {
                            anvHudfarg[number].etnicType = EtnicityType.Negro;
                            retVal = true;
                        }
                    } break;
                case "asian":
                    {
                        if (number < maxNoOfComplexions)
                        {
                            anvHudfarg[number].etnicType = EtnicityType.Asian;
                            retVal = true;
                        }
                    } break;
                case "indian":
                    {
                        if (number < maxNoOfComplexions)
                        {
                            anvHudfarg[number].etnicType = EtnicityType.Indian;
                            retVal = true;
                        }
                    } break;
                case "cocation":
                    {
                        if (number < maxNoOfComplexions)
                        {
                            anvHudfarg[number].etnicType = EtnicityType.Cocation;
                            retVal = true;
                        }
                    } break;
                case "mulatto":
                    {
                        if (number < maxNoOfComplexions)
                        {
                            anvHudfarg[number].etnicType = EtnicityType.Mulatto;
                            retVal = true;
                        }
                    } break;
                case "innuit":
                    {
                        if (number < maxNoOfComplexions)
                        {
                            anvHudfarg[number].etnicType = EtnicityType.Innuit;
                            retVal = true;
                        }
                    } break;
                case "pts":
                case "points":
                    {
                        if (type == currEnum.Gender)
                        {
                            if (number < maxNoOfGenderInfo)
                            {
                                anvKonsInfo[number].usedUnits = DistUnits.Points;
                                retVal = true;
                            }
                        }
                        else if (type == currEnum.Length)
                        {
                            if (number < maxNoOfLengthData)
                            {
                                anvLangdData[number].unit = DistUnits.Points;
                                retVal = true;
                            }
                        }
                        else if (type == currEnum.BrstSz)
                        {
                            if (number < maxNoOfChestData)
                            {
                                anvBrostData[number].units = DistUnits.Points;
                                retVal = true;
                            }
                        }
                        else if (type == currEnum.Face)
                        {
                            if (number < maxNoOfFaceData)
                            {
                                anvAnsiktsData[number].units = DistUnits.Points;
                                retVal = true;
                            }
                        }
                    } break;
                case "mm":
                case "millimeter":
                    {
                        if (type == currEnum.Gender)
                        {
                            if (number < maxNoOfGenderInfo)
                            {
                                anvKonsInfo[number].usedUnits = DistUnits.Millimeter;
                                retVal = true;
                            }
                        }
                        else if (type == currEnum.Length)
                        {
                            if (number < maxNoOfLengthData)
                            {
                                anvLangdData[number].unit = DistUnits.Millimeter;
                                retVal = true;
                            }
                        }
                        else if (type == currEnum.BrstSz)
                        {
                            if (number < maxNoOfChestData)
                            {
                                anvBrostData[number].units = DistUnits.Millimeter;
                                retVal = true;
                            }
                        }
                        else if (type == currEnum.Face)
                        {
                            if (number < maxNoOfFaceData)
                            {
                                anvAnsiktsData[number].units = DistUnits.Millimeter;
                                retVal = true;
                            }
                        }
                    } break;
                case "cm":
                case "centimeter":
                    {
                        if (type == currEnum.Gender)
                        {
                            if (number < maxNoOfGenderInfo)
                            {
                                anvKonsInfo[number].usedUnits = DistUnits.Centimeter;
                                retVal = true;
                            }
                        }
                        else if (type == currEnum.Length)
                        {
                            if (number < maxNoOfLengthData)
                            {
                                anvLangdData[number].unit = DistUnits.Centimeter;
                                retVal = true;
                            }
                        }
                        else if (type == currEnum.BrstSz)
                        {
                            if (number < maxNoOfChestData)
                            {
                                anvBrostData[number].units = DistUnits.Centimeter;
                                retVal = true;
                            }
                        }
                        else if (type == currEnum.Face)
                        {
                            if (number < maxNoOfFaceData)
                            {
                                anvAnsiktsData[number].units = DistUnits.Centimeter;
                                retVal = true;
                            }
                        }
                    } break;
                case "dm":
                case "decimeter":
                    {
                        if (type == currEnum.Gender)
                        {
                            if (number < maxNoOfGenderInfo)
                            {
                                anvKonsInfo[number].usedUnits = DistUnits.Decimeter;
                                retVal = true;
                            }
                        }
                        else if (type == currEnum.Length)
                        {
                            if (number < maxNoOfLengthData)
                            {
                                anvLangdData[number].unit = DistUnits.Decimeter;
                                retVal = true;
                            }
                        }
                        else if (type == currEnum.BrstSz)
                        {
                            if (number < maxNoOfChestData)
                            {
                                anvBrostData[number].units = DistUnits.Decimeter;
                                retVal = true;
                            }
                        }
                        else if (type == currEnum.Face)
                        {
                            if (number < maxNoOfFaceData)
                            {
                                anvAnsiktsData[number].units = DistUnits.Decimeter;
                                retVal = true;
                            }
                        }
                    } break;
                case "m":
                case "meter":
                    {
                        if (type == currEnum.Gender)
                        {
                            if (number < maxNoOfGenderInfo)
                            {
                                anvKonsInfo[number].usedUnits = DistUnits.Meter;
                                retVal = true;
                            }
                        }
                        else if (type == currEnum.Length)
                        {
                            if (number < maxNoOfLengthData)
                            {
                                anvLangdData[number].unit = DistUnits.Meter;
                                retVal = true;
                            }
                        }
                        else if (type == currEnum.BrstSz)
                        {
                            if (number < maxNoOfChestData)
                            {
                                anvBrostData[number].units = DistUnits.Meter;
                                retVal = true;
                            }
                        }
                        else if (type == currEnum.Face)
                        {
                            if (number < maxNoOfFaceData)
                            {
                                anvAnsiktsData[number].units = DistUnits.Meter;
                                retVal = true;
                            }
                        }
                    }
                    break;
                case "in":
                case "inch":
                    {
                        if (type == currEnum.Gender)
                        {
                            if (number < maxNoOfGenderInfo)
                            {
                                anvKonsInfo[number].usedUnits = DistUnits.Inch;
                                retVal = true;
                            }
                        }
                        else if (type == currEnum.Length)
                        {
                            if (number < maxNoOfLengthData)
                            {
                                anvLangdData[number].unit = DistUnits.Inch;
                                retVal = true;
                            }
                        }
                        else if (type == currEnum.BrstSz)
                        {
                            if (number < maxNoOfChestData)
                            {
                                anvBrostData[number].units = DistUnits.Inch;
                                retVal = true;
                            }
                        }
                        else if (type == currEnum.Face)
                        {
                            if (number < maxNoOfFaceData)
                            {
                                anvAnsiktsData[number].units = DistUnits.Inch;
                                retVal = true;
                            }
                        }
                    }
                    break;
                case "ft":
                case "feet":
                    {
                        if (type == currEnum.Gender)
                        {
                            if (number < maxNoOfGenderInfo)
                            {
                                anvKonsInfo[number].usedUnits = DistUnits.Feet;
                                retVal = true;
                            }
                        }
                        else if (type == currEnum.Length)
                        {
                            if (number < maxNoOfLengthData)
                            {
                                anvLangdData[number].unit = DistUnits.Feet;
                                retVal = true;
                            }
                        }
                        else if (type == currEnum.BrstSz)
                        {
                            if (number < maxNoOfChestData)
                            {
                                anvBrostData[number].units = DistUnits.Feet;
                                retVal = true;
                            }
                        }
                        else if (type == currEnum.Face)
                        {
                            if (number < maxNoOfFaceData)
                            {
                                anvAnsiktsData[number].units = DistUnits.Feet;
                                retVal = true;
                            }
                        }
                    }
                    break;
                case "yd":
                case "yard":
                    {
                        if (type == currEnum.Gender)
                        {
                            if (number < maxNoOfGenderInfo)
                            {
                                anvKonsInfo[number].usedUnits = DistUnits.Yard;
                                retVal = true;
                            }
                        }
                        else if (type == currEnum.Length)
                        {
                            if (number < maxNoOfLengthData)
                            {
                                anvLangdData[number].unit = DistUnits.Yard;
                                retVal = true;
                            }
                        }
                        else if (type == currEnum.BrstSz)
                        {
                            if (number < maxNoOfChestData)
                            {
                                anvBrostData[number].units = DistUnits.Yard;
                                retVal = true;
                            }
                        }
                        else if (type == currEnum.Face)
                        {
                            if (number < maxNoOfFaceData)
                            {
                                anvAnsiktsData[number].units = DistUnits.Yard;
                                retVal = true;
                            }
                        }
                    }
                    break;
                case "barbie":
                    {
                        if (number < maxNoOfGenderInfo)
                        {
                            anvKonsInfo[number].appearance = GenderAppearanceType.Barbie;
                            retVal = true;
                        }
                    } break;
                case "curtains":
                    {
                        if (number < maxNoOfGenderInfo)
                        {
                            anvKonsInfo[number].appearance = GenderAppearanceType.Curtains;
                            retVal = true;
                        }
                    } break;
                case "horse":
                case "horseshoe":
                    {
                        if (number < maxNoOfGenderInfo)
                        {
                            anvKonsInfo[number].appearance = GenderAppearanceType.Horseshoe;
                            retVal = true;
                        }
                    } break;
                case "puffy":
                    {
                        if (number < maxNoOfGenderInfo)
                        {
                            anvKonsInfo[number].appearance = GenderAppearanceType.Puffy;
                            retVal = true;
                        }
                    } break;
                case "tulip":
                    {
                        if (number < maxNoOfGenderInfo)
                        {
                            anvKonsInfo[number].appearance = GenderAppearanceType.Tulip;
                            retVal = true;
                        }
                    } break;
                case "1":
                case "i":
                    {
                        if (number < maxNoOfGenderInfo)
                        {
                            anvKonsInfo[number].appearance = GenderAppearanceType.I;
                            retVal = true;
                        }
                    } break;
                case "2":
                case "ii":
                    {
                        if (number < maxNoOfGenderInfo)
                        {
                            anvKonsInfo[number].appearance = GenderAppearanceType.II;
                            retVal = true;
                        }
                    } break;
                case "3":
                case "iii":
                    {
                        if (number < maxNoOfGenderInfo)
                        {
                            anvKonsInfo[number].appearance = GenderAppearanceType.III;
                            retVal = true;
                        }
                    } break;
                case "4":
                case "iv":
                    {
                        if (number < maxNoOfGenderInfo)
                        {
                            anvKonsInfo[number].appearance = GenderAppearanceType.IV;
                            retVal = true;
                        }
                    } break;
                case "5":
                case "v":
                    {
                        if (number < maxNoOfGenderInfo)
                        {
                            anvKonsInfo[number].appearance = GenderAppearanceType.V;
                            retVal = true;
                        }
                    } break;
                case "sloppy":
                    {
                        if (number < maxNoOfGenderInfo)
                        {
                            anvKonsInfo[number].behaviour = GenderBhaviourType.Sloppy;
                            retVal = true;
                        }
                    } break;
                case "stretch":
                case "stretchy":
                    {
                        if (number < maxNoOfGenderInfo)
                        {
                            anvKonsInfo[number].behaviour = GenderBhaviourType.Stretchy;
                            retVal = true;
                        }
                    } break;
                case "tight":
                    {
                        if (number < maxNoOfGenderInfo)
                        {
                            anvKonsInfo[number].behaviour = GenderBhaviourType.Tight;
                            retVal = true;
                        }
                    } break;
                case "stiff":
                case "stiffer":
                    {
                        if (number < maxNoOfGenderInfo)
                        {
                            anvKonsInfo[number].behaviour = GenderBhaviourType.Stiffer;
                            retVal = true;
                        }
                    } break;
                case "grow":
                case "grower":
                    {
                        if (number < maxNoOfGenderInfo)
                        {
                            anvKonsInfo[number].behaviour = GenderBhaviourType.Grower;
                            retVal = true;
                        }
                    } break;
                case "g":
                case "gram":
                    {
                        if ((type == currEnum.Weight) && (number < maxNoOfWeightData))
                        {
                            anvViktData[number].unit = WeightUnits.Gram;
                            retVal = true;
                        }
                    } break;
                case "kg":
                case "kilogram":
                    {
                        if ((type == currEnum.Weight) && (number < maxNoOfWeightData))
                        {
                            anvViktData[number].unit = WeightUnits.KiloGram;
                            retVal = true;
                        }
                    } break;
                case "tn":
                case "tonnes":
                    {
                        if ((type == currEnum.Weight) && (number < maxNoOfWeightData))
                        {
                            anvViktData[number].unit = WeightUnits.Tonnes;
                            retVal = true;
                        }
                    } break;
                case "pd":
                case "pound":
                    {
                        if ((type == currEnum.Weight) && (number < maxNoOfWeightData))
                        {
                            anvViktData[number].unit = WeightUnits.Pound;
                            retVal = true;
                        }
                    } break;
                case "st":
                case "stones":
                    {
                        if ((type == currEnum.Weight) && (number < maxNoOfWeightData))
                        {
                            anvViktData[number].unit = WeightUnits.Stones;
                            retVal = true;
                        }
                    } break;
                case "natural":
                    {
                        if (number < maxNoOfChestData)
                        {
                            anvBrostData[number].type = BreastType.NaturalBreasts;
                            retVal = true;
                        }
                    } break;
                case "fake":
                case "plastic":
                case "silicone":
                    {
                        if (number < maxNoOfChestData)
                        {
                            anvBrostData[number].type = BreastType.SiliconeBreasts;
                            retVal = true;
                        }
                    } break;
                case "aa":
                    { 
                        if (number < maxNoOfChestData)
                        {
                            anvBrostData[number].sizeType = BreastSizeType.AA;
                            retVal = true;
                        }
                    } break;
                case "a":
                    {
                        if (number < maxNoOfChestData)
                        {
                            anvBrostData[number].sizeType = BreastSizeType.A;
                            retVal = true;
                        }
                    } break;
                case "b":
                    {
                        if (number < maxNoOfChestData)
                        {
                            anvBrostData[number].sizeType = BreastSizeType.B;
                            retVal = true;
                        }
                    } break;
                case "bulgy":
                    {
                        if (number < maxNoOfChestData)
                        {
                            anvBrostData[number].sizeType = BreastSizeType.Bulgy;
                            retVal = true;
                        }
                    } break;
                case "c":
                    {
                        if (number < maxNoOfChestData)
                        {
                            anvBrostData[number].sizeType = BreastSizeType.C;
                            retVal = true;
                        }
                    } break;
                case "d":
                    {
                        if (number < maxNoOfChestData)
                        {
                            anvBrostData[number].sizeType = BreastSizeType.D;
                            retVal = true;
                        }
                    } break;
                case "f":
                    {
                        if (number < maxNoOfChestData)
                        {
                            anvBrostData[number].sizeType = BreastSizeType.F;
                            retVal = true;
                        }
                    } break;
                case "flat":
                    {
                        if (number < maxNoOfChestData)
                        {
                            anvBrostData[number].sizeType = BreastSizeType.Flat;
                            retVal = true;
                        }
                    } break;
                case "medium":
                case "normal":
                    {
                        if ((type == currEnum.BrstSz) && (number < maxNoOfChestData))
                        {
                            anvBrostData[number].sizeType = BreastSizeType.Medium;
                            retVal = true;
                        }
                        else if ((type == currEnum.RelImg) && (number < maxNoOfRelatedImagesData))
                        {
                            anvRelBilder[number].classificationLevel = ClassificationType.Confidential;
                            retVal = true;
                        }

                    }
                    break;
                case "oversize":
                    {
                        if (number < maxNoOfChestData)
                        {
                            anvBrostData[number].sizeType = BreastSizeType.Oversize;
                            retVal = true;
                        }
                    } break;
                case "straight":
                    {
                        if ((type == currEnum.HairText) && (noOfHairData < maxNoOfHairData))
                        {
                            anvHaar[number].textureTag = HairTextureType.Straight;
                            retVal = true;
                        }
                    } break;
                case "wavy":
                    {
                        if ((type == currEnum.HairText) && (noOfHairData < maxNoOfHairData))
                        {
                            anvHaar[number].textureTag = HairTextureType.Wavy;
                            retVal = true;
                        }
                    }
                    break;
                case "curly":
                    {
                        if ((type == currEnum.HairText) && (noOfHairData < maxNoOfHairData))
                        {
                            anvHaar[number].textureTag = HairTextureType.Curly;
                            retVal = true;
                        }
                    }
                    break;
                case "coily":
                    {
                        if ((type == currEnum.HairText) && (noOfHairData < maxNoOfHairData))
                        {
                            anvHaar[number].textureTag = HairTextureType.Coily;
                            retVal = true;
                        }
                    }
                    break;
                case "short":
                    {
                        if ((type == currEnum.HairLnt) && (number < maxNoOfHairData))
                        {
                            anvHaar[number].lengthTag = HairLengthType.Short;
                            retVal = true;
                        }
                    } 
                    break;
                case "neck":
                    {
                        if ((type == currEnum.HairLnt) && (number < maxNoOfHairData))
                        {
                            anvHaar[number].lengthTag = HairLengthType.Neck;
                            retVal = true;
                        }
                    }
                    break;
                case "shoulder":
                    {
                        if ((type == currEnum.HairLnt) && (number < maxNoOfHairData))
                        {
                            anvHaar[number].lengthTag = HairLengthType.Shoulder;
                            retVal = true;
                        }
                    }
                    break;
                case "mid":
                case "mid back":
                    {
                        if ((type == currEnum.HairLnt) && (number < maxNoOfHairData))
                        {
                            anvHaar[number].lengthTag = HairLengthType.MidBack;
                            retVal = true;
                        }
                    }
                    break;
                case "waist":
                    {
                        if ((type == currEnum.HairLnt) && (number < maxNoOfHairData))
                        {
                            anvHaar[number].lengthTag = HairLengthType.Waist;
                            retVal = true;
                        }
                    }
                    break;
                case "ass":
                    {
                        if ((type == currEnum.HairLnt) && (number < maxNoOfHairData))
                        {
                            anvHaar[number].lengthTag = HairLengthType.Ass;
                            retVal = true;
                        }
                    }
                    break;
                case "long":
                    {
                        if ((type == currEnum.HairLnt) && (number < maxNoOfHairData))
                        {
                            anvHaar[number].lengthTag = HairLengthType.Long;
                            retVal = true;
                        }
                    }
                    break;
                case "scar":
                    {
                        if ((type == currEnum.MrkTp) && (number < maxNoOfMarkingData))
                        {
                            anvMarkningar[number].markTag = MarkingType.Scar;
                            retVal = true;
                        }
                    } 
                    break;
                case "freckles":
                    {
                        if ((type == currEnum.MrkTp) && (number < maxNoOfMarkingData))
                        {
                            anvMarkningar[number].markTag = MarkingType.Freckles;
                            retVal = true;
                        }
                    }
                    break;
                case "birthmark":
                    {
                        if ((type == currEnum.MrkTp) && (number < maxNoOfMarkingData))
                        {
                            anvMarkningar[number].markTag = MarkingType.Birthmark;
                            retVal = true;
                        }
                    }
                    break;
                case "tatt":
                case "tattoo":
                    {
                        if ((type == currEnum.MrkTp) && (number < maxNoOfMarkingData))
                        {
                            anvMarkningar[number].markTag = MarkingType.Tattoo;
                            retVal = true;
                        }
                    }
                    break;
                case "pierce":
                case "piercing":
                    {
                        if ((type == currEnum.MrkTp) && (number < maxNoOfMarkingData))
                        {
                            anvMarkningar[number].markTag = MarkingType.Piercing;
                            retVal = true;
                        }
                    }
                    break;
                case "open":
                case "unclassified":
                    {
                        if ((type == currEnum.RelImg) && (number < maxNoOfRelatedImagesData))
                        {
                            anvRelBilder[number].classificationLevel = ClassificationType.Unclassified;
                            retVal = true;
                        }
                    } 
                    break;
                case "limited":
                    {
                        if ((type == currEnum.RelImg) && (number < maxNoOfRelatedImagesData))
                        {
                            anvRelBilder[number].classificationLevel = ClassificationType.Limited;
                            retVal = true;
                        }
                    }
                    break;
                case "relative":
                case "confidential":
                    {
                        if ((type == currEnum.RelImg) && (number < maxNoOfRelatedImagesData))
                        {
                            anvRelBilder[number].classificationLevel = ClassificationType.Confidential;
                            retVal = true;
                        }
                    }
                    break;
                case "secret":
                    {
                        if ((type == currEnum.RelImg) && (number < maxNoOfRelatedImagesData))
                        {
                            anvRelBilder[number].classificationLevel = ClassificationType.Secret;
                            retVal = true;
                        }
                    }
                    break;
                case "qualif":
                case "qualifsecret":
                    {
                        if ((type == currEnum.RelImg) && (number < maxNoOfRelatedImagesData))
                        {
                            anvRelBilder[number].classificationLevel = ClassificationType.QualifSecret;
                            retVal = true;
                        }
                    }
                    break;
                default:
                    {
                        // do nothing, retVal is allready "false";
                    } break;
            }
            return retVal;
        }
        public bool handleDate(string indata, int number, currEnum type)
        {
            // Possible formats : [YY]YY[-|.| ][MM][-|.| ][DD][ HH[:|.|-]]
            bool retVal = false;
            int calcYear = 0;
            int calcMonth = 0;
            int calcDay = 0;
            string largeDur = "";
            string smallDur = "";
            string durationType = "";
            char delimchar;
            if (indata.Contains("-"))
                delimchar = '-';
            else if (indata.Contains("."))
                delimchar = '.';
            else
                delimchar = ' ';
            int dp = indata.IndexOf(delimchar);
            if ((dp > 0) && (dp < indata.Length - 1))
            {
                if (int.TryParse(indata.Substring(0, dp), out calcYear))
                {
                    if (calcYear < 100)
                        calcYear = calcYear + 1900;
                    indata = indata.Substring(dp + 1, indata.Length - dp - 1);
                    // Possible formats : [MM][-|.| ][DD][ HH[:|.|-]]
                    if (indata.Contains("-"))
                        delimchar = '-';
                    else if (indata.Contains("."))
                        delimchar = '.';
                    else if (indata.Contains(" "))
                        delimchar = ' ';
                    dp = indata.IndexOf(delimchar);
                    if ((dp > 0) && (dp < indata.Length - 1))
                    {
                        if (int.TryParse(indata.Substring(0, dp), out calcMonth))
                        {
                            indata = indata.Substring(dp + 1, indata.Length - dp - 1);
                            // Possible formats : [DD][ HH[:|.|-]]
                            dp = indata.IndexOf(" ");
                            if ((dp > 0) && (dp < indata.Length))
                            {
                                if (int.TryParse(indata.Substring(0, dp), out calcDay))
                                    retVal = true;
                                else
                                    retVal = false;
                                indata = indata.Substring(dp + 1, indata.Length - dp - 1);
                                // Possible formats : HH[[:|.|-]MM]
                                if (indata.Contains(":"))
                                    delimchar = ':';
                                else if (indata.Contains("."))
                                    delimchar = '.';
                                else if (indata.Contains("-"))
                                    delimchar = '-';
                                else if (indata.Contains(" "))
                                    delimchar = ' ';
                                dp = indata.IndexOf(delimchar);
                                if ((dp > 0) && (dp < indata.Length - 1))
                                {
                                    largeDur = indata.Substring(0, dp);
                                    indata = indata.Substring(dp + 1, indata.Length - dp - 1);
                                    // Possible formats : [MM][time|hour|minute|second]
                                    dp = indata.IndexOf(" ");
                                    if ((dp > 0) && (dp < indata.Length - 1))
                                    {
                                        smallDur = indata.Substring(0, dp);
                                        durationType = indata.Substring(dp + 1, indata.Length - dp - 1);
                                    }
                                    else
                                        smallDur = indata;
                                }
                                else
                                    largeDur = indata;
                            }
                            else
                            {
                                if (int.TryParse(indata, out calcDay))
                                    retVal = true;
                                else
                                    retVal = false;
                            }
                        }
                    }
                    else
                    {
                        if (int.TryParse(indata, out calcMonth))
                            retVal = true;
                        else
                            retVal = false;
                    }
                }
                else
                    retVal = false;
            }
            else
            {
                if (int.TryParse(indata, out calcYear))
                {
                    if (calcYear < 100)
                        calcYear = calcYear + 1900;
                    retVal = true;
                }
                else
                    retVal = false;
            }
            // actual handling
            if (type == currEnum.BrthDta)
            {
                if (calcYear > 0)
                    anvFodlseData.brthDate.year = calcYear;
                if (calcMonth > 0)
                    anvFodlseData.brthDate.month = calcMonth;
                if (calcDay > 0)
                    anvFodlseData.brthDate.day = calcDay;
                retVal = true;
            }
            else if ((type == currEnum.Etnicity) && (number < maxNoOfComplexions))
            {
                if (calcYear > 0)
                    anvHudfarg[number].validDate.year = calcYear;
                if (calcMonth > 0)
                    anvHudfarg[number].validDate.month = calcMonth;
                if (calcDay > 0)
                    anvHudfarg[number].validDate.day = calcDay;
                retVal = true;
            }
            else if ((type == currEnum.Gender) && (number < maxNoOfGenderInfo))
            {
                if (calcYear > 0)
                    anvKonsInfo[number].validDate.year = calcYear;
                if (calcMonth > 0)
                    anvKonsInfo[number].validDate.month = calcMonth;
                if (calcDay > 0)
                    anvKonsInfo[number].validDate.day = calcDay;
                retVal = true;
            }
            else if ((type == currEnum.Length) && (number < maxNoOfLengthData))
            {
                if (calcYear > 0)
                    anvLangdData[number].validDate.year = calcYear;
                if (calcMonth > 0)
                    anvLangdData[number].validDate.month = calcMonth;
                if (calcDay > 0)
                    anvLangdData[number].validDate.day = calcDay;
                retVal = true;
            }
            else if ((type == currEnum.Weight) && (number < maxNoOfWeightData))
            {
                if (calcYear > 0)
                    anvViktData[number].validDate.year = calcYear;
                if (calcMonth > 0)
                    anvViktData[number].validDate.month = calcMonth;
                if (calcDay > 0)
                    anvViktData[number].validDate.day = calcDay;
                retVal = true;
            }
            else if (((type == currEnum.BrstSz) || (type == currEnum.BrstTp)) && (number < maxNoOfChestData))
            {
                if (calcYear > 0)
                    anvBrostData[number].validDate.year = calcYear;
                if (calcMonth > 0)
                    anvBrostData[number].validDate.month = calcMonth;
                if (calcDay > 0)
                    anvBrostData[number].validDate.day = calcDay;
                retVal = true;
            }
            else if ((type == currEnum.Face) && (number < maxNoOfFaceData))
            {
                if (calcYear > 0)
                    anvAnsiktsData[number].validDate.year = calcYear;
                if (calcMonth > 0)
                    anvAnsiktsData[number].validDate.month = calcMonth;
                if (calcDay > 0)
                    anvAnsiktsData[number].validDate.day = calcDay;
                retVal = true;
            }
            else if ((type == currEnum.Residence) && (number < maxNoOfResidences))
            {
                if (anvBostader[number].boughtDate.year == 0)
                {
                    if (calcYear > 0)
                        anvBostader[number].boughtDate.year = calcYear;
                    if (calcMonth > 0)
                        anvBostader[number].boughtDate.month = calcMonth;
                    if (calcDay > 0)
                        anvBostader[number].boughtDate.day = calcDay;
                    retVal = true;
                }   
                else if (anvBostader[number].salesDate.year == 0)
                {
                    if (calcYear > 0)
                        anvBostader[number].salesDate.year = calcYear;
                    if (calcMonth > 0)
                        anvBostader[number].salesDate.month = calcMonth;
                    if (calcDay > 0)
                        anvBostader[number].salesDate.day = calcDay;
                    retVal = true;
                }
            }
            else if (((type == currEnum.HairLnt) || (type == currEnum.HairText)) && (number < maxNoOfHairData))
            {
                if (calcYear > 0)
                    anvHaar[number].validDate.year = calcYear;
                if (calcMonth > 0)
                    anvHaar[number].validDate.month = calcMonth;
                if (calcDay > 0)
                    anvHaar[number].validDate.day = calcDay;
                retVal = true;
            }
            else if ((type == currEnum.Eyes) && (number < maxNoOfEyeColorData))
            {
                if (calcYear > 0)
                    anvEyes[number].validDate.year = calcYear;
                if (calcMonth > 0)
                    anvEyes[number].validDate.month = calcMonth;
                if (calcDay > 0)
                    anvEyes[number].validDate.day = calcDay;
                retVal = true;
            }
            else if ((type == currEnum.MrkTp) && (number < maxNoOfMarkingData))
            {
                if (calcYear > 0)
                    anvMarkningar[number].validDate.year = calcYear;
                if (calcMonth > 0)
                    anvMarkningar[number].validDate.month = calcMonth;
                if (calcDay > 0)
                    anvMarkningar[number].validDate.year = calcDay;
                retVal = true;
            }
            else if ((type == currEnum.Occupation) && (number < maxNoOfOccupationData))
            {
                if (anvAnstallningar[number].started.year == 0)
                {
                    if (calcYear > 0)
                        anvAnstallningar[number].started.year = calcYear;
                    if (calcMonth > 0)
                        anvAnstallningar[number].started.month = calcMonth;
                    if (calcDay > 0)
                        anvAnstallningar[number].started.day = calcDay;
                    retVal = true;
                }
                else if (anvAnstallningar[number].ended.year == 0)
                {
                    if (calcYear > 0)
                        anvAnstallningar[number].ended.year = calcYear;
                    if (calcMonth > 0)
                        anvAnstallningar[number].ended.month = calcMonth;
                    if (calcDay > 0)
                        anvAnstallningar[number].ended.day = calcDay;
                    retVal = true;
                }
            }
            else if ((type == currEnum.Event) && (number < maxNoOfAttendedEventData))
            {
                if (anvTillstallningar[number].started.year == 0)
                {
                    if (calcYear > 0)
                        anvTillstallningar[number].started.year = calcYear;
                    if (calcMonth > 0)
                        anvTillstallningar[number].started.month = calcMonth;
                    if (calcDay > 0)
                        anvTillstallningar[number].started.day = calcDay;
                    retVal = true;
                }
                else if (anvTillstallningar[number].ended.year == 0)
                {
                    if (calcYear > 0)
                        anvTillstallningar[number].ended.year = calcYear;
                    if (calcMonth > 0)
                        anvTillstallningar[number].ended.month = calcMonth;
                    if (calcDay > 0)
                        anvTillstallningar[number].ended.day = calcDay;
                    retVal = true;
                }
            }
            return retVal;
        }
        //public string getActorDataDir() { return "C:\\Users\\"}
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
                tnr = userID.IndexOf("_");
                if (!((tnr > 0) && (tnr < userID.Length)))
                    userID = "ActorData_" + userID;
                tnr = userID.IndexOf(".");
                if (!((tnr > 0) && (tnr < userID.Length)))
                    userID = userID + ".acf";
                filename = rootPath + "\\ActorData\\" + userID;
            }
            if (System.IO.File.Exists(filename))
            {
                foreach (string line in System.IO.File.ReadLines(filename))
                {
                    if (line != "-1")
                    {
                        int dp0, dp1, dp2, dp3, dp4, dp5, dp6, dp7, dp8, dp9, dp10, dp11, dp12;
                        string dataTag = line.Substring(0, 8).ToLower();
                        string dataInfo = line.Substring(11, line.Length - 11);
                        switch (dataTag)
                        {
                            case "userid  ":
                                {
                                    this.userId = dataInfo;
                                    noOfDataSet++;
                                } break;
                            case "username":
                                {
                                    if (noOfNames < maxNoOfNames)
                                    {
                                        // format <surname>[ <midname(s)>][ <familyname>][; <nametype>]
                                        dp0 = dataInfo.IndexOf(";");
                                        if ((dp0 > 0) && (dp0 < dataInfo.Length - 2))
                                        {
                                            // We have name(s) and nametype
                                            string namntyp = dataInfo.Substring(dp0 + 2, dataInfo.Length - dp0 - 2);
                                            if (!(checkEnumeration(namntyp, noOfNames, currEnum.NmeTp)))
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
                            case "contact ":
                                {
                                    // Format: <Type of Contact>; <contact path>
                                    if (noOfContacts < maxNoOfContacts)
                                    {
                                        char delimChar = ';';
                                        int delimLength = 2;
                                        if (dataInfo.Contains(","))
                                        {
                                            delimChar = ',';
                                            delimLength = 2;
                                        }
                                        dp0 = dataInfo.IndexOf(delimChar);
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
                                                    anvKontakter[noOfContacts].contactPath = dataInfo.Substring(dp0 + delimLength, dataInfo.Length - dp0 - delimLength);
                                                }
                                            }
                                            if (!(foundCategory))
                                            {
                                                if (addContactCategory(cttt, "", "Undefined"))
                                                {
                                                    anvKontakter[noOfContacts].type.tag = cttt;
                                                    anvKontakter[noOfContacts].contactPath = dataInfo.Substring(dp0 + delimLength, dataInfo.Length - dp0 - delimLength);
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
                            case "brthdata":
                                {
                                    // BrthData : Streetname; number; additive; City; Area; Zipcode; Country; Date; Security code; Gender; <latvalue> <dir>; <lonvalue> <dir>
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
                                                                    handleDate(tmpString, 0, currEnum.BrthDta);
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
                                                                            if (!(checkEnumeration(tmpString, 0, currEnum.BrthDta)))
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
                                                                                    if (!(checkEnumeration(tmpString, 0, currEnum.BrthDta)))
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
                                                                                        if (!(checkEnumeration(dataInfo, 0, currEnum.BrthDta)))
                                                                                            anvFodlseData.lonDir = GeogrDir.UndefGeoDir;
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                        else
                                                                        {
                                                                            if ((dataInfo.Length > 0) && (dataInfo != " "))
                                                                            {
                                                                                if (!(checkEnumeration(dataInfo, 0, currEnum.BrthDta)))
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
                            case "skintone":
                                {
                                    // SkinTone : Complexion; R-value; G-value; B-value; Valid-Date
                                    if (noOfComplexions < maxNoOfComplexions)
                                    {
                                        dp0 = dataInfo.IndexOf(";");
                                        if ((dp0 > 0) && (dp0 < dataInfo.Length - 2))
                                        {
                                            // Complexion; R-value; G-value; B-value; Valid-Date
                                            string cplx = dataInfo.Substring(0, dp0);
                                            if (!(checkEnumeration(cplx, noOfComplexions, currEnum.Etnicity)))
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
                                                        handleDate(dataInfo, noOfComplexions, currEnum.Etnicity);
                                                    }
                                                }
                                            }
                                        }
                                        noOfComplexions++;
                                    }
                                } break;
                            case "relstats":
                                {
                                    // RelStats : <status>
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
                            case "gender  ":
                                {
                                    // Gender   : GenderType; length; circumference; usedUnits; GenderAppearanceType; GenderBhaviourType; presentation; validDate
                                    if (noOfGenderInfo < maxNoOfGenderInfo)
                                    {
                                        dp0 = dataInfo.IndexOf(";");
                                        if ((dp0 > 0) && (dp0 < dataInfo.Length - 2))
                                        {
                                            // GenderType "type"; float "length"; float "circumference"; DistUnits "usedUnits"; GenderAppearanceType "appearance"; GenderBhaviourType "behaviour"; string "presentation"; DateTime "validDate"
                                            string gdrtyp = dataInfo.Substring(0, dp0);
                                            if (!(checkEnumeration(gdrtyp, noOfGenderInfo, currEnum.Gender)))
                                                anvKonsInfo[noOfGenderInfo].type = GenderType.UndefGender;
                                            noOfDataSet++;
                                            dataInfo = dataInfo.Substring(dp0 + 2, dataInfo.Length - dp0 - 2);
                                            dp1 = dataInfo.IndexOf(";");
                                            //int resval;
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
                                                        if (!(checkEnumeration(disu, noOfGenderInfo, currEnum.Gender)))
                                                            anvKonsInfo[noOfGenderInfo].usedUnits = DistUnits.UndefDistUnit;
                                                        noOfDataSet++;
                                                        dataInfo = dataInfo.Substring(dp3 + 2, dataInfo.Length - dp3 - 2);
                                                        dp4 = dataInfo.IndexOf(";");
                                                        if ((dp4 > 0) && (dp4 < dataInfo.Length - 2))
                                                        {
                                                            // GenderAppearanceType "appearance"; GenderBhaviourType "behaviour"; string "presentation"; DateTime "validDate"
                                                            string gat = dataInfo.Substring(0, dp4);
                                                            if (!(checkEnumeration(gat, noOfGenderInfo, currEnum.Gender)))
                                                                anvKonsInfo[noOfGenderInfo].appearance = GenderAppearanceType.UndefGenderAppearance;
                                                            noOfDataSet++;
                                                            dataInfo = dataInfo.Substring(dp4 + 2, dataInfo.Length - dp4 - 2);
                                                            dp5 = dataInfo.IndexOf(";");
                                                            if ((dp5 > 0) && (dp5 < dataInfo.Length - 2))
                                                            {
                                                                // GenderBhaviourType "behaviour"; string "presentation"; DateTime "validDate"
                                                                string gbt = dataInfo.Substring(0, dp5);
                                                                if (!(checkEnumeration(gbt, noOfGenderInfo, currEnum.Gender)))
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
                                                                    handleDate(dataInfo, noOfGenderInfo, currEnum.Gender);
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
                            case "length  ":
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
                                                if (!(checkEnumeration(sUnit, noOfLengthData, currEnum.Length)))
                                                    anvLangdData[noOfLengthData].unit = DistUnits.UndefDistUnit;
                                                noOfDataSet++;
                                                dataInfo = dataInfo.Substring(dp1 + 2, dataInfo.Length - dp1 - 2);
                                                handleDate(dataInfo, noOfLengthData, currEnum.Length);
                                            }
                                            else
                                            {
                                                // We only have unit
                                                if (!(checkEnumeration(dataInfo, noOfLengthData, currEnum.Length)))
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
                            case "weight  ":
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
                                                if (!(checkEnumeration(sUnit, noOfWeightData, currEnum.Weight)))
                                                    anvViktData[noOfWeightData].unit = WeightUnits.UndefWeightUnit;
                                                dataInfo = dataInfo.Substring(dp1 + 2, dataInfo.Length - dp1 - 2);
                                                handleDate(dataInfo, noOfWeightData, currEnum.Weight);
                                            }
                                            else
                                            {
                                                // We only have unit
                                                if (!(checkEnumeration(dataInfo, noOfWeightData, currEnum.Weight)))
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
                            case "boobs   ":
                                {
                                    // Boobs    : Type-tag; Circumf; Unit; Size-tag; Valid-Date
                                    if (noOfChestData < maxNoOfChestData)
                                    {
                                        dp0 = dataInfo.IndexOf(";");
                                        if ((dp0 > 0) && (dp0 < dataInfo.Length - 2))
                                        {
                                            // Type-tag; Circumf; Unit; Size-tag; Valid-Date
                                            string sChTp = dataInfo.Substring(0, dp0);
                                            if (!(checkEnumeration(sChTp, noOfChestData, currEnum.BrstTp)))
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
                                                    if (!(checkEnumeration(sChUnt, noOfChestData, currEnum.BrstSz)))
                                                        anvBrostData[noOfChestData].units = DistUnits.UndefDistUnit;
                                                    dataInfo = dataInfo.Substring(dp2 + 2, dataInfo.Length - dp2 - 2);
                                                    dp3 = dataInfo.IndexOf(";");
                                                    if ((dp3 > 0) && (dp3 < dataInfo.Length - 2))
                                                    {
                                                        // Size-tag; Valid-Date
                                                        string chSzTag = dataInfo.Substring(0, dp3);
                                                        if (!(checkEnumeration(chSzTag, noOfChestData, currEnum.BrstSz)))
                                                            anvBrostData[noOfChestData].sizeType = BreastSizeType.UndefBreastSize;
                                                        dataInfo = dataInfo.Substring(dp3 + 2, dataInfo.Length - dp3 - 2);
                                                        handleDate(dataInfo, noOfChestData, currEnum.BrstSz);
                                                    }
                                                }
                                            }
                                        }
                                        noOfChestData++;
                                    }
                                } break;
                            case "facedata":
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
                                                                if (!(checkEnumeration(sunt, noOfFaceData, currEnum.Face)))
                                                                    anvAnsiktsData[noOfFaceData].units = DistUnits.UndefDistUnit;
                                                                dataInfo = dataInfo.Substring(dp5 + 2, dataInfo.Length - dp5 - 2);
                                                                handleDate(dataInfo, noOfFaceData, currEnum.Face);
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        noOfFaceData++;
                                    }
                                } break;
                            case "residnce":
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
                                            float fresdata;
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
                                                                        if (float.TryParse(spch, out fresdata))
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
                                                                            anvBostader[noOfResidences].boughtDate.year = 0;
                                                                            handleDate(sindat, noOfResidences, currEnum.Residence);
                                                                            dataInfo = dataInfo.Substring(dp8 + 2, dataInfo.Length - dp8 - 2);
                                                                            dp9 = dataInfo.IndexOf(";");
                                                                            if ((dp9 > 0) && (dp9 < dataInfo.Length - 1))
                                                                            {
                                                                                // Out-Date; Sale; Currency
                                                                                string soutdat = dataInfo.Substring(0, dp9);
                                                                                anvBostader[noOfResidences].salesDate.year = 0;
                                                                                handleDate(soutdat, noOfResidences, currEnum.Residence);
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
                            case "haircolr":
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
                                            if (!(checkEnumeration(hrtxt, noOfHairData, currEnum.HairText)))
                                                anvHaar[noOfHairData].textureTag = HairTextureType.UndefHairTexture;
                                            dataInfo = dataInfo.Substring(dp1 + 2, dataInfo.Length - dp1 - 2);
                                            dp2 = dataInfo.IndexOf(";");
                                            if ((dp2 > 0) && (dp2 < dataInfo.Length - 1))
                                            {
                                                string hrlnt = dataInfo.Substring(0, dp2);
                                                if (!(checkEnumeration(hrlnt, noOfHairData, currEnum.HairLnt)))
                                                    anvHaar[noOfHairData].lengthTag = HairLengthType.UndefHairLength;
                                                dataInfo = dataInfo.Substring(dp2 + 2, dataInfo.Length - dp2 - 2);
                                                handleDate(dataInfo, noOfHairData, currEnum.HairLnt);
                                            }
                                        }
                                        noOfHairData++;
                                    }
                                } break;
                            case "eyecolor":
                                {
                                    // EyeColor : Color-tag; Form-tag; Valid-Date; Glasses
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
                                                dp2 = dataInfo.IndexOf(";");
                                                if ((dp2 > 0) && (dp2 < dataInfo.Length - 1))
                                                {
                                                    handleDate(dataInfo.Substring(0, dp2), noOfEyeColorData, currEnum.Eyes);
                                                    dataInfo = dataInfo.Substring(dp2 + 2, dataInfo.Length - dp2 - 2);
                                                    if ((dataInfo.ToLower().Contains("no")) || (dataInfo.ToLower().Contains("none")) || (dataInfo.ToLower().Contains("false")))
                                                        anvEyes[noOfEyeColorData].glasses = false;
                                                    else
                                                        anvEyes[noOfEyeColorData].glasses = true;
                                                }
                                                else
                                                {
                                                    handleDate(dataInfo, noOfEyeColorData, currEnum.Eyes);
                                                    anvEyes[noOfEyeColorData].glasses = false;
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
                            case "markdata":
                                {
                                    // MarkData : type-tag; placement; Motif; Valid-Date
                                    if (noOfMarkingData < maxNoOfMarkingData)
                                    {
                                        // type-tag; placement; Motif; Valid-Date
                                        dp0 = dataInfo.IndexOf(";");
                                        if ((dp0 > 0) && (dp0 < dataInfo.Length))
                                        {
                                            string mrktpe = dataInfo.Substring(0, dp0);
                                            if (!(checkEnumeration(mrktpe, noOfMarkingData, currEnum.MrkTp)))
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
                                                    handleDate(dataInfo, noOfMarkingData, currEnum.MrkTp);
                                                }
                                            }
                                        }
                                        noOfMarkingData++;
                                    }
                                } break;
                            case "ocupatn ":
                                {
                                    // Ocupatn  : Title; Company; Streetname; Number; Cityname; State; Areaname; Zipcode; Country; Start-Date; End-Date
                                    if (noOfOccupationData < maxNoOfOccupationData)
                                    {
                                        dp0 = dataInfo.IndexOf(";");
                                        if ((dp0 > 0) && (dp0 < dataInfo.Length - 2))
                                        {
                                            // Title; Company; Streetname; Number;  Cityname; State; Areaname; Zipcode; Country; Start-Date; End-Date
                                            string ttl = dataInfo.Substring(0, dp0);
                                            if ((ttl != "Unknown") && (ttl != "Undefined"))
                                                anvAnstallningar[noOfOccupationData].title = ttl;
                                            dataInfo = dataInfo.Substring(dp0 + 2, dataInfo.Length - dp0 - 2);
                                            dp1 = dataInfo.IndexOf(";");
                                            if ((dp1 > 0) && (dp1 < dataInfo.Length - 2))
                                            {
                                                // Company; Streetname; Number;  Cityname; State; Areaname; Zipcode; Country; Start-Date; End-Date
                                                string cpny = dataInfo.Substring(0, dp1);
                                                if ((cpny != "Unknown") && (cpny != "Undefined"))
                                                    anvAnstallningar[noOfOccupationData].company = cpny;
                                                dataInfo = dataInfo.Substring(dp1 + 2, dataInfo.Length - dp1 - 2);
                                                dp2 = dataInfo.IndexOf(";");
                                                if ((dp2 > 0) && (dp2 < dataInfo.Length - 2))
                                                {
                                                    // Streetname; Number;  Cityname; State; Areaname; Zipcode; Country; Start-Date; End-Date
                                                    string strtnm = dataInfo.Substring(0, dp2);
                                                    if ((strtnm != "Undefined") && (strtnm != "Unknown"))
                                                        anvAnstallningar[noOfOccupationData].streetname = strtnm;
                                                    dataInfo = dataInfo.Substring(dp2 + 2, dataInfo.Length - dp2 - 2);
                                                    dp3 = dataInfo.IndexOf(";");
                                                    if ((dp3 > 0) && (dp3 < dataInfo.Length - 2))
                                                    {
                                                        // Number; Cityname; State; Areaname; Zipcode; Country; Start-Date; End-Date
                                                        string sno = dataInfo.Substring(0, dp3);
                                                        if ((sno != "Undefined") && (sno != "Unknown"))
                                                            anvAnstallningar[noOfOccupationData].number = int.Parse(sno);
                                                        dataInfo = dataInfo.Substring(dp3 + 2, dataInfo.Length - dp3 - 2);
                                                        int dpp = dataInfo.IndexOf(";");
                                                        if ((dpp > 0) && (dpp < dataInfo.Length - 2))
                                                        {
                                                            //  Cityname; State; Areaname; Zipcode; Country; Start-Date; End-Date
                                                            string cty = dataInfo.Substring(0, dpp);
                                                            if ((cty != "Undefined") && (cty != "Unknown"))
                                                                anvAnstallningar[noOfOccupationData].cityname = cty;
                                                            dataInfo = dataInfo.Substring(dpp + 2, dataInfo.Length - dpp - 2);
                                                            dp4 = dataInfo.IndexOf(";");
                                                            if ((dp4 > 0) && (dp4 < dataInfo.Length - 1))
                                                            {
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
                                                                                anvAnstallningar[noOfOccupationData].started.year = 0;
                                                                                handleDate(stdt, noOfOccupationData, currEnum.Occupation);
                                                                                dataInfo = dataInfo.Substring(dp8 + 2, dataInfo.Length - dp8 - 2);
                                                                                anvAnstallningar[noOfOccupationData].ended.year = 0;
                                                                                handleDate(dataInfo, noOfOccupationData, currEnum.Occupation);
                                                                            }
                                                                            else
                                                                            {
                                                                                // Start-Date
                                                                                anvAnstallningar[noOfOccupationData].started.year = 0;
                                                                                handleDate(dataInfo, noOfOccupationData, currEnum.Occupation);
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
                            case "attended":
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
                                                    anvTillstallningar[noOfAttendedEventData].started.year = 0;
                                                    handleDate(stdt, noOfAttendedEventData, currEnum.Event);
                                                    dataInfo = dataInfo.Substring(dp2 + 2, dataInfo.Length - dp2 - 2);
                                                    // End-Date;  Role
                                                    int dpp = dataInfo.IndexOf(";");
                                                    if ((dpp > 0) && (dpp < dataInfo.Length - 1))
                                                    {
                                                        string stds = dataInfo.Substring(0, dpp);
                                                        anvTillstallningar[noOfAttendedEventData].ended.year = 0;
                                                        handleDate(stds, noOfAttendedEventData, currEnum.Event);
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
                                                        anvTillstallningar[noOfAttendedEventData].ended.year = 0;
                                                        handleDate(dataInfo, noOfAttendedEventData, currEnum.Event);
                                                    }
                                                }
                                                else
                                                {
                                                    anvTillstallningar[noOfAttendedEventData].started.year = 0;
                                                    handleDate(dataInfo, noOfAttendedEventData, currEnum.Event);
                                                }
                                            }
                                        }
                                        noOfAttendedEventData++;
                                    }
                                } break;
                            case "rootdir ":
                                {
                                    if (noOfRootDirs < maxNoOfRootDirs)
                                    {
                                        userRootDir[noOfRootDirs++] = dataInfo;
                                    }
                                } break;
                            case "reltdimg":
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
                            case "national":
                                {
                                    // National : Nationality-name[; tag]
                                    if (noOfNationalityData < maxNoOfNationalityData)
                                    {
                                        dp0 = dataInfo.IndexOf(";");
                                        if ((dp0 > 0) && (dp0 < dataInfo.Length - 1))
                                        {
                                            string natnam = dataInfo.Substring(0, dp0);
                                            string nattag = dataInfo.Substring(dp0 + 2, dataInfo.Length - dp0 - 2);
                                            Boolean foundItem = false;
                                            for (int i = 0; i < noOfNationalityCategories; i++)
                                            {
                                                if ((nationalityCategories[i].tag == nattag) && (nationalityCategories[i].description == natnam))
                                                    foundItem = true;
                                            }
                                            if (!(foundItem))
                                                addNationalityCategory(nattag, natnam, "Open");
                                            anvNationalities[noOfNationalityData].nationName = natnam;
                                            anvNationalities[noOfNationalityData].nationTag = nattag;
                                        }
                                        else
                                            anvNationalities[noOfNationalityData].nationName = dataInfo;
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
            if ((storagePath != "") && (System.IO.File.Exists(storagePath)))
            {
                filename = storagePath;
            }
            else if ((storagePath != "") && (System.IO.Directory.Exists(storagePath)) &&
                (System.IO.File.Exists(storagePath + "\\ActorData_" + userID + ".acf")))
            {
                filename = storagePath + "\\ActorData_" + userID + ".acf";
            }
            else
            {
                return false;
            }
            using (System.IO.FileStream afs = System.IO.File.Create(filename))
            {
                using (System.IO.StreamWriter sw = new System.IO.StreamWriter(afs))
                {
                    #region UserId   : <ID>
                    var line = "UserId   : " + this.userId; // + Environment.NewLine;
                    sw.WriteLine(line);
                    #endregion
                    #region UserName : <Name>[ <midname>][ <familyname>]; <NameTag>]
                    for (int i = 0; i < this.getNoOfUserNames(); i++)
                    {
                        line = "UserName : " + this.getUserSurName(i) + " " + this.getUserMidName(i) + " " + this.getUserFamName(i) + "; " + this.getUserNameTag(i);
                        sw.WriteLine(line);
                    }
                    #endregion
                    #region Contact  : <ContactTypeTag>; <Path>
                    for (int i = 0; i < this.getNoOfUserContacts(); i++)
                    {
                        line = "Contact  : " + this.getUserContactType(i) + "; " + this.getUserContactPath(i);
                        sw.WriteLine(line);
                    }
                    #endregion
                    #region BrthData : <StreetName>; <Number>; <AddOn>; <CityName>; <AreaName>; <ZipCode>; <Country>; <Date>; <SocSecNo>; <Gender>; <Latitude>; <Longitude>
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
                    #endregion
                    #region SkinTone : <SkinToneTag>; <RedValue>; <GreenValue>; <BlueValue>; <Date>
                    for (int i = 0; i < getNumberOfSkinTones(); i++)
                    {
                        line = "SkinTone : " + getUserSkinToneTag(i) + "; " + getUserSkinToneRedChannel(i) + "; " + getUserSkinToneGreenChannel(i) + "; " + getUserSkinToneBlueChannel(i) + "; " + getUserSkinToneValidDate(i);
                        sw.WriteLine(line);
                    }
                    #endregion
                    #region RelStats : <RelationTag>; <Description>; <SecurityLevelTag>
                    line = "RelStats : " + anvAktuellRelation.tag + "; " + anvAktuellRelation.description + "; " + anvAktuellRelation.level.ToString();
                    sw.WriteLine(line);
                    #endregion
                    #region Gender   : <GenderTypeTag>; <Length>; <Circ.>; <UnitTag>; <AppearanceTag>; <BehaviourTag>; <Presentation>; <Date>
                    for (int i = 0; i < getNumberOfGenderData(); i++)
                    {
                        line = "Gender   : " + getUserGenderType(i);
                        line = line + "; " + getUserGenderLengthValue(i);
                        line = line + "; " + getUserGenderCircumfValue(i);
                        line = line + "; " + getUserGenderUnit(i);
                        line = line + "; " + getUserGenderAppearance(i);
                        line = line + "; " + getUserGenderBehaviour(i);
                        line = line + "; " + getUserGenderPres(i);
                        line = line + "; " + getUserGenderInfoValidDate(i);
                        sw.WriteLine(line);
                    }
                    #endregion
                    #region Weight   : <Value>; <UnitTag>; <Date>
                    for (int i = 0; i < getNumberOfWeightData(); i++)
                    {
                        line = "Weight   : " + getUserWeightVal(i) + "; " + getUserWeightUnit(i) + "; " + getUserWeightInfoValidDate(i);
                        sw.WriteLine(line);
                    }
                    #endregion
                    #region Length  : <Value>; <UnitTag; <date>
                    for (int i = 0; i < getNumberOfLengthData(); i++)
                    {
                        line = "Length   : " + getUserLengthVal(i) + "; " + getUserLengthUnit(i) + "; " + getUserLengthInfoValidDate(i);
                        sw.WriteLine(line);
                    }
                    #endregion
                    #region Boobs    : <TypeTag>; <Circ.>; <UnitTag>; <SizeTag>; <Date>
                    for (int i = 0; i < getNumberOfChestData(); i++)
                    {
                        line = "Boobs    : " + getUserChestType(i);
                        line = line + "; " + getUserChestCircumfVal(i);
                        line = line + "; " + getUserChestCircumfUnit(i);
                        line = line + "; " + getUserChestSizeType(i);
                        line = line + "; " + getUserChestInfoValidDate(i);
                        sw.WriteLine(line);
                    }
                    #endregion
                    #region HairColr : <ColorTag>; <TextureTag>; <LengthTag>; <Date>
                    for (int i = 0; i < getNumberOfHairData(); i++)
                    {
                        line = "HairColr : " + getUserHairColor(i);
                        line = line + "; " + getUserHairTexture(i);
                        line = line + "; " + getUserHairLength(i);
                        line = line + "; " + getUserHairValidDate(i);
                        sw.WriteLine(line);
                    }
                    #endregion
                    #region MarkData : <TypeTag>; <Place>; <Motif>; <Date>
                    for (int i = 0; i < getNoOfMarkingsData(); i++)
                    {
                        // MarkData : type-tag; placement; Motif; Valid-Date
                        line = "MarkData : " + getActorMarkingType(i) + "; " + getActorMarkingPlace(i) + "; " + getActorMarkingMotif(i) + "; " + getActorMarkingValidDate(i);
                        sw.WriteLine(line);
                    }
                    #endregion
                    #region Occupatn : <Title>; <Company>; <Streetname>; <Number>; <State>; <Area>; <ZipCode>; <Country>; <StartDate>[; <EndDate>]
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
                    #endregion
                    #region [FaceData : <EyeWidth>; <CheekboneWidth>; <ChinWidth>; <MouthWidth>; <Height>; <UnitTag>; <Date>]
                    for (int i = 0; i < getNumberOfFaceData(); i++)
                    {
                        line = "FaceData : " + getFaceEyeWidth(i);
                        line = line + getFaceCheekboneWidth(i);
                        line = line + getFaceChinWidth(i);
                        line = line + getFaceMouthWidth(i);
                        line = line + getFaceHeight(i);
                        line = line + getFaceUsedUnit(i);
                        line = line + getFaceInfoValidDate(i);
                        sw.WriteLine(line);
                    }
                    #endregion
                    #region Residnce : <Street>; <No>; <Add>; <City>, <ZipCode>; <Country>; <BoughtFor>; <BoughtDate>; <SaleDate>; <SoldFor>; <CurrencyTag>
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
                    #endregion
                    #region [EyeColor : <ColorTag>; <EyeFormTag>; <Date>]
                    for (int i = 0; i < getNumberOfEyeData(); i++)
                    {
                        // EyeColor : Color-tag; Form-tag; Valid-Date
                        line = "EyeColor : " + getUserEyeColorTag(i) + "; " + getUserEyeFormTag(i) + "; " + getUserEyeDataValidDate(i);
                        sw.WriteLine(line);
                    }
                    #endregion
                    #region Attended : <EventID>; <EventTypeTag>; <StartDateTime>; <EndDateTime>; <RoleTag>
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
                    #endregion
                    #region ReltdImg : <PathNameExt>; <ContentTag>; <SecurityLevelTag>
                    for (int i = 0; i < getNoOfRelatedImages(); i++)
                    {
                        // ReltdImg : Path-Name-ext; Type-Of-Content-Tag; Security-Level
                        line = "ReltdImg : " + getActorRelatedImagePath(i) + "; " + getActorRelatedImageContent(i) + "; " + getActorRelatedImageClass(i);
                        sw.WriteLine(line);
                    }
                    #endregion
                    #region RootDir  : <RootPath>
                    for (int i = 0; i < noOfRootDirs; i++)
                    {
                        // RootDir  : root dir path
                        line = "RootDir  : " + getActorRootDir(i);
                        sw.WriteLine(line);
                    }
                    #endregion
                    #region [National : <NationalityTag>]
                    for (int i = 0; i < getNoOfNationalityData(); i++)
                    {
                        // National : Nationality-name
                        line = "National : " + getNationalityData(i);
                        sw.WriteLine(line);
                    }
                    #endregion
                    // Last line of this "using"-clause.
                    sw.Close();
                }
                afs.Close();
            }
            return retVal;
        }
        public int getNoOfBreastSizeStrings() { return BreastSizeStrings.Length; }
        public string getBreastSizeString(int nr) { return BreastSizeStrings[nr]; }
        public int getNoOfBreastTypeStrings() { return BreastTypeStrings.Length; }
        public string getBreastTypeString(int nr) { return BreastTypeStrings[nr]; }
        public int getNoOfHairTextureStrings() { return HairTextureStrings.Length; }
        public string getHairTextureString(int nr) { return HairTextureStrings[nr]; }
        public int getNoOfHairLengthStrings() { return HairLengthStrings.Length; }
        public string getHairLengthString(int nr) { return HairLengthStrings[nr]; }
        public int getNoOfSecrecyStrings() { return SecrecyLevelStrings.Length; }
        public string getSecrecyString(int nr) { return SecrecyLevelStrings[nr]; }
        public int getNoOfMarkingStrings() { return MarkingsTypeStrings.Length; }
        public string getMarkingString(int nr) { return MarkingsTypeStrings[nr]; }
        #region category handling
        #region EventCategories
        public int getNoOfEventCategories() { return noOfEventCategories; }
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
        public string getEventCategoryTag(int nr) { return eventCategories[nr].tag; }
        public string getEventCategoryDescription(int nr) { return eventCategories[nr].description; }
        public int getEventCategoryLevel(int nr) { return eventCategories[nr].level; }
        public bool removeEventCategory(int nr)
        {
            bool retVal = false;
            if ((nr <= 0) && (nr < noOfEventCategories))
            {
                for (int i = nr; i < noOfEventCategories; i++)
                {
                    eventCategories[i].description = eventCategories[i + 1].description;
                    eventCategories[i].level = eventCategories[i + 1].level;
                    eventCategories[i].tag = eventCategories[i + 1].tag;
                    eventCategories[i].value = eventCategories[i + 1].value;
                }
                retVal = true;
                noOfEventCategories--;
            }
            return retVal;
        }
        #endregion
        #region ContentCategories
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
        public int getNoOfContentCategories() { return noOfContentCategories; }
        public string getContentCategoryTag(int nr) { return contentCategories[nr].tag; }
        public string getContentCategoryDescription(int nr) { return contentCategories[nr].description; }
        public int getContentCateogryLevel(int nr) { return contentCategories[nr].level; }
        public bool removeContentCategory(int nr)
        {
            bool retVal = false;
            if ((nr <= 0) && (nr < noOfContentCategories))
            {
                for (int i = nr; i < noOfContentCategories; i++)
                {
                    contentCategories[i].description = contentCategories[i + 1].description;
                    contentCategories[i].level = contentCategories[i + 1].level;
                    contentCategories[i].tag = contentCategories[i + 1].tag;
                    contentCategories[i].value = contentCategories[i + 1].value;
                }
                retVal = true;
                noOfContentCategories--;
            }
            return retVal;
        }
        #endregion
        #region ContextCategories
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
        public int getNoOfContextCategories() { return noOfContextCategories; }
        public string getContextCategoryTag(int nr) { return contextCategories[nr].tag; }
        public string getContextCategoryDescription(int nr) { return contextCategories[nr].description; }
        public int getContextCategoryLevel(int nr) { return contextCategories[nr].level; }
        public bool removeContextCategory(int nr)
        {
            bool retVal = false;
            if ((nr <= 0) && (nr < noOfContextCategories))
            {
                for (int i = nr; i < noOfContextCategories; i++)
                {
                    contextCategories[i].description = contextCategories[i + 1].description;
                    contextCategories[i].level = contextCategories[i + 1].level;
                    contextCategories[i].tag = contextCategories[i + 1].tag;
                    contextCategories[i].value = contextCategories[i + 1].value;
                }
                retVal = true;
                noOfContextCategories--;
            }
            return retVal;
        }
        #endregion
        #region RelationCategories
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
        public int getNoOfRelationCategories() { return noOfRelationCategories; }
        public string getRelationCategoryTag(int nr) { return relationCategories[nr].tag; }
        public string getRelationCategoryDescription(int nr) { return relationCategories[nr].description; }
        public int getRelationCategoryLevel(int nr) { return relationCategories[nr].level; }
        public bool removeRelationCategory(int nr)
        {
            bool retVal = false;
            if ((nr >= 0) && (nr < noOfRelationCategories))
            {
                for (int i = nr; i < noOfRelationCategories; i++)
                {
                    relationCategories[i].description = relationCategories[i + 1].description;
                    relationCategories[i].level = relationCategories[i + 1].level;
                    relationCategories[i].tag = relationCategories[i + 1].tag;
                    relationCategories[i].value = relationCategories[i + 1].value;
                }
                retVal = true;
                noOfRelationCategories--;
            }
            return retVal;
        }
        #endregion
        #region NationalityCategories
        public bool addNationalityCategory(string tag, string desc, string level)
        {
            Boolean retVal = false;
            if (noOfNationalityCategories < maxNoOfNationalityCategories)
            {
                nationalityCategories[noOfNationalityCategories].tag = tag;
                nationalityCategories[noOfNationalityCategories].description = desc;
                if (level == "Open")
                    nationalityCategories[noOfNationalityCategories].level = 1;
                else if (level == "Limited")
                    nationalityCategories[noOfNationalityCategories].level = 2;
                else if (level == "Medium")
                    nationalityCategories[noOfNationalityCategories].level = 3;
                else if (level == "Relative")
                    nationalityCategories[noOfNationalityCategories].level = 4;
                else if (level == "Secret")
                    nationalityCategories[noOfNationalityCategories].level = 5;
                else if (level == "QualifSecret")
                    nationalityCategories[noOfNationalityCategories].level = 6;
                else
                    nationalityCategories[noOfNationalityCategories].level = 0;
                noOfNationalityCategories++;
                retVal = true;
            }
            return retVal;
        }
        public int getNoOfNationalityCategories() { return noOfNationalityCategories; }
        public string getNationalityCategoryTag(int nr) { return nationalityCategories[nr].tag; }
        public string getNationalityCategoryDescription(int nr) { return nationalityCategories[nr].description; }
        public int getNationalityCategoryLevel(int nr) { return nationalityCategories[nr].level; }
        public bool removeNationalityCategory(int nr)
        {
            bool retVal = false;
            if ((nr >= 0) && (nr < noOfNationalityCategories))
            { 
                for (int i = nr; i < noOfNationalityCategories; i++)
                {
                    nationalityCategories[i].description = nationalityCategories[i + 1].description;
                    nationalityCategories[i].level = nationalityCategories[i + 1].level;
                    nationalityCategories[i].tag = nationalityCategories[i + 1].tag;
                    nationalityCategories[i].value = nationalityCategories[i + 1].value;
                }
                retVal = true;
                noOfNationalityCategories--;
            }
            return retVal;
        }
        #endregion
        #region ComplexionCategories
        public bool addComplexionCategory(string tag, string desc, string level)
        {
            bool retVal = false;
            if (noOfComplexions < maxNoOfComplexions)
            {
                bool foundCategory = false;
                for (int i = 0; i < maxNoOfComplexionCategories; i++)
                {
                    if (tag == complexionCategory[i].tag)
                        foundCategory = true;
                }
                if (!(foundCategory))
                {
                    complexionCategory[noOfComplexionCategories].tag = tag;
                    complexionCategory[noOfComplexionCategories].description = desc;
                    if (level.ToLower() == "unclassified")
                        complexionCategory[noOfComplexionCategories].level = 1;
                    else if (level.ToLower() == "limited")
                        complexionCategory[noOfComplexionCategories].level = 2;
                    else if (level.ToLower() == "confidential")
                        complexionCategory[noOfComplexionCategories].level = 3;
                    else if (level.ToLower() == "secret")
                        complexionCategory[noOfComplexionCategories].level = 4;
                    else if (level.ToLower() == "qualifsecret")
                        complexionCategory[noOfComplexionCategories].level = 5;
                    else
                        complexionCategory[noOfComplexionCategories].level = 0;
                    retVal = true;
                    noOfComplexionCategories++;
                }
            }
            return retVal;
        }
        public int getNoOfComplexionCategories() { return noOfComplexionCategories; }
        public string getComplexionCategoryTag(int nr) { return complexionCategory[nr].tag; }
        public string getComplexionCategoryDescirption(int nr) { return complexionCategory[nr].description; }
        public int getComplexionCategoryLevel(int nr) { return complexionCategory[nr].level; }
        public bool removeComplexionCategory(int nr)
        {
            bool retVal = false;
            if ((nr >= 0) && (nr < noOfComplexionCategories))
            {
                for (int i = nr; i < noOfComplexionCategories; i++)
                {
                    complexionCategory[i].tag = complexionCategory[i + 1].tag;
                    complexionCategory[i].description = complexionCategory[i + 1].description;
                    complexionCategory[i].level = complexionCategory[i + 1].level;
                    retVal = true;
                    noOfComplexionCategories--;
                }
            }
            return retVal;
        }
        #endregion
        #region CurrencyCategories
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
        public int getNoOfCurrencyCategories() { return noOfCurrencyCategories; }
        public string getCurrencyCategoryTag(int nr) { return currencyCategory[nr].tag; }
        public string getCurrencyCategoryDescription(int nr) { return currencyCategory[nr].description; }
        public int getCurrencyCategoryLevel(int nr) { return currencyCategory[nr].level; }
        public double getCurrencyCategoryValue(int nr) { return currencyCategory[nr].value; }
        public bool removeCurrencyCategory(int nr)
        {
            bool retVal = false;
            if ((nr <= 0) && (nr < noOfCurrencyCategories))
            {
                for (int i = nr; i < noOfCurrencyCategories; i++)
                {
                    currencyCategory[i].description = currencyCategory[i + 1].description;
                    currencyCategory[i].level = currencyCategory[i + 1].level;
                    currencyCategory[i].tag = currencyCategory[i + 1].tag;
                    currencyCategory[i].value = currencyCategory[i + 1].value;
                }
                retVal = true;
                noOfCurrencyCategories--;
            }
            return retVal;
        }
        #endregion
        #region ContactCategory
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
        public int getNoOfContactCategories() { return noOfContactCategories; }
        public string getContactCategoryTag(int nr) { return contactCategory[nr].tag; }
        public string getContactCategoryDescription(int nr) { return contactCategory[nr].description; }
        public int getContactCategoryLevel(int nr) { return contactCategory[nr].level; }
        public bool removeContactCategory(int nr)
        {
            bool retVal = false;
            if ((nr <= 0) && (nr < noOfContactCategories))
            {
                for (int i = nr; i < noOfContactCategories; i++)
                {
                    contactCategory[i].description = contactCategory[i + 1].description;
                    contactCategory[i].level = contactCategory[i + 1].level;
                    contactCategory[i].tag = contactCategory[i + 1].tag;
                    contactCategory[i].value = contactCategory[i + 1].value;
                }
                retVal = true;
                noOfContactCategories--;
            }
            return retVal;
        }
        #endregion
        #region HairColorCategory
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
        public int getNoOfHairColorCategories() { return noOfHairColorCategories; }
        public string getHairColorCategoryTag(int nr) { return hairColorCategory[nr].tag; }
        public string getHairColorCategoryDescription(int nr) { return hairColorCategory[nr].description; }
        public int getHairColorCategoryLevel(int nr) { return hairColorCategory[nr].level; }
        public bool removeHairColorCategory(int nr)
        {
            bool retVal = false;
            if ((nr <= 0) && (nr < noOfHairColorCategories))
            {
                for (int i = nr; i < noOfHairColorCategories; i++)
                {
                    hairColorCategory[i].description = hairColorCategory[i + 1].description;
                    hairColorCategory[i].level = hairColorCategory[i + 1].level;
                    hairColorCategory[i].tag = hairColorCategory[i + 1].tag;
                    hairColorCategory[i].value = hairColorCategory[i + 1].value;
                }
                retVal = true;
                noOfHairColorCategories--;
            }
            return retVal;
        }
        #endregion
        #region RoleCategory
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
        public int getNoOfRoleCategories() { return noOfRoleCategories; }
        public string getRoleCategoryTag(int nr) { return roleCategories[nr].tag; }
        public string getRoleCategoryDescription(int nr) { return roleCategories[nr].description; }
        public int getRoleCategoryLevel(int nr) { return roleCategories[nr].level; }
        public bool removeRoleCategory(int nr)
        {
            bool retVal = false;
            if ((nr <= 0) && (nr < noOfRoleCategories))
            {
                for (int i = nr; i < noOfRoleCategories; i++)
                {
                    roleCategories[i].description = roleCategories[i + 1].description;
                    roleCategories[i].level = roleCategories[i + 1].level;
                    roleCategories[i].tag = roleCategories[i + 1].tag;
                    roleCategories[i].value = roleCategories[i + 1].value;
                }
                retVal = true;
                noOfRoleCategories--;
            }
            return retVal;
        }
        #endregion
        #endregion
        #region names and ID
        public string getUserId() { return userId; }
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
                if (!(checkEnumeration(type, noOfNames, currEnum.NmeTp)))
                    anvNamn[noOfNames].nameType = NameType.UndefNameType;
                anvNamn[noOfNames].Surname = surn;
                anvNamn[noOfNames].Midname = midn;
                anvNamn[noOfNames].Famname = famn;
                return true;
            }
            else
                return false;
        }
        public bool removeUserName(int nr)
        {
            bool retVal = false;
            if ((nr >= 0) && (nr < noOfNames))
            {
                for (int i = nr; i < noOfNames; i++)
                {
                    anvNamn[i].Famname = anvNamn[i + 1].Famname;
                    anvNamn[i].Midname = anvNamn[i + 1].Midname;
                    anvNamn[i].nameType = anvNamn[i + 1].nameType;
                    anvNamn[i].Surname = anvNamn[i + 1].Surname;
                }
                retVal = true;
                noOfNames--;
            }
            return retVal;
        }
        #endregion
        #region contact handling
        public int getNoOfUserContacts() { return noOfContacts; }
        public string getUserContactType(int nr) 
        {
            if ((nr >= 0) && (nr < maxNoOfContacts) && (anvKontakter[nr].type.tag != null))
                return anvKontakter[nr].type.tag;
            else
                return "";
        }
        public string getUserContactPath(int nr)
        {
            if ((nr >= 0) && (nr < maxNoOfContacts))
                return anvKontakter[nr].contactPath;
            else
                return "";
        }
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
        public bool removeUserContact(int nr)
        {
            bool retVal = false;
            if ((nr >= 0) && (nr < noOfContacts))
            {
                for (int i = nr; i < noOfContacts; i++)
                {
                    anvKontakter[i].contactPath = anvKontakter[i + 1].contactPath;
                    anvKontakter[i].type = anvKontakter[i + 1].type;
                }
                retVal = true;
                noOfContacts--;
            }
            return retVal;
        }
        #endregion
        #region birthData
        public string getUserBirthStreet() { return anvFodlseData.brthStreet; }
        public string getUserBirthStreetNumber() { return anvFodlseData.brthStreetNumber.ToString(); }
        public string getUserBirthStreetNumberAddon() { return anvFodlseData.brthStreetNumberAddon; }
        public string getUserBirthCityname() { return anvFodlseData.brthCityname; }
        public string getUserBirthAreaname() { return anvFodlseData.brthAreaname; }
        public string getUserBirthZipcode() { return anvFodlseData.brthZipcode; }
        public string getUserBirthCountry() { return anvFodlseData.brthCountryname; }
        public string getUserBirthDate() 
        {
            if ((anvFodlseData.brthDate.year > 0) && (anvFodlseData.brthDate.month > 9) && (anvFodlseData.brthDate.day > 9))
                return anvFodlseData.brthDate.year.ToString() + "-" + anvFodlseData.brthDate.month.ToString() + "-" + anvFodlseData.brthDate.day.ToString();
            else if ((anvFodlseData.brthDate.year > 0) && (anvFodlseData.brthDate.month > 9) && (anvFodlseData.brthDate.day > 0))
                return anvFodlseData.brthDate.year.ToString() + "-" + anvFodlseData.brthDate.month.ToString() + "-0" + anvFodlseData.brthDate.day.ToString();
            else if ((anvFodlseData.brthDate.year > 0) && (anvFodlseData.brthDate.month > 0) && (anvFodlseData.brthDate.day > 9))
                return anvFodlseData.brthDate.year.ToString() + "-0" + anvFodlseData.brthDate.month.ToString() + "-" + anvFodlseData.brthDate.day.ToString();
            else if ((anvFodlseData.brthDate.year > 0) && (anvFodlseData.brthDate.month > 0) && (anvFodlseData.brthDate.day > 0))
                return anvFodlseData.brthDate.year.ToString() + "-0" + anvFodlseData.brthDate.month.ToString() + "-0" + anvFodlseData.brthDate.day.ToString();
            else if ((anvFodlseData.brthDate.year > 0) && (anvFodlseData.brthDate.month > 9))
                return anvFodlseData.brthDate.year.ToString() + "-" + anvFodlseData.brthDate.month.ToString();
            else if ((anvFodlseData.brthDate.year > 0) && (anvFodlseData.brthDate.month > 0))
                return anvFodlseData.brthDate.year.ToString() + "-0" + anvFodlseData.brthDate.month.ToString();
            else if (anvFodlseData.brthDate.year > 0)
                return anvFodlseData.brthDate.year.ToString();
            else
                return "";
        }
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
                    if (!(checkEnumeration(dirstr, 0, currEnum.BrthDta)))
                        anvFodlseData.latDir = GeogrDir.UndefGeoDir;
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
                        if (!(checkEnumeration(dirstr, 0, currEnum.BrthDta)))
                            anvFodlseData.latDir = GeogrDir.UndefGeoDir;
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
                        if (!(checkEnumeration(dirstr, 0, currEnum.BrthDta)))
                            anvFodlseData.latDir = GeogrDir.UndefGeoDir;
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
                    if (!(checkEnumeration(dirstr, 0, currEnum.BrthDta)))
                        anvFodlseData.latDir = GeogrDir.UndefGeoDir;
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
                        if (!(checkEnumeration(dirstr, 0, currEnum.BrthDta)))
                            anvFodlseData.latDir = GeogrDir.UndefGeoDir;
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
                                if (!(checkEnumeration(indata, 0, currEnum.BrthDta)))
                                    anvFodlseData.latDir = GeogrDir.UndefGeoDir;
                            }
                            else
                            {
                                // Only degrees, minutes, seconds, and direction
                                anvFodlseData.latVal = (degrint * 10000) + (minint * 100) + (secint * 1);// + (semisecint / 100);
                                if (!(checkEnumeration(indata, 0, currEnum.BrthDta)))
                                    anvFodlseData.latDir = GeogrDir.UndefGeoDir;
                            }
                        }
                        else
                        {
                            // Only degrees, minutes, and direction
                            anvFodlseData.latVal = (degrint * 10000) + (minint * 100);// + (secint * 1) + (semisecint / 100);
                            if (!(checkEnumeration(indata, 0, currEnum.BrthDta)))
                                anvFodlseData.latDir = GeogrDir.UndefGeoDir;
                        }
                    }
                    else
                    {
                        // Possibly direction
                        anvFodlseData.latVal = (degrint * 10000);// + (minint * 100) + (secint * 1) + (semisecint / 100);
                        if (!(checkEnumeration(indata, 0, currEnum.BrthDta)))
                            anvFodlseData.latDir = GeogrDir.UndefGeoDir;
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
                            if (!(checkEnumeration(indata, 0, currEnum.BrthDta)))
                                anvFodlseData.latDir = GeogrDir.UndefGeoDir;
                        }
                        else
                        {
                            // Format: DDD.MM.SS [East|east|E|e]|[West|west|W|w]
                            anvFodlseData.lonVal = (degpart * 10000) + (minpart * 100) + secpart;
                            if (!(checkEnumeration(indata, 0, currEnum.BrthDta)))
                                anvFodlseData.latDir = GeogrDir.UndefGeoDir;
                        }
                    }
                    else
                    {
                        // Format: DDD.MM [East|east|E|e]|[West|west|W|w]
                        anvFodlseData.lonVal = (degpart * 10000) + (minpart * 100);
                        if (!(checkEnumeration(indata, 0, currEnum.BrthDta)))
                            anvFodlseData.latDir = GeogrDir.UndefGeoDir;
                    }
                }
                else
                {
                    // Format DDD [East|east|E|e]|[West|west|W|w]
                    anvFodlseData.lonVal = (degpart * 10000);
                    if (!(checkEnumeration(indata, 0, currEnum.BrthDta)))
                        anvFodlseData.latDir = GeogrDir.UndefGeoDir;
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
                                if (!(checkEnumeration(indata, 0, currEnum.BrthDta)))
                                    anvFodlseData.latDir = GeogrDir.UndefGeoDir;
                            }
                            else
                            {
                                anvFodlseData.lonVal = (degpart * 10000) + (minpart * 100) + secpart;
                                if (!(checkEnumeration(indata, 0, currEnum.BrthDta)))
                                    anvFodlseData.latDir = GeogrDir.UndefGeoDir;
                            }
                        }
                        else
                        {
                            anvFodlseData.lonVal = (degpart * 10000) + (minpart * 100);
                            if (!(checkEnumeration(indata, 0, currEnum.BrthDta)))
                                anvFodlseData.latDir = GeogrDir.UndefGeoDir;
                        }
                    }
                    else
                    {
                        // Format: DDD [East|east|E|e]|[West|west|W|w]
                        anvFodlseData.lonVal = (degpart * 10000);
                        if (!(checkEnumeration(indata, 0, currEnum.BrthDta)))
                            anvFodlseData.latDir = GeogrDir.UndefGeoDir;
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
            retVal = handleDate(instr, 0, currEnum.BrthDta);
            return retVal;
        }
        #endregion
        #region faceData
        // --- Face data ---
        public int getNumberOfFaceData() { return noOfFaceData; }
        public string getFaceEyeWidth(int nr) { return anvAnsiktsData[nr].eyeWidth.ToString(); }
        public string getFaceCheekboneWidth(int nr) { return anvAnsiktsData[nr].cheekboneWidth.ToString(); }
        public string getFaceChinWidth(int nr) { return anvAnsiktsData[nr].chinWidth.ToString(); }
        // Height; Unit; Valid-Date
        public string getFaceMouthWidth(int nr) { return anvAnsiktsData[nr].mouthWidth.ToString(); }
        public string getFaceHeight(int nr) { return anvAnsiktsData[nr].height.ToString(); }
        public string getFaceUsedUnit(int nr) { return anvAnsiktsData[nr].units.ToString(); }
        public string getFaceInfoValidDate(int nr)
        {
            if ((anvAnsiktsData[nr].validDate.year > 0) && (anvAnsiktsData[nr].validDate.month > 9) && (anvAnsiktsData[nr].validDate.day > 9))
                return anvAnsiktsData[nr].validDate.year.ToString() + "-" + anvAnsiktsData[nr].validDate.month.ToString() + "-" + anvAnsiktsData[nr].validDate.day.ToString();
            else if ((anvAnsiktsData[nr].validDate.year > 0) && (anvAnsiktsData[nr].validDate.month > 9) && (anvAnsiktsData[nr].validDate.day > 0))
                return anvAnsiktsData[nr].validDate.year.ToString() + "-" + anvAnsiktsData[nr].validDate.month.ToString() + "-0" + anvAnsiktsData[nr].validDate.day.ToString();
            else if ((anvAnsiktsData[nr].validDate.year > 0) && (anvAnsiktsData[nr].validDate.month > 0) && (anvAnsiktsData[nr].validDate.day > 9))
                return anvAnsiktsData[nr].validDate.year.ToString() + "-0" + anvAnsiktsData[nr].validDate.month.ToString() + "-" + anvAnsiktsData[nr].validDate.day.ToString();
            else if ((anvAnsiktsData[nr].validDate.year > 0) && (anvAnsiktsData[nr].validDate.month > 0) && (anvAnsiktsData[nr].validDate.day > 0))
                return anvAnsiktsData[nr].validDate.year.ToString() + "-0" + anvAnsiktsData[nr].validDate.month.ToString() + "-0" + anvAnsiktsData[nr].validDate.day.ToString();
            else if ((anvAnsiktsData[nr].validDate.year > 0) && (anvAnsiktsData[nr].validDate.month > 9))
                return anvAnsiktsData[nr].validDate.year.ToString() + "-" + anvAnsiktsData[nr].validDate.month.ToString();
            else if ((anvAnsiktsData[nr].validDate.year > 0) && (anvAnsiktsData[nr].validDate.month > 0))
                return anvAnsiktsData[nr].validDate.year.ToString() + "-0" + anvAnsiktsData[nr].validDate.month.ToString();
            else if ((anvAnsiktsData[nr].validDate.year > 0))
                return anvAnsiktsData[nr].validDate.year.ToString();
            else
                return "";
        }
        public bool setFaceInfoData(int nr, int ewdt, int cbwdt, int chnwdt, int mthwdt, int fcehgt, string usdunt, string dat)
        {
            bool retVal = false;
            if (nr < maxNoOfFaceData)
            {
                anvAnsiktsData[nr].eyeWidth = ewdt;
                anvAnsiktsData[nr].cheekboneWidth = cbwdt;
                anvAnsiktsData[nr].chinWidth = chnwdt;
                anvAnsiktsData[nr].mouthWidth = mthwdt;
                anvAnsiktsData[nr].height = fcehgt;
                if (checkEnumeration(usdunt, nr, currEnum.Face))
                    retVal = handleDate(dat, nr, currEnum.Face);
            }
            return retVal;
        }
        public bool removeFaceInfoData(int nr)
        {
            bool retVal = false;
            if ((nr >= 0) && (nr < noOfFaceData))
            {
                for (int i = nr; i < noOfFaceData; i++)
                {
                    anvAnsiktsData[i].cheekboneWidth = anvAnsiktsData[i + 1].cheekboneWidth;
                    anvAnsiktsData[i].chinWidth = anvAnsiktsData[i + 1].chinWidth;
                    anvAnsiktsData[i].eyeWidth = anvAnsiktsData[i + 1].eyeWidth;
                    anvAnsiktsData[i].height = anvAnsiktsData[i + 1].height;
                    anvAnsiktsData[i].mouthWidth = anvAnsiktsData[i + 1].mouthWidth;
                    anvAnsiktsData[i].units = anvAnsiktsData[i + 1].units;
                    anvAnsiktsData[i].validDate = anvAnsiktsData[i + 1].validDate;
                }
                retVal = true;
                noOfFaceData--;
            }
            return retVal;
        }
        #endregion
        #region Etnicity
        // --- Skin tones ---
        public int getNumberOfSkinTones() { return noOfComplexions; }
        public string getUserSkinToneValidDate(int nr)
        {
            if ((anvHudfarg[nr].validDate.year > 0) && (anvHudfarg[nr].validDate.month > 9) && (anvHudfarg[nr].validDate.day > 9))
                return anvHudfarg[nr].validDate.year.ToString() + "-" + anvHudfarg[nr].validDate.month.ToString() + "-" + anvHudfarg[nr].validDate.day.ToString();
            else if ((anvHudfarg[nr].validDate.year > 0) && (anvHudfarg[nr].validDate.month > 9) && (anvHudfarg[nr].validDate.day > 0))
                return anvHudfarg[nr].validDate.year.ToString() + "-" + anvHudfarg[nr].validDate.month.ToString() + "-0" + anvHudfarg[nr].validDate.day.ToString();
            else if ((anvHudfarg[nr].validDate.year > 0) && (anvHudfarg[nr].validDate.month > 0) && (anvHudfarg[nr].validDate.day > 9))
                return anvHudfarg[nr].validDate.year.ToString() + "-0" + anvHudfarg[nr].validDate.month.ToString() + "-" + anvHudfarg[nr].validDate.day.ToString();
            else if ((anvHudfarg[nr].validDate.year > 0) && (anvHudfarg[nr].validDate.month > 0) && (anvHudfarg[nr].validDate.day > 0))
                return anvHudfarg[nr].validDate.year.ToString() + "-0" + anvHudfarg[nr].validDate.month.ToString() + "-0" + anvHudfarg[nr].validDate.day.ToString();
            else if ((anvHudfarg[nr].validDate.year > 0) && (anvHudfarg[nr].validDate.month > 9))
                return anvHudfarg[nr].validDate.year.ToString() + "-" + anvHudfarg[nr].validDate.month.ToString();
            else if ((anvHudfarg[nr].validDate.year > 0) && (anvHudfarg[nr].validDate.month > 0))
                return anvHudfarg[nr].validDate.year.ToString() + "-0" + anvHudfarg[nr].validDate.month.ToString();
            else if ((anvHudfarg[nr].validDate.year > 0))
                return anvHudfarg[nr].validDate.year.ToString();
            else
                return "";
        }
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
            if (!(checkEnumeration(hf, noOfComplexions, currEnum.Etnicity)))
            {
                anvHudfarg[noOfComplexions].etnicType = EtnicityType.UndefEtnicity;
                return false;
            }
            return true;
        }
        public bool setUserSkinTone(int nr, string tag, int rval, int gval, int bval, string dat)
        {
            bool retVal = false;
            if (nr < maxNoOfComplexions)
            {
                if (checkEnumeration(tag, nr, currEnum.Etnicity))
                {
                    anvHudfarg[nr].redChannelValue = rval;
                    anvHudfarg[nr].greenChannelValue = gval;
                    anvHudfarg[nr].blueChannelValue = bval;
                    retVal = handleDate(dat, nr, currEnum.Etnicity);
                }
            }
            return retVal;
        }
        public bool removeUserSkinTone(int nr)
        {
            bool retVal = false;
            if ((nr >= 0) && (nr < noOfComplexions))
            {
                for (int i = nr; i < noOfComplexions; i++)
                {
                    anvHudfarg[i].blueChannelValue = anvHudfarg[i + 1].blueChannelValue;
                    anvHudfarg[i].etnicType = anvHudfarg[i + 1].etnicType;
                    anvHudfarg[i].greenChannelValue = anvHudfarg[i + 1].greenChannelValue;
                    anvHudfarg[i].redChannelValue = anvHudfarg[i + 1].redChannelValue;
                    anvHudfarg[i].validDate = anvHudfarg[i + 1].validDate;
                }
                retVal = true;
                noOfComplexions--;
            }
            return retVal;
        }
        #endregion
        #region EyeColor
        // --- Eye Color Data ---
        public int getNumberOfEyeData() { return noOfEyeColorData; }
        public string getUserEyeColorTag(int nr) { return anvEyes[Math.Max(nr, 0)].colorTag; }
        public string getUserEyeFormTag(int nr) { return anvEyes[Math.Max(nr, 0)].formTag; }
        public string getUserEyeDataValidDate(int nr)
        {
            if ((anvEyes[nr].validDate.year > 0) && (anvEyes[nr].validDate.month > 9) && (anvEyes[nr].validDate.day > 9))
                return anvEyes[nr].validDate.year.ToString() + "-" + anvEyes[nr].validDate.month.ToString() + "-" + anvEyes[nr].validDate.day.ToString();
            else if ((anvEyes[nr].validDate.year > 0) && (anvEyes[nr].validDate.month > 9) && (anvEyes[nr].validDate.day > 0))
                return anvEyes[nr].validDate.year.ToString() + "-" + anvEyes[nr].validDate.month.ToString() + "-0" + anvEyes[nr].validDate.day.ToString();
            else if ((anvEyes[nr].validDate.year > 0) && (anvEyes[nr].validDate.month > 0) && (anvEyes[nr].validDate.day > 9))
                return anvEyes[nr].validDate.year.ToString() + "-0" + anvEyes[nr].validDate.month.ToString() + "-" + anvEyes[nr].validDate.day.ToString();
            else if ((anvEyes[nr].validDate.year > 0) && (anvEyes[nr].validDate.month > 0) && (anvEyes[nr].validDate.day > 0))
                return anvEyes[nr].validDate.year.ToString() + "-0" + anvEyes[nr].validDate.month.ToString() + "-0" + anvEyes[nr].validDate.day.ToString();
            else if ((anvEyes[nr].validDate.year > 0) && (anvEyes[nr].validDate.month > 9))
                return anvEyes[nr].validDate.year.ToString() + "-" + anvEyes[nr].validDate.month.ToString();
            else if ((anvEyes[nr].validDate.year > 0) && (anvEyes[nr].validDate.month > 0))
                return anvEyes[nr].validDate.year.ToString() + "-0" + anvEyes[nr].validDate.month.ToString();
            else if (anvEyes[nr].validDate.year > 0)
                return anvEyes[nr].validDate.year.ToString();
            else
                return "";
        }
        public bool getUserEyeGlasses(int nr) { return anvEyes[nr].glasses; }
        public bool setUserEyeData(int nr, string clr, string frm, string dte)
        {
            bool retVal = false;
            if (nr < maxNoOfEyeColorData)
            {
                anvEyes[nr].colorTag = clr;
                anvEyes[nr].formTag = frm;
                retVal = handleDate(dte, nr, currEnum.Eyes);
            }
            return retVal;
        }
        public bool setUserEyeData(int nr, string clr, string frm, string dte, bool glasses)
        {
            bool retVal = false;
            if (nr < maxNoOfEyeColorData)
            {
                anvEyes[nr].colorTag = clr;
                anvEyes[nr].formTag = frm;
                retVal = handleDate(dte, nr, currEnum.Eyes);
                anvEyes[nr].glasses = glasses;
            }
            return retVal;
        }
        public bool addUserEyeData(string clr, string frm, string dte)
        {
            bool retVal = false;
            if (noOfEyeColorData < maxNoOfEyeColorData)
            {
                anvEyes[noOfEyeColorData].colorTag = clr;
                anvEyes[noOfEyeColorData].formTag = frm;
                retVal = handleDate(dte, noOfEyeColorData, currEnum.Eyes);
                noOfEyeColorData++;
            }
            return retVal;
        }
        public bool addUserEyeData(string clr, string frm, string dte, bool glasses)
        {
            bool retVal = false;
            if (noOfEyeColorData < maxNoOfEyeColorData)
            {
                anvEyes[noOfEyeColorData].colorTag = clr;
                anvEyes[noOfEyeColorData].formTag = frm;
                retVal = handleDate(dte, noOfEyeColorData, currEnum.Eyes);
                anvEyes[noOfEyeColorData].glasses = glasses;
                noOfEyeColorData++;
            }
            return retVal;
        }
        public bool removeUserEyeData(int nr)
        {
            bool retVal = false;
            if ((nr >= 0) && (nr < noOfEyeColorData))
            {
                for (int i = nr; i < noOfEyeColorData; i++)
                {
                    anvEyes[i].colorTag = anvEyes[i + 1].colorTag;
                    anvEyes[i].formTag = anvEyes[i + 1].formTag;
                    anvEyes[i].glasses = anvEyes[i + 1].glasses;
                    anvEyes[i].validDate = anvEyes[i + 1].validDate;
                }
                retVal = true;
                noOfEyeColorData--;
            }
            return retVal;
        }
        #endregion
        #region GenderData
        // --- Gender Data ---
        public int getNumberOfGenderData() { return noOfGenderInfo; }
        public string getUserGenderType(int nr) { return anvKonsInfo[nr].type.ToString(); }
        public string getUserGenderLength(int nr) { return anvKonsInfo[nr].length.ToString() + " " + anvKonsInfo[nr].usedUnits.ToString(); }
        public string getUserGenderLengthValue(int nr) { return anvKonsInfo[nr].length.ToString(); }
        public string getUserGenderCircumf(int nr) { return anvKonsInfo[nr].circumference.ToString() + " " + anvKonsInfo[nr].usedUnits.ToString(); }
        public string getUserGenderCircumfValue(int nr) { return anvKonsInfo[nr].circumference.ToString(); }
        public string getUserGenderUnit(int nr) { return anvKonsInfo[nr].usedUnits.ToString(); }
        public string getUserGenderAppearance(int nr) { return anvKonsInfo[nr].appearance.ToString(); }
        public string getUserGenderBehaviour(int nr) { return anvKonsInfo[nr].behaviour.ToString(); }
        public string getUserGenderPres(int nr)
        {
            if (anvKonsInfo[nr].presentation != null)
                return anvKonsInfo[nr].presentation;
            else
                return "";
        }
        public string getUserGenderInfoValidDate(int nr) 
        {
            if ((anvKonsInfo[nr].validDate.month > 9) && (anvKonsInfo[nr].validDate.day > 9))
                return anvKonsInfo[nr].validDate.year.ToString() + "-" + anvKonsInfo[nr].validDate.month.ToString() + "-" + anvKonsInfo[nr].validDate.day.ToString();
            else if ((anvKonsInfo[nr].validDate.month > 9) && (anvKonsInfo[nr].validDate.day > 0))
                return anvKonsInfo[nr].validDate.year.ToString() + "-" + anvKonsInfo[nr].validDate.month.ToString() + "-0" + anvKonsInfo[nr].validDate.day.ToString();
            else if ((anvKonsInfo[nr].validDate.month > 0) && (anvKonsInfo[nr].validDate.day > 9))
                return anvKonsInfo[nr].validDate.year.ToString() + "-0" + anvKonsInfo[nr].validDate.month.ToString() + "-" + anvKonsInfo[nr].validDate.day.ToString();
            else if ((anvKonsInfo[nr].validDate.month > 0) && (anvKonsInfo[nr].validDate.day > 0))
                return anvKonsInfo[nr].validDate.year.ToString() + "-0" + anvKonsInfo[nr].validDate.month.ToString() + "-0" + anvKonsInfo[nr].validDate.day.ToString();
            else if (anvKonsInfo[nr].validDate.month > 9)
                return anvKonsInfo[nr].validDate.year.ToString() + "-" + anvKonsInfo[nr].validDate.month.ToString();
            else if (anvKonsInfo[nr].validDate.month > 0)
                return anvKonsInfo[nr].validDate.year.ToString() + "-0" + anvKonsInfo[nr].validDate.month.ToString();
            else
                return anvKonsInfo[nr].validDate.year.ToString();
        }
        public bool setUserGenderData(int nr, string gentyp, int lnt, int circ, string unit, string aprnc, string behv, string gprs, string dat)
        {
            bool retVal = false;
            if (nr < maxNoOfGenderInfo)
            {
                if(checkEnumeration(gentyp, nr, currEnum.Gender))
                {
                    if(checkEnumeration(unit, nr, currEnum.Gender))
                    {
                        if(checkEnumeration(aprnc, nr, currEnum.GenApp))
                        {
                            if(checkEnumeration(behv, nr, currEnum.GenBeh))
                            {
                                anvKonsInfo[nr].length = lnt;
                                anvKonsInfo[nr].circumference = circ;
                                anvKonsInfo[nr].presentation = gprs;
                                retVal = handleDate(dat, nr, currEnum.Gender);
                            }
                        }
                    }
                }
            }
            return retVal;
        }
        public bool addUserGenderData(string gentyp, float lnt, float circ, string unit, string aprnc, string behv, string gprs, string dat)
        {
            bool retVal = false;
            if (noOfGenderInfo < maxNoOfGenderInfo)
            {
                if (checkEnumeration(gentyp, noOfGenderInfo, currEnum.Gender))
                {
                    if (checkEnumeration(unit, noOfGenderInfo, currEnum.Gender))
                    {
                        if (checkEnumeration(aprnc, noOfGenderInfo, currEnum.GenApp))
                        {
                            if (checkEnumeration(behv, noOfGenderInfo, currEnum.GenBeh))
                            {
                                anvKonsInfo[noOfGenderInfo].length = lnt;
                                anvKonsInfo[noOfGenderInfo].circumference = circ;
                                anvKonsInfo[noOfGenderInfo].presentation = gprs;
                                retVal = handleDate(dat, noOfGenderInfo, currEnum.Gender);
                                noOfGenderInfo++;
                            }
                        }
                    }
                }
            }
            return retVal;
        }
        public bool removeUserGenderData(int nr)
        {
            bool retVal = false;
            if ((nr >= 0) && (nr < noOfGenderInfo))
            {
                for (int i = nr; i < noOfGenderInfo; i++)
                {
                    anvKonsInfo[i].appearance = anvKonsInfo[i + 1].appearance;
                    anvKonsInfo[i].behaviour = anvKonsInfo[i + 1].behaviour;
                    anvKonsInfo[i].circumference = anvKonsInfo[i + 1].circumference;
                    anvKonsInfo[i].length = anvKonsInfo[i + 1].length;
                    anvKonsInfo[i].presentation = anvKonsInfo[i + 1].presentation;
                    anvKonsInfo[i].type = anvKonsInfo[i + 1].type;
                    anvKonsInfo[i].usedUnits = anvKonsInfo[i + 1].usedUnits;
                    anvKonsInfo[i].validDate = anvKonsInfo[i + 1].validDate;
                }
                retVal = true;
                noOfGenderInfo--;
            }
            return retVal;
        }
        #endregion
        #region LengthData
        // --- Length Data ---
        public int getNumberOfLengthData() { return noOfLengthData; }
        public string getUserLengthValue(int nr) { return anvLangdData[nr].value.ToString() + " " + anvLangdData[nr].unit.ToString(); }
        public string getUserLengthVal(int nr) { return anvLangdData[nr].value.ToString(); }
        public string getUserLengthUnit(int nr) { return anvLangdData[nr].unit.ToString(); }
        public string getUserLengthInfoValidDate(int nr) 
        {
            if ((anvLangdData[nr].validDate.month > 9) && (anvLangdData[nr].validDate.day > 9))
                return anvLangdData[nr].validDate.year.ToString() + "-" + anvLangdData[nr].validDate.month.ToString() + "-" + anvLangdData[nr].validDate.day.ToString();
            else if ((anvLangdData[nr].validDate.month > 9) && (anvLangdData[nr].validDate.day > 0))
                return anvLangdData[nr].validDate.year.ToString() + "-" + anvLangdData[nr].validDate.month.ToString() + "-0" + anvLangdData[nr].validDate.day.ToString();
            else if ((anvLangdData[nr].validDate.month > 0) && (anvLangdData[nr].validDate.day > 9))
                return anvLangdData[nr].validDate.year.ToString() + "-0" + anvLangdData[nr].validDate.month.ToString() + "-" + anvLangdData[nr].validDate.day.ToString();
            else if ((anvLangdData[nr].validDate.month > 0) && (anvLangdData[nr].validDate.day > 0))
                return anvLangdData[nr].validDate.year.ToString() + "-0" + anvLangdData[nr].validDate.month.ToString() + "-0" + anvLangdData[nr].validDate.day.ToString();
            else if (anvLangdData[nr].validDate.month > 9)
                return anvLangdData[nr].validDate.year.ToString() + "-" + anvLangdData[nr].validDate.month.ToString();
            else if (anvLangdData[nr].validDate.month > 0)
                return anvLangdData[nr].validDate.year.ToString() + "-0" + anvLangdData[nr].validDate.month.ToString();
            else
                return anvLangdData[nr].validDate.year.ToString();
        }
        public bool setUserLength(int nr, float value, string unit, string dat)
        {
            bool retVal = false;
            if (nr < maxNoOfLengthData)
            {
                if (checkEnumeration(unit, nr, currEnum.Length))
                {
                    anvLangdData[nr].value = value;
                    retVal = handleDate(dat, nr, currEnum.Length);
                }
            }
            return retVal;
        }
        public bool addUserLength(float value, string unit, string dat)
        {
            bool retVal = false;
            if (noOfLengthData < maxNoOfLengthData)
            {
                if (checkEnumeration(unit, noOfLengthData, currEnum.Length))
                {
                    anvLangdData[noOfLengthData].value = value;
                    retVal = handleDate(dat, noOfLengthData, currEnum.Length);
                    noOfLengthData++;
                }
            }
            return retVal;
        }
        public bool removeUserLength(int nr)
        {
            bool retVal = false;
            if ((nr < maxNoOfLengthData) && (nr < noOfLengthData - 1))
            {
                for (int i = nr; i < maxNoOfLengthData; i++)
                {
                    anvLangdData[i].unit = anvLangdData[i + 1].unit;
                    anvLangdData[i].validDate = anvLangdData[i + 1].validDate;
                    anvLangdData[i].value = anvLangdData[i + 1].value;
                }
                retVal = true;
                noOfLengthData--;
            }
            return retVal;
        }
        #endregion
        #region WeightData
        // --- Weight Data ---
        public int getNumberOfWeightData() { return noOfWeightData; }
        public string getUserWeightValue(int nr) { return anvViktData[nr].value.ToString() + " " + anvViktData[nr].unit.ToString(); }
        public string getUserWeightVal(int nr) { return anvViktData[nr].value.ToString(); }
        public string getUserWeightUnit(int nr) { return anvViktData[nr].unit.ToString(); }
        public string getUserWeightInfoValidDate(int nr) 
        {
            if ((anvViktData[nr].validDate.month > 9) && (anvViktData[nr].validDate.day > 9))
                return anvViktData[nr].validDate.year.ToString() + "-" + anvViktData[nr].validDate.month.ToString() + "-" + anvViktData[nr].validDate.day.ToString();
            else if ((anvViktData[nr].validDate.month > 9) && (anvViktData[nr].validDate.day > 0))
                return anvViktData[nr].validDate.year.ToString() + "-" + anvViktData[nr].validDate.month.ToString() + "-0" + anvViktData[nr].validDate.day.ToString();
            else if ((anvViktData[nr].validDate.month > 0) && (anvViktData[nr].validDate.day > 9))
                return anvViktData[nr].validDate.year.ToString() + "-0" + anvViktData[nr].validDate.month.ToString() + "-" + anvViktData[nr].validDate.day.ToString();
            else if ((anvViktData[nr].validDate.month > 0) && (anvViktData[nr].validDate.day > 0))
                return anvViktData[nr].validDate.year.ToString() + "-0" + anvViktData[nr].validDate.month.ToString() + "-0" + anvViktData[nr].validDate.day.ToString();
            else if (anvViktData[nr].validDate.month > 9)
                return anvViktData[nr].validDate.year.ToString() + "-" + anvViktData[nr].validDate.month.ToString();
            else if (anvViktData[nr].validDate.month > 0)
                return anvViktData[nr].validDate.year.ToString() + "-0" + anvViktData[nr].validDate.month.ToString();
            else
                return anvViktData[nr].validDate.year.ToString();
        }
        public bool setUserWeightData(int nr, float value, string unit, string date)
        {
            bool retVal = false;
            if (nr < maxNoOfWeightData)
            {
                if (checkEnumeration(unit, nr, currEnum.Weight))
                {
                    anvViktData[nr].value = value;
                    retVal = handleDate(date, nr, currEnum.Weight);
                }
            }
            return retVal;
        }
        public bool addUserWeightData(float value, string unit, string date)
        {
            bool retVal = false;
            if (noOfWeightData < maxNoOfWeightData)
            {
                if (checkEnumeration(unit, noOfWeightData, currEnum.Weight))
                {
                    anvViktData[noOfWeightData].value = value;
                    retVal = handleDate(date, noOfWeightData, currEnum.Weight);
                    noOfWeightData++;
                }
            }
            return retVal;
        }
        public bool removeUserWeightData(int nr)
        {
            bool retVal = false;
            if ((nr >= 0) && (nr < noOfWeightData) && (nr < maxNoOfWeightData))
            {
                for (int i = nr; i < maxNoOfWeightData; i++)
                {
                    anvViktData[i].value = anvViktData[i + 1].value;
                    anvViktData[i].unit = anvViktData[i + 1].unit;
                    anvViktData[i].validDate = anvViktData[i + 1].validDate;
                }
                retVal = true;
                noOfWeightData--;
            }
            return retVal;
        }
        #endregion
        #region ChestData
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
            else if ((anvBrostData[nr].validDate.month > 9) && (anvBrostData[nr].validDate.day > 0))
                return anvBrostData[nr].validDate.year.ToString() + "-" + anvBrostData[nr].validDate.month.ToString() + "-0" + anvBrostData[nr].validDate.day.ToString();
            else if ((anvBrostData[nr].validDate.month > 0) && (anvBrostData[nr].validDate.day > 9))
                return anvBrostData[nr].validDate.year.ToString() + "-0" + anvBrostData[nr].validDate.month.ToString() + "-" + anvBrostData[nr].validDate.day.ToString();
            else if ((anvBrostData[nr].validDate.month > 0) && (anvBrostData[nr].validDate.day > 0))
                return anvBrostData[nr].validDate.year.ToString() + "-0" + anvBrostData[nr].validDate.month.ToString() + "-0" + anvBrostData[nr].validDate.day.ToString();
            else if (anvBrostData[nr].validDate.month > 9)
                return anvBrostData[nr].validDate.year.ToString() + "-" + anvBrostData[nr].validDate.month.ToString();
            else if (anvBrostData[nr].validDate.month > 0)
                return anvBrostData[nr].validDate.year.ToString() + "-0" + anvBrostData[nr].validDate.month.ToString();
            else
                return anvBrostData[nr].validDate.year.ToString();
        }
        public bool setUserChestData(int nr, string chtp, float circ, string chsztp, string dat)
        {
            bool retVal = false;
            if (nr < maxNoOfChestData)
            {
                if (checkEnumeration(chtp, nr, currEnum.BrstTp))
                {
                    if (checkEnumeration(chsztp, nr, currEnum.BrstSz))
                    {
                        anvBrostData[nr].circumference = circ;
                        retVal = handleDate(dat, nr, currEnum.BrstTp);
                    }
                }
            }
            return retVal;
        }
        public bool addUserChestData(string chtp, float circ, string unit, string chsztp, string dat)
        {
            bool retVal = false;
            if (noOfChestData < maxNoOfChestData)
            {
                if (checkEnumeration(chtp, noOfChestData, currEnum.BrstTp))
                {
                    if (checkEnumeration(chsztp, noOfChestData, currEnum.BrstSz))
                    {
                        if (checkEnumeration(unit, noOfChestData, currEnum.BrstSz))
                        {
                            anvBrostData[noOfChestData].circumference = circ;
                            retVal = handleDate(dat, noOfChestData, currEnum.BrstTp);
                        }
                    }
                }
                noOfChestData++;
            }
            return retVal;
        }
        public bool removeUserChestData(int nr)
        {
            bool retVal = false;
            if ((nr >= 0) && (nr < noOfChestData))
            {
                for (int i = nr; i < noOfChestData; i++)
                {
                    anvBrostData[i].type = anvBrostData[i + 1].type;
                    anvBrostData[i].circumference = anvBrostData[i + 1].circumference;
                    anvBrostData[i].units = anvBrostData[i + 1].units;
                    anvBrostData[i].sizeType = anvBrostData[i + 1].sizeType;
                    anvBrostData[i].validDate = anvBrostData[i + 1].validDate;
                }
                retVal = false;
                noOfChestData--;
            }
            return retVal;
        }
        #endregion
        #region HairData
        // --- Hair Data ---
        public int getNumberOfHairData() { return noOfHairData; }
        public string getUserHairColor(int nr) { return anvHaar[nr].hairColor.tag; }
        public string getUserHairTexture(int nr) { return anvHaar[nr].textureTag.ToString(); }
        public string getUserHairLength(int nr) { return anvHaar[nr].lengthTag.ToString(); }
        public string getUserHairValidDate(int nr) 
        {
            if ((anvHaar[nr].validDate.month > 9) && (anvHaar[nr].validDate.day > 9))
                return anvHaar[nr].validDate.year.ToString() + "-" + anvHaar[nr].validDate.month.ToString() + "-" + anvHaar[nr].validDate.day.ToString();
            else if ((anvHaar[nr].validDate.month > 9) && (anvHaar[nr].validDate.month > 0))
                return anvHaar[nr].validDate.year.ToString() + "-" + anvHaar[nr].validDate.month.ToString() + "-0" + anvHaar[nr].validDate.day.ToString();
            else if ((anvHaar[nr].validDate.month > 0) && (anvHaar[nr].validDate.day > 9))
                return anvHaar[nr].validDate.year.ToString() + "-0" + anvHaar[nr].validDate.month.ToString() + "-" + anvHaar[nr].validDate.day.ToString();
            else if ((anvHaar[nr].validDate.month > 0) && (anvHaar[nr].validDate.day > 0))
                return anvHaar[nr].validDate.year.ToString() + "-0" + anvHaar[nr].validDate.month.ToString() + "-0" + anvHaar[nr].validDate.day.ToString();
            else if (anvHaar[nr].validDate.month > 9)
                return anvHaar[nr].validDate.year.ToString() + "-" + anvHaar[nr].validDate.month.ToString();
            else if (anvHaar[nr].validDate.month > 0)
                return anvHaar[nr].validDate.year.ToString() + "-0" + anvHaar[nr].validDate.month.ToString();
            else
                return anvHaar[nr].validDate.year.ToString();
        }
        public bool setUserHairData(int nr, string clrtg, string txtrtg, string lnttg, string date)
        {
            bool retVal = false;
            if (nr < maxNoOfHairData)
            {
                if (checkEnumeration(txtrtg, nr, currEnum.HairText))
                {
                    if (checkEnumeration(lnttg, nr, currEnum.HairLnt))
                    {
                        bool foundcategory = false;
                        for (int i = 0; i < noOfHairColorCategories; i++)
                        {
                            if (clrtg == hairColorCategory[i].tag)
                            {
                                foundcategory = true;
                                anvHaar[nr].hairColor = hairColorCategory[i];
                                retVal = true;
                            }
                        }
                        if ((!(foundcategory)) && (noOfHairColorCategories < maxNoOfHairColorCategories))
                        {
                            anvHaar[nr].hairColor.tag = clrtg;
                            anvHaar[nr].hairColor.description = "";
                            anvHaar[nr].hairColor.level = 0;
                            addHairColorCategory(clrtg, "", "Undefined");
                            retVal = true;
                        }
                        if (retVal)
                            retVal = handleDate(date, nr, currEnum.HairLnt);
                    }
                }
            }
            return retVal;
        }
        public bool addUserHairData(string clrtg, string txtrtg, string lnttg, string date)
        {
            bool retVal = false;
            if (noOfHairData < maxNoOfHairData)
            {
                if (checkEnumeration(txtrtg, noOfHairData, currEnum.HairText))
                {
                    if (checkEnumeration(lnttg, noOfHairData, currEnum.HairLnt))
                    {
                        bool foundcategory = false;
                        for (int i = 0; i < noOfHairColorCategories; i++)
                        {
                            if (clrtg == hairColorCategory[i].tag)
                            {
                                foundcategory = true;
                                anvHaar[noOfHairData].hairColor = hairColorCategory[i];
                                retVal = true;
                            }
                        }
                        if ((!(foundcategory)) && (noOfHairColorCategories < maxNoOfHairColorCategories))
                        {
                            anvHaar[noOfHairData].hairColor.tag = clrtg;
                            anvHaar[noOfHairData].hairColor.description = "";
                            anvHaar[noOfHairData].hairColor.level = 0;
                            addHairColorCategory(clrtg, "", "Undefined");
                            retVal = true;
                        }
                        if (retVal)
                            retVal = handleDate(date, noOfHairData, currEnum.HairLnt);
                        noOfHairData++;
                    }
                }
            }
            return retVal;
        }
        public bool removeUserHairData(int nr)
        {
            bool retVal = false;
            if ((nr >= 0) && (nr < noOfHairData))
            {
                for (int i = nr; i < noOfHairData; i++)
                {
                    anvHaar[i].hairColor.tag = anvHaar[i + 1].hairColor.tag;
                    anvHaar[i].hairColor.level = anvHaar[i + 1].hairColor.level;
                    anvHaar[i].hairColor.description = anvHaar[i + 1].hairColor.description;
                    anvHaar[i].hairColor.value = anvHaar[i + 1].hairColor.value;
                }
                retVal = true;
                noOfHairData--;
            }
            return retVal;
        }
        #endregion
        #region MarkingsData
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
        public string getActorMarkingMotif(int nr)
        {
            if ((nr >= 0) && (nr < noOfMarkingData))
                return anvMarkningar[nr].motif;
            else
                return "";
        }
        public string getActorMarkingValidDate(int nr) 
        {
            if ((anvMarkningar[nr].validDate.month > 9) && (anvMarkningar[nr].validDate.day > 9))
                return anvMarkningar[nr].validDate.year.ToString() + "-" + anvMarkningar[nr].validDate.month.ToString() + "-" + anvMarkningar[nr].validDate.day.ToString();
            else if ((anvMarkningar[nr].validDate.month > 9) && (anvMarkningar[nr].validDate.day > 0))
                return anvMarkningar[nr].validDate.year.ToString() + "-" + anvMarkningar[nr].validDate.month.ToString() + "-0" + anvMarkningar[nr].validDate.day.ToString();
            else if ((anvMarkningar[nr].validDate.month > 0) && (anvMarkningar[nr].validDate.day > 9))
                return anvMarkningar[nr].validDate.year.ToString() + "-0" + anvMarkningar[nr].validDate.month.ToString() + "-" + anvMarkningar[nr].validDate.day.ToString();
            else if ((anvMarkningar[nr].validDate.month > 0) && (anvMarkningar[nr].validDate.day > 0))
                return anvMarkningar[nr].validDate.year.ToString() + "-0" + anvMarkningar[nr].validDate.month.ToString() + "-0" + anvMarkningar[nr].validDate.day.ToString();
            else if (anvMarkningar[nr].validDate.month > 9)
                return anvMarkningar[nr].validDate.year.ToString() + "-" + anvMarkningar[nr].validDate.month.ToString();
            else if (anvMarkningar[nr].validDate.month > 0)
                return anvMarkningar[nr].validDate.year.ToString() + "-0" + anvMarkningar[nr].validDate.month.ToString();
            else
                return anvMarkningar[nr].validDate.year.ToString();
        }
        public bool setActorMarkingData(int nr, string typeTag, string placement, string motif, string date)
        {
            bool retVal = false;
            if (nr < maxNoOfMarkingData)
            {
                if (checkEnumeration(typeTag, nr, currEnum.MrkTp))
                {
                    anvMarkningar[nr].placement = placement;
                    anvMarkningar[nr].motif = motif;
                    retVal = handleDate(date, nr, currEnum.MrkTp);
                }
            }
            return retVal;
        }
        public bool addActorMarkingData(string typeTag, string placement, string motif, string date)
        {
            bool retVal = false;
            if (noOfMarkingData < maxNoOfMarkingData)
            {
                if (checkEnumeration(typeTag, noOfMarkingData, currEnum.MrkTp))
                {
                    anvMarkningar[noOfMarkingData].placement = placement;
                    anvMarkningar[noOfMarkingData].motif = motif;
                    retVal = handleDate(date, noOfMarkingData, currEnum.MrkTp);
                    noOfMarkingData++;
                }
            }
            return retVal;
        }
        public bool removeActorMarkingData(int nr)
        {
            bool retVal = false;
            if ((nr >= 0) && (nr < noOfMarkingData))
            {
                for (int i = nr; i < noOfMarkingData; i++)
                {
                    anvMarkningar[i].markTag = anvMarkningar[i + 1].markTag;
                    anvMarkningar[i].placement = anvMarkningar[i + 1].placement;
                    anvMarkningar[i].motif = anvMarkningar[i + 1].motif;
                    anvMarkningar[i].validDate = anvMarkningar[i + 1].validDate;
                }
                noOfMarkingData--;
                retVal = true;
            }
            return retVal;
        }
        #endregion
        #region OccupationData
        // --- Occupation Data ---
        public int getNoOfOccupationsData() { return noOfOccupationData; }
        public string getActorOccupationTitle(int nr)
        {
            if ((nr >= 0) && (nr < noOfOccupationData))
                return anvAnstallningar[nr].title;
            else
                return "";
        }
        public string getActorOccupationCompany(int nr)
        {
            if ((nr >= 0) && (nr < noOfOccupationData))
                return anvAnstallningar[nr].company;
            else
                return "";
        }
        public string getActorOccupationStreetname(int nr) 
        {
            if ((nr >= 0) && (nr < noOfOccupationData))
                return anvAnstallningar[nr].streetname + " " + anvAnstallningar[nr].number.ToString();
            else
                return "";
        }
        public string getActorOccupationStrtNme(int nr) 
        {
            if ((nr >= 0) && (nr < noOfOccupationData))
                return anvAnstallningar[nr].streetname;
            else
                return "";
        }
        public string getActorOccupationStrtNum(int nr) 
        {
            if ((nr >= 0) && (nr < noOfOccupationData))
                return anvAnstallningar[nr].number.ToString();
            else
                return "";
        }
        public string getActorOccupationStatename(int nr) 
        {
            if ((nr >= 0) && (nr < noOfOccupationData))
                return anvAnstallningar[nr].statename;
            else
                return "";
        }
        public string getActorOccupationAreaname(int nr) 
        {
            if ((nr >= 0) && (nr < noOfOccupationData))
                return anvAnstallningar[nr].areaname;
            else
                return "";
        }
        public string getActorOccupationZipcode(int nr) 
        {
            if ((nr >= 0) && (nr < noOfOccupationData))
                return anvAnstallningar[nr].zipcode.ToString();
            else
                return "";
        }
        public string getActorOccupationCountry(int nr) 
        {
            if ((nr >= 0) && (nr < noOfOccupationData))
                return anvAnstallningar[nr].country;
            else
                return "";
        }
        public string getActorOccupationCity(int nr) 
        {
            if ((nr >= 0) && (nr < noOfOccupationData))
                return anvAnstallningar[nr].cityname;
            else
                return "";
        }
        public string getActorOccupationStarted(int nr)
        {
            if (anvAnstallningar[nr].started.year > 0)
            {
                if ((anvAnstallningar[nr].started.month > 9) && (anvAnstallningar[nr].started.day > 9))
                    return anvAnstallningar[nr].started.year.ToString() + "-" + anvAnstallningar[nr].started.month.ToString() + "-" + anvAnstallningar[nr].started.day.ToString();
                else if ((anvAnstallningar[nr].started.month > 9) && (anvAnstallningar[nr].started.day > 0))
                    return anvAnstallningar[nr].started.year.ToString() + "-" + anvAnstallningar[nr].started.month.ToString() + "-0" + anvAnstallningar[nr].started.day.ToString();
                else if ((anvAnstallningar[nr].started.month > 0) && (anvAnstallningar[nr].started.day > 9))
                    return anvAnstallningar[nr].started.year.ToString() + "-0" + anvAnstallningar[nr].started.month.ToString() + "-" + anvAnstallningar[nr].started.day.ToString();
                else if ((anvAnstallningar[nr].started.day > 0) && (anvAnstallningar[nr].started.month > 0))
                    return anvAnstallningar[nr].started.year.ToString() + "-0" + anvAnstallningar[nr].started.month.ToString() + "-0" + anvAnstallningar[nr].started.day.ToString();
                else if (anvAnstallningar[nr].started.month > 9)
                    return anvAnstallningar[nr].started.year.ToString() + "-" + anvAnstallningar[nr].started.month.ToString();
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
                else if ((anvAnstallningar[nr].ended.month > 9) && (anvAnstallningar[nr].ended.day > 0))
                    return anvAnstallningar[nr].ended.year.ToString() + "-" + anvAnstallningar[nr].ended.month.ToString() + "-0" + anvAnstallningar[nr].ended.day.ToString();
                else if ((anvAnstallningar[nr].ended.month > 0) && (anvAnstallningar[nr].ended.day > 9))
                    return anvAnstallningar[nr].ended.year.ToString() + "-0" + anvAnstallningar[nr].ended.month.ToString() + "-" + anvAnstallningar[nr].ended.day.ToString();
                else if ((anvAnstallningar[nr].ended.month > 0) && (anvAnstallningar[nr].ended.month > 0))
                    return anvAnstallningar[nr].ended.year.ToString() + "-0" + anvAnstallningar[nr].ended.month.ToString() + "-0" + anvAnstallningar[nr].ended.day.ToString();
                else if (anvAnstallningar[nr].ended.month > 9)
                    return anvAnstallningar[nr].ended.year.ToString() + "-" + anvAnstallningar[nr].ended.month.ToString();
                else if (anvAnstallningar[nr].ended.month > 0)
                    return anvAnstallningar[nr].ended.year.ToString() + "-0" + anvAnstallningar[nr].ended.month.ToString();
                else
                    return anvAnstallningar[nr].ended.year.ToString();

            }
            else
                return "";
        }
        public bool addActorOccupationData(string title, string company, string street, string city, string state, string area, string zipcode, string country, string start, string end)
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
                anvAnstallningar[noOfOccupationData].cityname = city;
                anvAnstallningar[noOfOccupationData].statename = state;
                anvAnstallningar[noOfOccupationData].zipcode = int.Parse(zipcode);
                anvAnstallningar[noOfOccupationData].country = country;
                anvAnstallningar[noOfOccupationData].started.year = 0;
                handleDate(start, noOfOccupationData, currEnum.Occupation);
                anvAnstallningar[noOfOccupationData].ended.year = 0;
                handleDate(end, noOfOccupationData, currEnum.Occupation);
                noOfOccupationData++;
                retVal = true;
            }
            return retVal;
        }
        public bool settingActorOccupationData(int nr, string title, string company, string street, string city, string state, string area, string zipcode, string country, string start, string end)
        {
            bool retVal = false;
            if (nr < maxNoOfOccupationData)
            {
                anvAnstallningar[nr].title = title;
                anvAnstallningar[nr].company = company;
                int dp0 = street.IndexOf(" ");
                if ((dp0 > 0) && (dp0 < street.Length))
                {
                    string strt = street.Substring(0, dp0);
                    anvAnstallningar[nr].streetname = strt;
                    string nmbr = street.Substring(dp0 + 1, street.Length - dp0 - 1);
                    anvAnstallningar[nr].number = int.Parse(nmbr);
                }
                else
                    anvAnstallningar[nr].streetname = street;
                anvAnstallningar[nr].cityname = city;
                anvAnstallningar[nr].statename = state;
                anvAnstallningar[nr].zipcode = int.Parse(zipcode);
                anvAnstallningar[nr].country = country;
                anvAnstallningar[nr].started.year = 0;
                handleDate(start, nr, currEnum.Occupation);
                anvAnstallningar[nr].ended.year = 0;
                handleDate(end, nr, currEnum.Occupation);
                retVal = true;
            }
            return retVal;
        }
        public bool removeActorOccupationData(int nr)
        {
            bool retVal = false;
            if ((nr >= 0) && (nr < noOfOccupationData))
            {
                for (int i = nr; i < noOfOccupationData; i++)
                {
                    anvAnstallningar[i].title = anvAnstallningar[i + 1].title;
                    anvAnstallningar[i].company = anvAnstallningar[i + 1].company;
                    anvAnstallningar[i].streetname = anvAnstallningar[i + 1].streetname;
                    anvAnstallningar[i].number = anvAnstallningar[i + 1].number;
                    anvAnstallningar[i].cityname = anvAnstallningar[i + 1].cityname;
                    anvAnstallningar[i].statename = anvAnstallningar[i + 1].statename;
                    anvAnstallningar[i].areaname = anvAnstallningar[i + 1].areaname;
                    anvAnstallningar[i].country = anvAnstallningar[i + 1].country;
                    anvAnstallningar[i].ended = anvAnstallningar[i + 1].ended;
                    anvAnstallningar[i].started = anvAnstallningar[i + 1].started;
                    anvAnstallningar[i].zipcode = anvAnstallningar[i + 1].zipcode;
                }
                retVal = true;
                noOfOccupationData--;
            }
            return retVal;
        }
        #endregion
        #region ResicenceData
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
                else if ((anvBostader[nr].boughtDate.month > 9) && (anvBostader[nr].boughtDate.day > 0))
                    return anvBostader[nr].boughtDate.year.ToString() + "-" + anvBostader[nr].boughtDate.month.ToString() + "-0" + anvBostader[nr].boughtDate.day.ToString();
                else if ((anvBostader[nr].boughtDate.month > 0) && (anvBostader[nr].boughtDate.day > 9))
                    return anvBostader[nr].boughtDate.year.ToString() + "-0" + anvBostader[nr].boughtDate.month.ToString() + "-" + anvBostader[nr].boughtDate.day.ToString();
                else if ((anvBostader[nr].boughtDate.month > 0) && (anvBostader[nr].boughtDate.day > 0))
                    return anvBostader[nr].boughtDate.year.ToString() + "-0" + anvBostader[nr].boughtDate.month.ToString() + "-0" + anvBostader[nr].boughtDate.day.ToString();
                else if (anvBostader[nr].boughtDate.month > 9)
                    return anvBostader[nr].boughtDate.year.ToString() + "-" + anvBostader[nr].boughtDate.month.ToString();
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
            else if ((anvBostader[nr].salesDate.month > 9) && (anvBostader[nr].salesDate.day > 0))
                return anvBostader[nr].salesDate.year.ToString() + "-" + anvBostader[nr].salesDate.month.ToString() + "-0" + anvBostader[nr].salesDate.day.ToString();
            else if ((anvBostader[nr].salesDate.month > 0) && (anvBostader[nr].salesDate.day > 9))
                return anvBostader[nr].salesDate.year.ToString() + "-0" + anvBostader[nr].salesDate.month.ToString() + "-" + anvBostader[nr].salesDate.day.ToString();
            else if ((anvBostader[nr].salesDate.month > 0) && (anvBostader[nr].salesDate.day > 0))
                return anvBostader[nr].salesDate.year.ToString() + "-0" + anvBostader[nr].salesDate.month.ToString() + "-0" + anvBostader[nr].salesDate.day.ToString();
            else if (anvBostader[nr].salesDate.month > 9)
                return anvBostader[nr].salesDate.year.ToString() + "-" + anvBostader[nr].salesDate.month.ToString();
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
        public bool setActorResidenceData(int nr, string streetname, int number, string addon, string cityname, string areaname, int zipCode, string country, string bought, string sold, float boughtSum, float salesSum, string currency)
        {
            bool retVal = false;
            if (nr < maxNoOfResidences)
            {
                anvBostader[nr].boughtDate.year = 0;
                if (handleDate(bought, nr, currEnum.Residence))
                {
                    anvBostader[nr].streetname = streetname;
                    anvBostader[nr].number = number;
                    anvBostader[nr].additive = addon;
                    anvBostader[nr].city = cityname;
                    anvBostader[nr].area = areaname;
                    anvBostader[nr].zipcode = zipCode;
                    anvBostader[nr].country = country;
                    // string sold, float boughtSum, float salesSum, string currency
                    anvBostader[nr].salesDate.year = 0;
                    anvBostader[nr].boughtValue = boughtSum;
                    if (handleDate(sold, nr, currEnum.Residence))
                        anvBostader[nr].salesValue = salesSum;
                    bool foundcurrency = false;
                    for (int i = 0; i < maxNoOfCurrencyCategories; i++)
                    {
                        if (currencyCategory[i].tag == currency)
                        {
                            anvBostader[nr].usedCurrency = currencyCategory[i];
                            foundcurrency = true;
                        }
                    }
                    if ((!(foundcurrency)) && (noOfCurrencyCategories < maxNoOfCurrencyCategories))
                    {
                        anvBostader[nr].usedCurrency.tag = currency;
                        anvBostader[nr].usedCurrency.description = "";
                        anvBostader[nr].usedCurrency.level = 0;
                        addCurrencyCategory(currency, "", "Undefiled", 0);
                    }
                    retVal = true;
                }
            }
            return retVal;
        }
        public bool addActorResidenceData(string streetname, int number, string addon, string cityname, string areaname, int zipCode, string country, string bought, string sold, float boughtSum, float salesSum, string currency)
        {
            bool retVal = false;
            if (noOfResidences < maxNoOfResidences)
            {
                anvBostader[noOfResidences].boughtDate.year = 0;
                if (handleDate(bought, noOfResidences, currEnum.Residence))
                {
                    anvBostader[noOfResidences].streetname = streetname;
                    anvBostader[noOfResidences].number = number;
                    anvBostader[noOfResidences].additive = addon;
                    anvBostader[noOfResidences].city = cityname;
                    anvBostader[noOfResidences].area = areaname;
                    anvBostader[noOfResidences].zipcode = zipCode;
                    anvBostader[noOfResidences].country = country;
                    // string sold, float boughtSum, float salesSum, string currency
                    anvBostader[noOfResidences].salesDate.year = 0;
                    anvBostader[noOfResidences].boughtValue = boughtSum;
                    if (handleDate(sold, noOfResidences, currEnum.Residence))
                        anvBostader[noOfResidences].salesValue = salesSum;
                    else
                        anvBostader[noOfResidences].salesValue = 0;
                    bool foundcurrency = false;
                    for (int i = 0; i < maxNoOfCurrencyCategories; i++)
                    {
                        if (currencyCategory[i].tag == currency)
                        {
                            anvBostader[noOfResidences].usedCurrency = currencyCategory[i];
                            foundcurrency = true;
                        }
                    }
                    if ((!(foundcurrency)) && (noOfCurrencyCategories < maxNoOfCurrencyCategories))
                    {
                        anvBostader[noOfResidences].usedCurrency.tag = currency;
                        anvBostader[noOfResidences].usedCurrency.description = "";
                        anvBostader[noOfResidences].usedCurrency.level = 0;
                        addCurrencyCategory(currency, "", "Undefiled", 0);
                    }
                    retVal = true;
                    noOfResidences++;
                }
            }
            return retVal;
        }
        public bool removeActorResidence(int nr)
        {
            bool retVal = false;
            if ((nr >= 0) && (nr < noOfResidences))
            {
                for (int i = nr; i < noOfResidences; i++)
                {
                    anvBostader[i].streetname = anvBostader[i + 1].streetname;
                    anvBostader[i].number = anvBostader[i + 1].number;
                    anvBostader[i].additive = anvBostader[i + 1].additive;
                    anvBostader[i].city = anvBostader[i + 1].city;
                    anvBostader[i].area = anvBostader[i + 1].area;
                    anvBostader[i].zipcode = anvBostader[i + 1].zipcode;
                    anvBostader[i].country = anvBostader[i + 1].country;
                    anvBostader[i].boughtDate = anvBostader[i + 1].boughtDate;
                    anvBostader[i].boughtValue = anvBostader[i + 1].boughtValue;
                    anvBostader[i].salesDate = anvBostader[i + 1].salesDate;
                    anvBostader[i].salesValue = anvBostader[i + 1].salesValue;
                    anvBostader[i].usedCurrency = anvBostader[i + 1].usedCurrency;
                }
                retVal = true;
                noOfResidences--;
            }
            return retVal;
        }
        #endregion
        #region AttdEventData
        // --- Attended Events Data ---
        public int getNoOfAttendedEventsData() { return noOfAttendedEventData; }
        public string getActorAttendedEventID(int nr) { return anvTillstallningar[nr].eventID; }
        public string getActorAttendedEventType(int nr) { return anvTillstallningar[nr].eventCategory; }//anvTillstallningar[nr].type.ToString(); }
        public string getActorAttendedEventStarted(int nr) 
        {
            if ((anvTillstallningar[nr].started.month > 9) && (anvTillstallningar[nr].started.day > 9))
                return anvTillstallningar[nr].started.year.ToString() + "-" + anvTillstallningar[nr].started.month.ToString() + "-" + anvTillstallningar[nr].started.day.ToString();
            else if ((anvTillstallningar[nr].started.month > 0) && (anvTillstallningar[nr].started.day > 9))
                return anvTillstallningar[nr].started.year.ToString() + "-0" + anvTillstallningar[nr].started.month.ToString() + "-" + anvTillstallningar[nr].started.day.ToString();
            else if ((anvTillstallningar[nr].started.month > 9) && (anvTillstallningar[nr].started.day > 0))
                return anvTillstallningar[nr].started.year.ToString() + "-" + anvTillstallningar[nr].started.month.ToString() + "-0" + anvTillstallningar[nr].started.day.ToString();
            else if ((anvTillstallningar[nr].started.month > 0) && (anvTillstallningar[nr].started.day > 0))
                return anvTillstallningar[nr].started.year.ToString() + "-0" + anvTillstallningar[nr].started.month.ToString() + "-0" + anvTillstallningar[nr].started.day.ToString();
            else if (anvTillstallningar[nr].started.month > 9)
                return anvTillstallningar[nr].started.year.ToString() + "-" + anvTillstallningar[nr].started.month.ToString();
            else if (anvTillstallningar[nr].started.month > 0)
                return anvTillstallningar[nr].started.year.ToString() + "-0" + anvTillstallningar[nr].started.month.ToString();
            else
                return anvTillstallningar[nr].started.year.ToString();
        }
        public string getActorAttendedEventEnded(int nr)
        {
            if ((anvTillstallningar[nr].ended.month > 9) && (anvTillstallningar[nr].ended.day > 9))
                return anvTillstallningar[nr].ended.year.ToString() + "-" + anvTillstallningar[nr].ended.month.ToString() + "-" + anvTillstallningar[nr].ended.day.ToString();
            else if ((anvTillstallningar[nr].ended.month > 0) && (anvTillstallningar[nr].ended.day > 9))
                return anvTillstallningar[nr].ended.year.ToString() + "-0" + anvTillstallningar[nr].ended.month.ToString() + "-" + anvTillstallningar[nr].ended.day.ToString();
            else if ((anvTillstallningar[nr].ended.month > 9) && (anvTillstallningar[nr].ended.day > 0))
                return anvTillstallningar[nr].ended.year.ToString() + "-" + anvTillstallningar[nr].ended.month.ToString() + "-0" + anvTillstallningar[nr].ended.day.ToString();
            else if ((anvTillstallningar[nr].ended.month > 0) && (anvTillstallningar[nr].ended.day > 0))
                return anvTillstallningar[nr].ended.year.ToString() + "-0" + anvTillstallningar[nr].ended.month.ToString() + "-0" + anvTillstallningar[nr].ended.day.ToString();
            else if (anvTillstallningar[nr].ended.month > 9)
                return anvTillstallningar[nr].ended.year.ToString() + "-" + anvTillstallningar[nr].ended.month.ToString();
            else if (anvTillstallningar[nr].ended.month > 0)
                return anvTillstallningar[nr].ended.year.ToString() + "-0" + anvTillstallningar[nr].ended.month.ToString();
            else
                return anvTillstallningar[nr].ended.year.ToString();
        }
        public string getActorAttendedEventRoleTag(int nr) { return anvTillstallningar[nr].role.tag; }
        public string getActorAttendedEventRoleDescription(int nr) { return anvTillstallningar[nr].role.description; }
        public int getActorAttendedEventRoleLevel(int nr) { return anvTillstallningar[nr].role.level; }
        public bool addActorAttendedEventData(string id, string typ, string start, string ended, string roleTag)
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
                anvTillstallningar[noOfAttendedEventData].started.year = 0;
                handleDate(start, noOfAttendedEventData, currEnum.Event);
                anvTillstallningar[noOfAttendedEventData].ended.year = 0;
                handleDate(ended, noOfAttendedEventData, currEnum.Event);
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
        public bool setActorAttendedEventData(int nr, string id, string typ, string start, string ended, string roleTag)
        {
            bool retVal = false;
            if (nr < maxNoOfAttendedEventData)
            {
                anvTillstallningar[nr].eventID = id;
                bool foundCategory = false;
                for (int i = 0; i < noOfEventCategories; i++)
                {
                    if (typ == eventCategories[i].tag)
                    {
                        anvTillstallningar[nr].eventCategory = eventCategories[i].tag;
                        foundCategory = true;
                    }
                }
                if (!(foundCategory))
                {
                    addEventCategory(typ, "No description", "Undefined");
                    anvTillstallningar[nr].eventType.tag = typ;
                }
                anvTillstallningar[nr].started.year = 0;
                handleDate(start, nr, currEnum.Event);
                anvTillstallningar[nr].ended.year = 0;
                handleDate(ended, nr, currEnum.Event);
                bool foundRoleCategory = false;
                for (int i = 0; i < noOfRoleCategories; i++)
                {
                    if (roleTag == roleCategories[i].tag)
                    {
                        foundRoleCategory = true;
                        anvTillstallningar[nr].role = roleCategories[i];
                    }
                }
                if ((!(foundRoleCategory)) && (noOfRoleCategories < maxNoOfRoleCategories))
                {
                    anvTillstallningar[nr].role.tag = roleTag;
                    anvTillstallningar[nr].role.description = "";
                    anvTillstallningar[nr].role.level = 0;
                    addRoleCategory(roleTag, "", "Undefined");
                }
                retVal = true;
            }
            return retVal;
        }
        public bool removeActorAttendedEventData(int nr)
        {
            bool retVal = false;
            if ((nr >= 0) && (nr < noOfAttendedEventData))
            {
                for (int i = nr; i < noOfAttendedEventData; i++)
                {
                    anvTillstallningar[i].eventID = anvTillstallningar[i + 1].eventID;
                    anvTillstallningar[i].eventCategory = anvTillstallningar[i + 1].eventCategory;
                    anvTillstallningar[i].ended = anvTillstallningar[i + 1].ended;
                    anvTillstallningar[i].eventType = anvTillstallningar[i + 1].eventType;
                    anvTillstallningar[i].role = anvTillstallningar[i + 1].role;
                    anvTillstallningar[i].started = anvTillstallningar[i + 1].started;
                }
                retVal = true;
                noOfAttendedEventData--;
            }
            return retVal;
        }
        #endregion
        #region ReltdImages
        // --- Related Images Data ---
        private void findFilesInDir(string sDir)
        {
            string[] fileArray = System.IO.Directory.GetFiles(sDir);
            foreach (var exiFile in fileArray)
            {
                bool foundRootImagePath = false;
                for (int i = 0; i < noOfRelatedImagesData; i++)
                {
                    if (exiFile == anvRelBilder[i].imagePathName)
                    {
                        foundRootImagePath = true;
                    }
                }
                if ((!foundRootImagePath) && (noOfRelatedImagesData < maxNoOfRelatedImagesData))
                {
                    anvRelBilder[noOfRelatedImagesData].imagePathName = exiFile;
                    anvRelBilder[noOfRelatedImagesData].imageContext.tag = "Unknown";
                    anvRelBilder[noOfRelatedImagesData].imageContext.description = "";
                    anvRelBilder[noOfRelatedImagesData].imageContext.level = 0;
                    anvRelBilder[noOfRelatedImagesData].classificationLevel = ClassificationType.Unclassified;
                    noOfRelatedImagesData++;
                }
            }
        }
        public void findDirsInDir(string sDir, string sStore)
        {
            string[] directoryArray = System.IO.Directory.GetDirectories(sDir);
            foreach (var exiDir in directoryArray)
            {
                string sStepDir = exiDir;
                findFilesInDir(sStepDir);
                findDirsInDir(sStepDir, sStore);
            }
            //saveActorData(userId, sStore);
        }
        public int getNoOfRelatedImages() { return noOfRelatedImagesData; }
        public string getActorRelatedImagePath(int nr) { return anvRelBilder[nr].imagePathName; }
        public string getActorRelatedImageContent(int nr) { return anvRelBilder[Math.Max(nr, 0)].imageContext.tag; }
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
                if (!(checkEnumeration(imlvl, noOfRelatedImagesData, currEnum.RelImg)))
                    anvRelBilder[noOfRelatedImagesData].classificationLevel = ClassificationType.UndefLevel;
                noOfRelatedImagesData++;
                retVal = true;
            }
            return retVal;
        }
        public bool removeActorRelatedImage(int nr)
        {
            bool retVal = false;
            if ((nr >= 0) && (nr < maxNoOfRelatedImagesData) && (nr < noOfRelatedImagesData))
            {
                for (int i = nr; i < noOfRelatedImagesData; i++)
                {
                    anvRelBilder[i].imagePathName = anvRelBilder[i + 1].imagePathName;
                    anvRelBilder[i].imageContext = anvRelBilder[i + 1].imageContext;
                    anvRelBilder[i].classificationLevel = anvRelBilder[i + 1].classificationLevel;
                }
                retVal = true;
                noOfRelatedImagesData--;
            }
            return retVal;
        }
        #endregion
        #region UserRootDir
        public int getMaxNoOfRootDirs() { return maxNoOfRootDirs; }
        public int getNoOfRootDirs() { return noOfRootDirs; }
        public bool removeActorRootDir(string instr, string sStore)
        {
            bool retVal = false;
            for (int i = 0; i < noOfRootDirs; i++)
            {
                if (userRootDir[i] == instr)
                {
                    retVal = true;
                    for (int j = i; (j + 1) < maxNoOfRootDirs; j++)
                    {
                        userRootDir[j] = userRootDir[j + 1];
                    }
                }
            }
            noOfRootDirs = Math.Max(noOfRootDirs - 1, 0);
            saveActorData(userId, sStore);
            return retVal;
        }
        public bool addActorRootDir(string instr, string sStore) 
        {
            bool foundDir = false;
            for (int i = 0; i < noOfRootDirs; i++)
            {
                if (userRootDir[i] == instr)
                    foundDir = true;
            }
            if ((noOfRootDirs < maxNoOfRootDirs) && (!foundDir))
            {
                userRootDir[noOfRootDirs++] = instr;
                saveActorData(userId, sStore);
                return true;
            }
            return false;
        }
        public string getActorRootDir(int nr) { return userRootDir[nr]; }
        #endregion
        #region Nationality
        // --- Nationality ---
        public int getNoOfNationalityData() { return noOfNationalityData; }
        public string getNationalityData(int nr) { return anvNationalities[nr].nationName; }
        public string getNationalityName(int nr) { return anvNationalities[nr].nationName; }
        public string getNationalityTag(int nr) { return anvNationalities[nr].nationTag; }
        public int getNationalityValue(int nr) { return anvNationalities[nr].nationNumber; }
        public bool setNationalityData(string indata)
        {
            bool retVal = false;
            if (noOfNationalityData < maxNoOfNationalityData)
            {
                anvNationalities[noOfNationalityData].nationName = indata;
                noOfNationalityData++;
                retVal = true;
            }
            return retVal;
        }
        public bool setNationalityInfo(string name, string tag)
        {
            bool retVal = false;
            if (noOfNationalityData < maxNoOfNationalityData)
            {
                anvNationalities[noOfNationalityData].nationName = name;
                anvNationalities[noOfNationalityData].nationTag = tag;
                noOfNationalityData++;
                retVal = true;
            }
            return retVal;
        }
        public bool removeNationalityData(int nr)
        {
            bool retVal = false;
            if ((nr >= 0) && (nr < noOfNationalityData))
            {
                for (int i = nr; i < noOfNationalityData; i++)
                {
                    anvNationalities[i].nationName = anvNationalities[i + 1].nationName;
                    anvNationalities[i].nationTag = anvNationalities[i + 1].nationTag;
                }
                retVal = true;
                noOfNationalityData--;
            }
            return retVal;
        }
        #endregion
        // ---------------------------
    }
}
