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

using System.IO;
using System.Globalization;

namespace myLoginForm
{
    public partial class LoginForm : Form
    {
        #region EnumAndStructDef
        public enum rightsLevel
        {
            Undefined, Open, Limited, Medium, Relative, Secret, QualifSecret
        }
        public enum imageLeftRightPosition
        {
            Left, right
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
        public imageLeftRightPosition imgPos;
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
        public string actorStoragePath;
        public string eventStoragePath;
        public string lastImageDirectory = "";
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
        public int noOfEyeColors;
        public categoryDefinition[] eyeColorsDefined = new categoryDefinition[maxNoOfEyeColors];
        public const int maxNoOfRoles = 150;
        public int noOfRoles;
        public categoryDefinition[] rolesDefined = new categoryDefinition[maxNoOfRoles];
        #endregion
        public LoginForm()
        {
            actorStoragePath = "";
            eventStoragePath = "";
            InitializeComponent();
            this.ClientSize = new System.Drawing.Size(345, 465);
        }
        private void getUserData(string userid, string userpwd, object sender, EventArgs e)
        {
            if (!(validated))
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
                            int dp0, dp1;
                            string dataTag = line.Substring(0, 8);
                            string dataInfo = line.Substring(11, line.Length - 11);
                            switch (dataTag)
                            {
                                case "userId  ":
                                    {
                                        this.userId = dataInfo;
                                        txtBxUserIdentity.Text = this.userId;
                                    }
                                    break;
                                case "userPaswd":
                                    {
                                        this.userPassword = dataInfo;
                                        txtBxUserPassword.Text = this.userPassword;
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
                                        else if ((dataInfo == "QualifSecret") || (dataInfo == "qualifsecret") || (dataInfo == "QualifiedSecret") || (dataInfo == "qualifiedsecret"))
                                            this.userRightsLevel = rightsLevel.QualifSecret;
                                        else
                                            this.userRightsLevel = rightsLevel.Undefined;
                                    }
                                    break;
                                case "ImgPlacd":
                                    {
                                        if ((dataInfo == "left") || (dataInfo == "Left"))
                                            this.imgPos = imageLeftRightPosition.Left;
                                        else
                                            this.imgPos = imageLeftRightPosition.right;

                                    }
                                    break;
                                case "MainXPos":
                                    {
                                        int varde = 0;
                                        if (int.TryParse(dataInfo, out varde))
                                            this.mainWinX = varde;
                                    }
                                    break;
                                case "MainYPos":
                                    {
                                        int varde = 0;
                                        if (int.TryParse(dataInfo, out varde))
                                            this.mainWinY = varde;
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
                                case "ActrStor":
                                    {
                                        actorStoragePath = dataInfo;
                                    }
                                    break;
                                case "EvntStor":
                                    {
                                        eventStoragePath = dataInfo;
                                    }
                                    break;
                                case "LstImDir":
                                    {
                                        this.lastImageDirectory = dataInfo;
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
        }
        #region publicMethods
        public void saveUserData()
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
                using (System.IO.StreamWriter sw = new System.IO.StreamWriter(ufs))
                {
                    // handle user id changes
                    var line = "userId   : " + txtBxUserIdentity.Text + Environment.NewLine;
                    this.userId = txtBxUserIdentity.Text;
                    sw.Write(line);//Line(line);
                    // Write user password
                    if (txtBxUserPassword.Text != "")
                        this.userPassword = txtBxUserPassword.Text;
                    else
                        this.userPassword = "password";
                    line = "usrPaswd : " + this.userPassword + Environment.NewLine;
                    sw.Write(line);
                    // handle image placement comboBox1 --> imgPos
                    line = "ImgPlacd : " + this.imgPos.ToString() + Environment.NewLine;
                    sw.Write(line);
                    // Handle Actor storage// ActrStor, EvntStor
                    if (this.actorStoragePath != "")
                    {
                        line = "ActrStor : " + this.actorStoragePath + Environment.NewLine;
                        sw.Write(line);
                    }
                    // Handle Event storage
                    if (this.eventStoragePath != "")
                    {
                        line = "EvntStor : " + this.eventStoragePath + Environment.NewLine;
                        sw.Write(line);
                    }
                    // Handle last image directory info
                    if (this.lastImageDirectory != "")
                    {
                        line = "LstImDir : " + this.lastImageDirectory + Environment.NewLine;
                        sw.Write(line);
                    }
                    // handle user rights
                    if ((cmbbxUserRights.SelectedIndex == 1) || (this.userRightsLevel == rightsLevel.Open))
                        this.userRightsLevel = rightsLevel.Open;
                    else if ((cmbbxUserRights.SelectedIndex == 2) || (this.userRightsLevel == rightsLevel.Limited))
                        this.userRightsLevel = rightsLevel.Limited;
                    else if ((cmbbxUserRights.SelectedIndex == 3) || (this.userRightsLevel == rightsLevel.Medium))
                        this.userRightsLevel = rightsLevel.Medium;
                    else if ((cmbbxUserRights.SelectedIndex == 4) || (this.userRightsLevel == rightsLevel.Relative))
                        this.userRightsLevel = rightsLevel.Relative;
                    else if ((cmbbxUserRights.SelectedIndex == 5) || (this.userRightsLevel == rightsLevel.Secret))
                        this.userRightsLevel = rightsLevel.Secret;
                    else if ((cmbbxUserRights.SelectedIndex == 6) || (this.userRightsLevel == rightsLevel.QualifSecret))
                        this.userRightsLevel = rightsLevel.QualifSecret;
                    else
                        this.userRightsLevel = rightsLevel.Undefined;
                    line = "UsrRight : " + this.userRightsLevel.ToString() + Environment.NewLine;
                    sw.Write(line);
                    // Write main window x
                    line = "MainXPos : " + this.mainWinX.ToString() + Environment.NewLine;
                    sw.Write(line);
                    // Write main window y
                    line = "MainYPos : " + this.mainWinY.ToString() + Environment.NewLine;
                    sw.Write(line);
                    // Write main window top side
                    line = "TopSide  : " + this.mainProgTopSide.ToString() + Environment.NewLine;
                    sw.Write(line);
                    // Write main window left side
                    line = "LeftSide : " + this.mainProgLeftSide.ToString() + Environment.NewLine;
                    sw.Write(line);
                    // Write main window right side
                    line = "RightSid : " + this.mainProgRightSide.ToString() + Environment.NewLine;
                    sw.Write(line);
                    // write main window bottom side
                    line = "BottomSi : " + this.mainProgBottomSide.ToString() + Environment.NewLine;
                    sw.Write(line);
                    // Write "SmImgWdt : "
                    line = "SmImgWdt : " + this.smallImageWidth.ToString() + Environment.NewLine;
                    sw.Write(line);
                    // Write "SmImgHgt : "
                    line = "SmImgHgt : " + this.smallImageHeight.ToString() + Environment.NewLine;
                    sw.Write(line);
                    // Write "LgImgWdt : "
                    line = "LgImgWdt : " + this.largeImageWidth.ToString() + Environment.NewLine;
                    sw.Write(line);
                    // Write "LgImgHgt : "
                    line = "LgImgHgt : " + this.largeImageHeight.ToString() + Environment.NewLine;
                    sw.Write(line);
                    // Write user rights
                    line = "UsrRight : " + this.userRightsLevel.ToString() + Environment.NewLine;
                    sw.Write(line);
                    // Write "ImgCateg : " loop of format <tag>; <description>; <level>
                    if (noOfImageCategories > 0)
                    {
                        for (int i = 0; i < noOfImageCategories; i++)
                        {
                            line = "ImgCateg : " + this.imageCategories[i].tag + "; " + this.imageCategories[i].description + "; " + this.imageCategories[i].catReqReq.ToString() + Environment.NewLine;
                            sw.Write(line);
                        }
                    }
                    // Write "EvtCateg : " loop of format <tag>; <description>; <level>
                    if (noOfEventCategories > 0)
                    {
                        for (int i = 0; i < noOfEventCategories; i++)
                        {
                            line = "EvtCateg : " + this.eventCategories[i].tag + "; " + this.eventCategories[i].description + "; " + this.eventCategories[i].catReqReq.ToString() + Environment.NewLine;
                            sw.Write(line);
                        }
                    }
                    // Write "contcat  : " loop of rormat <tag>; <description>; <level>
                    if (noOfContentCategories > 0)
                    {
                        for (int i = 0; i < noOfContentCategories; i++)
                        {
                            line = "ContCat  : " + this.contentCategories[i].tag + "; " + this.contentCategories[i].description + "; " + this.contentCategories[i].catReqReq.ToString() + Environment.NewLine;
                            sw.Write(line);
                        }
                    }
                    // Write "RelCateg : " loop of format <tag>; <description>; <level>
                    if (noOfRelationCategories > 0)
                    {
                        for (int i = 0; i < noOfRelationCategories; i++)
                        {
                            line = "RelCateg : " + this.relationCategories[i].tag + "; " + this.relationCategories[i].description + "; " + this.relationCategories[i].catReqReq.ToString() + Environment.NewLine;
                            sw.Write(line);
                        }
                    }
                    // Write "NatCateg : " loop of format <tag>; <description>; <prefix>
                    if (noOfNationalities > 0)
                    {
                        for (int i = 0; i < noOfNationalities; i++)
                        {
                            line = "NatCateg : " + this.nationalityCategories[i].tag + "; " + this.nationalityCategories[i].description + "; " + this.nationalityCategories[i].prefixNumber.ToString() + Environment.NewLine;
                            sw.Write(line);
                        }
                    }
                    // Write "Currency : " loop of format <tag>; <description>; <float value>
                    if (noOfCurrencies > 0)
                    {
                        for (int i = 0; i < noOfCurrencies; i++)
                        {
                            line = "Currency : " + this.currencyCategories[i].tag + "; " + this.currencyCategories[i].description + "; " + this.currencyCategories[i].value.ToString() + Environment.NewLine;
                            sw.Write(line);
                        }
                    }
                    // Write "Contacts : " loop of format <tag>; <description>; <level>
                    if (noOfContactWays > 0)
                    {
                        for (int i = 0; i < noOfContactWays; i++)
                        {
                            line = "Contacts : " + this.contactWays[i].tag + "; " + this.contactWays[i].description + "; " + this.contactWays[i].catReqReq.ToString() + Environment.NewLine;
                            sw.Write(line);
                        }
                    }
                    // Write "Complexn : " loop of format <tag>; <description>; <level>
                    if (noOfComplexions > 0)
                    {
                        for (int i = 0; i < noOfComplexions; i++)
                        {
                            line = "Complexn : " + this.complexionCategories[i].tag + "; " + this.complexionCategories[i].description + "; " + this.complexionCategories[i].catReqReq.ToString() + Environment.NewLine;
                            sw.Write(line);
                        }
                    }
                    // Write "HairColr : " loop of format <tag>; <description>; <level>
                    if (noOfHairColors > 0)
                    {
                        for (int i = 0; i < noOfHairColors; i++)
                        {
                            line = "HairColr : " + this.hairColorsDefined[i].tag + "; " + this.hairColorsDefined[i].description + "; " + this.hairColorsDefined[i].catReqReq.ToString() + Environment.NewLine;
                            sw.Write(line);
                        }
                    }
                    // Write "EyeColor : " loop of format <tag>; <description>; <level>
                    if (noOfEyeColors > 0)
                    {
                        for (int i = 0; i < noOfEyeColors; i++)
                        {
                            line = "EyeColor : " + this.eyeColorsDefined[i].tag + "; " + this.eyeColorsDefined[i].description + "; " + this.eyeColorsDefined[i].catReqReq.ToString() + Environment.NewLine;
                            sw.Write(line);
                        }
                    }
                    // Write "Roles    : " loop of format <tag>; <description>; <level>
                    if (noOfRoles > 0)
                    {
                        for (int i = 0; i < noOfRoles; i++)
                        {
                            line = "Roles    : " + this.rolesDefined[i].tag + "; " + this.rolesDefined[i].description + "; " + this.rolesDefined[i].catReqReq.ToString() + Environment.NewLine;
                            sw.Write(line);
                        }
                    }
                    sw.Close();
                }
                ufs.Close();
            }
        }
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
        public int getUserRightsValue()
        {
            if (this.userRightsLevel == rightsLevel.Open)
                return 1;
            else if (this.userRightsLevel == rightsLevel.Limited)
                return 2;
            else if (this.userRightsLevel == rightsLevel.Medium)
                return 3;
            else if (this.userRightsLevel == rightsLevel.Relative)
                return 4;
            else if (this.userRightsLevel == rightsLevel.Secret)
                return 5;
            else if (this.userRightsLevel == rightsLevel.QualifSecret)
                return 6;
            else
                return 0;
        }
        public string getLastImageDirectoryValue() { return this.lastImageDirectory; }
        public void setLastImageDirectoryValue(string ind) { this.lastImageDirectory = ind; }
        public string getActorStoragePath() { return this.actorStoragePath; }
        public string getEventStoragePath() { return this.eventStoragePath; }
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
        public int getEvtCatLevelValue(int nr)
        {
            if (this.eventCategories[nr].catReqReq == rightsLevel.Open)
                return 1;
            else if (this.eventCategories[nr].catReqReq == rightsLevel.Limited)
                return 2;
            else if (this.eventCategories[nr].catReqReq == rightsLevel.Medium)
                return 3;
            else if (this.eventCategories[nr].catReqReq == rightsLevel.Relative)
                return 4;
            else if (this.eventCategories[nr].catReqReq == rightsLevel.Secret)
                return 5;
            else if (this.eventCategories[nr].catReqReq == rightsLevel.QualifSecret)
                return 6;
            else
                return 0;        }
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
        public int getNoOfRelationCategories() { return this.noOfRelationCategories; }
        public string getRelationCategoryTag(int nr) { return relationCategories[nr].tag.ToString(); }
        public string getRelationCategoryDescription(int nr) { return relationCategories[nr].description; }
        public string getRelationCategoryLevel(int nr) { return relationCategories[nr].catReqReq.ToString(); }
        public int getNoOfNationalityCategories() { return this.noOfNationalities; }
        public string getNatCatTag(int nr) { return this.nationalityCategories[nr].tag; }
        public string getNatCatDescr(int nr) { return this.nationalityCategories[nr].description; }
        public int getNatCatPrefix(int nr) { return this.nationalityCategories[nr].prefixNumber; }
        public int getNoOfCurrencies() { return this.noOfCurrencies; }
        public string getCurrencyTag(int nr) { return this.currencyCategories[nr].tag; }
        public string getCurrencyDescr(int nr) { return this.currencyCategories[nr].description; }
        public float getCurrencyValue(int nr) { return this.currencyCategories[nr].value; }
        public int getNoOfContactWays() { return this.noOfContactWays; }
        public string getContactWayTag(int nr) { return this.contactWays[nr].tag; }
        public string getContactWayDescr(int nr) { return this.contactWays[nr].description; }
        public string getContactWayLevel(int nr) { return this.contactWays[nr].catReqReq.ToString(); }
        public int getNoOfComplexions() { return this.noOfComplexions; } 
        public string getComplexionTag(int nr) { return this.complexionCategories[nr].tag; }
        public string getComplexionDescr(int nr) { return this.complexionCategories[nr].description; }
        public string getComplexionLevel(int nr) { return this.complexionCategories[nr].catReqReq.ToString(); }
        public int getNoOfHairColors() { return this.noOfHairColors; }
        public string getHairColorTag(int nr) { return this.hairColorsDefined[nr].tag; }
        public string getHairColorDescr(int nr) { return this.hairColorsDefined[nr].description; }
        public string getHairColorLevel(int nr) { return this.hairColorsDefined[nr].catReqReq.ToString(); }
        public int getNoOfEyeColors() { return this.noOfEyeColors; }
        public string getEyeColorTag(int nr) { return this.eyeColorsDefined[nr].tag; }
        public string getEyeColorDescr(int nr) { return this.eyeColorsDefined[nr].description; }
        public string getEyeColorLevel(int nr) { return this.eyeColorsDefined[nr].catReqReq.ToString(); }
        public int getNoOfRoles() { return this.noOfRoles; }
        public string getRoleTag(int nr) { return this.rolesDefined[nr].tag; }
        public string getRoleDescr(int nr) { return this.rolesDefined[nr].description; }
        public string getRoleLevel(int nr) { return this.rolesDefined[nr].catReqReq.ToString(); }
        #endregion
        private void txtBxUserIdentity_TextChanged(object sender, EventArgs e)
        {
            txtBxUserPassword.Enabled = true;
            btnEditUserData.Enabled = true;
        }
        private void txtBxUserPassword_TextChanged(object sender, EventArgs e)
        {
            btnCheckCred.Enabled = true;
            string currChar = txtBxUserPassword.Text;
            if (txtBxUserPassword.Text.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None).Length > 1)
                btnCheckCred_Click(sender, e);
        }
        private void btnCheckCred_Click(object sender, EventArgs e)
        {
            if (!(addingNewUser || editingExistingUser))
                getUserData(txtBxUserIdentity.Text, txtBxUserPassword.Text, sender, e);
            if (valuesChanged)
                saveUserData();
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
            cmbBoxExistingCurrencies.Items.Add("Select...");
            cmbBxExistingContactWays.Items.Add("Select...");
            cmbBxExistingComplexions.Items.Add("Select...");
            cmbBxExistingHairColors.Items.Add("Select...");
            cmbBxExistingEyeColors.Items.Add("Select...");
            cmbBxExistingRoles.Items.Add("Select...");
            if (validated)
            {
                editingExistingUser = true;
                this.ClientSize = new System.Drawing.Size(678, 485);
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
                if (this.imgPos == imageLeftRightPosition.Left)
                    comboBox1.SelectedIndex = 1;
                else
                    comboBox1.SelectedIndex = 2;
                label5.Visible = true;
                comboBox1.Visible = true;
                comboBox1.Enabled = true;
                lblMainWinPos.Visible = true;
                nudMainWinX.Visible = true;
                nudMainWinX.Enabled = true;
                nudMainWinX.Value = this.mainWinX;
                nudMainWinY.Visible = true;
                nudMainWinY.Enabled = true;
                nudMainWinY.Value = this.mainWinY;
                // TODO - Currently don't use main window size changes.
                //nudTopBorder.Value = this.mainProgTopSide;
                //nudLeftBorder.Value = this.mainProgLeftSide;
                //nudRightBorder.Value = this.mainProgRightSide;
                //nudBottomBorder.Value = this.mainProgBottomSide;
                nudSmallImageWidth.Value = this.smallImageWidth;
                nudSmallImageHeight.Value = this.smallImageHeight;
                nudLargeImageWidth.Value = this.largeImageWidth;
                nudLargeImageHeight.Value = this.largeImageHeight;
                for (int i = 0; i < noOfImageCategories - 1; i++)
                    cmbBxExistingMotifTypes.Items.Add(this.imageCategories[i].tag);
                for (int i = 0; i < noOfEventCategories - 1; i++)
                    cmbBxExistingEventTypes.Items.Add(this.eventCategories[i].tag);
                for (int i = 0; i < noOfContentCategories - 1; i++)
                    cmbBxExistingContentTypes.Items.Add(this.contentCategories[i].tag);
                for (int i = 0; i < noOfRelationCategories - 1; i++)
                    cmbBxExistingRelationTypes.Items.Add(this.relationCategories[i].tag);
                for (int i = 0; i < noOfNationalities - 1; i++)
                    cmbBxExistingNationalityTypes.Items.Add(this.nationalityCategories[i].tag);
                for (int i = 0; i < noOfCurrencies - 1; i++)
                    cmbBoxExistingCurrencies.Items.Add(this.currencyCategories[i].tag);
                for (int i = 0; i < noOfContactWays - 1; i++)
                    cmbBxExistingContactWays.Items.Add(this.contactWays[i].tag);
                for (int i = 0; i < noOfComplexions - 1; i++)
                    cmbBxExistingComplexions.Items.Add(this.complexionCategories[i].tag);
                for (int i = 0; i < noOfHairColors - 1; i++)
                    cmbBxExistingHairColors.Items.Add(this.hairColorsDefined[i].tag);
                for (int i = 0; i < noOfEyeColors - 1; i++)
                    cmbBxExistingEyeColors.Items.Add(this.eyeColorsDefined[i].tag);
                for (int i = 0; i < noOfRoles - 1; i++)
                    cmbBxExistingRoles.Items.Add(this.rolesDefined[i].tag);
            }
            cmbBxExistingMotifTypes.Items.Add("Add Item...");
            cmbBxExistingEventTypes.Items.Add("Add Item...");
            cmbBxExistingContentTypes.Items.Add("Add Item...");
            cmbBxExistingRelationTypes.Items.Add("Add Item...");
            cmbBxExistingNationalityTypes.Items.Add("Add Item...");
            cmbBoxExistingCurrencies.Items.Add("Add Item...");
            cmbBxExistingContactWays.Items.Add("Add Item...");
            cmbBxExistingComplexions.Items.Add("Add Item...");
            cmbBxExistingHairColors.Items.Add("Add Item...");
            cmbBxExistingEyeColors.Items.Add("Add Item...");
            cmbBxExistingRoles.Items.Add("Add Item...");
            cmbBxExistingMotifTypes.SelectedIndex = 0;
            cmbBxExistingEventTypes.SelectedIndex = 0;
            cmbBxExistingContentTypes.SelectedIndex = 0;
            cmbBxExistingRelationTypes.SelectedIndex = 0;
            cmbBxExistingNationalityTypes.SelectedIndex = 0;
            cmbBoxExistingCurrencies.SelectedIndex = 0;
            cmbBxExistingContactWays.SelectedIndex = 0;
            cmbBxExistingComplexions.SelectedIndex = 0;
            cmbBxExistingHairColors.SelectedIndex = 0;
            cmbBxExistingEyeColors.SelectedIndex = 0;
            cmbBxExistingRoles.SelectedIndex = 0;
            if (actorStoragePath != "")
            {
                btnActorStorage.Text = "Edit Actor Storage";
                txtBoxSetActorStorage.Text = actorStoragePath;
            }
            else
            {
                btnActorStorage.Text = "Set Actor Storage";
                txtBoxSetActorStorage.Text = "";
            }
            btnActorStorage.Visible = true;
            btnActorStorage.Enabled = true;
            txtBoxSetActorStorage.Visible = true;
            if (eventStoragePath != "")
            {
                btnEventStorage.Text = "Edit Event Storage";
                txtBoxSetActorStorage.Text = eventStoragePath;
            }
            else
            {
                btnEventStorage.Text = "Set Event Storage";
                txtBoxSetEventStorage.Text = "";
            }
            btnEventStorage.Visible = true;
            btnEventStorage.Enabled = true;
            txtBoxSetEventStorage.Visible = true;
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
            lblCurrency.Visible = true;
            cmbBoxExistingCurrencies.Enabled = true;
            cmbBoxExistingCurrencies.Visible = true;
            lblContactWays.Visible = true;
            cmbBxExistingContactWays.Enabled = true;
            cmbBxExistingContactWays.Visible = true;
            lblComplexion.Visible = true;
            cmbBxExistingComplexions.Enabled = true;
            cmbBxExistingComplexions.Visible = true;
            lblHairColor.Visible = true;
            cmbBxExistingHairColors.Enabled = true;
            cmbBxExistingHairColors.Visible = true;
            lblEyeColor.Visible = true;
            cmbBxExistingEyeColors.Enabled = true;
            cmbBxExistingEyeColors.Visible = true;
            lblRoles.Visible = true;
            cmbBxExistingRoles.Enabled = true;
            cmbBxExistingRoles.Visible = true;
            if (addingNewUser)
            {
                this.txtBxMotifTypeAdding.Size = new System.Drawing.Size(220, 20);
                this.txtBxMotifTypeAdding.Enabled = true;
                this.btnAddMotifType.Visible = true;
                this.btnAddMotifType.Enabled = true;
                this.txtbxEventTypeToAdd.Size = new System.Drawing.Size(220, 20);
                this.txtbxEventTypeToAdd.Enabled = true;
                this.btnAddEventType.Visible = true;
                this.btnAddEventType.Enabled = true;
                this.txtBxContentTypeToAdd.Size = new System.Drawing.Size(220, 20);
                this.txtBxContentTypeToAdd.Enabled = true;
                this.btnAddContentType.Visible = true;
                this.btnAddContentType.Enabled = true;
                this.txtBxRelationTypeToAdd.Size = new System.Drawing.Size(220, 20);
                this.txtBxRelationTypeToAdd.Enabled = true;
                this.btnAddRelationType.Visible = true;
                this.btnAddRelationType.Enabled = true;
                this.txtBxNationalityTypeToAdd.Size = new System.Drawing.Size(220, 20);
                this.txtBxNationalityTypeToAdd.Enabled = true;
                this.btnAddNationalityType.Visible = true;
                this.btnAddNationalityType.Enabled = true;
                this.txtBxCurrencyToAdd.Size = new System.Drawing.Size(220, 20);
                this.txtBxCurrencyToAdd.Enabled = true;
                this.btnAddCurrency.Visible = true;
                this.btnAddCurrency.Enabled = true;
                this.txtBxContactWaysToAdd.Size = new System.Drawing.Size(220, 20);
                this.txtBxContactWaysToAdd.Enabled = true;
                this.btnAddContactWay.Visible = true;
                this.btnAddContactWay.Enabled = true;
                this.txtBxComplexionToAdd.Size = new System.Drawing.Size(220, 20);
                this.txtBxComplexionToAdd.Enabled = true;
                this.btnAddComplexion.Visible = true;
                this.btnAddComplexion.Enabled = true;
                this.txtBxHairColorToAdd.Size = new System.Drawing.Size(220, 20);
                this.txtBxHairColorToAdd.Enabled = true;
                this.btnAddHairColor.Visible = true;
                this.btnAddHairColor.Enabled = true;
                this.txtBxEyeColorToAdd.Size = new System.Drawing.Size(220, 20);
                this.txtBxEyeColorToAdd.Enabled = true;
                this.btnAddEyeColor.Visible = true;
                this.btnAddEyeColor.Enabled = true;
                this.txtBxRoleToAdd.Size = new System.Drawing.Size(220, 20);
                this.txtBxRoleToAdd.Enabled = true;
                this.btnAddRole.Visible = true;
                this.btnAddRole.Enabled = true;
            }
            else
            {
                this.txtBxMotifTypeAdding.Size = new System.Drawing.Size(275, 20);
                this.txtBxMotifTypeAdding.Enabled = false;
                this.btnAddMotifType.Visible = false;
                this.txtbxEventTypeToAdd.Size = new System.Drawing.Size(275, 20);
                this.txtbxEventTypeToAdd.Enabled = false;
                this.btnAddEventType.Visible = false;
                this.txtBxContentTypeToAdd.Size = new System.Drawing.Size(257, 20);
                this.txtBxContentTypeToAdd.Enabled = false;
                this.btnAddContentType.Visible = false;
                this.txtBxRelationTypeToAdd.Size = new System.Drawing.Size(275, 20);
                this.txtBxRelationTypeToAdd.Enabled = false;
                this.btnAddRelationType.Visible = false;
                this.txtBxNationalityTypeToAdd.Size = new System.Drawing.Size(275, 20);
                this.txtBxNationalityTypeToAdd.Enabled = false;
                this.btnAddNationalityType.Visible = false;
                this.txtBxCurrencyToAdd.Size = new System.Drawing.Size(275, 20);
                this.txtBxCurrencyToAdd.Enabled = false;
                this.btnAddCurrency.Visible = false;
                this.btnAddCurrency.Enabled = false;
                this.txtBxContactWaysToAdd.Size = new System.Drawing.Size(275, 20);
                this.txtBxContactWaysToAdd.Enabled = false;
                this.btnAddContactWay.Enabled = false;
                this.btnAddContactWay.Visible = false;
                this.txtBxComplexionToAdd.Size = new System.Drawing.Size(275, 20);
                this.txtBxComplexionToAdd.Enabled = false;
                this.btnAddComplexion.Enabled = false;
                this.btnAddComplexion.Visible = false;
                this.txtBxHairColorToAdd.Size = new System.Drawing.Size(275, 20);
                this.txtBxHairColorToAdd.Enabled = false;
                this.btnAddHairColor.Enabled = false;
                this.btnAddHairColor.Visible = false;
                this.txtBxEyeColorToAdd.Size = new System.Drawing.Size(275, 20);
                this.txtBxEyeColorToAdd.Enabled = false;
                this.btnAddEyeColor.Enabled = false;
                this.btnAddEyeColor.Visible = false;
                this.txtBxRoleToAdd.Size = new System.Drawing.Size(275, 20);
                this.txtBxRoleToAdd.Enabled = false;
                this.btnAddRole.Enabled = false;
                this.btnAddRole.Visible = false;
            }
            this.txtBxMotifTypeAdding.Visible = true;
            this.txtbxEventTypeToAdd.Visible = true;
            this.txtBxContentTypeToAdd.Visible = true;
            this.txtBxRelationTypeToAdd.Visible = true;
            this.txtBxNationalityTypeToAdd.Visible = true;
            this.txtBxCurrencyToAdd.Visible = true;
            this.txtBxContactWaysToAdd.Visible = true;
            this.txtBxComplexionToAdd.Visible = true;
            this.txtBxHairColorToAdd.Visible = true;
            this.txtBxEyeColorToAdd.Visible = true;
            this.txtBxRoleToAdd.Visible = true;
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
            this.mainWinX = Convert.ToInt32(nudMainWinX.Value);
            valuesChanged = true;
        }
        private void nudMainWinY_ValueChanged(object sender, EventArgs e)
        {
            // TODO - Handle the Y-position setting for the main window.
            this.mainWinY = Convert.ToInt32(nudMainWinY.Value);
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
            this.mainProgTopSide = Convert.ToInt32(nudTopBorder.Value);
            this.Top = this.mainProgTopSide;
            valuesChanged = true;
        }
        private void nudLeftBorder_ValueChanged(object sender, EventArgs e)
        {
            // Do nothing, changes are handled in the save button.
            this.mainProgLeftSide = Convert.ToInt32(nudLeftBorder.Value);
            this.Left = this.mainProgLeftSide;
            valuesChanged = true;
        }
        private void nudRightBorder_ValueChanged(object sender, EventArgs e)
        {
            // Do nothing, changes are handled in the save button.
            this.mainProgRightSide = Convert.ToInt32(nudRightBorder.Value);
            this.Width = this.mainProgRightSide - this.mainProgLeftSide;
            valuesChanged = true;
        }
        private void nudBottomBorder_ValueChanged(object sender, EventArgs e)
        {
            // Do nothing, changes are handled in the save button.
            this.mainProgBottomSide = Convert.ToInt32(nudBottomBorder.Value);
            this.Height = this.mainProgBottomSide - this.mainProgTopSide;
            valuesChanged = true;
        }
        private void nudSmallImageWidth_ValueChanged(object sender, EventArgs e)
        {
            // Do nothing, changes are handled in the save button.
            this.smallImageWidth = Convert.ToInt32(nudSmallImageWidth.Value);
            valuesChanged = true;
        }
        private void nudSmallImageHeight_ValueChanged(object sender, EventArgs e)
        {
            // Do nothing, changes are handled in the save button.
            this.smallImageHeight = Convert.ToInt32(nudSmallImageHeight.Value);
            valuesChanged = true;
        }
        private void nudLargeImageWidth_ValueChanged(object sender, EventArgs e)
        {
            // Do nothing, changes are handled in the save button.
            this.largeImageWidth = Convert.ToInt32(nudLargeImageWidth.Value);
            valuesChanged = true;
        }
        private void nudLargeImageHeight_ValueChanged(object sender, EventArgs e)
        {
            // Do nothing, changes are handled in the save button.
            this.largeImageHeight = Convert.ToInt32(nudLargeImageHeight.Value);
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
                    txtBxMotifTypeAdding.SetBounds(10, 72, 220, 20);
                    txtBxMotifTypeAdding.Enabled = true;
                    //          Lit the button.
                    btnAddMotifType.Visible = true;
                    btnAddMotifType.Enabled = true;
                }
                else
                {
                    txtBxMotifTypeAdding.Text = imageCategories[selIndex - 1].tag + ", " + imageCategories[selIndex - 1].description + ", " + imageCategories[selIndex - 1].catReqReq;
                    txtBxMotifTypeAdding.SetBounds(10, 72, 275, 20);
                }
            }
        }
        private void txtBxMotifTypeAdding_TextChanged(object sender, EventArgs e)
        {
            // Do nothing, this is handled in btnAddMotifType_Click.
            btnAddMotifType.Visible = true;
            btnAddMotifType.Enabled = true;
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
                else
                {
                    MessageBox.Show("Expected a tag!", "Format info", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                theNewInfo = theNewInfo.Substring(dp0 + 2, theNewInfo.Length - dp0 - 2);
                int dp1 = theNewInfo.IndexOf(",");
                if ((dp1 > 0) && (dp1 < theNewInfo.Length - 2))
                {
                    string theNewDescription = theNewInfo.Substring(0, dp1);
                    if ((theNewDescription != "Undefined") && (theNewDescription != "Unknown") && (theNewDescription != "undefined") && (theNewDescription != "unknown"))
                        imageCategories[noOfImageCategories].description = theNewDescription;
                    else
                    {
                        MessageBox.Show("Expected a description!", "Format info", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
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
                    noOfImageCategories++;
                }
                txtBxMotifTypeAdding.Text = "";
                txtBxMotifTypeAdding.SetBounds(10, 72, 275, 20);
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
                    else
                    {
                        MessageBox.Show("Expected a tag!", "Format info", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                    theNewInfo = theNewInfo.Substring(dp0 + 2, theNewInfo.Length - dp0 - 2);
                    int dp1 = theNewInfo.IndexOf(";");
                    if ((dp1 > 0) && (dp1 < theNewInfo.Length - 2))
                    {
                        string theNewDescription = theNewInfo.Substring(0, dp1);
                        if ((theNewDescription != "Undefined") && (theNewDescription != "Unknown") && (theNewDescription != "undefined") && (theNewDescription != "unknown"))
                            imageCategories[noOfImageCategories].description = theNewDescription;
                        else
                        {
                            MessageBox.Show("Expected a description!", "Format info", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return;
                        }
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
                        noOfImageCategories++;
                    }
                    else
                    {
                        MessageBox.Show("Expected format is \"<tag>[,|;] <explanation>[,|;] <level>", "Format info", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    txtBxMotifTypeAdding.Text = "";
                    txtBxMotifTypeAdding.SetBounds(10, 72, 275, 20);
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
                    txtbxEventTypeToAdd.SetBounds(10, 122, 220, 20);
                    txtbxEventTypeToAdd.Enabled = true;
                    //          Lit the button.
                    btnAddEventType.Visible = true;
                    btnAddEventType.Enabled = true;
                }
                else
                {
                    txtbxEventTypeToAdd.Text = eventCategories[selIndex - 1].tag + ", " + eventCategories[selIndex - 1].description + ", " + eventCategories[selIndex - 1].catReqReq;
                    txtbxEventTypeToAdd.SetBounds(10, 122, 275, 20);
                }
            }
        }
        private void txtbxEventTypeToAdd_TextChanged(object sender, EventArgs e)
        {
            // Do nothing, this is handled in btnAddEventType_Click.
            btnAddEventType.Visible = true;
            btnAddEventType.Enabled = true;
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
                else
                {
                    MessageBox.Show("Expected a tag!", "Format info", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                theNewInfo = theNewInfo.Substring(dp0 + 2, theNewInfo.Length - dp0 - 2);
                int dp1 = theNewInfo.IndexOf(",");
                if ((dp1 > 0) && (dp1 < theNewInfo.Length - 2))
                {
                    string theNewDescription = theNewInfo.Substring(0, dp1);
                    if ((theNewDescription != "Undefined") && (theNewDescription != "Unknown") && (theNewDescription != "undefined") && (theNewDescription != "unknown"))
                        eventCategories[noOfEventCategories].description = theNewDescription;
                    else
                    {
                        MessageBox.Show("Expected a description!", "Format info", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
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
                    noOfEventCategories++;
                }
                txtbxEventTypeToAdd.Text = "";
                txtbxEventTypeToAdd.SetBounds(10, 122, 275, 20);
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
                    else
                    {
                        MessageBox.Show("Expected a tag!", "Format info", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                    theNewInfo = theNewInfo.Substring(dp0 + 2, theNewInfo.Length - dp0 - 2);
                    int dp1 = theNewInfo.IndexOf(";");
                    if ((dp1 > 0) && (dp1 < theNewInfo.Length - 2))
                    {
                        string theNewDescription = theNewInfo.Substring(0, dp1);
                        if ((theNewDescription != "Undefined") && (theNewDescription != "Unknown") && (theNewDescription != "undefined") && (theNewDescription != "unknown"))
                            eventCategories[noOfEventCategories].description = theNewDescription;
                        else
                        {
                            MessageBox.Show("Expected a description!", "Format info", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return;
                        }
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
                        noOfEventCategories++;
                    }
                    else
                    {
                        MessageBox.Show("Expected format is \"<tag>[,|;] <explanation>[,|;] <level>", "Format info", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    txtbxEventTypeToAdd.Text = "";
                    txtbxEventTypeToAdd.SetBounds(10, 122, 275, 20);
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
                    txtBxContentTypeToAdd.SetBounds(10, 172, 220, 20);
                    txtBxContentTypeToAdd.Enabled = true;
                    //          Lit the button.
                    btnAddContentType.Visible = true;
                    btnAddContentType.Enabled = true;
                }
                else
                {
                    txtBxContentTypeToAdd.Text = contentCategories[selIndex - 1].tag + ", " + contentCategories[selIndex - 1].description + ", " + contentCategories[selIndex - 1].catReqReq;
                    txtBxContentTypeToAdd.SetBounds(10, 172, 275, 20);
                    btnAddContentType.Visible = false;
                    btnAddContentType.Enabled = false;
                }
            }
        }
        private void txtBxContentTypeToAdd_TextChanged(object sender, EventArgs e)
        {
            // Do nothing, this is handled in btnAddContentType_Click.
            btnAddContentType.Visible = true;
            btnAddContentType.Enabled = true;
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
                else
                {
                    MessageBox.Show("Expected a tag!", "Format info", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                theNewInfo = theNewInfo.Substring(dp0 + 2, theNewInfo.Length - dp0 - 2);
                int dp1 = theNewInfo.IndexOf(",");
                if ((dp1 > 0) && (dp1 < theNewInfo.Length - 2))
                {
                    string theNewDescription = theNewInfo.Substring(0, dp1);
                    if ((theNewDescription != "Undefined") && (theNewDescription != "Unknown") && (theNewDescription != "undefined") && (theNewDescription != "unknown"))
                        contentCategories[noOfContentCategories].description = theNewDescription;
                    else
                    {
                        MessageBox.Show("Expected a description!", "Format info", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
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
                    noOfContentCategories++;
                }
                else
                {
                    MessageBox.Show("Expected format is \"<tag>[,|;] <explanation>[,|;] <level>", "Format info", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                txtBxContentTypeToAdd.Text = "";
                txtBxContentTypeToAdd.SetBounds(10, 172, 275, 20);
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
                    else
                    {
                        MessageBox.Show("Expected a tag!", "Format info", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                    theNewInfo = theNewInfo.Substring(dp0 + 2, theNewInfo.Length - dp0 - 2);
                    int dp1 = theNewInfo.IndexOf(";");
                    if ((dp1 > 0) && (dp1 < theNewInfo.Length - 2))
                    {
                        string theNewDescription = theNewInfo.Substring(0, dp1);
                        if ((theNewDescription != "Undefined") && (theNewDescription != "Unknown") && (theNewDescription != "undefined") && (theNewDescription != "unknown"))
                            contentCategories[noOfContentCategories].description = theNewDescription;
                        else
                        {
                            MessageBox.Show("Expected a description!", "Format info", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return;
                        }
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
                        noOfContentCategories++;
                    }
                    else
                    {
                        MessageBox.Show("Expected format is \"<tag>[,|;] <explanation>[,|;] <value[x.x]>", "Format info", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    txtBxContentTypeToAdd.Text = "";
                    txtbxEventTypeToAdd.SetBounds(10, 172, 275, 20);
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
                    txtBxRelationTypeToAdd.SetBounds(10, 223, 220, 20);
                    txtBxRelationTypeToAdd.Enabled = true;
                    //          Lit the button.
                    btnAddRelationType.Visible = true;
                    btnAddRelationType.Enabled = true;
                }
                else
                {
                    txtBxRelationTypeToAdd.Text = relationCategories[selIndex - 1].tag + ", " + relationCategories[selIndex - 1].description + ", " + relationCategories[selIndex - 1].catReqReq;
                    txtbxEventTypeToAdd.SetBounds(10, 223, 275, 20);
                }
            }
        }
        private void txtBxRelationTypeToAdd_TextChanged(object sender, EventArgs e)
        {
            // Do nothing, this is handled in btnAddRelationType_Click.
            btnAddRelationType.Visible = true;
            btnAddRelationType.Enabled = true;
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
                else
                {
                    MessageBox.Show("Expected a tag!", "Format info", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                theNewInfo = theNewInfo.Substring(dp0 + 2, theNewInfo.Length - dp0 - 2);
                int dp1 = theNewInfo.IndexOf(",");
                if ((dp1 > 0) && (dp1 < theNewInfo.Length - 2))
                {
                    string theNewDescription = theNewInfo.Substring(0, dp1);
                    if ((theNewDescription != "Undefined") && (theNewDescription != "Unknown") && (theNewDescription != "undefined") && (theNewDescription != "unknown"))
                        relationCategories[noOfRelationCategories].description = theNewDescription;
                    else
                    {
                        MessageBox.Show("Expected a description!", "Format info", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
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
                    noOfRelationCategories++;
                }
                txtBxRelationTypeToAdd.Text = "";
                txtBxRelationTypeToAdd.SetBounds(10, 223, 275, 20);
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
                    else
                    {
                        MessageBox.Show("Expected a tag!", "Format info", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                    theNewInfo = theNewInfo.Substring(dp0 + 2, theNewInfo.Length - dp0 - 2);
                    int dp1 = theNewInfo.IndexOf(";");
                    if ((dp1 > 0) && (dp1 < theNewInfo.Length - 2))
                    {
                        string theNewDescription = theNewInfo.Substring(0, dp1);
                        if ((theNewDescription != "Undefined") && (theNewDescription != "Unknown") && (theNewDescription != "undefined") && (theNewDescription != "unknown"))
                            relationCategories[noOfRelationCategories].description = theNewDescription;
                        else
                        {
                            MessageBox.Show("Expected a description!", "Format info", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return;
                        }
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
                        noOfRelationCategories++;
                    }
                    else
                    {
                        MessageBox.Show("Expected format is \"<tag>[,|;] <explanation>[,|;] <level>", "Format info", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    txtBxRelationTypeToAdd.Text = "";
                    txtBxRelationTypeToAdd.SetBounds(10, 223, 275, 20);
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
                    txtBxNationalityTypeToAdd.SetBounds(10, 273, 220, 20);
                    txtBxNationalityTypeToAdd.Enabled = true;
                    //          Lit the button.
                    btnAddNationalityType.Visible = true;
                    btnAddNationalityType.Enabled = true;
                }
                else
                {
                    txtBxNationalityTypeToAdd.Text = nationalityCategories[selIndex - 1].tag + ", " + nationalityCategories[selIndex - 1].description + ", " + nationalityCategories[selIndex - 1].prefixNumber.ToString();
                    txtBxNationalityTypeToAdd.SetBounds(10, 273, 275, 20);
                }
            }
        }
        private void txtBxNationalityTypeToAdd_TextChanged(object sender, EventArgs e)
        {
            // Do nothing, this is handled in btnAddNationalityType_Click.
            btnAddNationalityType.Visible = true;
            btnAddNationalityType.Enabled = true;
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
                else
                {
                    MessageBox.Show("Expected a tag!", "Format info", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                theNewInfo = theNewInfo.Substring(dp0 + 2, theNewInfo.Length - dp0 - 2);
                int dp1 = theNewInfo.IndexOf(",");
                if ((dp1 > 0) && (dp1 < theNewInfo.Length - 2))
                {
                    string theNewDescription = theNewInfo.Substring(0, dp1);
                    if ((theNewDescription != "Undefined") && (theNewDescription != "Unknown") && (theNewDescription != "undefined") && (theNewDescription != "unknown"))
                        nationalityCategories[noOfNationalities].description = theNewDescription;
                    else
                    {
                        MessageBox.Show("Expected a description!", "Format info", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                    theNewInfo = theNewInfo.Substring(dp1 + 2, theNewInfo.Length - dp1 - 2);
                    if ((theNewInfo != "Undefined") && (theNewInfo != "Unknown") && (theNewInfo != "undefined") && (theNewInfo != "unknown"))
                    {
                        int nr = 0;
                        if (int.TryParse(theNewInfo, out nr))
                            nationalityCategories[noOfNationalities].prefixNumber = nr;
                    }
                    noOfNationalities++;
                }
                else
                {
                    MessageBox.Show("Expected format is \"<tag>[,|;] <explanation>[,|;] <prefix>", "Format info", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
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
                    else
                    {
                        MessageBox.Show("Expected a tag!", "Format info", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                    theNewInfo = theNewInfo.Substring(dp0 + 2, theNewInfo.Length - dp0 - 2);
                    int dp1 = theNewInfo.IndexOf(";");
                    if ((dp1 > 0) && (dp1 < theNewInfo.Length - 2))
                    {
                        string theNewDescription = theNewInfo.Substring(0, dp1);
                        if ((theNewDescription != "Undefined") && (theNewDescription != "Unknown") && (theNewDescription != "undefined") && (theNewDescription != "unknown"))
                            nationalityCategories[noOfNationalities].description = theNewDescription;
                        else
                        {
                            MessageBox.Show("Expected a description!", "Format info", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return;
                        }
                        theNewInfo = theNewInfo.Substring(dp1 + 2, theNewInfo.Length - dp1 - 2);
                        if ((theNewInfo != "Undefined") && (theNewInfo != "Unknown") && (theNewInfo != "undefined") && (theNewInfo != "unknown"))
                        {
                            int nr = 0;
                            if (int.TryParse(theNewInfo, out nr))
                                nationalityCategories[noOfNationalities].prefixNumber = nr;
                        }
                        noOfNationalities++;
                    }
                    else
                    {
                        MessageBox.Show("Expected format is \"<tag>[,|;] <explanation>[,|;] <value[x.x]>", "Format info", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
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
            int selIndex = cmbBoxExistingCurrencies.SelectedIndex;
            if (selIndex > 0)
            {
                if ((cmbBoxExistingCurrencies.SelectedItem.ToString() == "Add Item...") || ((selIndex - 1) > noOfCurrencies))
                {
                    // New currency being added, set textbox width to small.
                    txtBxCurrencyToAdd.Text = "";
                    txtBxCurrencyToAdd.SetBounds(10, 323, 220, 20);
                    txtBxCurrencyToAdd.Enabled = true;
                    btnAddCurrency.Visible = true;
                    btnAddCurrency.Enabled = true;
                }
                else
                {
                    txtBxCurrencyToAdd.Text = currencyCategories[selIndex - 1].tag + ", " + currencyCategories[selIndex - 1].description + ", " + currencyCategories[selIndex - 1].value.ToString();
                    txtBxCurrencyToAdd.SetBounds(10, 323, 275, 20);
                }
            }
        }
        private void txtBxCurrencyToAdd_TextChanged(object sender, EventArgs e)
        {
            // Do nothing, this is handled in btnAddCurrency_Click.
            btnAddCurrency.Visible = true;
            btnAddCurrency.Enabled = true;
        }
        private void btnAddCurrency_Click(object sender, EventArgs e)
        {
            string theNewInfo = txtBxCurrencyToAdd.Text;
            int dp0 = theNewInfo.IndexOf(",");
            if ((dp0 > 0) && (dp0 < theNewInfo.Length - 2))
            {
                string theNewTag = theNewInfo.Substring(0, dp0);
                if ((theNewTag != "Undefined") && (theNewTag != "Unknown") && (theNewTag != "undefined") && (theNewTag != "unknown"))
                    currencyCategories[noOfCurrencies].tag = theNewTag;
                else
                {
                    MessageBox.Show("Expected a tag!", "Format info", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                theNewInfo = theNewInfo.Substring(dp0 + 2, theNewInfo.Length - dp0 - 2);
                int dp1 = theNewInfo.IndexOf(",");
                if ((dp1 > 0) && (dp1 < theNewInfo.Length - 2))
                {
                    string theNewDescription = theNewInfo.Substring(0, dp1);
                    if ((theNewDescription != "Undefined") && (theNewDescription != "Unknown") && (theNewDescription != "undefined") && (theNewDescription != "unknown"))
                        currencyCategories[noOfCurrencies].description = theNewDescription;
                    else
                    {
                        MessageBox.Show("Expected a description!", "Format info", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                    theNewInfo = theNewInfo.Substring(dp1 + 2, theNewInfo.Length - dp1 - 2);
                    if ((theNewInfo != "Undefined") && (theNewInfo != "Unknown") && (theNewInfo != "undefined") && (theNewInfo != "unknown"))
                    {
                        float varde = 0;
                        if (float.TryParse(theNewInfo, out varde))
                            currencyCategories[noOfCurrencies].value = varde;
                    }
                    noOfCurrencies++;
                }
                else
                {
                    MessageBox.Show("Expected format is \"<tag>[,|;] <explanation>[,|;] <value[x.x]>", "Format info", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                txtBxCurrencyToAdd.Text = "";
                txtBxCurrencyToAdd.SetBounds(10, 323, 275, 20);
                txtBxCurrencyToAdd.Enabled = false;
                btnAddCurrency.Enabled = false;
                btnAddCurrency.Visible = false;
                cmbBoxExistingCurrencies.Items.Clear();
                cmbBoxExistingCurrencies.Items.Add("Select...");
                for (int i = 0; i < noOfCurrencies - 1; i++)
                    cmbBoxExistingCurrencies.Items.Add(currencyCategories[i].tag);
                cmbBoxExistingCurrencies.Items.Add("Add Item...");
                cmbBoxExistingCurrencies.SelectedIndex = 0;
                valuesChanged = true;
            }
            else
            {
                dp0 = theNewInfo.IndexOf(";");
                if ((dp0 > 0) && (dp0 < theNewInfo.Length - 2))
                {
                    string theNewTag = theNewInfo.Substring(0, dp0);
                    if ((theNewTag != "Undefined") && (theNewTag != "Unknown") && (theNewTag != "undefined") && (theNewTag != "unknown"))
                        currencyCategories[noOfCurrencies].tag = theNewTag;
                    else
                    {
                        MessageBox.Show("Expected a tag!", "Format info", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                    theNewInfo = theNewInfo.Substring(dp0 + 2, theNewInfo.Length - dp0 - 2);
                    int dp1 = theNewInfo.IndexOf(";");
                    if ((dp1 > 0) && (dp1 < theNewInfo.Length - 2))
                    {
                        string theNewDescription = theNewInfo.Substring(0, dp1);
                        if ((theNewDescription != "Undefined") && (theNewDescription != "Unknown") && (theNewDescription != "undefined") && (theNewDescription != "unknown"))
                            currencyCategories[noOfCurrencies].description = theNewDescription;
                        else
                        {
                            MessageBox.Show("Expected a description!", "Format info", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return;
                        }
                        theNewInfo = theNewInfo.Substring(dp1 + 2, theNewInfo.Length - dp1 - 2);
                        if ((theNewInfo != "Undefined") && (theNewInfo != "Unknown") && (theNewInfo != "undefined") && (theNewInfo != "unknown"))
                        {
                            float varde = 0;
                            if (float.TryParse(theNewInfo, out varde))
                                currencyCategories[noOfCurrencies].value = varde;
                        }
                        noOfCurrencies++;
                    }
                    else
                    {
                        MessageBox.Show("Expected format is \"<tag>[,|;] <explanation>[,|;] <value[x.x]>", "Format info", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    txtBxCurrencyToAdd.Text = "";
                    txtBxCurrencyToAdd.SetBounds(10, 323, 275, 20);
                    txtBxCurrencyToAdd.Enabled = false;
                    btnAddCurrency.Enabled = false;
                    btnAddCurrency.Visible = false;
                    cmbBoxExistingCurrencies.Items.Clear();
                    cmbBoxExistingCurrencies.Items.Add("Select...");
                    for (int i = 0; i < noOfCurrencies - 1; i++)
                        cmbBoxExistingCurrencies.Items.Add(currencyCategories[i].tag);
                    cmbBoxExistingCurrencies.Items.Add("Add Item...");
                    cmbBoxExistingCurrencies.SelectedIndex = 0;
                    valuesChanged = true;
                }
                else
                {
                    MessageBox.Show("Expected format is \"<tag>[,|;] <explanation>[,|;] <value[x.x]>", "Format info", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }
        private void cmbBxExistingContactWays_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selIndex = cmbBxExistingContactWays.SelectedIndex;
            if (selIndex > 0)
            {
                if ((cmbBxExistingContactWays.SelectedItem.ToString() == "Add Item...") || ((selIndex - 1) > noOfContactWays))
                {
                    // New contact way being added, set textbox width to small and lit the button
                    txtBxContactWaysToAdd.Text = "";
                    txtBxContactWaysToAdd.SetBounds(10, 373, 220, 20);
                    txtBxContactWaysToAdd.Enabled = true;
                    btnAddContactWay.Visible = true;
                    btnAddContactWay.Enabled = true;
                }
                else
                {
                    txtBxContactWaysToAdd.Text = contactWays[selIndex - 1].tag + ", " + contactWays[selIndex - 1].description + ", " + contactWays[selIndex - 1].catReqReq.ToString();
                    txtBxCurrencyToAdd.SetBounds(10, 373, 275, 20);
                    btnAddContactWay.Visible = false;
                    btnAddContactWay.Enabled = false;
                }
            }
        }
        private void txtBxContactWaysToAdd_TextChanged(object sender, EventArgs e)
        {
            // Do nothing, it is handled in btnAddContactWay_Click.
            btnAddContactWay.Visible = true;
            btnAddContactWay.Enabled = true;
        }
        private void btnAddContactWay_Click(object sender, EventArgs e)
        {
            string theNewInfo = txtBxContactWaysToAdd.Text;
            int dp0 = theNewInfo.IndexOf(",");
            if ((dp0 > 0) && (dp0 < theNewInfo.Length - 2))
            {
                string theNewTag = theNewInfo.Substring(0, dp0);
                if ((theNewTag != "Undefined") && (theNewTag != "Unknown") && (theNewTag != "undefined") && (theNewTag != "unknown"))
                    contactWays[noOfContactWays].tag = theNewTag;
                else
                {
                    MessageBox.Show("Expected a tag!", "Format info", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                theNewInfo = theNewInfo.Substring(dp0 + 2, theNewInfo.Length - dp0 - 2);
                int dp1 = theNewInfo.IndexOf(",");
                if ((dp1 > 0) && (dp1 < theNewInfo.Length - 2))
                {
                    string theNewDescription = theNewInfo.Substring(0, dp1);
                    if ((theNewDescription != "Undefined") && (theNewDescription != "Unknown") && (theNewDescription != "undefined") && (theNewDescription != "unknown"))
                        contactWays[noOfContactWays].description = theNewDescription;
                    else
                    {
                        MessageBox.Show("Expected a description!", "Format info", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                    theNewInfo = theNewInfo.Substring(dp1 + 2, theNewInfo.Length - dp1 - 2);
                    if ((theNewInfo == "Open") || (theNewInfo == "open"))
                        contactWays[noOfContactWays].catReqReq = rightsLevel.Open;
                    else if ((theNewInfo == "Limited") || (theNewInfo == "limited"))
                        contactWays[noOfContactWays].catReqReq = rightsLevel.Limited;
                    else if ((theNewInfo == "Medium") || (theNewInfo == "medium"))
                        contactWays[noOfContactWays].catReqReq = rightsLevel.Medium;
                    else if ((theNewInfo == "Relative") || (theNewInfo == "relative"))
                        contactWays[noOfContactWays].catReqReq = rightsLevel.Relative;
                    else if ((theNewInfo == "Secret") || (theNewInfo == "secret"))
                        contactWays[noOfContactWays].catReqReq = rightsLevel.Secret;
                    else if ((theNewInfo == "Qualified") || (theNewInfo == "qualified") || (theNewInfo == "QualifiedSecret") || (theNewInfo == "qualifiedsecret"))
                        contactWays[noOfContactWays].catReqReq = rightsLevel.QualifSecret;
                    else
                        contactWays[noOfContactWays].catReqReq = rightsLevel.Undefined;
                    noOfContactWays++;
                }
                txtBxContactWaysToAdd.Text = "";
                txtBxContactWaysToAdd.SetBounds(10, 373, 275, 20);
                txtBxContactWaysToAdd.Enabled = false;
                btnAddContactWay.Enabled = false;
                btnAddContactWay.Visible = false;
                cmbBxExistingContactWays.Items.Clear();
                cmbBxExistingContactWays.Items.Add("Select...");
                for (int i = 0; i < noOfContactWays - 1; i++)
                    cmbBxExistingContactWays.Items.Add(contactWays[i].tag);
                cmbBxExistingContactWays.Items.Add("Add Item...");
                cmbBxExistingContactWays.SelectedIndex = 0;
                valuesChanged = true;
            }
            else
            {
                dp0 = theNewInfo.IndexOf(";");
                if ((dp0 > 0) && (dp0 < theNewInfo.Length - 2))
                {
                    string theNewTag = theNewInfo.Substring(0, dp0);
                    if ((theNewTag != "Undefined") && (theNewTag != "Unknown") && (theNewTag != "undefined") && (theNewTag != "unknown"))
                        contactWays[noOfContactWays].tag = theNewTag;
                    else
                    {
                        MessageBox.Show("Expected a tag!", "Format info", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                    theNewInfo = theNewInfo.Substring(dp0 + 2, theNewInfo.Length - dp0 - 2);
                    int dp1 = theNewInfo.IndexOf(";");
                    if ((dp1 > 0) && (dp1 < theNewInfo.Length - 2))
                    {
                        string theNewDescription = theNewInfo.Substring(0, dp1);
                        if ((theNewDescription != "Undefined") && (theNewDescription != "Unknown") && (theNewDescription != "undefined") && (theNewDescription != "unknown"))
                            contactWays[noOfContactWays].description = theNewDescription;
                        else
                        {
                            MessageBox.Show("Expected a description!", "Format info", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return;
                        }
                        theNewInfo = theNewInfo.Substring(dp1 + 2, theNewInfo.Length - dp1 - 2);
                        if ((theNewInfo == "Open") || (theNewInfo == "open"))
                            contactWays[noOfContactWays].catReqReq = rightsLevel.Open;
                        else if ((theNewInfo == "Limited") || (theNewInfo == "limited"))
                            contactWays[noOfContactWays].catReqReq = rightsLevel.Limited;
                        else if ((theNewInfo == "Medium") || (theNewInfo == "medium"))
                            contactWays[noOfContactWays].catReqReq = rightsLevel.Medium;
                        else if ((theNewInfo == "Relative") || (theNewInfo == "relative"))
                            contactWays[noOfContactWays].catReqReq = rightsLevel.Relative;
                        else if ((theNewInfo == "Secret") || (theNewInfo == "secret"))
                            contactWays[noOfContactWays].catReqReq = rightsLevel.Secret;
                        else if ((theNewInfo == "Qualified") || (theNewInfo == "qualified") || (theNewInfo == "QualifiedSecret") || (theNewInfo == "qualifiedsecret"))
                            contactWays[noOfContactWays].catReqReq = rightsLevel.QualifSecret;
                        else
                            contactWays[noOfContactWays].catReqReq = rightsLevel.Undefined;
                        noOfContactWays++;
                    }
                    else
                    {
                        MessageBox.Show("Expected format is \"<tag>[,|;] <explanation>[,|;] <level>", "Format info", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    txtBxContactWaysToAdd.Text = "";
                    txtBxContactWaysToAdd.SetBounds(10, 373, 275, 20);
                    txtBxContactWaysToAdd.Enabled = false;
                    btnAddContactWay.Enabled = false;
                    btnAddContactWay.Visible = false;
                    cmbBxExistingContactWays.Items.Clear();
                    cmbBxExistingContactWays.Items.Add("Select...");
                    for (int i = 0; i < noOfContactWays - 1; i++)
                        cmbBxExistingContactWays.Items.Add(contactWays[i].tag);
                    cmbBxExistingContactWays.Items.Add("Add Item...");
                    cmbBxExistingContactWays.SelectedIndex = 0;
                    valuesChanged = true;
                }
                else
                {
                    MessageBox.Show("Expected format is \"<tag>[,|;] <explanation>[,|;] <level>", "Format info", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }
        private void cmbBxExistingComplexions_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selIndex = cmbBxExistingComplexions.SelectedIndex;
            if (selIndex > 0)
            {
                if ((cmbBxExistingComplexions.SelectedItem.ToString() == "Add Item...") || ((selIndex - 1) > noOfComplexions))
                {
                    // A new complexion is being added, set textbox width to small and lit the button.
                    txtBxComplexionToAdd.Text = "";
                    txtBxComplexionToAdd.SetBounds(10, 425, 220, 20);
                    txtBxComplexionToAdd.Enabled = true;
                    btnAddComplexion.Visible = true;
                    btnAddComplexion.Enabled = true;
                }
                else
                {
                    txtBxComplexionToAdd.Text = complexionCategories[selIndex - 1].tag + ", " + complexionCategories[selIndex - 1].description + ", " + complexionCategories[selIndex - 1].catReqReq.ToString();
                    txtBxComplexionToAdd.SetBounds(10, 425, 275, 20);
                    btnAddComplexion.Visible = false;
                    btnAddComplexion.Enabled = false;
                }
            }
        }
        private void txtBxComplexionToAdd_TextChanged(object sender, EventArgs e)
        {
            // Do nothing, this is handled in btnAddComplexion_Click.
            btnAddComplexion.Visible = true;
            btnAddComplexion.Enabled = true;
        }
        private void btnAddComplexion_Click(object sender, EventArgs e)
        {
            string theNewInfo = txtBxComplexionToAdd.Text;
            int dp0 = theNewInfo.IndexOf(",");
            if ((dp0 > 0) && (dp0 < theNewInfo.Length - 2))
            {
                string theNewTag = theNewInfo.Substring(0, dp0);
                if ((theNewTag != "Undefined") && (theNewTag != "Unknown") && (theNewTag != "undefined") && (theNewTag != "unknown"))
                    complexionCategories[noOfComplexions].tag = theNewTag;
                else
                {
                    MessageBox.Show("Expected a tag!", "Format info", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                theNewInfo = theNewInfo.Substring(dp0 + 2, theNewInfo.Length - dp0 - 2);
                int dp1 = theNewInfo.IndexOf(",");
                if ((dp1 > 0) && (dp1 < theNewInfo.Length - 2))
                {
                    string theNewDescription = theNewInfo.Substring(0, dp1);
                    if ((theNewDescription != "Undefined") && (theNewDescription != "Unknown") && (theNewDescription != "undefined") && (theNewDescription != "unknown"))
                        complexionCategories[noOfComplexions].description = theNewDescription;
                    else
                    {
                        MessageBox.Show("Expected a description!", "Format info", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                    theNewInfo = theNewInfo.Substring(dp1 + 2, theNewInfo.Length - dp1 - 2);
                    if ((theNewInfo == "Open") || (theNewInfo == "open"))
                        complexionCategories[noOfComplexions].catReqReq = rightsLevel.Open;
                    else if ((theNewInfo == "Limited") || (theNewInfo == "limited"))
                        complexionCategories[noOfComplexions].catReqReq = rightsLevel.Limited;
                    else if ((theNewInfo == "Medium") || (theNewInfo == "medium"))
                        complexionCategories[noOfComplexions].catReqReq = rightsLevel.Medium;
                    else if ((theNewInfo == "Relative") || (theNewInfo == "relative"))
                        complexionCategories[noOfComplexions].catReqReq = rightsLevel.Relative;
                    else if ((theNewInfo == "Secret") || (theNewInfo == "secret"))
                        complexionCategories[noOfComplexions].catReqReq = rightsLevel.Secret;
                    else if ((theNewInfo == "Qualified") || (theNewInfo == "qualified") || (theNewInfo == "QualifiedSecret") || (theNewInfo == "qualifiedsecret"))
                        complexionCategories[noOfComplexions].catReqReq = rightsLevel.QualifSecret;
                    else
                        complexionCategories[noOfComplexions].catReqReq = rightsLevel.Undefined;
                    noOfComplexions++;
                }
                else
                {
                    MessageBox.Show("Expected format is \"<tag>[,|;] <explanation>[,|;] <level>", "Format info", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                txtBxComplexionToAdd.Text = "";
                txtBxComplexionToAdd.SetBounds(10, 425, 275, 20);
                txtBxComplexionToAdd.Enabled = false;
                btnAddComplexion.Enabled = false;
                btnAddComplexion.Visible = false;
                cmbBxExistingComplexions.Items.Clear();
                cmbBxExistingComplexions.Items.Add("Select...");
                for (int i = 0; i < noOfComplexions; i++)
                    cmbBxExistingComplexions.Items.Add(complexionCategories[i].tag);
                cmbBxExistingComplexions.Items.Add("Add Item...");
                cmbBxExistingComplexions.SelectedIndex = 0;
                valuesChanged = true;
            }
            else
            {
                dp0 = theNewInfo.IndexOf(";");
                if ((dp0 > 0) && (dp0 < theNewInfo.Length - 2))
                {
                    string theNewTag = theNewInfo.Substring(0, dp0);
                    if ((theNewTag != "Undefined") && (theNewTag != "Unknown") && (theNewTag != "undefined") && (theNewTag != "unknown"))
                        complexionCategories[noOfComplexions].tag = theNewTag;
                    else
                    {
                        MessageBox.Show("Expected a tag!", "Format info", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                    theNewInfo = theNewInfo.Substring(dp0 + 2, theNewInfo.Length - dp0 - 2);
                    int dp1 = theNewInfo.IndexOf(";");
                    if ((dp1 > 0) && (dp1 < theNewInfo.Length - 2))
                    {
                        string theNewDescription = theNewInfo.Substring(0, dp1);
                        if ((theNewDescription != "Undefined") && (theNewDescription != "Unknown") && (theNewDescription != "undefined") && (theNewDescription != "unknown"))
                            complexionCategories[noOfComplexions].description = theNewDescription;
                        else
                        {
                            MessageBox.Show("Expected a description!", "Format info", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return;
                        }
                        theNewInfo = theNewInfo.Substring(dp1 + 2, theNewInfo.Length - dp1 - 2);
                        if ((theNewInfo == "Open") || (theNewInfo == "open"))
                            complexionCategories[noOfComplexions].catReqReq = rightsLevel.Open;
                        else if ((theNewInfo == "Limited") || (theNewInfo == "limited"))
                            complexionCategories[noOfComplexions].catReqReq = rightsLevel.Limited;
                        else if ((theNewInfo == "Medium") || (theNewInfo == "medium"))
                            complexionCategories[noOfComplexions].catReqReq = rightsLevel.Medium;
                        else if ((theNewInfo == "Relative") || (theNewInfo == "relative"))
                            complexionCategories[noOfComplexions].catReqReq = rightsLevel.Relative;
                        else if ((theNewInfo == "Secret") || (theNewInfo == "secret"))
                            complexionCategories[noOfComplexions].catReqReq = rightsLevel.Secret;
                        else if ((theNewInfo == "Qualified") || (theNewInfo == "qualified") || (theNewInfo == "QualifiedSecret") || (theNewInfo == "qualifiedsecret"))
                            complexionCategories[noOfComplexions].catReqReq = rightsLevel.QualifSecret;
                        else
                            complexionCategories[noOfComplexions].catReqReq = rightsLevel.Undefined;
                        noOfComplexions++;
                    }
                    else
                    {
                        MessageBox.Show("Expected format is \"<tag>[,|;] <explanation>[,|;] <level>", "Format info", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    txtBxComplexionToAdd.Text = "";
                    txtBxComplexionToAdd.SetBounds(10, 425, 275, 20);
                    txtBxComplexionToAdd.Enabled = false;
                    btnAddComplexion.Enabled = false;
                    btnAddComplexion.Visible = false;
                    cmbBxExistingComplexions.Items.Clear();
                    cmbBxExistingComplexions.Items.Add("Select...");
                    for (int i = 0; i < noOfComplexions; i++)
                        cmbBxExistingComplexions.Items.Add(complexionCategories[i].tag);
                    cmbBxExistingComplexions.Items.Add("Add Item...");
                    cmbBxExistingComplexions.SelectedIndex = 0;
                    valuesChanged = true;
                }
                else
                {
                    MessageBox.Show("Expected format is \"<tag>[,|;] <explanation>[,|;] <level>", "Format info", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }
        private void cmbBxExistingHairColors_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selIndex = cmbBxExistingHairColors.SelectedIndex;
            if (selIndex > 0)
            {
                if ((cmbBxExistingHairColors.SelectedItem.ToString() == "Add Item...") || ((selIndex - 1) > noOfHairColors))
                {
                    txtBxHairColorToAdd.Text = "";
                    txtBxHairColorToAdd.SetBounds(10, 475, 220, 20);
                    txtBxHairColorToAdd.Enabled = true;
                    btnAddHairColor.Visible = true;
                    btnAddHairColor.Enabled = true;
                }
                else
                {
                    txtBxHairColorToAdd.Text = hairColorsDefined[selIndex - 1].tag + ", " + hairColorsDefined[selIndex - 1].description + ", " + hairColorsDefined[selIndex - 1].catReqReq.ToString();
                    txtBxHairColorToAdd.SetBounds(10, 475, 275, 20);
                    btnAddHairColor.Visible = false;
                    btnAddHairColor.Enabled = false;
                }
            }
        }
        private void txtBxHairColorToAdd_TextChanged(object sender, EventArgs e)
        {
            // Do nothing, this is handled by btnAddHairColor_Click.
            btnAddHairColor.Visible = true;
            btnAddHairColor.Enabled = true;
        }
        private void btnAddHairColor_Click(object sender, EventArgs e)
        {
            string theNewInfo = txtBxHairColorToAdd.Text;
            int dp0 = theNewInfo.IndexOf(",");
            if ((dp0 > 0) && (dp0 < theNewInfo.Length - 2))
            {
                string theNewTag = theNewInfo.Substring(0, dp0);
                if ((theNewTag != "Undefined") && (theNewTag != "Unknown") && (theNewTag != "undefined") && (theNewTag != "unknown"))
                    hairColorsDefined[noOfHairColors].tag = theNewTag;
                else
                {
                    MessageBox.Show("Expected a tag!", "Format info", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                theNewInfo = theNewInfo.Substring(dp0 + 2, theNewInfo.Length - dp0 - 2);
                int dp1 = theNewInfo.IndexOf(",");
                if ((dp1 > 0) && (dp1 < theNewInfo.Length - 2))
                {
                    string theNewDescription = theNewInfo.Substring(0, dp1);
                    if ((theNewDescription != "Undefined") && (theNewDescription != "Unknown") && (theNewDescription != "undefined") && (theNewDescription != "unknown"))
                        hairColorsDefined[noOfHairColors].description = theNewDescription;
                    else
                    {
                        MessageBox.Show("Expected a description!", "Format info", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                    theNewInfo = theNewInfo.Substring(dp1 + 2, theNewInfo.Length - dp1 - 2);
                    if ((theNewInfo == "Open") || (theNewInfo == "open"))
                        hairColorsDefined[noOfHairColors].catReqReq = rightsLevel.Open;
                    else if ((theNewInfo == "Limited") || (theNewInfo == "limited"))
                        hairColorsDefined[noOfHairColors].catReqReq = rightsLevel.Limited;
                    else if ((theNewInfo == "Medium") || (theNewInfo == "medium"))
                        hairColorsDefined[noOfHairColors].catReqReq = rightsLevel.Medium;
                    else if ((theNewInfo == "Relative") || (theNewInfo == "relative"))
                        hairColorsDefined[noOfHairColors].catReqReq = rightsLevel.Relative;
                    else if ((theNewInfo == "Secret") || (theNewInfo == "secret"))
                        hairColorsDefined[noOfHairColors].catReqReq = rightsLevel.Secret;
                    else if ((theNewInfo == "Qualified") || (theNewInfo == "qualified") || (theNewInfo == "QualifiedSecret") || (theNewInfo == "qualifiedsecret"))
                        hairColorsDefined[noOfHairColors].catReqReq = rightsLevel.QualifSecret;
                    else
                        hairColorsDefined[noOfHairColors].catReqReq = rightsLevel.Undefined;
                    noOfHairColors++;
                }
                else
                {
                    MessageBox.Show("Expected format is \"<tag>[,|;] <explanation>[,|;] <level>", "Format info", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                txtBxHairColorToAdd.Text = "";
                txtBxHairColorToAdd.SetBounds(10, 475, 275, 20);
                txtBxHairColorToAdd.Enabled = false;
                btnAddHairColor.Enabled = false;
                btnAddHairColor.Visible = false;
                cmbBxExistingHairColors.Items.Clear();
                cmbBxExistingHairColors.Items.Add("Select...");
                for (int i = 0; i < noOfHairColors; i++)
                    cmbBxExistingHairColors.Items.Add(hairColorsDefined[i].tag);
                cmbBxExistingHairColors.Items.Add("Add Item...");
                cmbBxExistingHairColors.SelectedIndex = 0;
                valuesChanged = true;
            }
            else
            {
                dp0 = theNewInfo.IndexOf(";");
                if ((dp0 > 0) && (dp0 < theNewInfo.Length - 2))
                {
                    string theNewTag = theNewInfo.Substring(0, dp0);
                    if ((theNewTag != "Undefined") && (theNewTag != "Unknown") && (theNewTag != "undefined") && (theNewTag != "unknown"))
                        hairColorsDefined[noOfHairColors].tag = theNewTag;
                    else
                    {
                        MessageBox.Show("Expected a tag!", "Format info", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                    theNewInfo = theNewInfo.Substring(dp0 + 2, theNewInfo.Length - dp0 - 2);
                    int dp1 = theNewInfo.IndexOf(";");
                    if ((dp1 > 0) && (dp1 < theNewInfo.Length - 2))
                    {
                        string theNewDescription = theNewInfo.Substring(0, dp1);
                        if ((theNewDescription != "Undefined") && (theNewDescription != "Unknown") && (theNewDescription != "undefined") && (theNewDescription != "unknown"))
                            hairColorsDefined[noOfHairColors].description = theNewDescription;
                        else
                        {
                            MessageBox.Show("Expected a description!", "Format info", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return;
                        }
                        theNewInfo = theNewInfo.Substring(dp1 + 2, theNewInfo.Length - dp1 - 2);
                        if ((theNewInfo == "Open") || (theNewInfo == "open"))
                            hairColorsDefined[noOfHairColors].catReqReq = rightsLevel.Open;
                        else if ((theNewInfo == "Limited") || (theNewInfo == "limited"))
                            hairColorsDefined[noOfHairColors].catReqReq = rightsLevel.Limited;
                        else if ((theNewInfo == "Medium") || (theNewInfo == "medium"))
                            hairColorsDefined[noOfHairColors].catReqReq = rightsLevel.Medium;
                        else if ((theNewInfo == "Relative") || (theNewInfo == "relative"))
                            hairColorsDefined[noOfHairColors].catReqReq = rightsLevel.Relative;
                        else if ((theNewInfo == "Secret") || (theNewInfo == "secret"))
                            hairColorsDefined[noOfHairColors].catReqReq = rightsLevel.Secret;
                        else if ((theNewInfo == "Qualified") || (theNewInfo == "qualified") || (theNewInfo == "QualifiedSecret") || (theNewInfo == "qualifiedsecret"))
                            hairColorsDefined[noOfHairColors].catReqReq = rightsLevel.QualifSecret;
                        else
                            hairColorsDefined[noOfHairColors].catReqReq = rightsLevel.Undefined;
                        noOfHairColors++;
                    }
                    else
                    {
                        MessageBox.Show("Expected format is \"<tag>[,|;] <explanation>[,|;] <level>", "Format info", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    txtBxHairColorToAdd.Text = "";
                    txtBxHairColorToAdd.SetBounds(10, 475, 275, 20);
                    txtBxHairColorToAdd.Enabled = false;
                    btnAddHairColor.Enabled = false;
                    btnAddHairColor.Visible = false;
                    cmbBxExistingHairColors.Items.Clear();
                    cmbBxExistingHairColors.Items.Add("Select...");
                    for (int i = 0; i < noOfHairColors - 1; i++)
                        cmbBxExistingHairColors.Items.Add(hairColorsDefined[i].tag);
                    cmbBxExistingHairColors.Items.Add("Add Item...");
                    cmbBxExistingHairColors.SelectedIndex = 0;
                    valuesChanged = true;
                }
                else
                {
                    MessageBox.Show("Expected format is \"<tag>[,|;] <explanation>[,|;] <level>", "Format info", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }
        private void cmbBxExistingEyeColors_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selIndex = cmbBxExistingEyeColors.SelectedIndex;
            if (selIndex > 0)
            {
                if ((cmbBxExistingEyeColors.SelectedItem.ToString() == "Add Item...") || ((selIndex - 1) > noOfEyeColors))
                {
                    txtBxEyeColorToAdd.Text = "";
                    txtBxEyeColorToAdd.SetBounds(10, 525, 220, 20);
                    txtBxEyeColorToAdd.Enabled = true;
                    btnAddEyeColor.Visible = true;
                    btnAddEyeColor.Enabled = true;
                }
                else
                {
                    txtBxEyeColorToAdd.Text = eyeColorsDefined[selIndex - 1].tag + ", " + eyeColorsDefined[selIndex - 1].description + ", " + eyeColorsDefined[selIndex - 1].catReqReq.ToString();
                    txtBxEyeColorToAdd.SetBounds(10, 525, 275, 20);
                    btnAddEyeColor.Visible = false;
                    btnAddEyeColor.Enabled = false;
                }
            }
        }
        private void txtBxEyeColorToAdd_TextChanged(object sender, EventArgs e)
        {
            // Do nothing, this is handled in btnAddEyeColor_Click.
            btnAddEyeColor.Visible = true;
            btnAddEyeColor.Enabled = true;
        }
        private void btnAddEyeColor_Click(object sender, EventArgs e)
        {
            string theNewInfo = txtBxEyeColorToAdd.Text;
            int dp0 = theNewInfo.IndexOf(",");
            if ((dp0 > 0) && (dp0 < theNewInfo.Length - 2))
            {
                string theNewTag = theNewInfo.Substring(0, dp0);
                if ((theNewTag != "Undefined") && (theNewTag != "Unknown") && (theNewTag != "undefined") && (theNewTag != "unknown"))
                    eyeColorsDefined[noOfEyeColors].tag = theNewTag;
                else
                {
                    MessageBox.Show("Expected a tag!", "Format info", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                theNewInfo = theNewInfo.Substring(dp0 + 2, theNewInfo.Length - dp0 - 2);
                int dp1 = theNewInfo.IndexOf(",");
                if ((dp1 > 0) && (dp1 < theNewInfo.Length - 2))
                {
                    string theNewDescription = theNewInfo.Substring(0, dp1);
                    if ((theNewDescription != "Undefined") && (theNewDescription != "Unknown") && (theNewDescription != "undefined") && (theNewDescription != "unknown"))
                        eyeColorsDefined[noOfEyeColors].description = theNewDescription;
                    else
                    {
                        MessageBox.Show("Expected a description!", "Format info", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                    theNewInfo = theNewInfo.Substring(dp1 + 2, theNewInfo.Length - dp1 - 2);
                    if ((theNewInfo == "Open") || (theNewInfo == "open"))
                        eyeColorsDefined[noOfEyeColors].catReqReq = rightsLevel.Open;
                    else if ((theNewInfo == "Limited") || (theNewInfo == "limited"))
                        eyeColorsDefined[noOfEyeColors].catReqReq = rightsLevel.Limited;
                    else if ((theNewInfo == "Medium") || (theNewInfo == "medium"))
                        eyeColorsDefined[noOfEyeColors].catReqReq = rightsLevel.Medium;
                    else if ((theNewInfo == "Relative") || (theNewInfo == "relative"))
                        eyeColorsDefined[noOfEyeColors].catReqReq = rightsLevel.Relative;
                    else if ((theNewInfo == "Secret") || (theNewInfo == "secret"))
                        eyeColorsDefined[noOfEyeColors].catReqReq = rightsLevel.Secret;
                    else if ((theNewInfo == "Qualified") || (theNewInfo == "qualified") || (theNewInfo == "QualifiedSecret") || (theNewInfo == "qualifiedsecret"))
                        eyeColorsDefined[noOfEyeColors].catReqReq = rightsLevel.QualifSecret;
                    else
                        eyeColorsDefined[noOfEyeColors].catReqReq = rightsLevel.Undefined;
                    noOfEyeColors++;
                }
            }
            else
            {
                dp0 = theNewInfo.IndexOf(";");
                if ((dp0 > 0) && (dp0 < theNewInfo.Length - 2))
                {
                    string theNewTag = theNewInfo.Substring(0, dp0);
                    if ((theNewTag != "Undefined") && (theNewTag != "Unknown") && (theNewTag != "undefined") && (theNewTag != "unknown"))
                        eyeColorsDefined[noOfEyeColors].tag = theNewTag;
                    else
                    {
                        MessageBox.Show("Expected a tag!", "Format info", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                    theNewInfo = theNewInfo.Substring(dp0 + 2, theNewInfo.Length - dp0 - 2);
                    int dp1 = theNewInfo.IndexOf(";");
                    if ((dp1 > 0) && (dp1 < theNewInfo.Length - 2))
                    {
                        string theNewDescription = theNewInfo.Substring(0, dp1);
                        if ((theNewDescription != "Undefined") && (theNewDescription != "Unknown") && (theNewDescription != "undefined") && (theNewDescription != "unknown"))
                            eyeColorsDefined[noOfEyeColors].description = theNewDescription;
                        else
                        {
                            MessageBox.Show("Expected a description!", "Format info", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return;
                        }
                        theNewInfo = theNewInfo.Substring(dp1 + 2, theNewInfo.Length - dp1 - 2);
                        if ((theNewInfo == "Open") || (theNewInfo == "open"))
                            eyeColorsDefined[noOfEyeColors].catReqReq = rightsLevel.Open;
                        else if ((theNewInfo == "Limited") || (theNewInfo == "limited"))
                            eyeColorsDefined[noOfEyeColors].catReqReq = rightsLevel.Limited;
                        else if ((theNewInfo == "Medium") || (theNewInfo == "medium"))
                            eyeColorsDefined[noOfEyeColors].catReqReq = rightsLevel.Medium;
                        else if ((theNewInfo == "Relative") || (theNewInfo == "relative"))
                            eyeColorsDefined[noOfEyeColors].catReqReq = rightsLevel.Relative;
                        else if ((theNewInfo == "Secret") || (theNewInfo == "secret"))
                            eyeColorsDefined[noOfEyeColors].catReqReq = rightsLevel.Secret;
                        else if ((theNewInfo == "Qualified") || (theNewInfo == "qualified") || (theNewInfo == "QualifiedSecret") || (theNewInfo == "qualifiedsecret"))
                            eyeColorsDefined[noOfEyeColors].catReqReq = rightsLevel.QualifSecret;
                        else
                            eyeColorsDefined[noOfEyeColors].catReqReq = rightsLevel.Undefined;
                        noOfEyeColors++;
                    }
                    else
                    {
                        MessageBox.Show("Expected format is \"<tag>[,|;] <explanation>[,|;] <level>", "Format info", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    txtBxEyeColorToAdd.Text = "";
                    txtBxEyeColorToAdd.SetBounds(10, 525, 275, 20);
                    txtBxEyeColorToAdd.Enabled = false;
                    btnAddEyeColor.Enabled = false;
                    btnAddEyeColor.Visible = false;
                    cmbBxExistingEyeColors.Items.Clear();
                    cmbBxExistingEyeColors.Items.Add("Select...");
                    for (int i = 0; i < noOfEyeColors; i++)
                        cmbBxExistingEyeColors.Items.Add(eyeColorsDefined[i].tag);
                    cmbBxExistingEyeColors.Items.Add("Add Item...");
                    cmbBxExistingEyeColors.SelectedIndex = 0;
                    valuesChanged = true;
                }
                else
                {
                    MessageBox.Show("Expected format is \"<tag>[,|;] <explanation>[,|;] <level>", "Format info", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }
        private void cmbBxExistingRoles_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selIndex = cmbBxExistingRoles.SelectedIndex;
            if (selIndex > 0)
            {
                if ((cmbBxExistingRoles.SelectedIndex.ToString() == "Add Item...") || ((selIndex - 1) > noOfRoles))
                {
                    txtBxRoleToAdd.Text = "";
                    txtBxRoleToAdd.SetBounds(10, 575, 220, 20);
                    txtBxRoleToAdd.Enabled = true;
                    btnAddRole.Visible = true;
                    btnAddRole.Enabled = true;
                }
                else
                {
                    txtBxRoleToAdd.Text = rolesDefined[selIndex - 1].tag + ", " + rolesDefined[selIndex - 1].description + ", " + rolesDefined[selIndex - 1].catReqReq.ToString();
                    txtBxRoleToAdd.SetBounds(10, 575, 275, 20);
                    btnAddRole.Visible = false;
                    btnAddRole.Enabled = false;
                }
            }
        }
        private void txtBxRoleToAdd_TextChanged(object sender, EventArgs e)
        {
            // Do nothing, this is handled in btnAddRole_Click.
            btnAddRole.Visible = true;
            btnAddRole.Enabled = true;
        }
        private void btnAddRole_Click(object sender, EventArgs e)
        {
            string theNewInfo = txtBxRoleToAdd.Text;
            int dp0 = theNewInfo.IndexOf(",");
            if ((dp0 > 0) && (dp0 < theNewInfo.Length - 2))
            {
                string theNewTag = theNewInfo.Substring(0, dp0);
                if ((theNewTag != "Undefined") && (theNewTag != "Unknown") && (theNewTag != "undefined") && (theNewTag != "unknown"))
                    rolesDefined[noOfRoles].tag = theNewTag;
                else
                {
                    MessageBox.Show("Expected a tag!", "Format info", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                theNewInfo = theNewInfo.Substring(dp0 + 2, theNewInfo.Length - dp0 - 2);
                int dp1 = theNewInfo.IndexOf(",");
                if ((dp1 > 0) && (dp1 < theNewInfo.Length - 2))
                {
                    string theNewDescription = theNewInfo.Substring(0, dp1);
                    if ((theNewDescription != "Undefined") && (theNewDescription != "Unknown") && (theNewDescription != "undefined") && (theNewDescription != "unknown"))
                        rolesDefined[noOfRoles].description = theNewDescription;
                    else
                    {
                        MessageBox.Show("Expected a description!", "Format info", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                    theNewInfo = theNewInfo.Substring(dp1 + 2, theNewInfo.Length - dp1 - 2);
                    if ((theNewInfo == "Open") || (theNewInfo == "open"))
                        rolesDefined[noOfRoles].catReqReq = rightsLevel.Open;
                    else if ((theNewInfo == "Limited") || (theNewInfo == "limited"))
                        rolesDefined[noOfRoles].catReqReq = rightsLevel.Limited;
                    else if ((theNewInfo == "Medium") || (theNewInfo == "medium"))
                        rolesDefined[noOfRoles].catReqReq = rightsLevel.Medium;
                    else if ((theNewInfo == "Relative") || (theNewInfo == "relative"))
                        rolesDefined[noOfRoles].catReqReq = rightsLevel.Relative;
                    else if ((theNewInfo == "Secret") || (theNewInfo == "secret"))
                        rolesDefined[noOfRoles].catReqReq = rightsLevel.Secret;
                    else if ((theNewInfo == "Qualified") || (theNewInfo == "qualified") || (theNewInfo == "QualifiedSecret") || (theNewInfo == "qualifiedsecret"))
                        rolesDefined[noOfRoles].catReqReq = rightsLevel.QualifSecret;
                    else
                        rolesDefined[noOfRoles].catReqReq = rightsLevel.Undefined;
                    noOfRoles++;
                }
                else
                {
                    MessageBox.Show("Expected format is \"<tag>[,|;] <explanation>[,|;] <level>", "Format info", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                txtBxRoleToAdd.Text = "";
                txtBxRoleToAdd.SetBounds(10, 575, 275, 20);
                txtBxRoleToAdd.Enabled = false;
                btnAddRole.Enabled = false;
                btnAddRole.Visible = false;
                cmbBxExistingRoles.Items.Clear();
                cmbBxExistingRoles.Items.Add("Select...");
                for (int i = 0; i < noOfRoles; i++)
                    cmbBxExistingRoles.Items.Add(rolesDefined[i].tag);
                cmbBxExistingRoles.Items.Add("Add Item...");
                cmbBxExistingRoles.SelectedIndex = 0;
                valuesChanged = true;
            }
            else
            {
                dp0 = theNewInfo.IndexOf(";");
                if ((dp0 > 0) && (dp0 < theNewInfo.Length - 2))
                {
                    string theNewTag = theNewInfo.Substring(0, dp0);
                    if ((theNewTag != "Undefined") && (theNewTag != "Unknown") && (theNewTag != "undefined") && (theNewTag != "unknown"))
                        rolesDefined[noOfRoles].tag = theNewTag;
                    else
                    {
                        MessageBox.Show("Expected a tag!", "Format info", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                    theNewInfo = theNewInfo.Substring(dp0 + 2, theNewInfo.Length - dp0 - 2);
                    int dp1 = theNewInfo.IndexOf(";");
                    if ((dp1 > 0) && (dp1 < theNewInfo.Length - 2))
                    {
                        string theNewDescription = theNewInfo.Substring(0, dp1);
                        if ((theNewDescription != "Undefined") && (theNewDescription != "Unknown") && (theNewDescription != "undefined") && (theNewDescription != "unknown"))
                            rolesDefined[noOfRoles].description = theNewDescription;
                        else
                        {
                            MessageBox.Show("Expected a description!", "Format info", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return;
                        }
                        theNewInfo = theNewInfo.Substring(dp1 + 2, theNewInfo.Length - dp1 - 2);
                        if ((theNewInfo == "Open") || (theNewInfo == "open"))
                            rolesDefined[noOfRoles].catReqReq = rightsLevel.Open;
                        else if ((theNewInfo == "Limited") || (theNewInfo == "limited"))
                            rolesDefined[noOfRoles].catReqReq = rightsLevel.Limited;
                        else if ((theNewInfo == "Medium") || (theNewInfo == "medium"))
                            rolesDefined[noOfRoles].catReqReq = rightsLevel.Medium;
                        else if ((theNewInfo == "Relative") || (theNewInfo == "relative"))
                            rolesDefined[noOfRoles].catReqReq = rightsLevel.Relative;
                        else if ((theNewInfo == "Secret") || (theNewInfo == "secret"))
                            rolesDefined[noOfRoles].catReqReq = rightsLevel.Secret;
                        else if ((theNewInfo == "Qualified") || (theNewInfo == "qualified") || (theNewInfo == "QualifiedSecret") || (theNewInfo == "qualifiedsecret"))
                            rolesDefined[noOfRoles].catReqReq = rightsLevel.QualifSecret;
                        else
                            rolesDefined[noOfRoles].catReqReq = rightsLevel.Undefined;
                        noOfRoles++;
                    }
                    else
                    {
                        MessageBox.Show("Expected format is \"<tag>[,|;] <explanation>[,|;] <level>", "Format info", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    txtBxRoleToAdd.Text = "";
                    txtBxRoleToAdd.SetBounds(10, 575, 275, 20);
                    txtBxRoleToAdd.Enabled = false;
                    btnAddRole.Enabled = false;
                    btnAddRole.Visible = false;
                    cmbBxExistingRoles.Items.Clear();
                    cmbBxExistingRoles.Items.Add("Select...");
                    for (int i = 0; i < noOfRoles; i++)
                        cmbBxExistingRoles.Items.Add(rolesDefined[i].tag);
                    cmbBxExistingRoles.Items.Add("Add Item...");
                    cmbBxExistingRoles.SelectedIndex = 0;
                    valuesChanged = true;
                }
                else
                {
                    MessageBox.Show("Expected format is \"<tag>[,|;] <explanation>[,|;] <level>", "Format info", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }
        private void btnActorStorage_Click(object sender, EventArgs e)
        {
            DialogResult dres = folderBrowserDialogActorStorage.ShowDialog();
            this.actorStoragePath = folderBrowserDialogActorStorage.SelectedPath.ToString();
            txtBoxSetActorStorage.Text = actorStoragePath;
        }
        private void btnEventStorage_Click(object sender, EventArgs e)
        {
            DialogResult dres = folderBrowserDialogEventStorage.ShowDialog();
            this.eventStoragePath = folderBrowserDialogEventStorage.SelectedPath.ToString();
            txtBoxSetEventStorage.Text = eventStoragePath;
        }
    }
}
