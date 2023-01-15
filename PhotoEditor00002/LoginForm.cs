using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Claims;
using System.Security.Principal;

namespace myLoginForm
{
    public partial class LoginForm : Form
    {
        #region EnumAndStructDef
        public enum rightsLevel
        {
            Undefined, Open, Limited, Medium, Relative, Secret, QualifSecret
        }
        public struct categoryDefinition
        {
            public string tag;
            public string description;
            public int prefixNumber;
            public float value;
            public rightsLevel catReqReq;
        }
        #endregion
        #region ParameterDef
        public bool addingNewUser = false;
        public bool editingExistingUser = false;
        public bool valuesChanged = true;
        public bool validated = false;
        public string userId;
        public string userPassword;
        public rightsLevel userRightsLevel;
        public int mainWinX;
        public int mainWinY;
        public int imageWinX;
        public int imageWinY;
        public int mainProgTopSide;
        public int mainProgLeftSide;
        public int mainProgRightSide;
        public int mainProgBottomSide;
        public int smallImageWidth;
        public int smallImageHeight;
        public int largeImageWidth;
        public int largeImageHeight;
        public const int maxNoOfImageCategories = 150;
        public int noOfImageCategories;
        public categoryDefinition[] imageCategories = new categoryDefinition[maxNoOfImageCategories];
        public const int maxNoOfEventCategories = 150;
        public int noOfEventCategories;
        public categoryDefinition[] eventCategories = new categoryDefinition[maxNoOfEventCategories];
        public const int maxNoOfContentCategories = 150;
        public int noOfContentCategories;
        public categoryDefinition[] contentCategories = new categoryDefinition[maxNoOfContentCategories];
        public const int maxNoOfRelationCategories = 75;
        public int noOfRelationCategories;
        public categoryDefinition[] relationCategories = new categoryDefinition[maxNoOfRelationCategories];
        public const int maxNoOfNationalities = 75;
        public int noOfNationalities;
        public categoryDefinition[] nationalityCategories = new categoryDefinition[maxNoOfNationalities];
        public const int maxNoOfCurrencies = 75;
        public int noOfCurrencies;
        public categoryDefinition[] currencyCategories = new categoryDefinition[maxNoOfCurrencies];
        public const int maxNoOfContactWays = 75;
        public int noOfContactWays;
        public categoryDefinition[] contactWays = new categoryDefinition[maxNoOfContactWays];
        public const int maxNoOfComplexions = 75;
        public int noOfComplexions;
        public categoryDefinition[] complexionCategories = new categoryDefinition[maxNoOfComplexions];
        public const int maxNoOfHairColors = 75;
        public int noOfHairColors;
        public categoryDefinition[] hairColorsDefined = new categoryDefinition[maxNoOfHairColors];
        public const int maxNoOfEyeColors = 75;
        public int eyeColors;
        public categoryDefinition[] eyeColorsDefined = new categoryDefinition[maxNoOfEyeColors];
        public const int maxNoOfRoles = 150;
        public int noOfRoles;
        public categoryDefinition[] rolesDefined = new categoryDefinition[maxNoOfRoles];
        #endregion

        public LoginForm()
        {
            InitializeComponent();
            this.ClientSize = new System.Drawing.Size(345, 465);
        }

        private void getUserData(string userid, string userpwd, object sender, EventArgs e)
        {
            string scu = WindowsIdentity.GetCurrent().Name;
            string sProgPath = "source\\repos\\PhotoEditor00002\\PhotoEditor00002\\userData";
            int tnr = scu.IndexOf("\\");
            if ((tnr > 0) && (tnr < scu.Length - 1))
                scu = scu.Substring(tnr + 1, scu.Length - tnr - 1);
            string sRootPath = "C:\\Users\\" + scu + "\\" + sProgPath;
            string filename = sRootPath + "\\initVal_" + userid + ".udf";
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
                            case "userId  ":
                                {
                                    this.userId = dataInfo;
                                }
                                break;
                            case "userPaswd":
                                {
                                    this.userPassword = dataInfo;
                                }
                                break;
                            case "UsrRight":
                                {
                                    if ((dataInfo == "Open") || (dataInfo == "open"))
                                        this.userRightsLevel = rightsLevel.Open;
                                    else if ((dataInfo == "Limited") || (dataInfo == "limited"))
                                        this.userRightsLevel = rightsLevel.Limited;
                                    else if ((dataInfo == "Medium") || (dataInfo == "medium"))
                                        this.userRightsLevel = rightsLevel.Medium;
                                    else if ((dataInfo == "Relative") || (dataInfo == "relative"))
                                        this.userRightsLevel = rightsLevel.Relative;
                                    else if ((dataInfo == "Secret") || (dataInfo == "secret"))
                                        this.userRightsLevel = rightsLevel.Secret;
                                    else if ((dataInfo == "QualifSecret") || (dataInfo == "qualifsecret"))
                                        this.userRightsLevel = rightsLevel.QualifSecret;
                                    else
                                        this.userRightsLevel = rightsLevel.Undefined;
                                }
                                break;
                            case "TopSide ":
                                {
                                    int.TryParse(dataInfo, out this.mainProgTopSide);
                                }
                                break;
                            case "LeftSide":
                                {
                                    int.TryParse(dataInfo, out this.mainProgLeftSide);
                                }
                                break;
                            case "RightSid":
                                {
                                    int.TryParse(dataInfo, out this.mainProgRightSide);
                                }
                                break;
                            case "BottomSi":
                                {
                                    int.TryParse(dataInfo, out this.mainProgBottomSide);
                                }
                                break;
                            case "SmImgWdt":
                                {
                                    int.TryParse(dataInfo, out this.smallImageWidth);
                                }
                                break;
                            case "SmImgHgt":
                                {
                                    int.TryParse(dataInfo, out this.smallImageHeight);
                                }
                                break;
                            case "LgImgWdt":
                                {
                                    int.TryParse(dataInfo, out this.largeImageWidth);
                                }
                                break;
                            case "LgImgHgt":
                                {
                                    int.TryParse(dataInfo, out this.largeImageHeight);
                                }
                                break;
                            case "ImgCateg":
                                {
                                    if (this.noOfImageCategories < maxNoOfImageCategories)
                                    {
                                        // <tag>; <description>; <level>
                                        dp0 = dataInfo.IndexOf(";");
                                        if ((dp0 > 0) && (dp0 < dataInfo.Length - 2))
                                        {
                                            string imgCatTag = dataInfo.Substring(0, dp0);
                                            if ((imgCatTag != "Undefined") && (imgCatTag != "Unknown"))
                                                this.imageCategories[this.noOfImageCategories].tag = imgCatTag;
                                            dataInfo = dataInfo.Substring(dp0 + 2, dataInfo.Length - dp0 - 2);
                                            // <description>; <level>
                                            dp1 = dataInfo.IndexOf(";");
                                            if ((dp1 > 0) && (dp1 < dataInfo.Length - 2))
                                            {
                                                this.imageCategories[this.noOfImageCategories].description = dataInfo.Substring(0, dp1);
                                                dataInfo = dataInfo.Substring(dp1 + 2, dataInfo.Length - dp1 - 2);
                                                // <level>
                                                if ((dataInfo == "Open") || (dataInfo == "open"))
                                                    this.imageCategories[this.noOfImageCategories].catReqReq = rightsLevel.Open;
                                                else if ((dataInfo == "Limited") || (dataInfo == "limited"))
                                                    this.imageCategories[this.noOfImageCategories].catReqReq = rightsLevel.Limited;
                                                else if ((dataInfo == "Medium") || (dataInfo == "medium"))
                                                    this.imageCategories[this.noOfImageCategories].catReqReq = rightsLevel.Medium;
                                                else if ((dataInfo == "Relative") || (dataInfo == "relative"))
                                                    this.imageCategories[this.noOfImageCategories].catReqReq = rightsLevel.Relative;
                                                else if ((dataInfo == "Secret") || (dataInfo == "secret"))
                                                    this.imageCategories[this.noOfImageCategories].catReqReq = rightsLevel.Secret;
                                                else if ((dataInfo == "QualifSecret") || (dataInfo == "qualifsecret"))
                                                    this.imageCategories[this.noOfImageCategories].catReqReq = rightsLevel.QualifSecret;
                                                else
                                                    this.imageCategories[this.noOfImageCategories].catReqReq = rightsLevel.Undefined;
                                            }
                                        }
                                        this.noOfImageCategories++;
                                    }
                                }
                                break;
                            case "EvtCateg":
                                {
                                    if (this.noOfEventCategories < maxNoOfEventCategories)
                                    {
                                        // <tag>; <explanation>; <level>
                                        dp0 = dataInfo.IndexOf(";");
                                        if ((dp0 > 0) && (dp0 < dataInfo.Length - 2))
                                        {
                                            string evtCatTag = dataInfo.Substring(0, dp0);
                                            if ((evtCatTag != "Undefined") && (evtCatTag != "Unknown"))
                                                this.eventCategories[this.noOfEventCategories].tag = evtCatTag;
                                            dataInfo = dataInfo.Substring(dp0 + 2, dataInfo.Length - dp0 - 2);
                                            // <explanation>; <level>
                                            dp1 = dataInfo.IndexOf(";");
                                            if ((dp1 > 0) && (dp1 < dataInfo.Length - 2))
                                            {
                                                this.eventCategories[this.noOfEventCategories].description = dataInfo.Substring(0, dp1);
                                                dataInfo = dataInfo.Substring(dp1 + 2, dataInfo.Length - dp1 - 2);
                                                // <level>
                                                if ((dataInfo == "Open") || (dataInfo == "open"))
                                                    this.eventCategories[this.noOfEventCategories].catReqReq = rightsLevel.Open;
                                                else if ((dataInfo == "Limited") || (dataInfo == "limited"))
                                                    this.eventCategories[this.noOfEventCategories].catReqReq = rightsLevel.Limited;
                                                else if ((dataInfo == "Medium") || (dataInfo == "medium"))
                                                    this.eventCategories[this.noOfEventCategories].catReqReq = rightsLevel.Medium;
                                                else if ((dataInfo == "Relative") || (dataInfo == "relative"))
                                                    this.eventCategories[this.noOfEventCategories].catReqReq = rightsLevel.Relative;
                                                else if ((dataInfo == "Secret") || (dataInfo == "secret"))
                                                    this.eventCategories[this.noOfEventCategories].catReqReq = rightsLevel.Secret;
                                                else if ((dataInfo == "QualifSecret") || (dataInfo == "qualifsecret"))
                                                    this.eventCategories[this.noOfEventCategories].catReqReq = rightsLevel.QualifSecret;
                                                else
                                                    this.eventCategories[this.noOfEventCategories].catReqReq = rightsLevel.Undefined;
                                            }
                                        }
                                        this.noOfEventCategories++;
                                    }
                                }
                                break;
                            case "ContCat ":
                                {
                                    // ContCat  : <Content tag>; <Content description>; <level>
                                    if (this.noOfContentCategories < maxNoOfContentCategories)
                                    {
                                        dp0 = dataInfo.IndexOf(";");
                                        if ((dp0 > 0) && (dp0 < dataInfo.Length - 2))
                                        {
                                            // <tag>; <explanation>; <level>
                                            string cntCatTag = dataInfo.Substring(0, dp0);
                                            if ((cntCatTag != "Undefined") && (cntCatTag != "Unknown"))
                                                this.contentCategories[this.noOfContentCategories].tag = cntCatTag;
                                            dataInfo = dataInfo.Substring(dp0 + 2, dataInfo.Length - dp0 - 2);
                                            // <explanation>; <level>
                                            dp1 = dataInfo.IndexOf(";");
                                            if ((dp1 > 0) && (dp1 < dataInfo.Length - 2))
                                            {
                                                this.contentCategories[this.noOfContentCategories].description = dataInfo.Substring(0, dp1);
                                                dataInfo = dataInfo.Substring(dp1 + 2, dataInfo.Length - dp1 - 2);
                                                // <level>
                                                if ((dataInfo == "Open") || (dataInfo == "open"))
                                                    this.contentCategories[this.noOfContentCategories].catReqReq = rightsLevel.Open;
                                                else if ((dataInfo == "Limited") || (dataInfo == "limited"))
                                                    this.contentCategories[this.noOfContentCategories].catReqReq = rightsLevel.Limited;
                                                else if ((dataInfo == "Medium") || (dataInfo == "medium"))
                                                    this.contentCategories[this.noOfContentCategories].catReqReq = rightsLevel.Medium;
                                                else if ((dataInfo == "Relative") || (dataInfo == "relative"))
                                                    this.contentCategories[this.noOfContentCategories].catReqReq = rightsLevel.Relative;
                                                else if ((dataInfo == "Secret") || (dataInfo == "secret"))
                                                    this.contentCategories[this.noOfContentCategories].catReqReq = rightsLevel.Secret;
                                                else if ((dataInfo == "QualifSecret") || (dataInfo == "qualifsecret"))
                                                    this.contentCategories[this.noOfContentCategories].catReqReq = rightsLevel.QualifSecret;
                                                else
                                                    this.contentCategories[this.noOfContentCategories].catReqReq = rightsLevel.Undefined;
                                            }
                                        }
                                        this.noOfContentCategories++;
                                    }
                                }
                                break;
                            case "RelCateg":
                                {
                                    // RelCateg : <Relation type>; <Relation type description>; <level>
                                    if (this.noOfRelationCategories < maxNoOfRelationCategories)
                                    {
                                        dp0 = dataInfo.IndexOf(";");
                                        if ((dp0 > 0) && (dp0 < dataInfo.Length - 2))
                                        {
                                            // <tag>; <explanation>; <level>
                                            string cntCatTag = dataInfo.Substring(0, dp0);
                                            if ((cntCatTag != "Undefined") && (cntCatTag != "Unknown"))
                                                this.relationCategories[this.noOfRelationCategories].tag = cntCatTag;
                                            dataInfo = dataInfo.Substring(dp0 + 2, dataInfo.Length - dp0 - 2);
                                            // <explanation>; <level>
                                            dp1 = dataInfo.IndexOf(";");
                                            if ((dp1 > 0) && (dp1 < dataInfo.Length - 2))
                                            {
                                                this.relationCategories[this.noOfRelationCategories].description = dataInfo.Substring(0, dp1);
                                                dataInfo = dataInfo.Substring(dp1 + 2, dataInfo.Length - dp1 - 2);
                                                // <level>
                                                if ((dataInfo == "Open") || (dataInfo == "open"))
                                                    this.relationCategories[this.noOfRelationCategories].catReqReq = rightsLevel.Open;
                                                else if ((dataInfo == "Limited") || (dataInfo == "limited"))
                                                    this.relationCategories[this.noOfRelationCategories].catReqReq = rightsLevel.Limited;
                                                else if ((dataInfo == "Medium") || (dataInfo == "medium"))
                                                    this.relationCategories[this.noOfRelationCategories].catReqReq = rightsLevel.Medium;
                                                else if ((dataInfo == "Relative") || (dataInfo == "relative"))
                                                    this.relationCategories[this.noOfRelationCategories].catReqReq = rightsLevel.Relative;
                                                else if ((dataInfo == "Secret") || (dataInfo == "secret"))
                                                    this.relationCategories[this.noOfRelationCategories].catReqReq = rightsLevel.Secret;
                                                else if ((dataInfo == "QualifSecret") || (dataInfo == "qualifsecret"))
                                                    this.relationCategories[this.noOfRelationCategories].catReqReq = rightsLevel.QualifSecret;
                                                else
                                                    this.relationCategories[this.noOfRelationCategories].catReqReq = rightsLevel.Undefined;
                                            }
                                        }
                                        this.noOfRelationCategories++;
                                    }
                                }
                                break;
                            case "NatCateg":
                                {
                                    // NatCateg : <country tag>; <country name>; <country number>
                                    if (this.noOfNationalities < maxNoOfNationalities)
                                    {
                                        // <tag>; <name>; <number>
                                        dp0 = dataInfo.IndexOf(";");
                                        if ((dp0 > 0) && (dp0 < dataInfo.Length - 2))
                                        {
                                            string natCatTag = dataInfo.Substring(0, dp0);
                                            if ((natCatTag != "Undefined") && (natCatTag != "Unknown"))
                                                this.nationalityCategories[noOfNationalities].tag = natCatTag;
                                            dataInfo = dataInfo.Substring(dp0 + 2, dataInfo.Length - dp0 - 2);
                                            // <name>; <number>
                                            dp1 = dataInfo.IndexOf(";");
                                            if ((dp1 > 0) && (dp1 < dataInfo.Length - 2))
                                            {
                                                this.nationalityCategories[noOfNationalities].tag = dataInfo.Substring(0, dp1);
                                                dataInfo = dataInfo.Substring(dp1 + 2, dataInfo.Length - dp1 - 2);
                                                if (!(int.TryParse(dataInfo, out this.nationalityCategories[noOfNationalities].prefixNumber)))
                                                    this.nationalityCategories[noOfNationalities].prefixNumber = 0;
                                            }
                                        }
                                        this.noOfNationalities++;
                                    }
                                }
                                break;
                            default:
                                break;
                        }
                    }
                }

                if ((userid == this.userId) && (userpwd == this.userPassword))
                    validated = true;
                else
                    validated = false;
            }
            else
            {
                addingNewUser = true;
                btnEditUserData_Click(sender, e);
            }
            validated = true;
        }

        #region publicMethods
        public string getUserId() { return this.userId; }
        public string getUserRights()
        {
            if (this.userRightsLevel == rightsLevel.Open)
                return "Open";
            else if (this.userRightsLevel == rightsLevel.Limited)
                return "Limited";
            else if (this.userRightsLevel == rightsLevel.Medium)
                return "Medium";
            else if (this.userRightsLevel == rightsLevel.Relative)
                return "Relative";
            else if (this.userRightsLevel == rightsLevel.Secret)
                return "Secret";
            else if (this.userRightsLevel == rightsLevel.QualifSecret)
                return "QualifiedSecret";
            else
                return "Undefined";
        }
        public int getMainWindowTop() { return this.mainProgTopSide; }
        public int getMainWindowLeft() { return this.mainProgLeftSide; }
        public int getMainWindowRight() { return this.mainProgRightSide; }
        public int getMainWindowBottom() { return this.mainProgBottomSide; }
        public int getSmallImageWidth() { return this.smallImageWidth; }
        public int getSmallImageHeight() { return this.smallImageHeight; }
        public int getLargeImageWidth() { return this.largeImageWidth; }
        public int getLargeImageHeight() { return this.largeImageHeight; }
        public int getNoOfImgCat() { return noOfImageCategories; }
        public string getImgCatTag(int nr) { return this.imageCategories[nr].tag; }
        public string getImgCatDescr(int nr) { return this.imageCategories[nr].description; }
        public string getImgCatLevel(int nr)
        {
            if (this.imageCategories[nr].catReqReq == rightsLevel.Open)
                return "Open";
            else if (this.imageCategories[nr].catReqReq == rightsLevel.Limited)
                return "Limited";
            else if (this.imageCategories[nr].catReqReq == rightsLevel.Medium)
                return "Medium";
            else if (this.imageCategories[nr].catReqReq == rightsLevel.QualifSecret)
                return "Qualified Secret";
            else if (this.imageCategories[nr].catReqReq == rightsLevel.Relative)
                return "Relative";
            else if (this.imageCategories[nr].catReqReq == rightsLevel.Secret)
                return "Secret";
            else
                return "Undefined";
        }
        public int getNoOfEvtCat() { return noOfEventCategories; }
        public string getEvtCatTag(int nr) { return this.eventCategories[nr].tag; }
        public string getEvtCatDescr(int nr) { return this.eventCategories[nr].description; }
        public string getEvtCatLevel(int nr)
        {
            if (this.eventCategories[nr].catReqReq == rightsLevel.Open)
                return "Open";
            else if (this.eventCategories[nr].catReqReq == rightsLevel.Limited)
                return "Limited";
            else if (this.eventCategories[nr].catReqReq == rightsLevel.Medium)
                return "Medium";
            else if (this.eventCategories[nr].catReqReq == rightsLevel.Relative)
                return "Relative";
            else if (this.eventCategories[nr].catReqReq == rightsLevel.Secret)
                return "Secret";
            else if (this.eventCategories[nr].catReqReq == rightsLevel.QualifSecret)
                return "Qualified Secret";
            else
                return "Undefined";
        }
        public int getNoOfContentCategories() { return this.noOfContentCategories; }
        public string getContCatTag(int nr) { return this.contentCategories[nr].tag; }
        public string getContCatDescr(int nr) { return this.contentCategories[nr].description; }
        public string getContCatLevel(int nr)
        {
            if (this.contentCategories[nr].catReqReq == rightsLevel.Open)
                return "Open";
            else if (this.contentCategories[nr].catReqReq == rightsLevel.Relative)
                return "Relative";
            else if (this.contentCategories[nr].catReqReq == rightsLevel.Medium)
                return "Medium";
            else if (this.contentCategories[nr].catReqReq == rightsLevel.Limited)
                return "Limited";
            else if (this.contentCategories[nr].catReqReq == rightsLevel.Secret)
                return "Secret";
            else if (this.contentCategories[nr].catReqReq == rightsLevel.QualifSecret)
                return "Qualified Secret";
            else
                return "Undefined";
        }
        public int getNoOfNationalityCategories() { return this.noOfNationalities; }
        public string getNatCatTag(int nr) { return this.nationalityCategories[nr].tag; }
        public string getNatCatDescr(int nr) { return this.nationalityCategories[nr].description; }
        public int getNatCatPrefix(int nr) { return this.nationalityCategories[nr].prefixNumber; }
        #endregion

        private void txtBxUserIdentity_TextChanged(object sender, EventArgs e)
        {
            txtBxUserPassword.Enabled = true;
            btnEditUserData.Enabled = true;
        }
        private void txtBxUserPassword_TextChanged(object sender, EventArgs e)
        {
            btnCheckCred.Enabled = true;
        }
        private void btnCheckCred_Click(object sender, EventArgs e)
        {
            getUserData(txtBxUserIdentity.Text, txtBxUserPassword.Text, sender, e);
            if (addingNewUser || editingExistingUser)
            {
                // Save all user data
                string scu = WindowsIdentity.GetCurrent().Name;
                string sProgPath = "source\\repos\\PhotoEditor00002\\PhotoEditor00002\\userData";
                int tnr = scu.IndexOf("\\");
                if ((tnr > 0) && (tnr < scu.Length - 1))
                    scu = scu.Substring(tnr + 1, scu.Length - tnr - 1);
                string sRootPath = "C:\\Users\\" + scu + "\\" + sProgPath;
                string filename = sRootPath + "\\initVal_" + this.userId + ".udf";
                using (System.IO.FileStream ufs = System.IO.File.Create(filename))
                {
                    // Write user identity
                    int offs = 0;
                    string tsuid = "userId   : " + this.userId + Environment.NewLine;
                    char[] cuid = tsuid.ToCharArray();
                    ufs.Write(Encoding.ASCII.GetBytes(cuid), offs, cuid.Length);
                    // Write user password
                    offs += cuid.Length;
                    string tsupwd = "usrPaswd : " + this.userPassword + Environment.NewLine;
                    char[] cupwd = tsupwd.ToCharArray();
                    ufs.Write(Encoding.ASCII.GetBytes(cupwd), offs, cupwd.Length);
                    // Write user rights
                    offs += cupwd.Length;
                    string tsusrrgt = "UsrRight : " + this.userRightsLevel.ToString() + Environment.NewLine;
                    char[] cusrrgt = tsusrrgt.ToCharArray();
                    ufs.Write(Encoding.ASCII.GetBytes(cusrrgt), offs, cusrrgt.Length);
                    // Write main window x
                    offs += cusrrgt.Length;
                    string setXPos = this.mainWinX.ToString();
                    int iXPos = 0;
                    if (int.TryParse(setXPos, out iXPos))
                        this.mainWinX = iXPos;
                    string sMainXPos = "MainXPos : " + setXPos + Environment.NewLine;
                    char[] cmxp = sMainXPos.ToCharArray();
                    ufs.Write(Encoding.ASCII.GetBytes(cmxp), offs, cmxp.Length);
                    // Write main window y
                    offs += cmxp.Length;
                    string setYPos = this.mainWinY.ToString();
                    int iYPos = 0;
                    if (int.TryParse(setYPos, out iYPos))
                        this.mainWinY = iYPos;
                    string sMainYPos = "MainYPos : " + setYPos + Environment.NewLine;
                    char[] smyp = sMainYPos.ToCharArray();
                    ufs.Write(Encoding.ASCII.GetBytes(smyp), offs, smyp.Length);
                    // Write main window top side
                    offs += smyp.Length;
                    offs += cmxp.Length;
                    string tstwp = "TopSide  : " + this.mainProgTopSide.ToString() + Environment.NewLine;
                    char[] ctwp = tstwp.ToCharArray();
                    ufs.Write(Encoding.ASCII.GetBytes(ctwp), offs, ctwp.Length);
                    // Write main window left side
                    offs += ctwp.Length;
                    string tslwp = "LeftSide : " + this.mainProgLeftSide.ToString() + Environment.NewLine;
                    char[] clwp = tslwp.ToCharArray();
                    ufs.Write(Encoding.ASCII.GetBytes(clwp), offs, clwp.Length);
                    // Write main window right side
                    offs += clwp.Length;
                    string tsrwp = "RightSid : " + this.mainProgRightSide.ToString() + Environment.NewLine;
                    char[] crwp = tsrwp.ToCharArray();
                    ufs.Write(Encoding.ASCII.GetBytes(crwp), offs, crwp.Length);
                    // Write "SmImgWdt : "
                    offs += crwp.Length;
                    string tssiw = "SmImgHgt : " + this.smallImageWidth.ToString() + Environment.NewLine;
                    char[] csiw = tssiw.ToCharArray();
                    ufs.Write(Encoding.ASCII.GetBytes(csiw), offs, csiw.Length);
                    // Write "SmImgHgt : "
                    offs += csiw.Length;
                    string tssih = "SmImgHgt : " + this.smallImageHeight.ToString() + Environment.NewLine;
                    char[] csih = tssih.ToCharArray();
                    ufs.Write(Encoding.ASCII.GetBytes(csih), offs, csih.Length);
                    // Write "LgImgWdt : "
                    offs += csih.Length;
                    string tsliw = "LgImgWdt : " + this.largeImageWidth.ToString() + Environment.NewLine;
                    char[] cliw = tsliw.ToCharArray();
                    ufs.Write(Encoding.ASCII.GetBytes(cliw), offs, cliw.Length);
                    // Write "LgImgHgt : "
                    offs += cliw.Length;
                    string tslih = "LgImgHgt : " + this.largeImageHeight.ToString() + Environment.NewLine;
                    char[] clih = tslih.ToCharArray();
                    ufs.Write(Encoding.ASCII.GetBytes(clih), offs, clih.Length);
                    // Write "ImgCateg : " loop of format <tag>; <description>; <level>
                    offs += clih.Length;
                    for (int i = 0; i < noOfImageCategories; i++)
                    {
                        string tsimc = "ImgCateg : " + this.imageCategories[i].tag + "; " + this.imageCategories[i].description + "; " + this.imageCategories[i].catReqReq.ToString() + Environment.NewLine;
                        char[] cimc = tsimc.ToCharArray();
                        ufs.Write(Encoding.ASCII.GetBytes(cimc), offs, cimc.Length);
                        offs += cimc.Length;
                    }
                    // Write "EvtCateg : " loop of format <tag>; <description>; <level>
                    for (int i = 0; i < noOfEventCategories; i++)
                    {
                        string tsevc = "EvtCateg : " + this.eventCategories[i].tag + "; " + this.eventCategories[i].description + "; " + this.eventCategories[i].catReqReq.ToString() + Environment.NewLine;
                        char[] cevc = tsevc.ToCharArray();
                        ufs.Write(Encoding.ASCII.GetBytes(cevc), offs, cevc.Length);
                        offs += cevc.Length;
                    }
                    // Write "contcat  : " loop of rormat <tag>; <description>; <level>
                    for (int i = 0; i < noOfContentCategories; i++)
                    {
                        string tscnc = "ContCat  : " + this.contentCategories[i].tag + "; " + this.contentCategories[i].description + "; " + this.contentCategories[i].catReqReq.ToString() + Environment.NewLine;
                        char[] ccnc = tscnc.ToCharArray();
                        ufs.Write(Encoding.ASCII.GetBytes(ccnc), offs, ccnc.Length);
                        offs += ccnc.Length;
                    }
                    // Write "RelCateg : " loop of format <tag>; <description>; <level>
                    for (int i = 0; i < noOfRelationCategories; i++)
                    {
                        string tsrelc = "RelCateg : " + this.relationCategories[i].tag + "; " + this.relationCategories[i].description + "; " + this.relationCategories[i].catReqReq.ToString() + Environment.NewLine;
                        char[] crelc = tsrelc.ToCharArray();
                        ufs.Write(Encoding.ASCII.GetBytes(crelc), offs, crelc.Length);
                        offs += crelc.Length;
                    }
                    // Write "NatCateg : " loop of format <tag>; <description>; <prefix>
                    for (int i = 0; i < noOfNationalities; i++)
                    {
                        string tsnat = "NatCateg : " + this.nationalityCategories[i].tag + ";" + this.nationalityCategories[i].description + "; " + this.nationalityCategories[i].prefixNumber.ToString() + Environment.NewLine;
                        char[] cnat = tsnat.ToCharArray();
                        ufs.Write(Encoding.ASCII.GetBytes(cnat), offs, cnat.Length);
                        offs += cnat.Length;
                    }
                    // Write "Currency : " loop of format <tag>; <description>; <float value>
                    for (int i = 0; i < noOfCurrencies; i++)
                    {
                        string tscurr = "Currency : " + this.currencyCategories[i].tag + "; " + this.currencyCategories[i].description + "; " + this.currencyCategories[i].value.ToString() + Environment.NewLine;
                        char[] ccurr = tscurr.ToCharArray();
                        ufs.Write(Encoding.ASCII.GetBytes(ccurr), offs, ccurr.Length);
                        offs += ccurr.Length;
                    }
                    // Write "Contacts : " loop of format <tag>; <description>; <level>
                    // Write "Complexn : " loop of format <tag>; <description>; <level>
                    // Write "HairColr : " loop of format <tag>; <description>; <level>
                    // Write "EyeColor : " loop of format <tag>; <description>; <level>
                    // Write "Roles    : " loop of format <tag>; <description>; <level>
                }
            }
            this.Hide();
        }
        private void btnEditUserData_Click(object sender, EventArgs e)
        {
            getUserData(txtBxUserIdentity.Text, txtBxUserPassword.Text, sender, e);
            cmbBxExistingMotifTypes.Items.Add("Select...");
            cmbBxExistingEventTypes.Items.Add("Select...");
            cmbBxExistingContentTypes.Items.Add("Select...");
            cmbBxExistingRelationTypes.Items.Add("Select...");
            cmbBxExistingNationalityTypes.Items.Add("Select...");
            if (validated)
            {
                editingExistingUser = true;
                if (this.userRightsLevel == rightsLevel.Undefined)
                    cmbbxUserRights.SelectedIndex = 0;
                else if (this.userRightsLevel == rightsLevel.Open)
                    cmbbxUserRights.SelectedIndex = 1;
                else if (this.userRightsLevel == rightsLevel.Limited)
                    cmbbxUserRights.SelectedIndex = 2;
                else if (this.userRightsLevel == rightsLevel.Medium)
                    cmbbxUserRights.SelectedIndex = 3;
                else if (this.userRightsLevel == rightsLevel.Relative)
                    cmbbxUserRights.SelectedIndex = 4;
                else if (this.userRightsLevel == rightsLevel.Secret)
                    cmbbxUserRights.SelectedIndex = 5;
                else if (this.userRightsLevel == rightsLevel.QualifSecret)
                    cmbbxUserRights.SelectedIndex = 6;
                else
                    cmbbxUserRights.SelectedIndex = 0;
                nudTopBorder.Value = this.mainProgTopSide;
                nudLeftBorder.Value = this.mainProgLeftSide;
                nudRightBorder.Value = this.mainProgRightSide;
                nudBottomBorder.Value = this.mainProgBottomSide;
                nudSmallImageWidth.Value = this.smallImageWidth;
                nudSmallImageHeight.Value = this.smallImageHeight;
                nudLargeImageWidth.Value = this.largeImageWidth;
                nudLargeImageHeight.Value = this.largeImageHeight;
                for (int i = 0; i < noOfImageCategories - 1; i++)
                    cmbBxExistingMotifTypes.Items.Add(this.imageCategories[i].tag);
                cmbBxExistingMotifTypes.SelectedIndex = 0;
                for (int i = 0; i < noOfEventCategories - 1; i++)
                    cmbBxExistingEventTypes.Items.Add(this.eventCategories[i].tag);
                cmbBxExistingEventTypes.SelectedIndex = 0;
                for (int i = 0; i < noOfContentCategories - 1; i++)
                    cmbBxExistingContentTypes.Items.Add(this.contentCategories[i].tag);
                cmbBxExistingContentTypes.SelectedIndex = 0;
                for (int i = 0; i < noOfRelationCategories - 1; i++)
                    cmbBxExistingRelationTypes.Items.Add(this.relationCategories[i].tag);
                cmbBxExistingRelationTypes.SelectedIndex = 0;
                for (int i = 0; i < noOfNationalities - 1; i++)
                    cmbBxExistingNationalityTypes.Items.Add(this.nationalityCategories[i].tag);
                cmbBxExistingNationalityTypes.SelectedIndex = 0;
            }
            cmbBxExistingMotifTypes.Items.Add("Add item...");
            cmbBxExistingEventTypes.Items.Add("Add item...");
            cmbBxExistingContentTypes.Items.Add("Add item...");
            cmbBxExistingRelationTypes.Items.Add("Add item...");
            cmbBxExistingNationalityTypes.Items.Add("Add item...");
            this.ClientSize = new System.Drawing.Size(638, 465);
            btnEditUserData.Enabled = false;
            lblUserRights.Visible = true;
            cmbbxUserRights.Visible = true;
            cmbbxUserRights.Enabled = true;
            lblMainWinSize.Visible = true;
            nudTopBorder.Visible = true;
            nudTopBorder.Enabled = true;
            nudLeftBorder.Visible = true;
            nudLeftBorder.Enabled = true;
            nudRightBorder.Visible = true;
            nudRightBorder.Enabled = true;
            nudBottomBorder.Visible = true;
            nudBottomBorder.Enabled = true;
            lblImageDisplayDimensions.Visible = true;
            lblSmallImage.Visible = true;
            nudSmallImageWidth.Visible = true;
            nudSmallImageWidth.Enabled = true;
            nudSmallImageHeight.Visible = true;
            nudSmallImageHeight.Enabled = true;
            lblLargeImageDimensions.Visible = true;
            nudLargeImageWidth.Enabled = true;
            nudLargeImageWidth.Visible = true;
            nudLargeImageHeight.Enabled = true;
            nudLargeImageHeight.Visible = true;
            lblMotifType.Visible = true;
            cmbBxExistingMotifTypes.Visible = true;
            cmbBxExistingMotifTypes.Enabled = true;
            lblEventType.Visible = true;
            cmbBxExistingEventTypes.Visible = true;
            cmbBxExistingEventTypes.Enabled = true;
            lblContentType.Visible = true;
            cmbBxExistingContentTypes.Enabled = true;
            cmbBxExistingContentTypes.Visible = true;
            lblRelationType.Visible = true;
            cmbBxExistingRelationTypes.Enabled = true;
            cmbBxExistingRelationTypes.Visible = true;
            lblNationalityType.Visible = true;
            cmbBxExistingNationalityTypes.Enabled = true;
            cmbBxExistingNationalityTypes.Visible = true;
            if (addingNewUser)
            {
                this.txtBxMotifTypeAdding.Size = new System.Drawing.Size(205, 20);
                this.txtBxMotifTypeAdding.Enabled = true;
                this.btnAddMotifType.Visible = true;
                this.btnAddMotifType.Enabled = true;
                this.txtbxEventTypeToAdd.Size = new System.Drawing.Size(205, 20);
                this.txtbxEventTypeToAdd.Enabled = true;
                this.btnAddEventType.Visible = true;
                this.btnAddEventType.Enabled = true;
                this.txtBxContentTypeToAdd.Size = new System.Drawing.Size(205, 20);
                this.txtBxContentTypeToAdd.Enabled = true;
                this.btnAddContentType.Visible = true;
                this.btnAddContentType.Enabled = true;
                this.txtBxRelationTypeToAdd.Size = new System.Drawing.Size(205, 20);
                this.txtBxRelationTypeToAdd.Enabled = true;
                this.btnAddRelationType.Visible = true;
                this.btnAddRelationType.Enabled = true;
                this.txtBxNationalityTypeToAdd.Size = new System.Drawing.Size(205, 20);
                this.txtBxNationalityTypeToAdd.Enabled = true;
                this.btnAddNationalityType.Visible = true;
                this.btnAddNationalityType.Enabled = true;
            }
            else
            {
                this.txtBxMotifTypeAdding.Size = new System.Drawing.Size(263, 20);
                this.txtBxMotifTypeAdding.Enabled = false;
                this.btnAddMotifType.Visible = false;
                this.txtbxEventTypeToAdd.Size = new System.Drawing.Size(263, 20);
                this.txtbxEventTypeToAdd.Enabled = false;
                this.btnAddEventType.Visible = false;
                this.txtBxContentTypeToAdd.Size = new System.Drawing.Size(263, 20);
                this.txtBxContentTypeToAdd.Enabled = false;
                this.btnAddContentType.Visible = false;
                this.txtBxRelationTypeToAdd.Size = new System.Drawing.Size(263, 20);
                this.txtBxRelationTypeToAdd.Enabled = false;
                this.btnAddRelationType.Visible = false;
                this.txtBxNationalityTypeToAdd.Size = new System.Drawing.Size(263, 20);
                this.txtBxNationalityTypeToAdd.Enabled = false;
                this.btnAddNationalityType.Visible = false;
            }
            this.txtBxMotifTypeAdding.Visible = true;
            this.txtbxEventTypeToAdd.Visible = true;
            this.txtBxContentTypeToAdd.Visible = true;
            this.txtBxRelationTypeToAdd.Visible = true;
            this.txtBxNationalityTypeToAdd.Visible = true;
            // tänd allt som skall finnas!
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selImagePos = comboBox1.Text;
            if (selImagePos == "Image to the left")
            {
                this.imageWinX = 9;
                this.imageWinY = 26;
            }
            else if (selImagePos == "Image to the right")
            {
                this.imageWinX = 331;
                this.imageWinY = 26;
            }
            else
            {
                this.imageWinX = 9;
                this.imageWinY = 26;
            }
            valuesChanged = true;
        }
        private void nudMainWinX_ValueChanged(object sender, EventArgs e)
        {
            // TODO - Handle the X-position setting for the main window.
            valuesChanged = true;
        }
        private void nudMainWinY_ValueChanged(object sender, EventArgs e)
        {
            // TODO - Handle the Y-position setting for the main window.
            valuesChanged = true;
        }
        private void cmbbxUserRights_SelectedIndexChanged(object sender, EventArgs e)
        {
            // TODO - Get the selected index, fetch the data and put that in the textbox.
            string selUserRights = cmbbxUserRights.Text;
            if (selUserRights == "Open")
                this.userRightsLevel = rightsLevel.Open;
            else if (selUserRights == "Limited")
                this.userRightsLevel = rightsLevel.Limited;
            else if (selUserRights == "Medium")
                this.userRightsLevel = rightsLevel.Medium;
            else if (selUserRights == "Relative")
                this.userRightsLevel = rightsLevel.Relative;
            else if (selUserRights == "Secret")
                this.userRightsLevel = rightsLevel.Secret;
            else if (selUserRights == "QualifiedSecret")
                this.userRightsLevel = rightsLevel.QualifSecret;
            else
                this.userRightsLevel = rightsLevel.Undefined;
            valuesChanged = true;
        }
        private void nudTopBorder_ValueChanged(object sender, EventArgs e)
        {
            // Do nothing, changes are handled in the save button.
            valuesChanged = true;
        }
        private void nudLeftBorder_ValueChanged(object sender, EventArgs e)
        {
            // Do nothing, changes are handled in the save button.
            valuesChanged = true;
        }
        private void nudRightBorder_ValueChanged(object sender, EventArgs e)
        {
            // Do nothing, changes are handled in the save button.
            valuesChanged = true;
        }
        private void nudBottomBorder_ValueChanged(object sender, EventArgs e)
        {
            // Do nothing, changes are handled in the save button.
            valuesChanged = true;
        }
        private void nudSmallImageWidth_ValueChanged(object sender, EventArgs e)
        {
            // Do nothing, changes are handled in the save button.
            valuesChanged = true;
        }
        private void nudSmallImageHeight_ValueChanged(object sender, EventArgs e)
        {
            // Do nothing, changes are handled in the save button.
            valuesChanged = true;
        }
        private void nudLargeImageWidth_ValueChanged(object sender, EventArgs e)
        {
            // Do nothing, changes are handled in the save button.
            valuesChanged = true;
        }
        private void nudLargeImageHeight_ValueChanged(object sender, EventArgs e)
        {
            // Do nothing, changes are handled in the save button.
            valuesChanged = true;
        }
        private void cmbBxExistingMotifTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selIndex = cmbBxExistingMotifTypes.SelectedIndex;
            if (selIndex > 0)
            {
                if ((cmbBxExistingMotifTypes.SelectedItem.ToString() == "Add Item...") || ((selIndex - 1) > noOfImageCategories))
                {
                    //          Set textbox width to small.
                    txtBxMotifTypeAdding.Text = "";
                    txtBxMotifTypeAdding.SetBounds(356, 216, 204, 20);
                    txtBxMotifTypeAdding.Enabled = true;
                    //          Lit the button.
                    btnAddMotifType.Visible = true;
                    btnAddMotifType.Enabled = true;
                }
                else
                {
                    txtBxMotifTypeAdding.Text = imageCategories[selIndex - 1].tag + ", " + imageCategories[selIndex - 1].description + ", " + imageCategories[selIndex - 1].catReqReq;
                    txtBxMotifTypeAdding.SetBounds(356, 216, 263, 20);
                }
            }
        }
        private void txtBxMotifTypeAdding_TextChanged(object sender, EventArgs e)
        {
            // Do nothing, this is handled in btnAddMotifType_Click.
        }
        private void btnAddMotifType_Click(object sender, EventArgs e)
        {
            string theNewInfo = txtBxMotifTypeAdding.Text;
            int dp0 = theNewInfo.IndexOf(",");
            if ((dp0 > 0) && (dp0 < theNewInfo.Length - 2))
            {
                string theNewTag = theNewInfo.Substring(0, dp0);
                if ((theNewTag != "Undefined") && (theNewTag != "Unknown") && (theNewTag != "undefined") && (theNewTag != "unknown"))
                    imageCategories[noOfImageCategories].tag = theNewTag;
                theNewInfo = theNewInfo.Substring(dp0 + 2, theNewInfo.Length - dp0 - 2);
                int dp1 = theNewInfo.IndexOf(",");
                if ((dp1 > 0) && (dp1 < theNewInfo.Length - 2))
                {
                    string theNewDescription = theNewInfo.Substring(0, dp1);
                    if ((theNewDescription != "Undefined") && (theNewDescription != "Unknown") && (theNewDescription != "undefined") && (theNewDescription != "unknown"))
                        imageCategories[noOfImageCategories].description = theNewDescription;
                    theNewInfo = theNewInfo.Substring(dp1 + 2, theNewInfo.Length - dp1 - 2);
                    if ((theNewInfo == "Open") || (theNewInfo == "open"))
                        imageCategories[noOfImageCategories].catReqReq = rightsLevel.Open;
                    else if ((theNewInfo == "Limited") || (theNewInfo == "limited"))
                        imageCategories[noOfImageCategories].catReqReq = rightsLevel.Limited;
                    else if ((theNewInfo == "Medium") || (theNewInfo == "medium"))
                        imageCategories[noOfImageCategories].catReqReq = rightsLevel.Medium;
                    else if ((theNewInfo == "Relative") || (theNewInfo == "relative"))
                        imageCategories[noOfImageCategories].catReqReq = rightsLevel.Relative;
                    else if ((theNewInfo == "Secret") || (theNewInfo == "secret"))
                        imageCategories[noOfImageCategories].catReqReq = rightsLevel.Secret;
                    else if ((theNewInfo == "Qualified") || (theNewInfo == "qualified") || (theNewInfo == "QualifiedSecret") || (theNewInfo == "qualifiedsecret"))
                        imageCategories[noOfImageCategories].catReqReq = rightsLevel.QualifSecret;
                    else
                        imageCategories[noOfImageCategories].catReqReq = rightsLevel.Undefined;
                }
                noOfImageCategories++;
                txtBxMotifTypeAdding.Text = "";
                txtBxMotifTypeAdding.SetBounds(356, 216, 263, 20);
                txtBxMotifTypeAdding.Enabled = false;
                btnAddMotifType.Enabled = false;
                btnAddMotifType.Visible = false;
                cmbBxExistingMotifTypes.Items.Clear();
                cmbBxExistingMotifTypes.Items.Add("Select...");
                for (int i = 0; i < noOfImageCategories - 1; i++)
                    cmbBxExistingMotifTypes.Items.Add(imageCategories[i].tag);
                cmbBxExistingMotifTypes.Items.Add("Add Item...");
                cmbBxExistingMotifTypes.SelectedIndex = 0;
                valuesChanged = true;
            }
            else
            {
                dp0 = theNewInfo.IndexOf(";");
                if ((dp0 > 0) && (dp0 < theNewInfo.Length - 2))
                {
                    string theNewTag = theNewInfo.Substring(0, dp0);
                    if ((theNewTag != "Undefined") && (theNewTag != "Unknown") && (theNewTag != "undefined") && (theNewTag != "unknown"))
                        imageCategories[noOfImageCategories].tag = theNewTag;
                    theNewInfo = theNewInfo.Substring(dp0 + 2, theNewInfo.Length - dp0 - 2);
                    int dp1 = theNewInfo.IndexOf(";");
                    if ((dp1 > 0) && (dp1 < theNewInfo.Length - 2))
                    {
                        string theNewDescription = theNewInfo.Substring(0, dp1);
                        if ((theNewDescription != "Undefined") && (theNewDescription != "Unknown") && (theNewDescription != "undefined") && (theNewDescription != "unknown"))
                            imageCategories[noOfImageCategories].description = theNewDescription;
                        theNewInfo = theNewInfo.Substring(dp1 + 2, theNewInfo.Length - dp1 - 2);
                        if ((theNewInfo == "Open") || (theNewInfo == "open"))
                            imageCategories[noOfImageCategories].catReqReq = rightsLevel.Open;
                        else if ((theNewInfo == "Limited") || (theNewInfo == "limited"))
                            imageCategories[noOfImageCategories].catReqReq = rightsLevel.Limited;
                        else if ((theNewInfo == "Medium") || (theNewInfo == "medium"))
                            imageCategories[noOfImageCategories].catReqReq = rightsLevel.Medium;
                        else if ((theNewInfo == "Relative") || (theNewInfo == "relative"))
                            imageCategories[noOfImageCategories].catReqReq = rightsLevel.Relative;
                        else if ((theNewInfo == "Secret") || (theNewInfo == "secret"))
                            imageCategories[noOfImageCategories].catReqReq = rightsLevel.Secret;
                        else if ((theNewInfo == "Qualified") || (theNewInfo == "qualified") || (theNewInfo == "QualifiedSecret") || (theNewInfo == "qualifiedsecret"))
                            imageCategories[noOfImageCategories].catReqReq = rightsLevel.QualifSecret;
                        else
                            imageCategories[noOfImageCategories].catReqReq = rightsLevel.Undefined;
                    }
                    noOfImageCategories++;
                    txtBxMotifTypeAdding.Text = "";
                    txtBxMotifTypeAdding.SetBounds(356, 216, 263, 20);
                    txtBxMotifTypeAdding.Enabled = false;
                    btnAddMotifType.Enabled = false;
                    btnAddMotifType.Visible = false;
                    cmbBxExistingMotifTypes.Items.Clear();
                    cmbBxExistingMotifTypes.Items.Add("Select...");
                    for (int i = 0; i < noOfImageCategories - 1; i++)
                        cmbBxExistingMotifTypes.Items.Add(imageCategories[i].tag);
                    cmbBxExistingMotifTypes.Items.Add("Add Item...");
                    cmbBxExistingMotifTypes.SelectedIndex = 0;
                    valuesChanged = true;
                }
                else
                {
                    MessageBox.Show("Expected format is \"<tag>[,|;] <explanation>[,|;] <level>", "Format info", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }
        private void cmbBxExistingEventTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selIndex = cmbBxExistingEventTypes.SelectedIndex;
            if (selIndex > 0)
            {
                if ((cmbBxExistingEventTypes.SelectedItem.ToString() == "Add Item...") || ((selIndex - 1) > noOfEventCategories))
                {
                    //          Set textbox width to small.
                    txtbxEventTypeToAdd.Text = "";
                    txtbxEventTypeToAdd.SetBounds(356, 266, 204, 20);
                    txtbxEventTypeToAdd.Enabled = true;
                    //          Lit the button.
                    btnAddEventType.Visible = true;
                    btnAddEventType.Enabled = true;
                }
                else
                {
                    txtbxEventTypeToAdd.Text = eventCategories[selIndex - 1].tag + ", " + eventCategories[selIndex - 1].description + ", " + eventCategories[selIndex - 1].catReqReq;
                    txtbxEventTypeToAdd.SetBounds(356, 266, 263, 20);
                }
            }
        }
        private void txtbxEventTypeToAdd_TextChanged(object sender, EventArgs e)
        {
            // Do nothing, this is handled in btnAddEventType_Click.
        }
        private void btnAddEventType_Click(object sender, EventArgs e)
        {
            string theNewInfo = txtbxEventTypeToAdd.Text;
            int dp0 = theNewInfo.IndexOf(",");
            if ((dp0 > 0) && (dp0 < theNewInfo.Length - 2))
            {
                string theNewTag = theNewInfo.Substring(0, dp0);
                if ((theNewTag != "Undefined") && (theNewTag != "Unknown") && (theNewTag != "undefined") && (theNewTag != "unknown"))
                    eventCategories[noOfEventCategories].tag = theNewTag;
                theNewInfo = theNewInfo.Substring(dp0 + 2, theNewInfo.Length - dp0 - 2);
                int dp1 = theNewInfo.IndexOf(",");
                if ((dp1 > 0) && (dp1 < theNewInfo.Length - 2))
                {
                    string theNewDescription = theNewInfo.Substring(0, dp1);
                    if ((theNewDescription != "Undefined") && (theNewDescription != "Unknown") && (theNewDescription != "undefined") && (theNewDescription != "unknown"))
                        eventCategories[noOfEventCategories].description = theNewDescription;
                    theNewInfo = theNewInfo.Substring(dp1 + 2, theNewInfo.Length - dp1 - 2);
                    if ((theNewInfo == "Open") || (theNewInfo == "open"))
                        eventCategories[noOfEventCategories].catReqReq = rightsLevel.Open;
                    else if ((theNewInfo == "Limited") || (theNewInfo == "limited"))
                        eventCategories[noOfEventCategories].catReqReq = rightsLevel.Limited;
                    else if ((theNewInfo == "Medium") || (theNewInfo == "medium"))
                        eventCategories[noOfEventCategories].catReqReq = rightsLevel.Medium;
                    else if ((theNewInfo == "Relative") || (theNewInfo == "relative"))
                        eventCategories[noOfEventCategories].catReqReq = rightsLevel.Relative;
                    else if ((theNewInfo == "Secret") || (theNewInfo == "secret"))
                        eventCategories[noOfEventCategories].catReqReq = rightsLevel.Secret;
                    else if ((theNewInfo == "Qualified") || (theNewInfo == "qualified") || (theNewInfo == "QualifiedSecret") || (theNewInfo == "qualifiedsecret"))
                        eventCategories[noOfEventCategories].catReqReq = rightsLevel.QualifSecret;
                    else
                        eventCategories[noOfEventCategories].catReqReq = rightsLevel.Undefined;
                }
                noOfEventCategories++;
                txtbxEventTypeToAdd.Text = "";
                txtbxEventTypeToAdd.SetBounds(356, 266, 263, 20);
                txtbxEventTypeToAdd.Enabled = false;
                btnAddEventType.Enabled = false;
                btnAddEventType.Visible = false;
                cmbBxExistingEventTypes.Items.Clear();
                cmbBxExistingEventTypes.Items.Add("Select...");
                for (int i = 0; i < noOfEventCategories - 1; i++)
                    cmbBxExistingEventTypes.Items.Add(eventCategories[i].tag);
                cmbBxExistingEventTypes.Items.Add("Add Item...");
                cmbBxExistingEventTypes.SelectedIndex = 0;
                valuesChanged = true;
            }
            else
            {
                dp0 = theNewInfo.IndexOf(";");
                if ((dp0 > 0) && (dp0 < theNewInfo.Length - 2))
                {
                    string theNewTag = theNewInfo.Substring(0, dp0);
                    if ((theNewTag != "Undefined") && (theNewTag != "Unknown") && (theNewTag != "undefined") && (theNewTag != "unknown"))
                        eventCategories[noOfEventCategories].tag = theNewTag;
                    theNewInfo = theNewInfo.Substring(dp0 + 2, theNewInfo.Length - dp0 - 2);
                    int dp1 = theNewInfo.IndexOf(";");
                    if ((dp1 > 0) && (dp1 < theNewInfo.Length - 2))
                    {
                        string theNewDescription = theNewInfo.Substring(0, dp1);
                        if ((theNewDescription != "Undefined") && (theNewDescription != "Unknown") && (theNewDescription != "undefined") && (theNewDescription != "unknown"))
                            eventCategories[noOfEventCategories].description = theNewDescription;
                        theNewInfo = theNewInfo.Substring(dp1 + 2, theNewInfo.Length - dp1 - 2);
                        if ((theNewInfo == "Open") || (theNewInfo == "open"))
                            eventCategories[noOfEventCategories].catReqReq = rightsLevel.Open;
                        else if ((theNewInfo == "Limited") || (theNewInfo == "limited"))
                            eventCategories[noOfEventCategories].catReqReq = rightsLevel.Limited;
                        else if ((theNewInfo == "Medium") || (theNewInfo == "medium"))
                            eventCategories[noOfEventCategories].catReqReq = rightsLevel.Medium;
                        else if ((theNewInfo == "Relative") || (theNewInfo == "relative"))
                            eventCategories[noOfEventCategories].catReqReq = rightsLevel.Relative;
                        else if ((theNewInfo == "Secret") || (theNewInfo == "secret"))
                            eventCategories[noOfEventCategories].catReqReq = rightsLevel.Secret;
                        else if ((theNewInfo == "Qualified") || (theNewInfo == "qualified") || (theNewInfo == "QualifiedSecret") || (theNewInfo == "qualifiedsecret"))
                            eventCategories[noOfEventCategories].catReqReq = rightsLevel.QualifSecret;
                        else
                            eventCategories[noOfEventCategories].catReqReq = rightsLevel.Undefined;
                    }
                    noOfEventCategories++;
                    txtbxEventTypeToAdd.Text = "";
                    txtbxEventTypeToAdd.SetBounds(356, 216, 263, 20);
                    txtbxEventTypeToAdd.Enabled = false;
                    btnAddEventType.Enabled = false;
                    btnAddEventType.Visible = false;
                    cmbBxExistingEventTypes.Items.Clear();
                    cmbBxExistingEventTypes.Items.Add("Select...");
                    for (int i = 0; i < noOfEventCategories - 1; i++)
                        cmbBxExistingEventTypes.Items.Add(eventCategories[i].tag);
                    cmbBxExistingEventTypes.Items.Add("Add Item...");
                    cmbBxExistingEventTypes.SelectedIndex = 0;
                    valuesChanged = true;
                }
                else
                {
                    MessageBox.Show("Expected format is \"<tag>[,|;] <explanation>[,|;] <level>", "Format info", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }
        private void cmbBxExistingContentTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selIndex = cmbBxExistingContentTypes.SelectedIndex;
            if (selIndex > 0)
            {
                if ((cmbBxExistingContentTypes.SelectedItem.ToString() == "Add Item...") || ((selIndex - 1) > noOfContentCategories))
                {
                    //          Set textbox width to small.
                    txtBxContentTypeToAdd.Text = "";
                    txtBxContentTypeToAdd.SetBounds(356, 316, 204, 20);
                    txtBxContentTypeToAdd.Enabled = true;
                    //          Lit the button.
                    btnAddContentType.Visible = true;
                    btnAddContentType.Enabled = true;
                }
                else
                {
                    txtBxContentTypeToAdd.Text = contentCategories[selIndex - 1].tag + ", " + contentCategories[selIndex - 1].description + ", " + contentCategories[selIndex - 1].catReqReq;
                    txtbxEventTypeToAdd.SetBounds(356, 316, 263, 20);
                }
            }
        }
        private void txtBxContentTypeToAdd_TextChanged(object sender, EventArgs e)
        {
            // Do nothing, this is handled in btnAddContentType_Click.
        }
        private void btnAddContentType_Click(object sender, EventArgs e)
        {
            string theNewInfo = txtBxContentTypeToAdd.Text;
            int dp0 = theNewInfo.IndexOf(",");
            if ((dp0 > 0) && (dp0 < theNewInfo.Length - 2))
            {
                string theNewTag = theNewInfo.Substring(0, dp0);
                if ((theNewTag != "Undefined") && (theNewTag != "Unknown") && (theNewTag != "undefined") && (theNewTag != "unknown"))
                    contentCategories[noOfContentCategories].tag = theNewTag;
                theNewInfo = theNewInfo.Substring(dp0 + 2, theNewInfo.Length - dp0 - 2);
                int dp1 = theNewInfo.IndexOf(",");
                if ((dp1 > 0) && (dp1 < theNewInfo.Length - 2))
                {
                    string theNewDescription = theNewInfo.Substring(0, dp1);
                    if ((theNewDescription != "Undefined") && (theNewDescription != "Unknown") && (theNewDescription != "undefined") && (theNewDescription != "unknown"))
                        contentCategories[noOfContentCategories].description = theNewDescription;
                    theNewInfo = theNewInfo.Substring(dp1 + 2, theNewInfo.Length - dp1 - 2);
                    if ((theNewInfo == "Open") || (theNewInfo == "open"))
                        contentCategories[noOfContentCategories].catReqReq = rightsLevel.Open;
                    else if ((theNewInfo == "Limited") || (theNewInfo == "limited"))
                        contentCategories[noOfContentCategories].catReqReq = rightsLevel.Limited;
                    else if ((theNewInfo == "Medium") || (theNewInfo == "medium"))
                        contentCategories[noOfContentCategories].catReqReq = rightsLevel.Medium;
                    else if ((theNewInfo == "Relative") || (theNewInfo == "relative"))
                        contentCategories[noOfContentCategories].catReqReq = rightsLevel.Relative;
                    else if ((theNewInfo == "Secret") || (theNewInfo == "secret"))
                        contentCategories[noOfContentCategories].catReqReq = rightsLevel.Secret;
                    else if ((theNewInfo == "Qualified") || (theNewInfo == "qualified") || (theNewInfo == "QualifiedSecret") || (theNewInfo == "qualifiedsecret"))
                        contentCategories[noOfContentCategories].catReqReq = rightsLevel.QualifSecret;
                    else
                        contentCategories[noOfContentCategories].catReqReq = rightsLevel.Undefined;
                }
                noOfContentCategories++;
                txtBxContentTypeToAdd.Text = "";
                txtBxContentTypeToAdd.SetBounds(356, 316, 263, 20);
                txtBxContentTypeToAdd.Enabled = false;
                btnAddContentType.Enabled = false;
                btnAddContentType.Visible = false;
                cmbBxExistingContentTypes.Items.Clear();
                cmbBxExistingContentTypes.Items.Add("Select...");
                for (int i = 0; i < noOfContentCategories - 1; i++)
                    cmbBxExistingContentTypes.Items.Add(contentCategories[i].tag);
                cmbBxExistingContentTypes.Items.Add("Add Item...");
                cmbBxExistingContentTypes.SelectedIndex = 0;
                valuesChanged = true;
            }
            else
            {
                dp0 = theNewInfo.IndexOf(";");
                if ((dp0 > 0) && (dp0 < theNewInfo.Length - 2))
                {
                    string theNewTag = theNewInfo.Substring(0, dp0);
                    if ((theNewTag != "Undefined") && (theNewTag != "Unknown") && (theNewTag != "undefined") && (theNewTag != "unknown"))
                        contentCategories[noOfContentCategories].tag = theNewTag;
                    theNewInfo = theNewInfo.Substring(dp0 + 2, theNewInfo.Length - dp0 - 2);
                    int dp1 = theNewInfo.IndexOf(";");
                    if ((dp1 > 0) && (dp1 < theNewInfo.Length - 2))
                    {
                        string theNewDescription = theNewInfo.Substring(0, dp1);
                        if ((theNewDescription != "Undefined") && (theNewDescription != "Unknown") && (theNewDescription != "undefined") && (theNewDescription != "unknown"))
                            contentCategories[noOfContentCategories].description = theNewDescription;
                        theNewInfo = theNewInfo.Substring(dp1 + 2, theNewInfo.Length - dp1 - 2);
                        if ((theNewInfo == "Open") || (theNewInfo == "open"))
                            contentCategories[noOfContentCategories].catReqReq = rightsLevel.Open;
                        else if ((theNewInfo == "Limited") || (theNewInfo == "limited"))
                            contentCategories[noOfContentCategories].catReqReq = rightsLevel.Limited;
                        else if ((theNewInfo == "Medium") || (theNewInfo == "medium"))
                            contentCategories[noOfContentCategories].catReqReq = rightsLevel.Medium;
                        else if ((theNewInfo == "Relative") || (theNewInfo == "relative"))
                            contentCategories[noOfContentCategories].catReqReq = rightsLevel.Relative;
                        else if ((theNewInfo == "Secret") || (theNewInfo == "secret"))
                            contentCategories[noOfContentCategories].catReqReq = rightsLevel.Secret;
                        else if ((theNewInfo == "Qualified") || (theNewInfo == "qualified") || (theNewInfo == "QualifiedSecret") || (theNewInfo == "qualifiedsecret"))
                            contentCategories[noOfContentCategories].catReqReq = rightsLevel.QualifSecret;
                        else
                            contentCategories[noOfContentCategories].catReqReq = rightsLevel.Undefined;
                    }
                    noOfContentCategories++;
                    txtBxContentTypeToAdd.Text = "";
                    txtBxContentTypeToAdd.SetBounds(356, 316, 263, 20);
                    txtBxContentTypeToAdd.Enabled = false;
                    btnAddContentType.Enabled = false;
                    btnAddContentType.Visible = false;
                    cmbBxExistingContentTypes.Items.Clear();
                    cmbBxExistingContentTypes.Items.Add("Select...");
                    for (int i = 0; i < noOfContentCategories - 1; i++)
                        cmbBxExistingContentTypes.Items.Add(contentCategories[i].tag);
                    cmbBxExistingContentTypes.Items.Add("Add Item...");
                    cmbBxExistingContentTypes.SelectedIndex = 0;
                    valuesChanged = true;
                }
                else
                {
                    MessageBox.Show("Expected format is \"<tag>[,|;] <explanation>[,|;] <level>", "Format info", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }
        private void cmbBxExistingRelationTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selIndex = cmbBxExistingRelationTypes.SelectedIndex;
            if (selIndex > 0)
            {
                if ((cmbBxExistingRelationTypes.SelectedItem.ToString() == "Add Item...") || ((selIndex - 1) > noOfRelationCategories))
                {
                    //          Set textbox width to small.
                    txtBxRelationTypeToAdd.Text = "";
                    txtBxRelationTypeToAdd.SetBounds(356, 367, 204, 20);
                    txtBxRelationTypeToAdd.Enabled = true;
                    //          Lit the button.
                    btnAddRelationType.Visible = true;
                    btnAddRelationType.Enabled = true;
                }
                else
                {
                    txtBxRelationTypeToAdd.Text = relationCategories[selIndex - 1].tag + ", " + relationCategories[selIndex - 1].description + ", " + relationCategories[selIndex - 1].catReqReq;
                    txtbxEventTypeToAdd.SetBounds(356, 367, 263, 20);
                }
            }
        }
        private void txtBxRelationTypeToAdd_TextChanged(object sender, EventArgs e)
        {
            // Do nothing, this is handled in btnAddRelationType_Click.
        }
        private void btnAddRelationType_Click(object sender, EventArgs e)
        {
            string theNewInfo = txtBxRelationTypeToAdd.Text;
            int dp0 = theNewInfo.IndexOf(",");
            if ((dp0 > 0) && (dp0 < theNewInfo.Length - 2))
            {
                string theNewTag = theNewInfo.Substring(0, dp0);
                if ((theNewTag != "Undefined") && (theNewTag != "Unknown") && (theNewTag != "undefined") && (theNewTag != "unknown"))
                    relationCategories[noOfRelationCategories].tag = theNewTag;
                theNewInfo = theNewInfo.Substring(dp0 + 2, theNewInfo.Length - dp0 - 2);
                int dp1 = theNewInfo.IndexOf(",");
                if ((dp1 > 0) && (dp1 < theNewInfo.Length - 2))
                {
                    string theNewDescription = theNewInfo.Substring(0, dp1);
                    if ((theNewDescription != "Undefined") && (theNewDescription != "Unknown") && (theNewDescription != "undefined") && (theNewDescription != "unknown"))
                        relationCategories[noOfRelationCategories].description = theNewDescription;
                    theNewInfo = theNewInfo.Substring(dp1 + 2, theNewInfo.Length - dp1 - 2);
                    if ((theNewInfo == "Open") || (theNewInfo == "open"))
                        relationCategories[noOfRelationCategories].catReqReq = rightsLevel.Open;
                    else if ((theNewInfo == "Limited") || (theNewInfo == "limited"))
                        relationCategories[noOfRelationCategories].catReqReq = rightsLevel.Limited;
                    else if ((theNewInfo == "Medium") || (theNewInfo == "medium"))
                        relationCategories[noOfRelationCategories].catReqReq = rightsLevel.Medium;
                    else if ((theNewInfo == "Relative") || (theNewInfo == "relative"))
                        relationCategories[noOfRelationCategories].catReqReq = rightsLevel.Relative;
                    else if ((theNewInfo == "Secret") || (theNewInfo == "secret"))
                        relationCategories[noOfRelationCategories].catReqReq = rightsLevel.Secret;
                    else if ((theNewInfo == "Qualified") || (theNewInfo == "qualified") || (theNewInfo == "QualifiedSecret") || (theNewInfo == "qualifiedsecret"))
                        relationCategories[noOfRelationCategories].catReqReq = rightsLevel.QualifSecret;
                    else
                        relationCategories[noOfRelationCategories].catReqReq = rightsLevel.Undefined;
                }
                noOfRelationCategories++;
                txtBxRelationTypeToAdd.Text = "";
                txtBxRelationTypeToAdd.SetBounds(356, 367, 263, 20);
                txtBxRelationTypeToAdd.Enabled = false;
                btnAddRelationType.Enabled = false;
                btnAddRelationType.Visible = false;
                cmbBxExistingRelationTypes.Items.Clear();
                cmbBxExistingRelationTypes.Items.Add("Select...");
                for (int i = 0; i < noOfRelationCategories - 1; i++)
                    cmbBxExistingRelationTypes.Items.Add(relationCategories[i].tag);
                cmbBxExistingRelationTypes.Items.Add("Add Item...");
                cmbBxExistingRelationTypes.SelectedIndex = 0;
                valuesChanged = true;
            }
            else
            {
                dp0 = theNewInfo.IndexOf(";");
                if ((dp0 > 0) && (dp0 < theNewInfo.Length - 2))
                {
                    string theNewTag = theNewInfo.Substring(0, dp0);
                    if ((theNewTag != "Undefined") && (theNewTag != "Unknown") && (theNewTag != "undefined") && (theNewTag != "unknown"))
                        relationCategories[noOfRelationCategories].tag = theNewTag;
                    theNewInfo = theNewInfo.Substring(dp0 + 2, theNewInfo.Length - dp0 - 2);
                    int dp1 = theNewInfo.IndexOf(";");
                    if ((dp1 > 0) && (dp1 < theNewInfo.Length - 2))
                    {
                        string theNewDescription = theNewInfo.Substring(0, dp1);
                        if ((theNewDescription != "Undefined") && (theNewDescription != "Unknown") && (theNewDescription != "undefined") && (theNewDescription != "unknown"))
                            relationCategories[noOfRelationCategories].description = theNewDescription;
                        theNewInfo = theNewInfo.Substring(dp1 + 2, theNewInfo.Length - dp1 - 2);
                        if ((theNewInfo == "Open") || (theNewInfo == "open"))
                            relationCategories[noOfRelationCategories].catReqReq = rightsLevel.Open;
                        else if ((theNewInfo == "Limited") || (theNewInfo == "limited"))
                            relationCategories[noOfRelationCategories].catReqReq = rightsLevel.Limited;
                        else if ((theNewInfo == "Medium") || (theNewInfo == "medium"))
                            relationCategories[noOfRelationCategories].catReqReq = rightsLevel.Medium;
                        else if ((theNewInfo == "Relative") || (theNewInfo == "relative"))
                            relationCategories[noOfRelationCategories].catReqReq = rightsLevel.Relative;
                        else if ((theNewInfo == "Secret") || (theNewInfo == "secret"))
                            relationCategories[noOfRelationCategories].catReqReq = rightsLevel.Secret;
                        else if ((theNewInfo == "Qualified") || (theNewInfo == "qualified") || (theNewInfo == "QualifiedSecret") || (theNewInfo == "qualifiedsecret"))
                            relationCategories[noOfRelationCategories].catReqReq = rightsLevel.QualifSecret;
                        else
                            relationCategories[noOfRelationCategories].catReqReq = rightsLevel.Undefined;
                    }
                    noOfRelationCategories++;
                    txtBxRelationTypeToAdd.Text = "";
                    txtBxRelationTypeToAdd.SetBounds(356, 367, 263, 20);
                    txtBxRelationTypeToAdd.Enabled = false;
                    btnAddRelationType.Enabled = false;
                    btnAddRelationType.Visible = false;
                    cmbBxExistingRelationTypes.Items.Clear();
                    cmbBxExistingRelationTypes.Items.Add("Select...");
                    for (int i = 0; i < noOfRelationCategories - 1; i++)
                        cmbBxExistingRelationTypes.Items.Add(relationCategories[i].tag);
                    cmbBxExistingRelationTypes.Items.Add("Add Item...");
                    cmbBxExistingRelationTypes.SelectedIndex = 0;
                    valuesChanged = true;
                }
                else
                {
                    MessageBox.Show("Expected format is \"<tag>[,|;] <explanation>[,|;] <level>", "Format info", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }
        private void cmbBxExistingNationalityTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selIndex = cmbBxExistingNationalityTypes.SelectedIndex;
            if (selIndex > 0)
            {
                if ((cmbBxExistingNationalityTypes.SelectedItem.ToString() == "Add Item...") || ((selIndex - 1) > noOfNationalities))
                {
                    //          Set textbox width to small.
                    txtBxNationalityTypeToAdd.Text = "";
                    txtBxNationalityTypeToAdd.SetBounds(356, 417, 204, 20);
                    txtBxNationalityTypeToAdd.Enabled = true;
                    //          Lit the button.
                    btnAddNationalityType.Visible = true;
                    btnAddNationalityType.Enabled = true;
                }
                else
                {
                    txtBxNationalityTypeToAdd.Text = nationalityCategories[selIndex - 1].tag + ", " + nationalityCategories[selIndex - 1].description + ", " + nationalityCategories[selIndex - 1].prefixNumber.ToString();
                    txtBxNationalityTypeToAdd.SetBounds(356, 417, 263, 20);
                }
            }
        }
        private void txtBxNationalityTypeToAdd_TextChanged(object sender, EventArgs e)
        {
            // Do nothing, this is handled in btnAddNationalityType_Click.
        }
        private void btnAddNationalityType_Click(object sender, EventArgs e)
        {
            string theNewInfo = txtBxNationalityTypeToAdd.Text;
            int dp0 = theNewInfo.IndexOf(",");
            if ((dp0 > 0) && (dp0 < theNewInfo.Length - 2))
            {
                string theNewTag = theNewInfo.Substring(0, dp0);
                if ((theNewTag != "Undefined") && (theNewTag != "Unknown") && (theNewTag != "undefined") && (theNewTag != "unknown"))
                    nationalityCategories[noOfNationalities].tag = theNewTag;
                theNewInfo = theNewInfo.Substring(dp0 + 2, theNewInfo.Length - dp0 - 2);
                int dp1 = theNewInfo.IndexOf(",");
                if ((dp1 > 0) && (dp1 < theNewInfo.Length - 2))
                {
                    string theNewDescription = theNewInfo.Substring(0, dp1);
                    if ((theNewDescription != "Undefined") && (theNewDescription != "Unknown") && (theNewDescription != "undefined") && (theNewDescription != "unknown"))
                        nationalityCategories[noOfNationalities].description = theNewDescription;
                    theNewInfo = theNewInfo.Substring(dp1 + 2, theNewInfo.Length - dp1 - 2);
                    if ((theNewInfo != "Undefined") && (theNewInfo != "Unknown") && (theNewInfo != "undefined") && (theNewInfo != "unknown"))
                    {
                        int nr = 0;
                        if (int.TryParse(theNewInfo, out nr))
                            nationalityCategories[noOfNationalities].prefixNumber = nr;
                    }
                }
                noOfNationalities++;
                txtBxNationalityTypeToAdd.Text = "";
                txtBxNationalityTypeToAdd.SetBounds(356, 417, 263, 20);
                txtBxNationalityTypeToAdd.Enabled = false;
                btnAddNationalityType.Enabled = false;
                btnAddNationalityType.Visible = false;
                cmbBxExistingNationalityTypes.Items.Clear();
                cmbBxExistingNationalityTypes.Items.Add("Select...");
                for (int i = 0; i < noOfNationalities - 1; i++)
                    cmbBxExistingNationalityTypes.Items.Add(nationalityCategories[i].tag);
                cmbBxExistingNationalityTypes.Items.Add("Add Item...");
                cmbBxExistingNationalityTypes.SelectedIndex = 0;
                valuesChanged = true;
            }
            else
            {
                dp0 = theNewInfo.IndexOf(";");
                if ((dp0 > 0) && (dp0 < theNewInfo.Length - 2))
                {
                    string theNewTag = theNewInfo.Substring(0, dp0);
                    if ((theNewTag != "Undefined") && (theNewTag != "Unknown") && (theNewTag != "undefined") && (theNewTag != "unknown"))
                        nationalityCategories[noOfNationalities].tag = theNewTag;
                    theNewInfo = theNewInfo.Substring(dp0 + 2, theNewInfo.Length - dp0 - 2);
                    int dp1 = theNewInfo.IndexOf(";");
                    if ((dp1 > 0) && (dp1 < theNewInfo.Length - 2))
                    {
                        string theNewDescription = theNewInfo.Substring(0, dp1);
                        if ((theNewDescription != "Undefined") && (theNewDescription != "Unknown") && (theNewDescription != "undefined") && (theNewDescription != "unknown"))
                            nationalityCategories[noOfNationalities].description = theNewDescription;
                        theNewInfo = theNewInfo.Substring(dp1 + 2, theNewInfo.Length - dp1 - 2);
                        if ((theNewInfo != "Undefined") && (theNewInfo != "Unknown") && (theNewInfo != "undefined") && (theNewInfo != "unknown"))
                        {
                            int nr = 0;
                            if (int.TryParse(theNewInfo, out nr))
                                nationalityCategories[noOfNationalities].prefixNumber = nr;
                        }

                    }
                    noOfNationalities++;
                    txtBxNationalityTypeToAdd.Text = "";
                    txtBxNationalityTypeToAdd.SetBounds(356, 417, 263, 20);
                    txtBxNationalityTypeToAdd.Enabled = false;
                    btnAddNationalityType.Enabled = false;
                    btnAddNationalityType.Visible = false;
                    cmbBxExistingNationalityTypes.Items.Clear();
                    cmbBxExistingNationalityTypes.Items.Add("Select...");
                    for (int i = 0; i < noOfNationalities - 1; i++)
                        cmbBxExistingNationalityTypes.Items.Add(nationalityCategories[i].tag);
                    cmbBxExistingNationalityTypes.Items.Add("Add Item...");
                    cmbBxExistingNationalityTypes.SelectedIndex = 0;
                    valuesChanged = true;
                }
                else
                {
                    MessageBox.Show("Expected format is \"<tag>[,|;] <explanation>[,|;] <level>", "Format info", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }
        private void cmbBoxExistingCurrencies_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void txtBxCurrencyToAdd_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnAddCurrency_Click(object sender, EventArgs e)
        {

        }

        private void cmbBxExistingContactWays_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void txtBxContactWaysToAdd_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnAddContactWay_Click(object sender, EventArgs e)
        {

        }

        private void cmbBxExistingComplexions_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void txtBxComplexionToAdd_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnAddComplexion_Click(object sender, EventArgs e)
        {

        }

        private void cmbBxExistingHairColors_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void txtBxHairColorToAdd_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnAddHairColor_Click(object sender, EventArgs e)
        {

        }

        private void cmbBxExistingEyeColors_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void txtBxEyeColorToAdd_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnAddEyeColor_Click(object sender, EventArgs e)
        {

        }

        private void cmbBxExistingRoles_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void txtBxRoleToAdd_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnAddRole_Click(object sender, EventArgs e)
        {

        }
    }
}
