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
    public enum eventClassificationType
    {
        Undefined, Unclassified, Limited, Confidential, Secret, QualifSecret
    }
    public enum eventGeographicDirection
    {
        Undefined, North, South, East, West
    }
    public enum altitudeUnit
    {
        Undefined, mm, cm, dm, m, km, inch, foot, yard
    }
    public enum durationTypeDef
    {
        Undefined, Years, Months, Days, Time, Hours, Minutes, Seconds
    }
    public enum usedLangType
    {
        Undefined, Spoken, Dubbed, Subtext
    }
    #endregion
    #region struct-definitions
    public struct defaultEventStruct
    {
        public string tag;
        public string description;
        public int level;
    }
    public struct eventTimeStruct
    {
        public int year;
        public int month;
        public int day;
        public float duration;
        public durationTypeDef durtyp;
    }
    public struct eventGeographicPosition
    {
        public int latDeg;
        public int latMin;
        public int latSec;
        public int latSemiSec;
        public eventGeographicDirection latDir;
        public int lonDeg;
        public int lonMin;
        public int lonSec;
        public int lonSemiSec;
        public eventGeographicDirection lonDir;
        public float altitude;
        public altitudeUnit unit;
        public string geographicName;
    }
    public struct eventAddress
    {
        public string streetname;
        public int housenumber;
        public string housenumberaddon;
        public int floornumber;
        public string cityname;
        public string areaname;
        public string areacode;
        public string statename;
        public string countryname;
    }
    public struct eventAttender
    {
        public string eventAttenderId;
        public eventClassificationType eventAttenderLevel;
        public defaultEventStruct eventAttenderRole;
    }
    public struct eventImages
    {
        public string eventImagePathName;
        public eventClassificationType eventImageLevel;
        public defaultEventStruct eventImageContent;
    }
    public struct noteableEvent
    {
        public int year;
        public int month;
        public int day;
        public int largeTime;
        public int midTime;
        public int smallTime;
        public durationTypeDef timeType;
        public defaultEventStruct eventType;
    }
    public struct usedLanguage
    {
        public string name;
        public usedLangType type;
    }
    #endregion
    class PEEventClass
    {
        #region programParameters
        private string eventId;
        private eventClassificationType classificationLevel;
//        private string eventStorage;

        public const int maxNoOfEventCategories = 250;
        public int noOfEventCategories = 0;
        public defaultEventStruct[] eventCatagories = new defaultEventStruct[maxNoOfEventCategories];

        public const int maxNoOfContentCategories = 250;
        public int noOfContentCategories = 0;
        public defaultEventStruct[] contentCategories = new defaultEventStruct[maxNoOfContentCategories];

        public const int maxNoOfRoleCategories = 250;
        public int noOfRoleCategories = 0;
        public defaultEventStruct[] roleCategories = new defaultEventStruct[maxNoOfRoleCategories];

        public string eventOwner;
        public eventTimeStruct eventStarted;
        public eventTimeStruct eventEnded;
        public eventGeographicPosition geoPos;
        public eventAddress theEventAddress;
        public defaultEventStruct typeOfEvent; // one of eventCategories
        public string headlineDescription;
        public string objectTag;

        public const int maxNoOfEventAttender = 250;
        public int noOfEventAttender = 0;
        public eventAttender[] thisEventAttenders = new eventAttender[maxNoOfEventAttender];

        public const int maxNoOfEventImages = 500;
        public int noOfEventImages = 0;
        public eventImages[] thisEventImages = new eventImages[maxNoOfEventImages];

        public const int maxNoOfEventContentTags = 75;
        public int noOfEventContentTags = 0;
        public string[] thisEventContentTags = new string[maxNoOfEventContentTags];

        public const int maxNoOfDescriptionLines = 36;
        public int noOfDescriptionLines = 0;
        public string[] descriptionOfEvent = new string[maxNoOfDescriptionLines];

        public const int maxNoOfNoteableEvents = 36;
        public int noOfNoteableEvents = 0;
        public noteableEvent[] noteableEventInEvent = new noteableEvent[maxNoOfNoteableEvents];

        public const int maxNoOfUsedLanguages = 32;
        public int noOfUsedLanguages = 0;
        public usedLanguage[] thisEventUsedLanguage = new usedLanguage[maxNoOfUsedLanguages];

        public const int maxNoOfEventRootDirs = 16;
        public int noOfEventRootDirs = 0;
        public string[] eventRootDirs = new string[maxNoOfEventRootDirs];

        public string[] ClassificationStrings = { "Undefined", "Unclassified", "Limited", "Confidential", "Secret", "QualifSecret" };

        public bool needsSaving = false;
        #endregion
        public int getNoOfClassificationStrings() { return ClassificationStrings.Length; }
        public string getClassificationNo(int nr) { return ClassificationStrings[nr]; }
        public bool addEventCategory(string tag, string description, string level)
        {
            bool retVal = false;
            if (noOfEventCategories < maxNoOfEventCategories)
            {
                bool foundCategory = false;
                for (int i = 0; i < noOfEventCategories; i++)
                {
                    if (tag == eventCatagories[i].tag)
                        foundCategory = true;
                }
                if (!(foundCategory))
                {
                    eventCatagories[noOfEventCategories].tag = tag;
                    eventCatagories[noOfEventCategories].description = description;
                    if ((level == "Open") || (level == "Unclassified"))
                        eventCatagories[noOfEventCategories].level = 1;
                    else if (level == "Limited")
                        eventCatagories[noOfEventCategories].level = 2;
                    else if ((level == "Confidential") || (level == "Medium") || (level == "Relative"))
                        eventCatagories[noOfEventCategories].level = 3;
                    else if (level == "Secret")
                        eventCatagories[noOfEventCategories].level = 4;
                    else if (level == "QualifSecret")
                        eventCatagories[noOfEventCategories].level = 5;
                    else
                        eventCatagories[noOfEventCategories].level = 0;
                    noOfEventCategories++;
                    retVal = true;
                }
            }
            return retVal;
        }
        public bool addContentCategory(string tag, string description, string level)
        {
            bool retVal = false;
            if (noOfContentCategories < maxNoOfContentCategories)
            {
                bool foundCategory = false;
                for (int i = 0; i < noOfContentCategories; i++)
                {
                    if (tag == contentCategories[i].tag)
                        foundCategory = true;
                }
                if (!(foundCategory))
                {
                    contentCategories[noOfContentCategories].tag = tag;
                    contentCategories[noOfContentCategories].description = description;
                    if ((level == "Open") || (level == "Unclassified"))
                        contentCategories[noOfContentCategories].level = 1;
                    else if (level == "Limited")
                        contentCategories[noOfContentCategories].level = 2;
                    else if ((level == "Confidential") || (level == "Medium") || (level == "Relative"))
                        contentCategories[noOfContentCategories].level = 3;
                    else if (level == "Secret")
                        contentCategories[noOfContentCategories].level = 4;
                    else if (level == "QualifSecret")
                        contentCategories[noOfContentCategories].level = 5;
                    else
                        contentCategories[noOfContentCategories].level = 0;
                    noOfContentCategories++;
                    retVal = true;
                }
            }
            return retVal;
        }
        public bool addRoleCategory(string tag, string description, string level)
        {
            bool retVal = false;
            if (noOfRoleCategories < maxNoOfRoleCategories)
            {
                bool foundCategory = false;
                for (int i = 0; i < noOfRoleCategories; i++)
                {
                    if (tag == roleCategories[i].tag)
                        foundCategory = true;
                }
                if (!(foundCategory))
                {
                    roleCategories[noOfRoleCategories].tag = tag;
                    roleCategories[noOfRoleCategories].description = description;
                    if ((level == "Open") || (level == "Unclassified"))
                        roleCategories[noOfRoleCategories].level = 1;
                    else if (level == "Limited")
                        roleCategories[noOfRoleCategories].level = 2;
                    else if ((level == "Confidential") || (level == "Medium") || (level == "Relative"))
                        roleCategories[noOfRoleCategories].level = 3;
                    else if (level == "Secret")
                        roleCategories[noOfRoleCategories].level = 4;
                    else if (level == "QualifSecret")
                        roleCategories[noOfRoleCategories].level = 5;
                    else
                        roleCategories[noOfRoleCategories].level = 0;
                    noOfRoleCategories++;
                    retVal = true;
                }
            }
            return retVal;
        }
        public bool checkEnumeration(string tag, int number)
        {
            bool retVal = false;
            switch (tag.ToLower())
            {
                case "open":
                case "unclassified":
                case "1":
                    {
                        if (number >= 0)
                        {
                            if (number < maxNoOfEventImages)
                            {
                                this.thisEventImages[number].eventImageLevel = eventClassificationType.Unclassified;
                                retVal = true;
                            }
                            else
                                retVal = false;
                        }
                        else
                        {
                            this.classificationLevel = eventClassificationType.Unclassified;
                            retVal = true;
                        }
                    }
                    break;
                case "limited":
                case "2":
                    {
                        if (number < maxNoOfEventImages)
                        {
                            this.thisEventImages[number].eventImageLevel = eventClassificationType.Limited;
                            retVal = true;
                        }
                        else
                        {
                            this.classificationLevel = eventClassificationType.Limited;
                            retVal = true;
                        }
                    }
                    break;
                case "confidential":
                case "3":
                    {
                        if (number < maxNoOfEventImages)
                        {
                            this.thisEventImages[number].eventImageLevel = eventClassificationType.Confidential;
                            retVal = true;
                        }
                        else
                        {
                            this.classificationLevel = eventClassificationType.Confidential;
                            retVal = true;
                        }
                    }
                    break;
                case "secret":
                case "4":
                    {
                        if (number < maxNoOfEventImages)
                        {
                            this.thisEventImages[number].eventImageLevel = eventClassificationType.Secret;
                            retVal = true;
                        }
                        else
                        {
                            this.classificationLevel = eventClassificationType.Secret;
                            retVal = true;
                        }
                    }
                    break;
                case "qualifsecret":
                case "5":
                    {
                        if (number < maxNoOfEventImages)
                        {
                            this.thisEventImages[number].eventImageLevel = eventClassificationType.QualifSecret;
                            retVal = true;
                        }
                        else
                        {
                            this.classificationLevel = eventClassificationType.QualifSecret;
                            retVal = true;
                        }
                    }
                    break;
                case "time":
                    {
                        if (this.eventStarted.durtyp == durationTypeDef.Undefined)
                        {
                            this.eventStarted.durtyp = durationTypeDef.Time;
                            retVal = true;
                        }
                        else if (this.eventEnded.durtyp == durationTypeDef.Undefined)
                        {
                            this.eventEnded.durtyp = durationTypeDef.Time;
                            retVal = true;
                        }
                        else
                            retVal = false;
                    }
                    break;
                case "hours":
                    {
                        if (this.eventStarted.durtyp == durationTypeDef.Undefined)
                        {
                            this.eventStarted.durtyp = durationTypeDef.Hours;
                            retVal = true;
                        }
                        else if (this.eventEnded.durtyp == durationTypeDef.Undefined)
                        {
                            this.eventEnded.durtyp = durationTypeDef.Hours;
                            retVal = true;
                        }
                        else
                            retVal = false;
                    }
                    break;
                case "minutes":
                    {
                        if (this.eventStarted.durtyp == durationTypeDef.Undefined)
                        {
                            this.eventStarted.durtyp = durationTypeDef.Minutes;
                            retVal = true;
                        }
                        else if (this.eventEnded.durtyp == durationTypeDef.Undefined)
                        {
                            this.eventEnded.durtyp = durationTypeDef.Minutes;
                            retVal = true;
                        }
                        else
                            retVal = false;
                    }
                    break;
                case "seconds":
                    {
                        if (this.eventStarted.durtyp == durationTypeDef.Undefined)
                        {
                            this.eventStarted.durtyp = durationTypeDef.Seconds;
                            retVal = true;
                        }
                        else if (this.eventEnded.durtyp == durationTypeDef.Undefined)
                        {
                            this.eventEnded.durtyp = durationTypeDef.Seconds;
                            retVal = true;
                        }
                        else
                            retVal = false;
                    }
                    break;
                case "north":
                case "n":
                    {
                        geoPos.latDir = eventGeographicDirection.North;
                        retVal = true;
                    }
                    break;
                case "south":
                case "s":
                    {
                        geoPos.latDir = eventGeographicDirection.South;
                        retVal = true;
                    }
                    break;
                case "east":
                case "e":
                    {
                        geoPos.lonDir = eventGeographicDirection.East;
                        retVal = true;
                    }
                    break;
                case "west":
                case "w":
                    {
                        geoPos.lonDir = eventGeographicDirection.West;
                        retVal = true;
                    }
                    break;
                default:
                    {
                        retVal = false;
                    }
                    break;
            }
            return retVal;
        }
        public void loadEvent(string eventID, string storagePath)
        {
            string filename = "";
            if ((storagePath != "") && (System.IO.Directory.Exists(storagePath)) &&
                (System.IO.File.Exists(storagePath + "\\" + eventID)))
                filename = storagePath + "\\" + eventID;
            else if ((storagePath != "") && (System.IO.Directory.Exists(storagePath)) &&
                (System.IO.File.Exists(storagePath + "\\EventData_" + eventID + ".edf")))
                filename = storagePath + "\\EventData_" + eventID + ".edf";
            else
            {
                string baseDir = System.AppContext.BaseDirectory;
                // TODO - Rewrite this to cope with current position and/or the lower info.
                string scu = WindowsIdentity.GetCurrent().Name;
                string sProgPath = "source\\repos\\PhotoEditor00002\\PhotoEditor00002";
                int tnr = scu.IndexOf("\\");
                if ((tnr > 0) && (tnr < scu.Length - 1))
                    scu = scu.Substring(tnr + 1, scu.Length - tnr - 1);
                string rootPath = "C:\\Users\\" + scu + "\\" + sProgPath;
                filename = rootPath + "\\EventData\\" + eventID;
            }

            if (System.IO.File.Exists(filename))
            {
                foreach (string line in System.IO.File.ReadLines(filename))
                {
                    if ((line != "-1") && (line.Length > 11))
                    {
                        int dp0, dp1, dp2, dp3;
                        string dataTag = line.Substring(0, 8);
                        string dataInfo = line.Substring(11, line.Length - 11);
                        switch (dataTag)
                        {
                            case "Identity":
                                {
                                    this.eventId = dataInfo;
                                }
                                break;
                            case "Secrecy ":
                                {
                                    if (!(checkEnumeration(dataInfo, 0)))
                                        this.classificationLevel = eventClassificationType.Undefined;
                                }
                                break;
                            case "EvtOwner":
                                {
                                    this.eventOwner = dataInfo;
                                }
                                break;
                            case "StrtYear":
                                {
                                    if ((dataInfo != "") && (dataInfo != "0"))
                                    {
                                        this.eventStarted.year = int.Parse(dataInfo);
                                        needsSaving = true;
                                    }
                                }
                                break;
                            case "StrtMnth":
                                {
                                    if ((dataInfo != "") && (dataInfo != "0"))
                                    {
                                        this.eventStarted.month = int.Parse(dataInfo);
                                        needsSaving = true;
                                    }
                                }
                                break;
                            case "StrtHour":
                                {
                                    if ((dataInfo != "") && (dataInfo != "0"))
                                    {
                                        string tmpvar = this.eventStarted.duration.ToString();
                                        int dp = tmpvar.IndexOf(".");
                                        if ((dp > 0) && (dp < tmpvar.Length))
                                        {
                                            tmpvar = dataInfo + "." + tmpvar.Substring(dp + 1, tmpvar.Length - dp - 1);
                                            this.eventStarted.duration = float.Parse(tmpvar);
                                            needsSaving = true;
                                        }
                                    }
                                }
                                break;
                            case "StrtMin ":
                                {
                                    if ((dataInfo != "") && (dataInfo != "0"))
                                    {
                                        string tmpvar = this.eventStarted.duration.ToString();
                                        int dp = tmpvar.IndexOf(".");
                                        if ((dp > 0) && (dp < tmpvar.Length))
                                        {
                                            tmpvar = tmpvar.Substring(0, dp) + "." + dataInfo;
                                            this.eventStarted.duration = float.Parse(tmpvar);
                                            needsSaving = true;
                                        }
                                    }
                                }
                                break;
                            case "EvtTime ":
                                {
                                    // Format : EvtTime  : <date>; <time[Y|M|D|H|s]|"Unknown">; <type of event>
                                    dp0 = dataInfo.IndexOf(";");
                                    if ((dp0 > 0) && (dp0 < dataInfo.Length))
                                    {
                                        string sPossDate = dataInfo.Substring(0, dp0);
                                        if ((sPossDate.ToLower() != "undefined") && (sPossDate.ToLower() != "unknown"))
                                        {
                                            int iTV01 = 0;
                                            int iTV02 = 0;
                                            int iTV03 = 0;
                                            dp1 = sPossDate.IndexOf("-");
                                            if ((dp1 > 0) && (dp1 < sPossDate.Length))
                                            {
                                                // Have "-" delimeter, format year-month-day or day-month-year
                                                iTV01 = int.Parse(sPossDate.Substring(0, dp1));
                                                if (iTV01 < 100)
                                                    iTV01 = iTV01 + 1900;
                                                sPossDate = sPossDate.Substring(dp1 + 1, sPossDate.Length - dp1 - 1);
                                                dp1 = sPossDate.IndexOf("-");
                                                if ((dp1 > 0) && (dp1 < sPossDate.Length))
                                                {
                                                    // have part 2 and 3
                                                    iTV02 = int.Parse(sPossDate.Substring(0, dp1));
                                                    iTV03 = int.Parse(sPossDate.Substring(dp1 + 1, sPossDate.Length - dp1 - 1));
                                                }
                                                else
                                                {
                                                    iTV02 = 0;
                                                    iTV03 = 0;
                                                }
                                            }
                                            else
                                            {
                                                // Have no delimeter YYYYMMDD(8)|YYMMDD(6)|YYYY(4)
                                                if (sPossDate.Length > 7)
                                                {
                                                    iTV01 = int.Parse(sPossDate.Substring(0, 4));
                                                    iTV02 = int.Parse(sPossDate.Substring(4, 2));
                                                    iTV03 = int.Parse(sPossDate.Substring(6, 2));
                                                }
                                                else if (sPossDate.Length > 5)
                                                {
                                                    iTV01 = int.Parse(sPossDate.Substring(0, 2)) + 1900;
                                                    iTV02 = int.Parse(sPossDate.Substring(2, 2));
                                                    iTV03 = int.Parse(sPossDate.Substring(4, 2));
                                                }
                                                else if (sPossDate.Length > 3)
                                                {
                                                    iTV01 = int.Parse(sPossDate);
                                                }
                                                else
                                                {
                                                    iTV01 = int.Parse(sPossDate) + 1900;
                                                }
                                            }
                                            noteableEventInEvent[noOfNoteableEvents].year = iTV01;
                                            noteableEventInEvent[noOfNoteableEvents].month = iTV02;
                                            noteableEventInEvent[noOfNoteableEvents].day = iTV03;
                                        }
                                        dataInfo = dataInfo.Substring(dp0 + 2, dataInfo.Length - dp0 - 2);
                                        // Format : <time[Y|M|D|H|s]|"Unknown">; <type of event>
                                        dp1 = dataInfo.IndexOf(";");
                                        if ((dp1 > 0) && (dp1 < dataInfo.Length))
                                        {
                                            // Time and type to handle
                                            string sSomeTime = dataInfo.Substring(0, dp1);
                                            char cTimeType = char.Parse(sSomeTime.Substring(sSomeTime.Length - 1, 1));
                                            if ((cTimeType == 'Y') || (cTimeType == 'y'))
                                            {
                                                sSomeTime = sSomeTime.Substring(0, sSomeTime.Length - 1);
                                                noteableEventInEvent[noOfNoteableEvents].timeType = durationTypeDef.Years;
                                            }
                                            else if (cTimeType == 'M')
                                            {
                                                sSomeTime = sSomeTime.Substring(0, sSomeTime.Length - 1);
                                                noteableEventInEvent[noOfNoteableEvents].timeType = durationTypeDef.Months;
                                            }
                                            else if ((cTimeType == 'D') || (cTimeType == 'd'))
                                            {
                                                sSomeTime = sSomeTime.Substring(0, sSomeTime.Length - 1);
                                                noteableEventInEvent[noOfNoteableEvents].timeType = durationTypeDef.Days;
                                            }
                                            else if ((cTimeType == 'H') || (cTimeType == 'h'))
                                            {
                                                sSomeTime = sSomeTime.Substring(0, sSomeTime.Length - 1);
                                                noteableEventInEvent[noOfNoteableEvents].timeType = durationTypeDef.Hours;
                                            }
                                            else if (cTimeType == 'm')
                                            {
                                                sSomeTime = sSomeTime.Substring(0, sSomeTime.Length - 1);
                                                noteableEventInEvent[noOfNoteableEvents].timeType = durationTypeDef.Minutes;
                                            }
                                            else if ((cTimeType == 'S') || (cTimeType == 's'))
                                            {
                                                sSomeTime = sSomeTime.Substring(0, sSomeTime.Length - 1);
                                                noteableEventInEvent[noOfNoteableEvents].timeType = durationTypeDef.Seconds;
                                            }
                                            if ((sSomeTime.ToLower() != "undefined") && (sSomeTime.ToLower() != "unknown"))
                                            {
                                                dp2 = sSomeTime.IndexOf(":");
                                                if ((dp2 > 0) && (dp2 < sSomeTime.Length))
                                                {
                                                    noteableEventInEvent[noOfNoteableEvents].largeTime = int.Parse(sSomeTime.Substring(0, dp2));
                                                    sSomeTime = sSomeTime.Substring(dp2 + 1, sSomeTime.Length - dp2 - 1);
                                                    dp3 = sSomeTime.IndexOf(".");
                                                    if ((dp3 > 0) && (dp3 < sSomeTime.Length))
                                                    {
                                                        noteableEventInEvent[noOfNoteableEvents].midTime = int.Parse(sSomeTime.Substring(0, dp3));
                                                        noteableEventInEvent[noOfNoteableEvents].smallTime = int.Parse(sSomeTime.Substring(dp3 + 1, sSomeTime.Length - dp3 - 1));
                                                    }
                                                    else
                                                    {
                                                        noteableEventInEvent[noOfNoteableEvents].smallTime = int.Parse(sSomeTime);
                                                    }
                                                }
                                                else
                                                {
                                                    if (sSomeTime.Length > 4)
                                                    {
                                                        // Numbers format: XXXxx|XXXXxx
                                                        noteableEventInEvent[noOfNoteableEvents].smallTime = int.Parse(sSomeTime.Substring(sSomeTime.Length - 2, 2));
                                                        noteableEventInEvent[noOfNoteableEvents].midTime = int.Parse(sSomeTime.Substring(sSomeTime.Length - 4, 2));
                                                        noteableEventInEvent[noOfNoteableEvents].largeTime = int.Parse(sSomeTime.Substring(0, sSomeTime.Length - 4));
                                                    }
                                                    else if (sSomeTime.Length > 2)
                                                    {
                                                        // Numbers format: Xxx|XXxx
                                                        noteableEventInEvent[noOfNoteableEvents].smallTime = int.Parse(sSomeTime.Substring(sSomeTime.Length - 2, 2));
                                                        noteableEventInEvent[noOfNoteableEvents].largeTime = int.Parse(sSomeTime.Substring(0, sSomeTime.Length - 1));
                                                    }
                                                    else
                                                    {
                                                        // Numbers format: xx|x
                                                        noteableEventInEvent[noOfNoteableEvents].smallTime = int.Parse(sSomeTime);
                                                    }
                                                }
                                            }
                                            dataInfo = dataInfo.Substring(dp1 + 2, dataInfo.Length - dp1 - 2);
                                            // Format : <type of event>
                                            bool foundType = false;
                                            for (int i = 0; i < noOfEventCategories; i++)
                                            {
                                                if (eventCatagories[i].tag == dataInfo)
                                                {
                                                    foundType = true;
                                                    noteableEventInEvent[noOfNoteableEvents].eventType = eventCatagories[i];
                                                }
                                            }
                                            if (!(foundType))
                                            {
                                                addEventCategory(dataInfo, "Unknown", "Undefined");
                                            }
                                        }
                                        else
                                        {
                                            // Only time to handle
                                            string sSomeTime = dataInfo;
                                            if ((sSomeTime.ToLower() != "undefined") && (sSomeTime.ToLower() != "unknown"))
                                            {
                                                dp2 = sSomeTime.IndexOf(":");
                                                if ((dp2 > 0) && (dp2 < sSomeTime.Length))
                                                {
                                                    noteableEventInEvent[noOfNoteableEvents].largeTime = int.Parse(sSomeTime.Substring(0, dp2));
                                                    sSomeTime = sSomeTime.Substring(dp2 + 1, sSomeTime.Length - dp2 - 1);
                                                    dp3 = sSomeTime.IndexOf(".");
                                                    if ((dp3 > 0) && (dp3 < sSomeTime.Length))
                                                    {
                                                        noteableEventInEvent[noOfNoteableEvents].midTime = int.Parse(sSomeTime.Substring(0, dp3));
                                                        noteableEventInEvent[noOfNoteableEvents].smallTime = int.Parse(sSomeTime.Substring(dp3 + 1, sSomeTime.Length - dp3 - 1));
                                                    }
                                                    else
                                                    {
                                                        noteableEventInEvent[noOfNoteableEvents].smallTime = int.Parse(sSomeTime);
                                                    }
                                                }
                                                else
                                                {
                                                    if (sSomeTime.Length > 4)
                                                    {
                                                        // Numbers format: XXXxx|XXXXxx
                                                        noteableEventInEvent[noOfNoteableEvents].smallTime = int.Parse(sSomeTime.Substring(sSomeTime.Length - 2, 2));
                                                        noteableEventInEvent[noOfNoteableEvents].midTime = int.Parse(sSomeTime.Substring(sSomeTime.Length - 4, 2));
                                                        noteableEventInEvent[noOfNoteableEvents].largeTime = int.Parse(sSomeTime.Substring(0, sSomeTime.Length - 4));
                                                    }
                                                    else if (sSomeTime.Length > 2)
                                                    {
                                                        // Numbers format: Xxx|XXxx
                                                        noteableEventInEvent[noOfNoteableEvents].smallTime = int.Parse(sSomeTime.Substring(sSomeTime.Length - 2, 2));
                                                        noteableEventInEvent[noOfNoteableEvents].largeTime = int.Parse(sSomeTime.Substring(0, sSomeTime.Length - 1));
                                                    }
                                                    else
                                                    {
                                                        // Numbers format: xx|x
                                                        noteableEventInEvent[noOfNoteableEvents].smallTime = int.Parse(sSomeTime);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        // Only date to handle
                                        string sPossDate = dataInfo;
                                        if ((sPossDate.ToLower() != "undefined") && (sPossDate.ToLower() != "unknown"))
                                        {
                                            int iTV01 = 0;
                                            int iTV02 = 0;
                                            int iTV03 = 0;
                                            dp1 = sPossDate.IndexOf("-");
                                            if ((dp1 > 0) && (dp1 < sPossDate.Length))
                                            {
                                                // Have "-" delimeter, format year-month-day or day-month-year
                                                iTV01 = int.Parse(sPossDate.Substring(0, dp1));
                                                if (iTV01 < 100)
                                                    iTV01 = iTV01 + 1900;
                                                sPossDate = sPossDate.Substring(dp1 + 1, sPossDate.Length - dp1 - 1);
                                                dp1 = sPossDate.IndexOf("-");
                                                if ((dp1 > 0) && (dp1 < sPossDate.Length))
                                                {
                                                    // have part 2 and 3
                                                    iTV02 = int.Parse(sPossDate.Substring(0, dp1));
                                                    iTV03 = int.Parse(sPossDate.Substring(dp1 + 1, sPossDate.Length - dp1 - 1));
                                                }
                                                else
                                                {
                                                    iTV02 = 0;
                                                    iTV03 = 0;
                                                }
                                            }
                                            else
                                            {
                                                // Have no delimeter YYYYMMDD(8)|YYMMDD(6)|YYYY(4)
                                                if (sPossDate.Length > 7)
                                                {
                                                    iTV01 = int.Parse(sPossDate.Substring(0, 4));
                                                    iTV02 = int.Parse(sPossDate.Substring(4, 2));
                                                    iTV03 = int.Parse(sPossDate.Substring(6, 2));
                                                }
                                                else if (sPossDate.Length > 5)
                                                {
                                                    iTV01 = int.Parse(sPossDate.Substring(0, 2)) + 1900;
                                                    iTV02 = int.Parse(sPossDate.Substring(2, 2));
                                                    iTV03 = int.Parse(sPossDate.Substring(4, 2));
                                                }
                                                else if (sPossDate.Length > 3)
                                                {
                                                    iTV01 = int.Parse(sPossDate);
                                                }
                                                else
                                                {
                                                    iTV01 = int.Parse(sPossDate) + 1900;
                                                }
                                            }
                                            noteableEventInEvent[noOfNoteableEvents].year = iTV01;
                                            noteableEventInEvent[noOfNoteableEvents].month = iTV02;
                                            noteableEventInEvent[noOfNoteableEvents].day = iTV03;
                                        }
                                    }
                                }
                                break;
                            case "Language":
                                {
                                    // Format : Language : <original language>; [[,]<dubbing lanuage(s)>]; [[,]<text languages>]
                                    dp0 = dataInfo.IndexOf(";");
                                    if ((dp0 > 0) && (dp0 < dataInfo.Length))
                                    {
                                        // Format : <original language>; [[,]<dubbing lanuage(s)>]; [[,]<text languages>]
                                        string oriLan = dataInfo.Substring(0, dp0);
                                        if ((oriLan.ToLower() != "unknown") && (oriLan.ToLower() != "undefined"))
                                        {
                                            thisEventUsedLanguage[noOfUsedLanguages].name = oriLan;
                                            thisEventUsedLanguage[noOfUsedLanguages].type = usedLangType.Spoken;
                                            noOfUsedLanguages++;
                                        }
                                        dataInfo = dataInfo.Substring(dp0 + 2, dataInfo.Length - dp0 - 2);
                                        // Format : [[,]<dubbing lanuage(s)>]; [[,]<text languages>]
                                        dp1 = dataInfo.IndexOf(";");
                                        if ((dp1 > 0) && (dp1 < dataInfo.Length))
                                        {
                                            string dubLanBatch = dataInfo.Substring(0, dp1);
                                            dp2 = dubLanBatch.IndexOf(",");
                                            while ((dp2 > 0) && (dp2 < dataInfo.Length))
                                            {
                                                thisEventUsedLanguage[noOfUsedLanguages].name = dubLanBatch.Substring(0, dp2);
                                                thisEventUsedLanguage[noOfUsedLanguages].type = usedLangType.Dubbed;
                                                noOfUsedLanguages++;
                                                dubLanBatch = dubLanBatch.Substring(dp2 + 2, dubLanBatch.Length - dp2 - 2);
                                                dp2 = dubLanBatch.IndexOf(",");
                                            }
                                            thisEventUsedLanguage[noOfUsedLanguages].name = dubLanBatch.Substring(0, dp2);
                                            thisEventUsedLanguage[noOfUsedLanguages].type = usedLangType.Dubbed;
                                            noOfUsedLanguages++;
                                        }
                                        dataInfo = dataInfo.Substring(dp1 + 2, dataInfo.Length - dp1 - 2);
                                        // Format : [[, ]<text languages>]
                                        dp2 = dataInfo.IndexOf(",");
                                        while ((dp2 > 0) && (dp2 < dataInfo.Length))
                                        {
                                            thisEventUsedLanguage[noOfUsedLanguages].name = dataInfo.Substring(0, dp2);
                                            thisEventUsedLanguage[noOfUsedLanguages].type = usedLangType.Subtext;
                                            noOfUsedLanguages++;
                                            dataInfo = dataInfo.Substring(dp2 + 2, dataInfo.Length - dp2 - 2);
                                            dp2 = dataInfo.IndexOf(",");
                                        }
                                        thisEventUsedLanguage[noOfUsedLanguages].name = dataInfo.Substring(0, dp2);
                                        thisEventUsedLanguage[noOfUsedLanguages].type = usedLangType.Subtext;
                                        noOfUsedLanguages++;
                                    }
                                }
                                break;
                            case "Started ":
                                {
                                    // Started  : <year>-<month>-<day>; <hour>:<minute>; [time|duration]
                                    dp0 = dataInfo.IndexOf(";");
                                    if ((dp0 > 0) && (dp0 < dataInfo.Length))
                                    {
                                        // <year>-<month>-<day>; <hour>:<minute>; [time|duration]
                                        string strtDate = dataInfo.Substring(0, dp0);
                                        if ((strtDate != "Undefined") && (strtDate != "Unknown"))
                                        {
                                            dp1 = strtDate.IndexOf("-");
                                            if ((dp1 > 0) && (dp1 < strtDate.Length - 2))
                                            {
                                                // We have "-" delimiters.
                                                if (strtDate.Length > 9)
                                                {
                                                    // Format YYYY-MM-DD
                                                    string yrstr = strtDate.Substring(0, 4);
                                                    string mtstr = strtDate.Substring(5, 2);
                                                    string dystr = strtDate.Substring(8, 2);
                                                    this.eventStarted.year = int.Parse(yrstr);
                                                    this.eventStarted.month = int.Parse(mtstr);
                                                    this.eventStarted.day = int.Parse(dystr);
                                                }
                                                else if (strtDate.Length > 7)
                                                {
                                                    // Format YY-MM-DD
                                                    string yrstr = strtDate.Substring(0, 2);
                                                    string mtstr = strtDate.Substring(3, 2);
                                                    string dystr = strtDate.Substring(6, 2);
                                                    this.eventStarted.year = int.Parse(yrstr) + 1900;
                                                    this.eventStarted.month = int.Parse(mtstr);
                                                    this.eventStarted.day = int.Parse(dystr);
                                                }
                                                else if (strtDate.Length > 6)
                                                {
                                                    // Format YYYY-MM
                                                    string yrstr = strtDate.Substring(0, 4);
                                                    string mtstr = strtDate.Substring(5, 2);
                                                    this.eventStarted.year = int.Parse(yrstr);
                                                    this.eventStarted.month = int.Parse(mtstr);
                                                }
                                                else if (strtDate.Length > 4)
                                                {
                                                    // Format YY-MM
                                                    string yrstr = strtDate.Substring(0, 2);
                                                    string mtstr = strtDate.Substring(3, 2);
                                                    this.eventStarted.year = int.Parse(yrstr) + 1900;
                                                    this.eventStarted.month = int.Parse(mtstr);
                                                }
                                            }
                                            else
                                            {
                                                // We don't have delimiters.
                                                if (strtDate.Length > 7)
                                                {
                                                    // Format YYYYMMDD
                                                    string yrstr = strtDate.Substring(0, 4);
                                                    string mtstr = strtDate.Substring(4, 2);
                                                    string dystr = strtDate.Substring(6, 2);
                                                    this.eventStarted.year = int.Parse(yrstr);
                                                    this.eventStarted.month = int.Parse(mtstr);
                                                    this.eventStarted.day = int.Parse(dystr);
                                                }
                                                else if (strtDate.Length > 5)
                                                {
                                                    // Format YYMMDD
                                                    string yrstr = strtDate.Substring(0, 2);
                                                    string mtstr = strtDate.Substring(2, 2);
                                                    string dystr = strtDate.Substring(4, 2);
                                                    this.eventStarted.year = int.Parse(yrstr) + 1900;
                                                    this.eventStarted.month = int.Parse(mtstr);
                                                    this.eventStarted.day = int.Parse(dystr);
                                                }
                                                else
                                                {
                                                    // Format YYYY
                                                    string yrstr = strtDate.Substring(0, 4);
                                                    this.eventStarted.year = int.Parse(yrstr);
                                                }
                                            }
                                        }
                                        dataInfo = dataInfo.Substring(dp0 + 2, dataInfo.Length - dp0 - 2);
                                        dp1 = dataInfo.IndexOf(";");
                                        if ((dp1 > 0) && (dp1 < dataInfo.Length - 4))
                                        {
                                            // <hour>:<minute>; [time|hours|minutes|seconds]
                                            string strValue = dataInfo.Substring(0, dp1);
                                            dp2 = strValue.IndexOf(":");
                                            if ((dp2 > 0) && (dp2 < strValue.Length))
                                            {
                                                string strMajor = strValue.Substring(0, dp2);
                                                string strMinor = strValue.Substring(dp2 + 1, strValue.Length - dp2 - 1);
                                                this.eventStarted.duration = (float)(int.Parse(strMajor) + (int.Parse(strMinor) / 100));
                                            }
                                            else if ((strValue.ToLower(CultureInfo.InvariantCulture) != "unknown") && (strValue.ToLower(CultureInfo.InvariantCulture) != "undefined"))
                                                this.eventStarted.duration = (float)(int.Parse(strValue));
                                            string strType = dataInfo.Substring(dp1 + 2, dataInfo.Length - dp1 - 2);
                                            if (!(checkEnumeration(strType, 0)))
                                                this.eventStarted.durtyp = durationTypeDef.Undefined;
                                        }
                                        else
                                        {
                                            // <hour>:<minute>                Time assumed
                                            dp2 = dataInfo.IndexOf(":");
                                            if ((dp2 > 0) && (dp2 < dataInfo.Length))
                                            {
                                                string strMajor = dataInfo.Substring(0, dp2);
                                                string strMinor = dataInfo.Substring(dp2 + 1, dataInfo.Length - dp2 - 1);
                                                this.eventStarted.duration = (float)(int.Parse(strMajor) + (int.Parse(strMinor) / 100));
                                            }
                                            else
                                                this.eventStarted.duration = (float)(int.Parse(dataInfo));
                                            this.eventStarted.durtyp = durationTypeDef.Time;
                                        }
                                    }
                                }
                                break;
                            case "StopYear":
                                {
                                    if ((dataInfo != "") && (dataInfo != "0"))
                                    {
                                        if (int.TryParse(dataInfo, out this.eventEnded.year))
                                            needsSaving = true;
                                    }
                                }
                                break;
                            case "StopMnth":
                                {
                                    if ((dataInfo != "") && (dataInfo != "0"))
                                    {
                                        if (int.TryParse(dataInfo, out this.eventStarted.month))
                                            needsSaving = true;
                                    }
                                }
                                break;
                            case "StopHour":
                                {
                                    if ((dataInfo != "") && (dataInfo != "0"))
                                    {
                                        string tmpvar = this.eventEnded.duration.ToString();
                                        int dp = tmpvar.IndexOf(".");
                                        if ((dp > 0) && (dp < tmpvar.Length))
                                        {
                                            tmpvar = dataInfo + "." + tmpvar.Substring(dp + 1, tmpvar.Length - dp - 1);
                                            this.eventStarted.duration = float.Parse(tmpvar);
                                            needsSaving = true;
                                        }
                                    }
                                }
                                break;
                            case "StopMin ":
                                {
                                    if ((dataInfo != "") && (dataInfo != "0"))
                                    {
                                        string tmpvar = this.eventEnded.duration.ToString();
                                        int dp = tmpvar.IndexOf(".");
                                        if ((dp > 0) && (dp < tmpvar.Length))
                                        {
                                            tmpvar = tmpvar.Substring(0, dp) + "." + dataInfo;
                                            this.eventEnded.duration = float.Parse(tmpvar);
                                            needsSaving = true;
                                        }
                                    }
                                }
                                break;
                            case "Ended   ":
                                {
                                    // Ended    : <year>-<month>-<day>; <hour>:<minute>
                                    dp0 = dataInfo.IndexOf(";");
                                    if ((dp0 > 0) && (dp0 < dataInfo.Length))
                                    {
                                        // <year>-<month>-<day>; <hour>:<minute>; [time|duration]
                                        string strtDate = dataInfo.Substring(0, dp0);
                                        if ((strtDate != "Undefined") && (strtDate != "Unknown"))
                                        {
                                            dp1 = strtDate.IndexOf("-");
                                            if ((dp1 > 0) && (dp1 < strtDate.Length - 2))
                                            {
                                                // We have "-" delimiters.
                                                if (strtDate.Length > 9)
                                                {
                                                    // Format YYYY-MM-DD
                                                    string yrstr = strtDate.Substring(0, 4);
                                                    string mtstr = strtDate.Substring(5, 2);
                                                    string dystr = strtDate.Substring(8, 2);
                                                    this.eventEnded.year = int.Parse(yrstr);
                                                    this.eventEnded.month = int.Parse(mtstr);
                                                    this.eventEnded.day = int.Parse(dystr);
                                                }
                                                else if (strtDate.Length > 7)
                                                {
                                                    // Format YY-MM-DD
                                                    string yrstr = strtDate.Substring(0, 2);
                                                    string mtstr = strtDate.Substring(3, 2);
                                                    string dystr = strtDate.Substring(6, 2);
                                                    this.eventEnded.year = int.Parse(yrstr) + 1900;
                                                    this.eventEnded.month = int.Parse(mtstr);
                                                    this.eventEnded.day = int.Parse(dystr);
                                                }
                                                else if (strtDate.Length > 6)
                                                {
                                                    // Format YYYY-MM
                                                    string yrstr = strtDate.Substring(0, 4);
                                                    string mtstr = strtDate.Substring(5, 2);
                                                    this.eventEnded.year = int.Parse(yrstr);
                                                    this.eventEnded.month = int.Parse(mtstr);
                                                }
                                                else if (strtDate.Length > 4)
                                                {
                                                    // Format YY-MM
                                                    string yrstr = strtDate.Substring(0, 2);
                                                    string mtstr = strtDate.Substring(3, 2);
                                                    this.eventEnded.year = int.Parse(yrstr) + 1900;
                                                    this.eventEnded.month = int.Parse(mtstr);
                                                }
                                            }
                                            else
                                            {
                                                // We don't have delimiters.
                                                if (strtDate.Length > 7)
                                                {
                                                    // Format YYYYMMDD
                                                    string yrstr = strtDate.Substring(0, 4);
                                                    string mtstr = strtDate.Substring(4, 2);
                                                    string dystr = strtDate.Substring(6, 2);
                                                    this.eventEnded.year = int.Parse(yrstr);
                                                    this.eventEnded.month = int.Parse(mtstr);
                                                    this.eventEnded.day = int.Parse(dystr);
                                                }
                                                else if (strtDate.Length > 5)
                                                {
                                                    // Format YYMMDD
                                                    string yrstr = strtDate.Substring(0, 2);
                                                    string mtstr = strtDate.Substring(2, 2);
                                                    string dystr = strtDate.Substring(4, 2);
                                                    this.eventEnded.year = int.Parse(yrstr) + 1900;
                                                    this.eventEnded.month = int.Parse(mtstr);
                                                    this.eventEnded.day = int.Parse(dystr);
                                                }
                                                else if (strtDate.Length > 3)
                                                {
                                                    // Format YYYY
                                                    string yrstr = strtDate.Substring(0, 4);
                                                    this.eventEnded.year = int.Parse(yrstr);
                                                }
                                            }
                                        }
                                        dataInfo = dataInfo.Substring(dp0 + 2, dataInfo.Length - dp0 - 2);
                                        dp1 = dataInfo.IndexOf(";");
                                        if ((dp1 > 0) && (dp1 < dataInfo.Length - 4))
                                        {
                                            // <hour>:<minute>; [time|hours|minutes|seconds]
                                            string strValue = dataInfo.Substring(0, dp1);
                                            dp2 = strValue.IndexOf(":");
                                            if ((dp2 > 0) && (dp2 < strValue.Length))
                                            {
                                                string strMajor = strValue.Substring(0, dp2);
                                                string strMinor = strValue.Substring(dp2 + 1, strValue.Length - dp2 - 1);
                                                this.eventEnded.duration = (float)(int.Parse(strMajor) + (int.Parse(strMinor) / 100));
                                            }
                                            else if ((strValue.ToLower() != "unknown") && (strValue.ToLower() != "undefined"))
                                            {
                                                this.eventEnded.duration = (float)(int.Parse(strValue));
                                                string strType = dataInfo.Substring(dp1 + 2, dataInfo.Length - dp1 - 2);
                                                if (!(checkEnumeration(strType, 0)))
                                                    this.eventEnded.durtyp = durationTypeDef.Undefined;
                                            }
                                        }
                                        else
                                        {
                                            // <hour>:<minute>                Time assumed
                                            dp2 = dataInfo.IndexOf(":");
                                            if ((dp2 > 0) && (dp2 < dataInfo.Length))
                                            {
                                                string strMajor = dataInfo.Substring(0, dp2);
                                                string strMinor = dataInfo.Substring(dp2 + 1, dataInfo.Length - dp2 - 1);
                                                this.eventEnded.duration = (float)(int.Parse(strMajor) + (int.Parse(strMinor) / 100));
                                            }
                                            else
                                                this.eventEnded.duration = (float)(int.Parse(dataInfo));
                                            this.eventEnded.durtyp = durationTypeDef.Time;
                                        }
                                    }
                                }
                                break;
                            case "LatGrad ":
                                {
                                    int ilag = int.Parse(dataInfo);
                                    if ((ilag >= -90) && (ilag <= 90))
                                    {
                                        this.geoPos.latDeg = ilag;
                                        needsSaving = true;
                                    }
                                }
                                break;
                            case "LatMin  ":
                                {
                                    int ilam = int.Parse(dataInfo);
                                    if ((ilam >= 0) && (ilam < 100))
                                    {
                                        this.geoPos.latMin = ilam;
                                        needsSaving = true;
                                    }
                                }
                                break;
                            case "LatSec  ":
                                {
                                    int ilas = int.Parse(dataInfo);
                                    if ((ilas >= 0) && (ilas < 100))
                                    {
                                        this.geoPos.latSec = ilas;
                                        needsSaving = true;
                                    }
                                }
                                break;
                            case "LatSemSc":
                                {
                                    int ilass = int.Parse(dataInfo);
                                    if ((ilass >= 0) && (ilass < 100))
                                    {
                                        this.geoPos.latSemiSec = ilass;
                                        needsSaving = true;
                                    }
                                }
                                break;
                            case "LatDir  ":
                                {
                                    if (!(checkEnumeration(dataInfo, 0)))
                                        this.geoPos.latDir = eventGeographicDirection.Undefined;
                                    needsSaving = true;
                                }
                                break;
                            case "Latitude":
                                {
                                    // Latitude : <degrees>; <minutes>; <seconds>.<semiseconds>; [North|South]
                                    dp0 = dataInfo.IndexOf(";");
                                    if ((dp0 > 0) && (dp0 < dataInfo.Length - 2))
                                    {
                                        // <degrees>; <minutes>; <seconds>.<semiseconds>; [North|South]
                                        string latdeg = dataInfo.Substring(0, dp0);
                                        if ((latdeg != "Undefined") && (latdeg != "Unknown") && (latdeg != "undefined") && (latdeg != "unknown"))
                                        {
                                            int ild = 0;
                                            if (int.TryParse(latdeg, out ild))
                                            {
                                                if ((ild <= 90) && (ild >= -90))
                                                    this.geoPos.latDeg = ild;
                                                else
                                                    this.geoPos.latDeg = 0;
                                            }
                                        }
                                        dataInfo = dataInfo.Substring(dp0 + 2, dataInfo.Length - dp0 - 2);
                                        // <minutes>; <seconds>.<semiseconds>; [North|South]
                                        dp1 = dataInfo.IndexOf(";");
                                        if ((dp1 > 0) && (dp1 < dataInfo.Length - 2))
                                        {
                                            string slatmin = dataInfo.Substring(0, dp1);
                                            if ((slatmin != "Undefined") && (slatmin != "Unknown") && (slatmin != "undefined") && (slatmin != "unknown"))
                                            {
                                                int ilm = 0;
                                                if (int.TryParse(slatmin, out ilm))
                                                {
                                                    if ((ilm >= 0) && (ilm <= 60))
                                                        this.geoPos.latMin = ilm;
                                                    else
                                                        this.geoPos.latMin = 0;
                                                }
                                            }
                                            dataInfo = dataInfo.Substring(dp1 + 2, dataInfo.Length - dp1 - 2);
                                            // <seconds>.<semiseconds>; [North|South]
                                            dp2 = dataInfo.IndexOf(".");
                                            if ((dp2 > 0) && (dp2 < dataInfo.Length - 2))
                                            {
                                                string slatsec = dataInfo.Substring(0, dp2);
                                                if ((slatsec != "Undefined") && (slatsec != "Unknown") && (slatsec != "undefined") && (slatsec != "unknown"))
                                                {
                                                    int ilatsc = 0;
                                                    if (int.TryParse(slatsec, out ilatsc))
                                                    {
                                                        if ((ilatsc >= 0) && (ilatsc <= 60))
                                                            this.geoPos.latSec = ilatsc;
                                                        else
                                                            this.geoPos.latSec = 0;
                                                    }
                                                }
                                                dataInfo = dataInfo.Substring(dp2 + 2, dataInfo.Length - dp2 - 2);
                                                // <semiseconds>; [North|South]
                                                dp3 = dataInfo.IndexOf(";");
                                                if ((dp3 > 0) && (dp3 < dataInfo.Length - 2))
                                                {
                                                    string strlatsemisec = dataInfo.Substring(0, dp3);
                                                    if ((strlatsemisec != "Undefined") && (strlatsemisec != "Unknown") && (strlatsemisec != "undefined") && (strlatsemisec != "unknown"))
                                                    {
                                                        int intlatsemisec = 0;
                                                        if (int.TryParse(strlatsemisec, out intlatsemisec))
                                                        {
                                                            if ((intlatsemisec >= 0) && (intlatsemisec <= 100))
                                                                this.geoPos.latSemiSec = intlatsemisec;
                                                            else
                                                                this.geoPos.latSemiSec = 0;
                                                        }
                                                    }
                                                    dataInfo = dataInfo.Substring(dp3 + 2, dataInfo.Length - dp3 - 2);
                                                    // [North|South]
                                                    if (!(checkEnumeration(dataInfo, 0)))
                                                        this.geoPos.latDir = eventGeographicDirection.Undefined;
                                                }
                                            }
                                        }

                                    }
                                }
                                break;
                            case "LonGrad ":
                                {
                                    int ilog = int.Parse(dataInfo);
                                    if ((ilog >= 0) && (ilog <= 180))
                                    {
                                        this.geoPos.lonDeg = ilog;
                                        needsSaving = true;
                                    }
                                }
                                break;
                            case "LonMin  ":
                                {
                                    int ilom = int.Parse(dataInfo);
                                    if ((ilom >= 0) && (ilom < 100))
                                    {
                                        this.geoPos.lonMin = ilom;
                                        needsSaving = true;
                                    }
                                }
                                break;
                            case "LonSec  ":
                                {
                                    int ilos = int.Parse(dataInfo);
                                    if ((ilos >= 0) && (ilos < 100))
                                    {
                                        this.geoPos.lonSec = ilos;
                                        needsSaving = true;
                                    }
                                }
                                break;
                            case "LonSemSc":
                                {
                                    int iloss = int.Parse(dataInfo);
                                    if ((iloss >= 0) && (iloss < 100))
                                    {
                                        this.geoPos.lonSemiSec = iloss;
                                        needsSaving = true;
                                    }
                                }
                                break;
                            case "LonDir  ":
                                {
                                    if (!(checkEnumeration(dataInfo, 0)))
                                        this.geoPos.lonDir = eventGeographicDirection.Undefined;
                                    needsSaving = true;
                                }
                                break;
                            case "Longitud":
                                {
                                    // Longitud : <degrees>; <minutes>; <seconds>.<semiseconds>; [East|West]
                                    dp0 = dataInfo.IndexOf(";");
                                    if ((dp0 > 0) && (dp0 < dataInfo.Length - 2))
                                    {
                                        // <degrees>; <minutes>; <seconds>.<semiseconds>; [North|South]
                                        string strlondeg = dataInfo.Substring(0, dp0);
                                        if ((strlondeg != "Undefined") && (strlondeg != "Unknown") && (strlondeg != "undefined") && (strlondeg != "unknown"))
                                        {
                                            int intlondeg = 0;
                                            if (int.TryParse(strlondeg, out intlondeg))
                                            {
                                                if ((intlondeg <= 180) && (intlondeg >= -180))
                                                    this.geoPos.lonDeg = intlondeg;
                                                else
                                                    this.geoPos.lonDeg = 0;
                                            }
                                        }
                                        dataInfo = dataInfo.Substring(dp0 + 2, dataInfo.Length - dp0 - 2);
                                        // <minutes>; <seconds>.<semiseconds>; [North|South]
                                        dp1 = dataInfo.IndexOf(";");
                                        if ((dp1 > 0) && (dp1 < dataInfo.Length - 2))
                                        {
                                            string strlonmin = dataInfo.Substring(0, dp1);
                                            if ((strlonmin != "Undefined") && (strlonmin != "Unknown") && (strlonmin != "undefined") && (strlonmin != "unknown"))
                                            {
                                                int intlonmin = 0;
                                                if (int.TryParse(strlonmin, out intlonmin))
                                                {
                                                    if ((intlonmin >= 0) && (intlonmin <= 60))
                                                        this.geoPos.lonMin = intlonmin;
                                                    else
                                                        this.geoPos.lonMin = 0;
                                                }
                                            }
                                            dataInfo = dataInfo.Substring(dp1 + 2, dataInfo.Length - dp1 - 2);
                                            // <seconds>.<semiseconds>; [North|South]
                                            dp2 = dataInfo.IndexOf(".");
                                            if ((dp2 > 0) && (dp2 < dataInfo.Length - 2))
                                            {
                                                string strlonsec = dataInfo.Substring(0, dp2);
                                                if ((strlonsec != "Undefined") && (strlonsec != "Unknown") && (strlonsec != "undefined") && (strlonsec != "unknown"))
                                                {
                                                    int intlonsec = 0;
                                                    if (int.TryParse(strlonsec, out intlonsec))
                                                    {
                                                        if ((intlonsec >= 0) && (intlonsec <= 60))
                                                            this.geoPos.lonSec = intlonsec;
                                                        else
                                                            this.geoPos.lonSec = 0;
                                                    }
                                                }
                                                dataInfo = dataInfo.Substring(dp2 + 2, dataInfo.Length - dp2 - 2);
                                                // <semiseconds>; [North|South]
                                                dp3 = dataInfo.IndexOf(";");
                                                if ((dp3 > 0) && (dp3 < dataInfo.Length - 2))
                                                {
                                                    string strlonsemisec = dataInfo.Substring(0, dp3);
                                                    if ((strlonsemisec != "Undefined") && (strlonsemisec != "Unknown") && (strlonsemisec != "undefined") && (strlonsemisec != "unknown"))
                                                    {
                                                        int intlonsemisec = 0;
                                                        if (int.TryParse(strlonsemisec, out intlonsemisec))
                                                        {
                                                            if ((intlonsemisec >= 0) && (intlonsemisec <= 100))
                                                                this.geoPos.lonSemiSec = intlonsemisec;
                                                            else
                                                                this.geoPos.lonSemiSec = 0;
                                                        }
                                                    }
                                                    dataInfo = dataInfo.Substring(dp3 + 2, dataInfo.Length - dp3 - 2);
                                                    // [North|South]
                                                    if (!(checkEnumeration(dataInfo, 0)))
                                                        this.geoPos.lonDir = eventGeographicDirection.Undefined;
                                                }
                                            }
                                        }

                                    }
                                }
                                break;
                            case "GeoName ":
                            case "GeoNames":
                                {
                                    // GeoName  : <geographical name> 
                                    this.geoPos.geographicName = dataInfo;
                                }
                                break;
                            case "EvtStrt ":
                                {
                                    // EvtStrt  : <streetname>; <number>[; <addon>]
                                    dp0 = dataInfo.IndexOf(";");
                                    if ((dp0 > 0) && (dp0 < dataInfo.Length - 2))
                                    {
                                        string strnme = dataInfo.Substring(0, dp0);
                                        if ((strnme != "Undefined") && (strnme != "Unknown") && (strnme != "undefined") && (strnme != "unknown"))
                                            this.theEventAddress.streetname = strnme;
                                        dataInfo = dataInfo.Substring(dp0 + 2, dataInfo.Length - dp0 - 2);
                                        // <number>[; <addon>]
                                        dp1 = dataInfo.IndexOf(";");
                                        if ((dp1 > 0) && (dp1 < dataInfo.Length - 1))
                                        {
                                            // We have both number and addon
                                            string strnum = dataInfo.Substring(0, dp1);
                                            if ((strnum != "Undefined") && (strnum != "Unknown") && (strnum != "undefined") && (strnum != "unknown"))
                                                this.theEventAddress.housenumber = int.Parse(strnum);
                                            dataInfo = dataInfo.Substring(dp1 + 2, dataInfo.Length - dp1 - 2);
                                            if ((dataInfo != "Undefined") && (dataInfo != "Unknown") && (dataInfo != "undefined") && (dataInfo != "unknown"))
                                                this.theEventAddress.housenumberaddon = dataInfo;
                                        }
                                        else
                                        {
                                            // We only have number
                                            if ((dataInfo != "Undefined") && (dataInfo != "Unknown") && (dataInfo != "undefined") && (dataInfo != "unknown"))
                                                this.theEventAddress.housenumber = int.Parse(dataInfo);
                                        }
                                    }
                                    else if (dataInfo.Length > 0)
                                    {
                                        if ((dataInfo != "Undefined") && (dataInfo != "Unknown") && (dataInfo != "undefined") && (dataInfo != "unknown"))
                                            this.theEventAddress.streetname = dataInfo;
                                    }
                                }
                                break;
                            case "EvtCity ":
                                {
                                    // EvtCity  : <cityname>
                                    if ((dataInfo != "Undefined") && (dataInfo != "Unknown") && (dataInfo != "undefined") && (dataInfo != "unknown"))
                                        this.theEventAddress.cityname = dataInfo;
                                }
                                break;
                            case "EvtAreaN":
                            case "evtAreaN":
                                {
                                    // EvtAreaN : <Areaname>
                                    if ((dataInfo != "Undefined") && (dataInfo != "Unknown") && (dataInfo != "undefined") && (dataInfo != "unknown"))
                                        this.theEventAddress.areaname = dataInfo;
                                }
                                break;
                            case "EvtAreaC":
                                {
                                    // EvtAreaC : <Areacode>
                                    if ((dataInfo != "Undefined") && (dataInfo != "Unknown") && (dataInfo != "undefined") && (dataInfo != "unknown"))
                                        this.theEventAddress.areacode = dataInfo;
                                }
                                break;
                            case "EvtCntry":
                                {
                                    // EvtCntry : <Countryname>
                                    if ((dataInfo != "Undefined") && (dataInfo != "Unknown") && (dataInfo != "undefined") && (dataInfo != "unknown"))
                                        this.theEventAddress.countryname = dataInfo;
                                }
                                break;
                            case "EvtType ":
                                {
                                    // EvtType  : <type of event>
                                    if ((dataInfo != "Undefined") && (dataInfo != "Unknown") && (dataInfo != "undefined") && (dataInfo != "unknown"))
                                    {
                                        bool foundCategory = false;
                                        for (int i = 0; i < noOfEventCategories; i++)
                                        {
                                            if (dataInfo == eventCatagories[i].tag)
                                            {
                                                foundCategory = true;
                                                this.typeOfEvent = eventCatagories[i];
                                            }
                                        }
                                        if (!(foundCategory))
                                        {
                                            if (addEventCategory(dataInfo, "No description", "Undefined"))
                                                this.typeOfEvent.tag = dataInfo;
                                        }
                                    }
                                }
                                break;
                            case "Headline":
                                {
                                    // Headline : <Descriptive event headline>[; <identity no>]
                                    dp0 = dataInfo.IndexOf(";");
                                    if ((dp0 > 0) && (dp0 < dataInfo.Length))
                                    {
                                        string sHdLne = dataInfo.Substring(0, dp0);
                                        if ((sHdLne != "Undefined") && (sHdLne != "Unknown") && (sHdLne != "undefined") && (sHdLne != "unknown"))
                                            this.headlineDescription = sHdLne;
                                        string sTagNo = dataInfo.Substring(dp0 + 2, dataInfo.Length - dp0 - 2);
                                        if ((sTagNo != "Undefined") && (sTagNo != "Unknown") && (sTagNo != "undefined") && (sTagNo != "unknown"))
                                            this.objectTag = sTagNo;
                                    }
                                    else
                                    {
                                        if ((dataInfo != "Undefined") && (dataInfo != "Unknown") && (dataInfo != "undefined") && (dataInfo != "unknown"))
                                            this.headlineDescription = dataInfo;
                                    }
                                }
                                break;
                            case "EvtAttdr":
                                {
                                    // {EvtAttdr : <Attender Id>; <level>; <role>}
                                    if (noOfEventAttender < maxNoOfEventAttender)
                                    {
                                        char delimsign;
                                        if (dataInfo.Contains(","))
                                            delimsign = ',';
                                        else
                                            delimsign = ';';

                                        dp0 = dataInfo.IndexOf(delimsign);
                                        if ((dp0 > 0) && (dp0 < dataInfo.Length - 2))
                                        {
                                            // <Attender Id>; <level>; <role>
                                            string struid = dataInfo.Substring(0, dp0);
                                            if ((struid != "Undefined") && (struid != "Unknown") && (struid != "undefined") && (struid != "unknown"))
                                                this.thisEventAttenders[noOfEventAttender].eventAttenderId = struid;
                                            dataInfo = dataInfo.Substring(dp0 + 2, dataInfo.Length - dp0 - 2);
                                            if (dataInfo.Contains(","))
                                                delimsign = ',';
                                            else
                                                delimsign = ';';
                                            dp1 = dataInfo.IndexOf(delimsign);
                                            if ((dp1 > 0) && (dp1 < dataInfo.Length - 2))
                                            {
                                                // <level>; <role>
                                                string strlvl = dataInfo.Substring(0, dp1);
                                                if ((strlvl == "Unclassified") || (strlvl == "unclassified"))
                                                    this.thisEventAttenders[noOfEventAttender].eventAttenderLevel = eventClassificationType.Unclassified;
                                                else if ((strlvl == "Limited") || (strlvl == "limited"))
                                                    this.thisEventAttenders[noOfEventAttender].eventAttenderLevel = eventClassificationType.Limited;
                                                else if ((strlvl == "Confidential") || (strlvl == "confidential"))
                                                    this.thisEventAttenders[noOfEventAttender].eventAttenderLevel = eventClassificationType.Confidential;
                                                else if ((strlvl == "Secret") || (strlvl == "secret"))
                                                    this.thisEventAttenders[noOfEventAttender].eventAttenderLevel = eventClassificationType.Secret;
                                                else if ((strlvl == "QualifSecret") || (strlvl == "qualif") || (strlvl == "Qualif") || (strlvl == "qualifsecret"))
                                                    this.thisEventAttenders[noOfEventAttender].eventAttenderLevel = eventClassificationType.QualifSecret;
                                                else
                                                    this.thisEventAttenders[noOfEventAttender].eventAttenderLevel = eventClassificationType.Undefined;
                                                dataInfo = dataInfo.Substring(dp1 + 2, dataInfo.Length - dp1 - 2);
                                                bool foundRoleCategory = false;
                                                for (int j = 0; j < noOfRoleCategories; j++)
                                                {
                                                    if (dataInfo == roleCategories[j].tag)
                                                    {
                                                        this.thisEventAttenders[noOfEventAttender].eventAttenderRole = roleCategories[j];
                                                        foundRoleCategory = true;
                                                    }
                                                }
                                                if (!(foundRoleCategory))
                                                {
                                                    if (addRoleCategory(dataInfo, "No description", "Undefined"))
                                                        this.thisEventAttenders[noOfEventAttender].eventAttenderRole.tag = dataInfo;
                                                }
                                            }
                                            else
                                            {
                                                // <level>
                                                string strlvl = dataInfo;
                                                if ((strlvl == "Unclassified") || (strlvl == "unclassified"))
                                                    this.thisEventAttenders[noOfEventAttender].eventAttenderLevel = eventClassificationType.Unclassified;
                                                else if ((strlvl == "Limited") || (strlvl == "limited"))
                                                    this.thisEventAttenders[noOfEventAttender].eventAttenderLevel = eventClassificationType.Limited;
                                                else if ((strlvl == "Confidential") || (strlvl == "confidential"))
                                                    this.thisEventAttenders[noOfEventAttender].eventAttenderLevel = eventClassificationType.Confidential;
                                                else if ((strlvl == "Secret") || (strlvl == "secret"))
                                                    this.thisEventAttenders[noOfEventAttender].eventAttenderLevel = eventClassificationType.Secret;
                                                else if ((strlvl == "QualifSecret") || (strlvl == "qualif") || (strlvl == "Qualif") || (strlvl == "qualifsecret"))
                                                    this.thisEventAttenders[noOfEventAttender].eventAttenderLevel = eventClassificationType.QualifSecret;
                                                else
                                                    this.thisEventAttenders[noOfEventAttender].eventAttenderLevel = eventClassificationType.Undefined;
                                            }
                                        }
                                        else
                                        {
                                            // <Attender Id>
                                            this.thisEventAttenders[noOfEventAttender].eventAttenderId = dataInfo;
                                        }
                                        noOfEventAttender++;
                                    }
                                }
                                break;
                            case "EvtImage":
                                {
                                    // {EvtImage : <Image path and name>; <type of content>; <level>}
                                    if (noOfEventImages < maxNoOfEventImages)
                                    {
                                        char delimsign;
                                        if (dataInfo.Contains(","))
                                            delimsign = ',';
                                        else
                                            delimsign = ';';
                                        dp0 = dataInfo.IndexOf(delimsign);
                                        if ((dp0 > 0) && (dp0 < dataInfo.Length - 2))
                                        {
                                            string strimpn = dataInfo.Substring(0, dp0);
                                            if ((strimpn != "Unknown") && (strimpn != "Undefined") && (strimpn != "unknown") && (strimpn != "undefined"))
                                                this.thisEventImages[noOfEventImages].eventImagePathName = strimpn;
                                            dataInfo = dataInfo.Substring(dp0 + 2, dataInfo.Length - dp0 - 2);
                                            if (dataInfo.Contains(","))
                                                delimsign = ',';
                                            else
                                                delimsign = ';';
                                            dp1 = dataInfo.IndexOf(delimsign);
                                            if ((dp1 > 0) && (dp1 < dataInfo.Length - 2))
                                            {
                                                string strtyc = dataInfo.Substring(0, dp1);
                                                bool foundImageContent = false;
                                                for (int i = 0; i < noOfContentCategories; i++)
                                                {
                                                    if (strtyc == contentCategories[i].tag)
                                                    {
                                                        this.thisEventImages[noOfEventImages].eventImageContent = contentCategories[i];
                                                        foundImageContent = true;
                                                    }
                                                }
                                                if (!(foundImageContent))
                                                {
                                                    this.thisEventImages[noOfEventImages].eventImageContent.tag = strtyc;
                                                    addContentCategory(strtyc, "No description", "Undefined");
                                                }
                                                dataInfo = dataInfo.Substring(dp1 + 2, dataInfo.Length - dp1 - 2);
                                                if (!(checkEnumeration(dataInfo, noOfEventImages)))
                                                {
                                                    if (noOfEventImages < maxNoOfEventImages)
                                                        this.thisEventImages[noOfEventImages].eventImageLevel = eventClassificationType.Undefined;
                                                }
                                            }
                                            else
                                            {
                                                string strtyc = dataInfo;
                                                bool foundImageContent = false;
                                                for (int i = 0; i < noOfContentCategories; i++)
                                                {
                                                    if (strtyc == contentCategories[i].tag)
                                                    {
                                                        this.thisEventImages[noOfEventImages].eventImageContent = contentCategories[i];
                                                        foundImageContent = true;
                                                    }
                                                }
                                                if (!(foundImageContent))
                                                {
                                                    this.thisEventImages[noOfEventImages].eventImageContent.tag = strtyc;
                                                    addContentCategory(strtyc, "No description", "Undefined");
                                                }
                                                this.thisEventImages[noOfEventImages].eventImageLevel = eventClassificationType.Undefined;
                                            }
                                        }
                                        noOfEventImages++;
                                    }
                                }
                                break;
                            case "Content ":
                                {
                                    // {Content  : {<Content tags>}}
                                    if (noOfEventContentTags < maxNoOfEventContentTags)
                                        this.thisEventContentTags[noOfEventContentTags++] = dataInfo;
                                }
                                break;
                            case "Root    ":
                                {
                                    if (noOfEventRootDirs < maxNoOfEventRootDirs)
                                        eventRootDirs[noOfEventRootDirs++] = dataInfo;
                                }
                                break;
                            case "Descrptn":
                                {
                                    // Descrptn : <Description of the event>
                                    if (noOfDescriptionLines < maxNoOfDescriptionLines)
                                        this.descriptionOfEvent[noOfDescriptionLines++] = dataInfo;
                                }
                                break;
                            case "Productn":
                                {
                                    char delimsign;
                                    delimsign = ';';
                                    int dp = dataInfo.IndexOf(delimsign);
                                    if ((dp > 0) && (dp < dataInfo.Length - 1))
                                    {
                                        this.eventId = dataInfo.Substring(0, dp);
                                        dataInfo = dataInfo.Substring(dp + 2, dataInfo.Length - dp - 2);
                                        if (dataInfo.Contains(";"))
                                            delimsign = ';';
                                        else
                                            delimsign = ',';
                                        dp = dataInfo.IndexOf(delimsign);
                                        if ((dp > 0) && (dp < dataInfo.Length - 1))
                                        {
                                            string evttpe = dataInfo.Substring(0, dp);
                                            dataInfo = dataInfo.Substring(dp + 2, dataInfo.Length - dp - 2);
                                            if (dataInfo.Contains(";"))
                                                delimsign = ';';
                                            else
                                                delimsign = ',';
                                            dp = dataInfo.IndexOf(delimsign);
                                            if ((dp > 0) && (dp < dataInfo.Length - 1))
                                            {
                                                this.setEventHeadline(dataInfo.Substring(0, dp) + " " + evttpe);
                                                dataInfo = dataInfo.Substring(dp + 2, dataInfo.Length - dp - 2);
                                                if (dataInfo.Contains(";"))
                                                    delimsign = ';';
                                                else
                                                    delimsign = ',';
                                                dp = dataInfo.IndexOf(delimsign);
                                                if ((dp > 0) && (dp < dataInfo.Length - 1))
                                                {
                                                    this.headlineDescription = dataInfo.Substring(0, dp) + ":" + this.headlineDescription;
                                                    dataInfo = dataInfo.Substring(dp + 2, dataInfo.Length - dp - 2);
                                                    if (dataInfo.Contains(";"))
                                                        delimsign = ';';
                                                    else
                                                        delimsign = ',';
                                                    dp = dataInfo.IndexOf(delimsign);
                                                    if ((dp > 0) && (dp < dataInfo.Length - 1))
                                                    {
                                                        string possDate = dataInfo.Substring(0, dp);
                                                        dataInfo = dataInfo.Substring(dp + 2, dataInfo.Length - dp - 2);
                                                        if (dataInfo.Contains(";"))
                                                            delimsign = ';';
                                                        else
                                                            delimsign = ',';
                                                        dp = dataInfo.IndexOf(delimsign);
                                                        if ((dp > 0) && (dp < dataInfo.Length - 1))
                                                        {
                                                            string posstime = dataInfo.Substring(0, dp);
                                                            if (posstime.Contains(" "))
                                                            {
                                                                int dps = posstime.IndexOf(" ");
                                                                posstime = posstime.Substring(dps + 1, posstime.Length - dps - 1);
                                                            }
                                                            this.setEventStarted(possDate + "; " + posstime + " hours");
                                                            dataInfo = dataInfo.Substring(dp + 2, dataInfo.Length - dp - 2);
                                                            if (dataInfo.Contains(";"))
                                                                delimsign = ';';
                                                            else
                                                                delimsign = ',';
                                                            dp = dataInfo.IndexOf(delimsign);
                                                            if ((dp > 0) && (dp < dataInfo.Length - 1))
                                                            {
                                                                dataInfo = dataInfo.Substring(dp + 2, dataInfo.Length - dp - 2);
                                                                if (dataInfo.Contains(","))
                                                                    delimsign = ',';
                                                                else
                                                                    delimsign = ';';
                                                                dp = dataInfo.IndexOf(delimsign);
                                                                if ((dp > 0) && (dp < dataInfo.Length - 1))
                                                                {
                                                                    this.descriptionOfEvent[noOfDescriptionLines++] = dataInfo.Substring(0, dp);
                                                                    dataInfo = dataInfo.Substring(dp + 2, dataInfo.Length - dp - 2);
                                                                    if (dataInfo.Contains(","))
                                                                        delimsign = ',';
                                                                    else
                                                                        delimsign = ';';
                                                                    dp = dataInfo.IndexOf(delimsign);
                                                                    while ((dp > -1) && (dp < dataInfo.Length) && (noOfEventContentTags < maxNoOfEventContentTags))
                                                                    {
                                                                        this.thisEventContentTags[noOfEventContentTags++] = dataInfo.Substring(0, dp);
                                                                        dataInfo = dataInfo.Substring(dp + 2, dataInfo.Length - dp - 2);
                                                                        if (dataInfo.Contains(","))
                                                                            delimsign = ',';
                                                                        else
                                                                            delimsign = ';';
                                                                        dp = dataInfo.IndexOf(delimsign);
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    needsSaving = true;
                                }
                                break;
                            default:
                                {
                                    // Erroneous branch, should not be called.
                                }
                                break;
                        }
                    }
                }
            }
        }
        public bool getNeedsSaving() { return needsSaving; }
        public void setNeedsSaving(bool indata) { needsSaving = indata; }
        public bool saveEvent(string eventID, string storagePath)
        {
            bool retVal = false;
            string filename = "";
            if ((storagePath != "") && (System.IO.Directory.Exists(storagePath)))
            {
                filename = storagePath + "\\EventData_" + eventID + ".edf";
            }
            else
            {
                string curru = WindowsIdentity.GetCurrent().Name;
                int tnr = curru.LastIndexOf("\\");
                if ((tnr > 0) && (tnr < curru.Length - 1))
                    curru = curru.Substring(tnr + 1, curru.Length - tnr - 1);
                string rootPath = "C:\\Users\\" + curru + "\\source\\repos\\PhotoEditor00002\\PhotoEditor00002";
                filename = rootPath + "\\EventData\\EventData_" + eventID + ".edf";
            }
            using (System.IO.FileStream efs = System.IO.File.Create(filename))
            {
                using (System.IO.StreamWriter sw = new System.IO.StreamWriter(efs))
                {
                    var line = "";
                    if (this.eventId != "")
                    {
                        line = "Identity : " + this.eventId;
                        sw.WriteLine(line);
                    }
                    line = "Secrecy  : " + this.classificationLevel.ToString();
                    sw.WriteLine(line);
                    if ((this.eventOwner != "") && (this.eventOwner != null))
                    {
                        line = "EvtOwner : " + this.eventOwner;
                        sw.WriteLine(line);
                    }
                    if (this.eventStarted.year > 0)
                    {
                        line = "Started  : " + this.eventStarted.year.ToString();
                        if (this.eventStarted.month > 0)
                        {
                            if (this.eventStarted.month > 9)
                                line = line + "-" + this.eventStarted.month.ToString();
                            else
                                line = line + "-0" + this.eventStarted.month.ToString();
                            if (this.eventStarted.day > 9)
                                line = line + "-" + this.eventStarted.day.ToString();
                            else if (this.eventStarted.day > 0)
                                line = line + "-0" + this.eventStarted.day.ToString();
                        }
                        if (this.eventStarted.duration > 0)
                        {
                            string strdur = this.eventStarted.duration.ToString();
                            int dp = strdur.IndexOf(".");
                            if ((dp > 0) && (dp < strdur.Length - 1))
                            {
                                string majorPart = strdur.Substring(0, dp);
                                string minorPart = strdur.Substring(dp + 1, strdur.Length - dp - 1);
                                line = line + "; " + majorPart + ":" + minorPart;
                            }
                            else
                                line = line + "; " + strdur + ":00";
                        }
                        else
                        {
                            line = line + "; Unknown";
                        }
                        line = line + "; " + this.eventStarted.durtyp.ToString();
                        sw.WriteLine(line);
                    }
                    if (this.eventEnded.year >= this.eventStarted.year)
                    {
                        line = "Ended    : " + this.eventEnded.year.ToString();
                        if (this.eventEnded.month > 0)
                        {
                            if (this.eventEnded.month > 9)
                                line = line + "-" + this.eventEnded.month.ToString();
                            else
                                line = line + "-0" + this.eventEnded.month.ToString();
                            if (this.eventEnded.day > 9)
                                line = line + "-" + this.eventEnded.day.ToString();
                            else if (this.eventEnded.durtyp > 0)
                                line = line + "-0" + this.eventEnded.day.ToString();
                        }
                        if (this.eventEnded.duration > 0)
                        {
                            string strdur = this.eventEnded.duration.ToString();
                            int dp = strdur.IndexOf(".");
                            if ((dp > 0) && (dp < strdur.Length - 1))
                            {
                                string strMajor = strdur.Substring(0, dp);
                                string strMinor = strdur.Substring(dp + 1, strdur.Length - dp - 1);
                                line = line + "; " + strMajor + ":" + strMinor;
                            }
                            else
                                line = line + "; " + strdur + ":00";
                        }
                        else
                        {
                            line = line + "; Unknown";
                        }
                        line = line + "; " + this.eventEnded.durtyp.ToString();
                        sw.WriteLine(line);
                    }
                    if (this.geoPos.latDeg >= 0)
                    {
                        if ((this.geoPos.latDir != eventGeographicDirection.Undefined) && ((this.geoPos.latDeg > 0) || (this.geoPos.latMin > 0) || (this.geoPos.latSec > 0) || (this.geoPos.latSemiSec > 0)))
                        {
                            line = "Latitude : ";
                            if (this.geoPos.latDeg > 9)
                                line = line + this.geoPos.latDeg.ToString();
                            else
                                line = line + "0" + this.geoPos.latDeg.ToString();
                            if (this.geoPos.latMin > 9)
                                line = line + "; " + this.geoPos.latMin.ToString();
                            else
                                line = line + "; 0" + this.geoPos.latMin.ToString();
                            if (this.geoPos.latSec > 9)
                                line = line + "; " + this.geoPos.latSec.ToString();
                            else
                                line = line + "; 0" + this.geoPos.latSec.ToString();
                            if (this.geoPos.latSemiSec > 9)
                                line = line + "; " + this.geoPos.latSemiSec.ToString();
                            else
                                line = line + "; 0" + this.geoPos.latSemiSec.ToString();
                            if (this.geoPos.latDir == eventGeographicDirection.North)
                                line = line + "; North";
                            else
                                line = line + "; South";
                            sw.WriteLine(line);
                        }
                    }
                    if (this.geoPos.lonDeg >= 0)
                    {
                        if ((this.geoPos.lonDir != eventGeographicDirection.Undefined) && ((this.geoPos.lonDeg > 0) || (this.geoPos.lonMin > 0) || (this.geoPos.lonSec > 0) || (this.geoPos.lonSemiSec > 0)))
                        {
                            line = "Longitud : ";
                            if (this.geoPos.lonDeg > 99)
                                line = line + this.geoPos.lonDeg.ToString();
                            else if (this.geoPos.lonDeg > 9)
                                line = line + "0" + this.geoPos.lonDeg.ToString();
                            else
                                line = line + "00" + this.geoPos.lonDeg.ToString();
                            if (this.geoPos.lonMin > 9)
                                line = line + "; " + this.geoPos.lonMin.ToString();
                            else
                                line = line + "; 0" + this.geoPos.lonMin.ToString();
                            if (this.geoPos.lonSec > 9)
                                line = line + "; " + this.geoPos.lonSec.ToString();
                            else
                                line = line + "; 0" + this.geoPos.lonSec.ToString();
                            if (this.geoPos.lonSemiSec > 9)
                                line = line + "." + this.geoPos.lonSemiSec.ToString();
                            else
                                line = line + ".0" + this.geoPos.lonSemiSec.ToString();
                            if (this.geoPos.lonDir == eventGeographicDirection.East)
                                line = line + "; East";
                            else
                                line = line + "; West";
                            sw.WriteLine(line);
                        }
                    }
                    if (this.geoPos.geographicName != "")
                    {
                        if ((this.geoPos.geographicName != null) && (this.geoPos.geographicName != ""))
                        {
                            line = "GeoName  : " + this.geoPos.geographicName;
                            sw.WriteLine(line);
                        }
                    }
                    if (this.theEventAddress.streetname != "")
                    {
                        if ((this.theEventAddress.streetname != null) && (this.theEventAddress.streetname != ""))
                        {
                            line = "EvtStrt  : " + this.theEventAddress.streetname + "; " + this.theEventAddress.housenumber.ToString() + "; " + this.theEventAddress.housenumberaddon;
                            sw.WriteLine(line);
                        }
                    }
                    if (this.theEventAddress.cityname != "")
                    {
                        if ((this.theEventAddress.cityname != null) && (this.theEventAddress.cityname != ""))
                        {
                            line = "EvtCity  : " + this.theEventAddress.cityname;
                            sw.WriteLine(line);
                        }
                    }
                    if (this.theEventAddress.areaname != "")
                    {
                        if ((this.theEventAddress.areaname != null) && (this.theEventAddress.areaname != ""))
                        {
                            line = "evtAreaN : " + this.theEventAddress.areaname;
                            sw.WriteLine(line);
                        }
                    }
                    if (this.theEventAddress.areacode != "")
                    {
                        if ((this.theEventAddress.areacode != null) && (this.theEventAddress.areacode != ""))
                        {
                            line = "EvtAreaC : " + this.theEventAddress.areacode;
                            sw.WriteLine(line);
                        }
                    }
                    if (this.theEventAddress.countryname != "")
                    {
                        if ((this.theEventAddress.countryname != null) && (this.theEventAddress.countryname != ""))
                        {
                            line = "EvtCntry : " + this.theEventAddress.countryname;
                            sw.WriteLine(line);
                        }
                    }
                    if (this.typeOfEvent.tag != "")
                    {
                        if ((this.typeOfEvent.tag != null) && (this.typeOfEvent.tag != ""))
                        {
                            line = "EvtType  : ";
                            line = line + this.typeOfEvent.tag;
                            if (this.typeOfEvent.description != "")
                            {
                                line = line + "; " + this.typeOfEvent.description;
                                line = line + "; " + this.typeOfEvent.level.ToString();
                            }
                            sw.WriteLine(line);
                        }
                    }
                    if (this.headlineDescription != "")
                    {
                        if ((this.headlineDescription != null) && (this.headlineDescription != ""))
                        {
                            line = "Headline : " + this.headlineDescription;
                            sw.WriteLine(line);
                        }
                    }
                    if (this.noOfEventAttender > 0)
                    {
                        for (int i = 0; i < this.noOfEventAttender; i++)
                        {
                            if ((this.thisEventAttenders[i].eventAttenderId != null) && (this.thisEventAttenders[i].eventAttenderId != ""))
                            {
                                line = "EvtAttdr : " + this.thisEventAttenders[i].eventAttenderId + "; " + this.thisEventAttenders[i].eventAttenderLevel.ToString();
                                if (this.thisEventAttenders[i].eventAttenderRole.tag != "")
                                    line = line + "; " + this.thisEventAttenders[i].eventAttenderRole.tag;
                                sw.WriteLine(line);
                            }
                        }
                    }
                    if (this.noOfEventImages > 0)
                    {
                        for (int i = 0; i < this.noOfEventImages; i++)
                        {
                            if ((this.thisEventImages[i].eventImagePathName != null) && (this.thisEventImages[i].eventImagePathName != ""))
                            {
                                line = "EvtImage : " + this.thisEventImages[i].eventImagePathName;
                                if ((this.thisEventImages[i].eventImageContent.tag != null) && (this.thisEventImages[i].eventImageContent.tag != ""))
                                {
                                    line = line + "; " + this.thisEventImages[i].eventImageContent.tag + "; " + this.thisEventImages[i].eventImageLevel.ToString();
                                }
                                sw.WriteLine(line);
                            }
                        }
                    }
                    if (this.noOfEventContentTags > 0)
                    {
                        for (int i = 0; i < this.noOfEventContentTags; i++)
                        {
                            if ((this.thisEventContentTags[i] != null) && (this.thisEventContentTags[i] != ""))
                            {
                                line = "Content  : " + this.thisEventContentTags[i];
                                sw.WriteLine(line);
                            }
                        }
                    }
                    if (noOfEventRootDirs > 0)
                    {
                        for (int i = 0; i < noOfEventRootDirs; i++)
                        {
                            if ((eventRootDirs[i] != null) && (eventRootDirs[i] != ""))
                            {
                                line = "Root    : " + eventRootDirs[i];
                                sw.WriteLine(line);
                            }
                        }
                    }
                    if (this.noOfDescriptionLines > 0)
                    {
                        for (int i = 0; i < this.noOfDescriptionLines; i++)
                        {
                            if ((this.descriptionOfEvent[i] != null) && (this.descriptionOfEvent[i] != ""))
                            {
                                line = "Descrptn : " + this.descriptionOfEvent[i];
                                sw.WriteLine(line);
                            }
                        }
                    }
                    sw.Close();
                }
                efs.Close();
                needsSaving = false;
            }
            return retVal;
        }
        public void setEventID(string indata) { this.eventId = indata; }
        public string getEventID() { return this.eventId; }
        public string getEventOwner() { return this.eventOwner; }
        public void setEventOwner(string nyOwner) { this.eventOwner = nyOwner; }
        public int getEventLevelValue()
        {
            for (int i = 0; i < getNoOfClassificationStrings(); i++)
            {
                if (classificationLevel.ToString() == getClassificationNo(i))
                    return i;
            }
            return -1;
        }
        public string getEventLevel() { return this.classificationLevel.ToString(); }
        public bool setEventLevel(string nyEventLevel)
        {
            if (!(checkEnumeration(nyEventLevel, 0)))
            {
                this.classificationLevel = eventClassificationType.Undefined;
                return false;
            }
            else
                return true;
        }
        public string getEventStarted()
        {
            // Format : YY[YY][-MM[-DD]] H[H[H]]:mm [hours|minutes|seconds|time]
            string retVal = "";
            if (this.eventStarted.year > 0)
            {
                retVal = retVal + this.eventStarted.year.ToString();
                if (this.eventStarted.month > 9)
                    retVal = retVal + "-" + this.eventStarted.month.ToString();
                else if (this.eventStarted.month > 0)
                    retVal = retVal + "-0" + this.eventStarted.month.ToString();
                else
                    retVal = retVal + "-MM";
                if (this.eventStarted.day > 9)
                    retVal = retVal + "-" + this.eventStarted.day.ToString();
                else if (this.eventStarted.day > 0)
                    retVal = retVal + "-0" + this.eventStarted.day.ToString();
                else
                    retVal = retVal + "-DD";
                if (this.eventStarted.duration > 0.0)
                {
                    //                retVal = retVal + " " + this.eventStarted.duration.ToString();
                    if (this.eventStarted.durtyp == durationTypeDef.Hours)
                        retVal = retVal + " " + this.eventStarted.duration.ToString() + " hours";
                    else if (this.eventStarted.durtyp == durationTypeDef.Minutes)
                        retVal = retVal + " " + this.eventStarted.duration.ToString() + " minutes";
                    else if (this.eventStarted.durtyp == durationTypeDef.Seconds)
                        retVal = retVal + " " + this.eventStarted.duration.ToString() + " seconds";
                    else if (this.eventStarted.durtyp == durationTypeDef.Time)
                        retVal = retVal + " " + Math.Truncate(this.eventStarted.duration) + ":" + Math.IEEERemainder(this.eventStarted.duration, 2);
                }
            }
            return retVal;
        }
        public bool setEventStarted(string indata)
        {
            // Format : YY[YY][-MM[-DD]] H[H[H]]:mm [hours|minutes|seconds|time]
            bool retVal = true;
            int dp = indata.IndexOf("-");
            if ((dp > 0) && (dp < indata.Length - 1))
            {
                if (Int32.TryParse(indata.Substring(0, dp), out this.eventStarted.year))
                {
                    indata = indata.Substring(dp + 1, indata.Length - dp - 1);
                    // Format : MM[-DD] H[H[H]]:mm [hours|minutes|seconds|time]
                    dp = indata.IndexOf("-");
                    if ((dp > 0) && (dp < indata.Length - 1))
                    {
                        if (Int32.TryParse(indata.Substring(0, dp), out this.eventStarted.month))
                        {
                            indata = indata.Substring(dp + 1, indata.Length - dp - 1);
                            // Format : DD H[H[H]]:mm [hours|minutes|seconds|time]
                            dp = indata.IndexOf(";");
                            if ((dp > 0) && (dp < indata.Length - 1))
                            {
                                if (Int32.TryParse(indata.Substring(0, dp), out this.eventStarted.day))
                                {
                                    indata = indata.Substring(dp + 2, indata.Length - dp - 2);
                                    // Format : H[H[H]]:mm [hours|minutes|seconds|time]
                                    dp = indata.IndexOf(" ");
                                    if ((dp > 0) && (dp < indata.Length - 1))
                                    {
                                        string possTime = indata.Substring(0, dp);
                                        // Format : H[H[H]]:mm
                                        int dppt = possTime.IndexOf(":");
                                        if ((dppt > 0) && (dppt < possTime.Length - 1))
                                        {
                                            string stort = possTime.Substring(0, dppt);
                                            string litet = possTime.Substring(dppt + 1, possTime.Length - dppt - 1);
                                            string restid = stort + "," + litet;
                                            if (float.TryParse(restid, out this.eventStarted.duration))
                                            {
                                                indata = indata.Substring(dp + 1, indata.Length - dp - 1);
                                                this.eventStarted.durtyp = durationTypeDef.Undefined;
                                                if (!(checkEnumeration(indata, 0)))
                                                    this.eventStarted.durtyp = durationTypeDef.Undefined;
                                            }
                                            retVal = true;
                                        }
                                        else
                                        {
                                            if (float.TryParse(possTime, out this.eventStarted.duration))
                                            {
                                                indata = indata.Substring(dp + 1, indata.Length - dp - 1);
                                                this.eventStarted.durtyp = durationTypeDef.Undefined;
                                                if (!(checkEnumeration(indata, 0)))
                                                    this.eventStarted.durtyp = durationTypeDef.Undefined;
                                            }
                                            retVal = true;
                                        }
                                    }
                                    else
                                    {
                                        // Format : H[H[H]]:mm
                                        dp = indata.IndexOf(":");
                                        if ((dp > 0) && (dp < indata.Length - 1))
                                        {
                                            int stort, litet;
                                            if (Int32.TryParse(indata.Substring(0, dp), out stort))
                                            {
                                                if (Int32.TryParse(indata.Substring(dp + 1, indata.Length - dp - 1), out litet))
                                                    this.eventStarted.duration = (float)(stort + (litet / 100));
                                                else
                                                    this.eventStarted.duration = (float)(stort);
                                            }
                                            else
                                            {
                                                float.TryParse(indata, out this.eventStarted.duration);
                                            }
                                            retVal = true;
                                        }
                                        else
                                        {
                                            float.TryParse(indata, out this.eventStarted.duration);
                                            retVal = true;
                                        }
                                    }
                                }
                                else
                                {
                                    dp = indata.IndexOf(" ");
                                    indata = indata.Substring(dp + 1, indata.Length - dp - 1);
                                    // Format : H[H[H]]:mm [hours|minutes|seconds|time]
                                    dp = indata.IndexOf(" ");
                                    if ((dp > 0) && (dp < indata.Length - 1))
                                    {
                                        string possTime = indata.Substring(0, dp);
                                        // Format : H[H[H]]:mm
                                        dp = possTime.IndexOf(":");
                                        if ((dp > 0) && (dp < possTime.Length - 1))
                                        {
                                            int stort, litet;
                                            if (Int32.TryParse(possTime.Substring(0, dp), out stort))
                                            {
                                                if (Int32.TryParse(possTime.Substring(dp + 1, possTime.Length - dp - 1), out litet))
                                                    this.eventStarted.duration = (float)(stort + (litet / 100));
                                                else
                                                    this.eventStarted.duration = (float)(stort);
                                                indata = indata.Substring(dp + 1, indata.Length - dp - 1);
                                                this.eventStarted.durtyp = durationTypeDef.Undefined;
                                                if (!(checkEnumeration(indata, 0)))
                                                    this.eventStarted.durtyp = durationTypeDef.Undefined;
                                            }
                                            else
                                            {
                                                if (float.TryParse(possTime, out this.eventStarted.duration))
                                                {
                                                    indata = indata.Substring(dp + 1, indata.Length - dp - 1);
                                                    this.eventStarted.durtyp = durationTypeDef.Undefined;
                                                    if (!(checkEnumeration(indata, 0)))
                                                        this.eventStarted.durtyp = durationTypeDef.Undefined;
                                                }
                                            }
                                            retVal = true;
                                        }
                                        else
                                        {
                                            if (float.TryParse(possTime, out this.eventStarted.duration))
                                            {
                                                indata = indata.Substring(dp + 1, indata.Length - dp - 1);
                                                this.eventStarted.durtyp = durationTypeDef.Undefined;
                                                if (!(checkEnumeration(indata, 0)))
                                                    this.eventStarted.durtyp = durationTypeDef.Undefined;
                                            }
                                            retVal = true;
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            dp = indata.IndexOf(" ");
                            indata = indata.Substring(dp + 1, indata.Length - dp - 1);
                            // Format : H[H[H]]:mm [hours|minutes|seconds|time]
                            dp = indata.IndexOf(" ");
                            if ((dp > 0) && (dp < indata.Length - 1))
                            {
                                string possTime = indata.Substring(0, dp);
                                // Format : H[H[H]]:mm
                                int dppt = possTime.IndexOf(":");
                                if ((dppt > 0) && (dppt < possTime.Length - 1))
                                {
                                    string stort = possTime.Substring(0, dppt);
                                    string litet = possTime.Substring(dppt + 1, possTime.Length - dppt - 1);
                                    string restid = stort + "," + litet;
                                    if (float.TryParse(restid, out this.eventStarted.duration))
                                    {
                                        indata = indata.Substring(dp + 1, indata.Length - dp - 1);
                                        this.eventStarted.durtyp = durationTypeDef.Undefined;
                                        if (!(checkEnumeration(indata, 0)))
                                            this.eventStarted.durtyp = durationTypeDef.Undefined;
                                    }
                                    retVal = true;
                                }
                                else
                                {
                                    if (float.TryParse(possTime, out this.eventStarted.duration))
                                    {
                                        indata = indata.Substring(dp + 1, indata.Length - dp - 1);
                                        this.eventStarted.durtyp = durationTypeDef.Undefined;
                                        if (!(checkEnumeration(indata, 0)))
                                            this.eventStarted.durtyp = durationTypeDef.Undefined;
                                    }
                                    retVal = true;
                                }
                            }
                            else
                            {
                                // Format : H[H[H]]:mm
                                dp = indata.IndexOf(":");
                                if ((dp > 0) && (dp < indata.Length - 1))
                                {
                                    int stort, litet;
                                    if (Int32.TryParse(indata.Substring(0, dp), out stort))
                                    {
                                        if (Int32.TryParse(indata.Substring(dp + 1, indata.Length - dp - 1), out litet))
                                            this.eventStarted.duration = (float)(stort + (litet / 100));
                                        else
                                            this.eventStarted.duration = (float)(stort);
                                    }
                                    else
                                    {
                                        float.TryParse(indata, out this.eventStarted.duration);
                                    }
                                    retVal = true;
                                }
                                else
                                {
                                    float.TryParse(indata, out this.eventStarted.duration);
                                    retVal = true;
                                }
                            }
                        }
                    }
                }
            }
            return retVal;
        }
        public string getEventEnded()
        {
            string retVal = "";
            if (this.eventEnded.year > 0)
            {
                retVal = retVal + this.eventEnded.year.ToString();
                if (this.eventEnded.month > 9)
                    retVal = retVal + "-" + this.eventEnded.month.ToString();
                else if (this.eventEnded.month > 0)
                    retVal = retVal + "-0" + this.eventEnded.month.ToString();
                else
                    retVal = retVal + "-MM";
                if (this.eventEnded.day > 9)
                    retVal = retVal + "-" + this.eventEnded.day.ToString();
                else if (this.eventEnded.day > 0)
                    retVal = retVal + "-0" + this.eventEnded.day.ToString();
                else
                    retVal = retVal + "-DD";
                if (this.eventEnded.duration > 0.0)
                {
                    //                retVal = retVal + " " + this.eventStarted.duration.ToString();
                    if (this.eventEnded.durtyp == durationTypeDef.Hours)
                        retVal = retVal + " " + this.eventEnded.duration.ToString() + " hours";
                    else if (this.eventEnded.durtyp == durationTypeDef.Minutes)
                        retVal = retVal + " " + this.eventEnded.duration.ToString() + " minutes";
                    else if (this.eventEnded.durtyp == durationTypeDef.Seconds)
                        retVal = retVal + " " + this.eventEnded.duration.ToString() + " seconds";
                    else if (this.eventEnded.durtyp == durationTypeDef.Time)
                        retVal = retVal + " " + Math.Truncate(this.eventEnded.duration) + ":" + Math.IEEERemainder(this.eventEnded.duration, 2);
                }
            }
            return retVal;
        }
        public bool setEventEnded(string indata)
        {
            // Format : YY[YY][-MM[-DD]] H[H[H]]:mm [hours|minutes|seconds|time]
            bool retVal = true;
            int dp = indata.IndexOf("-");
            if ((dp > 0) && (dp < indata.Length - 1))
            {
                if (Int32.TryParse(indata.Substring(0, dp), out this.eventEnded.year))
                {
                    indata = indata.Substring(dp + 1, indata.Length - dp - 1);
                    // Format : MM[-DD] H[H[H]]:mm [hours|minutes|seconds|time]
                    dp = indata.IndexOf("-");
                    if ((dp > 0) && (dp < indata.Length - 1))
                    {
                        if (Int32.TryParse(indata.Substring(0, dp), out this.eventEnded.month))
                        {
                            indata = indata.Substring(dp + 1, indata.Length - dp - 1);
                            // Format : DD H[H[H]]:mm [hours|minutes|seconds|time]
                            dp = indata.IndexOf(" ");
                            if ((dp > 0) && (dp < indata.Length - 1))
                            {
                                if (Int32.TryParse(indata.Substring(0, dp), out this.eventEnded.day))
                                {
                                    indata = indata.Substring(dp + 1, indata.Length - dp - 1);
                                    // Format : H[H[H]]:mm [hours|minutes|seconds|time]
                                    dp = indata.IndexOf(" ");
                                    if ((dp > 0) && (dp < indata.Length - 1))
                                    {
                                        string possTime = indata.Substring(0, dp);
                                        // Format : H[H[H]]:mm
                                        dp = possTime.IndexOf(":");
                                        if ((dp > 0) && (dp < possTime.Length - 1))
                                        {
                                            int stort, litet;
                                            if (Int32.TryParse(possTime.Substring(0, dp), out stort))
                                            {
                                                if (Int32.TryParse(possTime.Substring(dp + 1, possTime.Length - dp - 1), out litet))
                                                    this.eventEnded.duration = (float)(stort + (litet / 100));
                                                else
                                                    this.eventEnded.duration = (float)(stort);
                                                indata = indata.Substring(dp + 1, indata.Length - dp - 1);
                                                this.eventEnded.durtyp = durationTypeDef.Undefined;
                                                if (!(checkEnumeration(indata, 0)))
                                                    this.eventEnded.durtyp = durationTypeDef.Undefined;
                                            }
                                            else
                                            {
                                                if (float.TryParse(possTime, out this.eventEnded.duration))
                                                {
                                                    indata = indata.Substring(dp + 1, indata.Length - dp - 1);
                                                    this.eventEnded.durtyp = durationTypeDef.Undefined;
                                                    if (!(checkEnumeration(indata, 0)))
                                                        this.eventEnded.durtyp = durationTypeDef.Undefined;
                                                }
                                            }
                                            retVal = true;
                                        }
                                        else
                                        {
                                            if (float.TryParse(possTime, out this.eventEnded.duration))
                                            {
                                                indata = indata.Substring(dp + 1, indata.Length - dp - 1);
                                                this.eventEnded.durtyp = durationTypeDef.Undefined;
                                                if (!(checkEnumeration(indata, 0)))
                                                    this.eventEnded.durtyp = durationTypeDef.Undefined;
                                            }
                                            retVal = true;
                                        }
                                    }
                                    else
                                    {
                                        // Format : H[H[H]]:mm
                                        dp = indata.IndexOf(":");
                                        if ((dp > 0) && (dp < indata.Length - 1))
                                        {
                                            int stort, litet;
                                            if (Int32.TryParse(indata.Substring(0, dp), out stort))
                                            {
                                                if (Int32.TryParse(indata.Substring(dp + 1, indata.Length - dp - 1), out litet))
                                                    this.eventEnded.duration = (float)(stort + (litet / 100));
                                                else
                                                    this.eventEnded.duration = (float)(stort);
                                            }
                                            else
                                            {
                                                float.TryParse(indata, out this.eventEnded.duration);
                                            }
                                            retVal = true;
                                        }
                                        else
                                        {
                                            float.TryParse(indata, out this.eventEnded.duration);
                                            retVal = true;
                                        }
                                    }
                                }
                                else
                                    retVal = false;
                            }
                        }
                    }
                }
            }
            return retVal;
        }
        public string getEventHeadline() { return this.headlineDescription; }
        public bool setEventHeadline(string indata)
        {
            this.headlineDescription = indata;
            return true;
        }
        public string getEventLatPos()
        {
            if ((geoPos.latDeg == 0) && (geoPos.latMin == 0) && (geoPos.latSec == 0) && (geoPos.latSemiSec == 0))
                return "Undefined";
            else if ((this.geoPos.latMin > 9) && (this.geoPos.latSec > 9) && (this.geoPos.latSemiSec > 9))
                return this.geoPos.latDeg.ToString() + "\u00B0" + this.geoPos.latMin.ToString() + "'" + this.geoPos.latSec.ToString() + "." + this.geoPos.latSemiSec.ToString() + " " + this.geoPos.latDir.ToString();
            else if ((this.geoPos.latMin > 9) && (this.geoPos.latSec > 9))
                return this.geoPos.latDeg.ToString() + "\u00B0" + this.geoPos.latMin.ToString() + "'" + this.geoPos.latSec.ToString() + ".0" + this.geoPos.latSemiSec.ToString() + " " + this.geoPos.latDir.ToString();
            else if ((this.geoPos.latMin > 9) && (this.geoPos.latSemiSec > 9))
                return this.geoPos.latDeg.ToString() + "\u00B0" + this.geoPos.latMin.ToString() + "'0" + this.geoPos.latSec.ToString() + "." + this.geoPos.latSemiSec.ToString() + " " + this.geoPos.latDir.ToString();
            else if ((this.geoPos.latMin > 9))
                return this.geoPos.latDeg.ToString() + "\u00B0" + this.geoPos.latMin.ToString() + "'0" + this.geoPos.latSec.ToString() + ".0" + this.geoPos.latSemiSec.ToString() + " " + this.geoPos.latDir.ToString();
            else if ((this.geoPos.latMin > 9) && (this.geoPos.latSemiSec > 9))
                return this.geoPos.latDeg.ToString() + "\u00B0" + this.geoPos.latMin.ToString() + "'0" + this.geoPos.latSec.ToString() + "." + this.geoPos.latSemiSec.ToString() + " " + this.geoPos.latDir.ToString();
            else if ((this.geoPos.latSec > 9) && (this.geoPos.latSemiSec > 9))
                return this.geoPos.latDeg.ToString() + "\u00B00" + this.geoPos.latMin.ToString() + "'" + this.geoPos.latSec.ToString() + "." + this.geoPos.latSemiSec.ToString() + " " + this.geoPos.latDir.ToString();
            else if ((this.geoPos.latSemiSec > 9))
                return this.geoPos.latDeg.ToString() + "\u00B00" + this.geoPos.latMin.ToString() + "'0" + this.geoPos.latSec.ToString() + "." + this.geoPos.latSemiSec.ToString() + " " + this.geoPos.latDir.ToString();
            else if ((this.geoPos.latSec > 9))
                return this.geoPos.latDeg.ToString() + "\u00B00" + this.geoPos.latMin.ToString() + "'" + this.geoPos.latSec.ToString() + ".0" + this.geoPos.latSemiSec.ToString() + " " + this.geoPos.latDir.ToString();
            else
                return this.geoPos.latDeg.ToString() + "\u00B00" + this.geoPos.latMin.ToString() + "'0" + this.geoPos.latSec.ToString() + ".0" + this.geoPos.latSemiSec.ToString() + " " + this.geoPos.latDir.ToString();
        }
        public bool setEventLatPos(string indata)
        {
            bool retVal = false;
            if (indata.Contains(":"))
            {
                // Expected format : DD:mm:SS.ss [E|East|e|east|W|West|w|west]
                int dp = indata.IndexOf(":");
                if ((dp > 0) && (dp < indata.Length - 1))
                {
                    this.geoPos.latDeg = Int32.Parse(indata.Substring(0, dp));
                    indata = indata.Substring(dp + 1, indata.Length - dp - 1);
                    // Expected format : mm:SS.ss [E|East|e|east|W|West|w|west]
                    dp = indata.IndexOf(":");
                    if ((dp > 0) && (dp < indata.Length - 1))
                    {
                        this.geoPos.latMin = Int32.Parse(indata.Substring(0, dp));
                        indata = indata.Substring(dp + 1, indata.Length - dp - 1);
                        // Expected format : SS.ss [E|East|e|east|W|West|w|west]
                        dp = indata.IndexOf(".");
                        if ((dp > 0) && (dp < indata.Length - 1))
                        {
                            this.geoPos.latSec = Int32.Parse(indata.Substring(0, dp));
                            indata = indata.Substring(dp + 1, indata.Length - dp - 1);
                            // Expected format : ss [E|East|e|east|W|West|w|west]
                            dp = indata.IndexOf(" ");
                            if ((dp > 0) && (dp < indata.Length - 1))
                            {
                                this.geoPos.latSemiSec = Int32.Parse(indata.Substring(0, dp));
                                indata = indata.Substring(dp + 1, indata.Length - dp - 1);
                                if (!(checkEnumeration(indata, 0)))
                                    this.geoPos.latDir = eventGeographicDirection.Undefined;
                            }
                            else
                                this.geoPos.latSemiSec = Int32.Parse(indata);
                        }
                        else
                            this.geoPos.latSec = Int32.Parse(indata);
                    }
                    else
                        this.geoPos.latMin = Int32.Parse(indata);
                }
                else
                    this.geoPos.latDeg = Int32.Parse(indata);
                retVal = true;
            }
            else if (indata.Contains("\u00B00"))
            {
                // Expected format : DDD<gradtecken>mm'SS.ss [E|East|e|east|W|West|w|west]
                int dp = indata.IndexOf("\u00B00");
                if ((dp > 0) && (dp < indata.Length - 1))
                {
                    try { this.geoPos.latDeg = Int32.Parse(indata.Substring(0, dp)); } catch { }
                    indata = indata.Substring(dp + 1, indata.Length - dp - 1);
                    // Expected format : mm'SS.ss [E|East|e|east|W|West|w|west]
                    dp = indata.IndexOf("'");
                    if ((dp > 0) && (dp < indata.Length - 1))
                    {
                        try { this.geoPos.latMin = Int32.Parse(indata.Substring(0, dp)); } catch { }
                        indata = indata.Substring(dp + 1, indata.Length - dp - 1);
                        // Expected format : SS.ss [E|East|e|east|W|West|w|west]
                        dp = indata.IndexOf(".");
                        if ((dp > 0) && (dp < indata.Length - 1))
                        {
                            try { this.geoPos.latSec = Int32.Parse(indata.Substring(0, dp)); } catch { }
                            indata = indata.Substring(dp + 1, indata.Length - dp - 1);
                            // Expected format : ss [E|East|e|east|W|West|w|west]
                            dp = indata.IndexOf(" ");
                            if ((dp > 0) && (dp < indata.Length))
                            {
                                try { this.geoPos.latSemiSec = Int32.Parse(indata.Substring(0, dp)); } catch { }
                                indata = indata.Substring(dp + 1, indata.Length - dp - 1);
                                if ((indata.ToLower(CultureInfo.InvariantCulture) == "e") || (indata.ToLower(CultureInfo.InvariantCulture) == "east"))
                                    this.geoPos.latDir = eventGeographicDirection.East;
                                else
                                    this.geoPos.latDir = eventGeographicDirection.West;
                            }
                            else
                                try { this.geoPos.latSemiSec = Int32.Parse(indata); } catch { }
                        }
                        else
                            try { this.geoPos.latSec = Int32.Parse(indata); } catch { }
                    }
                    else
                        try { this.geoPos.latDeg = Int32.Parse(indata); } catch { }
                }
            }
            else if (indata.Contains("-"))
            {
                // Expected format : DD-mm-SS.ss [E|East|e|east|W|West|w|west]
                int dp = indata.IndexOf("-");
                if ((dp > 0) && (dp < indata.Length - 1))
                {
                    this.geoPos.latDeg = Int32.Parse(indata.Substring(0, dp));
                    indata = indata.Substring(dp + 1, indata.Length - dp - 1);
                    // Expected format : mm:SS.ss [E|East|e|east|W|West|w|west]
                    dp = indata.IndexOf("-");
                    if ((dp > 0) && (dp < indata.Length - 1))
                    {
                        this.geoPos.latMin = Int32.Parse(indata.Substring(0, dp));
                        indata = indata.Substring(dp + 1, indata.Length - dp - 1);
                        // Expected format : SS.ss [E|East|e|east|W|West|w|west]
                        dp = indata.IndexOf(".");
                        if ((dp > 0) && (dp < indata.Length - 1))
                        {
                            this.geoPos.latSec = Int32.Parse(indata.Substring(0, dp));
                            indata = indata.Substring(dp + 1, indata.Length - dp - 1);
                            // Expected format : ss [E|East|e|east|W|West|w|west]
                            dp = indata.IndexOf(" ");
                            if ((dp > 0) && (dp < indata.Length - 1))
                            {
                                this.geoPos.latSemiSec = Int32.Parse(indata.Substring(0, dp));
                                indata = indata.Substring(dp + 1, indata.Length - dp - 1);
                                // Expected format : [E|East|e|east|W|West|w|west]
                                if ((indata.ToLower(CultureInfo.InvariantCulture) == "e") || (indata.ToLower(CultureInfo.InvariantCulture) == "east"))
                                    this.geoPos.latDir = eventGeographicDirection.East;
                                else
                                    this.geoPos.latDir = eventGeographicDirection.West;
                            }
                            else
                                this.geoPos.latSemiSec = Int32.Parse(indata);
                        }
                        else
                            this.geoPos.latSec = Int32.Parse(indata);
                    }
                    else
                        this.geoPos.latMin = Int32.Parse(indata);
                }
                else
                    this.geoPos.latDeg = Int32.Parse(indata);
                retVal = true;
            }
            else
            {
                // Expected format : DD mm SS.ss [E|East|e|east|W|West|w|west]
                int dp = indata.IndexOf(" ");
                if ((dp > 0) && (dp < indata.Length - 1))
                {
                    try { this.geoPos.latDeg = Int32.Parse(indata.Substring(0, dp)); } catch { }
                    indata = indata.Substring(dp + 1, indata.Length - dp - 1);
                    // Expected format : mm:SS.ss [E|East|e|east|W|West|w|west]
                    dp = indata.IndexOf(" ");
                    if ((dp > 0) && (dp < indata.Length - 1))
                    {
                        this.geoPos.latMin = Int32.Parse(indata.Substring(0, dp));
                        indata = indata.Substring(dp + 1, indata.Length - dp - 1);
                        // Expected format : SS.ss [E|East|e|east|W|West|w|west]
                        dp = indata.IndexOf(".");
                        if ((dp > 0) && (dp < indata.Length - 1))
                        {
                            this.geoPos.latSec = Int32.Parse(indata.Substring(0, dp));
                            indata = indata.Substring(dp + 1, indata.Length - dp - 1);
                            // Expected format : ss [E|East|e|east|W|West|w|west]
                            dp = indata.IndexOf(" ");
                            if ((dp > 0) && (dp < indata.Length - 1))
                            {
                                this.geoPos.latSemiSec = Int32.Parse(indata.Substring(0, dp));
                                indata = indata.Substring(dp + 1, indata.Length - dp - 1);
                                // Expected format : [E|East|e|east|W|West|w|west]
                                if ((indata.ToLower(CultureInfo.InvariantCulture) == "e") || (indata.ToLower(CultureInfo.InvariantCulture) == "east"))
                                    this.geoPos.latDir = eventGeographicDirection.East;
                                else
                                    this.geoPos.latDir = eventGeographicDirection.West;
                            }
                            else
                                try { this.geoPos.latSemiSec = Int32.Parse(indata); } catch { }
                        }
                        else
                            try { this.geoPos.latSec = Int32.Parse(indata); } catch { }
                    }
                    else
                        try { this.geoPos.latMin = Int32.Parse(indata); } catch { }
                }
                else
                    try { this.geoPos.latDeg = Int32.Parse(indata); } catch { }
                retVal = true;
            }
            return retVal;
        }
        public string getEventLonPos()
        {
            if ((geoPos.lonDeg == 0) && (geoPos.lonMin == 0) && (geoPos.lonSec == 0) && (geoPos.lonSemiSec == 0))
                return "Undefined";
            else if ((this.geoPos.lonMin > 9) && (this.geoPos.lonSec > 9) && (this.geoPos.lonSemiSec > 9))
                return this.geoPos.lonDeg.ToString() + "\u00B0" + this.geoPos.lonMin.ToString() + "'" + this.geoPos.lonSec.ToString() + "." + this.geoPos.lonSemiSec.ToString() + " " + this.geoPos.lonDir.ToString();
            else if ((this.geoPos.lonMin > 9) && (this.geoPos.lonSec > 9))
                return this.geoPos.lonDeg.ToString() + "\u00B0" + this.geoPos.lonMin.ToString() + "'" + this.geoPos.lonSec.ToString() + ".0" + this.geoPos.lonSemiSec.ToString() + " " + this.geoPos.lonDir.ToString();
            else if ((this.geoPos.lonMin > 9) && (this.geoPos.lonSemiSec > 9))
                return this.geoPos.lonDeg.ToString() + "\u00B0" + this.geoPos.lonMin.ToString() + "'0" + this.geoPos.lonSec.ToString() + "." + this.geoPos.lonSemiSec.ToString() + " " + this.geoPos.lonDir.ToString();
            else if ((this.geoPos.lonMin > 9))
                return this.geoPos.lonDeg.ToString() + "\u00B0" + this.geoPos.lonMin.ToString() + "'0" + this.geoPos.lonSec.ToString() + ".0" + this.geoPos.lonSemiSec.ToString() + " " + this.geoPos.lonDir.ToString();
            else if ((this.geoPos.lonMin > 9) && (this.geoPos.lonSemiSec > 9))
                return this.geoPos.lonDeg.ToString() + "\u00B0" + this.geoPos.lonMin.ToString() + "'0" + this.geoPos.lonSec.ToString() + "." + this.geoPos.lonSemiSec.ToString() + " " + this.geoPos.lonDir.ToString();
            else if ((this.geoPos.lonSec > 9) && (this.geoPos.lonSemiSec > 9))
                return this.geoPos.lonDeg.ToString() + "\u00B00" + this.geoPos.lonMin.ToString() + "'" + this.geoPos.lonSec.ToString() + "." + this.geoPos.lonSemiSec.ToString() + " " + this.geoPos.lonDir.ToString();
            else if ((this.geoPos.lonSemiSec > 9))
                return this.geoPos.lonDeg.ToString() + "\u00B00" + this.geoPos.lonMin.ToString() + "'0" + this.geoPos.lonSec.ToString() + "." + this.geoPos.lonSemiSec.ToString() + " " + this.geoPos.lonDir.ToString();
            else if ((this.geoPos.lonSec > 9))
                return this.geoPos.lonDeg.ToString() + "\u00B00" + this.geoPos.lonMin.ToString() + "'" + this.geoPos.lonSec.ToString() + ".0" + this.geoPos.lonSemiSec.ToString() + " " + this.geoPos.lonDir.ToString();
            else
                return this.geoPos.lonDeg.ToString() + "\u00B00" + this.geoPos.lonMin.ToString() + "'0" + this.geoPos.lonSec.ToString() + ".0" + this.geoPos.lonSemiSec.ToString() + " " + this.geoPos.lonDir.ToString();
        }
        public bool setEventLonPos(string indata)
        {
            bool retVal = false;
            if (indata.Contains(":"))
            {
                // Expected format : DDD:mm:SS.ss [N|North|n|north|S|South|s|south]
                int dp = indata.IndexOf(":");
                if ((dp > 0) && (dp < indata.Length - 1))
                {
                    this.geoPos.lonDeg = Int32.Parse(indata.Substring(0, dp));
                    indata = indata.Substring(dp + 1, indata.Length - dp - 1);
                    // Expected format : mm:SS.ss [N|North|n|north|S|South|s|south]
                    dp = indata.IndexOf(":");
                    if ((dp > 0) && (dp < indata.Length - 1))
                    {
                        this.geoPos.lonMin = Int32.Parse(indata.Substring(0, dp));
                        indata = indata.Substring(dp + 1, indata.Length - dp - 1);
                        // Expected format : SS.ss [N|North|n|north|S|South|s|south]
                        dp = indata.IndexOf(".");
                        if ((dp > 0) && (dp < indata.Length - 1))
                        {
                            this.geoPos.lonSec = Int32.Parse(indata.Substring(0, dp));
                            indata = indata.Substring(dp + 1, indata.Length - dp - 1);
                            // Expected format : ss [N|North|n|north|S|South|s|south]
                            dp = indata.IndexOf(" ");
                            if ((dp > 0) && (dp < indata.Length - 1))
                            {
                                this.geoPos.lonSemiSec = Int32.Parse(indata.Substring(0, dp));
                                indata = indata.Substring(dp + 1, indata.Length - dp - 1);
                                if (!(checkEnumeration(indata, 0)))
                                    this.geoPos.lonDir = eventGeographicDirection.Undefined;
                            }
                            else
                                this.geoPos.lonSemiSec = Int32.Parse(indata);
                        }
                        else
                            this.geoPos.lonSec = Int32.Parse(indata);
                    }
                    else
                        this.geoPos.lonMin = Int32.Parse(indata);
                }
                else
                    this.geoPos.lonDeg = Int32.Parse(indata);
                retVal = true;
            }
            else if (indata.Contains("\u00B00"))
            {
                // Expected format : DD<gradtecken>mm'SS.ss [N|North|n|north|S|South|s|south]
                int dp = indata.IndexOf("\u00B00");
                if ((dp > 0) && (dp < indata.Length - 1))
                {
                    try { this.geoPos.lonDeg = Int32.Parse(indata.Substring(0, dp)); } catch { }
                    indata = indata.Substring(dp + 1, indata.Length - dp - 1);
                    // Expected format : mm'SS.ss [N|North|n|north|S|South|s|south]
                    dp = indata.IndexOf("'");
                    if ((dp > 0) && (dp < indata.Length - 1))
                    {
                        try { this.geoPos.lonMin = Int32.Parse(indata.Substring(0, dp)); } catch { }
                        indata = indata.Substring(dp + 1, indata.Length - dp - 1);
                        // Expected format : SS.ss [N|North|n|north|S|South|s|south]
                        dp = indata.IndexOf(".");
                        if ((dp > 0) && (dp < indata.Length - 1))
                        {
                            try { this.geoPos.lonSec = Int32.Parse(indata.Substring(0, dp)); } catch { }
                            indata = indata.Substring(dp + 1, indata.Length - dp - 1);
                            // Expected format : ss [N|North|n|north|S|South|s|south]
                            dp = indata.IndexOf(" ");
                            if ((dp > 0) && (dp < indata.Length - 1))
                            {
                                try { this.geoPos.lonSemiSec = Int32.Parse(indata.Substring(0, dp)); } catch { }
                                indata = indata.Substring(dp + 1, indata.Length - dp - 1);
                                // Expected format : [N|North|n|north|S|South|s|south]
                                if ((indata == "N") || (indata == "North") || (indata == "n") || (indata == "north"))
                                    this.geoPos.latDir = eventGeographicDirection.North;
                                else
                                    this.geoPos.latDir = eventGeographicDirection.South;
                            }
                            else
                                try { this.geoPos.lonSemiSec = Int32.Parse(indata); } catch { }
                        }
                        else
                            try { this.geoPos.lonSec = Int32.Parse(indata); } catch { }
                    }
                    else
                        try { this.geoPos.lonMin = Int32.Parse(indata); } catch { }
                }
                else
                    try { this.geoPos.lonDeg = Int32.Parse(indata); } catch { }
            }
            else if (indata.Contains("-"))
            {
                // Expected format : DD-mm-SS.ss [N|North|n|north|S|South|s|south]
                int dp = indata.IndexOf("-");
                if ((dp > 0) && (dp < indata.Length - 1))
                {
                    try { this.geoPos.lonDeg = Int32.Parse(indata.Substring(0, dp)); } catch { }
                    indata = indata.Substring(dp + 1, indata.Length - dp - 1);
                    // Expected format : mm:SS.ss [N|North|n|north|S|South|s|south]
                    dp = indata.IndexOf("-");
                    if ((dp > 0) && (dp < indata.Length - 1))
                    {
                        this.geoPos.lonMin = Int32.Parse(indata.Substring(0, dp));
                        indata = indata.Substring(dp + 1, indata.Length - dp - 1);
                        // Expected format : SS.ss [N|North|n|north|S|South|s|south]
                        dp = indata.IndexOf(".");
                        if ((dp > 0) && (dp < indata.Length - 1))
                        {
                            this.geoPos.lonSec = Int32.Parse(indata.Substring(0, dp));
                            indata = indata.Substring(dp + 1, indata.Length - dp - 1);
                            // Expected format : ss [N|North|n|north|S|South|s|south]
                            dp = indata.IndexOf(" ");
                            if ((dp > 0) && (dp < indata.Length - 1))
                            {
                                this.geoPos.lonSemiSec = Int32.Parse(indata.Substring(0, dp));
                                indata = indata.Substring(dp + 1, indata.Length - dp - 1);
                                // Expected format : [N|North|n|north|S|South|s|south]
                                if ((indata.ToLower(CultureInfo.InvariantCulture) == "n") || (indata.ToLower(CultureInfo.InvariantCulture) == "north"))
                                    this.geoPos.lonDir = eventGeographicDirection.North;
                                else
                                    this.geoPos.lonDir = eventGeographicDirection.South;
                            }
                            else
                                this.geoPos.lonSemiSec = Int32.Parse(indata);
                        }
                        else
                            this.geoPos.lonSec = Int32.Parse(indata);
                    }
                    else
                        this.geoPos.lonMin = Int32.Parse(indata);
                }
                else
                    this.geoPos.lonDeg = Int32.Parse(indata);
                retVal = true;
            }
            else
            {
                // Expected format : DD mm SS.ss [N|North|n|north|S|South|s|south]
                int dp = indata.IndexOf(" ");
                if ((dp > 0) && (dp < indata.Length - 1))
                {
                    try { this.geoPos.lonDeg = Int32.Parse(indata.Substring(0, dp)); } catch { }
                    indata = indata.Substring(dp + 1, indata.Length - dp - 1);
                    // Expected format : mm:SS.ss [N|North|n|north|S|South|s|south]
                    dp = indata.IndexOf(" ");
                    if ((dp > 0) && (dp < indata.Length - 1))
                    {
                        try { this.geoPos.lonMin = Int32.Parse(indata.Substring(0, dp)); } catch { }
                        indata = indata.Substring(dp + 1, indata.Length - dp - 1);
                        // Expected format : SS.ss [N|North|n|north|S|South|s|south]
                        dp = indata.IndexOf(".");
                        if ((dp > 0) && (dp < indata.Length - 1))
                        {
                            try { this.geoPos.lonSec = Int32.Parse(indata.Substring(0, dp)); } catch { }
                            indata = indata.Substring(dp + 1, indata.Length - dp - 1);
                            // Expected format : ss [N|North|n|north|S|South|s|south]
                            dp = indata.IndexOf(" ");
                            if ((dp > 0) && (dp < indata.Length - 1))
                            {
                                try { this.geoPos.lonSemiSec = Int32.Parse(indata.Substring(0, dp)); } catch { }
                                indata = indata.Substring(dp + 1, indata.Length - dp - 1);
                                // Expected format : [N|North|n|north|S|South|s|south]
                                if ((indata.ToLower(CultureInfo.InvariantCulture) == "n") || (indata.ToLower(CultureInfo.InvariantCulture) == "north"))
                                    this.geoPos.lonDir = eventGeographicDirection.North;
                                else
                                    this.geoPos.lonDir = eventGeographicDirection.South;
                            }
                            else
                                try { this.geoPos.lonSemiSec = Int32.Parse(indata); } catch { }
                        }
                        else
                            try { this.geoPos.lonSec = Int32.Parse(indata); } catch { }
                    }
                    else
                        try { this.geoPos.lonMin = Int32.Parse(indata); } catch { }
                }
                else
                    try { this.geoPos.lonDeg = Int32.Parse(indata); } catch { }
                retVal = true;
            }
            return retVal;

        }
        public string getEventGeoPosName() { return this.geoPos.geographicName; }
        public bool setEventGeoPosName(string indata)
        {
            this.geoPos.geographicName = indata;
            return true;
        }
        public string getEventStreetname()
        {
            string retStr = this.theEventAddress.streetname;
            if (this.theEventAddress.housenumber > 0)
                retStr = retStr + " " + this.theEventAddress.housenumber.ToString();
            if (this.theEventAddress.housenumberaddon != "")
                retStr = retStr + this.theEventAddress.housenumberaddon;
            if (this.theEventAddress.floornumber > 0)
                retStr = retStr + " " + this.theEventAddress.floornumber.ToString() + " stairs";
            return retStr;
        }
        public bool setEventStreetname(string indata)
        {
            bool retVal = false;
            // Syntax: <streetnamepart>[ <streetname part>][ <streetnamepart>][ <number>[<addon>][ <number> [stairs|floor|floors|level]
            string currPrt;
            int currNum;
            int dp = indata.LastIndexOf(" ");
            while ((!(retVal)) && (dp > 0))
            {
                currPrt = indata.Substring(dp + 1, indata.Length - dp - 1);
                if ((currPrt == "stairs") || (currPrt == "floor") || (currPrt == "floors") || (currPrt == "level"))
                {
                    // floornumber
                    indata = indata.Substring(0, dp);
                    dp = indata.LastIndexOf(" ");
                    if (int.TryParse(indata.Substring(dp + 1, indata.Length - dp - 1), out currNum))
                        this.theEventAddress.floornumber = currNum;
                }
                else if (int.TryParse(currPrt, out currNum))
                {
                    // housenumber
                    this.theEventAddress.housenumber = currNum;
                }
                else if ((currPrt.ToUpper().Contains("A")) || (currPrt.ToUpper().Contains("B")) || (currPrt.ToUpper().Contains("C")) || (currPrt.ToUpper().Contains("D")) ||
                         (currPrt.ToUpper().Contains("E")) || (currPrt.ToUpper().Contains("F")) || (currPrt.ToUpper().Contains("G")) || (currPrt.ToUpper().Contains("H")) ||
                         (currPrt.ToUpper().Contains("I")) || (currPrt.ToUpper().Contains("J")) || (currPrt.ToUpper().Contains("K")) || (currPrt.ToUpper().Contains("L")) ||
                         (currPrt.ToUpper().Contains("M")) || (currPrt.ToUpper().Contains("N")) || (currPrt.ToUpper().Contains("O")) || (currPrt.ToUpper().Contains("P")) ||
                         (currPrt.ToUpper().Contains("Q")) || (currPrt.ToUpper().Contains("R")) || (currPrt.ToUpper().Contains("S")) || (currPrt.ToUpper().Contains("T")) ||
                         (currPrt.ToUpper().Contains("U")) || (currPrt.ToUpper().Contains("V")) || (currPrt.ToUpper().Contains("X")) || (currPrt.ToUpper().Contains("Y")) ||
                         (currPrt.ToUpper().Contains("Z")) || (currPrt.ToUpper().Contains("Å")) || (currPrt.ToUpper().Contains("Ä")) || (currPrt.ToUpper().Contains("Ö")))
                {
                    if (int.TryParse(currPrt.Substring(0, currPrt.Length - 1), out currNum))
                    {
                        this.theEventAddress.housenumber = currNum;
                        this.theEventAddress.housenumberaddon = currPrt.Substring(currPrt.Length - 1, 1);
                    }
                    else
                    {
                        this.theEventAddress.statename = indata;
                        retVal = true;
                    }
                }
                indata = indata.Substring(0, dp);
                dp = indata.LastIndexOf(" ");
            }
            return retVal;
        }
        public string getEventCityname() { return this.theEventAddress.cityname; }
        public bool setEventCityname(string indata)
        {
            this.theEventAddress.cityname = indata;
            return true;
        }
        public string getEventAreaname() { return this.theEventAddress.areaname; }
        public bool setEventAreaname(string indata)
        {
            this.theEventAddress.areaname = indata;
            return true;
        }
        public string getEventAreacode() { return this.theEventAddress.areacode; }
        public bool setEventAreacode(string indata)
        {
            this.theEventAddress.areacode = indata;
            return true;
        }
        public string getEventStatename() { return this.theEventAddress.statename; }
        public bool setEventStatename(string indata)
        {
            this.theEventAddress.statename = indata;
            return true;
        }
        public string getEventCountryname() { return this.theEventAddress.countryname; }
        public bool setEventCountryname(string indata)
        {
            this.theEventAddress.countryname = indata;
            return true;
        }
        public int getNoOfEventAttender() { return this.noOfEventAttender; }
        public string getEventAttenderID(int nr)
        {
            if ((nr >= 0) && (nr < this.noOfEventAttender))
                return this.thisEventAttenders[nr].eventAttenderId;
            else
                return "";
        }
        public string getEventAttenderLevel(int nr)
        {
            if ((nr > 0) && (nr < this.noOfEventAttender))
                return this.thisEventAttenders[nr].eventAttenderLevel.ToString();
            else
                return "";
        }
        public string getEventAttenderRoleTag(int nr)
        {
            if ((nr > 0) && (nr < this.noOfEventAttender))
                return this.thisEventAttenders[nr].eventAttenderRole.tag;
            else
                return "";
        }
        public string getEventAttenderRoleDescription(int nr)
        {
            if ((nr > 0) && (nr < this.noOfEventAttender))
                return this.thisEventAttenders[nr].eventAttenderRole.description;
            else
                return "";
        }
        public int getEventAttenderRoleLevel(int nr)
        {
            if ((nr > 0) && (nr < this.noOfEventAttender))
                return this.thisEventAttenders[nr].eventAttenderRole.level;
            else
                return 0;
        }
        public bool addEventAttender(string id, string level, string role, string roleDescription, int rolelevel)
        {
            bool retVal = false;
            if (noOfEventAttender < maxNoOfEventAttender)
            {
                thisEventAttenders[noOfEventAttender].eventAttenderId = id;
                // Undefined, Unclassified, Limited, Confidential, Secret, QualifSecret
                if (level.ToLower(CultureInfo.InvariantCulture) == "unclassified")
                    thisEventAttenders[noOfEventAttender].eventAttenderLevel = eventClassificationType.Unclassified;
                else if (level.ToLower(CultureInfo.InvariantCulture) == "limited")
                    thisEventAttenders[noOfEventAttender].eventAttenderLevel = eventClassificationType.Limited;
                else if (level.ToLower(CultureInfo.InvariantCulture) == "confidential")
                    thisEventAttenders[noOfEventAttender].eventAttenderLevel = eventClassificationType.Confidential;
                else if (level.ToLower(CultureInfo.InvariantCulture) == "secret")
                    thisEventAttenders[noOfEventAttender].eventAttenderLevel = eventClassificationType.Secret;
                else if (level.ToLower(CultureInfo.InvariantCulture) == "qualifiedsecret")
                    thisEventAttenders[noOfEventAttender].eventAttenderLevel = eventClassificationType.QualifSecret;
                else
                    thisEventAttenders[noOfEventAttender].eventAttenderLevel = eventClassificationType.Undefined;
                bool foundCategory = false;
                for (int i = 0; i < noOfRoleCategories; i++)
                {
                    if (role == roleCategories[i].tag)
                    {
                        thisEventAttenders[noOfEventAttender].eventAttenderRole.tag = roleCategories[i].tag;
                        if (roleCategories[i].description != "")
                            thisEventAttenders[noOfEventAttender].eventAttenderRole.description = roleCategories[i].description;
                        else if (roleDescription != "")
                            thisEventAttenders[noOfEventAttender].eventAttenderRole.description = roleDescription;
                        if (roleCategories[i].level > 0)
                            thisEventAttenders[noOfEventAttender].eventAttenderRole.level = roleCategories[i].level;
                        else if (roleCategories[i].level > rolelevel)
                            thisEventAttenders[noOfEventAttender].eventAttenderRole.level = rolelevel;
                        else
                            thisEventAttenders[noOfEventAttender].eventAttenderRole.level = 0;
                        noOfEventAttender++;
                    }
                }
                if (!(foundCategory))
                {
                    addRoleCategory(role, roleDescription, rolelevel.ToString());
                    thisEventAttenders[noOfEventAttender].eventAttenderRole.tag = role;
                    thisEventAttenders[noOfEventAttender].eventAttenderRole.description = roleDescription;
                    thisEventAttenders[noOfEventAttender].eventAttenderRole.level = rolelevel;
                    noOfEventAttender++;
                }
                retVal = true;
            }
            return retVal;
        }
        public bool addEventAttender(string id, string level, string role, string roleDescription, string rolelevel)
        {
            bool retVal = false;
            if (noOfEventAttender < maxNoOfEventAttender)
            {
                thisEventAttenders[noOfEventAttender].eventAttenderId = id;
                // Undefined, Unclassified, Limited, Confidential, Secret, QualifSecret
                if (level.ToLower(CultureInfo.InvariantCulture) == "unclassified")
                    thisEventAttenders[noOfEventAttender].eventAttenderLevel = eventClassificationType.Unclassified;
                else if (level.ToLower(CultureInfo.InvariantCulture) == "limited")
                    thisEventAttenders[noOfEventAttender].eventAttenderLevel = eventClassificationType.Limited;
                else if (level.ToLower(CultureInfo.InvariantCulture) == "confidential")
                    thisEventAttenders[noOfEventAttender].eventAttenderLevel = eventClassificationType.Confidential;
                else if (level.ToLower(CultureInfo.InvariantCulture) == "secret")
                    thisEventAttenders[noOfEventAttender].eventAttenderLevel = eventClassificationType.Secret;
                else if (level.ToLower(CultureInfo.InvariantCulture) == "qualifiedsecret")
                    thisEventAttenders[noOfEventAttender].eventAttenderLevel = eventClassificationType.QualifSecret;
                else
                    thisEventAttenders[noOfEventAttender].eventAttenderLevel = eventClassificationType.Undefined;
                bool foundCategory = false;
                for (int i = 0; i < noOfRoleCategories; i++)
                {
                    if (role == roleCategories[i].tag)
                    {
                        thisEventAttenders[noOfEventAttender].eventAttenderRole.tag = roleCategories[i].tag;
                        if (roleCategories[i].description != "")
                            thisEventAttenders[noOfEventAttender].eventAttenderRole.description = roleCategories[i].description;
                        else if (roleDescription != "")
                            thisEventAttenders[noOfEventAttender].eventAttenderRole.description = roleDescription;
                        if (roleCategories[i].level > 0)
                            thisEventAttenders[noOfEventAttender].eventAttenderRole.level = roleCategories[i].level;
                        if (rolelevel == "unclassified")
                            thisEventAttenders[noOfEventAttender].eventAttenderRole.level = 1;
                        else if (rolelevel == "limited")
                            thisEventAttenders[noOfEventAttender].eventAttenderRole.level = 2;
                        else if (rolelevel == "confidential")
                            thisEventAttenders[noOfEventAttender].eventAttenderRole.level = 3;
                        else if (rolelevel == "secret")
                            thisEventAttenders[noOfEventAttender].eventAttenderRole.level = 4;
                        else if (rolelevel == "qualifiedsecret")
                            thisEventAttenders[noOfEventAttender].eventAttenderRole.level = 5;
                        else
                            thisEventAttenders[noOfEventAttender].eventAttenderRole.level = 0;
                        noOfEventAttender++;
                    }
                }
                if (!(foundCategory))
                {
                    addRoleCategory(role, roleDescription, rolelevel.ToString());
                    thisEventAttenders[noOfEventAttender].eventAttenderRole.tag = role;
                    thisEventAttenders[noOfEventAttender].eventAttenderRole.description = roleDescription;
                    if (rolelevel == "unclassified")
                        thisEventAttenders[noOfEventAttender].eventAttenderRole.level = 1;
                    else if (rolelevel == "limited")
                        thisEventAttenders[noOfEventAttender].eventAttenderRole.level = 2;
                    else if (rolelevel == "confidential")
                        thisEventAttenders[noOfEventAttender].eventAttenderRole.level = 3;
                    else if (rolelevel == "secret")
                        thisEventAttenders[noOfEventAttender].eventAttenderRole.level = 4;
                    else if (rolelevel == "qualifiedsecret")
                        thisEventAttenders[noOfEventAttender].eventAttenderRole.level = 5;
                    else
                        thisEventAttenders[noOfEventAttender].eventAttenderRole.level = 0;
                    noOfEventAttender++;
                }
                retVal = true;
            }
            return retVal;
        }
        public bool addEventAttender(string id, string level, string role, string roleDescription)
        {
            bool retVal = false;
            if (noOfEventAttender < maxNoOfEventAttender)
            {
                thisEventAttenders[noOfEventAttender].eventAttenderId = id;
                // Undefined, Unclassified, Limited, Confidential, Secret, QualifSecret
                if (level.ToLower(CultureInfo.InvariantCulture) == "unclassified")
                    thisEventAttenders[noOfEventAttender].eventAttenderLevel = eventClassificationType.Unclassified;
                else if (level.ToLower(CultureInfo.InvariantCulture) == "limited")
                    thisEventAttenders[noOfEventAttender].eventAttenderLevel = eventClassificationType.Limited;
                else if (level.ToLower(CultureInfo.InvariantCulture) == "confidential")
                    thisEventAttenders[noOfEventAttender].eventAttenderLevel = eventClassificationType.Confidential;
                else if (level.ToLower(CultureInfo.InvariantCulture) == "secret")
                    thisEventAttenders[noOfEventAttender].eventAttenderLevel = eventClassificationType.Secret;
                else if (level.ToLower(CultureInfo.InvariantCulture) == "qualifiedsecret")
                    thisEventAttenders[noOfEventAttender].eventAttenderLevel = eventClassificationType.QualifSecret;
                else
                    thisEventAttenders[noOfEventAttender].eventAttenderLevel = eventClassificationType.Undefined;
                bool foundCategory = false;
                for (int i = 0; i < noOfRoleCategories; i++)
                {
                    if (role == roleCategories[i].tag)
                    {
                        thisEventAttenders[noOfEventAttender].eventAttenderRole.tag = roleCategories[i].tag;
                        if (roleCategories[i].description != "")
                            thisEventAttenders[noOfEventAttender].eventAttenderRole.description = roleCategories[i].description;
                        else if (roleDescription != "")
                            thisEventAttenders[noOfEventAttender].eventAttenderRole.description = roleDescription;
                        thisEventAttenders[noOfEventAttender].eventAttenderRole.level = 0;
                        noOfEventAttender++;
                    }
                }
                if (!(foundCategory))
                {
                    addRoleCategory(role, roleDescription, "Unclassified");
                    thisEventAttenders[noOfEventAttender].eventAttenderRole.tag = role;
                    thisEventAttenders[noOfEventAttender].eventAttenderRole.description = roleDescription;
                    thisEventAttenders[noOfEventAttender].eventAttenderRole.level = 1;
                    noOfEventAttender++;
                }
                retVal = true;
            }
            return retVal;
        }
        public bool addEventAttender(string id, string level, string role)
        {
            bool retVal = false;
            if (noOfEventAttender < maxNoOfEventAttender)
            {
                thisEventAttenders[noOfEventAttender].eventAttenderId = id;
                // Undefined, Unclassified, Limited, Confidential, Secret, QualifSecret
                if (level.ToLower(CultureInfo.InvariantCulture) == "unclassified")
                    thisEventAttenders[noOfEventAttender].eventAttenderLevel = eventClassificationType.Unclassified;
                else if (level.ToLower(CultureInfo.InvariantCulture) == "limited")
                    thisEventAttenders[noOfEventAttender].eventAttenderLevel = eventClassificationType.Limited;
                else if (level.ToLower(CultureInfo.InvariantCulture) == "confidential")
                    thisEventAttenders[noOfEventAttender].eventAttenderLevel = eventClassificationType.Confidential;
                else if (level.ToLower(CultureInfo.InvariantCulture) == "secret")
                    thisEventAttenders[noOfEventAttender].eventAttenderLevel = eventClassificationType.Secret;
                else if (level.ToLower(CultureInfo.InvariantCulture) == "qualifiedsecret")
                    thisEventAttenders[noOfEventAttender].eventAttenderLevel = eventClassificationType.QualifSecret;
                else
                    thisEventAttenders[noOfEventAttender].eventAttenderLevel = eventClassificationType.Undefined;
                bool foundCategory = false;
                for (int i = 0; i < noOfRoleCategories; i++)
                {
                    if (role == roleCategories[i].tag)
                    {
                        thisEventAttenders[noOfEventAttender].eventAttenderRole.tag = roleCategories[i].tag;
                        thisEventAttenders[noOfEventAttender].eventAttenderRole.description = roleCategories[i].description;
                        thisEventAttenders[noOfEventAttender].eventAttenderRole.level = 0;
                        noOfEventAttender++;
                    }
                }
                if (!(foundCategory))
                {
                    addRoleCategory(role, "", "Unclassified");
                    thisEventAttenders[noOfEventAttender].eventAttenderRole.tag = role;
                    thisEventAttenders[noOfEventAttender].eventAttenderRole.description = "";
                    thisEventAttenders[noOfEventAttender].eventAttenderRole.level = 1;
                    noOfEventAttender++;
                }
                retVal = true;
            }
            return retVal;
        }
        public int getNoOfEventImages() { return this.noOfEventImages; }
        public int getNumberOfContentDescription() { return this.noOfDescriptionLines; }
        public string getEventContentDescription(int nr) { return this.descriptionOfEvent[nr]; }
        public bool addEventContentDescription(string indata)
        {
            if (noOfDescriptionLines < maxNoOfDescriptionLines)
            {
                descriptionOfEvent[noOfDescriptionLines++] = indata;
                return true;
            }
            else
                return false;
        }
        public string getEventImageContentDescription(int nr) { return this.thisEventImages[nr].eventImageContent.description; }
        public string getEventImageName(int nr)
        {
            string eipn = this.thisEventImages[nr].eventImagePathName;
            if (eipn != null)
            {
                int dp0 = eipn.LastIndexOf("\\");
                string strimn = this.thisEventImages[nr].eventImagePathName.Substring(dp0 + 1, this.thisEventImages[nr].eventImagePathName.Length - dp0 - 1);
                return strimn;
            }
            else
                return "";
        }
        public string getEventImagePathName(int nr) { return this.thisEventImages[nr].eventImagePathName; }
        public string getEventImageLevel(int nr) { return this.thisEventImages[nr].eventImageLevel.ToString(); }
        public string getEventImageContentTag(int nr)
        {
            if (this.thisEventImages[nr].eventImageContent.tag != null)
                return this.thisEventImages[nr].eventImageContent.tag;
            else
                return "Not Set";
        }
        public bool addEventImage(string pathname, string level, string tag, string description)
        {
            bool retVal = false;
            if (noOfEventImages < maxNoOfEventImages)
            {
                thisEventImages[noOfEventImages].eventImagePathName = pathname;
                if (!(checkEnumeration(level, noOfEventImages)))
                    thisEventImages[noOfEventImages].eventImageLevel = eventClassificationType.Undefined;
                bool foundTag = false;
                for (int i = 0; i < noOfContentCategories; i++)
                {
                    if (contentCategories[i].tag == tag)
                    {
                        thisEventImages[noOfEventImages].eventImageContent = contentCategories[i];
                        foundTag = true;
                    }
                }
                if (!(foundTag))
                {
                    addContentCategory(tag, description, "");
                    thisEventImages[noOfEventImages].eventImageContent.tag = tag;
                    thisEventImages[noOfEventImages].eventImageContent.description = description;
                    noOfContentCategories++;
                }
                noOfEventImages++;
                retVal = true;
            }
            return retVal;
        }
        public int getEventImageContentLevel(int nr) { return this.thisEventImages[nr].eventImageContent.level; }
        public int getMaxNoOfEventRoots() { return maxNoOfEventRootDirs; }
        public int getNoOfEventRoots() { return noOfEventRootDirs; }
        public string getEventRoot(int nr) { return eventRootDirs[nr]; }
        public bool addEventRoot(int nr, string value)
        {
            if (nr < maxNoOfEventRootDirs)
            {
                eventRootDirs[nr] = value;
                if (nr >= noOfEventRootDirs)
                    noOfEventRootDirs = nr + 1;
                return true;
            }
            return false;
        }
    }
}