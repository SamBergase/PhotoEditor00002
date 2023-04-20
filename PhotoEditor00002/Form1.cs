using MetadataExtractor;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Security.AccessControl;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Globalization;

using System.Runtime.InteropServices;
//[DllImport("kernel32.dll", CharSet = CharSet.Unicode)]

using myLoginForm;
//using myHistogramForm;
//using LoginFunction;
//using GroupDocs.Metadata;
//using GroupDocs.Metadata.Standards.Exif;
//using System.Runtime.Remoting;
//using System.Runtime.Remoting.MetadataServices;

namespace PhotoEditor00002
{
    public enum informationType
    {
        UNDEF = 0,
        INFO = 1,
        ERROR = 2,
        FATAL = 3
    }
    public enum programMode
    {
        IMAGEVIEW = 0,
        DIRECTORYVIEW = 1,
        RESTOREVIEW = 2,
        SORTINGVIEW = 3,
        ACTORVIEW = 4,
        EVENTVIEW = 5,
        USERVIEW = 6
    }

    public partial class Form1 : Form
    {
        public programMode currentMode;
        public string fileContent = string.Empty;
        public string filePath = string.Empty;
        public int noOfRegisteredActors;
        public string[] actorFilePaths;
        public int noOfRegisteredEvents;
        public string[] eventFilePaths;
        public Image picture;
        public string detFileTypeName;
        public int usrCmtMetadataID;
        public bool changesToSave = false;
        public bool startUpDone = false;
        public bool userCommentChanged = false;
        public bool imageTitleChanged = false;
        public bool subjectChanged = false;
        public bool geoLatValueChanged = false;
        public bool geoLonValueChanged = false;
        public bool bhsOwnerChanged = false;
        public int recovLevel = -2;
        public int recovType = -2;

        const int maxNoOfLoadedActors = 16;
        int numberOfUsedActors = 0;
        ActorClass[] arrayOfActors = new ActorClass[maxNoOfLoadedActors];
        ActorClass actorClass;

        const int maxNoOfLoadedEvents = 16;
        int numberOfLoadedEvents = 0;
        PEEventClass[] arrayOfEvents = new PEEventClass[maxNoOfLoadedEvents];
        PEEventClass eventClass;

        private string loadedImageFileName;
//        private bool artistListBoxActive = false;
        private string currUser = WindowsIdentity.GetCurrent().Name;
        private string sProgPath = "source\\repos\\PhotoEditor00002\\PhotoEditor00002";
        private string sLogPath = "source\\repos\\PhotoEditor00002\\PhotoEditor00002\\Logs\\" + DateTime.Now.ToString("yyyy-MM-dd-hhmmss") + ".log";
        private string rootPath = "";
        private string logPath = "";
        private string sFSPath = "";
        private bool expandedImage = false;
        private int startval = 0;
        private int stopval = int.MaxValue;
//        private string smplRootPath = "";
//        FileStream fsLog;

        private Button[] sortButtons;
        private int maxSortButtons = 22, noOfSortingButtons = 0;
        private String baseFolderName;
        private String[] arrayItemIndex = new string[2000];        
        private LoginForm linwin;
        public Form1()
        {
            Login_Win_Open();
            InitializeComponent();
            sortButtons = new[] { sortingButton1, sortingButton2, sortingButton3, sortingButton4, sortingButton5, sortingButton6, sortingButton7,
                sortingButton8, sortingButton9, sortingButton10, sortingButton11, sortingButton12, sortingButton13, sortingButton14,
                sortingButton15, sortingButton16, sortingButton17, sortingButton18, sortingButton19, sortingButton20, sortingButton21,
                sortingButton22 };
            int tnr = currUser.IndexOf("\\");
            if ((tnr > 0) && (tnr < currUser.Length - 1))
                currUser = currUser.Substring(tnr + 1, currUser.Length - tnr - 1);
            rootPath = "C:\\Users\\" + currUser + "\\" + sProgPath;
            logPath = "C:\\Users\\" + currUser + "\\" + sLogPath;    
        }
        // --- Private Support functions ---
        private void setInformationText(string inString, informationType inInfoType, object sender, EventArgs e)
        {
            switch (inInfoType)
            {
                case informationType.INFO:
                    {
                        informationTextBox.Text = System.DateTime.Now.ToString("yyyy-MM-dd - HH:mm:ss.ff") + " : Information : " + inString;
                        using (StreamWriter sw = File.AppendText(logPath))
                            sw.WriteLine(System.DateTime.Now.ToString("yyyy-MM-dd - HH:mm:ss.ff") + " : Information : " + inString);
                    }
                    break;
                case informationType.ERROR:
                    {
                        informationTextBox.Text = System.DateTime.Now.ToString("yyyy-MM-dd - HH:mm:ss.ff") + " : ERROR : " + inString;
                        using (StreamWriter sw = File.AppendText(logPath))
                            sw.WriteLine(System.DateTime.Now.ToString("yyyy-MM-dd - HH:mm:ss.ff") + " : ERROR : " + inString);
                    }
                    break;
                case informationType.FATAL:
                    {
                        informationTextBox.Text = System.DateTime.Now.ToString("yyyy-MM-dd - HH:mm:ss.ff") + " : FATAL ERROR : " + inString + " : " + sender.ToString();
                        using (StreamWriter sw = File.AppendText(logPath))
                            sw.WriteLine(System.DateTime.Now.ToString("yyyy-MM-dd - HH:mm:ss.ff") + " : FATAL ERROR : " + inString);
                        CloseDown(sender, e);
                    }
                    break;
                default:
                    {
                        informationTextBox.Text = System.DateTime.Now.ToString("yyyy-MM-dd - HH:mm:ss.ff") + " : " + inString;
                        using (StreamWriter sw = File.AppendText(logPath))
                            sw.WriteLine(System.DateTime.Now.ToString("yyyy-MM-dd - HH:mm:ss.ff") + " :  " + inString);
                    }
                    break;
            }
        }
        private static ImageCodecInfo GetEncoderInfo(string mimeType)
        {
            ImageCodecInfo[] encoders;
            encoders = ImageCodecInfo.GetImageEncoders();
            for (int j = 0; j < encoders.Length; ++j)
            {
                if (encoders[j].MimeType == mimeType)
                    return encoders[j];
            }
            return null;
        }
        private void CloseDown(object sender, EventArgs e)
        {
            if (changesToSave)
            {
                var result = MessageBox.Show("Save changes to the image?", "Existing changes", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                    try { picture.Save(loadedImageFileName); } catch { }
                picture.Dispose();
            }
            setInformationText("Closing the program", informationType.INFO, sender, e);
//            try { linwin.saveUserData(); } catch { setInformationText("Cought an exeption when saving user data.", informationType.ERROR, sender, e); }
            //try { LoginForm.Close}
            try { Dispose(); } catch { setInformationText("Cought an exeption when disposing.", informationType.ERROR, sender, e); }
            try { Close(); } catch { setInformationText("Cought an exeption when closing the program.", informationType.ERROR, sender, e); }
        }
        // --- Main menu items ---
        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            // Note! Intentionally left empty, clicking the menu-strip should not render any action.
        }
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (linwin.validated)
            {
                if (changesToSave)
                {
                    var result = MessageBox.Show("Save changes to the image?", "Existing changes", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                        try { picture.Save(loadedImageFileName); } catch { }
                }
                cleanImageGUIMetaDataValues();
                OpenFileDialog ofd = new OpenFileDialog();
                if (linwin.getLastImageDirectoryValue() == "")
                    ofd.InitialDirectory = "c:\\";
                else
                    ofd.InitialDirectory = linwin.getLastImageDirectoryValue();
                ofd.Filter = "jpg files (*.jpg)|*.jpg|" +
                             "gif files (*.gif)|*.gif|" +
                             "bmp files (*.bmp)|*.bmp|" +
                             "mpg files (*.mpg)|*.mpg|" +
                             "mp3 files (*.mp3)|*.mp3|" +
                             "all files (*.*)|*.*";
                ofd.FilterIndex = 6;
                ofd.RestoreDirectory = true;
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    loadedImageFileName = ofd.FileName;
                    int dpp = loadedImageFileName.LastIndexOf("\\");
                    linwin.setLastImageDirectoryValue(loadedImageFileName.Substring(0, dpp));

                    picture = Image.FromFile(ofd.FileName);
                    pictureCanvas.SizeMode = PictureBoxSizeMode.Zoom;
                    getImageMetadataValues(ofd.FileName, sender, e);
                    pictureCanvas.Image = picture;
                    pictureName.Text = ofd.FileName;
                    startUpDone = true;
                    expandedImage = true;
                    saveAsToolStripMenuItem.Enabled = true;
                }
                ofd.Dispose();
                this.imageViewToolStripMenuItem_Click(sender, e);
            }
            else
            {
                setInformationText("User validation needed", informationType.INFO, sender, e);
            }
        }
        private void cleanImageGUIMetaDataValues()
        {
            ImageTitleTextBox.Text = "";
            ISOSpeedRatTextBox.Text = "";
            ExpProgTextBox.Text = "";
            FNumTextBox.Text = "";
            ExpTimeTextBox.Text = "";
            CopyrightTextBox.Text = "";
            YCbCrPosTextBox.Text = "";
            ActiveArtistsComboBox.Items.Clear();
            ImageDateTimeTextBox.Text = "";
            CompTypeTextBox.Text = "";
            DataPrecisionTextBox.Text = "";
            ImageHeightTextBox.Text = "";
            ImageWidthTextBox.Text = "";
            HWMakeTextBox.Text = "";
            HWModelTextBox.Text = "";
            ImageOrientationTextBox.Text = "";
            ImageXResTextBox.Text = "";
            ImageYResTextBox.Text = "";
            SensitivityTypeTextBox.Text = "";
            RekExpIndexTextBox.Text = "";
            ExifVersionTextBox.Text = "";
            OriginalDateTimeTextBox.Text = "";
            DigitizedDateTimeTextBox.Text = "";
            ComponentConfigTextBox.Text = "";
            ShutterSpeedTextBox.Text = "";
            ApertureTextBox.Text = "";
            ExpBiasValueTextBox.Text = "";
            MeteringModeTextBox.Text = "";
            FlashTextBox.Text = "";
            FocalLengthTextBox.Text = "";
            UserCommentTextBox.Text = "";
            SubSecTimeTextBox.Text = "";
            OrigSubSecTimeTextBox.Text = "";
            DigSubSecTimeTextBox.Text = "";
            FlashPixVersionTextBox.Text = "";
            ColorSpaceTextBox.Text = "";
            ExifImageWidthTextBox.Text = "";
            ExifImageHeightTextBox.Text = "";
            FocalPlaneXResTextBox.Text = "";
            FocalPlaneYResTextBox.Text = "";
            CustomRenderedTextBox.Text = "";
            ExpModeTextBox.Text = "";
            WhiteBalanceModeTextBox.Text = "";
            SceneCaptureTextBox.Text = "";
            OwnerTextBox.Text = "";
            SerialNumberTextBox.Text = "";
            LensSpecTextBox.Text = "";
            LensModelTextBox.Text = "";
            LensSerNoTextBox.Text = "";
            MacroModeTextBox.Text = "";
            SelfTimerDelayTextBox.Text = "";
            QualityTextBox.Text = "";
            FlashModeTextBox.Text = "";
            ContinousDriveTextBox.Text = "";
            FocusModeTextBox.Text = "";
            RecordModeTextBox.Text = "";
            ImageSizeTextBox.Text = "";
            EasyShootingModeTextBox.Text = "";
            DigitalZoomTextBox.Text = "";
            ContrastTextBox.Text = "";
            SaturationTextBox.Text = "";
            SharpnessInfoTextBox.Text = "";
            ISODataTextBox.Text = "";
            FocusMeteringInfoTextBox.Text = "";
            FocusTypeInfoTextBox.Text = "";
            AFPointSelectedInfoTextBox.Text = "";
            LensTypeInfoTextBox.Text = "";
            FocalMaxLengthInfoTextBox.Text = "";
            FocalMinLengthInfoTextBox.Text = "";
            FocalUnitInfoTextBox.Text = "";
            MaxApertureTextBox.Text = "";
            MinApertureTextBox.Text = "";
            ApertureTextBox.Text = "";
            FlashActivityInfoTextBox.Text = "";
            FlashDetailsTextBox.Text = "";
            ContinouosFocusTextBox.Text = "";
            AESettingsTextBox.Text = "";
            DisplayApertureTextBox.Text = "";
            ZoomSourceWidthTextBox.Text = "";
            ZoomSourceHeightTextBox.Text = "";
            SpotMeteringModeTextBox.Text = "";
            LatRefTextBox.Text = "";
            LatValueTextBox.Text = "";
            LonRefTextBox.Text = "";
            LonValueTextBox.Text = "";
            subjectTextBox.Text = "";
            activeEventComboBox.Items.Clear();
            AttdEventsDateComboBox.Items.Clear();
            ChangeDateTimeTextBox.Text = "";
            noOfComponentsTextBox.Text = "";
            detFileTypeName = "";
            EventAttenderIDComboBox1.Items.Clear();
            EventImageNameComboBox.Items.Clear();
        }
        private void getImageMetadataValues(string instr, object sender, EventArgs e)
        {
            if (linwin.validated)
            {
                var directories = ImageMetadataReader.ReadMetadata(instr);
                activeEventComboBox.Items.Add("Select...");
                AttdEventsDateComboBox.Items.Add("Select...");
                foreach (var directory in directories)
                {
                    foreach (var tag in directory.Tags)
                    {
                        switch (tag.Name)
                        {
                            case "ISO Speed Ratings":
                                {
                                    // Property Value = 0x8827
                                    ISOSpeedRatTextBox.Text = tag.Description;
                                } break;
                            case "Exposure Program":
                                {
                                    // Property Value = 0x8822
                                    ExpProgTextBox.Text = tag.Description;
                                } break;
                            case "F-Number":
                                {
                                    // Property Value = 0x829d
                                    FNumTextBox.Text = tag.Description;
                                } break;
                            case "Exposure Time":
                                {
                                    // Property Value = 0x829a
                                    ExpTimeTextBox.Text = tag.Description;
                                } break;
                            case "Copyright":
                                {
                                    // Property Value = 0x503b
                                    CopyrightTextBox.Text = tag.Description;
                                } break;
                            case "YCbCr Positioning":
                                {
                                    // Property Value = 0x0213
                                    YCbCrPosTextBox.Text = tag.Description;
                                } break;
                            case "Artist":
                                {
                                    // Property Value = 315 = 0x013b
                                    string attActors = tag.Description;
                                    int delimPos = attActors.IndexOf(";");
                                    if ((delimPos > 0) && (delimPos < attActors.Length - 1))
                                    {
                                        string currActor = attActors.Substring(0, delimPos);
                                        while ((delimPos > 0) && (delimPos < attActors.Length - 1))
                                        {
                                            ActiveArtistsComboBox.Items.Add(currActor);
                                            attActors = attActors.Substring(delimPos + 1, attActors.Length - delimPos - 1);
                                            delimPos = attActors.IndexOf(";");
                                            if ((delimPos > 0) && (delimPos < attActors.Length - 1))
                                                currActor = attActors.Substring(0, delimPos);
                                            else
                                                currActor = attActors;
                                        }
                                        ActiveArtistsComboBox.Items.Add(currActor);
                                    }
                                    else
                                    {
                                        ActiveArtistsComboBox.Items.Add(attActors);
                                    }
                                } break;
                            case "Date/Time":
                                {
                                    // Property Value = 0x0132
                                    ImageDateTimeTextBox.Text = tag.Description;
                                } break;
                            case "Compression Type":
                                {
                                    // Property Value = 0x0103
                                    CompTypeTextBox.Text = tag.Description;
                                } break;
                            case "Data Precision":
                                {
                                    // Property Value = ?
                                    DataPrecisionTextBox.Text = tag.Description;
                                } break;
                            case "Image Height":
                                {
                                    // Property Value = 0x0101
                                    ImageHeightTextBox.Text = tag.Description;
                                } break;
                            case "Image Width":
                                {
                                    // Property Value = 0x0100
                                    ImageWidthTextBox.Text = tag.Description;
                                } break;
                            case "Make":
                                {
                                    // Property Value = 271 = 0x010f
                                    HWMakeTextBox.Text = tag.Description;
                                } break;
                            case "Model":
                                {
                                    // Property Value = 272 = 0x0110
                                    HWModelTextBox.Text = tag.Description;
                                } break;
                            case "Orientation":
                                {
                                    // Property Value = 0x0112
                                    ImageOrientationTextBox.Text = tag.Description;
                                } break;
                            case "X Resolution":
                                {
                                    // Property Value = 0x011a
                                    ImageXResTextBox.Text = tag.Description;
                                } break;
                            case "Y Resolution":
                                {
                                    // Property Value = 0x011b
                                    ImageYResTextBox.Text = tag.Description;
                                } break;
                            case "Sensitivity Type":
                                {
                                    // Property Value = 
                                    SensitivityTypeTextBox.Text = tag.Description;
                                } break;
                            case "Recommended Exposure Index":
                                {
                                    // Property Value = 0xa215
                                    RekExpIndexTextBox.Text = tag.Description;
                                } break;
                            case "Exif Version":
                                {
                                    if (ExifVersionTextBox.Text == "")
                                        ExifVersionTextBox.Text = tag.Description;
                                } break;
                            case "Version":
                                {
                                    if (ExifVersionTextBox.Text == "")
                                        ExifVersionTextBox.Text = tag.Description;
                                } break;
                            case "Date/Time Original":
                                {
                                    // Property Value = 0x9003
                                    OriginalDateTimeTextBox.Text = tag.Description;
                                } break;
                            case "Date/Time Digitized":
                                {
                                    // Property Value = 0x9292
                                    DigitizedDateTimeTextBox.Text = tag.Description;
                                } break;
                            case "Components Configuration":
                                {
                                    // Property Value = 0x9101
                                    ComponentConfigTextBox.Text = tag.Description;
                                } break;
                            case "Shutter Speed Value":
                                {
                                    // Property Value = 0x9201
                                    ShutterSpeedTextBox.Text = tag.Description;
                                } break;
                            case "Aperture Data":
                                {
                                    // Property Value = 0x9202
                                    ApertureTextBox.Text = tag.Description;
                                } break;
                            case "Exposure Bias Value":
                                {
                                    // Property Value = 0x9204
                                    ExpBiasValueTextBox.Text = tag.Description;
                                } break;
                            case "Metering Mode":
                                {
                                    // Property Value = 0x9207
                                    MeteringModeTextBox.Text = tag.Description;
                                } break;
                            case "Flash":
                                {
                                    // Property Value = 0x9209
                                    FlashTextBox.Text = tag.Description;
                                } break;
                            case "Focal Length":
                                {
                                    // Property Value = 0x920a
                                    FocalLengthTextBox.Text = tag.Description;
                                } break;
                            case "User Comment":
                                {
                                    if (UserCommentTextBox.Text != "")
                                        UserCommentTextBox.Text = UserCommentTextBox.Text.ToString() + Environment.NewLine + tag.Description;
                                    else
                                        UserCommentTextBox.Text = tag.Description;
                                } break;
                            case "Sub-Sec Time":
                                {
                                    // Property Value = 
                                    SubSecTimeTextBox.Text = tag.Description;
                                } break;
                            case "Sub-Sec Time Original":
                                {
                                    OrigSubSecTimeTextBox.Text = tag.Description;
                                } break;
                            case "Sub-Sec Time Digitized":
                                {
                                    DigSubSecTimeTextBox.Text = tag.Description;
                                } break;
                            case "FlashPix Version":
                                {
                                    FlashPixVersionTextBox.Text = tag.Description;
                                } break;
                            case "Color Space":
                                {
                                    ColorSpaceTextBox.Text = tag.Description;
                                } break;
                            case "Exif Image Width":
                                {
                                    ExifImageWidthTextBox.Text = tag.Description;
                                } break;
                            case "Exif Image Height":
                                {
                                    ExifImageHeightTextBox.Text = tag.Description;
                                } break;
                            case "Focal Plane X Resolution":
                                {
                                    FocalPlaneXResTextBox.Text = tag.Description;
                                } break;
                            case "Focal Plane Y Resolution":
                                {
                                    FocalPlaneYResTextBox.Text = tag.Description;
                                } break;
                            case "Custom Rendered":
                                {
                                    CustomRenderedTextBox.Text = tag.Description;
                                } break;
                            case "Exposure Mode":
                                {
                                    ExpModeTextBox.Text = tag.Description;
                                } break;
                            case "White Balance Mode":
                                {
                                    WhiteBalanceModeTextBox.Text = tag.Description;
                                } break;
                            case "Scene Capture Type":
                                {
                                    SceneCaptureTextBox.Text = tag.Description;
                                } break;
                            case "Camera Owner Name":
                                {
                                    // tagName = "Camera Owner Name"
                                    // tagType = 42032
                                    OwnerTextBox.Text = tag.Description;
                                } break;
                            case "Body Serial Number":
                                {
                                    SerialNumberTextBox.Text = tag.Description;
                                } break;
                            case "Lens Specification":
                                {
                                    LensSpecTextBox.Text = tag.Description;
                                } break;
                            case "Lens Model":
                                {
                                    LensModelTextBox.Text = tag.Description;
                                } break;
                            case "Lens Serial Number":
                                {
                                    LensSerNoTextBox.Text = tag.Description;
                                } break;
                            case "Macro Mode":
                                {
                                    MacroModeTextBox.Text = tag.Description;
                                } break;
                            case "Self Timer Delay":
                                {
                                    SelfTimerDelayTextBox.Text = tag.Description;
                                } break;
                            case "Quality":
                                {
                                    QualityTextBox.Text = tag.Description;
                                } break;
                            case "Flash Mode":
                                {
                                    FlashModeTextBox.Text = tag.Description;
                                } break;
                            case "Continuous Drive Mode":
                                {
                                    ContinousDriveTextBox.Text = tag.Description;
                                } break;
                            case "Focus Mode":
                                {
                                    FocusModeTextBox.Text = tag.Description;
                                } break;
                            case "Record Mode":
                                {
                                    RecordModeTextBox.Text = tag.Description;
                                } break;
                            case "Image Size":
                                {
                                    if (ImageSizeTextBox.Text != "")
                                        ImageSizeTextBox.Text = ImageSizeTextBox.Text.ToString() + ", " + tag.Description;
                                    else
                                        ImageSizeTextBox.Text = tag.Description;
                                } break;
                            case "File Size":
                                {
                                    if (ImageSizeTextBox.Text != "")
                                        ImageSizeTextBox.Text = ImageSizeTextBox.Text.ToString() + ", " + tag.Description;
                                    else
                                        ImageSizeTextBox.Text = tag.Description;
                                } break;
                            case "Easy Shooting Mode":
                                {
                                    EasyShootingModeTextBox.Text = tag.Description;
                                } break;
                            case "Digital Zoom":
                                {
                                    DigitalZoomTextBox.Text = tag.Description;
                                } break;
                            case "Contrast":
                                {
                                    ContrastTextBox.Text = tag.Description;
                                } break;
                            case "Saturation":
                                {
                                    SaturationTextBox.Text = tag.Description;
                                } break;
                            case "Sharpness":
                                {
                                    SharpnessInfoTextBox.Text = tag.Description;
                                } break;
                            case "Iso":
                                {
                                    ISODataTextBox.Text = tag.Description;
                                } break;
                            case "Metering mode":
                                {
                                    FocusMeteringInfoTextBox.Text = tag.Description;
                                } break;
                            case "Focus Type":
                                {
                                    FocusTypeInfoTextBox.Text = tag.Description;
                                } break;
                            case "AF Point Selected":
                                {
                                    AFPointSelectedInfoTextBox.Text = tag.Description;
                                } break;
                            case "Lens Type":
                                {
                                    LensTypeInfoTextBox.Text = tag.Description;
                                } break;
                            case "Long Focal Length":
                                {
                                    FocalMaxLengthInfoTextBox.Text = tag.Description;
                                } break;
                            case "Short Focal Length":
                                {
                                    FocalMinLengthInfoTextBox.Text = tag.Description;
                                } break;
                            case "Focal Units per mm":
                                {
                                    FocalUnitInfoTextBox.Text = tag.Description;
                                } break;
                            case "Max Aperture":
                                {
                                    MaxApertureTextBox.Text = tag.Description;
                                } break;
                            case "Min Aperture":
                                {
                                    MinApertureTextBox.Text = tag.Description;
                                } break;
                            case "Aperture Value":
                                {
                                    ApertureTextBox.Text = tag.Description;
                                } break;
                            case "Flash Activity":
                                {
                                    FlashActivityInfoTextBox.Text = tag.Description;
                                } break;
                            case "Flash Details":
                                {
                                    FlashDetailsTextBox.Text = tag.Description;
                                } break;
                            case "Focus Continuous":
                                {
                                    ContinouosFocusTextBox.Text = tag.Description;
                                } break;
                            case "AE Setting":
                                {
                                    AESettingsTextBox.Text = tag.Description;
                                } break;
                            case "Display Aperture":
                                {
                                    DisplayApertureTextBox.Text = tag.Description;
                                } break;
                            case "Zoom Source Width":
                                {
                                    ZoomSourceWidthTextBox.Text = tag.Description;
                                } break;
                            case "Zoom Target Width":
                                {
                                    ZoomSourceHeightTextBox.Text = tag.Description;
                                } break;
                            case "Spot Metering Mode":
                                {
                                    SpotMeteringModeTextBox.Text = tag.Description;
                                } break;
                            case "Image Description":
                                {
                                    if (UserCommentTextBox.Text != "")
                                        UserCommentTextBox.Text = UserCommentTextBox.Text.ToString() + Environment.NewLine + tag.Description;
                                    else
                                        UserCommentTextBox.Text = tag.Description;
                                } break;
                            case "GPS Latitude Ref":
                                {
                                    if (tag.Description == "N")
                                        LatRefTextBox.Text = "North";
                                    else if (tag.Description == "S")
                                        LatRefTextBox.Text = "South";
                                    else
                                        LatRefTextBox.Text = "?";
                                } break;
                            case "GPS Latitude":
                                {
                                    LatValueTextBox.Text = tag.Description;
                                } break;
                            case "GPS Longitude Ref":
                                {
                                    if (tag.Description == "E")
                                        LonRefTextBox.Text = "East";
                                    else if (tag.Description == "W")
                                        LonRefTextBox.Text = "West";
                                    else
                                        LonRefTextBox.Text = "?";
                                } break;
                            case "GPS Longitude":
                                {
                                    LonValueTextBox.Text = tag.Description;
                                } break;
                            case "Windows XP Comment":
                                {
                                    if (UserCommentTextBox.Text != "")
                                        UserCommentTextBox.Text = UserCommentTextBox.Text.ToString() + Environment.NewLine + tag.Description;
                                    else
                                        UserCommentTextBox.Text = tag.Description;
                                } break;
                            case "Windows XP Subject":
                                {
                                    subjectTextBox.Text = tag.Description;
                                } break;
                            case "Windows XP Keywords":
                                {
                                    string exiEvents = tag.Description;
                                    int delimPos = exiEvents.IndexOf(";");
                                    if ((delimPos > 0) && (delimPos < exiEvents.Length - 1))
                                    {
                                        string currEvent = exiEvents.Substring(0, delimPos);
                                        while ((delimPos > 0) && (delimPos < exiEvents.Length - 1))
                                        {
                                            activeEventComboBox.Items.Add(currEvent);
                                            AttdEventsDateComboBox.Items.Add(currEvent);
                                            exiEvents = exiEvents.Substring(delimPos + 1, exiEvents.Length - delimPos - 1);
                                            delimPos = exiEvents.IndexOf(";");
                                            if ((delimPos > 0) && (delimPos < exiEvents.Length - 1))
                                                currEvent = exiEvents.Substring(0, delimPos);
                                            else
                                                currEvent = exiEvents;
                                        }
                                        activeEventComboBox.Items.Add(currEvent);
                                        AttdEventsDateComboBox.Items.Add(currEvent);
                                    }
                                    else
                                    {
                                        activeEventComboBox.Items.Add(exiEvents);
                                        AttdEventsDateComboBox.Items.Add(exiEvents);
                                    }
                                } break;
                            case "File Modified Date":
                                {
                                    ChangeDateTimeTextBox.Text = tag.Description;
                                } break;
                            case "JPEG Comment":
                                {
                                    if (UserCommentTextBox.Text != "")
                                        UserCommentTextBox.Text = UserCommentTextBox.Text.ToString() + Environment.NewLine + tag.Description;
                                    else
                                        UserCommentTextBox.Text = tag.Description;
                                } break;
                            case "Number of Tables":
                                {
                                    noOfComponentsTextBox.Text = tag.Description;
                                } break;
                            case "Detected File Type Name":
                                {
                                    detFileTypeName = tag.Description;
                                } break;
                            default:
                                {
                                    setInformationText($"[{directory.Name}] {tag.Name} = {tag.Description}, not displayed in GUI.", informationType.ERROR, sender, e);
                                } break;
                        }
                    }
                }
                ActiveArtistsComboBox.Items.Add("Add Item...");
                ActiveArtistsComboBox.Visible = true;
                AttdEventsDateComboBox.Items.Add("Add Item...");
                AttdEventsDateComboBox.Visible = true;
                if (AttdEventsDateComboBox.Items.Count == 3)
                {
                    AttdEventsDateComboBox.SelectedIndex = 1;
                    EventIdTextBox.Text = AttdEventsDateComboBox.SelectedItem.ToString();
                }
                else
                {
                    AttdEventsDateComboBox.SelectedIndex = 0;
                    EventIdTextBox.Text = "";
                }
                activeEventComboBox.Items.Add("Add Item...");
                activeEventComboBox.Visible = true;
                if (activeEventComboBox.Items.Count == 3)
                {
                    activeEventComboBox.SelectedIndex = 1;
                    eventClass = new PEEventClass();
                    for (int i = 0; i < linwin.noOfEventCategories; i++)
                        eventClass.addEventCategory(linwin.getEvtCatTag(i), linwin.getEvtCatDescr(i), linwin.getEvtCatLevel(i));
                    for (int i = 0; i < linwin.noOfContentCategories; i++)
                        eventClass.addContentCategory(linwin.getContCatTag(i), linwin.getContCatDescr(i), linwin.getContCatLevel(i));
                    for (int i = 0; i < linwin.getNoOfRoles(); i++)
                        eventClass.addRoleCategory(linwin.getRoleTag(i), linwin.getRoleDescr(i), linwin.getRoleLevel(i));

                    string selitem = activeEventComboBox.SelectedItem.ToString();
                    string storpa = linwin.getEventStoragePath();
                    if (((storpa != null) && (selitem != null)) && (System.IO.Directory.Exists(storpa)) && (System.IO.File.Exists(storpa + "\\EventData_" + selitem + ".edf")))
                        eventClass.loadEvent(selitem, storpa);
                    else if ((System.IO.Directory.Exists("C:\\Users\\" + currUser + "\\" + sProgPath + "\\EventData")) &&
                             (System.IO.File.Exists("C:\\Users\\" + currUser + "\\" + sProgPath + "\\EventData\\EventData_" + selitem + ".edf")))
                        eventClass.loadEvent((string)activeEventComboBox.SelectedItem, "C:\\Users\\" + currUser + "\\" + sProgPath + "\\EventData");
                    else
                        setInformationText("Event directory or file does not exist.", informationType.ERROR, sender, e);

                    int eventToShow = 0;
                    if (numberOfLoadedEvents < maxNoOfLoadedEvents)
                    {
                        arrayOfEvents[numberOfLoadedEvents] = eventClass;
                        eventToShow = numberOfLoadedEvents;
                    }
                    else
                    {
                        arrayOfEvents[0].saveEvent(activeEventComboBox.SelectedItem.ToString(), storpa); // TODO - fishy
                        for (int i = 0; i < numberOfLoadedEvents; i++)
                            arrayOfEvents[i] = arrayOfEvents[i + 1];
                        arrayOfEvents[numberOfLoadedEvents] = eventClass;
                    }
                    eventToShow = numberOfLoadedEvents;
                    CopyrightTextBox.Text = arrayOfEvents[eventToShow].getEventOwner();
                    EventSecrecyLevelTextBox.Text = arrayOfEvents[eventToShow].getEventLevel();
                    EventStartTextBox.Text = arrayOfEvents[eventToShow].getEventStarted();
                    EventEndTextBox.Text = arrayOfEvents[eventToShow].getEventEnded();
                    EventHeadlineTextBox.Text = arrayOfEvents[eventToShow].getEventHeadline();
                    EventLatitudeTextBox.Text = arrayOfEvents[eventToShow].getEventLatPos();
                    EventLongitudeTextBox.Text = arrayOfEvents[eventToShow].getEventLonPos();
                    EventGeographNameTextBox.Text = arrayOfEvents[eventToShow].getEventGeoPosName();
                    EventStreetnameNumberTextBox.Text = arrayOfEvents[eventToShow].getEventStreetname();
                    EventZipCodeTextBox.Text = arrayOfEvents[eventToShow].getEventAreacode();
                    EventAreanameTextBox.Text = arrayOfEvents[eventToShow].getEventAreaname();
                    EventCitynameTextBox.Text = arrayOfEvents[eventToShow].getEventCityname();
                    EventCountrynameTextBox.Text = arrayOfEvents[eventToShow].getEventCountryname();
                    EventAttenderIDComboBox1.Items.Clear();
                    EventAttenderIDComboBox1.Items.Add("Select...");
                    for (int i = 0; i < arrayOfEvents[eventToShow].getNoOfEventAttender(); i++)
                    {
                        if (arrayOfEvents[eventToShow].getEventAttenderRoleLevel(i) <= linwin.getUserRightsValue())
                            EventAttenderIDComboBox1.Items.Add(arrayOfEvents[eventToShow].getEventAttenderID(i));
                    }
                    EventAttenderIDComboBox1.Items.Add("Add Item...");
                    EventAttenderIDComboBox1.Visible = true;
                    EventAttenderIDComboBox1.Enabled = true;
                    EventAttenderNaneTextBox.Text = "";
                    EventAttenderIDComboBox1.SelectedIndex = 0;
                    EventImageNameComboBox.Items.Clear();
                    EventImageNameComboBox.Items.Add("Select...");
                    for (int i = 0; i < arrayOfEvents[eventToShow].getNoOfEventImages(); i++)
                    {
                        if (arrayOfEvents[eventToShow].getEventImageContentLevel(i) <= linwin.getUserRightsValue())
                        {
                            string tempName = arrayOfEvents[eventToShow].getEventImageName(i);
                            int dpin = tempName.LastIndexOf("\\");
                            if ((dpin > 0) && (dpin < tempName.Length - 1))
                                EventImageNameComboBox.Items.Add(tempName.Substring(dpin + 1, tempName.Length - dpin - 1));
                            else
                                EventImageNameComboBox.Items.Add(tempName);
                        }
                    }
                    EventImageNameComboBox.Items.Add("Add Item...");
                    EventImageNameComboBox.Visible = true;
                    EventImageNameComboBox.Enabled = true;
                    if ((EventImageNameComboBox.Items.Count == 3) && (arrayOfEvents[eventToShow].getEventImageContentLevel(0) <= linwin.getUserRightsValue()))
                    {
                        EventImageLevelTextBox.Text = arrayOfEvents[eventToShow].getEventImageContentDescription(0);
                        EventImageNameComboBox.SelectedIndex = 1;
                    }
                    else
                    {
                        EventImageLevelTextBox.Text = "";
                        EventImageNameComboBox.SelectedIndex = 0;
                    }
                }
                else
                    activeEventComboBox.SelectedIndex = 0;
                if (directories.Count() > 0)
                    directories = null;
            }
            else
            {
                setInformationText("User validation needed", informationType.INFO, sender, e);
            }
        }
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CloseDown(sender, e);
        }
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (linwin.validated)
            {
                try
                {
                    ImageCodecInfo myImageCodecInfo;
                    System.Drawing.Imaging.Encoder myEncoder;
                    EncoderParameter myEncoderParameter;
                    EncoderParameters myEncoderParameters;

                    if ((detFileTypeName == "jpg") || (detFileTypeName == "JPG") || (detFileTypeName == "jpeg") || (detFileTypeName == "JPEG"))
                        myImageCodecInfo = GetEncoderInfo("image/jpeg");
                    else if ((detFileTypeName == "bmp") || (detFileTypeName == "BMP"))
                        myImageCodecInfo = GetEncoderInfo("image/bmp");
                    else if ((detFileTypeName == "gif") || (detFileTypeName == "GIF"))
                        myImageCodecInfo = GetEncoderInfo("image/gif");
                    else if ((detFileTypeName == "tiff") || (detFileTypeName == "TIFF"))
                        myImageCodecInfo = GetEncoderInfo("image/tiff");
                    else if ((detFileTypeName == "png") || (detFileTypeName == "PNG"))
                        myImageCodecInfo = GetEncoderInfo("image/png");
                    else
                        myImageCodecInfo = GetEncoderInfo("image/jpeg");

                    myEncoder = System.Drawing.Imaging.Encoder.Quality;

                    myEncoderParameters = new EncoderParameters(1);

                    myEncoderParameter = new EncoderParameter(myEncoder, 100L); // 25L);
                    myEncoderParameters.Param[0] = myEncoderParameter;

                    picture.Save(loadedImageFileName);

                    setInformationText("Saved file " + loadedImageFileName, informationType.INFO, sender, e);
                    picture = Image.FromFile(loadedImageFileName);
                    pictureCanvas.Image = picture;

                    changesToSave = false;
                    saveToolStripMenuItem.Enabled = false;
                }
                catch (Exception err)
                {
                    setInformationText("Saving file " + loadedImageFileName + " failed! " + Environment.NewLine + err.ToString(), informationType.ERROR, sender, e);
                }
            }
            else
            {
                setInformationText("User validation needed", informationType.INFO, sender, e);
            }
        }
        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (linwin.validated)
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "txt files (*.txt)|*.txt|" +
                             "doc files (*.doc)|*.doc|" +
                             "xml files (*.xml)|*.xml|" +
                             "jpg files (*.jpg)|*.jpg|" +
                             "gif files (*.gif)|*.gif|" +
                             "bmp files (*.bmp)|*.bmp|" +
                             "mpg files (*.mpg)|*.mpg|" +
                             "mp3 files (*.mp3)|*.mp3|" +
                             "all files (*.*)|*.*";
                sfd.FilterIndex = 9;
                sfd.RestoreDirectory = true;

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    if (sfd.FileName.ToString() != "")
                    {
                        string originalFileName = loadedImageFileName;
                        int dpe = sfd.FileName.ToString().LastIndexOf(".");
                        string fleExt = sfd.FileName.ToString().Substring(dpe + 1, sfd.FileName.ToString().Length - dpe - 1);
                        // --- Clear the picture and loadedImageFile ---
                        // --- Fetch original file to bitmap ---
                        Image theNewFile = pictureCanvas.Image;//Image.FromFile(loadedImageFileName);
                        bool managedToSave = false;
                        switch (fleExt.ToLower())
                        {
                            case "bmp":
                                {
                                    theNewFile.Save(sfd.FileName.ToString(), ImageFormat.Bmp);
                                    managedToSave = true;
                                } break;
                            case "emf":
                                {
                                    theNewFile.Save(sfd.FileName.ToString(), ImageFormat.Emf);
                                    managedToSave = true;
                                } break;
                            case "exif":
                                {
                                    theNewFile.Save(sfd.FileName.ToString(), ImageFormat.Exif);
                                    managedToSave = true;
                                } break;
                            case "gif":
                                {
                                    theNewFile.Save(sfd.FileName.ToString(), ImageFormat.Gif);
                                    managedToSave = true;
                                } break;
                            case "icon":
                                { 
                                    theNewFile.Save(sfd.FileName.ToString(), ImageFormat.Icon);
                                    managedToSave = true;
                                } break;
                            case "jpeg":
                            case "jpg":
                                { 
                                    theNewFile.Save(sfd.FileName.ToString(), ImageFormat.Jpeg);
                                    managedToSave = true;
                                } break;
                            case "png":
                                { 
                                    theNewFile.Save(sfd.FileName.ToString(), ImageFormat.Png);
                                    managedToSave = true;
                                } break;
                            case "tiff":
                                { 
                                    theNewFile.Save(sfd.FileName.ToString(), ImageFormat.Tiff);
                                    managedToSave = true;
                                } break;
                            case "wmf":
                                { 
                                    theNewFile.Save(sfd.FileName.ToString(), ImageFormat.Wmf);
                                    managedToSave = true;
                                } break;
                            default:
                                setInformationText("Requested image format," + fleExt.ToLower() + ", not handled!", informationType.ERROR, sender, e);
                                break;
                        }
                        //theNewFile.Dispose();
                        // --- Load the new file ---
                        if (managedToSave)
                        {
                            //pictureCanvas.Image.Dispose();
                            //picture.Dispose();
                            loadedImageFileName = sfd.FileName.ToString();
                            //picture = theNewFile;//Image.FromFile(loadedImageFileName);
                            //pictureCanvas.SizeMode = PictureBoxSizeMode.Zoom;
                            //getImageMetadataValues(sfd.FileName.ToString(), sender, e);
                            //pictureCanvas.Image = picture; // TODO - parametern är inte giltig.
                            //pictureName.Text = loadedImageFileName;
                            //expandedImage = true;
                            //saveAsToolStripMenuItem.Enabled = true;
                        }
                        theNewFile.Dispose();
                    }
                    else
                        setInformationText("Failed to save file", informationType.ERROR, sender, e);
                }
                sfd.Dispose();
                this.imageViewToolStripMenuItem_Click(sender, e);
            }
            else
            {
                setInformationText("User validation needed", informationType.INFO, sender, e);
            }
        }
        private void openDirectoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (linwin.validated)
            {
                this.imageSortingToolStripMenuItem_Click(sender, e);
            }
            else
            {
                setInformationText("User validation needed", informationType.INFO, sender, e);
            }
        }
        // --- Restore directory handling ---
        private string[] findAllDirectoriesBelow(string startDir, string searchPattern)
        {
            try
            {
                string[] resultStrArr = System.IO.Directory.GetDirectories(startDir, searchPattern);
                if (resultStrArr != null)
                {
                    foreach (var exiDir in resultStrArr)
                    {
                        string[] tempArr = findAllDirectoriesBelow(exiDir, searchPattern);
                        resultStrArr = resultStrArr.Concat(tempArr).ToArray();
                    }
                }
                return resultStrArr;
            }
            catch
            {
                string[] errorResult = new string[0];
                return errorResult;
            }
        }
        private void searchDeletedFiles(int fileType, string startDir, string searchPhrase, object sender, EventArgs e)
        {
            switch (recovType)
            {
                // recovType = Images|Document|Code|Anything
                case 0:
                    {
                        // Images, need a type and/or maybe a phrase.
                        if ((searchPhrase != "") || (searchPhrase == "*"))
                        {
                            string tmpOutStr = "Recovering deleted images containing phrase \"" + searchPhrase + "\", not implemented yet";
                            setInformationText(tmpOutStr, informationType.INFO, sender, e);
                            // TODO - implement image recovering from deleted files.
                        }
                        else
                        {
                            setInformationText("Recovering deleted images, not implemented yet.", informationType.INFO, sender, e);
                            // TODO - implement image recovering from deleted files.
                        }
                    } break;
                case 1:
                    {
                        // Document, need a type and/or maybe a phrase.
                        if ((searchPhrase != "") || (searchPhrase == "*"))
                        {
                            string tmpOutStr = "Recovering deleted documents containing phrase \"" + searchPhrase + "\", not implemented yet";
                            setInformationText(tmpOutStr, informationType.INFO, sender, e);
                            // TODO - implement document recovering from deleted files.
                        }
                        else
                        {
                            setInformationText("Recovering deleted documents, not implemented yet.", informationType.INFO, sender, e);
                            // TODO - implement document recovering from deleted files.
                        }
                    } break;
                case 2:
                    {
                        // Code, need a type and/or maybe a phrase.
                        if ((searchPhrase != "") || (searchPhrase == "*"))
                        {
                            string tmpOutStr = "Recovering deleted code containing phrase \"" + searchPhrase + "\", not implemented yet";
                            setInformationText(tmpOutStr, informationType.INFO, sender, e);
                            // TODO - implement code recovering from deleted files.
                        }
                        else
                        {
                            setInformationText("Recovering deleted code, not implemented yet.", informationType.INFO, sender, e);
                            // TODO - implement code recovering from deleted files.
                        }
                    } break;
                case 3:
                    {
                        // Anything, need a type and/or a phrase.
                        if ((searchPhrase != "") || (searchPhrase == "*"))
                        {
                            string tmpOutStr = "Recovering deleted files containing phrase \"" + searchPhrase + "\", not implemented yet";
                            setInformationText(tmpOutStr, informationType.INFO, sender, e);
                            // TODO - implement file recovering from deleted files.
                        }
                        else
                        {
                            setInformationText("Recovering deleted files, not implemented yet.", informationType.INFO, sender, e);
                            // TODO - implement file recovering from deleted files.
                        }
                    } break;
                default:
                    {
                        setInformationText("Faulty branch!", informationType.ERROR, sender, e);
                    } break;
            }
        }
        private void searchShallow(int fileType, string startDir, string searchPhrase, object sender, EventArgs e)
        {
            switch (recovType)
            {
                // recovType = Images|Document|Code|Anything
                case 0:
                    {
                        // Images, need a type and/or maybe a phrase.
                    } break;
                case 1:
                    {
                        // Document, need a type and/or maybe a phrase.
                    } break;
                case 2:
                    {
                        // Code, need a type and/or maybe a phrase.
                    } break;
                case 3:
                    {
                        // Anything, need a type and/or a phrase.
                    } break;
                default:
                    {
                        setInformationText("Faulty branch!", informationType.ERROR, sender, e);
                    } break;
            }
        }
        private void searchDeep(int fileType, string startDir, string searchPhrase, object sender, EventArgs e)
        {
            switch (recovType)
            {
                // recovType = Images|Document|Code|Anything
                case 0:
                    {
                        // Images, need a type and/or maybe a phrase.
                    } break;
                case 1:
                    {
                        // Document, need a type and/or maybe a phrase.
                    } break;
                case 2:
                    {
                        // Code, need a type and/or maybe a phrase.
                    } break;
                case 3:
                    {
                        // Anything, need a type and/or a phrase.
                    } break;
                default:
                    {
                        setInformationText("Faulty branch!", informationType.ERROR, sender, e);
                    } break;
            }
        }
        private void searchExistingFiles(int fileType, string startDir, string searchPhrase, object sender, EventArgs e)
        {
            #region imageTypeListings
            List<string> jpg = new List<string> { "FF", "D8" };
            List<string> bmp = new List<string> { "42", "4D" };
            List<string> gif = new List<string> { "47", "49", "46" };
            List<string> png = new List<string> { "89", "50", "4E", "47", "0D", "0A", "1A", "0A" };
            List<string> ico = new List<string> { "00", "00", "01", "00" };
            List<string> tiff = new List<string> { "49", "49", "2A", "00" };
            List<string> wavi = new List<string> { "52", "49", "46", "46" };
            List<string> wmva = new List<string> { "30", "26", "B2", "75", "8E", "66", "D9", "00", "AA", "00", "62", "CE", "6C" };
            List<string> webp811 = new List<string> { "77", "65", "62", "70" };
            List<List<string>> imgTypes = new List<List<string>> { jpg, bmp, gif, png, ico, tiff, wavi, wmva, webp811 };
            string[] imgItems = { "jpg", "bmp", "gif", "png", "tiff", "mp4", "mpg", "gif", "webp" };
            string[] movieItems = { "avi", "mp4", "wmov", "gif", "webp" };
            #endregion
            #region docTypeListings
            List<string> doc = new List<string> { "D0", "CF", "11", "E0", "A1", "B1", "1A", "FF" };
            List<string> pdf = new List<string> { "25", "50", "44", "46", "2D", "31", "2E" };
            List<string> zipd = new List<string> { "50", "4B", "03", "04" };
            List<List<string>> docTypes = new List<List<string>> { doc, pdf, zipd };
            string[] docItems = { "doc", "docx", "pdf", "zip", "txt" };
            #endregion
            #region codeTypeListings
            string[] codeItems = { "c", "cpp", "class", "h", "hpp", "html", "java", "jar", "par", "pl", "pm", "pas" };
            #endregion
            int dpast = -1, dpproc = -1;
            var srchPhrases = new List<string>();
            string origSearchPhrase = searchPhrase;
            if ((searchPhrase.Contains("*")) || (searchPhrase.Contains("%")))
            {
                dpast = searchPhrase.IndexOf("*");
                dpproc = searchPhrase.IndexOf("%");
                if (((dpproc == 0) || (dpast == 0)) && (srchPhrases.Count == 0))
                {
                    searchPhrase = searchPhrase.Substring(1, searchPhrase.Length - 1);
                    dpast = searchPhrase.IndexOf("*");
                    dpproc = searchPhrase.IndexOf("%");
                }
                while (((dpproc > 0) && (dpproc < searchPhrase.Length)) ||
                       ((dpast > 0) && (dpast < searchPhrase.Length)))
                {
                    int stepval = -1;
                    if ((dpast >= 0) && (dpproc >= 0))
                        stepval = Math.Min(dpast, dpproc);
                    else if (dpast >= 0)
                        stepval = dpast;
                    else if (dpast >= 0)
                        stepval = dpproc;

                    if (stepval > -1)
                    {
                        srchPhrases.Add(searchPhrase.Substring(0, stepval));
                        searchPhrase = searchPhrase.Substring(stepval + 1, searchPhrase.Length - stepval - 1);
                        dpast = searchPhrase.IndexOf("*");
                        dpproc = searchPhrase.IndexOf("%");
                    }
                    else
                    {
                        dpast = -1;
                        dpproc = -1;
                    }
                }
            }
            switch (recovType)
            {
                case 0:
                    {
                        // Recovery type image
                        string[] allDirectories = findAllDirectoriesBelow(startDir, "*");
                        var resultFiles = new List<string>();
                        string[] fileArray = System.IO.Directory.GetFiles(startDir);
                        foreach (var exiFile in fileArray)
                        {
                            System.IO.FileStream tempFile = new System.IO.FileStream(exiFile, FileMode.Open, FileAccess.Read, FileShare.None);
                            int dp0 = exiFile.LastIndexOf(".");
                            string filext = "";
                            if ((dp0 > 0) && (dp0 < exiFile.Length))
                                filext = exiFile.Substring(dp0 + 1, exiFile.Length - dp0 - 1);

                            bool isAnImageFile = false;
                            if (imgItems.Contains<string>(filext.ToLower()))
                                isAnImageFile = true;
                            else
                            {
                                for (int i = 0; i < imgTypes.Count; i++)
                                {
                                    isAnImageFile = true;
                                    for (int j = 0; j < imgTypes[i].Count; j++)
                                    {
                                        string bit = tempFile.ReadByte().ToString("X2");
                                        if (imgTypes[i][j] != bit)
                                            isAnImageFile = false;
                                        if ((i == 9) && (imgTypes[i][j + 8] != bit))
                                            isAnImageFile = false;
                                    }
                                }
                            }

                            if ((isAnImageFile) && (srchPhrases.Count == 0) && (!(resultFiles.Contains(exiFile.ToString()))))
                            {
                                resultFiles.Add(exiFile.ToString());
                            }
                            else if (isAnImageFile)
                            {
                                int clearedCriterias = 0;
                                foreach (var phrse in srchPhrases)
                                {
                                    if (exiFile.Contains(phrse))
                                    {
                                        clearedCriterias++;
                                        if ((clearedCriterias == srchPhrases.Count()) && (!(resultFiles.Contains(exiFile.ToString()))))
                                            resultFiles.Add(exiFile.ToString());
                                    }
                                }
                            }
                            tempFile.Close();
                        }
                        foreach (var exiDir in allDirectories)
                        {
                            try
                            {
                                fileArray = System.IO.Directory.GetFiles(exiDir);
                                foreach (var exiFile in fileArray)
                                {
                                    System.IO.FileStream tempFile = new System.IO.FileStream(exiFile, FileMode.Open, FileAccess.Read, FileShare.None);
                                    int dp0 = exiFile.LastIndexOf(".");
                                    string filext = "";
                                    if ((dp0 > 0) && (dp0 < exiFile.Length))
                                        filext = exiFile.Substring(dp0 + 1, exiFile.Length - dp0 - 1);
                                    bool isAnImagefile = false;

                                    if (imgItems.Contains<string>(filext.ToLower()))
                                        isAnImagefile = true;
                                    else
                                    {
                                        for (int i = 0; i < imgTypes.Count; i++)
                                        {
                                            isAnImagefile = true;
                                            for (int j = 0; j < imgTypes[i].Count; j++)
                                            {
                                                string bit = tempFile.ReadByte().ToString("X2");
                                                if (imgTypes[i][j] != bit)
                                                    isAnImagefile = false;
                                            }
                                        }
                                    }

                                    if ((isAnImagefile) && (srchPhrases.Count() == 0))
                                    {
                                        resultFiles.Add(exiFile.ToString());
                                    }
                                    else if (isAnImagefile)
                                    {
                                        int clearedCriterias = 0;
                                        foreach (var phrse in srchPhrases)
                                        {
                                            if ((exiFile.Contains(phrse)) && (!(resultFiles.Contains(exiFile.ToString()))))
                                            {
                                                clearedCriterias++;
                                                if ((clearedCriterias == srchPhrases.Count()) && (!(resultFiles.Contains(exiFile.ToString()))))
                                                    resultFiles.Add(exiFile.ToString());
                                            }
                                        }
                                    }
                                    tempFile.Close();
                                }
                            }
                            catch (Exception err)
                            {
                                setInformationText("Method searchExistingFiles failed: " + err.ToString(), informationType.ERROR, sender, e);
                            }
                        }
                        imageList.Dispose();
                        arrayItemIndex.Initialize();
                        int nr = 0;
                        if (resultFiles.Count > 0)
                        {
                            foreach (var resFile in resultFiles)
                            {
                                try
                                {
                                    ListViewItem item = new ListViewItem();
                                    item.Name = resFile;
                                    item.Text = resFile;
                                    item.ImageIndex = nr;
                                    listView.Items.Add(item);
                                    imageList.Images.Add(Image.FromFile(resFile));
                                    imageList.Images.SetKeyName(nr, resFile);
                                    arrayItemIndex[nr] = resFile;
                                }
                                catch (Exception err)
                                {
                                    setInformationText("Displaying resulting images failed: " + err.ToString(), informationType.ERROR, sender, e);
                                }
                                nr++;
                            }
                            listView.View = View.LargeIcon;
                            imageList.ImageSize = new Size(Math.Max(linwin.getSmallImageWidth(), 32), Math.Max(linwin.getSmallImageHeight(), 32));
                            listView.LargeImageList = imageList;
                            listView.Visible = true;
                        }
                        else
                            setInformationText("No images was found.", informationType.INFO, sender, e);
                    } break;
                case 1:
                    {
                        // Document, need a type and/or maybe a phrase.
                        string[] allDirectories = findAllDirectoriesBelow(startDir, "*");
                        var resultFiles = new List<string>();
                        foreach (var exiDir in allDirectories)
                        {
                            try
                            {
                                string[] fileArray = System.IO.Directory.GetFiles(exiDir);
                                foreach (var exiFile in fileArray)
                                {
                                    System.IO.FileStream tempFile = new System.IO.FileStream(exiFile, FileMode.Open, FileAccess.Read, FileShare.None);
                                    int dp0 = exiFile.LastIndexOf(".");
                                    string filext = "";
                                    if ((dp0 > 0) && (dp0 < exiFile.Length))
                                        filext = exiFile.Substring(dp0 + 1, exiFile.Length - dp0 - 1);
                                    bool isAnDocument = false;

                                    if (docItems.Contains<string>(filext.ToLower()))
                                        isAnDocument = true;
                                    else
                                    {
                                        for (int i = 0; i < docTypes.Count; i++)
                                        {
                                            isAnDocument = true;
                                            for (int j = 0; j < docTypes[i].Count; j++)
                                            {
                                                string bit = tempFile.ReadByte().ToString("X2");
                                                if (docTypes[i][j] != bit)
                                                    isAnDocument = false;
                                            }
                                        }
                                    }

                                    if ((isAnDocument) && (srchPhrases.Count() == 0))
                                    {
                                        resultFiles.Add(exiFile.ToString());
                                    }
                                    else if (isAnDocument)
                                    {
                                        int clearedCriterias = 0;
                                        foreach (var phrse in srchPhrases)
                                        {
                                            if (exiFile.Contains(phrse))
                                            {
                                                clearedCriterias++;
                                                if ((clearedCriterias == srchPhrases.Count())&& (!(resultFiles.Contains(exiFile.ToString()))))
                                                    resultFiles.Add(exiFile.ToString());
                                            }
                                        }
                                    }
                                    tempFile.Close();
                                }
                            }
                            catch (Exception err)
                            {
                                setInformationText("Displaying resulting documents failed: " + err.ToString(), informationType.ERROR, sender, e);
                            }
                        }
                        listView.ResetText();
                        listView.Clear();
                        //int nr = 0;
                        if (resultFiles.Count > 0)
                        {
                            listView.Alignment = ListViewAlignment.Left;
                            listView.View = View.List;
                            foreach (var resFile in resultFiles)
                            {
                                try
                                {
                                    // TODO - Kläm in hanteringen här!
                                    ListViewItem item = new ListViewItem(resFile);
                                    item.Name = resFile;
                                    listView.Items.Add(item);
                                }
                                catch (Exception err)
                                {
                                    setInformationText("Display resulting documents failed:" + err.ToString(), informationType.ERROR, sender, e);
                                }
                            }
                            listView.Visible = true;
                        }
                        else
                            setInformationText("No documents where found.", informationType.INFO, sender, e);
                    } break;
                case 2:
                    {
                        // Code, need a type and/or maybe a phrase.
                        string[] allDirectories = findAllDirectoriesBelow(startDir, "*");
                        var resultFiles = new List<string>();
                        foreach (var exiDir in allDirectories)
                        {
                            try
                            {
                                string[] fileArray = System.IO.Directory.GetFiles(exiDir);
                                foreach (var exiFile in fileArray)
                                {
                                    System.IO.FileStream tempFile = new System.IO.FileStream(exiFile, FileMode.Open, FileAccess.Read, FileShare.None);
                                    int dp0 = exiFile.LastIndexOf(".");
                                    string filext = "";
                                    if ((dp0 > 0) && (dp0 < exiFile.Length))
                                        filext = exiFile.Substring(dp0 + 1, exiFile.Length - dp0 - 1);
                                    bool isAnDocument = false;

                                    if (codeItems.Contains<string>(filext.ToLower()))
                                        isAnDocument = true;

                                    if ((isAnDocument) && (srchPhrases.Count() == 0))
                                    {
                                        resultFiles.Add(exiFile.ToString());
                                    }
                                    else if (isAnDocument)
                                    {
                                        int clearedCriterias = 0;
                                        foreach (var phrse in srchPhrases)
                                        {
                                            if ((exiFile.Contains(phrse)) && (!(resultFiles.Contains(exiFile.ToString()))))
                                            {
                                                clearedCriterias++;
                                                if (clearedCriterias == srchPhrases.Count())
                                                    resultFiles.Add(exiFile.ToString());
                                            }
                                        }
                                    }
                                    tempFile.Close();
                                }
                            }
                            catch (Exception err)
                            {
                                setInformationText("Displaying resulting documents failed: " + err.ToString(), informationType.ERROR, sender, e);
                            }
                        }
                        listView.ResetText();
                        listView.Clear();
                        listView.Alignment = ListViewAlignment.Left;
                        listView.View = View.List;
                        //int nr = 0;
                        if (resultFiles.Count > 0)
                        {
                            foreach (var resFile in resultFiles)
                            {
                                try
                                {
                                    // TODO - Kläm in hanteringen här!
                                    ListViewItem item = new ListViewItem(resFile);
                                    item.Name = resFile;
                                    listView.Items.Add(item);
                                }
                                catch (Exception err)
                                {
                                    setInformationText("Display resulting documents failed:" + err.ToString(), informationType.ERROR, sender, e);
                                }
                            }
                            listView.Visible = true;
                        }
                        else
                            setInformationText("No documents where found.", informationType.INFO, sender, e);
                    } break;
                case 3:
                    {
                        // Anything, need a type and/or a phrase.
                        string[] allDirectories = findAllDirectoriesBelow(startDir, "*");
                        // string cmd = "dir " + "/s" + "/q" + "/-c" + "/t" + startDir;
                        // ProcessStartInfo info = new ProcessStartInfo(cmd);
                        var resultFiles = new List<string>();
                        var outFileType = new List<string>();
                        var outFileHash = new List<int>();
                        var outFileSize = new List<long>();
                        string[] fileArray = System.IO.Directory.GetFiles(startDir);
                        foreach (var exiFile in fileArray)
                        {
                            System.IO.FileStream tempFile = new System.IO.FileStream(exiFile, FileMode.Open, FileAccess.Read, FileShare.None);

                            outFileHash.Add(tempFile.GetHashCode());
                            outFileSize.Add(tempFile.Length);

                            int dp0 = exiFile.LastIndexOf(".");
                            string filext = "";
                            if ((dp0 > 0) && (dp0 < exiFile.Length))
                                filext = exiFile.Substring(dp0 + 1, exiFile.Length - dp0 - 1);

                            if (movieItems.Contains<string>(filext.ToLower()))
                                outFileType.Add("Movie file");
                            else if (imgItems.Contains<string>(filext.ToLower()))
                                outFileType.Add("Image file");
                            else if (docItems.Contains<string>(filext.ToLower()))
                                outFileType.Add("Document file");
                            else if (codeItems.Contains<string>(filext.ToLower()))
                                outFileType.Add("Source code file");
                            else
                                outFileType.Add("Unknown filetype");

                            if (srchPhrases.Count > 0)
                            {
                                int clearedCriterias = 0;
                                foreach (var phrse in srchPhrases)
                                {
                                    if (exiFile.Contains(phrse))
                                    {
                                        clearedCriterias++;
                                        if ((clearedCriterias == srchPhrases.Count()) && (!(resultFiles.Contains(exiFile.ToString()))))
                                            resultFiles.Add(exiFile.ToString());
                                    }
                                }
                            }
                            else
                                resultFiles.Add(exiFile.ToString());
                            tempFile.Close();
                        }
                        foreach (var exiDir in allDirectories)
                        {
                            try
                            {
                                fileArray = System.IO.Directory.GetFiles(exiDir);
                                foreach (var exiFile in fileArray)
                                {
                                    System.IO.FileStream tempFile = new System.IO.FileStream(exiFile, FileMode.Open, FileAccess.Read, FileShare.None);

                                    outFileHash.Add(tempFile.GetHashCode());
                                    outFileSize.Add(tempFile.Length);

                                    int dp0 = exiFile.LastIndexOf(".");
                                    string filext = "";
                                    if ((dp0 > 0) && (dp0 < exiFile.Length))
                                        filext = exiFile.Substring(dp0 + 1, exiFile.Length - dp0 - 1);

                                    if (movieItems.Contains<string>(filext.ToLower()))
                                        outFileType.Add("Movie file");
                                    else if (imgItems.Contains<string>(filext.ToLower()))
                                        outFileType.Add("Image file");
                                    else if (docItems.Contains<string>(filext.ToLower()))
                                        outFileType.Add("Document file");
                                    else if (codeItems.Contains<string>(filext.ToLower()))
                                        outFileType.Add("Source code file");
                                    else
                                        outFileType.Add("Unknown filetype");

                                    if (srchPhrases.Count > 0)
                                    {
                                        int clearedCriterias = 0;
                                        foreach (var phrse in srchPhrases)
                                        {
                                            if (exiFile.Contains(phrse))
                                            {
                                                clearedCriterias++;
                                                if ((clearedCriterias == srchPhrases.Count()) && (!(resultFiles.Contains(exiFile.ToString()))))
                                                    resultFiles.Add(exiFile.ToString());
                                            }
                                        }
                                    }
                                    else
                                        resultFiles.Add(exiFile.ToString());
                                    tempFile.Close();
                                }
                            }
                            catch (Exception err)
                            {
                                setInformationText("Finding the file-sort and info failed: " + err.ToString(), informationType.ERROR, sender, e);
                            }
                        }
                        listView.ResetText();
                        listView.Clear();
                        listView.Alignment = ListViewAlignment.Top;//Left;
                        listView.View = View.List;
                        if (resultFiles.Count > 0)
                        {
                            int rfn = 0;
                            string currDateTime = DateTime.Now.ToString("yyyy-MM-dd-hhmmss");
                            sFSPath = "C:\\Users\\" + currUser + "\\source\\repos\\PhotoEditor00002\\PhotoEditor00002\\Logs\\FileSearch" + currDateTime + ".log";
                            using (StreamWriter rsw = File.AppendText(sFSPath))
                            {
                                rsw.WriteLine("--------- All files search result ---------");
                                rsw.WriteLine("--- Any file searched from " + startDir);
                                rsw.WriteLine("--- Search made at " + currDateTime);
                                rsw.WriteLine("--- Search phrase \"" + origSearchPhrase + "\"");
                                rsw.WriteLine("-------------------------------------------");
                                rsw.WriteLine("Filepath and name" + "\t" + "File type" + "\t" + "File size" + "\t" + "Hash code");
                                foreach (var resFile in resultFiles)
                                {
                                    try
                                    {
                                        ListViewItem item = new ListViewItem(resFile);
                                        item.Name = resFile;
                                        item.Text = resFile;
                                        rsw.WriteLine(resFile + "\t" + outFileType[rfn] + "\t" + outFileSize[rfn] + "\t" + outFileHash[rfn++].ToString());
                                        listView.Items.Add(item);
                                    }
                                    catch (Exception err)
                                    {
                                        setInformationText("Displaying resulting files failed: " + err.ToString(), informationType.ERROR, sender, e);
                                    }
                                }
                                rsw.WriteLine("---" + "\t" + "---" + "\t" + "---" + "\t" + "---");
                                rsw.Close();
                            }
                            listView.Visible = true;
                        }
                        else
                            setInformationText("No files matched the search criteria.", informationType.INFO, sender, e);
                    } break;
                default:
                    {
                        setInformationText("Faulty branch!", informationType.ERROR, sender, e);
                    } break;
            }
        }
        private void restoreToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (linwin.validated)
            {
                //if (this.currentMode == programMode.SORTINGVIEW)
                if (imageList.Images.Count > 0)
                {
                    imageList.Dispose();
                }
                if (listView.Items.Count > 0)
                {
                    listView.Dispose();
                }
                this.imageRestoringToolStripMenuItem_Click(sender, e);
            }
            else
            {
                setInformationText("User validation needed", informationType.INFO, sender, e);
            }
        }
        private void imageRestoringToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (linwin.validated)
            {
                if (listView.Items.Count > 0)
                {
                    listView.Items.Clear();
                    listView.Dispose();
                }
                if (imageList.Images.Count > 0)
                {
                    imageList.Images.Clear();
                    imageList.Tag = "";
                    imageList.Dispose();
                }
                if (picture != null)
                {
                    picture.Dispose();
                }
                if (pictureCanvas.Image != null)
                {
                    pictureCanvas.Image.Dispose();
                }
                this.currentMode = programMode.RESTOREVIEW;
                setInformationText("Entered image restoring mode.", informationType.INFO, sender, e);
                this.pictureCanvas.Visible = false;
                this.listView.Visible = false;
                this.tabControl.Visible = true;
                this.tabControl.SelectedTab = this.tabControl.TabPages["recoverTab"];
            }
            else
            {
                setInformationText("User validation needed", informationType.INFO, sender, e);
            }
        }
        private void recoverSelCmbBx_SelectedIndexChanged(object sender, EventArgs e)
        {
            recovLevel = recoverSelCmbBx.SelectedIndex;
            switch (recovLevel)
            {
                case 0:
                    {
                        setInformationText("Recovery level \"Deleted Files\"", informationType.INFO, sender, e);
                        this.pictureCanvas.Visible = false;
                        this.listView.Visible = false;
                        this.searchPhraseTxtBx.Enabled = false;
                        if (recovType > -1)
                            startRecoveryBtn.Enabled = true;
                        else
                            startRecoveryBtn.Enabled = false;
                    } break;
                case 1:
                    {
                        setInformationText("Recovery level \"Shallow detection\"", informationType.INFO, sender, e);
                        this.pictureCanvas.Visible = false;
                        this.listView.Visible = false;
                        this.searchPhraseTxtBx.Enabled = false;
                        if (recovType > -1)
                            startRecoveryBtn.Enabled = true;
                        else
                            startRecoveryBtn.Enabled = false;
                    } break;
                case 2:
                    {
                        setInformationText("Recovery level \"Deep detection\"", informationType.INFO, sender, e);
                        this.pictureCanvas.Visible = false;
                        this.listView.Visible = false;
                        this.searchPhraseTxtBx.Enabled = false;
                        if (recovType > -1)
                            startRecoveryBtn.Enabled = true;
                        else
                            startRecoveryBtn.Enabled = false;
                    } break;
                case 3:
                    {
                        setInformationText("Recovery level \"File search\"", informationType.INFO, sender, e);
                        this.pictureCanvas.Visible = false;
                        this.listView.Visible = false;
                        this.searchPhraseTxtBx.Enabled = true;
                        if ((recovType > -1) && (searchPhraseTxtBx.Text != ""))
                            startRecoveryBtn.Enabled = true;
                        else
                            startRecoveryBtn.Enabled = false;
                    }
                    break;
                default:
                    {
                        setInformationText("Recovery level unknown!", informationType.INFO, sender, e);
                        this.pictureCanvas.Visible = false;
                        this.listView.Visible = false;
                        startRecoveryBtn.Enabled = false;
                    } break;
            }
        }
        private void recoverTypecmbBx_SelectedIndexChanged(object sender, EventArgs e)
        {
            recovType = recoverTypecmbBx.SelectedIndex;
            switch (recovType)
            {
                case 0:
                    {
                        setInformationText("Recovery type \"Images\"", informationType.INFO, sender, e);
                        this.pictureCanvas.Visible = false;
                        this.listView.Visible = false;
                        if (recovLevel > -1)
                            startRecoveryBtn.Enabled = true;
                        else
                            startRecoveryBtn.Enabled = false;
                    } break;
                case 1:
                    {
                        setInformationText("Recovery type \"Document\"", informationType.INFO, sender, e);
                        this.pictureCanvas.Visible = false;
                        this.listView.Visible = false;
                        if (recovLevel > -1)
                            startRecoveryBtn.Enabled = true;
                        else
                            startRecoveryBtn.Enabled = false;
                    } break;
                case 2:
                    {
                        setInformationText("Recovery type \"Code\"", informationType.INFO, sender, e);
                        this.pictureCanvas.Visible = false;
                        this.listView.Visible = false;
                        if (recovLevel > -1)
                            startRecoveryBtn.Enabled = true;
                        else
                            startRecoveryBtn.Enabled = false;
                    } break;
                case 3:
                    {
                        setInformationText("Recovery type \"Anything\"", informationType.INFO, sender, e);
                        this.pictureCanvas.Visible = false;
                        this.listView.Visible = false;
                        if (recovLevel > -1)
                            startRecoveryBtn.Enabled = true;
                        else
                            startRecoveryBtn.Enabled = false;
                    } break;
                default:
                    {
                        setInformationText("Recovery type unknown!", informationType.INFO, sender, e);
                        startRecoveryBtn.Enabled = false;
                    } break;
            }
        }
        private void searchPhraseTxtBx_TextChanged(object sender, EventArgs e)
        {
            string srcPhrse = searchPhraseTxtBx.Text;
            string outstring = "Search phrase set to: \"" + srcPhrse + "\"";
            setInformationText(outstring, informationType.INFO, sender, e);
        }
        private void startRecoveryBtn_Click(object sender, EventArgs e)
        {
            if (recovLevel > -1)
            {
                if (recovType > -1)
                {
                    // TODO - Research how to recover damaged or deleted files.
                    this.UseWaitCursor = true;
                    // Get a phrase if there is one.
                    string strSrcPhrse = searchPhraseTxtBx.Text;
                    // Find starting position by asking user for start-directory.
                    searchFolderBrowserDialog.Reset();
                    searchFolderBrowserDialog.RootFolder = Environment.SpecialFolder.MyComputer;
                    DialogResult dres = searchFolderBrowserDialog.ShowDialog();
                    string bfn = searchFolderBrowserDialog.SelectedPath.ToString();
                    if (bfn != "")
                    {
                        pictureName.Text = bfn;
                        switch (recovLevel)
                        {
                            // recovLevel = Deleted files|Shallow detection|Deep detection|File search
                            case 0:
                                {
                                    // Deleted files, need a type and/or maybe a phrase.
                                    searchDeletedFiles(recovType, bfn, strSrcPhrse, sender, e);
                                } break;
                            case 1:
                                {
                                    // Shallow detection, need a type and/or maybe a phrase.
                                    searchShallow(recovType, bfn, strSrcPhrse, sender, e);
                                } break;
                            case 2:
                                {
                                    // Deep detection, need a type and/or maybe a phrase.
                                    searchDeep(recovType, bfn, strSrcPhrse, sender, e);
                                } break;
                            case 3:
                                {
                                    // File search, need a type and/or maybe a phrase.
                                    searchExistingFiles(recovType, bfn, strSrcPhrse, sender, e);
                                } break;
                            default:
                                {
                                    setInformationText("Faulty branch!", informationType.ERROR, sender, e);
                                } break;
                        }
                    }
                    this.UseWaitCursor = false;
                }
                else
                    setInformationText("You must provide a recovery type.", informationType.INFO, sender, e);
            }
            else
                setInformationText("You must provide a recovery level.", informationType.INFO, sender, e);
        }
        // --- Image View handling ---
        private void imageViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (linwin.validated)
            {
                this.currentMode = programMode.IMAGEVIEW;
                setInformationText("Entered image view mode.", informationType.INFO, sender, e);
                this.pictureCanvas.Visible = true;
                this.listView.Visible = false;
                this.tabControl.Visible = true;
            }
            else
            {
                setInformationText("User validation needed", informationType.INFO, sender, e);
            }
        }
        private void xdirectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (linwin.validated)
            {
                picture.RotateFlip(RotateFlipType.RotateNoneFlipX);
                pictureCanvas.Image = picture;
                saveToolStripMenuItem.Enabled = true;
            }
            else
            {
                setInformationText("User validation needed", informationType.INFO, sender, e);
            }
        }
        private void ydirectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (linwin.validated)
            {
                picture.RotateFlip(RotateFlipType.RotateNoneFlipY);
                pictureCanvas.Image = picture;
                saveToolStripMenuItem.Enabled = true;
            }
            else
            {
                setInformationText("User validation needed", informationType.INFO, sender, e);
            }
        }
        private void degRightToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (linwin.validated)
            {
                picture.RotateFlip(RotateFlipType.Rotate90FlipNone);
                pictureCanvas.Image = picture;
                saveToolStripMenuItem.Enabled = true;
            }
            else
            {
                setInformationText("User validation needed", informationType.INFO, sender, e);
            }
        }
        private void degLeftToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (linwin.validated)
            {
                picture.RotateFlip(RotateFlipType.Rotate270FlipNone);
                pictureCanvas.Image = picture;
                saveToolStripMenuItem.Enabled = true;
            }
            else
            {
                setInformationText("User validation needed", informationType.INFO, sender, e);
            }
        }
        private void degToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (linwin.validated)
            {
                picture.RotateFlip(RotateFlipType.Rotate180FlipNone);
                pictureCanvas.Image = picture;
                saveToolStripMenuItem.Enabled = true;
            }
            else
            {
                setInformationText("User validation needed", informationType.INFO, sender, e);
            }
        }
        private void graphToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (linwin.validated)
            {
                myHistogramForm histForm = new myHistogramForm(loadedImageFileName);
                histForm.ShowDialog(this);
                if (histForm.validImage)
                {
                    startval = histForm.setMinLimit;
                    stopval = histForm.setMaxLimit;
                    filterToolStripMenuItem_Click(sender, e);
                }
                histForm.Dispose();
            }
        }
        private void filterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UseWaitCursor = true;
            if ((pictureCanvas.Image != null) && (loadedImageFileName != null))
            {
                Bitmap bmp = new Bitmap(pictureCanvas.Image);
                Color pixel;
                int limitval = 0;
                int[] grayVal = new int[256];
                if (bmp.Height > bmp.Width)
                {
                    for (int i = 0; i <= bmp.Width - 1; i++)
                    {
                        for (int j = 0; j < bmp.Height - 1; j++)
                        {
                            pixel = bmp.GetPixel(i, j);
                            // Grayscale are calculated with the formula: 0.2989 * R(x,y) + 0.5870 * G(x,y) + 0.1140 * B(x,y)
                            grayVal[(int)((pixel.R * 0.2989) + (pixel.G * 0.5870) + (pixel.B * 0.1140))]++;
                        }
                    }
                }
                else
                {
                    for (int i = 0; i <= bmp.Height - 1; i++)
                    {
                        for (int j = 0; j < bmp.Width - 1; j++)
                        {
                            pixel = bmp.GetPixel(j, i);
                            // Grayscale are calculated with the formula: 0.2989 * R(x,y) + 0.5870 * G(x,y) + 0.1140 * B(x,y)
                            grayVal[(int)((pixel.R * 0.2989) + (pixel.G * 0.5870) + (pixel.B * 0.1140))]++;
                        }
                    }
                }
                if ((startval == 0) && (stopval == int.MaxValue))
                {
                    for (int i = 0; i < 256; i++)
                    {
                        int gve = grayVal[255 - i];
                        if ((startval == 0) && (grayVal[i] > limitval))
                            startval = i;
                        if ((stopval == int.MaxValue) && (grayVal[255 - i] > limitval))
                            stopval = 255 - i;
                    }
                }
                float amplifyer = (float)((float)255 / (stopval - startval));
                if (bmp.Height > bmp.Width)
                {
                    for (int i = 0; i <= bmp.Width - 1; i++)
                    {
                        for (int j = 0; j < bmp.Height - 1; j++)
                        {
                            pixel = bmp.GetPixel(i, j);
                            int crv = Math.Max(0, Math.Min(255, Convert.ToInt32(pixel.R * amplifyer)));
                            int cgv = Math.Max(0, Math.Min(255, Convert.ToInt32(pixel.G * amplifyer)));
                            int cbv = Math.Max(0, Math.Min(255, Convert.ToInt32(pixel.B * amplifyer)));
                            Color newPixel = Color.FromArgb(pixel.A, crv, cgv, cbv);
                            bmp.SetPixel(i, j, newPixel);
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < bmp.Height; i++)// - 1; i++)
                    {
                        for (int j = 0; j < bmp.Width; j++)// - 1; j++)
                        {
                            pixel = bmp.GetPixel(j, i);
                            int crv = Math.Max(0, Math.Min(255, Convert.ToInt32(pixel.R * amplifyer)));
                            int cgv = Math.Max(0, Math.Min(255, Convert.ToInt32(pixel.G * amplifyer)));
                            int cbv = Math.Max(0, Math.Min(255, Convert.ToInt32(pixel.B * amplifyer)));
                            Color newPixel = Color.FromArgb(pixel.A, crv, cgv, cbv);
                            bmp.SetPixel(j, i, newPixel);
                        }
                    }
                }
                pictureCanvas.SizeMode = PictureBoxSizeMode.Zoom;
                pictureCanvas.Image = bmp;
                expandedImage = true;
                saveAsToolStripMenuItem.Enabled = true;
            }
            else
                setInformationText("Loaded file cannot be \"null\"", informationType.INFO, sender, e);
            UseWaitCursor = false;
        }
        private void grayImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UseWaitCursor = true;
            if ((pictureCanvas.Image != null) && (loadedImageFileName != null))
            {
                Bitmap bmp = new Bitmap(pictureCanvas.Image);//loadedImageFileName);
                if (bmp.Height > bmp.Width)
                {
                    for (int i = 0; i <= bmp.Width - 1; i++)
                    {
                        for (int j = 0; j < bmp.Height - 1; j++)
                        {
                            Color pixel = bmp.GetPixel(i, j);
                            int grayVal = Math.Max(0, Math.Min(255, Convert.ToInt32((pixel.R * 0.2989) + (pixel.G * 0.5870) + (pixel.B * 0.1140))));
                            Color newPixel = Color.FromArgb(pixel.A, grayVal, grayVal, grayVal);
                            bmp.SetPixel(i, j, newPixel);
                        }
                    }
                }
                else
                {
                    for (int i = 0; i <= bmp.Height - 1; i++)
                    {
                        for (int j = 0; j < bmp.Height - 1; j++)
                        {
                            Color pixel = bmp.GetPixel(j, i);
                            int grayVal = Math.Max(0, Math.Min(255, Convert.ToInt32((pixel.R * 0.2989) + (pixel.G * 0.5870) + (pixel.B * 0.1140))));
                            Color newPixel = Color.FromArgb(pixel.A, grayVal, grayVal, grayVal);
                            bmp.SetPixel(j, i, newPixel);
                        }
                    }
                }
                pictureCanvas.SizeMode = PictureBoxSizeMode.Zoom;
                pictureCanvas.Image = bmp;
                expandedImage = true;
                saveAsToolStripMenuItem.Enabled = true;
                saveToolStripMenuItem.Enabled = true;
            }
            else
                setInformationText("Need to have a loaded file to work with.", informationType.INFO, sender, e);
            UseWaitCursor = false;
        }
        // --- --- General information handling --- ---
        private void SaveGenDataChangesButton_Click(object sender, EventArgs e)
        {
            bool managedToSaveSomething = false;
            if (linwin.validated)
            {
                if (subjectChanged)
                {
                    try
                    {
                        // 40095 - 0x9C9F : .......... (subjectTextBox)
                        string newSubject = subjectTextBox.Text.ToString();
                        byte[] newSubjBte = Encoding.Unicode.GetBytes(newSubject);//Encoding.ASCII.GetBytes(newSubject);
                        try
                        {
                            PropertyItem subjItm = picture.GetPropertyItem(40095);
                            subjItm.Value = newSubjBte;
                            subjItm.Len = newSubjBte.Length;
                            picture.SetPropertyItem(subjItm);
                        }
                        catch
                        {
                            var subjItm = (PropertyItem)FormatterServices.GetUninitializedObject(typeof(PropertyItem));
                            subjItm.Type = 1;
                            subjItm.Id = 40095;
                            subjItm.Value = newSubjBte;
                            subjItm.Len = subjItm.Value.Length;
                            picture.SetPropertyItem(subjItm);
                        }
                        managedToSaveSomething = true;
                        subjectChanged = false;
                    }
                    catch (Exception err)
                    {
                        setInformationText("Trying to save metadata \"Subject\" ended with : " + err.ToString(), informationType.ERROR, sender, e);
                    }
                }
                if (userCommentChanged)
                {
                    try
                    {
                        // 40092 - 0x9C9C : .......... (User Comment)
                        string newUsrCmt = UserCommentTextBox.Text.ToString();
                        byte[] newUsrCmtBte = Encoding.Unicode.GetBytes(newUsrCmt);//ASCII.GetBytes(newUsrCmt);
                        try
                        {
                            PropertyItem usrCmtItm = picture.GetPropertyItem(40092);
                            usrCmtItm.Value = newUsrCmtBte;
                            usrCmtItm.Len = newUsrCmtBte.Length;
                            picture.SetPropertyItem(usrCmtItm);
                        }
                        catch
                        {
                            var usrCmtItm = (PropertyItem)FormatterServices.GetUninitializedObject(typeof(PropertyItem));
                            usrCmtItm.Type = 1;
                            usrCmtItm.Id = 40092;
                            usrCmtItm.Value = newUsrCmtBte;
                            usrCmtItm.Len = usrCmtItm.Value.Length;
                            picture.SetPropertyItem(usrCmtItm);
                        }
                        managedToSaveSomething = true;
                        userCommentChanged = false;
                    }
                    catch (Exception err)
                    {
                        setInformationText("Trying to save metadata \"User comment\" ended with: " + Environment.NewLine + err.ToString(), informationType.ERROR, sender, e);
                    }
                }
                if (imageTitleChanged)
                {
                    try
                    {
                        // 270 - 
                        string newImgTtl = ImageTitleTextBox.Text.ToString();
                        byte[] newImgTtlBte = Encoding.Unicode.GetBytes(newImgTtl);//ASCII.GetBytes(newImgTtl);
                        try
                        {
                            PropertyItem usrImgTtl = picture.GetPropertyItem(270);
                            usrImgTtl.Value = newImgTtlBte;
                            usrImgTtl.Len = newImgTtlBte.Length;
                            picture.SetPropertyItem(usrImgTtl);
                        }
                        catch
                        {
                            var usrImgTtl = (PropertyItem)FormatterServices.GetUninitializedObject(typeof(PropertyItem));
                            usrImgTtl.Type = 1;
                            usrImgTtl.Id = 270;
                            usrImgTtl.Value = newImgTtlBte;
                            usrImgTtl.Len = usrImgTtl.Value.Length;
                            picture.SetPropertyItem(usrImgTtl);
                        }
                        managedToSaveSomething = true;
                        imageTitleChanged = false;
                    }
                    catch (Exception err)
                    {
                        setInformationText("Trying to save metadata \"image title\" ended with: " + Environment.NewLine + err.ToString(), informationType.ERROR, sender, e);
                    }
                }

                if (managedToSaveSomething)
                {
                    changesToSave = true;
                    saveToolStripMenuItem.Enabled = true;
                    SaveGenDataChangesButton.Enabled = false;
                    DiscardGenDataChangesButton.Enabled = false;
                }
            }
            else
            {
                setInformationText("User validation needed", informationType.INFO, sender, e);
            }
        }
        private void DiscardGenDataChangesButton_Click(object sender, EventArgs e)
        {
            if (linwin.validated)
            {
                if (subjectChanged)
                {
                    PropertyItem tmpPItm = picture.GetPropertyItem(40095);
                    subjectTextBox.Text = tmpPItm.Value.ToString();
                }
                if (userCommentChanged)
                {
                    PropertyItem tmpPItm = picture.GetPropertyItem(40092);
                    UserCommentTextBox.Text = tmpPItm.Value.ToString();
                }
                if (imageTitleChanged)
                {
                    PropertyItem tmpPItm = picture.GetPropertyItem(270);
                    ImageTitleTextBox.Text = tmpPItm.Value.ToString();
                }
            }
            else
            {
                setInformationText("User validation needed", informationType.INFO, sender, e);
            }
        }
        private void UserCommentTextBox_TextChanged(object sender, EventArgs e)
        {
            if (linwin.validated)
            {
                // User Comment edited
                if (startUpDone)
                {
                    SaveGenDataChangesButton.Enabled = true;
                    DiscardGenDataChangesButton.Enabled = true;
                    userCommentChanged = true;
                    setInformationText($"Comment textbox changed to : \" {UserCommentTextBox.Text}\"", informationType.INFO, sender, e);
                }
            }
            else
            {
                setInformationText("User validation needed", informationType.INFO, sender, e);
            }
        }
        private void ImageTitleTextBox_TextChanged(object sender, EventArgs e)
        {
            if (linwin.validated)
            {
                if (startUpDone)
                {
                    SaveGenDataChangesButton.Enabled = true;
                    DiscardGenDataChangesButton.Enabled = true;
                    imageTitleChanged = true;
                    setInformationText($"Image title changed to : \" {ImageTitleTextBox.Text.ToString()}\"", informationType.INFO, sender, e);
                }
            }
            else
            {
                setInformationText("User validation needed", informationType.INFO, sender, e);
            }
        }
        private void subjectTextBox_TextChanged(object sender, EventArgs e)
        {
            if (linwin.validated)
            {
                if (startUpDone)
                {
                    SaveGenDataChangesButton.Enabled = true;
                    DiscardGenDataChangesButton.Enabled = true;
                    subjectChanged = true;
                    setInformationText($"Subject text box changed to : \" {subjectTextBox.Text.ToString()}\"", informationType.INFO, sender, e);
                }
            }
            else
            {
                setInformationText("User validation needed", informationType.INFO, sender, e);
            }
        }
        private void ImageOrientationTextBox_TextChanged(object sender, EventArgs e)
        {
            // TODO - Handle this section
        }
        private void ImageXResTextBox_TextChanged(object sender, EventArgs e)
        {
            // Note! Intentionally left empty, cannot change resolution.
        }
        private void ImageYResTextBox_TextChanged(object sender, EventArgs e)
        {
            // Note! Intentionally left empty, cannot change resolution.
        }
        private void ContrastTextBox_TextChanged(object sender, EventArgs e)
        {
            // Note! Intentionally left empty, cannot change contrast.
        }
        private void SaturationTextBox_TextChanged(object sender, EventArgs e)
        {
            // Note! Intentionally left empty, cannot change saturation.
        }
        // --- --- Base Hardware information handling --- ---
        private void OwnerTextBox_TextChanged(object sender, EventArgs e)
        {
            if (linwin.validated)
            {
                if (startUpDone)
                {
                    SaveGenHWDataChangesButton.Enabled = true;
                    DiscardGenHWDataChangesButton.Enabled = true;
                    bhsOwnerChanged = true;
                    setInformationText($"Hardware owner changed to : \" {OwnerTextBox.Text}\"", informationType.INFO, sender, e);
                }
            }
            else
            {
                setInformationText("User validation needed", informationType.INFO, sender, e);
            }
        }
        private void SaveGenHWDataChangesButton_Click(object sender, EventArgs e)
        {
            bool managedToSaveSomething = false;
            if (linwin.validated)
            {
                if (bhsOwnerChanged)
                {
                    try
                    {
                        string newHWOwner = OwnerTextBox.Text;
                        byte[] newHWOwnerBte = Encoding.Unicode.GetBytes(newHWOwner);
                        try
                        {
                            PropertyItem hwOwnItm = picture.GetPropertyItem(42032);
                            hwOwnItm.Value = newHWOwnerBte;
                            hwOwnItm.Len = newHWOwnerBte.Length;
                            picture.SetPropertyItem(hwOwnItm);
                        }
                        catch
                        {
                            var hwOwnItm = (PropertyItem)FormatterServices.GetUninitializedObject(typeof(PropertyItem));
                            hwOwnItm.Type = 1;
                            hwOwnItm.Id = 42032;
                            hwOwnItm.Value = newHWOwnerBte;
                            hwOwnItm.Len = hwOwnItm.Value.Length;
                            picture.SetPropertyItem(hwOwnItm);
                        }
                        managedToSaveSomething = true;
                        bhsOwnerChanged = false;
                    }
                    catch (Exception err)
                    {
                        setInformationText("Trying to save metadata \"Hardware Owner\" ended with : " + err.ToString(), informationType.ERROR, sender, e);
                    }
                }
                if (managedToSaveSomething)
                {
                    changesToSave = true;
                    saveToolStripMenuItem.Enabled = true;
                    SaveGenHWDataChangesButton.Enabled = false;
                    DiscardGenHWDataChangesButton.Enabled = false;
                }
            }
            else
            {
                setInformationText("User validation needed", informationType.INFO, sender, e);
            }
        }
        private void DiscardGenHWDataChangesButton_Click(object sender, EventArgs e)
        {
            if (linwin.validated)
            {
                PropertyItem hwOwnItm = picture.GetPropertyItem(42032);
                if (hwOwnItm.Value.ToString() != "")
                    OwnerTextBox.Text = hwOwnItm.Value.ToString();
                else
                    OwnerTextBox.Text = "";
            }
            else
            {
                setInformationText("User validation needed", informationType.INFO, sender, e);
            }
        }
        // --- --- Geographical information handling --- ---
        private void LatValueTextBox_TextChanged(object sender, EventArgs e)
        {
            if (linwin.validated)
            {
                if (startUpDone)
                {
                    SaveGeoDataChangesButton.Enabled = true;
                    DiscardGeoDataChangesButton.Enabled = true;
                    geoLatValueChanged = true;
                    setInformationText($"Latitude value textbox changed to : \" {LatValueTextBox.Text}\"", informationType.INFO, sender, e);
                }
            }
            else
            {
                setInformationText("User validation needed", informationType.INFO, sender, e);
            }
        }
        private void LatRefTextBox_TextChanged(object sender, EventArgs e)
        {
            if (linwin.validated)
            {
                if (startUpDone)
                {
                    SaveGeoDataChangesButton.Enabled = true;
                    DiscardGenDataChangesButton.Enabled = true;
                    geoLatValueChanged = true;
                    setInformationText($"Latitude reference textbox changed to : \" {LatRefTextBox.Text}\"", informationType.INFO, sender, e);
                }
            }
            else
            {
                setInformationText("User validation needed", informationType.INFO, sender, e);
            }
        }
        private void LonValueTextBox_TextChanged(object sender, EventArgs e)
        {
            if (linwin.validated)
            {
                if (startUpDone)
                {
                    SaveGeoDataChangesButton.Enabled = true;
                    DiscardGenDataChangesButton.Enabled = true;
                    geoLonValueChanged = true;
                    setInformationText($"Latitude reference textbox changed to : \" {LonValueTextBox.Text}\"", informationType.INFO, sender, e);
                }
            }
            else
            {
                setInformationText("User validation needed", informationType.INFO, sender, e);
            }
        }
        private void LonRefTextBox_TextChanged(object sender, EventArgs e)
        {
            if (linwin.validated)
            {
                if (startUpDone)
                {
                    SaveGeoDataChangesButton.Enabled = true;
                    DiscardGenDataChangesButton.Enabled = true;
                    geoLonValueChanged = true;
                    setInformationText($"Latitude reference textbox changed to : \" {LonRefTextBox.Text}\"", informationType.INFO, sender, e);
                }
            }
            else
            {
                setInformationText("User validation needed", informationType.INFO, sender, e);
            }
        }
        private void ViewGeoDataPosButton_Click(object sender, EventArgs e)
        {
            if (linwin.validated)
            {
                float selectedLatValue = 0;
                string selectedLatDir = LatRefTextBox.Text;
                float selectedLonValue = 0;
                string selectedLonDir = LonRefTextBox.Text;
                float.TryParse(LatValueTextBox.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out selectedLatValue);
                if (selectedLatValue < 0)
                    selectedLatValue = -selectedLatValue;
                if (selectedLatValue > 90)
                    selectedLatValue = 90;
                if ((selectedLatDir == "S") || (selectedLatDir == "s") || (selectedLatDir == "South") || (selectedLatDir == "south"))
                    selectedLatValue = -selectedLatValue;
                float.TryParse(LonValueTextBox.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out selectedLonValue);
                if (selectedLonValue < 0)
                    selectedLonValue = -selectedLonValue;
                if (selectedLonValue > 180)
                    selectedLonValue = 180;
                if ((selectedLonDir == "W") || (selectedLonDir == "w") || (selectedLonDir == "West") || (selectedLonDir == "west"))
                    selectedLonValue = -selectedLonValue;
                // TODO - We have the position from given data, can we start Google-Earth to show the position?
            }
            else
            {
                setInformationText("User validation needed", informationType.INFO, sender, e);
            }
        }
        private void SaveGeoDataChangesButton_Click(object sender, EventArgs e)
        {
            if (linwin.validated)
            {
                if (geoLatValueChanged)
                {
                    try
                    {
                        string sNewLatValue = LatValueTextBox.Text;
                        byte[] bteNewLatValue = Encoding.Unicode.GetBytes(sNewLatValue);
                        string newLatDirection = LatRefTextBox.Text;
                        byte[] bteNewLatDirection = Encoding.Unicode.GetBytes(newLatDirection);
                        try
                        {
                            PropertyItem currLatItm = picture.GetPropertyItem(1);
                            currLatItm.Value = bteNewLatValue;
                            currLatItm.Len = bteNewLatValue.Length;
                            picture.SetPropertyItem(currLatItm);
                        }
                        catch (Exception err)
                        {
                            setInformationText("Saving latitude values failed with: " + err.ToString(), informationType.ERROR, sender, e);
                        }
                        try

                        {
                            PropertyItem currLatDir = picture.GetPropertyItem(2);
                            currLatDir.Value = bteNewLatDirection;
                            currLatDir.Len = bteNewLatDirection.Length;
                            picture.SetPropertyItem(currLatDir);
                        }
                        catch (Exception err)
                        {
                            setInformationText("Saving longitude values failed with: " + err.ToString(), informationType.ERROR, sender, e);
                        }
                    }
                    catch (Exception err)
                    {
                        setInformationText("Saving geographical Latitude position failed: " + Environment.NewLine + err.ToString(), informationType.ERROR, sender, e);
                    }
                }
            }
            else
            {
                setInformationText("User validation needed", informationType.INFO, sender, e);
            }
        }
        private void DiscardGeoDataChangesButton_Click(object sender, EventArgs e)
        {
            if (linwin.validated)
            {
                // TODO - Discard the changed geographical data.
            }
            else
            {
                setInformationText("User validation needed", informationType.INFO, sender, e);
            }
        }
        // --- --- Actor information handling --- ---
        private void actorViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (linwin.validated)
            {
                this.currentMode = programMode.ACTORVIEW;
                setInformationText("Entered actor data view mode.", informationType.INFO, sender, e);
                this.pictureCanvas.Visible = false;
                this.listView.Visible = true;
                this.tabControl.Visible = true;
                this.tabControl.SelectedTab = this.tabControl.TabPages["ActorData"];
                // TODO - Make all data editeable.
            }
            else
            {
                setInformationText("User validation needed", informationType.INFO, sender, e);
            }
        }
        private void setActorGUIInfo(ActorClass incommingActor)
        {
            // --- User names ---
            #region UserName
            NameTypeComboBox.Items.Clear();
            if (incommingActor.getNoOfUserNames() > 0)
            {
                for (int i = 0; i < incommingActor.getNoOfUserNames(); i++)
                {
                    NameTypeComboBox.Items.Add(incommingActor.getUserNameTag(i));
                }
                NameTypeComboBox.Items.Add("Add item...");
                NameTypeComboBox.SelectedItem = 0;
                SelNameTypeTextBox.Text = (incommingActor.getUserSurName(0) + " " + incommingActor.getUserMidName(0) + " " + incommingActor.getUserFamName(0));
            }
            else
                NameTypeComboBox.Items.Add("Add item...");
            #endregion
            // --- User contacts ---
            #region UserContacts
            ActorContactTypeComboBox.Items.Clear();
            if (incommingActor.getNoOfUserContacts() > 0)
            {
                for (int i = 0; i < incommingActor.getNoOfUserContacts(); i++)
                {
                    ActorContactTypeComboBox.Items.Add(incommingActor.getUserContactType(i));
                }
                ActorContactTypeComboBox.Items.Add("Add ítem...");
                ActorContactTypeComboBox.SelectedItem = 0;
                SelContactTypeTextBox.Text = incommingActor.getUserContactPath(0);
            }
            else
                ActorContactTypeComboBox.Items.Add("Add ítem...");
            #endregion
            // --- Birth data ---
            #region
            BirthStreetAddrTextBox.Text = (incommingActor.getUserBirthStreet() + " " + incommingActor.getUserBirthStreetNumber() + incommingActor.getUserBirthStreetNumberAddon());
            BirthZipCodeTextBox.Text = incommingActor.getUserBirthZipcode();
            BirthAreaNameTextBox.Text = incommingActor.getUserBirthAreaname();
            BirthCitynameTextBox.Text = incommingActor.getUserBirthCityname();
            BirthCountryTextBox.Text = incommingActor.getUserBirthCountry();
            BirthDateTextBox.Text = incommingActor.getUserBirthDate();
            SocSecNumberTextBox.Text = incommingActor.getUserBirthSocNo();
            #endregion
            // --- Skin-Tone ---
            #region SkinTone
            SkinToneDateTimePicker.Visible = false;
            SkinToneValidDateComboBox.Items.Clear();
            if (incommingActor.getNumberOfSkinTones() > 0)
            {
                for (int i = 0; i < incommingActor.getNumberOfSkinTones(); i++)
                    SkinToneValidDateComboBox.Items.Add(incommingActor.getUserSkinToneValidDate(i));
                SkinToneValidDateComboBox.Items.Add("Add item...");
                SkinToneValidDateComboBox.SelectedItem = 0;
                SkinToneTagTextBox.Text = incommingActor.getUserSkinToneTag(0);
            }
            else
                SkinToneValidDateComboBox.Items.Add("Add item...");
            SkinToneValidDateComboBox.Enabled = true;
            #endregion
            // --- Eyecolor ---
            #region EyeColor
            EyeColorValidDateComboBox.Items.Clear();
            EyeColorValidDateComboBox.Items.Add("Select...");
            for (int i = 0; i < incommingActor.getNumberOfEyeData(); i++)
                EyeColorValidDateComboBox.Items.Add(incommingActor.getUserEyeDataValidDate(i));
            EyeColorValidDateComboBox.Items.Add("Add Item...");
            if (incommingActor.getNumberOfEyeData() > 1)
            {
                EyeColorValidDateComboBox.SelectedIndex = 0;
                SelEyeColorTextBox.Text = "";
            }
            else
            {
                EyeColorValidDateComboBox.SelectedIndex = 1;
                SelEyeColorTextBox.Text = incommingActor.getUserEyeColorTag(0);
            }
            EyeColorValidDateComboBox.Enabled = true;
            #endregion
            // --- Gender Information ---
            #region GenderInfo
            if (linwin.getUserRightsValue() > 4)
            {
                GenderInfoValidDateComboBox.Items.Clear();
                if (incommingActor.getNumberOfGenderData() > 0)
                {
                    for (int i = 0; i < incommingActor.getNumberOfGenderData(); i++)
                        GenderInfoValidDateComboBox.Items.Add(incommingActor.getUserGenderInfoValidDate(i));
                    GenderInfoValidDateComboBox.Items.Add("Add item...");
                    GenderTypeTextBox.Text = incommingActor.getUserGenderType(0);
                    // ---                    : <Length unit-tb><circumference unit-tb> ---
                    GdrLengthTextBox.Text = incommingActor.getUserGenderLength(0);
                    GdrCircumfTextBox.Text = incommingActor.getUserGenderCircumf(0);
                    // ---                    : <look-tb><behave-tb> ---
                    GdrLookTypeTextBox.Text = incommingActor.getUserGenderAppearance(0);
                    GdrBehaveTypeTextBox.Text = incommingActor.getUserGenderBehaviour(0);
                    // ---                    : <presentation-tb> ---
                    GdrPresentTextBox.Text = incommingActor.getUserGenderPres(0);
                }
                else
                    GenderInfoValidDateComboBox.Items.Add("Add item...");
                GenderInfoValidDateComboBox.Enabled = true;
            }
            #endregion
            // --- Length Information ---
            #region LengthInfo
            LengthValidDateComboBox.Items.Clear();
            if (incommingActor.getNumberOfLengthData() > 0)
            {
                for (int i = 0; i < incommingActor.getNumberOfLengthData(); i++)
                    LengthValidDateComboBox.Items.Add(incommingActor.getUserLengthInfoValidDate(i));
                LengthValidDateComboBox.Items.Add("Add item...");
                LengthTextBox.Text = incommingActor.getUserLengthValue(0);
            }
            else
                LengthValidDateComboBox.Items.Add("Add item...");
            LengthValidDateComboBox.Enabled = true;
            #endregion
            // --- Weight Information ---
            #region WeightInformation
            WeightValidDateComboBox.Items.Clear();
            if (incommingActor.getNumberOfWeightData() > 0)
            {
                for (int i = 0; i < incommingActor.getNumberOfWeightData(); i++)
                    WeightValidDateComboBox.Items.Add(incommingActor.getUserWeightValue(i));
                WeightValidDateComboBox.Items.Add("Add item...");
                WeightTextBox.Text = incommingActor.getUserWeightValue(0);
            }
            else
                WeightValidDateComboBox.Items.Add("Add item...");
            WeightValidDateComboBox.Enabled = true;
            #endregion
            // --- Chest data ---
            #region ChestData
            ChestValidDateComboBox.Items.Clear();
            if (incommingActor.getNumberOfChestData() > 0)
            {
                for (int i = 0; i < incommingActor.getNumberOfChestData(); i++)
                    ChestValidDateComboBox.Items.Add(incommingActor.getUserChestInfoValidDate(i));
                ChestValidDateComboBox.Items.Add("Add item...");
                ChestTypeTextBox.Text = incommingActor.getUserChestType(0);
                // ---                    : <circ-tb><size-tb> ---
                ChestCircTextBox.Text = incommingActor.getUserChestCircumfValue(0);
                ChestSizeTextBox.Text = incommingActor.getUserChestSizeType(0);
            }
            else
                ChestValidDateComboBox.Items.Add("Add item...");
            ChestValidDateComboBox.Enabled = true;
            #endregion
            // --- Hair data ---
            #region HairData
            HairDataValidDateComboBox.Items.Clear();
            if (incommingActor.getNumberOfHairData() > 0)
            {
                for (int i = 0; i < incommingActor.getNumberOfHairData(); i++)
                    HairDataValidDateComboBox.Items.Add(incommingActor.getUserHairValidDate(i));
                HairDataValidDateComboBox.Items.Add("Add item...");
                HairColorTextBox.Text = incommingActor.getUserHairColor(0);
                // ---                    : <texture-tb><length-tb> ---
                HairTextureTypeTextBox.Text = incommingActor.getUserHairTexture(0);
                HairLengthTextBox.Text = incommingActor.getUserHairLength(0);
            }
            else
                HairDataValidDateComboBox.Items.Add("Add item...");
            HairDataValidDateComboBox.Enabled = true;
            #endregion
            // --- Markings data ---
            #region MarkingsData
            MarkingTypeComboBox.Items.Clear();
            MarkingsValidDateComboBox.Items.Clear();
            MarkingTypeComboBox.Items.Add("Select...");
            MarkingsValidDateComboBox.Items.Add("Select...");
            for (int i = 0; i < incommingActor.getNoOfMarkingsData(); i++)
            {
                MarkingTypeComboBox.Items.Add(incommingActor.getActorMarkingType(i));
                MarkingsValidDateComboBox.Items.Add(incommingActor.getActorMarkingValidDate(i));
            }
            MarkingTypeComboBox.Items.Add("Add Item...");
            MarkingsValidDateComboBox.Items.Add("Add Item...");
            if ((incommingActor.getNoOfMarkingsData() > 1) || (incommingActor.getNoOfMarkingsData() == 0))
            {
                MarkingPosTextBox.Text = "";
                MarkingMotifTextBox.Text = "";
                MarkingDateTextBox.Text = "";
                MarkingTypeComboBox.SelectedIndex = 0;
                MarkingsValidDateComboBox.SelectedIndex = 0;
            }
            else
            {
                MarkingPosTextBox.Text = incommingActor.getActorMarkingPlace(1);
                MarkingMotifTextBox.Text = incommingActor.getActorMarkingMotif(1);
                MarkingDateTextBox.Text = incommingActor.getActorMarkingValidDate(1);
                MarkingTypeComboBox.SelectedIndex = 1;
                MarkingsValidDateComboBox.SelectedIndex = 1;
            }
            MarkingTypeComboBox.Enabled = true;
            MarkingTypeComboBox.Visible = true;
            MarkingsValidDateComboBox.Enabled = true;
            MarkingsValidDateComboBox.Visible = true;
            #endregion
            // --- Occupation data ---
            #region OccupationData
            ActorOccupationStartDateComboBox.Items.Clear();
            ActorOccupationStartDateComboBox.Items.Add("Select...");
            ActorOccupationEndDaeComboBox.Items.Clear();
            ActorOccupationEndDaeComboBox.Items.Add("Select...");
            for (int i = 0; i < incommingActor.getNoOfOccupationsData(); i++)
            {
                if (incommingActor.getActorOccupationStarted(i) != "")
                    ActorOccupationStartDateComboBox.Items.Add(incommingActor.getActorOccupationStarted(i));
                if (incommingActor.getActorOccupationEnded(i) != "")
                    ActorOccupationEndDaeComboBox.Items.Add(incommingActor.getActorOccupationEnded(i));
            }
            ActorOccupationStartDateComboBox.Items.Add("Add Item...");
            ActorOccupationEndDaeComboBox.Items.Add("Add Item...");
            if (incommingActor.getNoOfOccupationsData() > 1)
            {
                ActorOccupationStartDateComboBox.SelectedIndex = 0;
                ActorOccupationEndDaeComboBox.SelectedIndex = 0;
                OccupTitleTextBox.Text = "";
                ActorOccupationCompanyTextBox.Text = "";
                ActorEmployAddressTextBox.Text = "";
                ActorEmployZipCodeTextBox.Text = "";
                ActorEmpoyAreanameTextBox.Text = "";
                ActorEmployCitynameTextBox.Text = "";
                ActorEmployCountryTextBox.Text = "";
                AddOccupationDataButton.Enabled = false;
            }
            else
            {
                ActorOccupationStartDateComboBox.SelectedIndex = 1;
                if (ActorOccupationEndDaeComboBox.Items.Count > 2)
                    ActorOccupationEndDaeComboBox.SelectedIndex = 1;
                else
                    ActorOccupationEndDaeComboBox.SelectedIndex = 0;
                OccupTitleTextBox.Text = incommingActor.getActorOccupationTitle(0);
                ActorOccupationCompanyTextBox.Text = incommingActor.getActorOccupationCompany(0);
                ActorEmployAddressTextBox.Text = incommingActor.getActorOccupationStreetname(0);
                ActorEmployZipCodeTextBox.Text = incommingActor.getActorOccupationZipcode(0);
                ActorEmpoyAreanameTextBox.Text = incommingActor.getActorOccupationAreaname(0);
                // TODO - Fix the occupation cityname, it is missing in the occupation data.
                //ActorEmployCitynameTextBox.Text = incommingActor.getActorOccupationCityname(0);
                ActorEmployCountryTextBox.Text = incommingActor.getActorOccupationCountry(0);
                AddOccupationDataButton.Enabled = false;
            }
            ActorOccupationStartDateComboBox.Enabled = true;
            ActorOccupationEndDaeComboBox.Enabled = true;
            #endregion
            // --- Residence Data ---
            #region ResidenceData
            ResidStartDateComboBox.Items.Clear();
            ResidStartDateComboBox.Items.Add("Select...");
            ResidEndDateComboBox.Items.Clear();
            ResidEndDateComboBox.Items.Add("Select...");
            for (int i = 0; i < incommingActor.getNoOfResicenceData(); i++)
            {
                if (incommingActor.getActorResidBought(i) != "")
                    ResidStartDateComboBox.Items.Add(incommingActor.getActorResidBought(i));
                if (incommingActor.getActorResidSold(i) != "")
                    ResidEndDateComboBox.Items.Add(incommingActor.getActorResidSold(i));
            }
            ResidStartDateComboBox.Items.Add("Add Item...");
            ResidEndDateComboBox.Items.Add("Add Item...");
            if (incommingActor.getNoOfResicenceData() > 1)
            {
                ResidAddressTextBox.Text = "";
                ResidZipCodeTextBox.Text = "";
                ResidAreanameTextBox.Text = "";
                ResidCitynameTextBox.Text = "";
                ResidCountryTextBox.Text = "";
                AddResidenceButton.Enabled = false;
            }
            else
            {
                ResidAddressTextBox.Text = incommingActor.getActorResidStreetname(0);
                ResidZipCodeTextBox.Text = incommingActor.getActorResidZipcode(0);
                ResidAreanameTextBox.Text = incommingActor.getActorResidArea(0);
                ResidCitynameTextBox.Text = incommingActor.getActorResidCity(0);
                ResidCountryTextBox.Text = incommingActor.getActorResidCountry(0);
                AddResidenceButton.Enabled = false;
            }
            ResidStartDateComboBox.Enabled = true;
            ResidEndDateComboBox.Enabled = true;
            #endregion
            // --- Events Data ---
            #region EventsData
            AttdEventsDateComboBox.Items.Clear();
            activeEventComboBox.Items.Clear();
            AttdEventsDateComboBox.Items.Add("Select...");
            activeEventComboBox.Items.Add("Select...");
            for (int i = 0; i < incommingActor.getNoOfAttendedEventsData(); i++)
            {
                string catTag = incommingActor.getActorAttendedEventType(i);
                int lvlVal = 0;
                for (int j = 0; j < linwin.getNoOfEvtCat(); j++)
                {
                    if (catTag == linwin.getEvtCatTag(j))
                        lvlVal = linwin.getEvtCatLevelValue(j);
                }
                if (linwin.getUserRightsValue() >= lvlVal)
                {

                    AttdEventsDateComboBox.Items.Add(incommingActor.getActorAttendedEventStarted(i));
                    activeEventComboBox.Items.Add(incommingActor.getActorAttendedEventStarted(i));
                }
            }
            AttdEventsDateComboBox.Items.Add("Add Item...");
            activeEventComboBox.Items.Add("Add Item...");
            if (AttdEventsDateComboBox.Items.Count == 3)
            {
                AttdEventsDateComboBox.SelectedIndex = 1;
                activeEventComboBox.SelectedIndex = 1;
                EventIdTextBox.Text = incommingActor.getActorAttendedEventID(0);
            }
            else
            {
                AttdEventsDateComboBox.SelectedIndex = 0;
                activeEventComboBox.SelectedIndex = 0;
                EventIdTextBox.Text = "";
            }
            AttdEventsDateComboBox.Enabled = true;
            #endregion
            // --- Images Data ---
            #region ImagesData
            ActorRelImageComboBox.Items.Clear();
            ActorRelImageComboBox.Items.Add("Select...");
            for (int i = 0; i < incommingActor.getNoOfRelatedImages(); i++)
            {
                if (linwin.getUserRightsValue() >= incommingActor.getActorRelatedImageClassValue(i))
                {
                    string tempImPth = incommingActor.getActorRelatedImagePath(i);
                    int dp0 = tempImPth.LastIndexOf("\\");
                    if ((dp0 > 0) && (dp0 < tempImPth.Length - 2))
                        tempImPth = tempImPth.Substring(dp0 + 1, tempImPth.Length - dp0 - 1);
                    ActorRelImageComboBox.Items.Add(tempImPth);
                }
            }
            ActorRelImageComboBox.Items.Add("Add Item...");
            if (ActorRelImageComboBox.Items.Count == 3)
                ActorRelImageComboBox.SelectedIndex = 1;
            else
                ActorRelImageComboBox.SelectedIndex = 0;
            ViewSelActorRelImageButton.Enabled = true;
            #endregion
        }
        private void ActiveArtistsComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (linwin.validated)
            {
                if (this.ActiveArtistsComboBox.Text == "Add actor...")
                {
                    // Operator selected "Add artist..."
                    this.NameTypeComboBox.Items.Clear();
                    this.ViewSelArtistDataButton.Visible = false;
                    this.ArtistIdentityEnterTextBox.Visible = true;
                    this.ArtistIdentityEnterTextBox.Enabled = true;
                    #region NewActorNameEntry
                    this.NameTypeComboBox.Items.Add("Birth Name");
                    this.NameTypeComboBox.Items.Add("Taken Name");
                    this.NameTypeComboBox.Items.Add("Married Name");
                    this.NameTypeComboBox.Items.Add("Alias Name");
                    this.NameTypeComboBox.Items.Add("Nickname");
                    this.NameTypeComboBox.Items.Add("Other type");
                    this.NameTypeComboBox.Enabled = true;
                    this.SelNameTypeTextBox.Width = 150;
                    this.SelNameTypeTextBox.Enabled = true;
                    this.AddNewNameButton.Visible = true;
                    this.AddNewNameButton.Enabled = true;
                    #endregion
                    #region NewActorContactsEntry
                    // UndefContactType, Phone, Email, Website, Facebook, Twitter, LinkedIn, Instagram, Snapchat
                    this.ActorContactTypeComboBox.Items.Add("Phone");
                    this.ActorContactTypeComboBox.Items.Add("Email");
                    this.ActorContactTypeComboBox.Items.Add("Website");
                    this.ActorContactTypeComboBox.Items.Add("Facebook");
                    this.ActorContactTypeComboBox.Items.Add("Twitter");
                    this.ActorContactTypeComboBox.Items.Add("LinkedIn");
                    this.ActorContactTypeComboBox.Items.Add("Instagram");
                    this.ActorContactTypeComboBox.Items.Add("Snapchat");
                    this.ActorContactTypeComboBox.Items.Add("Other type");
                    this.ActorContactTypeComboBox.Enabled = true;
                    this.SelContactTypeTextBox.Enabled = true;
                    this.SelContactTypeTextBox.Width = 150;
                    this.AddContactButton.Visible = true;
                    this.AddContactButton.Enabled = true;
                    #endregion
                    #region NewActorBirthEntry
                    this.BirthStreetAddrTextBox.Enabled = true;
                    this.BirthZipCodeTextBox.Enabled = true;
                    this.BirthAreaNameTextBox.Enabled = true;
                    this.BirthCitynameTextBox.Enabled = true;
                    this.BirthCountryTextBox.Enabled = true;
                    this.BirthDateTextBox.Enabled = true;
                    this.SocSecNumberTextBox.Enabled = true;
                    this.BirthLatitudeTextBox.Enabled = true;
                    this.BirthLongitudeTextBox.Enabled = true;
                    #endregion
                    #region NewActorComplexionData
                    this.SkinToneTagTextBox.Enabled = true;
                    this.SkinToneValidDateComboBox.Items.Add(System.DateTime.Now.ToString("yyyy-MM-dd"));
                    this.SkinToneValidDateComboBox.Items.Add("Other date");
                    this.SkinToneValidDateComboBox.Enabled = true;
                    this.SkinToneLabel.Visible = false;
                    this.AddSkinToneButton.Visible = true;
                    this.AddSkinToneButton.Enabled = true;
                    #endregion
                    #region NewActorEyeColorEntry
                    this.SelEyeColorTextBox.Enabled = true;
                    this.EyeColorValidDateComboBox.Items.Add(System.DateTime.Now.ToString("yyyy-MM-dd"));
                    this.EyeColorValidDateComboBox.Items.Add("Other date");
                    this.EyeColorValidDateComboBox.Enabled = true;
                    this.EyeColorLabel.Visible = false;
                    this.AddEyeColorButton.Visible = true;
                    this.AddEyeColorButton.Enabled = true;
                    #endregion
                    #region NewActorGenderInfoEntry
                    this.GenderInfoLabel.Visible = false;
                    this.AddGenderInfoButton.Visible = true;
                    this.AddGenderInfoButton.Enabled = true;
                    this.GenderTypeTextBox.Enabled = true;
                    this.GenderInfoValidDateComboBox.Items.Add(System.DateTime.Now.ToString("yyyy-MM-dd"));
                    this.GenderInfoValidDateComboBox.Items.Add("Other date");
                    this.GenderInfoValidDateComboBox.Enabled = true;
                    this.GdrLengthTextBox.Enabled = true;
                    this.GdrCircumfTextBox.Enabled = true;
                    this.GdrLookTypeTextBox.Enabled = true;
                    this.GdrBehaveTypeTextBox.Enabled = true;
                    this.GdrPresentTextBox.Enabled = true;
                    #endregion
                    #region NewActorLengthData
                    this.LengthLabel.Visible = false;
                    this.AddLengthInfoButton.Visible = true;
                    this.AddLengthInfoButton.Enabled = true;
                    this.LengthTextBox.Enabled = true;
                    this.LengthValidDateComboBox.Items.Add(System.DateTime.Now.ToString("yyyy-MM-dd"));
                    this.LengthValidDateComboBox.Items.Add("Other date");
                    this.LengthValidDateComboBox.Enabled = true;
                    #endregion
                    #region NewActorWeightData
                    this.WeightLabel.Visible = false;
                    this.AddWeightInfoButton.Visible = true;
                    this.AddWeightInfoButton.Enabled = true;
                    this.WeightTextBox.Enabled = true;
                    this.WeightValidDateComboBox.Items.Add(System.DateTime.Now.ToString("yyyy-MM-dd"));
                    this.WeightValidDateComboBox.Items.Add("Other date");
                    this.LengthValidDateComboBox.Enabled = true;
                    #endregion
                    #region NewActorChestData
                    this.ChestDataLabel.Visible = false;
                    this.AddChestInfoButton.Visible = true;
                    this.AddChestInfoButton.Enabled = true;
                    this.ChestCircTextBox.Enabled = true;
                    this.ChestValidDateComboBox.Items.Add(System.DateTime.Now.ToString("yyyy-MM-dd"));
                    this.ChestValidDateComboBox.Items.Add("Other date");
                    this.ChestValidDateComboBox.Enabled = true;
                    this.ChestSizeTextBox.Enabled = true;
                    this.ChestTypeTextBox.Enabled = true;
                    #endregion
                    #region NewActorHairData
                    this.HairDataLabel.Visible = false;
                    this.AddHairInfoButton.Visible = true;
                    this.AddHairInfoButton.Enabled = true;
                    // TODO - ComboBox with HairColorType
                    this.HairColorTextBox.Enabled = true;   
                    this.HairDataValidDateComboBox.Items.Add(System.DateTime.Now.ToString("yyyy-MM-dd"));
                    this.HairDataValidDateComboBox.Items.Add("Other date");
                    this.HairDataValidDateComboBox.Enabled = true;
                    // TODO - ComboBox with HairTextureType
                    this.HairTextureTypeTextBox.Enabled = true;
                    // TODO - ComboBox with HairLengthType
                    this.HairLengthTextBox.Enabled = true;      
                    #endregion
                    #region NewActorMarkingsInfoEntry
                    // UndefMarking, Scar, Freckles, Birthmark, Tattoo, Piercing
                    this.MarkingTypeComboBox.Items.Add("Scar");
                    this.MarkingTypeComboBox.Items.Add("Freckles");
                    this.MarkingTypeComboBox.Items.Add("Birthmarks");
                    this.MarkingTypeComboBox.Items.Add("Tattoo");
                    this.MarkingTypeComboBox.Items.Add("Piercings");
                    this.MarkingTypeComboBox.Items.Add("Other marking");
                    this.MarkingTypeComboBox.Enabled = true;
                    this.MarkingPosTextBox.Enabled = true;
                    this.MarkingMotifTextBox.Enabled = true;
                    this.MarkingDateTextBox.Enabled = false;
                    this.MarkingsValidDateComboBox.Items.Add(System.DateTime.Now.ToString("yyyy-MMM-dd"));
                    this.MarkingsValidDateComboBox.Items.Add("Other date");
                    this.MarkingsValidDateComboBox.Visible = true;
                    this.MarkingsValidDateComboBox.Enabled = true;
                    this.AddMarkingDataButton.Visible = true;
                    this.AddMarkingDataButton.Enabled = true;
                    #endregion
                    #region NewActorOccupationInfoEntry
                    this.OccupTitleTextBox.Enabled = true;
                    this.ActorOccupationStartDateComboBox.Items.Add(System.DateTime.Now.ToString("yyyy-MM-dd"));
                    this.ActorOccupationStartDateComboBox.Items.Add("Other date");
                    this.ActorOccupationStartDateComboBox.Enabled = true;
                    this.ActorOccupationCompanyTextBox.Enabled = true;
                    this.ActorEmployAddressTextBox.Enabled = true;
                    this.ActorEmployZipCodeTextBox.Enabled = true;
                    this.ActorEmpoyAreanameTextBox.Enabled = true;
                    this.ActorEmployCitynameTextBox.Enabled = true;
                    this.ActorEmployCountryTextBox.Enabled = true;
                    this.ActorOccupationEndDaeComboBox.Items.Add(System.DateTime.Now.ToString("yyyy-MM-dd"));
                    this.ActorOccupationEndDaeComboBox.Items.Add("Not known");
                    this.ActorOccupationEndDaeComboBox.Items.Add("Other date");
                    this.ActorOccupationEndDaeComboBox.Enabled = true;
                    this.AddOccupationDataButton.Visible = true;
                    this.AddOccupationDataButton.Enabled = true;
                    #endregion
                    #region NewActorResicenceInfoEntry
                    this.ResidAddressTextBox.Enabled = true;
                    this.ResidStartDateComboBox.Items.Add(System.DateTime.Now.ToString("yyyy-MM-dd"));
                    this.ResidStartDateComboBox.Items.Add("Other date");
                    this.ResidStartDateComboBox.Enabled = true;
                    this.ResidZipCodeTextBox.Enabled = true;
                    this.ResidAreanameTextBox.Enabled = true;
                    this.AddResidenceButton.Visible = true;
                    this.AddResidenceButton.Enabled = true;
                    this.ResidCitynameTextBox.Enabled = true;
                    this.ResidEndDateComboBox.Items.Add(System.DateTime.Now.ToString("yyyy-MM-dd"));
                    this.ResidEndDateComboBox.Items.Add("Other date");
                    this.ResidEndDateComboBox.Enabled = true;
                    this.ResidCountryTextBox.Enabled = true;
                    #endregion
                    #region NewActorAttendedEventsInfoEntry
                    // TODO - this might be better as a ComboBox with existing events?
                    this.EventIdTextBox.Enabled = true; 
                    this.AttdEventsDateComboBox.Items.Add(System.DateTime.Now.ToString("yyyyy-MM-dd"));
                    this.AttdEventsDateComboBox.Items.Add("Other date");
                    this.AttdEventsDateComboBox.Enabled = true;
                    #endregion
                    #region NewActorRelImagesComboBox
                    this.ViewSelActorRelImageButton.Enabled = false;
                    this.ViewSelActorRelImageButton.Visible = false;
                    this.ActorRelImageComboBox.Width = 200;
                    this.ActorRelImageComboBox.Items.Add("Select image");
                    // TODO - Add a functionality to detect last directory used and list the images there.
                    #endregion
                    this.SaveActorDataChangesButton.Enabled = true;
                    this.DiscardActorDataChangesButton.Enabled = true;
                }
                else
                {
                    // Operator selected an artist from the database.
                    if (ActiveArtistsComboBox == null) return;

                    actorClass = new ActorClass();
                    for (int i = 0; i < linwin.noOfEventCategories; i++)
                        actorClass.addEventCategory(linwin.getEvtCatTag(i), linwin.getEvtCatDescr(i), linwin.getEvtCatLevel(i));
                    for (int i = 0; i < linwin.noOfImageCategories; i++)
                        actorClass.addContextCategory(linwin.getImgCatTag(i), linwin.getImgCatDescr(i), linwin.getImgCatLevel(i));
                    for (int i = 0; i < linwin.noOfRelationCategories; i++)
                        actorClass.addRelationCategory(linwin.getRelationCategoryTag(i), linwin.getRelationCategoryDescription(i), linwin.getRelationCategoryLevel(i));
                    for (int i = 0; i < linwin.noOfCurrencies; i++)
                        actorClass.addCurrencyCategory(linwin.getCurrencyTag(i), linwin.getCurrencyDescr(i), "Undefined", (double)linwin.getCurrencyValue(i));
                    for (int i = 0; i < linwin.noOfHairColors; i++)
                        actorClass.addHairColorCategory(linwin.getHairColorTag(i), linwin.getHairColorDescr(i), linwin.getHairColorLevel(i));

                    if ((linwin.getActorStoragePath() != null) && (ActiveArtistsComboBox.SelectedItem != null) &&
                        (System.IO.Directory.Exists(linwin.getActorStoragePath()) && (System.IO.File.Exists(linwin.getActorStoragePath() + "\\ActorData_" + ActiveArtistsComboBox.SelectedItem + ".acf"))))
                        actorClass.loadActor((string)ActiveArtistsComboBox.SelectedItem, linwin.getActorStoragePath());
                    else if ((System.IO.Directory.Exists("C:\\Users\\" + currUser + "\\" + sProgPath + "\\ActorData")) &&
                             (System.IO.File.Exists("C:\\Users\\" + currUser + "\\" + sProgPath + "\\ActorData\\ActorData_" + ActiveArtistsComboBox.SelectedItem + ".acf")))
                        actorClass.loadActor((string)ActiveArtistsComboBox.SelectedItem, "C:\\Users\\" + currUser + "\\" + sProgPath + "\\ActorData");
                    else
                        setInformationText("Actor data directory or file does not exist.", informationType.ERROR, sender, e);

                    arrayOfActors[numberOfUsedActors] = actorClass;
                    ViewSelArtistDataButton.Visible = false;
                    ArtistIdentityEnterTextBox.Visible = false;
                    setActorGUIInfo(actorClass);
                    numberOfUsedActors++;
                }
            }
            else
            {
                setInformationText("User validation needed", informationType.INFO, sender, e);
            }
        }
        private void ArtistIdentityEnterTextBox_TextChanged(object sender, EventArgs e)
        {
            // A name search has been initiated, look for the given phrase as an id or name.
            string searchPhrase = ArtistIdentityEnterTextBox.Text;

            if ((linwin.getActorStoragePath() != null) && (searchPhrase != "") &&
                (System.IO.Directory.Exists(linwin.getActorStoragePath())) &&
                (System.IO.File.Exists(linwin.getActorStoragePath() + "\\ActorData_" + searchPhrase + ".acf")))
            {
                cleanImageGUIMetaDataValues();
                actorClass.loadActor(searchPhrase, linwin.getActorStoragePath());
            }
            else if ((System.IO.Directory.Exists("C:\\Users\\" + currUser + "\\" + sProgPath + "\\ActorData")) &&
                     (System.IO.File.Exists("C:\\Users\\" + currUser + "\\" + sProgPath + "\\ActorData\\ActorData_" + searchPhrase + ".acf")))
            {
                cleanImageGUIMetaDataValues();
                actorClass.loadActor(searchPhrase, "C:\\Users\\" + currUser + "\\" + sProgPath + "\\ActorData");
            }
            else if ((linwin.getActorStoragePath() != null) && (System.IO.Directory.Exists(linwin.getActorStoragePath())))
            {
                string[] fileArray = System.IO.Directory.GetFiles(linwin.getActorStoragePath());
                if (fileArray != null)
                {
                    cleanImageGUIMetaDataValues();
                    this.ActiveArtistsComboBox.Items.Clear();
                    this.ActiveArtistsComboBox.Items.Add("Select...");
                    ActorClass tempActorClass;
                    tempActorClass = new ActorClass();
                    for (int i = 0; i < linwin.noOfEventCategories; i++)
                        tempActorClass.addEventCategory(linwin.getEvtCatTag(i), linwin.getEvtCatDescr(i), linwin.getEvtCatLevel(i));
                    for (int i = 0; i < linwin.noOfImageCategories; i++)
                        tempActorClass.addContextCategory(linwin.getImgCatTag(i), linwin.getImgCatDescr(i), linwin.getImgCatLevel(i));
                    for (int i = 0; i < linwin.noOfRelationCategories; i++)
                        tempActorClass.addRelationCategory(linwin.getRelationCategoryTag(i), linwin.getRelationCategoryDescription(i), linwin.getRelationCategoryLevel(i));
                    for (int i = 0; i < linwin.noOfCurrencies; i++)
                        tempActorClass.addCurrencyCategory(linwin.getCurrencyTag(i), linwin.getCurrencyDescr(i), "Undefined", (double)linwin.getCurrencyValue(i));
                    for (int i = 0; i < linwin.noOfHairColors; i++)
                        tempActorClass.addHairColorCategory(linwin.getHairColorTag(i), linwin.getHairColorDescr(i), linwin.getHairColorLevel(i));


                    foreach (var exiFile in fileArray)
                    {
                        int dpU = exiFile.LastIndexOf("_");
                        int dpP = exiFile.LastIndexOf(".");
                        int noofartists = 0;
                        string strfuna = exiFile.Substring(dpU + 1, exiFile.Length - dpP);
                        tempActorClass.loadActor(strfuna, linwin.getActorStoragePath());
                        for (int i = 0; i < tempActorClass.getNoOfUserNames(); i++)
                        {
                            if ((tempActorClass.getUserFamName(i) == searchPhrase) || (tempActorClass.getUserSurName(i) == searchPhrase))
                            {
                                this.ActiveArtistsComboBox.Items.Add(tempActorClass.getUserNameTag(i));
                                arrayOfActors[noofartists] = tempActorClass;
                                noofartists++;
                            }
                        }
                        this.ActiveArtistsComboBox.Items.Add("Add Item...");
                        if (noofartists > 0)
                        {
                            if (noofartists > 1)
                            {
                                this.ActiveArtistsComboBox.SelectedIndex = 0;
                                ArtistIdentityEnterTextBox.Visible = false;
                            }
                            else
                            {
                                this.ActiveArtistsComboBox.SelectedIndex = 1;
                                ViewSelArtistDataButton.Visible = false;
                                ArtistIdentityEnterTextBox.Enabled = false;
                                setActorGUIInfo(arrayOfActors[0]);
                            }
                        }
                    }
                }
            }
            else
                setInformationText("Searchphrase was not found.", informationType.INFO, sender, e);
        }
        private void ViewSelArtistDataButton_Click(object sender, EventArgs e)
        {
            if (linwin.validated)
            {
                // TODO - Handle adding changed data into the active artists data.
                ViewSelArtistDataButton.Enabled = false;
                // TODO - Make all artist data GUIs enabled.
                ViewSelArtistDataButton.Text = "Save data";
            }
            else
            {
                setInformationText("User validation needed", informationType.INFO, sender, e);
            }
        }
        private void ViewSelArtistDataButton_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (linwin.validated)
            {
                ViewSelArtistDataButton.Visible = true;
                ArtistIdentityEnterTextBox.Visible = false;
            }
            else
            {
                setInformationText("User validation needed", informationType.INFO, sender, e);
            }
        }
        private void SkinToneValidDateComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (linwin.validated)
            {
                if (SkinToneValidDateComboBox.SelectedItem.ToString() == "Other date")
                {
                    // TODO - Make Calender selection visible.
                    this.SkinToneValidDateComboBox.Visible = false;
                    this.SkinToneDateTimePicker.Visible = true;
                    this.SkinToneDateTimePicker.Enabled = true;
                }
                else
                {
                    // TODO - Set Current date into the data holder.
                }
            }
            else
            {
                setInformationText("User validation needed", informationType.INFO, sender, e);
            }
        }
        private void DiscardActorDataChangesButton_Click(object sender, EventArgs e)
        {
            if (linwin.validated)
            {
                // TODO - Reload the original image metadata

                SaveActorDataChangesButton.Enabled = false;
                DiscardActorDataChangesButton.Enabled = false;
            }
            else
            {
                setInformationText("User validation needed", informationType.INFO, sender, e);
            }
        }
        private void SaveActorDataChangesButton_Click(object sender, EventArgs e)
        {
            if (linwin.validated)
            {
                // TODO - Ett allmänt fel från GDI+ om metadata för bilden har ändrats.
                picture.Save(loadedImageFileName); 
                SaveActorDataChangesButton.Enabled = false;
                DiscardActorDataChangesButton.Enabled = false;
                saveToolStripMenuItem.Enabled = false;
                //picture.Dispose();
                changesToSave = true;
                saveToolStripMenuItem.Enabled = true;
            }
            else
            {
                setInformationText("User validation needed", informationType.INFO, sender, e);
            }
        }
        private void NameTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (linwin.validated)
            {
                int selNmeIdx = NameTypeComboBox.SelectedIndex;
                if ((selNmeIdx - 1) < actorClass.getNoOfUserNames())
                {
                    if ((actorClass.getUserNameTag(selNmeIdx) == "Alias") || (actorClass.getUserNameTag(selNmeIdx) == "Nickname"))
                        SelNameTypeTextBox.Text = actorClass.getUserSurName(selNmeIdx);
                    else
                    {
                        if (actorClass.getUserMidName(selNmeIdx) == "")
                            SelNameTypeTextBox.Text = actorClass.getUserSurName(selNmeIdx) + " " + actorClass.getUserFamName(selNmeIdx);
                        else
                            SelNameTypeTextBox.Text = actorClass.getUserSurName(selNmeIdx) + " " + actorClass.getUserMidName(selNmeIdx) + " " + actorClass.getUserFamName(selNmeIdx);
                    }
                }
                else
                {
                    NameTypeComboBox.Items.Clear();
                    NameTypeComboBox.Items.Add("Select...");
                    NameTypeComboBox.Items.Add("Birth");
                    NameTypeComboBox.Items.Add("Taken");
                    NameTypeComboBox.Items.Add("Married");
                    NameTypeComboBox.Items.Add("Alias");
                    NameTypeComboBox.Items.Add("Nickname");
                    NameTypeComboBox.SelectedIndex = 0;
                    SelNameTypeTextBox.Text = "";
                    SelNameTypeTextBox.Enabled = true;
                }
            }
            else
            {
                setInformationText("User validation needed", informationType.INFO, sender, e);
            }
        }
        private void SelNameTypeTextBox_TextChanged(object sender, EventArgs e)
        {
            AddNewNameButton.Enabled = true;
        }
        private void AddNewNameButton_Click(object sender, EventArgs e)
        {
            string strNameToAdd = SelNameTypeTextBox.Text;
            int dpSN = strNameToAdd.IndexOf(" ");
            if ((dpSN > 0) && (dpSN < strNameToAdd.Length - 1))
            {
                // Have at least surname familyname
                string strurn = strNameToAdd.Substring(0, dpSN);
                strNameToAdd = strNameToAdd.Substring(dpSN + 1, strNameToAdd.Length - dpSN - 1);
                dpSN = strNameToAdd.IndexOf(" ");
                if ((dpSN > 0) && (dpSN < strNameToAdd.Length - 1))
                    actorClass.addUserName(NameTypeComboBox.SelectedItem.ToString(), strurn, strNameToAdd.Substring(0, dpSN), strNameToAdd.Substring(dpSN + 1, strNameToAdd.Length - dpSN - 1));
                else
                    actorClass.addUserName(NameTypeComboBox.SelectedItem.ToString(), strurn, strNameToAdd.Substring(0, dpSN), "");
            }
            else
                actorClass.addUserName(NameTypeComboBox.SelectedItem.ToString(), strNameToAdd, "", "");

            NameTypeComboBox.Items.Clear();
            NameTypeComboBox.Items.Add("Select...");
            for (int i = 0; i < actorClass.getNoOfUserNames(); i++)
                NameTypeComboBox.Items.Add(actorClass.getUserNameTag(i));
            NameTypeComboBox.Items.Add("Add Item...");
            NameTypeComboBox.SelectedIndex = 0;
            SelNameTypeTextBox.Text = "";
            SelNameTypeTextBox.Enabled = false;
            AddNewNameButton.Enabled = false;
        }
        private void ActorContactTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (linwin.validated)
            {
                int selCtcTpe = ActorContactTypeComboBox.SelectedIndex;
                if ((selCtcTpe - 1) < actorClass.getNoOfUserContacts())
                {
                    // Showing an existing contact
                    SelContactTypeTextBox.Text = actorClass.getUserContactPath(selCtcTpe - 1);
                }
                else
                {
                    // Adding a new contact: 
                    // - Clean ActorContactTypeComboBox and add possible types
                    // - Clean SelContactTypeTextBox and set it editable.
                    ActorContactTypeComboBox.Items.Clear();
                    ActorContactTypeComboBox.Items.Add("Select...");
                    ActorContactTypeComboBox.Items.Add("Phone");
                    ActorContactTypeComboBox.Items.Add("Email");
                    ActorContactTypeComboBox.Items.Add("Website");
                    ActorContactTypeComboBox.Items.Add("Facebook");
                    ActorContactTypeComboBox.Items.Add("Twitter");
                    ActorContactTypeComboBox.Items.Add("LinkedIn");
                    ActorContactTypeComboBox.Items.Add("Instagram");
                    ActorContactTypeComboBox.Items.Add("Snapchat");
                    SelContactTypeTextBox.Text = "";
                    SelContactTypeTextBox.Enabled = true;
                }
            }
            else
            {
                setInformationText("User validation needed", informationType.INFO, sender, e);
            }
        }
        private void SelContactTypeTextBox_TextChanged(object sender, EventArgs e)
        {
            AddContactButton.Enabled = true;
        }
        private void AddContactButton_Click(object sender, EventArgs e)
        {
            if (linwin.validated)
            {
                actorClass.addUserContact(ActorContactTypeComboBox.SelectedItem.ToString(), SelContactTypeTextBox.Text);

                ActorContactTypeComboBox.Items.Clear();
                ActorContactTypeComboBox.Items.Add("Select...");
                for (int i = 0; i < actorClass.getNoOfUserContacts(); i++)
                    ActorContactTypeComboBox.Items.Add(actorClass.getUserContactType(i));
                ActorContactTypeComboBox.Items.Add("Add Item...");
                ActorContactTypeComboBox.SelectedIndex = 0;
                SelContactTypeTextBox.Text = "";
                SelContactTypeTextBox.Enabled = false;
                AddContactButton.Enabled = false;
            }
            else
            {
                setInformationText("User validation needed", informationType.INFO, sender, e);
            }
        }
        private void BirthStreetAddrTextBox_TextChanged(object sender, EventArgs e)
        {
            if (linwin.validated)
            {
                string workstr = BirthStreetAddrTextBox.Text;
                int dp = workstr.LastIndexOf(" ");
                if ((dp > 0) && (dp < workstr.Length - 1))
                {
                    // Have both streetname and number
                    actorClass.setUserBirthStreet(workstr.Substring(0, dp));
                    workstr = workstr.Substring(dp + 1, workstr.Length - dp - 1);
                    for (int i = 0; i <= workstr.Length; i++)
                    {
                        if (workstr.ToUpper().Contains("A") || workstr.ToUpper().Contains("B") || workstr.ToUpper().Contains("C") || 
                            workstr.ToUpper().Contains("D") || workstr.ToUpper().Contains("E") || workstr.ToUpper().Contains("F") || 
                            workstr.ToUpper().Contains("G") || workstr.ToUpper().Contains("H") || workstr.ToUpper().Contains("I") || 
                            workstr.ToUpper().Contains("J") || workstr.ToUpper().Contains("K") || workstr.ToUpper().Contains("L") || 
                            workstr.ToUpper().Contains("M") || workstr.ToUpper().Contains("N") || workstr.ToUpper().Contains("O") || 
                            workstr.ToUpper().Contains("P") || workstr.ToUpper().Contains("Q") || workstr.ToUpper().Contains("R") || 
                            workstr.ToUpper().Contains("S") || workstr.ToUpper().Contains("T") || workstr.ToUpper().Contains("U") || 
                            workstr.ToUpper().Contains("V") || workstr.ToUpper().Contains("W") || workstr.ToUpper().Contains("X") || 
                            workstr.ToUpper().Contains("Y") || workstr.ToUpper().Contains("Z"))
                        {
                            actorClass.setUserBirthStreetNumber(workstr.Substring(0, workstr.Length - 1));
                            actorClass.setUserBirthStreetNumberAddon(workstr.Substring(workstr.Length - 1, 1));
                        }
                        else
                            actorClass.setUserBirthStreetNumber(workstr);
                    }
                }
                else
                {
                    // Have only streetname
                    actorClass.setUserBirthStreet(workstr);
                }
            }
            else
            {
                setInformationText("User validation needed", informationType.INFO, sender, e);
            }
        }
        private void BirthZipCodeTextBox_TextChanged(object sender, EventArgs e)
        {
            if (linwin.validated)
            {
                actorClass.setUserBirthZipcode(BirthZipCodeTextBox.Text);
            }
            else
            {
                setInformationText("User validation needed", informationType.INFO, sender, e);
            }
        }
        private void BirthAreaNameTextBox_TextChanged(object sender, EventArgs e)
        {
            if (linwin.validated)
            {
                actorClass.setUserBirthAreaname(BirthAreaNameTextBox.Text);
            }
            else
            {
                setInformationText("User validation needed", informationType.INFO, sender, e);
            }
        }
        private void BirthCitynameTextBox_TextChanged(object sender, EventArgs e)
        {
            if (linwin.validated)
            {
                actorClass.setUserBirthCityname(BirthCitynameTextBox.Text);
            }
            else
            {
                setInformationText("User validation needed", informationType.INFO, sender, e);
            }
        }
        private void BirthCountryTextBox_TextChanged(object sender, EventArgs e)
        {
            if (linwin.validated)
            {
                actorClass.setUserBirthCountry(BirthCountryTextBox.Text);
            }
            else
            {
                setInformationText("User validation needed", informationType.INFO, sender, e);
            }
        }
        private void BirthDateTextBox_TextChanged(object sender, EventArgs e)
        {
            if (linwin.validated)
            {
                actorClass.setUserBirthDate(BirthDateTextBox.Text);
            }
            else
            {
                setInformationText("User validation needed", informationType.INFO, sender, e);
            }
        }
        private void SocSecNumberTextBox_TextChanged(object sender, EventArgs e)
        {
            if (linwin.validated)
            {
                actorClass.setUserBirthSocNo(SocSecNumberTextBox.Text);
            }
            else
            {
                setInformationText("User validation needed", informationType.INFO, sender, e);
            }
        }
        private void BirthLatitudeTextBox_TextChanged(object sender, EventArgs e)
        {
            if (linwin.validated)
            {
                actorClass.setUserBirthLatitude(BirthLatitudeTextBox.Text);
            }
            else
            {
                setInformationText("User validation needed", informationType.INFO, sender, e);
            }
        }
        private void BirthLongitudeTextBox_TextChanged(object sender, EventArgs e)
        {
            if (linwin.validated)
            {
                actorClass.setUserBirthLongitude(BirthLongitudeTextBox.Text);
            }
            else
            {
                setInformationText("User validation needed", informationType.INFO, sender, e);
            }
        }
        private void ViewGeoPosButton_Click(object sender, EventArgs e)
        {
            if (linwin.validated)
            {
                // TODO - Handle this information.
                setInformationText("View geographical position is not implemented.", informationType.INFO, sender, e);
            }
            else
            {
                setInformationText("User validation needed", informationType.INFO, sender, e);
            }
        }
        private void AddSkinToneButton_Click(object sender, EventArgs e)
        {
            if (linwin.validated)
            {
                actorClass.setUserSkinToneTag(SkinToneTagTextBox.Text);
            }
            else
            {
                setInformationText("User validation needed", informationType.INFO, sender, e);
            }
        }
        private void SkinToneTagTextBox_TextChanged(object sender, EventArgs e)
        {
            if (linwin.validated)
            {
                AddSkinToneButton.Enabled = true;
            }
            else
            {
                setInformationText("User validation needed", informationType.INFO, sender, e);
            }
        }
        private void SkinToneDateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            if (linwin.validated)
            {
                DateTime sdt = SkinToneDateTimePicker.Value;
                // TODO - Handle this information.
            }
            else
            {
                setInformationText("User validation needed", informationType.INFO, sender, e);
            }
        }
        private void AddEyeColorButton_Click(object sender, EventArgs e)
        {
            if (linwin.validated)
            {
                // TODO - Handle this information.
            }
            else
            {
                setInformationText("User validation needed", informationType.INFO, sender, e);
            }
        }
        private void SelEyeColorTextBox_TextChanged(object sender, EventArgs e)
        {
            if (linwin.validated)
            {
                // TODO - Handle this information.
            }
            else
            {
                setInformationText("User validation needed", informationType.INFO, sender, e);
            }
        }
        private void EyeColorValidDateComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (linwin.validated)
            {
                // TODO - Handle this information.
            }
            else
            {
                setInformationText("User validation needed", informationType.INFO, sender, e);
            }
        }
        private void AddGenderInfoButton_Click(object sender, EventArgs e)
        {
            if (linwin.validated)
            {
                // TODO - Handle this information.
            }
            else
            {
                setInformationText("User validation needed", informationType.INFO, sender, e);
            }
        }
        private void GenderTypeTextBox_TextChanged(object sender, EventArgs e)
        {
            if (linwin.validated)
            {
                // TODO - Handle this information.
            }
            else
            {
                setInformationText("User validation needed", informationType.INFO, sender, e);
            }
        }
        private void GenderInfoValidDateComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (linwin.validated)
            {
                // TODO - Handle this information.
            }
            else
            {
                setInformationText("User validation needed", informationType.INFO, sender, e);
            }
        }
        private void GdrLengthTextBox_TextChanged(object sender, EventArgs e)
        {
            if (linwin.validated)
            {
                // TODO - Handle this information.
            }
            else
            {
                setInformationText("User validation needed", informationType.INFO, sender, e);
            }
        }
        private void GdrCircumfTextBox_TextChanged(object sender, EventArgs e)
        {
            if (linwin.validated)
            {
                // TODO - Handle this information.
            }
            else
            {
                setInformationText("User validation needed", informationType.INFO, sender, e);
            }
        }
        private void GdrLookTypeTextBox_TextChanged(object sender, EventArgs e)
        {
            if (linwin.validated)
            {
                // TODO - Handle this information.
            }
            else
            {
                setInformationText("User validation needed", informationType.INFO, sender, e);
            }
        }
        private void GdrBehaveTypeTextBox_TextChanged(object sender, EventArgs e)
        {
            if (linwin.validated)
            {
                // TODO - Handle this information.
            }
            else
            {
                setInformationText("User validation needed", informationType.INFO, sender, e);
            }
        }
        private void GdrPresentTextBox_TextChanged(object sender, EventArgs e)
        {
            if (linwin.validated)
            {
                // TODO - Handle this information.
            }
            else
            {
                setInformationText("User validation needed", informationType.INFO, sender, e);
            }
        }
        private void AddLengthInfoButton_Click(object sender, EventArgs e)
        {
            if (linwin.validated)
            {
                // TODO - Handle this information.
            }
            else
            {
                setInformationText("User validation needed", informationType.INFO, sender, e);
            }
        }
        private void LengthTextBox_TextChanged(object sender, EventArgs e)
        {
            if (linwin.validated)
            {
                // TODO - Handle this information.
            }
            else
            {
                setInformationText("User validation needed", informationType.INFO, sender, e);
            }
        }
        private void LengthValidDateComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (linwin.validated)
            {
                // TODO - Handle this information.
            }
            else
            {
                setInformationText("User validation needed", informationType.INFO, sender, e);
            }
        }
        private void AddWeightInfoButton_Click(object sender, EventArgs e)
        {
            if (linwin.validated)
            {
                // TODO - Handle this information.
            }
            else
            {
                setInformationText("User validation needed", informationType.INFO, sender, e);
            }
        }
        private void WeightTextBox_TextChanged(object sender, EventArgs e)
        {
            if (linwin.validated)
            {
                // TODO - Handle this information.
            }
            else
            {
                setInformationText("User validation needed", informationType.INFO, sender, e);
            }
        }
        private void WeightValidDateComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (linwin.validated)
            {
                // TODO - Handle this information.
            }
            else
            {
                setInformationText("User validation needed", informationType.INFO, sender, e);
            }
        }
        private void AddChestInfoButton_Click(object sender, EventArgs e)
        {
            if (linwin.validated)
            {
                // TODO - Handle this information.
            }
            else
            {
                setInformationText("User validation needed", informationType.INFO, sender, e);
            }
        }
        private void ChestTypeTextBox_TextChanged(object sender, EventArgs e)
        {
            if (linwin.validated)
            {
                // TODO - Handle this information.
            }
            else
            {
                setInformationText("User validation needed", informationType.INFO, sender, e);
            }
        }
        private void ChestValidDateComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (linwin.validated)
            {
                // TODO - Handle this information.
            }
            else
            {
                setInformationText("User validation needed", informationType.INFO, sender, e);
            }
        }
        private void ChestCircTextBox_TextChanged(object sender, EventArgs e)
        {
            if (linwin.validated)
            {
                // TODO - Handle this information.
            }
            else
            {
                setInformationText("User validation needed", informationType.INFO, sender, e);
            }
        }
        private void ChestSizeTextBox_TextChanged(object sender, EventArgs e)
        {
            if (linwin.validated)
            {
                // TODO - Handle this information.
            }
            else
            {
                setInformationText("User validation needed", informationType.INFO, sender, e);
            }
        }
        private void AddHairInfoButton_Click(object sender, EventArgs e)
        {
            if (linwin.validated)
            {
                // TODO - Handle this information.
            }
            else
            {
                setInformationText("User validation needed", informationType.INFO, sender, e);
            }
        }
        private void HairColorTextBox_TextChanged(object sender, EventArgs e)
        {
            if (linwin.validated)
            {
                // TODO - Handle this information.
            }
            else
            {
                setInformationText("User validation needed", informationType.INFO, sender, e);
            }
        }
        private void HairDataValidDateComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (linwin.validated)
            {
                // TODO - Handle this information.
            }
            else
            {
                setInformationText("User validation needed", informationType.INFO, sender, e);
            }
        }
        private void HairTextureTypeTextBox_TextChanged(object sender, EventArgs e)
        {
            if (linwin.validated)
            {
                // TODO - Handle this information.
            }
            else
            {
                setInformationText("User validation needed", informationType.INFO, sender, e);
            }
        }
        private void HairLengthTextBox_TextChanged(object sender, EventArgs e)
        {
            if (linwin.validated)
            {
                // TODO - Handle this information.
            }
            else
            {
                setInformationText("User validation needed", informationType.INFO, sender, e);
            }
        }
        private void MarkingTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (linwin.validated)
            {
                // TODO - Handle this information.
            }
            else
            {
                setInformationText("User validation needed", informationType.INFO, sender, e);
            }
        }
        private void MarkingPosTextBox_TextChanged(object sender, EventArgs e)
        {
            if (linwin.validated)
            {
                // TODO - Handle this information.
            }
            else
            {
                setInformationText("User validation needed", informationType.INFO, sender, e);
            }
        }
        private void AddMarkingDataButton_Click(object sender, EventArgs e)
        {
            if (linwin.validated)
            {
                // TODO - Handle this information.
            }
            else
            {
                setInformationText("User validation needed", informationType.INFO, sender, e);
            }
        }
        private void MarkingMotifTextBox_TextChanged(object sender, EventArgs e)
        {
            // TODO - Handle this information.
        }
        private void MarkingsValidDateComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (linwin.validated)
            {
                // TODO - Handle this information.
            }
            else
            {
                setInformationText("User validation needed", informationType.INFO, sender, e);
            }
        }
        private void OccupTitleTextBox_TextChanged(object sender, EventArgs e)
        {
            if (linwin.validated)
            {
                // TODO - Handle this information.
            }
            else
            {
                setInformationText("User validation needed", informationType.INFO, sender, e);
            }
        }
        private void ActorOccupationStartDateComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (linwin.validated)
            {
                // TODO - Handle this information.
            }
            else
            {
                setInformationText("User validation needed", informationType.INFO, sender, e);
            }
        }
        private void ActorOccupationCompanyTextBox_TextChanged(object sender, EventArgs e)
        {
            if (linwin.validated)
            {
                // TODO - Handle this information.
            }
            else
            {
                setInformationText("User validation needed", informationType.INFO, sender, e);
            }
        }
        private void ActorEmployAddressTextBox_TextChanged(object sender, EventArgs e)
        {
            if (linwin.validated)
            {
                // TODO - Handle this information.
            }
            else
            {
                setInformationText("User validation needed", informationType.INFO, sender, e);
            }
        }
        private void ActorEmployZipCodeTextBox_TextChanged(object sender, EventArgs e)
        {
            if (linwin.validated)
            {
                // TODO - Handle this information.
            }
            else
            {
                setInformationText("User validation needed", informationType.INFO, sender, e);
            }
        }
        private void ActorEmpoyAreanameTextBox_TextChanged(object sender, EventArgs e)
        {
            if (linwin.validated)
            {
                // TODO - Handle this information.
            }
            else
            {
                setInformationText("User validation needed", informationType.INFO, sender, e);
            }
        }
        private void ActorEmployCitynameTextBox_TextChanged(object sender, EventArgs e)
        {
            if (linwin.validated)
            {
                // TODO - Handle this information.
            }
            else
            {
                setInformationText("User validation needed", informationType.INFO, sender, e);
            }
        }
        private void ActorEmployCountryTextBox_TextChanged(object sender, EventArgs e)
        {
            if (linwin.validated)
            {
                // TODO - Handle this information.
            }
            else
            {
                setInformationText("User validation needed", informationType.INFO, sender, e);
            }
        }
        private void AddOccupationDataButton_Click(object sender, EventArgs e)
        {
            if (linwin.validated)
            {
                // TODO - Handle this information.
            }
            else
            {
                setInformationText("User validation needed", informationType.INFO, sender, e);
            }
        }
        private void ActorOccupationEndDaeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (linwin.validated)
            {
                // TODO - Handle this information.
            }
            else
            {
                setInformationText("User validation needed", informationType.INFO, sender, e);
            }
        }
        private void ResidAddressTextBox_TextChanged(object sender, EventArgs e)
        {
            if (linwin.validated)
            {
                // TODO - Handle this information.
            }
            else
            {
                setInformationText("User validation needed", informationType.INFO, sender, e);
            }
        }
        private void ResidZipCodeTextBox_TextChanged(object sender, EventArgs e)
        {
            if (linwin.validated)
            {
                // TODO - Handle this information.
            }
            else
            {
                setInformationText("User validation needed", informationType.INFO, sender, e);
            }
        }
        private void ResidAreanameTextBox_TextChanged(object sender, EventArgs e)
        {
            if (linwin.validated)
            {
                // TODO - Handle this information.
            }
            else
            {
                setInformationText("User validation needed", informationType.INFO, sender, e);
            }
        }
        private void ResidCitynameTextBox_TextChanged(object sender, EventArgs e)
        {
            if (linwin.validated)
            {
                // TODO - Handle this information.
            }
            else
            {
                setInformationText("User validation needed", informationType.INFO, sender, e);
            }
        }
        private void ResidCountryTextBox_TextChanged(object sender, EventArgs e)
        {
            if (linwin.validated)
            {
                // TODO - Handle this information.
            }
            else
            {
                setInformationText("User validation needed", informationType.INFO, sender, e);
            }
        }
        private void ResidStartDateComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (linwin.validated)
            {
                // TODO - Handle this information.
            }
            else
            {
                setInformationText("User validation needed", informationType.INFO, sender, e);
            }
        }
        private void AddResidenceButton_Click(object sender, EventArgs e)
        {
            if (linwin.validated)
            {
                // TODO - Handle this information.
            }
            else
            {
                setInformationText("User validation needed", informationType.INFO, sender, e);
            }
        }
        private void ResidEndDateComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (linwin.validated)
            {
                // TODO - Handle this information.
            }
            else
            {
                setInformationText("User validation needed", informationType.INFO, sender, e);
            }
        }
        private void EventIdTextBox_TextChanged(object sender, EventArgs e)
        {
            if (linwin.validated)
            {
                // TODO - Handle this information.
            }
            else
            {
                setInformationText("User validation needed", informationType.INFO, sender, e);
            }
        }
        private void AttdEventsDateComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (linwin.validated)
            {
                // TODO - Handle this information.
            }
            else
            {
                setInformationText("User validation needed", informationType.INFO, sender, e);
            }
        }
        private void ActorRelImageComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (linwin.validated)
            {
                // TODO - Handle this information.
            }
            else
            {
                setInformationText("User validation needed", informationType.INFO, sender, e);
            }
        }
        private void ViewSelActorRelImageButton_Click(object sender, EventArgs e)
        {
            if (linwin.validated)
            {
                // TODO - Handle this information.
            }
            else
            {
                setInformationText("User validation needed", informationType.INFO, sender, e);
            }
        }
        // --- Event information handling ---
        private void eventViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (linwin.validated)
            {
                setInformationText("Method \"eventViewToolStripMenuItem_Click\" entered.", informationType.INFO, sender, e);
                this.currentMode = programMode.EVENTVIEW;
//                setInformationText("Entered event data view mode.", informationType.INFO, sender, e);
                this.pictureCanvas.Visible = false;
                this.listView.Visible = true;
                this.tabControl.Visible = true;
                this.tabControl.SelectedTab = this.tabControl.TabPages["EventData"];
                this.noOfRegisteredEvents = 0;
                eventFilePaths = System.IO.Directory.GetFiles(rootPath + "\\EventData\\", "EventData_*.edf");
                foreach (var efp in eventFilePaths)
                {
                    int dp0 = efp.LastIndexOf("\\");
                    this.activeEventComboBox.Items.Add(efp.Substring(dp0 + 1, efp.Length - dp0 - 1));
                    this.noOfRegisteredEvents++;
                }
                this.activeEventComboBox.Items.Add("Add event...");
            }
            else
            {
                setInformationText("User validation needed", informationType.INFO, sender, e);
            }
        }
        private void activeEventComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (linwin.validated)
            {
                // TODO - Rewrite this.
                string valtEvent = activeEventComboBox.SelectedItem.ToString();
                if (this.currentMode == programMode.EVENTVIEW)
                {
                    if (activeEventComboBox == null) return;

                    // maxNoOfLoadedEvents, numberOfLoadedEvents, arrayOfEvents, eventClass
                    this.pictureCanvas.Visible = false;
//                    this.tabControl.Visible = true;
                    eventClass = new PEEventClass();
                    for (int i = 0; i < linwin.noOfEventCategories; i++)
                        eventClass.addEventCategory(linwin.getEvtCatTag(i), linwin.getEvtCatDescr(i), linwin.getEvtCatLevel(i));
                    for (int i = 0; i < linwin.noOfContentCategories; i++)
                        eventClass.addContentCategory(linwin.getContCatTag(i), linwin.getContCatDescr(i), linwin.getContCatLevel(i));
                    for (int i = 0; i < linwin.getNoOfRoles(); i++)
                        eventClass.addRoleCategory(linwin.getRoleTag(i), linwin.getRoleDescr(i), linwin.getRoleLevel(i));

                    string selitem = (string)activeEventComboBox.SelectedItem;
                    string storpa = linwin.getEventStoragePath();
                    if (((storpa != null) && (selitem != null)) && (System.IO.Directory.Exists(storpa)) && (System.IO.File.Exists(storpa + "\\EventData_" + selitem + ".edf")))
                        eventClass.loadEvent(selitem, storpa);
                    else if ((System.IO.Directory.Exists("C:\\Users\\" + currUser + "\\" + sProgPath + "\\EventData")) &&
                             (System.IO.File.Exists("C:\\Users\\" + currUser + "\\" + sProgPath + "\\EventData\\EventData_" + selitem + ".edf")))
                        eventClass.loadEvent((string)activeEventComboBox.SelectedItem, "C:\\Users\\" + currUser + "\\" + sProgPath + "\\EventData");
                    else
                        setInformationText("Event directory or file does not exist.", informationType.ERROR, sender, e);

                    int eventToShow = 0;
                    if (numberOfLoadedEvents < maxNoOfLoadedEvents)
                    {
                        arrayOfEvents[numberOfLoadedEvents] = eventClass;
                        eventToShow = numberOfLoadedEvents;
                        numberOfLoadedEvents++;
                    }
                    else
                    {
                        arrayOfEvents[0].saveEvent(activeEventComboBox.SelectedItem.ToString(), storpa); // TODO - fishy
                        for (int i = 0; i < numberOfLoadedEvents; i++)
                            arrayOfEvents[i] = arrayOfEvents[i + 1];
                        arrayOfEvents[numberOfLoadedEvents] = eventClass;
                        eventToShow = numberOfLoadedEvents;
                    }
                    CopyrightTextBox.Text = arrayOfEvents[eventToShow].getEventOwner();
                    EventSecrecyLevelTextBox.Text = arrayOfEvents[eventToShow].getEventLevel();
                    EventStartTextBox.Text = arrayOfEvents[eventToShow].getEventStarted();
                    EventEndTextBox.Text = arrayOfEvents[eventToShow].getEventEnded();
                    EventHeadlineTextBox.Text = arrayOfEvents[eventToShow].getEventHeadline();
                    EventLatitudeTextBox.Text = arrayOfEvents[eventToShow].getEventLatPos();
                    EventLongitudeTextBox.Text = arrayOfEvents[eventToShow].getEventLonPos();
                    EventGeographNameTextBox.Text = arrayOfEvents[eventToShow].getEventGeoPosName();
                    EventStreetnameNumberTextBox.Text = arrayOfEvents[eventToShow].getEventStreetname();
                    EventZipCodeTextBox.Text = arrayOfEvents[eventToShow].getEventAreacode();
                    EventAreanameTextBox.Text = arrayOfEvents[eventToShow].getEventAreaname();
                    EventCitynameTextBox.Text = arrayOfEvents[eventToShow].getEventCityname();
                    EventCountrynameTextBox.Text = arrayOfEvents[eventToShow].getEventCountryname();
                    EventAttenderIDComboBox1.Items.Clear();
                    EventAttenderIDComboBox1.Items.Add("Select...");
                    for (int i = 0; i < arrayOfEvents[eventToShow].getNoOfEventAttender(); i++)
                    {
                        if (arrayOfEvents[eventToShow].getEventAttenderRoleLevel(i) <= linwin.getUserRightsValue())
                            EventAttenderIDComboBox1.Items.Add(arrayOfEvents[eventToShow].getEventAttenderID(i));
                    }
                    EventAttenderIDComboBox1.Items.Add("Add Item...");
                    EventAttenderIDComboBox1.Visible = true;
                    EventAttenderIDComboBox1.Enabled = true;
                    EventAttenderNaneTextBox.Text = "";
                    EventAttenderIDComboBox1.SelectedIndex = 0;
                    EventImageNameComboBox.Items.Clear();
                    EventImageNameComboBox.Items.Add("Select...");
                    for (int i = 0; i < arrayOfEvents[eventToShow].getNoOfEventImages(); i++)
                    {
                        if (arrayOfEvents[eventToShow].getEventImageContentLevel(i) <= linwin.getUserRightsValue())
                        {
                            string tempImNam = arrayOfEvents[eventToShow].getEventImageName(i);
                            int dptim = tempImNam.LastIndexOf("\\");
                            if ((dptim > 0) && (dptim < tempImNam.Length - 1))
                                EventImageNameComboBox.Items.Add(tempImNam.Substring(dptim + 1, tempImNam.Length - dptim - 1));
                            else
                                EventImageNameComboBox.Items.Add(tempImNam);
                            // TODO - add each image as thumbnail in the listView.
                            /*
                                foreach (var exiFile in fileArray)
                                {
                                    string tempStrLong = exiFile.ToString();
                                    int dp1 = tempStrLong.LastIndexOf("\\");
                                    string tempStrShort = tempStrLong.Substring(dp1 + 1, tempStrLong.Length - dp1 - 1);
                                    // Add each icon to list view
                                    try
                                    {
                                        imageList.Images.Add(Image.FromFile(tempStrLong));
                                        imageList.Images.SetKeyName(nr, tempStrLong);
                                        arrayItemIndex[nr] = tempStrLong;
                                    }
                                    catch
                                    {
                                        setInformationText("\"" + tempStrShort + "\" is not an image file.", informationType.INFO, sender, e);
                                        imageList.Images.Add(Image.FromFile(rootPath + "\\Resources\\DefaultIcon.jpg"));
                                        imageList.Images.SetKeyName(nr, tempStrLong);
                                        arrayItemIndex[nr] = tempStrLong;
                                    }
                                    nr++;
                                }
                             */
//                            try
//                            {
//                                string strimpan = arrayOfEvents[eventToShow].getEventImagePathName(i).ToString();
//                                imageList.Images.Add(strimpan); // Needs to be a System.Drawing.Icon
//                            }
                        }
                    }
                    EventImageNameComboBox.Items.Add("Add Item...");
                    EventImageNameComboBox.Visible = true;
                    EventImageNameComboBox.Enabled = true;
                    if ((EventImageNameComboBox.Items.Count == 3) && (arrayOfEvents[eventToShow].getEventImageContentLevel(0) <= linwin.getUserRightsValue()))
                    {
                        EventImageLevelTextBox.Text = arrayOfEvents[eventToShow].getEventImageContentDescription(0);
                        EventImageNameComboBox.SelectedIndex = 1;
                    }
                    else
                    {
                        EventImageLevelTextBox.Text = "";
                        EventImageNameComboBox.SelectedIndex = 0;
                    }
                }
                else if (this.currentMode == programMode.IMAGEVIEW)
                {
                    setInformationText("activeEventComboBox changed in program mode IMAGEVIEW, not handled: " + activeEventComboBox.SelectedItem.ToString(), informationType.INFO, sender, e);
                    // TODO - set all data from the event.
                }
                else
                    setInformationText("Wrong mode for this action!", informationType.ERROR, sender, e);
            }
            else
            {
                setInformationText("User validation needed", informationType.INFO, sender, e);
            }
        }
        private void CopyrightTextBox_TextChanged(object sender, EventArgs e)
        {
//            activeEventComboBox.Items.Clear();
//            activeEventComboBox_SelectedIndexChanged(sender, e);
            if ((CopyrightTextBox.Text != "") && (this.currentMode == programMode.EVENTVIEW))
                eventClass.setEventOwner(CopyrightTextBox.Text); // This goes wrong when opening a image with event data...
        }
        private void EventSecrecyLevelTextBox_TextChanged(object sender, EventArgs e)
        {
            eventClass.setEventLevel(EventSecrecyLevelTextBox.Text);
        }
        private void EventStartTextBox_TextChanged(object sender, EventArgs e)
        {
            if (linwin.validated)
            {
                // TODO - Handle this information.
            }
            else
            {
                setInformationText("User validation needed", informationType.INFO, sender, e);
            }
        }
        private void EventEndTextBox_TextChanged(object sender, EventArgs e)
        {
            if (linwin.validated)
            {
                // TODO - Handle this information.
            }
            else
            {
                setInformationText("User validation needed", informationType.INFO, sender, e);
            }
        }
        private void EventHeadlineTextBox_TextChanged(object sender, EventArgs e)
        {
            if (linwin.validated)
            {
                // TODO - Handle this information.
            }
            else
            {
                setInformationText("User validation needed", informationType.INFO, sender, e);
            }
        }
        private void EventLatitudeTextBox_TextChanged(object sender, EventArgs e)
        {
            if (linwin.validated)
            {
                // TODO - Handle this information.
            }
            else
            {
                setInformationText("User validation needed", informationType.INFO, sender, e);
            }
        }
        private void EventLongitudeTextBox_TextChanged(object sender, EventArgs e)
        {
            if (linwin.validated)
            {
                // TODO - Handle this information.
            }
            else
            {
                setInformationText("User validation needed", informationType.INFO, sender, e);
            }
        }
        private void EventGeographNameTextBox_TextChanged(object sender, EventArgs e)
        {
            if (linwin.validated)
            {
                // TODO - Handle this information.
            }
            else
            {
                setInformationText("User validation needed", informationType.INFO, sender, e);
            }
        }
        private void EventStreetnameNumberTextBox_TextChanged(object sender, EventArgs e)
        {
            if (linwin.validated)
            {
                // TODO - Handle this information.
            }
            else
            {
                setInformationText("User validation needed", informationType.INFO, sender, e);
            }
        }
        private void EventZipCodeTextBox_TextChanged(object sender, EventArgs e)
        {
            if (linwin.validated)
            {
                // TODO - Handle this information.
            }
            else
            {
                setInformationText("User validation needed", informationType.INFO, sender, e);
            }
        }
        private void EventAreanameTextBox_TextChanged(object sender, EventArgs e)
        {
            if (linwin.validated)
            {
                // TODO - Handle this information.
            }
            else
            {
                setInformationText("User validation needed", informationType.INFO, sender, e);
            }
        }
        private void EventCitynameTextBox_TextChanged(object sender, EventArgs e)
        {
            if (linwin.validated)
            {
                // TODO - Handle this information.
            }
            else
            {
                setInformationText("User validation needed", informationType.INFO, sender, e);
            }
        }
        private void EventCountrynameTextBox_TextChanged(object sender, EventArgs e)
        {
            if (linwin.validated)
            {
                // TODO - Handle this information.
            }
            else
            {
                setInformationText("User validation needed", informationType.INFO, sender, e);
            }
        }
        private void EventAttenderNaneTextBox_TextChanged(object sender, EventArgs e)
        {
            if (linwin.validated)
            {
                // TODO - Handle this information.
            }
            else
            {
                setInformationText("User validation needed", informationType.INFO, sender, e);
            }
        }
        private void EventAttenderIDComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selval = EventAttenderIDComboBox1.SelectedIndex - 1;
            if (selval < eventClass.getNoOfEventAttender())
            {
                if (linwin.getUserRightsValue() >= eventClass.getEventAttenderRoleLevel(selval))
                {
                    EventAttenderNaneTextBox.Text = eventClass.getEventAttenderRoleDescription(selval);
                }
            }
            else
            {
                // TODO - Handle adding a new event attender.
            }
        }
        private void ViewAttenderAsActorButton_Click(object sender, EventArgs e)
        {
            if (linwin.validated)
            {
                // TODO - Handle this information.
            }
            else
            {
                setInformationText("User validation needed", informationType.INFO, sender, e);
            }
        }
        private void EventImageLevelTextBox_TextChanged(object sender, EventArgs e)
        {
            if (linwin.validated)
            {
                // TODO - Handle this information.
            }
            else
            {
                setInformationText("User validation needed", informationType.INFO, sender, e);
            }
        }
        private void EventImageNameComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (linwin.validated)
            {
                // TODO - Handle this information.
            }
            else
            {
                setInformationText("User validation needed", informationType.INFO, sender, e);
            }
        }
        private void ViewSelEventImageButton_Click(object sender, EventArgs e)
        {
            if (linwin.validated)
            {
                // TODO - Handle this information.
            }
            else
            {
                setInformationText("User validation needed", informationType.INFO, sender, e);
            }
        }
        private void EventDescriptionTextBox_TextChanged(object sender, EventArgs e)
        {
            if (linwin.validated)
            {
                // TODO - Handle this information.
            }
            else
            {
                setInformationText("User validation needed", informationType.INFO, sender, e);
            }
        }
        private void SaveEventDataChangesButton_Click(object sender, EventArgs e)
        {
            if (linwin.validated)
            {
                // TODO - Handle this information.
            }
            else
            {
                setInformationText("User validation needed", informationType.INFO, sender, e);
            }
        }
        private void DiscardEventDataChangesButton_Click(object sender, EventArgs e)
        {
            if (linwin.validated)
            {
                // TODO - Handle this information.
            }
            else
            {
                setInformationText("User validation needed", informationType.INFO, sender, e);
            }
        }
        // --- Help handling ---
        private void folderBrowserDialog_HelpRequest(object sender, EventArgs e)
        {
            if (linwin.validated)
            {
                // TODO - Handle this information.
            }
            else
            {
                setInformationText("User validation needed", informationType.INFO, sender, e);
            }
        }
        // --- Sorting handling ---
        private void fixImageDisplayAndSortingButtons(string inBaseFolder, object sender, EventArgs e)
        {
            if ((inBaseFolder != "") && (System.IO.Directory.Exists(inBaseFolder)))
            {
                pictureName.Text = inBaseFolder;
                for (int i = 0; i < maxSortButtons; i++)
                {
                    sortButtons[i].Enabled = false;
                    sortButtons[i].Text = "";
                    sortButtons[i].Visible = false;
                }
                noOfSortingButtons = 0;
                string[] directoryArray = System.IO.Directory.GetDirectories(inBaseFolder, "*");
                if (directoryArray != null)
                {
                    foreach (var exiDir in directoryArray)
                    {
                        string longCurrDirName = exiDir.ToString();
                        if ((noOfSortingButtons < maxSortButtons) && (longCurrDirName != ""))
                        {
                            int dp = longCurrDirName.LastIndexOf("\\");
                            string shortCurrDirName = longCurrDirName.Substring(dp + 1, longCurrDirName.Length - 1 - dp);
                            sortButtons[noOfSortingButtons].Text = shortCurrDirName;
                            sortButtons[noOfSortingButtons].Enabled = true;
                            sortButtons[noOfSortingButtons].Visible = true;
                            noOfSortingButtons++;
                        }
                    }
                }
                if (noOfSortingButtons < maxSortButtons)
                {
                    sortButtons[noOfSortingButtons].Text = "Add directory...";
                    sortButtons[noOfSortingButtons].Enabled = true;
                    sortButtons[noOfSortingButtons].Visible = true;
                }
                imageList.Images.Clear();
                listView.Items.Clear();
                string[] fileArray = System.IO.Directory.GetFiles(inBaseFolder);
                if (fileArray != null)
                {
                    int nr = 0;
                    foreach (var exiFile in fileArray)
                    {
                        string longHandledFile = exiFile.ToString();
                        int dp = longHandledFile.LastIndexOf("\\");
                        string shortHandledFile = longHandledFile.Substring(dp + 1, longHandledFile.Length - 1 - dp);
                        ListViewItem lwItm = new ListViewItem();
                        try
                        {
                            imageList.Images.Add(Image.FromFile(longHandledFile));
                            imageList.Images.SetKeyName(nr, longHandledFile);
                        }
                        catch
                        {
                            try
                            {
                                imageList.Images.Add(Image.FromFile(rootPath + "\\Resources\\DefaultIcon.jpg"));
                                imageList.Images.SetKeyName(nr, longHandledFile);
                            }
                            catch { }
                        }
                        lwItm.ImageIndex = nr;
                        lwItm.Tag = longHandledFile;
                        lwItm.Text = longHandledFile;
                        listView.Items.Add(lwItm);
//                        lwItm.Remove();
                        nr++;
                    }
                }
                listView.View = View.LargeIcon;
                imageList.ImageSize = new Size(Math.Max(linwin.getSmallImageWidth(), 32), Math.Max(linwin.getSmallImageHeight(), 32));
                listView.LargeImageList = imageList;
                listView.Visible = true;
            }
        }
        private void imageSortingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (linwin.validated)
            {
                this.currentMode = programMode.SORTINGVIEW;
                setInformationText("Entered image sorting mode.", informationType.INFO, sender, e);
                // Directory sorting mode, image table and sorting buttons for subdirectories
                this.pictureCanvas.Visible = false;
                this.tabControl.Visible = true;
                tabControl.SelectedTab = tabControl.TabPages["sortingTabPage"];
                folderBrowserDialog.RootFolder = Environment.SpecialFolder.MyComputer;
                DialogResult dres = folderBrowserDialog.ShowDialog();
                baseFolderName = folderBrowserDialog.SelectedPath.ToString();
                this.UseWaitCursor = true;
                fixImageDisplayAndSortingButtons(baseFolderName, sender, e);
                this.UseWaitCursor = false;
                folderBrowserDialog.Dispose();
                // TODO - set buttons allocated to number keys that are related to sub-directories.
            }
            else
            {
                setInformationText("User validation needed", informationType.INFO, sender, e);
            }
        }
        private void listView_Click(object sender, EventArgs e)
        {
            ListView.SelectedListViewItemCollection selItm = listView.SelectedItems;//.GetEnumerator();
            System.Collections.IEnumerator selEnum = selItm.GetEnumerator();
            ListView.SelectedListViewItemCollection sic = listView.SelectedItems;
            ListViewItem slvi = listView.FocusedItem;
            string selFile = slvi.Text;
            loadedImageFileName = selFile;
            if ((this.currentMode == programMode.RESTOREVIEW) || (this.currentMode == programMode.SORTINGVIEW))
            {
                if (expandedImage)
                {
                    pictureCanvas.Visible = false;
                    listView.Visible = true;
                    RetToListBtn.Enabled = false;
                    RetToListBtn.Visible = false;
                    expandedImage = false;
                }
                else
                {
                    // Display the selected image.
                    int dpp = selFile.LastIndexOf("\\");
                    linwin.setLastImageDirectoryValue(selFile.Substring(0, dpp));
                    picture = Image.FromFile(selFile);
                    pictureCanvas.SizeMode = PictureBoxSizeMode.Zoom;
                    getImageMetadataValues(selFile, sender, e);
                    pictureCanvas.Image = picture;
                    pictureName.Text = selFile;
                    listView.Visible = false;
                    pictureCanvas.Visible = true;
                    RetToListBtn.Enabled = true;
                    RetToListBtn.Visible = true;
                    expandedImage = true;
                }
            }
            setInformationText("User selected item " + selFile, informationType.INFO, sender, e);
        }
        private void pictureCanvas_Click(object sender, EventArgs e)
        {
            if (this.currentMode == programMode.RESTOREVIEW)
            {
                if (expandedImage)
                {
                    pictureCanvas.Visible = false;
                    listView.Visible = true;
                    RetToListBtn.Enabled = false;
                    RetToListBtn.Visible = false;
                    expandedImage = false;
                }
            }
        }
        private void listView_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void RetToListBtn_Click(object sender, EventArgs e)
        {
            pictureCanvas.Visible = false;
            listView.Visible = true;
            RetToListBtn.Enabled = false;
            RetToListBtn.Visible = false;
        }
        private bool addDirAndFixButtons(int buttonNo)
        {
            if (linwin.validated)
            {
                if (buttonNo <= maxSortButtons)
                {
                    addDirForm newDirForm = new addDirForm();
                    newDirForm.ShowDialog(this);
                    DirectoryInfo dififo = System.IO.Directory.CreateDirectory(baseFolderName + "\\" + newDirForm.newDirName);
                    sortButtons[buttonNo - 1].Text = dififo.Name;
                    sortButtons[buttonNo].Text = "Add directory...";
                    sortButtons[buttonNo].Enabled = true;
                    sortButtons[buttonNo].Visible = true;
                    newDirForm.MyDispose();
                    return true;
                }
                else
                    return false;
            }
            else
                return false;
        }
        private bool moveFileToDir(string buttonText, object sender, EventArgs e)
        {
            // TODO - Fix this FUCKING method!!!
            bool retVal = false;
            setInformationText("Sorting method not implemented yet.", informationType.INFO, sender, e);
            this.UseWaitCursor = true;
            ListViewItem selViewItem = listView.FocusedItem;
            string selectedFile = selViewItem.Text;
            string selectedFileShort = "";
            int dp = selectedFile.LastIndexOf("\\");
            if ((dp > 0) && (dp < selectedFile.Length))
                selectedFileShort = selectedFile.Substring(dp + 1, selectedFile.Length - dp - 1);
            else
            {
                selectedFileShort = selectedFile;
                selectedFile = baseFolderName + "\\" + selectedFileShort;
            }
            string targetName = baseFolderName + "\\" + buttonText + "\\" + selectedFileShort;
            pictureCanvas.Visible = false;
            pictureCanvas.Image.Dispose();
            expandedImage = false;
            for (int i = 0; i < imageList.Images.Count; i++)
            {
                try
                {
                    imageList.Images[i].Dispose();
                }
                catch (Exception err)
                {
                    setInformationText("Disposing image resources failed: " + err.ToString(), informationType.ERROR, sender, e);
                }
            }
            try
            {
                imageList.Images.Clear();
            }
            catch (Exception err)
            {
                setInformationText("Clearing \"imageList\" failed: " + err.ToString(), informationType.ERROR, sender, e);
            }
            listView.Visible = false;
            for (int i = 0; i < listView.Items.Count; i++)
            {
                try
                {
                    listView.Items[i].Remove();
                }
                catch (Exception err)
                {
                    setInformationText("Clearing \"listView\" items failed: " + err.ToString(), informationType.ERROR, sender, e);
                }
            }
            try
            {
                listView.Items.Clear();
            }
            catch (Exception err)
            {
                setInformationText("Claring \"listView\" failed: " + err.ToString(), informationType.ERROR, sender, e);
            }
            for (int i = 0; i < maxSortButtons; i++)
            {
                sortButtons[i].Text = "Sort button " + i.ToString();
                sortButtons[i].Enabled = false;
                sortButtons[i].Visible = false;
            }
            // Now try to move the @#&§@ file.
            try
            {
                File.Move(selectedFile, targetName);
                retVal = true;
                setInformationText("Moved file " + selectedFileShort, informationType.INFO, sender, e);
            }
            catch (Exception err)
            {
                setInformationText("Moving file " + selectedFileShort + "; failed: " + err.ToString(), informationType.ERROR, sender, e);
            }
            this.UseWaitCursor = false;
            /* --------------------------------------------------------------
//            FileSecurity fsSelFile = File.GetAccessControl(selFile);
//            fsSelFile.SetOwner(new NTAccount("NT AUTHORITY\\SYSTEM"));
            try
            {
                File.Move(selFile, baseFolderName + "\\" + buttonText + "\\" + selFileShort);
            }
            catch (Exception e1)
            {
                setInformationText(e1.ToString(), informationType.ERROR, sender, e);
            }
                /* -------------------------------------------
    //      try
    //      {
    //          File.Copy(selFile, baseFolderName + "\\" + buttonText + "\\" + selFileShort);
    //          // TODO - vad är "@"DomainName\AccountName"" för något?
    //          AddFileSecurity(selFile, @"DomainName\AccountName", FileSystemRights.FullControl, AccessControlType.Allow);
    //          File.Delete(selFile);
    //      }
    //      catch (Exception err)
    //      {
    //          setInformationText(err.ToString(), informationType.ERROR, sender, e);
    //          retVal = false;
    //      }
                }
                fixImageDisplayAndSortingButtons(baseFolderName, sender, e);
            this.UseWaitCursor = false;
             -------------------------------------------------- */
            fixImageDisplayAndSortingButtons(baseFolderName, sender, e);
            return retVal;
        }
        private void sortingButton1_Click(object sender, EventArgs e)
        {
            if (linwin.validated)
            {
                if (sortingButton1.Text == "Add directory...")
                {
                    if (!(addDirAndFixButtons(1)))
                        setInformationText("Failed to add directory.", informationType.ERROR, sender, e);
                }
                else
                {
                    moveFileToDir(sortingButton1.Text, sender, e);
                }
            }
            else
            {
                setInformationText("User validation needed", informationType.INFO, sender, e);
            }
        }
        private void sortingButton2_Click(object sender, EventArgs e)
        {
            if (linwin.validated)
            {
                if (sortingButton2.Text == "Add directory...")
                {
                    if (!(addDirAndFixButtons(2)))
                        setInformationText("Failed to add directory.", informationType.ERROR, sender, e);
                }
                else
                {
                    moveFileToDir(sortingButton2.Text, sender, e);
                }
            }
            else
            {
                setInformationText("User validation needed", informationType.INFO, sender, e);
            }
        }
        private void sortingButton3_Click(object sender, EventArgs e)
        {
            if (linwin.validated)
            {
                if (sortingButton3.Text == "Add directory...")
                {
                    if (!(addDirAndFixButtons(3)))
                        setInformationText("Failed to add directory.", informationType.ERROR, sender, e);
                }
                else
                {
                    moveFileToDir(sortingButton3.Text, sender, e);
                }
            }
            else
            {
                setInformationText("User validation needed", informationType.INFO, sender, e);
            }
        }
        private void sortingButton4_Click(object sender, EventArgs e)
        {
            if (linwin.validated)
            {
                if (sortingButton4.Text == "Add directory...")
                {
                    if (!(addDirAndFixButtons(4)))
                        setInformationText("Failed to add directory.", informationType.ERROR, sender, e);
                }
                else
                {
                    moveFileToDir(sortingButton4.Text, sender, e);
                }
            }
            else
            {
                setInformationText("User validation needed", informationType.INFO, sender, e);
            }
        }
        private void sortingButton5_Click(object sender, EventArgs e)
        {
            if (linwin.validated)
            {
                if (sortingButton5.Text == "Add directory...")
                {
                    if (!(addDirAndFixButtons(5)))
                        setInformationText("Failed to add directory.", informationType.ERROR, sender, e);
                }
                else
                {
                    moveFileToDir(sortingButton5.Text, sender, e);
                }
            }
            else
            {
                setInformationText("User validation needed", informationType.INFO, sender, e);
            }
        }
        private void sortingButton6_Click(object sender, EventArgs e)
        {
            if (linwin.validated)
            {
                if (sortingButton6.Text == "Add directory...")
                {
                    if (!(addDirAndFixButtons(6)))
                        setInformationText("Failed to add directory.", informationType.ERROR, sender, e);
                }
                else
                {
                    moveFileToDir(sortingButton6.Text, sender, e);
                }
            }
            else
            {
                setInformationText("User validation needed", informationType.INFO, sender, e);
            }
        }
        private void sortingButton7_Click(object sender, EventArgs e)
        {
            if (linwin.validated)
            {
                if (sortingButton7.Text == "Add directory...")
                {
                    if (!(addDirAndFixButtons(7)))
                        setInformationText("Failed to add directory.", informationType.ERROR, sender, e);
                }
                else
                {
                    moveFileToDir(sortingButton7.Text, sender, e);
                }
            }
            else
            {
                setInformationText("User validation needed", informationType.INFO, sender, e);
            }
        }
        private void sortingButton8_Click(object sender, EventArgs e)
        {
            if (linwin.validated)
            {
                if (sortingButton8.Text == "Add directory...")
                {
                    if (!(addDirAndFixButtons(8)))
                        setInformationText("Failed to add directory.", informationType.ERROR, sender, e);
                }
                else
                {
                    moveFileToDir(sortingButton8.Text, sender, e);
                }
            }
            else
            {
                setInformationText("User validation needed", informationType.INFO, sender, e);
            }
        }
        private void sortingButton9_Click(object sender, EventArgs e)
        {
            if (linwin.validated)
            {
                if (sortingButton9.Text == "Add directory...")
                {
                    if (!(addDirAndFixButtons(9)))
                        setInformationText("Failed to add directory.", informationType.ERROR, sender, e);
                }
                else
                {
                    moveFileToDir(sortingButton9.Text, sender, e);
                }
            }
            else
            {
                setInformationText("User validation needed", informationType.INFO, sender, e);
            }
        }
        private void sortingButton10_Click(object sender, EventArgs e)
        {
            if (linwin.validated)
            {
                if (sortingButton10.Text == "Add directory...")
                {
                    if (!(addDirAndFixButtons(10)))
                        setInformationText("Failed to add directory.", informationType.ERROR, sender, e);
                }
                else
                {
                    moveFileToDir(sortingButton10.Text, sender, e);
                }
            }
            else
            {
                setInformationText("User validation needed", informationType.INFO, sender, e);
            }
        }
        private void sortingButton11_Click(object sender, EventArgs e)
        {
            if (linwin.validated)
            {
                if (sortingButton11.Text == "Add directory...")
                {
                    if (!(addDirAndFixButtons(11)))
                        setInformationText("Failed to add directory.", informationType.ERROR, sender, e);
                }
                else
                {
                    moveFileToDir(sortingButton11.Text, sender, e);
                }
            }
            else
            {
                setInformationText("User validation needed", informationType.INFO, sender, e);
            }
        }
        private void sortingButton12_Click(object sender, EventArgs e)
        {
            if (linwin.validated)
            {
                if (sortingButton12.Text == "Add directory...")
                {
                    if (!(addDirAndFixButtons(12)))
                        setInformationText("Failed to add directory.", informationType.ERROR, sender, e);
                }
                else
                {
                    moveFileToDir(sortingButton12.Text, sender, e);
                }
            }
            else
            {
                setInformationText("User validation needed", informationType.INFO, sender, e);
            }
        }
        private void sortingButton13_Click(object sender, EventArgs e)
        {
            if (linwin.validated)
            {
                if (sortingButton13.Text == "Add directory...")
                {
                    if (!(addDirAndFixButtons(13)))
                        setInformationText("Failed to add directory.", informationType.ERROR, sender, e);
                }
                else
                {
                    moveFileToDir(sortingButton13.Text, sender, e);
                }
            }
            else
            {
                setInformationText("User validation needed", informationType.INFO, sender, e);
            }
        }
        private void sortingButton14_Click(object sender, EventArgs e)
        {
            if (linwin.validated)
            {
                if (sortingButton14.Text == "Add directory...")
                {
                    if (!(addDirAndFixButtons(14)))
                        setInformationText("Failed to add directory.", informationType.ERROR, sender, e);
                }
                else
                {
                    moveFileToDir(sortingButton14.Text, sender, e);
                }
            }
            else
            {
                setInformationText("User validation needed", informationType.INFO, sender, e);
            }
        }
        private void sortingButton15_Click(object sender, EventArgs e)
        {
            if (linwin.validated)
            {
                if (sortingButton15.Text == "Add directory...")
                {
                    if (!(addDirAndFixButtons(15)))
                        setInformationText("Failed to add directory.", informationType.ERROR, sender, e);
                }
                else
                {
                    moveFileToDir(sortingButton15.Text, sender, e);
                }
            }
            else
            {
                setInformationText("User validation needed", informationType.INFO, sender, e);
            }
        }
        private void sortingButton16_Click(object sender, EventArgs e)
        {
            if (linwin.validated)
            {
                if (sortingButton16.Text == "Add directory...")
                {
                    if (!(addDirAndFixButtons(16)))
                        setInformationText("Failed to add directory.", informationType.ERROR, sender, e);
                }
                else
                {
                    moveFileToDir(sortingButton16.Text, sender, e);
                }
            }
            else
            {
                setInformationText("User validation needed", informationType.INFO, sender, e);
            }
        }
        private void sortingButton17_Click(object sender, EventArgs e)
        {
            if (linwin.validated)
            {
                if (sortingButton17.Text == "Add directory...")
                {
                    if (!(addDirAndFixButtons(17)))
                        setInformationText("Failed to add directory.", informationType.ERROR, sender, e);
                }
                else
                {
                    moveFileToDir(sortingButton17.Text, sender, e);
                }
            }
            else
            {
                setInformationText("User validation needed", informationType.INFO, sender, e);
            }
        }
        private void sortingButton18_Click(object sender, EventArgs e)
        {
            if (linwin.validated)
            {
                if (sortingButton18.Text == "Add directory...")
                {
                    if (!(addDirAndFixButtons(18)))
                        setInformationText("Failed to add directory.", informationType.ERROR, sender, e);
                }
                else
                {
                    moveFileToDir(sortingButton18.Text, sender, e);
                }
            }
            else
            {
                setInformationText("User validation needed", informationType.INFO, sender, e);
            }
        }
        private void sortingButton19_Click(object sender, EventArgs e)
        {
            if (linwin.validated)
            {
                if (sortingButton19.Text == "Add directory...")
                {
                    if (!(addDirAndFixButtons(19)))
                        setInformationText("Failed to add directory.", informationType.ERROR, sender, e);
                }
                else
                {
                    moveFileToDir(sortingButton19.Text, sender, e);
                }
            }
            else
            {
                setInformationText("User validation needed", informationType.INFO, sender, e);
            }
        }
        private void sortingButton20_Click(object sender, EventArgs e)
        {
            if (linwin.validated)
            {
                if (sortingButton20.Text == "Add directory...")
                {
                    if (!(addDirAndFixButtons(20)))
                        setInformationText("Failed to add directory.", informationType.ERROR, sender, e);
                }
                else
                {
                    moveFileToDir(sortingButton20.Text, sender, e);
                }
            }
            else
            {
                setInformationText("User validation needed", informationType.INFO, sender, e);
            }
        }
        private void sortingButton21_Click(object sender, EventArgs e)
        {
            if (linwin.validated)
            {
                if (sortingButton21.Text == "Add directory...")
                {
                    if (!(addDirAndFixButtons(21)))
                        setInformationText("Failed to add directory.", informationType.ERROR, sender, e);
                }
                else
                {
                    moveFileToDir(sortingButton21.Text, sender, e);
                }
            }
            else
            {
                setInformationText("User validation needed", informationType.INFO, sender, e);
            }
        }
        private void sortingButton22_Click(object sender, EventArgs e)
        {
            if (linwin.validated)
            {
                if (sortingButton22.Text == "Add directory...")
                {
                    if (!(addDirAndFixButtons(22)))
                        setInformationText("Failed to add directory.", informationType.ERROR, sender, e);
                }
                else
                {
                    moveFileToDir(sortingButton22.Text, sender, e);
                }
            }
            else
            {
                setInformationText("User validation needed", informationType.INFO, sender, e);
            }
        }
        // --- User information handling ---
        private void userViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (linwin.validated)
            {
                this.currentMode = programMode.USERVIEW;
                setInformationText("Entered user view mode.", informationType.INFO, sender, e);
                this.pictureCanvas.Visible = false;
                this.listView.Visible = true;
            }
            else
            {
                setInformationText("User validation needed", informationType.INFO, sender, e);
                Login_Win_Open();
            }
        }
        private void Login_Win_Open()//(object sender, EventArgs e)
        {
            if (linwin == null)
            {
                linwin = new LoginForm();
            }
            else if (linwin.IsDisposed)
            {
                linwin = new LoginForm();
            }
            linwin.Show();
//            linwin.BringToFront();
        }
    }
}
