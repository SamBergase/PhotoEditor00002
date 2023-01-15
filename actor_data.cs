using System;
using System.IO;

namespace ns_actorData
{
    #region enumerated values declarations
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
    public enum genderTypes
    {
        UndefGender, Male, Female
    }
    public enum genderApperance
    {
        UndefGdrApp, BarbieGdr, CurtainsGdr, HorseshoeGdr, PuffyGdr, TulipGdr, IGdr, IIGdr, IIIGdr, IVGdr, VGdr
    }
    public enum genderBehaviour
    {
        UndefGdrBeh, SloppyGdrBeh, StretchyGdrBeh, TightGcrBeh, StifferGdrBeh, GrowerGdrBeh
    }
    public enum contactType
    {
        UndefContType, Phone, Email, Website, Facebook, Twitter, LinkedIn, Instagram, Snapchat
    }
    public enum breastSize
    {
        UndefBreast, AA, A, B, C, D, E, F, Flat, Medium, Bulgy, Oversize
    }
    public enum breastType
    {
        UndefBrType, NaturalBrType, SiliconeBrType
    }
    public enum etnicityType
    {
        UndefEtnicity, Negro, Asian, Indian, Cocation, Mulatto
    }
    public enum hairColorType
    {
        UndefHairColor, Black, DarkBrown, Brown, LightBrown, DarkChestnut, Chestnut, LightChestnut, DarkRed, Red, LightRed, DarkGray, Gray, LightGray
    }
    public enum hairTextureType
    {
        UndefHairTexture, Straight, Wavy, Curly, Coily
    }
    public enum hairLengthType
    {
        UndefHairLength, Short, Neck, Shoulder, MidBack, Waist, Ass, Long
    }
    public enum bodyType
    {
        UndefBodyType, Athletic, Slim, Spiny, Plump, Fat
    }
    // TODO - relationType should be read from the user file.
    public enum relationType
    {
        UndefRelation, Mother, Father, Sister, Brother, Son, Daughter, Engagee, Child, Single, Friend, Workfriend, Associate, Spouse, Partner,
        Fiancee, Married, Divorced, Widow, Lover, Master, Slave, Bull, Cuckold, Widower
    }
    public enum eyeColorType
    {
        UndefEyeColor, Black, Brown, Blue, Green, Gray, Yellow
    }
    public enum markingType
    {
        UndefMarking, Scar, Freckles, Birthmark, Tattoo, Piercing
    }
    // TODO - eventType should be read from the user file.
    public enum eventType
    {
        UndefEventType, Party, PhotoSession, Picnic
    }
    // TODO - roleType should be read from the user file.
    public enum roleType
    {
        UndefRoleType, Attender, Organizer, Responsible, Producer, Acting, Slave, Master, Cuckold, Bull, Boss
    }
    // TODO - imageType should be read from the user file.
    public enum imageType
    {
        UndefImageType, Nature, Portrait, Architecture, Proof, Family, Vacation, Vehicles, Erotic, Pornographic, Cuckold
    }
    public enum classificationLevel
    {
        UndefCfnLevel, Unclassified, Limited, Confidential, Secret, QualifSecret
    }
    public enum geogrDir
    {
        UndefDir, North, East, South, West
    }
    #endregion
    public class actorName
    {
        public nameType nTyp { get; set; }
        public string surn { get; set; }
        public string middn { get; set; }
        public string famn { get; set; }

        public actorName(nameType ntp, string snm, string mnm, string fnm)
        {
            nTyp = ntp;
            surn = snm;
            middn = mnm;
            famn = fnm;
        }
        public nameType getActorNameType() { return nTyp; }
        public string getActorNameTypeString()
        {
            string retVal = "";
            switch (nTyp)
            {
                case nameType.Alias:
                    retVal = "Alias";
                    break;
                case nameType.Birth:
                    retVal = "Birth";
                    break;
                case nameType.Married:
                    retVal = "Married";
                    break;
                case nameType.Nickname:
                    retVal = "Nick";
                    break;
                case nameType.Taken:
                    retVal = "Taken";
                    break;
                default:
                    retVal = "Undefined";
                    break;
            }
            return retVal;
        }
        public string getActorSurname() { return surn; }
        public string getActorMidname() { return middn; }
        public string getActorFamilyname() { return famn; }
    }
    /* --- addressData ---
        public struct addressData
        {
            string streetname;
            uint number;
            char addon;
            uint zipcode;
            string areaname;
            string cityname;
            string country;
            int latdeg;
            int latmin;
            int latsec;
            int latsemisec;
            geogrDir latdir;
            int londeg;
            int lonmin;
            int lonsec;
            int lonsemisec;
            geogrDir londir;
        }
       ------------------- */
    /* --- ActorBirthValues ---
        public struct ActorBirthValues
        {
            DateTime BirthDate;
            uint securityNumber;
            addressData birthPlace;
            genderTypes birthGender;
        }
       ------------------- */
    /* --- ActorGenderValues ---
        public struct ActorGenderValues
        {
            genderTypes actorGenderType;
            genderApperance actorGenderApperance;
            genderBehaviour actorGenderBehaviour;
            string actorGenderPresentation;
            string actorPreferences;
            float length;
            float circumference;
            distanceUnits usedUnit;
            DateTime validDate;
        }
       ------------------- */
    public class actorContact
    {
        public contactType cTyp { get; set; }
        public string cPath { get; set; }

        public actorContact(contactType ctp, string cpth)
        {
            cTyp = ctp;
            cPath = cpth;
        }
        public contactType getContactType() { return cTyp; }
        public string getContactTypeString()
        {
            string retVal = "";
            switch (cTyp)
            {
                case contactType.Email:
                    retVal = "Email";
                    break;
                case contactType.Facebook:
                    retVal = "Facebook";
                    break;
                case contactType.Instagram:
                    retVal = "Instagram";
                    break;
                case contactType.LinkedIn:
                    retVal = "LinkedIn";
                    break;
                case contactType.Phone:
                    retVal = "Phone";
                    break;
                case contactType.Snapchat:
                    retVal = "Snapchat";
                    break;
                case contactType.Twitter:
                    retVal = "Twitter";
                    break;
                case contactType.Website:
                    retVal = "Website";
                    break;
                default:
                    retVal = "Undefined";
                    break;
            }
            return retVal;
        }
        public string getContactPath() { return cPath; }
    }
    /* -------------------
        public struct ActorLengthValues
        {
            float actorLength;
            distanceUnits usedUnits;
            DateTime validDate;
        }
       ------------------- */
    /* -------------------
        public struct ActorWeightValues
        {
            float actorWeight;
            weightUnits usedUnits;
            DateTime validDate;
        }
       ------------------- */
    /* -------------------
        public struct ActorChestValues
        {
            float actorChestCircumference;
            distanceUnits usedUnits;
            breastSize actorBreastSize;
            breastType actorBreastType;
            DateTime validDate;
        }
       ------------------- */
    /* -------------------
        public struct ActorWaistValues
        {
            float actorWaistCircumference;
            distanceUnits usedUnits;
            DateTime validDate;
        }
       ------------------- */
    /* -------------------
    public struct ActorHipValues
    {
        float actorHipCircumference;
        distanceUnits usedUnits;
        DateTime validDate;
    }
       ------------------- */
    /* -------------------
    public struct ActorComplexionValues
    {
        etnicityType actorEtnicity;
        uint red;
        uint green;
        uint blue;
        DateTime validDate;
    }
       ------------------- */
    /* -------------------
    public struct ActorFaceformValues
    {
        float actorEyesHeightWidth;
        float actorCheekboneHeightWidth;
        float actorMouthHeightWidth;
        float actorChinHeightWidth;
        float actorFaceCenterHeight;
        distanceUnits usedUnits;
        DateTime validDate;
    }
       ------------------- */
    /* -------------------
    public struct ActorResicenceValues
    {
        addressData actorResidenceAddress;
        float actorResidenceBuyValue;
        float actorResidenceSaleValue;
        currencyUnits usedUnits;
        DateTime actorResidenceMovedIn;
        DateTime actorResidenceMovedOut;
    }
       ------------------- */
    /* -------------------
    public struct ActorHairValues
    {
        hairColorType actorHairColor;
        hairTextureType actorHairTexture;
        hairLengthType actorHairLength;
        DateTime validDate;
    }
       ------------------- */
    /* -------------------
    public struct ActorMarkingValues
    {
        markingType actorMarkingType;
        string actorMarkingPosition;
        string actorMarkingMotif;
        DateTime validDate;
    }
    --------------------- */
    /* -------------------
        public struct ActorOccupationValues
        {
            string actorOccupationTitle;
            string actorOccupationWorkdescr;
            string actorOccupationCompany;
            addressData actorOccupationAddress;
            float actorOccupationSallary;
            currencyUnits usedUnits;
            DateTime actorOccupationStarted;
            DateTime actorOccupationEnded;
        }
    --------------------- */
    /* -------------------
        public struct ActorRelatedEventValues
        {
            string actorRelatedEventId;
        }
    --------------------- */
    /* -------------------
        public struct ActorRelationValues
        {
            relationType actorRelationType;
            ActorNameValues actorRelationRelatorName;
            genderTypes actorRelationRelatorGender;
            DateTime actorRelationStarted;
            DateTime actorRelationEnded;
            //            bool actorRelationRelatedEventsOverflow;
            uint actorRelationNoOfRelatedEvents;
            public ActorRelatedEventValues[] arrActorRelationRelatedEvents;
        }
    --------------------- */
    /* -------------------
        public struct ActorAttendedEventsValues
        {
            string actorAttendedEventsId;
            eventType actorAttendedEventCategory;
            string actorAttendedEventTitle;
            classificationLevel actorAttendedEventClassificationLevel;
            string actorAttendedEventProducer;
            string actorAttendedEventDescription;
            roleType actorAttendedEventRole;
            DateTime validDate;
            float actorAttendedEventDuration;
        }
    --------------------- */
    /* -------------------
        public struct ActorRelatedImagesValues
        {
            string actorRelatedImagesPathName;
            imageType actorRelatedImageType;                            // Collect from image?
            string actorRelatedImageDescription;                        // Collect from image?
            classificationLevel actorRelatedImageClassificationLevel;   // Collect from image?
            DateTime actorRelatedImageExposureDateTime;                 // Collect from image?
        }
    --------------------- */
    /* -------------------
        public struct ActorBodyTypeValues
        {
            bodyType actorBodyType;
            DateTime validDate;
        }
       ------------------- */

    public class actor_data
    {
        public string userIdentity = "";
        private uint noOfNames = 0;
//        private nameType[] _namesTypes;
        private actorName[] names;
        private uint noOfContacts = 0;
        private actorContact[] contacts;

//        public static nameType[] actorNameType { get => actor_data._namesTypes; set => actor_data._namesTypes = value; }

        public actor_data(string userId)
        {
            this.userIdentity = userId;

            // string actorDataFilename = System.IO.Directory.GetCurrentDirectory() + userId + ".acf";
            string actorDataFilename = "C:\\Users\\esbberg\\source\\repos\\PhotoEditor00001\\PhotoEditor00001\\ActorData\\ActorData_" + userId + ".acf";

            if (System.IO.File.Exists(actorDataFilename))
            {
                // Standard content handling
                foreach (string line in System.IO.File.ReadLines(actorDataFilename))
                {
                    string actorDataTag = line.Substring(0, 8);
                    string actorDataInfo = line.Substring(11, line.Length - 11);
                    switch (actorDataTag)
                    {
                        case "UserID  ":
                            {
                                this.userIdentity = actorDataInfo;
                            }
                            break;
                        case "UserName":
                            {
                                string tempNameType = "";
                                string tempNameSet = "";
                                string tmpActSurn = "";
                                string tmpActMiddn = "";
                                string tmpActFamn = "";

                                int dp0 = actorDataInfo.IndexOf(";");
                                if ((dp0 > 0) && (dp0 < actorDataInfo.Length - 2))
                                {
                                    // Have both names and type.
                                    tempNameType = actorDataInfo.Substring(dp0 + 2, actorDataInfo.Length - dp0 - 2);
                                    tempNameSet = actorDataInfo.Substring(0, dp0);
                                }
                                else
                                {
                                    // Have only one entry, name?
                                    tempNameType = "Unknown";
                                    tempNameSet = actorDataInfo;
                                }

                                int dp1 = tempNameSet.IndexOf(" ");
                                if ((dp1 > 0) && (dp1 < tempNameSet.Length - 2))
                                {
                                    // At least "Surname" and something else.
                                    tmpActSurn = tempNameSet.Substring(0, dp1);
                                    tempNameSet = tempNameSet.Substring(dp1 + 1, tempNameSet.Length - dp1 - 1);
                                    int dp2 = tempNameSet.IndexOf(" ");
                                    if ((dp2 > 0) && (dp1 < tempNameSet.Length - 2))
                                    {
                                        // "midname" and "familyname"
                                        tmpActMiddn = tempNameSet.Substring(0, dp2);
                                        tmpActFamn = tempNameSet.Substring(dp2 + 1, tempNameSet.Length - dp2 - 1);
                                    }
                                    else
                                    {
                                        // only "familyname"
                                        tmpActFamn = tempNameSet;
                                    }
                                }
                                else
                                {
                                    // only "surname"
                                    tmpActSurn = tempNameSet;
                                }

                                if (tempNameType == "Alias")
                                    this.names[noOfNames].nTyp = nameType.Alias;
                                else if (tempNameType == "Birth")
                                    this.names[noOfNames].nTyp = nameType.Birth;
                                else if (tempNameType == "Married")
                                    this.names[noOfNames].nTyp = nameType.Married;
                                else if (tempNameType == "Nick")
                                    this.names[noOfNames].nTyp = nameType.Nickname;
                                else if (tempNameType == "Taken")
                                    this.names[noOfNames].nTyp = nameType.Taken;
                                else
                                    this.names[noOfNames].nTyp = nameType.UndefNameType;

                                this.names[noOfNames].surn = tmpActSurn;
                                this.names[noOfNames].middn = tmpActMiddn;
                                this.names[noOfNames].famn = tmpActFamn;

                                this.noOfNames++;
                            }
                            break;
                        case "Contact ":
                            {
                                // Contact  : <type>; Path
                                string tempContactType = "";
                                string tempContactPath = "";

                                int dp0 = actorDataInfo.IndexOf(";");
                                if ((dp0 > 0) && (dp0 < actorDataInfo.Length - 2))
                                {
                                    // Have both type and path
                                    tempContactType = actorDataInfo.Substring(0, dp0);
                                    tempContactPath = actorDataInfo.Substring(dp0 + 2, actorDataInfo.Length - dp0 - 2);
                                }
                                else
                                {
                                    // Have merely one of type or path
                                    tempContactType = "Undefined";
                                    tempContactPath = actorDataInfo;
                                }

                                if (tempContactType == "Email")
                                    this.contacts[noOfContacts].cTyp = contactType.Email;
                                else if (tempContactType == "Facebook")
                                    this.contacts[noOfContacts].cTyp = contactType.Facebook;
                                else if (tempContactType == "Instagram")
                                    this.contacts[noOfContacts].cTyp = contactType.Instagram;
                                else if (tempContactType == "LinkedIn")
                                    this.contacts[noOfContacts].cTyp = contactType.LinkedIn;
                                else if (tempContactType == "Phone")
                                    this.contacts[noOfContacts].cTyp = contactType.Phone;
                                else if (tempContactType == "Snapchat")
                                    this.contacts[noOfContacts].cTyp = contactType.Snapchat;
                                else if (tempContactType == "Twitter")
                                    this.contacts[noOfContacts].cTyp = contactType.Twitter;
                                else if (tempContactType == "Website")
                                    this.contacts[noOfContacts].cTyp = contactType.Website;
                                else
                                {
                                    if (tempContactPath.Contains("facebook"))
                                        this.contacts[noOfContacts].cTyp = contactType.Facebook;
                                    else if (tempContactPath.Contains("instagram"))
                                        this.contacts[noOfContacts].cTyp = contactType.Instagram;
                                    else if (tempContactPath.Contains("linkedin"))
                                        this.contacts[noOfContacts].cTyp = contactType.LinkedIn;
                                    else if (tempContactPath.Contains("snapchat"))
                                        this.contacts[noOfContacts].cTyp = contactType.Snapchat;
                                    else if (tempContactPath.Contains("twitter"))
                                        this.contacts[noOfContacts].cTyp = contactType.Twitter;
                                    else
                                        this.contacts[noOfContacts].cTyp = contactType.UndefContType;

                                    this.contacts[noOfContacts].cPath = tempContactPath;
                                    this.noOfContacts++;
                                }
                            }
                            break;
                            // BrthData : <Streetname>; <number>; <no-add>; <city>; <region>; <zipcode>; <country>; <date>; <sec-no>; <gender>
                            // SkinTone : <type>; <r-val>; <g-val>; <b-val>; <date>
                            // RelStats : <relation status>
                            // Gender   : <gender type>; <length>; <circ>; <Unit>; <gen appearance>; <gender behaviour>; <gender pres>; <date>
                            // Length   : <float value>; <unit>; <date>
                            // Weight   : <float value>; <unit>; <date>
                            // Boobs    : <type>; <
                        default:
                            {

                            }
                            break;
                    }
                }
            }
            else
            {
                //actorDataFilename = System.IO.Directory.GetCurrentDirectory() + userId + ".txt";
                actorDataFilename = "C:\\Users\\esbberg\\source\\repos\\PhotoEditor00001\\PhotoEditor00001\\ActorData\\ActorData_" + userId + ".txt";
                if (System.IO.File.Exists(actorDataFilename))
                {
                    // Textual content handling
                }
            }
        }

        public uint getNoOfNames() { return noOfNames; }
    }
}