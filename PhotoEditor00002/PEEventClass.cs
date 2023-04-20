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
        Undefined, Time, Hours, Minutes, Seconds
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
    #endregion
    class PEEventClass
    {
        #region programParameters
        private string eventId;
        private eventClassificationType classificationLevel;

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
        #endregion
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
        public void loadEvent(string eventID, string storagePath)
        {
            string filename = "";
            if ((storagePath != "") && (System.IO.Directory.Exists(storagePath)) && 
                (System.IO.File.Exists(storagePath + "\\EventData_" + eventID + ".edf")))
                filename = storagePath + "\\EventData_" + eventID + ".edf";
            else
            {
                string scu = WindowsIdentity.GetCurrent().Name;
                string sProgPath = "source\\repos\\PhotoEditor00002\\PhotoEditor00002"; // TODO - Remove this
                int tnr = scu.IndexOf("\\");
                if ((tnr > 0) && (tnr < scu.Length - 1))
                    scu = scu.Substring(tnr + 1, scu.Length - tnr - 1);
                string rootPath = "C:\\Users\\" + scu + "\\" + sProgPath;
                filename = rootPath + "\\EventData\\EventData_" + eventID + ".edf";
            }

            if (System.IO.File.Exists(filename))
            { 
                foreach (string line in System.IO.File.ReadLines(filename))
                {
                    if (line != "-1")
                    {
                        int dp0, dp1, dp2, dp3;
                        string dataTag = line.Substring(0, 8);
                        string dataInfo = line.Substring(11, line.Length - 11);
                        switch (dataTag)
                        {
                            case "Identity":
                                {
                                    this.eventId = dataInfo;
                                } break;
                            case "Secrecy ":
                                {
                                    if ((dataInfo == "Unclassified") || (dataInfo == "1"))
                                        this.classificationLevel = eventClassificationType.Unclassified;
                                    else if ((dataInfo == "Limited") || (dataInfo == "2"))
                                        this.classificationLevel = eventClassificationType.Limited;
                                    else if ((dataInfo == "Confidential") || (dataInfo == "3"))
                                        this.classificationLevel = eventClassificationType.Confidential;
                                    else if ((dataInfo == "Secret") || (dataInfo == "4"))
                                        this.classificationLevel = eventClassificationType.Secret;
                                    else if ((dataInfo == "QualifSecret") || (dataInfo == "5"))
                                        this.classificationLevel = eventClassificationType.QualifSecret;
                                    else
                                        this.classificationLevel = eventClassificationType.Undefined;
                                } break;
                            case "EvtOwner":
                                {
                                    this.eventOwner = dataInfo;
                                } break;
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
                                            else
                                                this.eventStarted.duration = (float)(int.Parse(strValue));
                                            string strType = dataInfo.Substring(dp1 + 2, dataInfo.Length - dp1 - 2);
                                            if ((strType == "time") || (strType == "Time"))
                                                this.eventStarted.durtyp = durationTypeDef.Time;
                                            else if ((strType == "hours") || (strType == "Hours"))
                                                this.eventStarted.durtyp = durationTypeDef.Hours;
                                            else if ((strType == "Minutes") || (strType == "minutes"))
                                                this.eventStarted.durtyp = durationTypeDef.Minutes;
                                            else if ((strType == "seconds") || (strType == "Seconds"))
                                                this.eventStarted.durtyp = durationTypeDef.Seconds;
                                            else
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
                                } break;
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
                                                else
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
                                            else
                                                this.eventEnded.duration = (float)(int.Parse(strValue));
                                            string strType = dataInfo.Substring(dp1 + 2, dataInfo.Length - dp1 - 2);
                                            if ((strType == "time") || (strType == "Time"))
                                                this.eventEnded.durtyp = durationTypeDef.Time;
                                            else if ((strType == "hours") || (strType == "Hours"))
                                                this.eventEnded.durtyp = durationTypeDef.Hours;
                                            else if ((strType == "Minutes") || (strType == "minutes"))
                                                this.eventEnded.durtyp = durationTypeDef.Minutes;
                                            else if ((strType == "seconds") || (strType == "Seconds"))
                                                this.eventEnded.durtyp = durationTypeDef.Seconds;
                                            else
                                                this.eventEnded.durtyp = durationTypeDef.Undefined;
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
                                } break;
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
                                                    if ((dataInfo == "South") || (dataInfo == "south"))
                                                        this.geoPos.latDir = eventGeographicDirection.South;
                                                    else
                                                        this.geoPos.latDir = eventGeographicDirection.North;
                                                }
                                            }
                                        }

                                    }
                                } break;
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
                                                    if ((dataInfo == "East") || (dataInfo == "east"))
                                                        this.geoPos.lonDir = eventGeographicDirection.East;
                                                    else
                                                        this.geoPos.latDir = eventGeographicDirection.West;
                                                }
                                            }
                                        }

                                    }
                                } break;
                            case "GeoName ":
                                {
                                    // GeoName  : <geographical name> 
                                    this.geoPos.geographicName = dataInfo;
                                } break;
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
                                } break;
                            case "EvtCity ":
                                {
                                    // EvtCity  : <cityname>
                                    if ((dataInfo != "Undefined") && (dataInfo != "Unknown") && (dataInfo != "undefined") && (dataInfo != "unknown"))
                                        this.theEventAddress.cityname = dataInfo;
                                } break;
                            case "evtAreaN":
                                {
                                    // EvtAreaN : <Areaname>
                                    if ((dataInfo != "Undefined") && (dataInfo != "Unknown") && (dataInfo != "undefined") && (dataInfo != "unknown"))
                                        this.theEventAddress.areaname = dataInfo;
                                } break;
                            case "EvtAreaC":
                                {
                                    // EvtAreaC : <Areacode>
                                    if ((dataInfo != "Undefined") && (dataInfo != "Unknown") && (dataInfo != "undefined") && (dataInfo != "unknown"))
                                        this.theEventAddress.areacode = dataInfo;
                                } break;
                            case "EvtCntry":
                                {
                                    // EvtCntry : <Countryname>
                                    if ((dataInfo != "Undefined") && (dataInfo != "Unknown") && (dataInfo != "undefined") && (dataInfo != "unknown"))
                                        this.theEventAddress.countryname = dataInfo;
                                } break;
                            case "EvtType":
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
                                } break;
                            case "Headline":
                                {
                                    // Headline : <Descriptive event headline>
                                    if ((dataInfo != "Undefined") && (dataInfo != "Unknown") && (dataInfo != "undefined") && (dataInfo != "unknown"))
                                        this.headlineDescription = dataInfo;
                                } break;
                            case "EvtAttdr":
                                {
                                    // {EvtAttdr : <Attender Id>; <level>; <role>}
                                    if (noOfEventAttender < maxNoOfEventAttender)
                                    {
                                        dp0 = dataInfo.IndexOf(";");
                                        if ((dp0 > 0) && (dp0 < dataInfo.Length - 2))
                                        {
                                            // <Attender Id>; <level>; <role>
                                            string struid = dataInfo.Substring(0, dp0);
                                            if ((struid != "Undefined") && (struid != "Unknown") && (struid != "undefined") && (struid != "unknown"))
                                                this.thisEventAttenders[noOfEventAttender].eventAttenderId = struid;
                                            dataInfo = dataInfo.Substring(dp0 + 2, dataInfo.Length - dp0 - 2);
                                            dp1 = dataInfo.IndexOf(";");
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
                                } break;
                            case "EvtImage":
                                {
                                    // {EvtImage : <Image path and name>; <type of content>; <level>}
                                    if (noOfEventImages < maxNoOfEventImages)
                                    {
                                        dp0 = dataInfo.IndexOf(";");
                                        if ((dp0 > 0) && (dp0 < dataInfo.Length - 2))
                                        {
                                            string strimpn = dataInfo.Substring(0, dp0);
                                            if ((strimpn != "Unknown") && (strimpn != "Undefined") && (strimpn != "unknown") && (strimpn != "undefined"))
                                                this.thisEventImages[noOfEventImages].eventImagePathName = strimpn;
                                            dataInfo = dataInfo.Substring(dp0 + 2, dataInfo.Length - dp0 - 2);
                                            dp1 = dataInfo.IndexOf(";");
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
                                                if (dataInfo == "Unclassified")
                                                    this.thisEventImages[noOfEventImages].eventImageLevel = eventClassificationType.Unclassified;
                                                else if (dataInfo == "Limited")
                                                    this.thisEventImages[noOfEventImages].eventImageLevel = eventClassificationType.Limited;
                                                else if (dataInfo == "Confidential")
                                                    this.thisEventImages[noOfEventImages].eventImageLevel = eventClassificationType.Confidential;
                                                else if (dataInfo == "Secret")
                                                    this.thisEventImages[noOfEventImages].eventImageLevel = eventClassificationType.Secret;
                                                else if ((dataInfo == "QualifSecret") || (dataInfo == "Qualif") || (dataInfo == "qualif"))
                                                    this.thisEventImages[noOfEventImages].eventImageLevel = eventClassificationType.QualifSecret;
                                                else
                                                    this.thisEventImages[noOfEventImages].eventImageLevel = eventClassificationType.Undefined;
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
                                } break;
                            case "Content ":
                                {
                                    // {Content  : {<Content tags>}}
                                    if (noOfEventContentTags < maxNoOfEventContentTags)
                                    {
                                        this.thisEventContentTags[noOfEventContentTags] = dataInfo;
                                        noOfEventContentTags++;
                                    }
                                } break;
                            case "Descrptn":
                                {
                                    // Descrptn : <Description of the event>
                                    if (noOfDescriptionLines < maxNoOfDescriptionLines)
                                    {
                                        this.descriptionOfEvent[noOfDescriptionLines] = dataInfo;
                                        noOfDescriptionLines++;
                                    }
                                } break;
                            default:
                                {
                                    // Erroneous branch, should not be called.
                                } break;
                        }
                    }
                }
            }
        }
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
                    var line = "Identity : " + this.eventId;
                    sw.WriteLine(line);
                    line = "Secrecy  : " + this.classificationLevel.ToString();
                    sw.WriteLine(line);
                    if (this.eventOwner != "")
                    {
                        line = "EvtOwner : " + this.eventOwner;
                        sw.WriteLine(line);
                    }
                    if ((this.eventStarted.month > 0) && (this.eventStarted.day > 0))
                    {
                        line = "Started  : " + this.eventStarted.year.ToString();
                        if (this.eventStarted.month > 9)
                            line = line + "-" + this.eventStarted.month.ToString();
                        else
                            line = line + "-0" + this.eventStarted.month.ToString();
                        if (this.eventStarted.day > 9)
                            line = line + "-" + this.eventStarted.day.ToString();
                        else
                            line = line + "-0" + this.eventStarted.day.ToString();
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
                    if ((this.eventEnded.month > 0) && (this.eventEnded.day > 0))
                    {
                        line = "Ended    : " + this.eventEnded.year.ToString();
                        if (this.eventEnded.month > 9)
                            line = line + "-" + this.eventEnded.month.ToString();
                        else
                            line = line + "-0" + this.eventEnded.month.ToString();
                        if (this.eventEnded.day > 9)
                            line = line + "-" + this.eventEnded.day.ToString();
                        else
                            line = line + "-0" + this.eventEnded.day.ToString();
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
                    if (this.geoPos.lonDeg >= 0)
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
                    if (this.geoPos.geographicName != "")
                    {
                        line = "GeoName  : " + this.geoPos.geographicName;
                        sw.WriteLine(line);
                    }
                    if (this.theEventAddress.streetname != "")
                    {
                        line = "EvtStrt  : " + this.theEventAddress.streetname + "; " + this.theEventAddress.housenumber.ToString() + "; " + this.theEventAddress.housenumberaddon;
                        sw.WriteLine(line);
                    }
                    if (this.theEventAddress.cityname != "")
                    {
                        line = "EvtCity  : " + this.theEventAddress.cityname;
                        sw.WriteLine(line);
                    }
                    if (this.theEventAddress.areaname != "")
                    {
                        line = "evtAreaN : " + this.theEventAddress.areaname;
                        sw.WriteLine(line);
                    }
                    if (this.theEventAddress.areacode != "")
                    {
                        line = "EvtAreaC : " + this.theEventAddress.areacode;
                        sw.WriteLine(line);
                    }
                    if (this.theEventAddress.countryname != "")
                    {
                        line = "EvtCntry : " + this.theEventAddress.countryname;
                        sw.WriteLine(line);
                    }
                    if (this.typeOfEvent.tag != "")
                    {
                        line = "EvtType : ";
                        line = line + this.typeOfEvent.tag;
                        if (this.typeOfEvent.description != "")
                        {
                            line = line + "; " + this.typeOfEvent.description;
                            line = line + "; " + this.typeOfEvent.level.ToString();
                        }
                        sw.WriteLine(line);
                    }
                    if (this.headlineDescription != "")
                    {
                        line = "Headline : " + this.headlineDescription;
                        sw.WriteLine(line);
                    }
                    if (this.noOfEventAttender > 0)
                    {
                        for (int i = 0; i < this.noOfEventAttender; i++)
                        {
                            line = "EvtAttdr : " + this.thisEventAttenders[i].eventAttenderId +
                                "; " + this.thisEventAttenders[i].eventAttenderLevel.ToString() +
                                "; " + this.thisEventAttenders[i].eventAttenderRole.tag;
                            sw.WriteLine(line);
                        }
                    }
                    if (this.noOfEventImages > 0)
                    {
                        for (int i = 0; i < this.noOfEventImages; i++)
                        {
                            line = "EvtImage : " + this.thisEventImages[i].eventImagePathName +
                                "; " + this.thisEventImages[i].eventImageContent.tag +
                                "; " + this.thisEventImages[i].eventImageLevel.ToString();
                            sw.WriteLine(line);
                        }
                    }
                    if (this.noOfEventContentTags > 0)
                    {
                        for (int i = 0; i < this.noOfEventContentTags; i++)
                        {
                            line = "Content  : " + this.thisEventContentTags[i];
                            sw.WriteLine(line);
                        }
                    }
                    if (this.noOfDescriptionLines > 0)
                    {
                        for (int i = 0; i < this.noOfDescriptionLines; i++)
                        {
                            line = "Descrptn : " + this.descriptionOfEvent[i];
                        }
                    }
                    sw.Close();
                }
                efs.Close();
            }
            return retVal;
        }
        public string getEventOwner() { return this.eventOwner; }
        public void setEventOwner(string nyOwner) { this.eventOwner = nyOwner; }
        public string getEventLevel() { return this.classificationLevel.ToString(); }
        public bool setEventLevel(string nyEventLevel)
        {
            if ((nyEventLevel == "Unclassified") || (nyEventLevel == "1"))
            {
                this.classificationLevel = eventClassificationType.Unclassified;
                return true;
            }
            else if ((nyEventLevel == "Limited") || (nyEventLevel == "2"))
            {
                this.classificationLevel = eventClassificationType.Limited;
                return true;
            }
            else if ((nyEventLevel == "Confidential") || (nyEventLevel == "3"))
            {
                this.classificationLevel = eventClassificationType.Confidential;
                return true;
            }
            else if ((nyEventLevel == "Secret") || (nyEventLevel == "4"))
            {
                this.classificationLevel = eventClassificationType.Secret;
                return true;
            }
            else if ((nyEventLevel == "QualifSecret") || (nyEventLevel == "5"))
            {
                this.classificationLevel = eventClassificationType.QualifSecret;
                return true;
            }
            else
            {
                this.classificationLevel = eventClassificationType.Undefined;
                return false;
            }
        }
        public string getEventStarted()
        { 
            if ((this.eventStarted.month > 9) && (this.eventStarted.day > 9))
                return this.eventStarted.year.ToString() + "-" + this.eventStarted.month.ToString() + "-" + this.eventStarted.day + " " + this.eventStarted.duration.ToString() + " " + this.eventStarted.durtyp.ToString(); 
            else if (this.eventStarted.month > 9)
                return this.eventStarted.year.ToString() + "-" + this.eventStarted.month.ToString() + "-0" + this.eventStarted.day + " " + this.eventStarted.duration.ToString() + " " + this.eventStarted.durtyp.ToString();
            else if (this.eventStarted.day > 9)
                return this.eventStarted.year.ToString() + "-0" + this.eventStarted.month.ToString() + "-" + this.eventStarted.day + " " + this.eventStarted.duration.ToString() + " " + this.eventStarted.durtyp.ToString();
            else
                return this.eventStarted.year.ToString() + "-0" + this.eventStarted.month.ToString() + "-0" + this.eventStarted.day + " " + this.eventStarted.duration.ToString() + " " + this.eventStarted.durtyp.ToString();
        }
        public string getEventEnded()
        {
            if ((this.eventEnded.month > 9) && (this.eventEnded.day > 9))
                return this.eventEnded.year.ToString() + "-" + this.eventEnded.month.ToString() + "-" + this.eventEnded.day + " " + this.eventEnded.duration.ToString() + " " + this.eventEnded.durtyp.ToString();
            else if (this.eventEnded.month > 9)
                return this.eventEnded.year.ToString() + "-" + this.eventEnded.month.ToString() + "-0" + this.eventEnded.day + " " + this.eventEnded.duration.ToString() + " " + this.eventEnded.durtyp.ToString();
            else if (this.eventEnded.day > 9)
                return this.eventEnded.year.ToString() + "-0" + this.eventEnded.month.ToString() + "-" + this.eventEnded.day + " " + this.eventEnded.duration.ToString() + " " + this.eventEnded.durtyp.ToString();
            else
                return this.eventEnded.year.ToString() + "-0" + this.eventEnded.month.ToString() + "-0" + this.eventEnded.day + " " + this.eventEnded.duration.ToString() + " " + this.eventEnded.durtyp.ToString();
        }
        public string getEventHeadline() { return this.headlineDescription; }
        public string getEventLatPos()
        {
            if ((this.geoPos.latMin > 9) && (this.geoPos.latSec > 9) && (this.geoPos.latSemiSec > 9))
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
        public string getEventLonPos()
        {
            if ((this.geoPos.lonMin > 9) && (this.geoPos.lonSec > 9) && (this.geoPos.lonSemiSec > 9))
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
        public string getEventGeoPosName() { return this.geoPos.geographicName; }
        public string getEventStreetname() { return this.theEventAddress.streetname + " " + this.theEventAddress.housenumber + this.theEventAddress.housenumberaddon + " " + this.theEventAddress.floornumber; }
        public string getEventCityname() { return this.theEventAddress.cityname; }
        public string getEventAreaname() { return this.theEventAddress.areaname; }
        public string getEventAreacode() { return this.theEventAddress.areacode; }
        public string getEventStatename() { return this.theEventAddress.statename; }
        public string getEventCountryname() { return this.theEventAddress.countryname; }
        public int getNoOfEventAttender() { return this.noOfEventAttender; }
        public string getEventAttenderID(int nr) 
        {
            if ((nr > 0) && (nr < this.noOfEventAttender))
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
        public int getNoOfEventImages() { return this.noOfEventImages; }
        public string getEventImageName(int nr)
        {
            int dp0 = this.thisEventImages[nr].eventImagePathName.LastIndexOf("\\");
            string strimn = this.thisEventImages[nr].eventImagePathName.Substring(dp0 + 1, this.thisEventImages[nr].eventImagePathName.Length - dp0 - 1);
            return strimn;
        }
        public string getEventImagePathName(int nr) { return this.thisEventImages[nr].eventImagePathName; }
        public string getEventImageLevel(int nr) { return this.thisEventImages[nr].eventImageLevel.ToString(); }
        public string getEventImageContentTag(int nr) { return this.thisEventImages[nr].eventImageContent.tag; }
        public string getEventImageContentDescription(int nr) { return this.thisEventImages[nr].eventImageContent.description; }
        public int getEventImageContentLevel(int nr) { return this.thisEventImages[nr].eventImageContent.level; }
    }
}