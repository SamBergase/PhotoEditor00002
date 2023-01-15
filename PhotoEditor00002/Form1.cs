using MetadataExtractor;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.Serialization;
using System.IO;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Globalization;

using myLoginForm;
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

        const int maxNoOfLoadedActors = 16;
        int numberOfUsedActors = 0;
        ActorClass[] arrayOfActors = new ActorClass[maxNoOfLoadedActors];
        ActorClass actorClass;

        private string loadedImageFileName;
        private bool artistListBoxActive = false;
        private string currUser = WindowsIdentity.GetCurrent().Name;
        private string sProgPath = "source\\repos\\PhotoEditor00002\\PhotoEditor00002";
        private string sLogPath = "source\\repos\\PhotoEditor00002\\PhotoEditor00002\\Logs\\" + DateTime.Now.ToString("yyyy-MM-dd-hhmmss") + ".log";
        private string rootPath = "";
        private string logPath = "";
        private string smplRootPath = "";
        FileStream fsLog;

        private Button[] sortButtons;
        private int maxSortButtons = 22, noOfSortingButtons = 0;
        private String baseFolderName;
        private String[] arrayItemIndex = new string[2000];
        //        private String[] sortBtnText = new string[] { };

        private LoginForm linwin;

        public Form1()
        {
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
            Login_Win_Open();
            linwin.BringToFront();
        }
        // --- Support functions ---
        private void setInformationText(string inString, informationType inInfoType, object sender, EventArgs e)
        {
            //            if (!(File.Exists(logPath)))
            //                File.Create(logPath);

            //            if (File.Exists(logPath))
            //            {
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
            //            }
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
            }
            setInformationText("Closing the program", informationType.INFO, sender, e);
            //try { LoginForm.Close}
            try { Dispose(); } catch { }
            try { Close(); } catch { }
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

                OpenFileDialog ofd = new OpenFileDialog();

                ofd.InitialDirectory = "c:\\";
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

                    picture = Image.FromFile(ofd.FileName);
                    //                if ((picture.Width > pictureCanvas.Width) || (picture.Height > pictureCanvas.Height))
                    pictureCanvas.SizeMode = PictureBoxSizeMode.Zoom;//AutoSize
                                                                     //                else
                                                                     //                    pictureCanvas.SizeMode = PictureBoxSizeMode.Normal;

                    getImageMetadataValues(ofd.FileName, sender, e);

                    pictureCanvas.Image = picture;//new Bitmap(ofd.FileName);
                    pictureName.Text = ofd.FileName;
                    startUpDone = true;
                }
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
            ChangeDateTimeTextBox.Text = "";
            noOfComponentsTextBox.Text = "";
            detFileTypeName = "";
        }
        private void getImageMetadataValues(string instr, object sender, EventArgs e)
        {
            if (linwin.validated)
            {
                cleanImageGUIMetaDataValues();
                var directories = ImageMetadataReader.ReadMetadata(instr);

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
                                }
                                break;
                            case "Exposure Program":
                                {
                                    // Property Value = 0x8822
                                    ExpProgTextBox.Text = tag.Description;
                                }
                                break;
                            case "F-Number":
                                {
                                    // Property Value = 0x829d
                                    FNumTextBox.Text = tag.Description;
                                }
                                break;
                            case "Exposure Time":
                                {
                                    // Property Value = 0x829a
                                    ExpTimeTextBox.Text = tag.Description;
                                }
                                break;
                            case "Copyright":
                                {
                                    // Property Value = 0x503b
                                    CopyrightTextBox.Text = tag.Description;
                                }
                                break;
                            case "YCbCr Positioning":
                                {
                                    // Property Value = 0x0213
                                    YCbCrPosTextBox.Text = tag.Description;
                                }
                                break;
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
                                }
                                break;
                            case "Date/Time":
                                {
                                    // Property Value = 0x0132
                                    ImageDateTimeTextBox.Text = tag.Description;
                                }
                                break;
                            case "Compression Type":
                                {
                                    // Property Value = 0x0103
                                    CompTypeTextBox.Text = tag.Description;
                                }
                                break;
                            case "Data Precision":
                                {
                                    // Property Value = ?
                                    DataPrecisionTextBox.Text = tag.Description;
                                }
                                break;
                            case "Image Height":
                                {
                                    // Property Value = 0x0101
                                    ImageHeightTextBox.Text = tag.Description;
                                }
                                break;
                            case "Image Width":
                                {
                                    // Property Value = 0x0100
                                    ImageWidthTextBox.Text = tag.Description;
                                }
                                break;
                            case "Make":
                                {
                                    // Property Value = 271 = 0x010f
                                    HWMakeTextBox.Text = tag.Description;
                                }
                                break;
                            case "Model":
                                {
                                    // Property Value = 272 = 0x0110
                                    HWModelTextBox.Text = tag.Description;
                                }
                                break;
                            case "Orientation":
                                {
                                    // Property Value = 0x0112
                                    ImageOrientationTextBox.Text = tag.Description;
                                }
                                break;
                            case "X Resolution":
                                {
                                    // Property Value = 0x011a
                                    ImageXResTextBox.Text = tag.Description;
                                }
                                break;
                            case "Y Resolution":
                                {
                                    // Property Value = 0x011b
                                    ImageYResTextBox.Text = tag.Description;
                                }
                                break;
                            case "Sensitivity Type":
                                {
                                    // Property Value = 
                                    SensitivityTypeTextBox.Text = tag.Description;
                                }
                                break;
                            case "Recommended Exposure Index":
                                {
                                    // Property Value = 0xa215
                                    RekExpIndexTextBox.Text = tag.Description;
                                }
                                break;
                            case "Exif Version":
                                {
                                    if (ExifVersionTextBox.Text == "")
                                        ExifVersionTextBox.Text = tag.Description;
                                }
                                break;
                            case "Version":
                                {
                                    if (ExifVersionTextBox.Text == "")
                                        ExifVersionTextBox.Text = tag.Description;
                                }
                                break;
                            case "Date/Time Original":
                                {
                                    // Property Value = 0x9003
                                    OriginalDateTimeTextBox.Text = tag.Description;
                                }
                                break;
                            case "Date/Time Digitized":
                                {
                                    // Property Value = 0x9292
                                    DigitizedDateTimeTextBox.Text = tag.Description;
                                }
                                break;
                            case "Components Configuration":
                                {
                                    // Property Value = 0x9101
                                    ComponentConfigTextBox.Text = tag.Description;
                                }
                                break;
                            case "Shutter Speed Value":
                                {
                                    // Property Value = 0x9201
                                    ShutterSpeedTextBox.Text = tag.Description;
                                }
                                break;
                            case "Aperture Data":
                                {
                                    // Property Value = 0x9202
                                    ApertureTextBox.Text = tag.Description;
                                }
                                break;
                            case "Exposure Bias Value":
                                {
                                    // Property Value = 0x9204
                                    ExpBiasValueTextBox.Text = tag.Description;
                                }
                                break;
                            case "Metering Mode":
                                {
                                    // Property Value = 0x9207
                                    MeteringModeTextBox.Text = tag.Description;
                                }
                                break;
                            case "Flash":
                                {
                                    // Property Value = 0x9209
                                    FlashTextBox.Text = tag.Description;
                                }
                                break;
                            case "Focal Length":
                                {
                                    // Property Value = 0x920a
                                    FocalLengthTextBox.Text = tag.Description;
                                }
                                break;
                            case "User Comment":
                                {
                                    if (UserCommentTextBox.Text != "")
                                        UserCommentTextBox.Text = UserCommentTextBox.Text.ToString() + Environment.NewLine + tag.Description;
                                    else
                                        UserCommentTextBox.Text = tag.Description;
                                }
                                break;
                            case "Sub-Sec Time":
                                {
                                    // Property Value = 
                                    SubSecTimeTextBox.Text = tag.Description;
                                }
                                break;
                            case "Sub-Sec Time Original":
                                {
                                    OrigSubSecTimeTextBox.Text = tag.Description;
                                }
                                break;
                            case "Sub-Sec Time Digitized":
                                {
                                    DigSubSecTimeTextBox.Text = tag.Description;
                                }
                                break;
                            case "FlashPix Version":
                                {
                                    FlashPixVersionTextBox.Text = tag.Description;
                                }
                                break;
                            case "Color Space":
                                {
                                    ColorSpaceTextBox.Text = tag.Description;
                                }
                                break;
                            case "Exif Image Width":
                                {
                                    ExifImageWidthTextBox.Text = tag.Description;
                                }
                                break;
                            case "Exif Image Height":
                                {
                                    ExifImageHeightTextBox.Text = tag.Description;
                                }
                                break;
                            case "Focal Plane X Resolution":
                                {
                                    FocalPlaneXResTextBox.Text = tag.Description;
                                }
                                break;
                            case "Focal Plane Y Resolution":
                                {
                                    FocalPlaneYResTextBox.Text = tag.Description;
                                }
                                break;
                            case "Custom Rendered":
                                {
                                    CustomRenderedTextBox.Text = tag.Description;
                                }
                                break;
                            case "Exposure Mode":
                                {
                                    ExpModeTextBox.Text = tag.Description;
                                }
                                break;
                            case "White Balance Mode":
                                {
                                    WhiteBalanceModeTextBox.Text = tag.Description;
                                }
                                break;
                            case "Scene Capture Type":
                                {
                                    SceneCaptureTextBox.Text = tag.Description;
                                }
                                break;
                            case "Camera Owner Name":
                                {
                                    OwnerTextBox.Text = tag.Description;
                                }
                                break;
                            case "Body Serial Number":
                                {
                                    SerialNumberTextBox.Text = tag.Description;
                                }
                                break;
                            case "Lens Specification":
                                {
                                    LensSpecTextBox.Text = tag.Description;
                                }
                                break;
                            case "Lens Model":
                                {
                                    LensModelTextBox.Text = tag.Description;
                                }
                                break;
                            case "Lens Serial Number":
                                {
                                    LensSerNoTextBox.Text = tag.Description;
                                }
                                break;
                            case "Macro Mode":
                                {
                                    MacroModeTextBox.Text = tag.Description;
                                }
                                break;
                            case "Self Timer Delay":
                                {
                                    SelfTimerDelayTextBox.Text = tag.Description;
                                }
                                break;
                            case "Quality":
                                {
                                    QualityTextBox.Text = tag.Description;
                                }
                                break;
                            case "Flash Mode":
                                {
                                    FlashModeTextBox.Text = tag.Description;
                                }
                                break;
                            case "Continuous Drive Mode":
                                {
                                    ContinousDriveTextBox.Text = tag.Description;
                                }
                                break;
                            case "Focus Mode":
                                {
                                    FocusModeTextBox.Text = tag.Description;
                                }
                                break;
                            case "Record Mode":
                                {
                                    RecordModeTextBox.Text = tag.Description;
                                }
                                break;
                            case "Image Size":
                                {
                                    if (ImageSizeTextBox.Text != "")
                                        ImageSizeTextBox.Text = ImageSizeTextBox.Text.ToString() + ", " + tag.Description;
                                    else
                                        ImageSizeTextBox.Text = tag.Description;
                                }
                                break;
                            case "File Size":
                                {
                                    if (ImageSizeTextBox.Text != "")
                                        ImageSizeTextBox.Text = ImageSizeTextBox.Text.ToString() + ", " + tag.Description;
                                    else
                                        ImageSizeTextBox.Text = tag.Description;
                                }
                                break;
                            case "Easy Shooting Mode":
                                {
                                    EasyShootingModeTextBox.Text = tag.Description;
                                }
                                break;
                            case "Digital Zoom":
                                {
                                    DigitalZoomTextBox.Text = tag.Description;
                                }
                                break;
                            case "Contrast":
                                {
                                    ContrastTextBox.Text = tag.Description;
                                }
                                break;
                            case "Saturation":
                                {
                                    SaturationTextBox.Text = tag.Description;
                                }
                                break;
                            case "Sharpness":
                                {
                                    SharpnessInfoTextBox.Text = tag.Description;
                                }
                                break;
                            case "Iso":
                                {
                                    ISODataTextBox.Text = tag.Description;
                                }
                                break;
                            case "Metering mode":
                                {
                                    FocusMeteringInfoTextBox.Text = tag.Description;
                                }
                                break;
                            case "Focus Type":
                                {
                                    FocusTypeInfoTextBox.Text = tag.Description;
                                }
                                break;
                            case "AF Point Selected":
                                {
                                    AFPointSelectedInfoTextBox.Text = tag.Description;
                                }
                                break;
                            case "Lens Type":
                                {
                                    LensTypeInfoTextBox.Text = tag.Description;
                                }
                                break;
                            case "Long Focal Length":
                                {
                                    FocalMaxLengthInfoTextBox.Text = tag.Description;
                                }
                                break;
                            case "Short Focal Length":
                                {
                                    FocalMinLengthInfoTextBox.Text = tag.Description;
                                }
                                break;
                            case "Focal Units per mm":
                                {
                                    FocalUnitInfoTextBox.Text = tag.Description;
                                }
                                break;
                            case "Max Aperture":
                                {
                                    MaxApertureTextBox.Text = tag.Description;
                                }
                                break;
                            case "Min Aperture":
                                {
                                    MinApertureTextBox.Text = tag.Description;
                                }
                                break;
                            case "Aperture Value":
                                {
                                    ApertureTextBox.Text = tag.Description;
                                }
                                break;
                            case "Flash Activity":
                                {
                                    FlashActivityInfoTextBox.Text = tag.Description;
                                }
                                break;
                            case "Flash Details":
                                {
                                    FlashDetailsTextBox.Text = tag.Description;
                                }
                                break;
                            case "Focus Continuous":
                                {
                                    ContinouosFocusTextBox.Text = tag.Description;
                                }
                                break;
                            case "AE Setting":
                                {
                                    AESettingsTextBox.Text = tag.Description;
                                }
                                break;
                            case "Display Aperture":
                                {
                                    DisplayApertureTextBox.Text = tag.Description;
                                }
                                break;
                            case "Zoom Source Width":
                                {
                                    ZoomSourceWidthTextBox.Text = tag.Description;
                                }
                                break;
                            case "Zoom Target Width":
                                {
                                    ZoomSourceHeightTextBox.Text = tag.Description;
                                }
                                break;
                            case "Spot Metering Mode":
                                {
                                    SpotMeteringModeTextBox.Text = tag.Description;
                                }
                                break;
                            case "Image Description":
                                {
                                    if (UserCommentTextBox.Text != "")
                                        UserCommentTextBox.Text = UserCommentTextBox.Text.ToString() + Environment.NewLine + tag.Description;
                                    else
                                        UserCommentTextBox.Text = tag.Description;
                                }
                                break;
                            case "GPS Latitude Ref":
                                {
                                    if (tag.Description == "N")
                                        LatRefTextBox.Text = "North";
                                    else if (tag.Description == "S")
                                        LatRefTextBox.Text = "South";
                                    else
                                        LatRefTextBox.Text = "?";
                                }
                                break;
                            case "GPS Latitude":
                                {
                                    LatValueTextBox.Text = tag.Description;
                                }
                                break;
                            case "GPS Longitude Ref":
                                {
                                    if (tag.Description == "E")
                                        LonRefTextBox.Text = "East";
                                    else if (tag.Description == "W")
                                        LonRefTextBox.Text = "West";
                                    else
                                        LonRefTextBox.Text = "?";
                                }
                                break;
                            case "GPS Longitude":
                                {
                                    LonValueTextBox.Text = tag.Description;
                                }
                                break;
                            case "Windows XP Comment":
                                {
                                    if (UserCommentTextBox.Text != "")
                                        UserCommentTextBox.Text = UserCommentTextBox.Text.ToString() + Environment.NewLine + tag.Description;
                                    else
                                        UserCommentTextBox.Text = tag.Description;
                                }
                                break;
                            case "Windows XP Subject":
                                {
                                    subjectTextBox.Text = tag.Description;
                                }
                                break;
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
                                            exiEvents = exiEvents.Substring(delimPos + 1, exiEvents.Length - delimPos - 1);
                                            delimPos = exiEvents.IndexOf(";");
                                            if ((delimPos > 0) && (delimPos < exiEvents.Length - 1))
                                                currEvent = exiEvents.Substring(0, delimPos);
                                            else
                                                currEvent = exiEvents;
                                        }
                                        activeEventComboBox.Items.Add(currEvent);
                                    }
                                    else
                                        activeEventComboBox.Items.Add(exiEvents);
                                }
                                break;
                            case "File Modified Date":
                                {
                                    ChangeDateTimeTextBox.Text = tag.Description;
                                }
                                break;
                            case "JPEG Comment":
                                {
                                    if (UserCommentTextBox.Text != "")
                                        UserCommentTextBox.Text = UserCommentTextBox.Text.ToString() + Environment.NewLine + tag.Description;
                                    else
                                        UserCommentTextBox.Text = tag.Description;
                                }
                                break;
                            case "Number of Tables":
                                {
                                    noOfComponentsTextBox.Text = tag.Description;
                                }
                                break;
                            case "Detected File Type Name":
                                {
                                    detFileTypeName = tag.Description;
                                }
                                break;
                            default:
                                {
                                    setInformationText($"[{directory.Name}] {tag.Name} = {tag.Description}, not displayed in GUI.", informationType.ERROR, sender, e);
                                }
                                break;
                        }
                    }
                }
                ActiveArtistsComboBox.Items.Add("Add actor...");
                ActiveArtistsComboBox.Visible = true;
                activeEventComboBox.Items.Add("Add event...");
                activeEventComboBox.Visible = true;
                // TODO - free directories variable.
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

                    //                File.Delete(loadedImageFileName);
                    //                picture.Save(loadedImageFileName, myImageCodecInfo, myEncoderParameters);
                    int dpDot = loadedImageFileName.LastIndexOf("\\");
                    string newILFileName = loadedImageFileName.Substring(0, dpDot) + "\\%" + loadedImageFileName.Substring(dpDot + 1, loadedImageFileName.Length - dpDot - 1);

                    picture.Save(newILFileName, myImageCodecInfo, myEncoderParameters);
                    saveToolStripMenuItem.Enabled = false;
                    picture.Dispose();
                    File.Delete(loadedImageFileName);
                    File.Move(newILFileName, loadedImageFileName);

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
                Stream myStream;
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
                    if ((myStream = sfd.OpenFile()) != null)
                    {
                        myStream.Close();
                        setInformationText("Saved file as " + sfd.FileName.ToString(), informationType.INFO, sender, e);
                    }
                    else
                        setInformationText("Failed to save file", informationType.ERROR, sender, e);
                }
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
        private void restoreToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (linwin.validated)
            {
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
                this.currentMode = programMode.RESTOREVIEW;
                setInformationText("Entered image restoring mode.", informationType.INFO, sender, e);
                this.pictureCanvas.Visible = false;
                this.listView.Visible = true;
                this.tabControl.Visible = false;
                // TODO - Design the restoring page.
            }
            else
            {
                setInformationText("User validation needed", informationType.INFO, sender, e);
            }
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
        // --- --- Base Hardware information handling --- ---
        private void OwnerTextBox_TextChanged(object sender, EventArgs e)
        {
            if (linwin.validated)
            {
                // TODO - Take care of owner data.
            }
            else
            {
                setInformationText("User validation needed", informationType.INFO, sender, e);
            }
        }
        private void SaveGenHWDataChangesButton_Click(object sender, EventArgs e)
        {
            if (linwin.validated)
            {
                // TODO - Save the changed owner data.
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
                // TODO - Discard the changes of owner data.
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
                // TODO - Save the changed geographical data.
                bool managedToSaveSomething = false;
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
                        catch
                        { }
                        try

                        {
                            PropertyItem currLatDir = picture.GetPropertyItem(2);
                            currLatDir.Value = bteNewLatDirection;
                            currLatDir.Len = bteNewLatDirection.Length;
                            picture.SetPropertyItem(currLatDir);
                        }
                        catch
                        {

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
                    this.HairColorTextBox.Enabled = true;   // TODO - ComboBox with HairColorType
                    this.HairDataValidDateComboBox.Items.Add(System.DateTime.Now.ToString("yyyy-MM-dd"));
                    this.HairDataValidDateComboBox.Items.Add("Other date");
                    this.HairDataValidDateComboBox.Enabled = true;
                    this.HairTextureTypeTextBox.Enabled = true; // TODO - ComboBox with HairTextureType
                    this.HairLengthTextBox.Enabled = true;      // TODO - ComboBox with HairLengthType
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
                    this.EventIdTextBox.Enabled = true; // TODO - this might be better as a ComboBox with existing events?
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
                    actorClass.loadActor((string)ActiveArtistsComboBox.SelectedItem);
                    arrayOfActors[numberOfUsedActors] = actorClass;
                    // --- User names ---
                    NameTypeComboBox.Items.Clear();
                    if (actorClass.getNoOfUserNames() > 0)
                    {
                        for (int i = 0; i < actorClass.getNoOfUserNames(); i++)
                        {
                            NameTypeComboBox.Items.Add(actorClass.getUserNameTag(i));
                        }
                        NameTypeComboBox.Items.Add("Add item...");
                        NameTypeComboBox.SelectedItem = 0;
                        SelNameTypeTextBox.Text = (actorClass.getUserSurName(0) + " " + actorClass.getUserMidName(0) + " " + actorClass.getUserFamName(0));
                    }
                    else
                        NameTypeComboBox.Items.Add("Add item...");
                    // --- User contacts ---
                    ActorContactTypeComboBox.Items.Clear();
                    if (actorClass.getNoOfUserContacts() > 0)
                    {
                        for (int i = 0; i < actorClass.getNoOfUserContacts(); i++)
                        {
                            ActorContactTypeComboBox.Items.Add(actorClass.getUserContactType(i));
                        }
                        ActorContactTypeComboBox.Items.Add("Add ítem...");
                        ActorContactTypeComboBox.SelectedItem = 0;
                        SelContactTypeTextBox.Text = actorClass.getUserContactPath(0);
                    }
                    else
                        ActorContactTypeComboBox.Items.Add("Add ítem...");
                    // --- Birth data ---
                    BirthStreetAddrTextBox.Text = (actorClass.getUserBirthStreet() + " " + actorClass.getUserBirthStreetNumber() + actorClass.getUserBirthStreetNumberAddon());
                    BirthZipCodeTextBox.Text = actorClass.getUserBirthZipcode();
                    BirthAreaNameTextBox.Text = actorClass.getUserBirthAreaname();
                    BirthCitynameTextBox.Text = actorClass.getUserBirthCityname();
                    BirthCountryTextBox.Text = actorClass.getUserBirthCountry();
                    BirthDateTextBox.Text = actorClass.getUserBirthDate();
                    SocSecNumberTextBox.Text = actorClass.getUserBirthSocNo();
                    // --- Skin-Tone ---
                    SkinToneDateTimePicker.Visible = false;
                    SkinToneValidDateComboBox.Items.Clear();
                    if (actorClass.getNumberOfSkinTones() > 0)
                    {
                        for (int i = 0; i < actorClass.getNumberOfSkinTones(); i++)
                            SkinToneValidDateComboBox.Items.Add(actorClass.getUserSkinToneValidDate(i));
                        SkinToneValidDateComboBox.Items.Add("Add item...");
                        SkinToneValidDateComboBox.SelectedItem = 0;
                        SkinToneTagTextBox.Text = actorClass.getUserSkinToneTag(0);
                    }
                    else
                        SkinToneValidDateComboBox.Items.Add("Add item...");
                    // --- Eyecolor           : <tag-tb><date-cb> ---
                    EyeColorValidDateComboBox.Items.Clear();
                    // TODO - fix eye color data
                    // --- Gender Information : <type-tb><date-cb> ---
                    GenderInfoValidDateComboBox.Items.Clear();
                    if (actorClass.getNumberOfGenderData() > 0)
                    {
                        for (int i = 0; i < actorClass.getNumberOfGenderData(); i++)
                            GenderInfoValidDateComboBox.Items.Add(actorClass.getUserGenderInfoValidDate(i));
                        GenderInfoValidDateComboBox.Items.Add("Add item...");
                        GenderTypeTextBox.Text = actorClass.getUserGenderType(0);
                        // ---                    : <Length unit-tb><circumference unit-tb> ---
                        GdrLengthTextBox.Text = actorClass.getUserGenderLength(0);
                        GdrCircumfTextBox.Text = actorClass.getUserGenderCircumf(0);
                        // ---                    : <look-tb><behave-tb> ---
                        GdrLookTypeTextBox.Text = actorClass.getUserGenderAppearance(0);
                        GdrBehaveTypeTextBox.Text = actorClass.getUserGenderBehaviour(0);
                        // ---                    : <presentation-tb> ---
                        GdrPresentTextBox.Text = actorClass.getUserGenderPres(0);
                    }
                    else
                        GenderInfoValidDateComboBox.Items.Add("Add item...");
                    // --- Length             : <value unit-tb><date-cb> ---
                    LengthValidDateComboBox.Items.Clear();
                    if (actorClass.getNumberOfLengthData() > 0)
                    {
                        for (int i = 0; i < actorClass.getNumberOfLengthData(); i++)
                            LengthValidDateComboBox.Items.Add(actorClass.getUserLengthInfoValidDate(i));
                        LengthValidDateComboBox.Items.Add("Add item...");
                        LengthTextBox.Text = actorClass.getUserLengthValue(0);
                    }
                    else
                        LengthValidDateComboBox.Items.Add("Add item...");
                    // --- Weight             : <value unit-tb><date-cb> ---
                    WeightValidDateComboBox.Items.Clear();
                    if (actorClass.getNumberOfWeightData() > 0)
                    {
                        for (int i = 0; i < actorClass.getNumberOfWeightData(); i++)
                            WeightValidDateComboBox.Items.Add(actorClass.getUserWeightValue(i));
                        WeightValidDateComboBox.Items.Add("Add item...");
                        WeightTextBox.Text = actorClass.getUserWeightValue(0);
                    }
                    else
                        WeightValidDateComboBox.Items.Add("Add item...");
                    // --- Chest data         : <type-tb><date-cb> ---
                    ChestValidDateComboBox.Items.Clear();
                    if (actorClass.getNumberOfChestData() > 0)
                    {
                        for (int i = 0; i < actorClass.getNumberOfChestData(); i++)
                            ChestValidDateComboBox.Items.Add(actorClass.getUserChestInfoValidDate(i));
                        ChestValidDateComboBox.Items.Add("Add item...");
                        ChestTypeTextBox.Text = actorClass.getUserChestType(0);
                        // ---                    : <circ-tb><size-tb> ---
                        ChestCircTextBox.Text = actorClass.getUserChestCircumfValue(0);
                        ChestSizeTextBox.Text = actorClass.getUserChestSizeType(0);
                    }
                    else
                        ChestValidDateComboBox.Items.Add("Add item...");
                    // --- Hair data          : <color-tb><date-cb> ---
                    HairDataValidDateComboBox.Items.Clear();
                    if (actorClass.getNumberOfHairData() > 0)
                    {
                        for (int i = 0; i < actorClass.getNumberOfHairData(); i++)
                            HairDataValidDateComboBox.Items.Add(actorClass.getUserHairValidDate(i));
                        HairDataValidDateComboBox.Items.Add("Add item...");
                        HairColorTextBox.Text = actorClass.getUserHairColor(0);
                        // ---                    : <texture-tb><length-tb> ---
                        HairTextureTypeTextBox.Text = actorClass.getUserHairTexture(0);
                        HairLengthTextBox.Text = actorClass.getUserHairLength(0);
                    }
                    else
                        HairDataValidDateComboBox.Items.Add("Add item...");
                    // --- Markings data      : <type-cb><pos-tb> ---
                    MarkingTypeComboBox.Items.Clear();
                    if (actorClass.getNoOfMarkingsData() > 0)
                    {
                        for (int i = 0; i < actorClass.getNoOfMarkingsData(); i++)
                            MarkingTypeComboBox.Items.Add(actorClass.getActorMarkingValidDate(i));
                        MarkingTypeComboBox.Items.Add("Add item...");
                        MarkingPosTextBox.Text = actorClass.getActorMarkingPlace(0);
                        // ---                    : <motif-tb><date-tb/cb> ---
                        MarkingMotifTextBox.Text = actorClass.getActorMarkingMotif(0);
                        MarkingDateTextBox.Text = actorClass.getActorMarkingValidDate(0);
                    }
                    else
                        MarkingTypeComboBox.Items.Add("Add item...");
                    // --- Occupation data    : <
                    // TODO - fetch the user data into correct GUI-parts.
                    numberOfUsedActors++;
                }
            }
            else
            {
                setInformationText("User validation needed", informationType.INFO, sender, e);
            }
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
                picture.Save(loadedImageFileName); // TODO - Ett allmänt fel från GDI+ om metadata för bilden har ändrats.
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
        // --- Event information handling ---
        private void eventViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (linwin.validated)
            {
                this.currentMode = programMode.EVENTVIEW;
                setInformationText("Entered event data view mode.", informationType.INFO, sender, e);
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
                string valtEvent = activeEventComboBox.SelectedItem.ToString();
                if (this.currentMode == programMode.EVENTVIEW)
                {
                    // TODO - set all data from the event and the items editeable.
                    // TODO - add each image as thumbnail in the listView.
                }
                else if (this.currentMode == programMode.IMAGEVIEW)
                {
                    // TODO - sett all data from the event.
                }
                else
                    this.setInformationText("Wrong mode for this action!", informationType.ERROR, sender, e);
            }
            else
            {
                setInformationText("User validation needed", informationType.INFO, sender, e);
            }
        }
        // --- Help handling ---
        private void folderBrowserDialog_HelpRequest(object sender, EventArgs e)
        {
            // TODO - add som e help  info.
        }
        // --- Sorting handling ---
        private void imageSortingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (linwin.validated)
            {
                setInformationText("Method \"sortingButton2_Click\" entered", informationType.INFO, sender, e);
                this.currentMode = programMode.SORTINGVIEW;
                setInformationText("Entered image sorting mode.", informationType.INFO, sender, e);
                // Directory sorting mode, image table and sorting buttons for subdirectories
                this.pictureCanvas.Visible = false;
                this.tabControl.Visible = true;
                tabControl.SelectedTab = tabControl.TabPages["sortingTabPage"];
                DialogResult dres = folderBrowserDialog.ShowDialog();
                baseFolderName = folderBrowserDialog.SelectedPath.ToString();
                pictureName.Text = baseFolderName;
                string[] directoryArray = System.IO.Directory.GetDirectories(baseFolderName, "*");
                noOfSortingButtons = 0;
                if (directoryArray != null)
                {
                    foreach (var exiDir in directoryArray)
                    {
                        string tempString = exiDir.ToString();
                        if ((noOfSortingButtons < maxSortButtons) && (tempString != ""))
                        {
                            int dp0 = tempString.LastIndexOf("\\");
                            string shortTempString = tempString.Substring(dp0 + 1, tempString.Length - dp0 - 1);
                            sortButtons[noOfSortingButtons].Text = shortTempString;
                            sortButtons[noOfSortingButtons].Enabled = true;
                            sortButtons[noOfSortingButtons].Visible = true;
                            // sortBtnText[noOfSortingButtons] = tempString;
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

                string[] fileArray = System.IO.Directory.GetFiles(baseFolderName);
                if (fileArray != null)
                {
                    int nr = 0;
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
                    listView.View = View.LargeIcon;
                    imageList.ImageSize = new Size(64, 64);
                    listView.LargeImageList = imageList;

                    for (int j = 0; j < imageList.Images.Count; j++)
                    {
                        ListViewItem item = new ListViewItem();
                        item.ImageIndex = j;
                        listView.Items.Add(item);
                    }
                    //                listView.Cursor.GetType = Cursor
                    listView.Visible = true;
                }
                // TODO - set buttons allocated to number keys that are related to sub-directories.
            }
            else
            {
                setInformationText("User validation needed", informationType.INFO, sender, e);
            }
        }
        private void sortingButton1_Click(object sender, EventArgs e)
        {
            if (linwin.validated)
            {
                setInformationText("Method \"sortingButton1_Click\" entered", informationType.INFO, sender, e);
                // First determine what ite is selected.
                int selItmIdx = listView.SelectedItems[0].ImageIndex;
                // Get the selected item name
                string selFileToMove = arrayItemIndex[selItmIdx];
                // Pick out the parts of that selected item name
                int dpDOT = selFileToMove.LastIndexOf("\\");
                string origFilePath = selFileToMove.Substring(0, dpDOT);
                string origFilename = selFileToMove.Substring(dpDOT + 1, arrayItemIndex[selItmIdx].Length - dpDOT - 1);
                string btn1RelDir = baseFolderName + "\\" + sortingButton1.Text;
                setInformationText(arrayItemIndex[selItmIdx] + " ---> " + btn1RelDir, informationType.INFO, sender, e);
                // Remove the item from listView
                listView.Items.Clear();
                imageList.Images.Clear();
                //            picture.Dispose();
                //            listView.Items.RemoveAt(selItmIdx);
                //            imageList.Images.RemoveAt(selItmIdx);

                // System.Security.AccessControl.FileSecurity fSecItem = File.GetAccessControl(arrayItemIndex[selItmIdx]);
                File.Move(selFileToMove, btn1RelDir + "\\" + origFilename);

                // File.Move(arrayItemIndex[selItmIdx], btn1RelDir + "\\.");
                //            for (int k = selItmIdx; k < 19; k++)
                //                arrayItemIndex[k] = arrayItemIndex[k + 1];
                //            arrayItemIndex[19] = "";
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
                setInformationText("Method \"sortingButton2_Click\" entered", informationType.INFO, sender, e);
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
                setInformationText("Method \"sortingButton3_Click\" entered", informationType.INFO, sender, e);
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
                setInformationText("Method \"sortingButton4_Click\" entered", informationType.INFO, sender, e);
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
                setInformationText("Method \"sortingButton5_Click\" entered", informationType.INFO, sender, e);
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
                setInformationText("Method \"sortingButton6_Click\" entered", informationType.INFO, sender, e);
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
                setInformationText("Method \"sortingButton7_Click\" entered", informationType.INFO, sender, e);
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
                setInformationText("Method \"sortingButton8_Click\" entered", informationType.INFO, sender, e);
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
                setInformationText("Method \"sortingButton9_Click\" entered", informationType.INFO, sender, e);
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
                setInformationText("Method \"sortingButton10_Click\" entered", informationType.INFO, sender, e);
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
                setInformationText("Method \"sortingButton11_Click\" entered", informationType.INFO, sender, e);
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
                setInformationText("Method \"sortingButton12_Click\" entered", informationType.INFO, sender, e);
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
                setInformationText("Method \"sortingButton13_Click\" entered", informationType.INFO, sender, e);
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
                setInformationText("Method \"sortingButton14_Click\" entered", informationType.INFO, sender, e);
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
                setInformationText("Method \"sortingButton15_Click\" entered", informationType.INFO, sender, e);
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
                setInformationText("Method \"sortingButton16_Click\" entered", informationType.INFO, sender, e);
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
                setInformationText("Method \"sortingButton17_Click\" entered", informationType.INFO, sender, e);
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
                setInformationText("Method \"sortingButton18_Click\" entered", informationType.INFO, sender, e);
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
                setInformationText("Method \"sortingButton19_Click\" entered", informationType.INFO, sender, e);
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
                setInformationText("Method \"sortingButton20_Click\" entered", informationType.INFO, sender, e);
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
                setInformationText("Method \"sortingButton21_Click\" entered", informationType.INFO, sender, e);
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
                setInformationText("Method \"sortingButton22_Click\" entered", informationType.INFO, sender, e);
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
