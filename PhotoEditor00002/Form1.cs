using MetadataExtractor;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.Serialization;
using System.Runtime.InteropServices;
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
using System.Net.Mail;

//using System.Web;

using myLoginForm;
using Microsoft.Win32;

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
    public struct saveableChanges
    {
        public bool imageChanges;
        public bool userChanges;
        public bool actorChanges;
        public bool eventChanges;
    }
    public partial class Form1 : Form
    {
        #region parameters
//        private bool mainWinMaxed = false;
        public programMode currentMode;
        public string filePath = string.Empty;
        public string[] actorFilePaths;
        public string[] eventFilePaths;
        public bool viewingImage = false;
        public Image picture;
        public string detFileTypeName;
        public saveableChanges toSave;
        public bool changesToSave = false;
        public bool startUpDone = false;
        public bool eventStartUpDone = false;
        public bool iAmWorking = false;
        public bool userCommentChanged = false;
        public bool imageTitleChanged = false;
        public bool subjectChanged = false;
        public bool geoLatValueChanged = false;
        public bool geoLonValueChanged = false;
        public bool bhsOwnerChanged = false;
        public int recovLevel = -2;
        public int recovType = -2;
        public bool enterPressed = false;
        private ToolTip tt = new ToolTip();

        const int maxNoOfLoadedActors = 16;
        ActorClass[] arrayOfActors = new ActorClass[maxNoOfLoadedActors];
        ActorClass actorClass;

        const int maxNoOfLoadedEvents = 16;
        int numberOfLoadedEvents = 0;
        PEEventClass[] arrayOfEvents = new PEEventClass[maxNoOfLoadedEvents];
        PEEventClass eventClass;
        int eventToShow = 0;

        private string loadedImageFileName;
        private string oldImageOrientation;
        private string currUser = WindowsIdentity.GetCurrent().Name;
        private string sProgPath = "source\\repos\\PhotoEditor00002\\PhotoEditor00002";
        private string sLogPath = "source\\repos\\PhotoEditor00002\\PhotoEditor00002\\Logs\\" + DateTime.Now.ToString("yyyy-MM-dd-hhmmss") + ".log";
        private string rootPath = "";
        private string logPath = "";
        private string sFSPath = "";
        private bool expandedImage = false;
        private int startval = 0;
        private int stopval = int.MaxValue;
        private Point mousePos;
        private double zoom = 1.0;
        private bool imageZoomed = false;

        private Button[] sortButtons;
        private int maxSortButtons = 22, noOfSortingButtons = 0;
        private String baseFolderName;
        private String[] arrayItemIndex = new string[1000];//TEST 2000];
        private LoginForm linwin;
        private bool maxSizeSet = false;

        private int noOfChangedGUIs = 0;
        private int noOfNarrowedMarkings = 0;
        //private String[] arrayOfNarrowedMarkings = new string[16];
        private int[] NarrowedOriginalNumber = new int[16];
        private int noOfEvents = 0;
        private int noOfActors = 0;
        //private bool genderTypeSet = false;
        //private bool genderLookSet = false;
        private Point startingPoint = Point.Empty;
        private Point movingPoint = Point.Empty;
        private bool panning = false;
        #endregion
        public Form1()
        {
            Login_Win_Open();
            InitializeComponent();
            sortButtons = new[] { sortingButton1, sortingButton2, sortingButton3, sortingButton4, sortingButton5, sortingButton6, sortingButton7,
                sortingButton8, sortingButton9, sortingButton10, sortingButton11, sortingButton12 };
            int tnr = currUser.IndexOf("\\");
            if ((tnr > 0) && (tnr < currUser.Length - 1))
                currUser = currUser.Substring(tnr + 1, currUser.Length - tnr - 1);
            rootPath = "C:\\Users\\" + currUser + "\\" + sProgPath;
            logPath = "C:\\Users\\" + currUser + "\\" + sLogPath;
            toSave.actorChanges = false;
            toSave.eventChanges = false;
            toSave.imageChanges = false;
            toSave.userChanges = false;
            startUpDone = true;
        }
        #region Private Support Functions
        // --- Private Support functions ---
        private void HandleGenDataTabGui(int width, int height)
        {
            int iDataBasePosX = 100;
            int iDataWidth = (int)(width - iDataBasePosX - 20);
            int iDataHalfWidth = (int)(iDataWidth / 2);
            GenData.Size = new Size(iDataWidth, height);
            ImageTitleLabel.Location = new Point(0, 6);
            ImageTitleLabel.Size = new Size(55, 13);
            ImageTitleTextBox.Location = new Point(iDataBasePosX, 3);
            ImageTitleTextBox.Size = new Size(iDataWidth, 20);
            ImageSzLabel.Location = new Point(0, 28);
            ImageSzLabel.Size = new Size(iDataWidth, 20);
            ImageSizeTextBox.Location = new Point(iDataBasePosX, 25);
            ImageSizeTextBox.Size = new Size(iDataWidth, 20);
            ImageSizeLabel.Location = new Point(0, 50);
            ImageSizeLabel.Size = new Size(97, 13);
            ImageWidthTextBox.Location = new Point(iDataBasePosX, 47);
            ImageWidthTextBox.Size = new Size(iDataHalfWidth, 20);
            ImageHeightTextBox.Location = new Point(iDataBasePosX + iDataHalfWidth, 47);
            ImageHeightTextBox.Size = new Size(iDataHalfWidth, 20);
            ImageOrientationLabel.Location = new Point(0, 73);
            ImageOrientationLabel.Size = new Size(88, 13);
            ImageOrientationTextBox.Location = new Point(iDataBasePosX, 70);
            ImageOrientationTextBox.Size = new Size(iDataWidth, 20);
            ImageResolutionLabel.Location = new Point(0, 96);
            ImageResolutionLabel.Size = new Size(84, 13);
            ImageXResTextBox.Location = new Point(iDataBasePosX, 93);
            ImageXResTextBox.Size = new Size(iDataHalfWidth, 20);
            ImageYResTextBox.Location = new Point(iDataBasePosX + iDataHalfWidth, 93);
            ImageYResTextBox.Size = new Size(iDataHalfWidth, 20);
            ImageDateTimeLabel.Location = new Point(0, 120);
            ImageDateTimeLabel.Size = new Size(104, 13);
            ImageDateTimeTextBox.Location = new Point(iDataBasePosX, 118);
            ImageDateTimeTextBox.Size = new Size(iDataWidth, 20);
            ChangeDateTimeLabel.Location = new Point(0, 140);
            ChangeDateTimeLabel.Size = new Size(96, 13);
            ChangeDateTimeTextBox.Location = new Point(iDataBasePosX, 140);
            ChangeDateTimeTextBox.Size = new Size(iDataWidth, 20);
            OriginalDateTimeLabel.Location = new Point(0, 166);
            OriginalDateTimeLabel.Size = new Size(90, 13);
            OriginalDateTimeTextBox.Location = new Point(iDataBasePosX, 163);
            OriginalDateTimeTextBox.Size = new Size(iDataWidth, 20);
            DigitizedDateTimeLabel.Location = new Point(0, 189);
            DigitizedDateTimeLabel.Size = new Size(99, 13);
            DigitizedDateTimeTextBox.Location = new Point(iDataBasePosX, 186);
            DigitizedDateTimeTextBox.Size = new Size(iDataWidth, 20);
            CompTypeLabel.Location = new Point(0, 211);
            CompTypeLabel.Size = new Size(90, 13);
            CompTypeTextBox.Location = new Point(iDataBasePosX, 208);
            CompTypeTextBox.Size = new Size(iDataWidth, 20);
            DataPresicionLabel.Location = new Point(0, 233);
            DataPresicionLabel.Size = new Size(75, 13);
            DataPrecisionTextBox.Location = new Point(iDataBasePosX, 230);
            DataPrecisionTextBox.Size = new Size(iDataWidth, 20);
            ExifVersionLabel.Location = new Point(0, 256);
            ExifVersionLabel.Size = new Size(61, 13);
            ExifVersionTextBox.Location = new Point(iDataBasePosX, 253);
            ExifVersionTextBox.Size = new Size((int)(iDataWidth / 3), 20);
            ExifImageWidthTextBox.Location = new Point(iDataBasePosX + (int)(iDataWidth / 3), 253);
            ExifImageWidthTextBox.Size = new Size((int)(iDataWidth / 3), 20);
            ExifImageHeightTextBox.Location = new Point(iDataBasePosX + (int)(2 * (iDataWidth / 3)), 253);
            ExifImageHeightTextBox.Size = new Size((int)(iDataWidth / 3) + 2, 20);
            ComponentConfigLabel.Location = new Point(0, 278);
            ComponentConfigLabel.Size = new Size(96, 13);
            ComponentConfigTextBox.Location = new Point(iDataBasePosX, 275);
            ComponentConfigTextBox.Size = new Size(iDataWidth, 20);
            SubSecTimeLabel.Location = new Point(0, 301);
            SubSecTimeLabel.Size = new Size(70, 13);
            OrigSubSecTimeTextBox.Location = new Point(iDataBasePosX, 298);
            OrigSubSecTimeTextBox.Size = new Size((int)(iDataWidth / 3), 20);
            DigSubSecTimeTextBox.Location = new Point(iDataBasePosX + (int)(iDataWidth / 3), 298);
            DigSubSecTimeTextBox.Size = new Size((int)(iDataWidth / 3), 20);
            SubSecTimeTextBox.Location = new Point(iDataBasePosX + (int)(2 * (iDataWidth / 3)), 298);
            SubSecTimeTextBox.Size = new Size((int)(iDataWidth / 3) + 2, 20);
            ColorSpaceLabel.Location = new Point(0, 324);
            ColorSpaceLabel.Size = new Size(63, 13);
            ColorSpaceTextBox.Location = new Point(iDataBasePosX, 321);
            ColorSpaceTextBox.Size = new Size((int)(iDataWidth / 3), 20);
            noOfComponentsTextBox.Location = new Point(iDataBasePosX + (int)(iDataWidth / 3), 321);
            noOfComponentsTextBox.Size = new Size((int)(2 * (iDataWidth / 3)) + 2, 20);
            ContrastAndSaturationLabel.Location = new Point(0, 347);
            ContrastAndSaturationLabel.Size = new Size(100, 13);
            ContrastTextBox.Location = new Point(iDataBasePosX, 344);
            ContrastTextBox.Size = new Size(iDataHalfWidth, 20);
            SaturationTextBox.Location = new Point(iDataBasePosX + iDataHalfWidth, 344);
            SaturationTextBox.Size = new Size(iDataHalfWidth, 20);
            subjectLabel.Location = new Point(0, height - 142);
            subjectLabel.Size = new Size(75, 13);
            subjectTextBox.Location = new Point(iDataBasePosX, height - 144);
            subjectTextBox.Size = new Size(iDataWidth, 20);
            UserCommentLabel.Location = new Point(0, height - 117);
            UserCommentLabel.Size = new Size(75, 13);
            UserCommentTextBox.Location = new Point(iDataBasePosX, height - 119);
            UserCommentTextBox.Size = new Size(iDataWidth, 64);
            SaveGenDataChangesButton.Location = new Point(0, height - 50);
            SaveGenDataChangesButton.Size = new Size((int)((width - 20) / 2), 27);
            DiscardGenDataChangesButton.Location = new Point((int)((width -20) / 2), height - 50);
            DiscardGenDataChangesButton.Size = new Size((int)((width - 20) / 2), 27);
            maxSizeSet = true;
            GenData.Refresh();
        }
        private void HandleHWDataTabGui(int width, int height)
        {
            int iDataWidth = (int)((2 * (width / 3)) - 10);
            int iDataHalfWidth = (int)(iDataWidth / 2);
            //int iBasePosX = 0;
            HWData.Size = new Size(width, height);
            // --- HWMake... ---
            HWMakeLabel.Location = new Point(0, 4);
            HWMakeLabel.Size = new Size(34, 13);
            HWMakeTextBox.Location = new Point(106, 2);
            HWMakeTextBox.Size = new Size(iDataWidth, 20);
            // --- HWModel... ---
            HWModelLabel.Location = new Point(0, 26);
            HWModelLabel.Size = new Size(36, 13);
            HWModelTextBox.Location = new Point(106, 24);
            HWModelTextBox.Size = new Size(iDataWidth, 20);
            // --- Owner... ---
            OwnerLabel.Location = new Point(0, 48);
            OwnerLabel.Size = new Size(38, 13);
            OwnerTextBox.Location = new Point(106, 48);
            OwnerTextBox.Size = new Size(iDataWidth, 20);
            // --- SerialNumber... ---
            SerialNumberLabel.Location = new Point(0, 70);
            SerialNumberLabel.Size = new Size(71, 13);
            SerialNumberTextBox.Location = new Point(106, 70);
            SerialNumberTextBox.Size = new Size(iDataWidth, 20);
            // --- CaptureMode... ---
            SceneCaptureLabel.Location = new Point(0, 94);
            SceneCaptureLabel.Size = new Size(101, 13);
            SceneCaptureTextBox.Location = new Point(106, 92);
            SceneCaptureTextBox.Size = new Size(iDataHalfWidth, 20);
            EasyShootingModeTextBox.Location = new Point(106 + iDataHalfWidth, 92);
            EasyShootingModeTextBox.Size = new Size(iDataHalfWidth, 20);
            // --- ISOData... ---
            ISODataLabel.Location = new Point(0, 116);
            ISODataLabel.Size = new Size(49, 13);
            ISOSpeedRatTextBox.Location = new Point(106, 114);
            ISOSpeedRatTextBox.Size = new Size(iDataHalfWidth, 20);
            ISODataTextBox.Location = new Point(106 + iDataHalfWidth, 114);
            ISODataTextBox.Size = new Size(iDataHalfWidth, 20);
            // --- E0osureData... ---
            ExposureDataLabel.Location = new Point(0, 138);
            ExposureDataLabel.Size = new Size(77, 13);
            ExpProgTextBox.Location = new Point(106, 136);
            ExpProgTextBox.Size = new Size((int)(iDataWidth * 0.37674), 20);
            ExpTimeTextBox.Location = new Point(106 + (int)(iDataWidth * 0.37674), 136);
            ExpTimeTextBox.Size = new Size((int)(iDataWidth * 0.23721) + 2, 20);
            ExpModeTextBox.Location = new Point(106 + (int)(iDataWidth * 0.61395), 136);
            ExpModeTextBox.Size = new Size((int)(iDataWidth * 0.39535) - 1, 20);
            // --- ExpBiasValue... ---
            ExpBiasValueLabel.Location = new Point(0, 160);
            ExpBiasValueLabel.Size = new Size(80, 13);
            ExpBiasValueTextBox.Location = new Point(106, 158);
            ExpBiasValueTextBox.Size = new Size(iDataWidth, 20);
            // --- RecExpIndex... ---
            RecExpIndexLabel.Location = new Point(0, 182);
            RecExpIndexLabel.Size = new Size(97, 13);
            RekExpIndexTextBox.Location = new Point(106, 180);
            RekExpIndexTextBox.Size = new Size(iDataWidth, 20);
            // --- YCbCr... ---
            YCbCrPosLabel.Location = new Point(0, 204);
            YCbCrPosLabel.Size = new Size(77, 13);
            YCbCrPosTextBox.Location = new Point(106, 202);
            YCbCrPosTextBox.Size = new Size(iDataWidth, 20);
            // --- SensitivityType... ---
            SensitivityTypeLabel.Location = new Point(0, 226);
            SensitivityTypeLabel.Size = new Size(81, 13);
            SensitivityTypeTextBox.Location = new Point(106, 224);
            SensitivityTypeTextBox.Size = new Size(iDataWidth, 20);
            // --- ShutterSpeed... ---
            ShutterSpeedLabel.Location = new Point(0, 248);
            ShutterSpeedLabel.Size = new Size(73, 13);
            ShutterSpeedTextBox.Location = new Point(106, 246);
            ShutterSpeedTextBox.Size = new Size(iDataWidth, 20);
            ShutterSpeedTextBox.Size = new Size(iDataWidth, 20);
            // --- ApertureData... ---
            ApertureDataLabel.Location = new Point(0, 274);
            ApertureDataLabel.Size = new Size(73, 13);
            MinApertureTextBox.Location = new Point(106, 272);
            MinApertureTextBox.Size = new Size((int)(iDataWidth / 3), 20);
            ApertureTextBox.Location = new Point(106 + (int)(iDataWidth / 3), 272);
            ApertureTextBox.Size = new Size((int)(iDataWidth / 3), 20);
            MaxApertureTextBox.Location = new Point(106 + (int)(2 * (iDataWidth / 3)), 272);
            MaxApertureTextBox.Size = new Size((int)(iDataWidth / 3) + 2, 20);
            // --- DisplayAperture... ---
            DisplayApertureLabel.Location = new Point(0, 297);
            DisplayApertureLabel.Size = new Size(83, 13);
            DisplayApertureTextBox.Location = new Point(106, 295);
            DisplayApertureTextBox.Size = new Size(iDataWidth, 20);
            // --- SelfTimer... ---
            SelfTimerLabel.Location = new Point(0, 320);
            SelfTimerLabel.Size = new Size(50, 13);
            SelfTimerDelayTextBox.Location = new Point(106, 318);
            SelfTimerDelayTextBox.Size = new Size(iDataWidth, 20);
            // -- MeteringModel... ---
            MeteringModeLabel.Location = new Point(0, 342);
            MeteringModeLabel.Size = new Size(105, 13);
            MeteringModeTextBox.Location = new Point(106, 340);
            MeteringModeTextBox.Size = new Size(iDataWidth, 20);
            // --- Flash info ---
            FlashDetailsLabel.Location = new Point(0, 343);
            FlashDetailsLabel.Size = new Size(100, 13);
            FlashDetailsTextBox.Location = new Point(106, 341);
            FlashDetailsTextBox.Size = new Size(iDataWidth, 20);
            FlashTextBox.Size = new Size(iDataWidth, 20);
            // --- Quality... ---
            QualityLabel.Location = new Point(0, 365);
            QualityLabel.Size = new Size(39, 13);
            QualityTextBox.Location = new Point(106, 363);
            QualityTextBox.Size = new Size(iDataWidth, 20);
            // --- ContinuousDrive... ---
            ContinousDriveLabel.Location = new Point(0, 388);
            ContinousDriveLabel.Size = new Size(80, 13);
            ContinousDriveTextBox.Location = new Point(106, 386);
            ContinousDriveTextBox.Size = new Size(iDataWidth, 20);
            // --- FocalLength... ---
            FocalLengthLabel.Location = new Point(0, 411);
            FocalLengthLabel.Size = new Size(65, 13);
            FocalLengthTextBox.Location = new Point(106, 409);
            FocalLengthTextBox.Size = new Size((int)(iDataWidth / 3), 20);
            FocalPlaneXResTextBox.Location = new Point(106 + (int)(iDataWidth / 3), 409);
            FocalPlaneXResTextBox.Size = new Size((int)(iDataWidth / 3), 20);
            FocalPlaneYResTextBox.Location = new Point(106 + (2 * (int)(iDataWidth / 3)), 409);
            FocalPlaneYResTextBox.Size = new Size((int)(iDataWidth / 3), 20);
            // --- CustomRender... ---
            CustomRenderedLabel.Location = new Point(0, 434);
            CustomRenderedLabel.Size = new Size(87, 13);
            CustomRenderedTextBox.Location = new Point(106, 432);
            CustomRenderedTextBox.Size = new Size(iDataWidth, 20);
            // --- FNum... ---
            FNumLabel.Location = new Point(0, 457);
            FNumLabel.Size = new Size(38, 13);
            FNumTextBox.Location = new Point(106, 455);
            FNumTextBox.Size = new Size(iDataWidth, 20);
            // --- WhiteBalance... ---
            WhiteBalanceInfoLabel.Location = new Point(0, 480);
            WhiteBalanceInfoLabel.Size = new Size(76, 13);
            WhiteBalanceModeTextBox.Location = new Point(106, 478);
            WhiteBalanceModeTextBox.Size = new Size(iDataWidth, 20);
            // --- RecordMode... ---
            RecordModeLabel.Location = new Point(0, 503);
            RecordModeLabel.Size = new Size(71, 13);
            RecordModeTextBox.Location = new Point(106, 501);
            RecordModeTextBox.Size = new Size(iDataWidth, 20);
            // --- Save/Discard-buttons ---
            SaveGenHWDataChangesButton.Location = new Point(0, height - 50);
            SaveGenHWDataChangesButton.Size = new Size((int)(width / 2), 27);
            DiscardGenHWDataChangesButton.Location = new Point((int)(width / 2), height - 50);
            DiscardGenHWDataChangesButton.Size = new Size((int)(width / 2), 27);
            // ----------------------------
            HWData.Refresh();
        }
        private void HandleHWAddonDataTabGui(int width, int height)
        {
            int dataWidth = (int)((2 * (width / 3)) - 10);
            int dataHalfWidth = (int)(dataWidth / 2);
            int rowNumber = 0;
            //int iBasePosX = 0;
            HWAddonTabPage.Size = new Size(width, height);
            // --- LensSpec... ---
            LensSpecificationLabel.Location = new Point(0, (rowNumber*23) + 4);
            LensSpecificationLabel.Size = new Size(92, 13);
            LensSpecTextBox.Location = new Point(106, (rowNumber * 23) + 2);
            LensSpecTextBox.Size = new Size(dataWidth, 20);
            // --- LensModel... ---
            rowNumber++;
            LensModelLabel.Location = new Point(0, (rowNumber * 23) + 4);
            LensModelLabel.Size = new Size(36, 13);
            LensModelTextBox.Location = new Point(106, (rowNumber * 23) + 2);
            LensModelTextBox.Size = new Size(dataWidth, 20);
            // --- LensSerialNumber... ---
            rowNumber++;
            LensSerialNumberLabel.Location = new Point(0, (rowNumber * 23) + 4);
            LensSerialNumberLabel.Size = new Size(71, 13);
            LensSerNoTextBox.Location = new Point(106, (rowNumber * 23) + 2);
            LensSerNoTextBox.Size = new Size(dataWidth, 20);
            // --- LensType... ---
            rowNumber++;
            LensTypeLabel.Location = new Point(0, (rowNumber * 23) + 4);
            LensTypeLabel.Size = new Size(53, 16);
            LensTypeInfoTextBox.Location = new Point(106, (rowNumber * 23) + 2);
            LensTypeInfoTextBox.Size = new Size(dataWidth, 20);
            // --- ZoomInfo... ---
            rowNumber++;
            ZoomInfoLabel.Location = new Point(0, (rowNumber * 23) + 4);
            ZoomInfoLabel.Size = new Size(80, 13);
            ManualZoomTextBox.Location = new Point(106, (rowNumber * 23) + 2);
            ManualZoomTextBox.Size = new Size(dataHalfWidth, 20);
            DigitalZoomTextBox.Location = new Point(106 + dataHalfWidth, (rowNumber * 23) + 2);
            DigitalZoomTextBox.Size = new Size(dataHalfWidth, 20);
            // --- ZoomSource... ---
            rowNumber++;
            ZoomSourceWidthHeightLabel.Location = new Point(0, (rowNumber * 23) + 4);
            ZoomSourceWidthHeightLabel.Size = new Size(90, 13);
            ZoomSourceWidthTextBox.Location = new Point(106, (rowNumber * 23) + 2);
            ZoomSourceWidthTextBox.Size = new Size(dataHalfWidth, 20);
            ZoomSourceHeightTextBox.Location = new Point(106 + dataHalfWidth, (rowNumber * 23) + 2);
            ZoomSourceHeightTextBox.Size = new Size(dataHalfWidth, 20);
            // --- FocusAndMacro... ---
            rowNumber++;
            FocusAndMacroModeLabel.Location = new Point(0, (rowNumber * 23) + 4);
            FocusAndMacroModeLabel.Size = new Size(101, 13);
            FocusModeTextBox.Location = new Point(106, (rowNumber * 23) + 2);
            FocusModeTextBox.Size = new Size(dataHalfWidth, 20);
            MacroModeTextBox.Location = new Point(106 + dataHalfWidth, (rowNumber * 23) + 2);
            MacroModeTextBox.Size = new Size(dataHalfWidth, 20);
            // --- FocusTypeAndMetering... ---
            rowNumber++;
            FocusTypeAndMeteringModeInfoLabel.Location = new Point(0, (rowNumber * 23) + 4);
            FocusTypeAndMeteringModeInfoLabel.Size = new Size(105, 13);
            FocusTypeInfoTextBox.Location = new Point(106, (rowNumber * 23) + 2);
            FocusTypeInfoTextBox.Size = new Size(dataHalfWidth, 20);
            FocusMeteringInfoTextBox.Location = new Point(106 + dataHalfWidth, (rowNumber * 23) + 2);
            FocusMeteringInfoTextBox.Size = new Size(dataHalfWidth, 20);
            // --- ContinuousFocus... Sharpness... ---
            rowNumber++;
            ContinousFocusLabel.Location = new Point(0, (rowNumber * 23) + 4);
            ContinousFocusLabel.Size = new Size(83, 13);
            ContinouosFocusTextBox.Location = new Point(106, (rowNumber * 23) + 2);
            ContinouosFocusTextBox.Size = new Size(dataHalfWidth, 20);
            SharpnessInfoTextBox.Location = new Point(106 + dataHalfWidth, (rowNumber * 23) + 2);
            SharpnessInfoTextBox.Size = new Size(dataHalfWidth, 20);
            // --- Focal... ---
            FocalMinMaxLengthUnitLabel.Location = new Point(0, (rowNumber * 23) + 4);
            FocalMinMaxLengthUnitLabel.Size = new Size(90, 13);
            FocalMinLengthInfoTextBox.Location = new Point(106, (rowNumber * 23) + 2);
            FocalMinLengthInfoTextBox.Size = new Size((int)(dataWidth / 3), 20);
            FocalMaxLengthInfoTextBox.Location = new Point(106 + (int)(dataWidth / 3), (rowNumber * 23) + 2);
            FocalMaxLengthInfoTextBox.Size = new Size((int)(dataWidth / 3), 20);
            FocalUnitInfoTextBox.Location = new Point(106 + (int)(2 * (dataWidth / 3)), (rowNumber * 23) + 2);
            FocalUnitInfoTextBox.Size = new Size((int)(dataWidth / 3), 20);
            // --- AFPoint... ---
            rowNumber++;
            AFPointSelectedLabel.Location = new Point(0, (rowNumber * 23) + 4);
            AFPointSelectedLabel.Size = new Size(66, 13);
            AFPointSelectedInfoTextBox.Location = new Point(106, (rowNumber * 23) + 2);
            AFPointSelectedInfoTextBox.Size = new Size(dataWidth, 20);
            // --- AESettings... ---
            rowNumber++;
            AESettingsLabel.Location = new Point(0, (rowNumber * 23) + 4);
            AESettingsLabel.Size = new Size(60, 13);
            AESettingsTextBox.Location = new Point(106, (rowNumber * 23) + 2);
            AESettingsTextBox.Size = new Size(dataWidth, 20);
            // --- SpotMeteringMode... ---
            rowNumber++;
            SpotMeteringModeLabel.Location = new Point(0, (rowNumber * 23) + 4);
            SpotMeteringModeLabel.Size = new Size(101, 13);
            SpotMeteringModeTextBox.Location = new Point(106, (rowNumber * 23) + 2);
            SpotMeteringModeTextBox.Size = new Size(dataWidth, 20);
            // --- Flash... ---
            rowNumber++;
            FlashLabel.Location = new Point(0, (rowNumber * 23) + 4);
            FlashLabel.Size = new Size(32, 13);
            MeteringModeTextBox.Location = new Point(106, (rowNumber * 23) + 2);
            MeteringModeTextBox.Size = new Size(dataWidth, 20);
            // --- FlashDetails... ---
            rowNumber++;
            FlashDetailsLabel.Location = new Point(0, (rowNumber * 23) + 4);
            FlashDetailsLabel.Size = new Size(65, 13);
            FlashDetailsTextBox.Location = new Point(106, (rowNumber * 23) + 2);
            FlashDetailsTextBox.Size = new Size(dataWidth, 20);
            // --- FlashInfo... ---
            rowNumber++;
            FlashInfoLabel.Location = new Point(0, (rowNumber * 23) + 4);
            FlashInfoLabel.Size = new Size(52, 13);
            FlashPixVersionTextBox.Location = new Point(106, (rowNumber * 23) + 2);
            FlashPixVersionTextBox.Size = new Size(dataWidth, 20);
            rowNumber++;
            FlashModeTextBox.Location = new Point(106, (rowNumber * 23) + 2);
            FlashModeTextBox.Size = new Size((int)(dataWidth * 0.3812953), 20);
            FlashActivityInfoTextBox.Location = new Point(106 + (int)(dataWidth * 0.3812953), (rowNumber * 23) + 2);
            FlashActivityInfoTextBox.Size = new Size((int)(dataWidth * 0.623256), 20);
            HWAddonTabPage.Refresh();
        }
        private void HandleGeoDataTabGui(int width, int height)
        {
            GeoData.Size = new Size(width, height);
            int dataWidth = (int)((2 * (width / 3)) - 10);
            int dataHalfWidth = (int)(dataWidth / 2);
            //int iBasePosX = 0;
            // --- GPSVersion... ---
            int rowNumber = 0;
            GPSVersionLabel.Location = new Point(0, (rowNumber * 23) + 4);
            GPSVersionLabel.Size = new Size(70, 17);
            GPSVersionTextBox.Location = new Point(106, (rowNumber * 23) + 2);
            GPSVersionTextBox.Size = new Size(dataWidth, 20);
            // --- NoOfSat... ---
            rowNumber++;
            NoOfSatLabel.Location = new Point(0, (rowNumber * 23) + 4);
            NoOfSatLabel.Size = new Size(78, 17);
            NoOfSatTextBox.Location = new Point(106, (rowNumber * 23) + 2);
            NoOfSatTextBox.Size = new Size(dataWidth, 20);
            // --- TrackRef... MeashMode... ---
            rowNumber++;
            TrackRefModeLabel.Location = new Point(0, (rowNumber * 23) + 4);
            TrackRefModeLabel.Size = new Size(83, 17);
            TrackRefTextBox.Location = new Point(106, (rowNumber * 23) + 2);
            TrackRefTextBox.Size = new Size(dataHalfWidth, 20);
            MeasModeTextBox.Location = new Point(106 + dataHalfWidth, (rowNumber * 23) + 2);
            MeasModeTextBox.Size = new Size(dataHalfWidth, 20);
            // --- TimeStamp... ---
            rowNumber++;
            TimeStampLabel.Location = new Point(0, (rowNumber * 23) + 4);
            TimeStampLabel.Size = new Size(63, 17);
            TimeStampTextBox.Location = new Point(106, (rowNumber * 23) + 2);
            TimeStampTextBox.Size = new Size(dataWidth, 20);
            // --- Lat... ---
            rowNumber++;
            LatLabel.Location = new Point(0, (rowNumber * 23) + 4);
            LatLabel.Size = new Size(45, 17);
            LatValueTextBox.Location = new Point(106, (rowNumber * 23) + 2);
            LatValueTextBox.Size = new Size((int)(dataWidth * 0.55), 20);
            LatRefTextBox.Location = new Point(106 + (int)(dataWidth * 0.55), (rowNumber * 23) + 2);
            LatRefTextBox.Size = new Size((int)(dataWidth * 0.17), 20);
            ViewGeoDataPosButton.Location = new Point(106 + (int)(dataWidth * 0.75), (rowNumber * 23) + 2);
            ViewGeoDataPosButton.Size = new Size(57, 47);
            // --- Lon... ---
            rowNumber++;
            LonLabel.Location = new Point(0, (rowNumber * 23) + 4);
            LonLabel.Size = new Size(54, 17);
            LonValueTextBox.Location = new Point(106, (rowNumber * 23) + 2);
            LonValueTextBox.Size = new Size((int)(dataWidth * 0.55), 20);
            LonRefTextBox.Location = new Point(106 + (int)(dataWidth * 0.55), (rowNumber * 23) + 2);
            LonRefTextBox.Size = new Size((int)(dataWidth * 0.17), 20);
            // --- Save... Discard... ---
            SaveGeoDataChangesButton.Location = new Point(0, height - 50);
            SaveGeoDataChangesButton.Size = new Size((int)(width / 2), 27);
            DiscardGeoDataChangesButton.Location = new Point((int)(width / 2), height - 50);
            DiscardGeoDataChangesButton.Size = new Size((int)(width / 2), 27);
            GeoData.Refresh();
        }
        private void HandleActorDataTabGui(int width, int height)
        {
            ActorData.Size = new Size(width, height);
            width -= 20;
            #region Artist
            int rowNumber = 0;
            ArtistLabel.Location = new Point(0, (rowNumber * 23) + 4);
            ArtistLabel.Size = new Size(100, 13);
            ActiveArtistsComboBox.Location = new Point(103, (rowNumber * 23) + 2);
            ActiveArtistsComboBox.Size = new Size(width - 109, 21);
            #endregion
            #region ArtistIdentity
            rowNumber++;
            ArtistIdentityEnterTextBox.Location = new Point(103, (rowNumber * 23) + 2);
            ArtistIdentityEnterTextBox.Size = new Size(width - 109, 20);
            #endregion
            #region NameType
            rowNumber++;
            NameTypeComboBox.Location = new Point(0, (rowNumber * 23) + 2);
            NameTypeComboBox.Size = new Size(100, 21);
            SelNameTypeTextBox.Location = new Point(103, (rowNumber * 23) + 2);
            SelNameTypeTextBox.Size = new Size(width - 109, 20);
            AddNewNameButton.Location = new Point(width - 53, (rowNumber * 23) + 2);
            AddNewNameButton.Size = new Size(50, 21);
            #endregion
            #region ActorContact
            if (linwin.userRightsLevel > LoginForm.rightsLevel.Medium)
            {
                rowNumber++;
                ActorContactTypeComboBox.Location = new Point(0, (rowNumber * 23) + 2);
                ActorContactTypeComboBox.Size = new Size(100, 21);
                SelContactTypeTextBox.Location = new Point(103, (rowNumber * 23) + 2);
                SelContactTypeTextBox.Size = new Size(width - 109, 20);
                AddContactButton.Location = new Point(width - 53, (rowNumber * 23) + 2);
                AddContactButton.Size = new Size(50, 21);
            }
            #endregion
            #region BirthData
            rowNumber++;
            BirthDataLabel.Location = new Point(0, (rowNumber * 23) + 4);
            BirthDataLabel.Size = new Size(100, 13);
            BirthStreetAddrTextBox.Location = new Point(103, (rowNumber * 23) + 6);
            BirthStreetAddrTextBox.Size = new Size(width - 109, 20);
            rowNumber++;
            BirthZipCodeTextBox.Location = new Point(103, (rowNumber * 23) + 4);
            BirthZipCodeTextBox.Size = new Size((int)((width - 109) / 3), 20);
            BirthAreaNameTextBox.Location = new Point(103 + (int)(width / 3), (rowNumber * 23) + 4);
            BirthAreaNameTextBox.Size = new Size((int)(2 * ((width - 109) / 3)), 20);
            rowNumber++;
            BirthCitynameTextBox.Location = new Point(103, (rowNumber * 23) + 4);
            BirthCitynameTextBox.Size = new Size(width - 109, 20);
            rowNumber++;
            BirthCountryTextBox.Location = new Point(103, (rowNumber * 23) + 4);
            BirthCountryTextBox.Size = new Size(width - 109, 20);
            rowNumber++;
            label1.Location = new Point(0, (rowNumber * 23) + 6);
            label1.Size = new Size(93, 13);
            BirthDateTextBox.Location = new Point(103, (rowNumber * 23) + 4);
            BirthDateTextBox.Size = new Size((int)(2 * ((width - 109) / 3)), 20);
            if (linwin.userRightsLevel >= LoginForm.rightsLevel.Secret)
            {
                SocSecNumberTextBox.Location = new Point(103 + (int)(2 * ((width - 109) / 3)), (rowNumber * 23) + 4);
                SocSecNumberTextBox.Size = new Size((int)((width - 109) / 3), 20);
                SocSecNumberTextBox.Visible = true;
            }
            else
                SocSecNumberTextBox.Visible = false;
            rowNumber++;
            #endregion
            #region Geographic Birth position
            label2.Location = new Point(0, (rowNumber * 23) + 6);
            label2.Size = new Size(100, 13);
            BirthLatitudeTextBox.Location = new Point(103, (rowNumber * 23) + 4);
            BirthLatitudeTextBox.Size = new Size((int)((width - 109) / 2), 20);
            BirthLongitudeTextBox.Location = new Point(103 + (int)((width - 109) / 2), (rowNumber * 23) + 4);
            BirthLongitudeTextBox.Size = new Size((int)((width - 109) / 2), 20);
            // --- ViewGeoDataPosButton ---
            ViewGeoDataPosButton.Visible = false;
            ViewGeoPosButton.Visible = true;
            rowNumber++;
            ViewGeoPosButton.Location = new Point(103, (rowNumber * 23) + 4);
            ViewGeoPosButton.Size = new Size(width - 109, 23);
            #endregion
            #region SkinTone
            if (linwin.userRightsLevel >= LoginForm.rightsLevel.Secret)
            {
                rowNumber++;
                SkinToneLabel.Location = new Point(0, (rowNumber * 23) + 7);
                SkinToneLabel.Size = new Size(100, 13);
                AddSkinToneButton.Location = new Point(0, (rowNumber * 23) + 5);
                AddSkinToneButton.Size = new Size(100, 21);
                SkinToneTagTextBox.Location = new Point(103, (rowNumber * 23) + 5);
                SkinToneTagTextBox.Size = new Size((int)((width - 109) / 2), 20);
                SkinToneDateTimePicker.Location = new Point(103 + (int)((width - 109) / 2), (rowNumber * 23) + 5);
                SkinToneDateTimePicker.Size = new Size((int)((width - 109) / 2), 20);
                SkinToneValidDateComboBox.Location = new Point(103 + (int)((width - 109) / 2), (rowNumber * 23) + 5);
                SkinToneValidDateComboBox.Size = new Size((int)((width - 109) / 2), 20);
            }
            #endregion
            #region EyeColor
            if (linwin.userRightsLevel >= LoginForm.rightsLevel.Secret)
            {
                rowNumber++;
                EyeColorLabel.Location = new Point(0, (rowNumber * 23) + 7);
                EyeColorLabel.Size = new Size(100, 13);
                AddEyeColorButton.Location = new Point(0, (rowNumber * 23) + 5);
                AddEyeColorButton.Size = new Size(100, 21);
                SelEyeColorTextBox.Location = new Point(103, (rowNumber * 23) + 5);
                SelEyeColorTextBox.Size = new Size((int)((width - 109) / 2), 20);
                EyeColorValidDateComboBox.Location = new Point(103 + (int)((width - 109) / 2), (rowNumber * 23) + 5);
                EyeColorValidDateComboBox.Size = new Size((int)((width - 109) / 2), 21);
            }
            #endregion
            #region GenderData
            if (linwin.userRightsLevel >= LoginForm.rightsLevel.QualifSecret)
            {
                rowNumber++;
                GenderInfoLabel.Location = new Point(0, (rowNumber * 23) + 9);
                GenderInfoLabel.Size = new Size(100, 13);
                GenderTypeTextBox.Location = new Point(103, (rowNumber * 23) + 7);
                GenderTypeTextBox.Size = new Size((int)((width - 109) / 2), 20);
                GenderInfoValidDateComboBox.Location = new Point(103 + (int)((width - 109) / 2), (rowNumber * 23) + 7);
                GenderInfoValidDateComboBox.Size = new Size((int)((width - 109) / 2), 21);
                rowNumber++;
                GdrLengthTextBox.Location = new Point(103, (rowNumber * 23) + 7);
                GdrLengthTextBox.Size = new Size((int)((width - 109) / 2), 20);
                GdrCircumfTextBox.Location = new Point(103 + (int)((width - 109) / 2), (rowNumber * 23) + 7);
                GdrCircumfTextBox.Size = new Size((int)((width - 109) / 2), 20);
                rowNumber++;
                GdrLookTypeTextBox.Location = new Point(103, (rowNumber * 23) + 7);
                GdrLookTypeTextBox.Size = new Size((int)((width - 109) / 2), 20);
                GdrBehaveTypeTextBox.Location = new Point(103 + (int)((width - 109) / 2), (rowNumber * 23) + 7);
                GdrBehaveTypeTextBox.Size = new Size((int)((width - 109) / 2), 20);
                rowNumber++;
                AddGenderInfoButton.Location = new Point(0, (rowNumber * 23) + 6);
                AddGenderInfoButton.Size = new Size(100, 21);
                GdrPresentTextBox.Location = new Point(103, (rowNumber * 23) + 6);
                GdrPresentTextBox.Size = new Size((int)(width - 109), 20);
            }
            #endregion
            #region Length
            rowNumber++;
            LengthLabel.Location = new Point(0, (rowNumber * 23) + 7);
            LengthLabel.Size = new Size(100, 21);
            AddLengthInfoButton.Location = new Point(0, (rowNumber * 23) + 7);
            AddLengthInfoButton.Size = new Size(100, 21);
            LengthTextBox.Location = new Point(103, (rowNumber * 23) + 7);
            LengthTextBox.Size = new Size(width - 192, 20);
            LengthValidDateComboBox.Location = new Point(103 + (width - 192), (rowNumber * 23) + 7);
            LengthValidDateComboBox.Size = new Size(width - 295, 21);
            #endregion
            #region Weight
            rowNumber++;
            WeightLabel.Location = new Point(0, (rowNumber * 23) + 7);
            WeightLabel.Size = new Size(100, 21);
            AddWeightInfoButton.Location = new Point(0, (rowNumber * 23) + 7);
            AddWeightInfoButton.Size = new Size(100, 21);
            WeightTextBox.Location = new Point(103, (rowNumber * 23) + 7);
            WeightTextBox.Size = new Size((int)(2 * (width - 109) / 3), 20);
            WeightValidDateComboBox.Location = new Point(103 + (int)(2 * (width - 109) / 3), (rowNumber * 23) + 7);
            WeightValidDateComboBox.Size = new Size((int)((width - 109) / 3), 21);
            rowNumber++;
            #endregion
            #region Hair
            ChestDataLabel.Location = new Point(0, (rowNumber * 23) + 7);
            ChestDataLabel.Size = new Size(100, 21);
            AddChestInfoButton.Location = new Point(0, (rowNumber * 23) + 7);
            AddChestInfoButton.Size = new Size(100, 21);
            ChestTypeTextBox.Location = new Point(103, (rowNumber * 23) + 7);
            ChestTypeTextBox.Size = new Size((int)(2 * (width - 109) / 3), 20);
            ChestValidDateComboBox.Location = new Point(103 + (int)(2 * (width - 109) / 3), (rowNumber * 23) + 7);
            ChestValidDateComboBox.Size = new Size((int)(width / 3), 21);
            rowNumber++;
            ChestCircTextBox.Location = new Point(103, (rowNumber * 23) + 7);
            ChestCircTextBox.Size = new Size((int)((width - 109) / 2), 20);
            ChestSizeTextBox.Location = new Point(103 + (int)((width - 109) / 2), (rowNumber * 23) + 7);
            ChestSizeTextBox.Size = new Size((int)((width - 109) / 2), 20);
            #endregion
            #region Hair
            rowNumber++;
            AddHairInfoButton.Location = new Point(0, (rowNumber * 23) + 7);
            AddHairInfoButton.Size = new Size(100, 21);
            HairDataLabel.Location = new Point(0, (rowNumber * 23) + 7);
            HairDataLabel.Size = new Size(100, 21);
            hairColorCmbBx.Location = new Point(103, (rowNumber * 23) + 7);
            hairColorCmbBx.Size = new Size((int)((width - 109) / 2), 21);
            HairColorTextBox.Location = new Point(103, (rowNumber * 23) + 7);
            HairColorTextBox.Size = new Size((int)((width - 109) / 2), 20);
            hairValidDateTxtBx.Location = new Point(103 + (int)((width - 109) / 2), (rowNumber * 23) + 7);
            hairValidDateTxtBx.Size = new Size((int)((width - 109) / 2), 20);
            HairDataValidDateComboBox.Location = new Point(103 + (int)((width - 109) / 2), (rowNumber * 23) + 7);
            HairDataValidDateComboBox.Size = new Size((int)((width - 109) / 2), 21);
            rowNumber++;
            HairTextureTypeTextBox.Location = new Point(103, (rowNumber * 23) + 7);
            HairTextureTypeTextBox.Size = new Size((int)((width - 109) / 2), 20);
            hairTextureCmbBx.Location = new Point(103, (rowNumber * 23) + 7);
            hairTextureCmbBx.Size = new Size((int)((width - 109) / 2), 21);
            HairLengthTextBox.Location = new Point(103 + (int)((width - 109) / 2), (rowNumber * 23) + 7);
            HairLengthTextBox.Size = new Size((int)((width - 109) / 2), 20);
            hairLengthCmbBx.Location = new Point(103 + (int)((width - 109) / 2), (rowNumber * 23) + 7);
            hairLengthCmbBx.Size = new Size((int)((width - 109) / 2), 21);
            #endregion
            #region Markings
            rowNumber++;
            MarkingsLabel.Location = new Point(0, (rowNumber * 23) + 9);
            MarkingsLabel.Size = new Size(100, 13);
            MarkingTypeComboBox.Location = new Point(103, (rowNumber * 23) + 7);
            MarkingTypeComboBox.Size = new Size((int)((width - 109) / 2), 21);
            MarkingPosTextBox.Location = new Point(103 + (int)((width - 109) / 2), (rowNumber * 23) + 7);
            MarkingPosTextBox.Size = new Size((int)((width - 109) / 2), 20);
            rowNumber++;
            AddMarkingDataButton.Location = new Point(0, (rowNumber * 23) + 7);
            AddMarkingDataButton.Size = new Size(100, 21);
            MarkingMotifTextBox.Location = new Point(103, (rowNumber * 23) + 7);
            MarkingMotifTextBox.Size = new Size((int)((width - 109) / 2), 20);
            MarkingsValidDateComboBox.Location = new Point(103 + (int)((width - 109) / 2), (rowNumber * 23) + 7);
            MarkingsValidDateComboBox.Size = new Size((int)((width - 109) / 2), 21);
            MarkingDateTextBox.Location = new Point(103 + (int)((width - 109) / 2), (rowNumber * 23) + 7);
            MarkingDateTextBox.Size = new Size((int)((width - 109) / 2), 21);
            #endregion
            #region Occupation
            rowNumber++;
            label4.Location = new Point(0, (rowNumber * 23) + 9);
            label4.Size = new Size(100, 13);
            OccupTitleTextBox.Location = new Point(103, (rowNumber * 23) + 7);
            OccupTitleTextBox.Size = new Size((int)(width - 109), 20);
            rowNumber++;
            ActorOccupationStartDateComboBox.Location = new Point(0, (rowNumber * 23) + 7);
            ActorOccupationStartDateComboBox.Size = new Size(100, 21);
            occupationStartTxtBx.Location = new Point(0, (rowNumber * 23) + 7);
            occupationStartTxtBx.Size = new Size(100, 20);
            ActorOccupationCompanyTextBox.Location = new Point(103, (rowNumber * 23) + 7);
            ActorOccupationCompanyTextBox.Size = new Size((int)(width - 109), 20);
            rowNumber++;
            AddOccupationDataButton.Location = new Point(0, (rowNumber * 23) + 7);
            AddOccupationDataButton.Size = new Size(100, 65);
            ActorEmployAddressTextBox.Location = new Point(103, (rowNumber * 23) + 7);
            ActorEmployAddressTextBox.Size = new Size((int)(width - 109), 20);
            rowNumber++;
            ActorEmployZipCodeTextBox.Location = new Point(103, (rowNumber * 23) + 7);
            ActorEmployZipCodeTextBox.Size = new Size((int)((width - 109) / 3), 20);
            ActorEmpoyAreanameTextBox.Location = new Point(103 + (int)((width - 109) / 3), (rowNumber * 23) + 7);
            ActorEmpoyAreanameTextBox.Size = new Size((int)(2 * (width - 109) / 3), 20);
            rowNumber++;
            ActorEmployCitynameTextBox.Location = new Point(103, (rowNumber * 23) + 7);
            ActorEmployCitynameTextBox.Size = new Size((int)(width - 109), 20);
            rowNumber++;
            ActorOccupationEndDaeComboBox.Location = new Point(0, (rowNumber * 23) + 7);
            ActorOccupationEndDaeComboBox.Size = new Size(100, 21);
            occupationEndTxtBx.Location = new Point(0, (rowNumber * 23) + 7);
            occupationEndTxtBx.Size = new Size(100, 20);
            ActorEmployCountryTextBox.Location = new Point(103, (rowNumber * 23) + 7);
            ActorEmployCountryTextBox.Size = new Size((int)(width - 109), 20);
            ActorEmployCountryTextBox.Visible = true;
            #endregion
            #region Residence
            rowNumber++;
            ResidenceLabel.Location = new Point(0, (rowNumber * 23) + 7);
            ResidenceLabel.Size = new Size(100, 13);
            ResidAddressTextBox.Location = new Point(103, (rowNumber * 23) + 7);
            ResidAddressTextBox.Size = new Size((int)(width - 109), 20);
            rowNumber++;
            ResidStartDateComboBox.Location = new Point(0, (rowNumber * 23) + 7);
            ResidStartDateComboBox.Size = new Size(100, 21);
            residenceStartTxtBx.Location = new Point(0, (rowNumber * 23) + 7);
            residenceStartTxtBx.Size = new Size(100, 20);
            ResidZipCodeTextBox.Location = new Point(103, (rowNumber * 23) + 7);
            ResidZipCodeTextBox.Size = new Size((int)((width - 109) / 2), 20);
            ResidAreanameTextBox.Location = new Point(103 + (int)((width - 109) / 2), (rowNumber * 23) + 7);
            ResidAreanameTextBox.Size = new Size((int)((width - 109) / 2), 20);
            rowNumber++;
            AddResidenceButton.Location = new Point(0, (rowNumber * 23) + 7);
            AddResidenceButton.Size = new Size(100, 20);
            ResidCitynameTextBox.Location = new Point(103, (rowNumber * 23) + 7);
            ResidCitynameTextBox.Size = new Size((int)(width - 109), 20);
            rowNumber++;
            ResidEndDateComboBox.Location = new Point(0, (rowNumber * 23) + 7);
            ResidEndDateComboBox.Size = new Size(100, 20);
            residenceEndTxtBx.Location = new Point(0, (rowNumber * 23) + 7);
            residenceEndTxtBx.Size = new Size(100, 20);
            ResidCountryTextBox.Location = new Point(103, (rowNumber * 23) + 7);
            ResidCountryTextBox.Size = new Size((int)(width - 109), 20);
            #endregion
            #region Event
            rowNumber++;
            AttEventLabel.Location = new Point(0, (rowNumber * 23) + 9);
            AttEventLabel.Size = new Size(90, 13);
            EventIdTextBox.Location = new Point(103, (rowNumber * 23) + 7);
            EventIdTextBox.Size = new Size((int)((width - 109) / 2), 20);
            attdEvtDateTxtBx.Location = new Point(103 + (int)((width - 109) / 2), (rowNumber * 23) + 7);
            attdEvtDateTxtBx.Size = new Size((int)((width - 109) / 2), 20);
            addAttdEventBtn.Location = new Point(0, (rowNumber * 23) + 7);
            addAttdEventBtn.Size = new Size(90, 13);
            AttEvtIDCmbBx.Location = new Point(103, (rowNumber * 23) + 7);
            AttEvtIDCmbBx.Size = new Size((int)((width - 109) / 2), 21);
            attdEvtDateTxtBx.Location = new Point(103 + (int)((width - 109) / 2), (rowNumber * 23) + 7);
            attdEvtDateTxtBx.Size = new Size((int)((width - 109) / 2), 21);
            AttdEventsDateComboBox.Location = new Point(103 + (int)((width - 109) / 2), (rowNumber * 23) + 7);
            AttdEventsDateComboBox.Size = new Size((int)((width - 109) / 2), 21);
            #endregion
            #region Related Images
            if (linwin.userRightsLevel > LoginForm.rightsLevel.Medium)
            {
                rowNumber++;
                ActorRelImagesLabel.Location = new Point(0, (rowNumber * 23) + 9);
                ActorRelImagesLabel.Size = new Size(90, 13);
                addRelActorImagesBtn.Location = new Point(0, (rowNumber * 23) + 7);
                addRelActorImagesBtn.Size = new Size(90, 22);
                ActorRelImageComboBox.Location = new Point(103, (rowNumber * 23) + 7);
                ActorRelImageComboBox.Size = new Size((int)(2 * (width - 109) / 3), 21);
                ViewSelActorRelImageButton.Location = new Point(103 + (int)(2 * (width - 109) / 3), (rowNumber * 23) + 6);
                ViewSelActorRelImageButton.Size = new Size((int)((width - 109) / 3), 22);
            }
            #endregion
            #region rotDir
            if (linwin.userRightsLevel > LoginForm.rightsLevel.Medium)
            {
                rowNumber++;
                updRootBtn.Location = new Point(0, (rowNumber * 23) + 7);
                updRootBtn.Size = new Size(90, 22);
                updRootBtn.Visible = true;
                rootDirTxtBx.Location = new Point(103, (rowNumber * 23) + 7);
                rootDirTxtBx.Size = new Size(width - 109, 21);
                rootDirTxtBx.Visible = false;
                cmbBoxRootDir.Location = new Point(103, (rowNumber * 23) + 7);
                cmbBoxRootDir.Size = new Size(width - 109, 21);
                cmbBoxRootDir.Visible = true;
            }
            #endregion
            #region Save-Discard_buttons
            rowNumber++;
            SaveActorDataChangesButton.Location = new Point(0, (rowNumber * 23) + 7);
            SaveActorDataChangesButton.Size = new Size((int)(width / 2), 30);
            DiscardActorDataChangesButton.Location = new Point((int)(width / 2), (rowNumber * 23) + 7);
            DiscardActorDataChangesButton.Size = new Size((int)(width / 2), 30);
            #endregion
            ActorData.Refresh();
        }
        private void HandleEventDataTabGui(int width, int height)
        {
            EventData.Size = new Size(width, height);
            width -= 20;
            int dataWidth = (int)(2 * width / 3);
            int dataHalfWidth = (int)(dataWidth / 2);
            int rowNumber = 0;
            #region EventIdentity
            EventIdLabel.Location = new Point(0, (rowNumber * 23) + 3);
            EventIdLabel.Size = new Size(71, 13);
            ActiveEventTextBox.Location = new Point(106, (rowNumber * 23));
            ActiveEventTextBox.Size = new Size(dataWidth, 20);
            activeEventComboBox.Location = new Point(106, (rowNumber * 23));
            activeEventComboBox.Size = new Size(dataWidth, 21);
            #endregion
            #region Copyright
            rowNumber++;
            CopyrightLabel.Location = new Point(0, (rowNumber * 23) + 3);
            CopyrightLabel.Size = new Size(103, 13);
            CopyrightTextBox.Location = new Point(106, (rowNumber * 23) + 1);
            CopyrightTextBox.Size = new Size(dataWidth, 20);
            #endregion
            #region EventSecrecy
            if (linwin.userRightsLevel > LoginForm.rightsLevel.Medium)
            {
                rowNumber++;
                EventSecrLevelLabel.Location = new Point(0, (rowNumber * 23) + 3);
                EventSecrLevelLabel.Size = new Size(103, 13);
                EventSecrecyLevelTextBox.Location = new Point(106, (rowNumber * 23) + 1);
                EventSecrecyLevelTextBox.Size = new Size(dataWidth, 20);
            }
            #endregion
            #region EventStartEnd
            rowNumber++;
            EventStartEndLabel.Location = new Point(0, (rowNumber * 23) + 3);
            EventStartEndLabel.Size = new Size(103, 13);
            EventStartTextBox.Location = new Point(106, (rowNumber * 23) + 1);
            EventStartTextBox.Size = new Size(dataHalfWidth, 20);
            EventEndTextBox.Location = new Point(105 + dataHalfWidth, (rowNumber * 23) + 1);
            EventEndTextBox.Size = new Size(dataHalfWidth, 20);
            #endregion
            #region EventHeadline
            if (linwin.userRightsLevel > LoginForm.rightsLevel.Medium)
            {
                rowNumber++;
                EventHeadlineLabel.Location = new Point(0, (rowNumber * 23) + 3);
                EventHeadlineLabel.Size = new Size(103, 13);
                EventHeadlineTextBox.Location = new Point(106, (rowNumber * 23) + 1);
                EventHeadlineTextBox.Size = new Size(dataWidth, 64);
            }
            #endregion
            #region GeoPos
            rowNumber++;
            rowNumber++;
            rowNumber++;
            EventGeoPosLabel.Location = new Point(0, (rowNumber * 23) + 1);
            EventGeoPosLabel.Size = new Size(103, 13);
            EventLatitudeTextBox.Location = new Point(106, (rowNumber * 23) - 1);
            EventLatitudeTextBox.Size = new Size(dataHalfWidth, 20);
            EventLongitudeTextBox.Location = new Point(105 + dataHalfWidth, (rowNumber * 23) - 1);
            EventLongitudeTextBox.Size = new Size(dataHalfWidth, 20);
            #endregion
            #region GeographName
            rowNumber++;
            GeoNameLabel.Location = new Point(0, (rowNumber * 23) + 1);
            GeoNameLabel.Size = new Size(103, 13);
            EventGeographNameTextBox.Location = new Point(106, (rowNumber * 23) - 1);
            EventGeographNameTextBox.Size = new Size(dataWidth, 20);
            #endregion
            #region EventAdress
            rowNumber++;
            EventAddressLabel.Location = new Point(0, (rowNumber * 23) + 1);
            EventAddressLabel.Size = new Size(103, 13);
            EventStreetnameNumberTextBox.Location = new Point(106, (rowNumber * 23) - 1);
            EventStreetnameNumberTextBox.Size = new Size(dataWidth, 20);
            rowNumber++;
            EventZipCodeTextBox.Location = new Point(106, (rowNumber * 23) - 1);
            EventZipCodeTextBox.Size = new Size(dataHalfWidth, 20);
            EventAreanameTextBox.Location = new Point(105 + dataHalfWidth, (rowNumber * 23) - 1);
            EventAreanameTextBox.Size = new Size(dataHalfWidth, 20);
            rowNumber++;
            EventCitynameTextBox.Location = new Point(106, (rowNumber * 23) - 1);
            EventCitynameTextBox.Size = new Size(dataWidth, 20);
            rowNumber++;
            EventCountrynameTextBox.Location = new Point(106, (rowNumber * 23) - 1);
            EventCountrynameTextBox.Size = new Size(dataWidth, 20);
            #endregion
            #region EventAttender
            if (linwin.userRightsLevel > LoginForm.rightsLevel.Medium)
            {
                rowNumber++;
                EventAttenderIDComboBox1.Location = new Point(0, (rowNumber * 23) - 1);
                EventAttenderIDComboBox1.Size = new Size(103, 21);
                EventAttenderNaneTextBox.Location = new Point(106, (rowNumber * 23) - 1);
                EventAttenderNaneTextBox.Size = new Size(dataWidth, 20);
            }
            #endregion
            #region ViewAttender
            if (linwin.userRightsLevel > LoginForm.rightsLevel.Medium)
            {
                rowNumber++;
                ViewAttenderAsActorButton.Location = new Point(106, (rowNumber * 23) - 1);
                ViewAttenderAsActorButton.Size = new Size(dataWidth, 20);
            }
            #endregion
            #region EventImage
            if (linwin.userRightsLevel > LoginForm.rightsLevel.Medium)
            {
                rowNumber++;
                EventImageNameComboBox.Location = new Point(0, (rowNumber * 23) - 1);
                EventImageNameComboBox.Size = new Size(103, 21);
                EventImageLevelTextBox.Location = new Point(106, (rowNumber * 23) - 1);
                EventImageLevelTextBox.Size = new Size((int)(dataWidth - 104), 20);
                ViewSelEventImageButton.Location = new Point(106 + (int)(dataWidth - 104), (rowNumber * 23) - 1);
                ViewSelEventImageButton.Size = new Size(104, 20);
            }
            #endregion
            rowNumber++;
            EventDescriptionTextBox.Location = new Point(0, (rowNumber * 23) + 1);
            EventDescriptionTextBox.Size = new Size(width, 122);
            SaveEventDataChangesButton.Location = new Point(0, height - 28);
            SaveEventDataChangesButton.Size = new Size((int)(width / 2) - 1, 27);
            DiscardEventDataChangesButton.Location = new Point((int)(width / 2) + 1, height - 28);
            DiscardEventDataChangesButton.Size = new Size((int)(width / 2) - 1, 27);
            EventData.Refresh();
        }
        private void HandleSortingTabGui(int width, int height)
        {
            sortingTabPage.Size = new Size(width, height);
            #region sortingButton1
            int rowNumber = 0;
            sortingButton1.Location = new Point(6, (rowNumber * 29) + 6);
            sortingButton1.Size = new Size(width - 12, 23);
            #endregion
            #region sortingButton2
            rowNumber++;
            sortingButton2.Location = new Point(6, (rowNumber * 29) + 6);
            sortingButton2.Size = new Size(width - 12, 23);
            #endregion
            #region sortingButton3
            rowNumber++;
            sortingButton3.Location = new Point(6, (rowNumber * 29) + 6);
            sortingButton3.Size = new Size(width - 12, 23);
            #endregion
            #region sortingButton4
            rowNumber++;
            sortingButton4.Location = new Point(6, (rowNumber * 29) + 6);
            sortingButton4.Size = new Size(width - 12, 23);
            #endregion
            #region sortingButton5
            rowNumber++;
            sortingButton5.Location = new Point(6, (rowNumber * 29) + 6);
            sortingButton5.Size = new Size(width - 12, 23);
            #endregion
            #region sortingButton6
            rowNumber++;
            sortingButton6.Location = new Point(6, (rowNumber * 29) + 6);
            sortingButton6.Size = new Size(width - 12, 23);
            #endregion
            #region sortingButton7
            rowNumber++;
            sortingButton7.Location = new Point(6, (rowNumber * 29) + 6);
            sortingButton7.Size = new Size(width - 12, 23);
            #endregion
            #region sortingButton8
            rowNumber++;
            sortingButton8.Location = new Point(6, (rowNumber * 29) + 6);
            sortingButton8.Size = new Size(width - 12, 23);
            #endregion
            #region sortingButton9
            rowNumber++;
            sortingButton9.Location = new Point(6, (rowNumber * 29) + 6);
            sortingButton9.Size = new Size(width - 12, 23);
            #endregion
            #region sortingButton10
            rowNumber++;
            sortingButton10.Location = new Point(6, (rowNumber * 29) + 6);
            sortingButton10.Size = new Size(width - 12, 23);
            #endregion
            #region sortingButton11
            rowNumber++;
            sortingButton11.Location = new Point(6, (rowNumber * 29) + 6);
            sortingButton11.Size = new Size(width - 12, 23);
            #endregion
            #region sortingButton12
            rowNumber++;
            sortingButton12.Location = new Point(6, (rowNumber * 29) + 6);
            sortingButton12.Size = new Size(width - 12, 23);
            #endregion

            sortingTabPage.Refresh();
        }
        private void HandleRecoverTabGui(int xp, int width, int height)
        {
            recoverTab.Size = new Size(width, height);
            int dataWidth = (int)((2 * (width / 3)) - 10);
            int dataHalfWidth = (int)(dataWidth / 2);
            #region recLvl
            int rowNumber = 0;
            recLvlLbl.Location = new Point(xp + 3, (rowNumber * 23) + 6);
            recLvlLbl.Size = new Size(78, 13);
            recoverSelCmbBx.Location = new Point(106, (rowNumber * 23) + 3);
            recoverSelCmbBx.Size = new Size(dataWidth, 21);
            #endregion
            #region recoverType
            rowNumber++;
            recoverTypeLbl.Location = new Point(xp + 3, (rowNumber * 23) + 6);
            recoverTypeLbl.Size = new Size(103, 13);
            recoverTypecmbBx.Location = new Point(106, (rowNumber * 23) + 4);
            recoverTypecmbBx.Size = new Size(dataWidth, 21);
            #endregion
            #region searchPhrase
            rowNumber++;
            searchPhraseLbl.Location = new Point(xp + 3, (rowNumber * 23) + 6);
            searchPhraseLbl.Size = new Size(103, 13);
            searchPhraseTxtBx.Location = new Point(106, (rowNumber * 23) + 5);
            searchPhraseTxtBx.Size = new Size(dataWidth, 20);
            #endregion
            #region startRecoveryBtn
            rowNumber++;
            startRecoveryBtn.Location = new Point(xp + 3, (rowNumber * 23) + 6);
            startRecoveryBtn.Size = new Size(width - 6, 22);
            #endregion
            #region toRecoveryListing
            rowNumber++;
            RetToListBtn.Location = new Point(xp + 3, (rowNumber * 23) + 9);
            RetToListBtn.Size = new Size(width - 3, 22);
            #endregion
            recoverTab.Refresh();
        }
        private void HandleSizeChange(object sender, EventArgs e)
        {
            if (startUpDone)
            {
                int mainWinWidth = 0;
                int mainWinHeight = 0;
                if (!maxSizeSet)
                {
                    mainWinWidth = this.Size.Width - 20;
                    mainWinHeight = this.Size.Height - 25;
                    setInformationText("Setting toolsize to (" + mainWinWidth.ToString() + "x" + mainWinHeight.ToString() + ").", informationType.INFO, sender, e);
                    this.Size = new Size(mainWinWidth, mainWinHeight);
                    pictureName.Location = new Point(mainWinWidth - 335, 26);
                    pictureName.Size = new Size(335, 20);
                    pictureName.Update();
                    // --- Tab handling ---
                    tabControl.Location = new Point(mainWinWidth - 335, 53);
                    tabControl.Size = new Size(330, mainWinHeight - 100);
                    HandleGenDataTabGui(330, mainWinHeight - 100);
                    HandleHWDataTabGui(330, mainWinHeight - 100);
                    HandleHWAddonDataTabGui(330, mainWinHeight - 100);
                    HandleGeoDataTabGui(330, mainWinHeight - 100);
                    HandleActorDataTabGui(330, mainWinHeight - 100);
                    HandleEventDataTabGui(330, mainWinHeight - 100);
                    HandleSortingTabGui(330, mainWinHeight - 100);

                    HandleRecoverTabGui(0, 330, mainWinHeight - 100);
                    tabControl.Refresh();
                    pictureCanvas.Location = new Point(5, 26);
                    pictureCanvas.Size = new Size(mainWinWidth - 349, mainWinHeight - 75);
                    listView.Size = new Size(mainWinWidth - 349, mainWinHeight - 75);
                    informationTextBox.Location = new Point(8, mainWinHeight - 44);
                    informationTextBox.Size = new Size(mainWinWidth - 15, 30);
                    this.Update();
                }
            }
        }
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
                        informationTextBox.Text = System.DateTime.Now.ToString("yyyy-MM-dd - HH:mm:ss.ff") + " : Information : " + inString;
                        using (StreamWriter sw = File.AppendText(logPath))
                            sw.WriteLine(System.DateTime.Now.ToString("yyyy-MM-dd - HH:mm:ss.ff") + " : ERROR : " + inString);
                    }
                    break;
                case informationType.FATAL:
                    {
                        string infoText = System.DateTime.Now.ToString("yyyy-MM-dd - HH:mm:ss.ff") + "\nFATAL ERROR : " + inString + "\nOrigin : " + sender.ToString();
                        if (MessageBox.Show(infoText, "Fatal Error", MessageBoxButtons.OK, MessageBoxIcon.Error) == DialogResult.OK)
                        {
                            CloseDown(sender, e);
                        }
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
        private void informationTextBox_MouseHover(object sender, EventArgs e)
        {
            tt.Show("Program operative information.", (Control)sender);
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
            if ((actorClass != null) && (toSave.actorChanges))
            {
                actorClass.saveActorData(actorClass.getUserId(), linwin.getActorStoragePath());
                toSave.actorChanges = false;
            }
            if ((eventClass != null) && (toSave.eventChanges))
            {
                eventClass.saveEvent(eventClass.getEventID(), linwin.getEventStoragePath());
                toSave.eventChanges = false;
            }
            if ((picture != null) && (toSave.imageChanges))
            {
                var result = MessageBox.Show("Save changes to the image?", "Existing changes", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if ((result == DialogResult.Yes) && (loadedImageFileName != null))
                    try { picture.Save(loadedImageFileName); } catch { }
                if (picture != null)
                    picture.Dispose();
            }
            if ((linwin != null) && (toSave.userChanges))
            {
                linwin.saveUserData();
            }

            setInformationText("Closing the program", informationType.INFO, sender, e);
//            try { linwin.saveUserData(); } catch { setInformationText("Cought an exeption when saving user data.", informationType.ERROR, sender, e); }
            //try { LoginForm.Close}
            try { Dispose(); } catch { setInformationText("Cought an exeption when disposing.", informationType.ERROR, sender, e); }
            try { Close(); } catch { setInformationText("Cought an exeption when closing the program.", informationType.ERROR, sender, e); }
        }
        private void button1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                enterPressed = true;
            else
                enterPressed = false;
        }
        private void getImageMetadataValues(string instr, object sender, EventArgs e)
        {
            var directories = ImageMetadataReader.ReadMetadata(instr);
                activeEventComboBox.Items.Add("Select...");
                AttdEventsDateComboBox.Items.Add("Select...");
                AttEvtIDCmbBx.Items.Add("Select...");
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
                                    oldImageOrientation = tag.Description;
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
                                    // tagName = "Camera Owner Name"
                                    // tagType = 42032
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
                                            AttdEventsDateComboBox.Items.Add(currEvent);
                                            AttEvtIDCmbBx.Items.Add(currEvent);
                                            exiEvents = exiEvents.Substring(delimPos + 1, exiEvents.Length - delimPos - 1);
                                            delimPos = exiEvents.IndexOf(";");
                                            if ((delimPos > 0) && (delimPos < exiEvents.Length - 1))
                                                currEvent = exiEvents.Substring(0, delimPos);
                                            else
                                                currEvent = exiEvents;
                                        }
                                        activeEventComboBox.Items.Add(currEvent);
                                        AttdEventsDateComboBox.Items.Add(currEvent);
                                        AttEvtIDCmbBx.Items.Add(currEvent);
                                    }
                                    else
                                    {
                                        activeEventComboBox.Items.Add(exiEvents);
                                        AttdEventsDateComboBox.Items.Add(exiEvents);
                                        AttEvtIDCmbBx.Items.Add(exiEvents);
                                    }
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
                ActiveArtistsComboBox.Items.Add("Add Item...");
                ActiveArtistsComboBox.Visible = true;
                AttdEventsDateComboBox.Items.Add("Add Item...");
                AttdEventsDateComboBox.Visible = true;
                AttEvtIDCmbBx.Items.Add("Add Item...");
                AttEvtIDCmbBx.Visible = false;
                if (AttdEventsDateComboBox.Items.Count == 3)
                {
                    AttdEventsDateComboBox.SelectedIndex = 1;
                    AttEvtIDCmbBx.SelectedIndex = 1;
                    EventIdTextBox.Text = AttdEventsDateComboBox.SelectedItem.ToString();
                }
                else
                {
                    AttdEventsDateComboBox.SelectedIndex = 0;
                    AttEvtIDCmbBx.SelectedIndex = 0;
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

                    if (numberOfLoadedEvents < maxNoOfLoadedEvents)
                    {
                        arrayOfEvents[numberOfLoadedEvents] = eventClass;
                    }
                    else
                    {
                        arrayOfEvents[0].saveEvent(activeEventComboBox.SelectedItem.ToString(), storpa);
                        for (int i = 0; i < numberOfLoadedEvents; i++)
                            arrayOfEvents[i] = arrayOfEvents[i + 1];
                        arrayOfEvents[numberOfLoadedEvents] = eventClass;
                    }
                    eventToShow = numberOfLoadedEvents++;
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
                    EventAttenderIDComboBox1.Items.Add("Select attender...");
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
                    EventImageNameComboBox.Items.Add("Select image...");
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
            AttEvtIDCmbBx.Items.Clear();
            ChangeDateTimeTextBox.Text = "";
            noOfComponentsTextBox.Text = "";
            detFileTypeName = "";
            EventAttenderIDComboBox1.Items.Clear();
            EventImageNameComboBox.Items.Clear();
        }
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (currentMode == programMode.SORTINGVIEW)
            {
                if (sortingButton1.Visible && e.KeyCode == Keys.F1)
                    sortingButton1_Click(sender, e);
                else if (sortingButton2.Visible && e.KeyCode == Keys.F2)
                    sortingButton2_Click(sender, e);
                else if (sortingButton3.Visible && e.KeyCode == Keys.F3)
                    sortingButton3_Click(sender, e);
                else if (sortingButton4.Visible && e.KeyCode == Keys.F4)
                    sortingButton4_Click(sender, e);
                else if (sortingButton5.Visible && e.KeyCode == Keys.F5)
                    sortingButton5_Click(sender, e);
                else if (sortingButton6.Visible && e.KeyCode == Keys.F6)
                    sortingButton6_Click(sender, e);
                else if (sortingButton7.Visible && e.KeyCode == Keys.F7)
                    sortingButton7_Click(sender, e);
                else if (sortingButton8.Visible && e.KeyCode == Keys.F8)
                    sortingButton8_Click(sender, e);
                else if (sortingButton9.Visible && e.KeyCode == Keys.F9)
                    sortingButton9_Click(sender, e);
                else if (sortingButton10.Visible && e.KeyCode == Keys.F10)
                    sortingButton10_Click(sender, e);
                else if (sortingButton11.Visible && e.KeyCode == Keys.F11)
                    sortingButton11_Click(sender, e);
                else if (sortingButton12.Visible && e.KeyCode == Keys.F12)
                    sortingButton12_Click(sender, e);
            }
            else if (currentMode == programMode.IMAGEVIEW)
            {
                if (e.KeyCode == Keys.Escape)
                    pictureCanvas.Cursor = Cursors.Hand;
            }
        }
        #endregion

        #region Main menu items
        // --- Main menu items ---
        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            // Note! Intentionally left empty, clicking the menu-strip should not render any action.
        }
        #region File_MenuItem
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            iAmWorking = true;
            this.UseWaitCursor = true;
            if (toSave.imageChanges)
            {
                var result = MessageBox.Show("Save changes to the image?", "Existing changes", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                    try { picture.Save(loadedImageFileName); } catch { }
            }
            oldImageOrientation = "";
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
                if ((picture.Width > pictureCanvas.Width) || (picture.Height > pictureCanvas.Height))
                    pictureCanvas.SizeMode = PictureBoxSizeMode.Zoom;
                else
                    pictureCanvas.SizeMode = PictureBoxSizeMode.Normal;
                getImageMetadataValues(ofd.FileName, sender, e);
                pictureCanvas.Image = picture;
                pictureName.Text = ofd.FileName;
                startUpDone = true;
                expandedImage = true;
                saveAsToolStripMenuItem.Enabled = true;
                currentMode = programMode.IMAGEVIEW;
                tabControl.SelectedTab = this.tabControl.TabPages["GenData"];
            }
            ofd.Dispose();
            imageViewToolStripMenuItem_Click(sender, e);
            UseWaitCursor = false;
            iAmWorking = false;
        }
        private void openDirectoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            currentMode = programMode.DIRECTORYVIEW;
            imageSortingToolStripMenuItem_Click(sender, e);
        }
        private void restoreToolStripMenuItem_Click(object sender, EventArgs e)
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
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (toSave.imageChanges)
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
                    toSave.imageChanges = false;
                }
                if (toSave.actorChanges)
                {
                    string sSavePath = linwin.getActorStoragePath() + "\\ActorData_" + actorClass.getUserId() + ".acf";
                    if (!(actorClass.saveActorData(actorClass.getUserId(), sSavePath)))
                    {
                        setInformationText("Could not save actor data.", informationType.ERROR, sender, e);
                        return;
                    }
                    else
                        toSave.actorChanges = false;
                }
                if (toSave.eventChanges)
                {
                    string sSavePath = linwin.getEventStoragePath() + "\\" + eventClass.getEventID() + ".edf";
                    if (!(eventClass.saveEvent(eventClass.getEventID(), sSavePath)))
                    {
                        setInformationText("Could not save event data.", informationType.ERROR, sender, e);
                    }
                    else
                        toSave.eventChanges = false;
                }
                if (toSave.userChanges)
                {
                    linwin.saveUserData();
                    toSave.userChanges = false;
                }

                saveToolStripMenuItem.Enabled = false;
                saveAsToolStripMenuItem.Enabled = false;
            }
            catch (Exception err)
            {
                setInformationText("Saving file " + loadedImageFileName + " failed! " + Environment.NewLine + err.ToString(), informationType.ERROR, sender, e);
            }
        }
        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {

            SaveFileDialog sfd = new SaveFileDialog();
            if (toSave.imageChanges)
            {
                sfd.Filter = "Text files (*.txt)|*.txt|" +
                             "Document files (*.doc)|*.doc|" +
                             "xml files (*.xml)|*.xml|" +
                             "jpg files (*.jpg)|*.jpg|" +
                             "gif files (*.gif)|*.gif|" +
                             "bmp files (*.bmp)|*.bmp|" +
                             "mpg files (*.mpg)|*.mpg|" +
                             "mp3 files (*.mp3)|*.mp3|" +
                             "All files (*.*)|*.*";
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
                                }
                                break;
                            case "emf":
                                {
                                    theNewFile.Save(sfd.FileName.ToString(), ImageFormat.Emf);
                                    managedToSave = true;
                                }
                                break;
                            case "exif":
                                {
                                    theNewFile.Save(sfd.FileName.ToString(), ImageFormat.Exif);
                                    managedToSave = true;
                                }
                                break;
                            case "gif":
                                {
                                    theNewFile.Save(sfd.FileName.ToString(), ImageFormat.Gif);
                                    managedToSave = true;
                                }
                                break;
                            case "icon":
                                {
                                    theNewFile.Save(sfd.FileName.ToString(), ImageFormat.Icon);
                                    managedToSave = true;
                                }
                                break;
                            case "jpeg":
                            case "jpg":
                                {
                                    theNewFile.Save(sfd.FileName.ToString(), ImageFormat.Jpeg);
                                    managedToSave = true;
                                }
                                break;
                            case "png":
                                {
                                    theNewFile.Save(sfd.FileName.ToString(), ImageFormat.Png);
                                    managedToSave = true;
                                }
                                break;
                            case "tiff":
                                {
                                    theNewFile.Save(sfd.FileName.ToString(), ImageFormat.Tiff);
                                    managedToSave = true;
                                }
                                break;
                            case "wmf":
                                {
                                    theNewFile.Save(sfd.FileName.ToString(), ImageFormat.Wmf);
                                    managedToSave = true;
                                }
                                break;
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
                            toSave.imageChanges = true;
                            saveToolStripMenuItem.Enabled = false;
                            saveAsToolStripMenuItem.Enabled = false;
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
            if (toSave.actorChanges)
            {
                sfd.Filter = "Actor data files (*.acf)|*.acf|" +
                             "Txt files (*.txt)|*.txt|" +
                             "All files (*.*)|*.*";
                sfd.FilterIndex = 3;
                sfd.RestoreDirectory = true;
                if ((sfd.ShowDialog() == DialogResult.OK) && (actorClass.getUserId() != null))
                {
                    if (!(actorClass.saveActorData(actorClass.getUserId(), sfd.FileName.ToString())))
                    {
                        setInformationText("Failed to save actor data!", informationType.ERROR, sender, e);
                        return;
                    }
                    toSave.actorChanges = false;
                }
                else
                {
                    setInformationText("Failed to save actor data!", informationType.ERROR, sender, e);
                }
                sfd.Dispose();
            }
            if (toSave.eventChanges)
            {
                sfd.Filter = "Event data files (*.edf)|*.edf|" +
                             "Text files (*.txt)|*.txt|" +
                             "All files (*.*)|*.*";
                sfd.FilterIndex = 3;
                sfd.RestoreDirectory = true;
                if ((sfd.ShowDialog() == DialogResult.OK) && (eventClass.getEventID() != null) && (eventClass.getEventID() != ""))
                {
                    if (!(eventClass.saveEvent(eventClass.getEventID(), sfd.FileName.ToString())))
                    {
                        setInformationText("Failed to save event data!", informationType.ERROR, sender, e);
                        return;
                    }
                    toSave.eventChanges = false;
                }
                else
                {
                    setInformationText("Failed to save event data!", informationType.ERROR, sender, e);
                }
                sfd.Dispose();
            }
            if (toSave.userChanges)
            {
                sfd.Filter = "User data files (*.acf)|*.acf|" +
                             "Text files (*.txt)|*.txt|" +
                             "All files (*.*)|*.*";
                sfd.FilterIndex = 3;
                sfd.RestoreDirectory = true;
                if ((sfd.ShowDialog() == DialogResult.OK) && (linwin.userId != null) && (linwin.userId != ""))
                {
                    linwin.saveUserData();
                }
                sfd.Dispose();
                toSave.userChanges = false;
            }
        }
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if ((toSave.imageChanges) || (toSave.actorChanges) || (toSave.eventChanges) || (toSave.userChanges))
                saveToolStripMenuItem_Click(sender, e);
            CloseDown(sender, e);
        }
        #endregion
        #region Mode_MenuItem
        private void imageViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.currentMode = programMode.IMAGEVIEW;
            setInformationText("Entered image view mode.", informationType.INFO, sender, e);
            this.pictureCanvas.Visible = true;
            this.listView.Visible = false;
            this.tabControl.Visible = true;
        }
        private void imageSortingToolStripMenuItem_Click(object sender, EventArgs e)
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
        }
        private void imageRestoringToolStripMenuItem_Click(object sender, EventArgs e)
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
        private void actorViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            currentMode = programMode.ACTORVIEW;
            setInformationText("Entered actor data view mode.", informationType.INFO, sender, e);
            pictureCanvas.Visible = true;
            listView.Visible = false;
            tabControl.Visible = true;
            tabControl.SelectedTab = this.tabControl.TabPages["ActorData"];
            if ((linwin.getActorStoragePath() != "") && (System.IO.Directory.Exists(linwin.getActorStoragePath())))
                actorFilePaths = System.IO.Directory.GetFiles(linwin.getActorStoragePath(), "ActorData_*.acf");
            else if (System.IO.Directory.Exists(rootPath + "\\ActorData\\"))
                actorFilePaths = System.IO.Directory.GetFiles(rootPath + "\\ActorData\\", "ActorData_*.acf");
            else
                return;
            ActiveArtistsComboBox.Items.Clear();
            ActiveArtistsComboBox.Items.Add("Select artist");
            if (actorFilePaths.Length > 0)
            {
                foreach (var afp in actorFilePaths)
                {
                    int dp = afp.LastIndexOf("\\");
                    this.ActiveArtistsComboBox.Items.Add(afp.Substring(dp + 1, afp.Length - dp - 1));
                }
            }
            else
                setInformationText("No artist files found at destination.", informationType.ERROR, sender, e);
            this.ActiveArtistsComboBox.Items.Add("Add artist...");
        }
        private void eventViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ActiveEventTextBox.Visible = false;
            setInformationText("Method \"eventViewToolStripMenuItem_Click\" entered.", informationType.INFO, sender, e);
            currentMode = programMode.EVENTVIEW;
            pictureCanvas.Visible = true;
            listView.Visible = false;
            tabControl.Visible = true;
            tabControl.SelectedTab = this.tabControl.TabPages["EventData"];
            //string sasp = linwin.getActorStoragePath();
            string sasp = linwin.getEventStoragePath();
            if ((sasp != "") && (System.IO.Directory.Exists(sasp)))
                eventFilePaths = System.IO.Directory.GetFiles(sasp + "\\", "EventData_*.edf");
            else
            {
                if ((rootPath != "") && (System.IO.Directory.Exists(rootPath)) && (System.IO.Directory.Exists(rootPath + "\\EventData")))
                    eventFilePaths = System.IO.Directory.GetFiles(rootPath + "\\EventData\\", "EventData_*.edf");
                else
                {
                    System.IO.Directory.CreateDirectory(rootPath);
                    System.IO.Directory.CreateDirectory(rootPath + "\\EventData");
                    toSave.userChanges = true;
                }
            }
            activeEventComboBox.Items.Clear();
            activeEventComboBox.Items.Add("Select...");
            foreach (var efp in eventFilePaths)
            {
                int dp0 = efp.LastIndexOf("\\");
                string stef = efp.Substring(dp0 + 1, efp.Length - dp0 - 1);
                activeEventComboBox.Items.Add(stef);
                noOfEvents++;
            }
            activeEventComboBox.Items.Add("Add event...");
            activeEventComboBox.Visible = true;
            activeEventComboBox.Enabled = true;
        }
        private void userViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            currentMode = programMode.USERVIEW;
            setInformationText("Entered user view mode.", informationType.INFO, sender, e);
            pictureCanvas.Visible = false;
            listView.Visible = true;
            linwin.startInEditMode();
        }
        #endregion
        #region Edit_MenuItem
        private void degRightToolStripMenuItem_Click(object sender, EventArgs e)
        {
            picture.RotateFlip(RotateFlipType.Rotate90FlipNone);
            pictureCanvas.Image = picture;
            saveToolStripMenuItem.Enabled = true;
        }
        private void degLeftToolStripMenuItem_Click(object sender, EventArgs e)
        {
            picture.RotateFlip(RotateFlipType.Rotate270FlipNone);
            pictureCanvas.Image = picture;
            saveToolStripMenuItem.Enabled = true;
        }
        private void degToolStripMenuItem_Click(object sender, EventArgs e)
        {
            picture.RotateFlip(RotateFlipType.Rotate180FlipNone);
            pictureCanvas.Image = picture;
            saveToolStripMenuItem.Enabled = true;
        }
        private void xdirectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            picture.RotateFlip(RotateFlipType.RotateNoneFlipX);
            pictureCanvas.Image = picture;
            saveToolStripMenuItem.Enabled = true;
        }
        private void ydirectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            picture.RotateFlip(RotateFlipType.RotateNoneFlipY);
            pictureCanvas.Image = picture;
            saveToolStripMenuItem.Enabled = true;
        }
        private void graphToolStripMenuItem_Click(object sender, EventArgs e)
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
                pictureCanvas.SizeMode = PictureBoxSizeMode.Normal;//Zoom;
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
                pictureCanvas.SizeMode = PictureBoxSizeMode.Normal;//.Zoom;
                pictureCanvas.Image = bmp;
                expandedImage = true;
                saveAsToolStripMenuItem.Enabled = true;
                saveToolStripMenuItem.Enabled = true;
            }
            else
                setInformationText("Need to have a loaded file to work with.", informationType.INFO, sender, e);
            UseWaitCursor = false;
        }
        #endregion
        #endregion

        #region Restore Directory Handling
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
            // TODO - Recovering deleted files not implemented yet.
            // See: https://stackoverflow.com/questions/8819188/c-sharp-classes-to-undelete-files
            // -------------------------------------------------------------
            string tempOutStr = "Trying to find deleted files";
            if (startDir != "")
            {
                if (searchPhrase != "")
                {
                    tempOutStr = tempOutStr + " from " + startDir + " directory containing " + searchPhrase + ".";
                }
                else
                {
                    tempOutStr = tempOutStr + " from " + startDir + " directory.";
                }
            }
            else
            {
                if (searchPhrase != "")
                {
                    tempOutStr = tempOutStr + " containing phrase " + searchPhrase + ".";
                }
                else
                {
                    tempOutStr = tempOutStr + ".";
                }
            }
            setInformationText(tempOutStr, informationType.INFO, sender, e);
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
        private void recoverSelCmbBx_SelectedIndexChanged(object sender, EventArgs e)
        {
            recovLevel = Math.Max(recoverSelCmbBx.SelectedIndex, 0);
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
            recovType = Math.Max(recoverTypecmbBx.SelectedIndex, 0);
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
        #endregion
        #region Image View Handling
        // --- Image View handling ---
        private void pictureName_MouseHover(object sender, EventArgs e)
        {
            tt.Show("Currently loaded image name.", (Control)sender);
        }
        void pictureCanvas_ClickDown(object sender, MouseEventArgs e)
        {
            panning = true;
            pictureCanvas.Cursor = Cursors.Hand;
            startingPoint = new Point(e.Location.X - movingPoint.X, e.Location.Y - movingPoint.Y);
            setInformationText("Panning started at position X=" + e.Location.X.ToString() + " and Y=" + e.Location.Y.ToString() + ".", informationType.INFO, sender, e);
        }
        void pictureCanvas_ClickUp(object sender, MouseEventArgs e)
        {
            panning = false;
            pictureCanvas.Cursor = Cursors.Default;
            setInformationText("Panning ended at position X=" + e.Location.X.ToString() + " and Y=" + e.Location.Y.ToString() + ".", informationType.INFO, sender, e);
        }
        void pictureCanvas_MouseLeave(object sender, EventArgs e)
        {
            panning = false;
            pictureCanvas.Cursor = Cursors.Default;
            setInformationText("Mouse left the canvas area!", informationType.INFO, sender, e);
        }
        void pictureCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            // TODO - This does not work!
            if ((picture != null) && (panning))
            {
                movingPoint = new Point(e.Location.X - startingPoint.X, e.Location.Y - startingPoint.Y);
                pictureCanvas.Invalidate();
                setInformationText("Mouse move on canvas, dX=" + movingPoint.X.ToString() + " and cY=" + movingPoint.Y.ToString() + ".", informationType.INFO, sender, e);
            }
        }
        void pictureCanvas_Paint(object sender, PaintEventArgs e)
        {
            // TODO - This does not work!
            e.Graphics.Clear(Color.White);
            e.Graphics.DrawImage(picture, movingPoint);
        }
        private void pictureCanvas_MouseWheel(object sender, MouseEventArgs e)
        {
            if (pictureCanvas.Image != null)
            {
                picture = Image.FromFile(loadedImageFileName);
                int originalImageHeight = picture.Height;
                int originalImageWidth = picture.Width;
                startingPoint = new Point(e.Location.X, e.Location.Y);
                if (pictureCanvas.SizeMode != PictureBoxSizeMode.Normal)
                {
                    pictureCanvas.SizeMode = PictureBoxSizeMode.Normal;
                    if ((picture.Width - pictureCanvas.Width) > (picture.Height - pictureCanvas.Height))
                        zoom = (float)pictureCanvas.Width / (float)picture.Width;
                    else
                        zoom = (float)pictureCanvas.Height / (float)picture.Height;
                }
                else
                {
                    if (e.Delta > 0)
                    {
                        //zoom += 0.01;
                        zoom *= 1.25;
                    }
                    else if (e.Delta < 0)
                    {
                        //zoom -= 0.01;
                        zoom /= 1.25;
                    }
                }
                imageZoomed = true;
                setInformationText("Zoom level :" + zoom.ToString(), informationType.INFO, sender, e);
                pictureCanvas.Image = picture;
                Bitmap zoomBmp = new Bitmap(pictureCanvas.Image, (int)(originalImageWidth * zoom), (int)(originalImageHeight * zoom));
                Graphics g = Graphics.FromImage(zoomBmp);
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBilinear;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                pictureCanvas.Image = zoomBmp;
                pictureCanvas.Update();
            }
        }
        #endregion
        #region General Information Handling
        // --- --- General information handling --- ---
        private void noOfComponentsTextBox_MouseHover(object sender, EventArgs e)
        {
            tt.Show("Number of color components.", (Control)sender);
        }
        private void subjectTextBox_MouseHover(object sender, EventArgs e)
        {
            tt.Show("Subject of the loaded image.", (Control)sender);
        }
        private void SaveGenDataChangesButton_MouseHover(object sender, EventArgs e)
        {
            tt.Show("Save image and metadata changes.", (Control)sender);
        }
        private void SaveGenDataChangesButton_Click(object sender, EventArgs e)
        {
            bool managedToSaveSomething = false;
            if (subjectChanged)
            {
                try
                {
                    // 40095 - 0x9C9F : .......... (subjectTextBox)
                    string newSubject = subjectTextBox.Text.ToString();
                    byte[] newSubjBte = Encoding.Unicode.GetBytes(newSubject);
                    try
                    {
                        PropertyItem subjItm = picture.GetPropertyItem(40095);
                        subjItm.Value = newSubjBte;
                        subjItm.Len = newSubjBte.Length + 1;
                        picture.SetPropertyItem(subjItm);
                    }
                    catch
                    {
                        var subjItm = (PropertyItem)FormatterServices.GetUninitializedObject(typeof(PropertyItem));
                        subjItm.Type = 1;
                        subjItm.Id = 40095;
                        subjItm.Value = newSubjBte;
                        subjItm.Len = subjItm.Value.Length + 1;
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
                        usrCmtItm.Len = newUsrCmtBte.Length + 1;
                        picture.SetPropertyItem(usrCmtItm);
                    }
                    catch
                    {
                        var usrCmtItm = (PropertyItem)FormatterServices.GetUninitializedObject(typeof(PropertyItem));
                        usrCmtItm.Type = 1;
                        usrCmtItm.Id = 40092;
                        usrCmtItm.Value = newUsrCmtBte;
                        usrCmtItm.Len = usrCmtItm.Value.Length + 1;
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
                        usrImgTtl.Len = newImgTtlBte.Length + 1;
                        picture.SetPropertyItem(usrImgTtl);
                    }
                    catch
                    {
                        var usrImgTtl = (PropertyItem)FormatterServices.GetUninitializedObject(typeof(PropertyItem));
                        usrImgTtl.Type = 1;
                        usrImgTtl.Id = 270;
                        usrImgTtl.Value = newImgTtlBte;
                        usrImgTtl.Len = usrImgTtl.Value.Length + 1;
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
                toSave.imageChanges = true;
                saveToolStripMenuItem_Click(sender, e);
                saveToolStripMenuItem.Enabled = false;
                SaveGenDataChangesButton.Enabled = false;
                DiscardGenDataChangesButton.Enabled = false;
            }
        }
        private void DiscardGenDataChangesButton_MouseHover(object sender, EventArgs e)
        {
            tt.Show("Reset changed information", (Control)sender);
        }
        private void DiscardGenDataChangesButton_Click(object sender, EventArgs e)
        {
            if (subjectChanged)
            {
                PropertyItem tmpPItm = picture.GetPropertyItem(40095);
                subjectTextBox.Text = tmpPItm.Value.ToString();
                subjectChanged = false;
            }
            if (userCommentChanged)
            {
                PropertyItem tmpPItm = picture.GetPropertyItem(40092);
                UserCommentTextBox.Text = tmpPItm.Value.ToString();
                userCommentChanged = false;
            }
            if (imageTitleChanged)
            {
                PropertyItem tmpPItm = picture.GetPropertyItem(270);
                ImageTitleTextBox.Text = tmpPItm.Value.ToString();
                imageTitleChanged = false;
            }
        }
        private void UserCommentTextBox_TextChanged(object sender, EventArgs e)
        {
            if (startUpDone)
            {
                SaveGenDataChangesButton.Enabled = true;
                DiscardGenDataChangesButton.Enabled = true;
                userCommentChanged = true;
                toSave.imageChanges = true;
                setInformationText($"Comment textbox changed to : \" {UserCommentTextBox.Text}\"", informationType.INFO, sender, e);
            }
        }
        private void ImageTitleTextBox_TextChanged(object sender, EventArgs e)
        {
            if (startUpDone)
            {
                SaveGenDataChangesButton.Enabled = true;
                DiscardGenDataChangesButton.Enabled = true;
                toSave.imageChanges = true;
                imageTitleChanged = true;
                setInformationText($"Image title changed to : \" {ImageTitleTextBox.Text.ToString()}\"", informationType.INFO, sender, e);
            }
        }
        private void subjectTextBox_TextChanged(object sender, EventArgs e)
        {
            if (startUpDone)
            {
                SaveGenDataChangesButton.Enabled = true;
                DiscardGenDataChangesButton.Enabled = true;
                toSave.imageChanges = true;
                subjectChanged = true;
                setInformationText($"Subject text box changed to : \" {subjectTextBox.Text.ToString()}\"", informationType.INFO, sender, e);
            }
        }
        private void ImageOrientationTextBox_TextChanged(object sender, EventArgs e)
        {
            if (!(iAmWorking))
            {
                if ((oldImageOrientation != null) && (ImageOrientationTextBox != null))
                {
                    if (ImageOrientationTextBox.Text.ToLower().Contains("rotate"))
                    {
                        if (ImageOrientationTextBox.Text.ToLower().Contains("right"))
                        {
                            picture.RotateFlip(RotateFlipType.Rotate90FlipNone);
                            pictureCanvas.Image = picture;
                            toSave.imageChanges = true;
                            saveToolStripMenuItem.Enabled = true;
                            ImageOrientationTextBox.Text = "";
                        }
                        else if (ImageOrientationTextBox.Text.ToLower().Contains("left"))
                        {
                            picture.RotateFlip(RotateFlipType.Rotate270FlipNone);
                            pictureCanvas.Image = picture;
                            toSave.imageChanges = true;
                            saveToolStripMenuItem.Enabled = true;
                            ImageOrientationTextBox.Text = "";
                        }
                        else
                            setInformationText("Define direction plese", informationType.INFO, sender, e);
                    }
                    else if (ImageOrientationTextBox.Text.ToLower().Contains("flip"))
                    {
                        if ((ImageOrientationTextBox.Text.ToLower().Contains("x")) ||
                            (ImageOrientationTextBox.Text.ToLower().Contains("horizontal")))
                        {
                            picture.RotateFlip(RotateFlipType.RotateNoneFlipX);
                            pictureCanvas.Image = picture;
                            toSave.imageChanges = true;
                            saveToolStripMenuItem.Enabled = true;
                            ImageOrientationTextBox.Text = "";
                        }
                        else if ((ImageOrientationTextBox.Text.ToLower().Contains("y")) ||
                                 (ImageOrientationTextBox.Text.ToLower().Contains("vertical")))
                        {
                            picture.RotateFlip(RotateFlipType.RotateNoneFlipY);
                            pictureCanvas.Image = picture;
                            toSave.imageChanges = true;
                            saveToolStripMenuItem.Enabled = true;
                            ImageOrientationTextBox.Text = "";
                        }
                        else
                            setInformationText("Define direction please.", informationType.INFO, sender, e);
                    }
                    else
                        setInformationText("Unknown rotation command, use 'flip', 'right' or 'left'.", informationType.INFO, sender, e);
                }
                else
                    oldImageOrientation = ImageOrientationTextBox.Text;
            }
        }
        private void ImageOrienteationTextBox_Enter(object sender, EventArgs e)
        {
            oldImageOrientation = ImageOrientationTextBox.Text;
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
        private void ContrastTextBox_MouseHover(object sender, EventArgs e)
        {
            tt.Show("Image contrast info.", (Control)sender);
        }
        private void SaturationTextBox_TextChanged(object sender, EventArgs e)
        {
            // Note! Intentionally left empty, cannot change saturation.
        }
        private void SaturationTextBox_MouseHover(object sender, EventArgs e)
        {
            tt.Show("Image saturation info.", (Control)sender);
        }
        private void ImageSizeTextBox_MouseHover(object sender, EventArgs e)
        {
            tt.Show("Image size info.", (Control)sender);
        }
        private void ExifImageHeightTextBox_MouseHover(object sender, EventArgs e)
        {
            tt.Show("Exif data for image height.", (Control)sender);
        }
        private void ExifImageWidthTextBox_MouseHover(object sender, EventArgs e)
        {
            tt.Show("Exif data for image width.", (Control)sender);
        }
        private void ColorSpaceTextBox_MouseHover(object sender, EventArgs e)
        {
            tt.Show("Image colorspace.", (Control)sender);
        }
        private void SubSecTimeTextBox_MouseHover(object sender, EventArgs e)
        {
            tt.Show("Image creation time.", (Control)sender);
        }
        private void DigSubSecTimeTextBox_MouseHover(object sender, EventArgs e)
        {
            tt.Show("Digitalisation time.", (Control)sender);
        }
        private void OrigSubSecTimeTextBox_MouseHover(object sender, EventArgs e)
        {
            tt.Show("Image original time.", (Control)sender);
        }
        #endregion
        #region Base Hardware Information Handling
        // --- --- Base Hardware information handling --- ---
        private void OwnerTextBox_TextChanged(object sender, EventArgs e)
        {
            if (startUpDone)
            {
                SaveGenHWDataChangesButton.Enabled = true;
                DiscardGenHWDataChangesButton.Enabled = true;
                toSave.imageChanges = true;
                bhsOwnerChanged = true;
                setInformationText($"Hardware owner changed to : \" {OwnerTextBox.Text}\"", informationType.INFO, sender, e);
            }
        }
        private void SaveGenHWDataChangesButton_Click(object sender, EventArgs e)
        {
            bool managedToSaveSomething = false;
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
                toSave.imageChanges = true;
                saveToolStripMenuItem.Enabled = true;
                toSave.imageChanges = true;
                SaveGenHWDataChangesButton.Enabled = false;
                DiscardGenHWDataChangesButton.Enabled = false;
            }
        }
        private void DiscardGenHWDataChangesButton_Click(object sender, EventArgs e)
        {
            PropertyItem hwOwnItm = picture.GetPropertyItem(42032);
            if (hwOwnItm.Value.ToString() != "")
                OwnerTextBox.Text = hwOwnItm.Value.ToString();
            else
                OwnerTextBox.Text = "";
            bhsOwnerChanged = false;
        }
        #endregion
        #region Geographical Information Handling
        // --- --- Geographical information handling --- ---
        private void LatValueTextBox_TextChanged(object sender, EventArgs e)
        {
            if (startUpDone)
            {
                SaveGeoDataChangesButton.Enabled = true;
                DiscardGeoDataChangesButton.Enabled = true;
                toSave.imageChanges = true;
                geoLatValueChanged = true;
                setInformationText($"Latitude value textbox changed to : \" {LatValueTextBox.Text}\"", informationType.INFO, sender, e);
            }
        }
        private void LatRefTextBox_TextChanged(object sender, EventArgs e)
        {
            if (startUpDone)
            {
                SaveGeoDataChangesButton.Enabled = true;
                DiscardGenDataChangesButton.Enabled = true;
                toSave.imageChanges = true;
                geoLatValueChanged = true;
                setInformationText($"Latitude reference textbox changed to : \" {LatRefTextBox.Text}\"", informationType.INFO, sender, e);
            }
        }
        private void LonValueTextBox_TextChanged(object sender, EventArgs e)
        {
            if (startUpDone)
            {
                SaveGeoDataChangesButton.Enabled = true;
                DiscardGenDataChangesButton.Enabled = true;
                toSave.imageChanges = true;
                geoLonValueChanged = true;
                setInformationText($"Latitude reference textbox changed to : \" {LonValueTextBox.Text}\"", informationType.INFO, sender, e);
            }
        }
        private void LonRefTextBox_TextChanged(object sender, EventArgs e)
        {
            if (startUpDone)
            {
                SaveGeoDataChangesButton.Enabled = true;
                DiscardGenDataChangesButton.Enabled = true;
                toSave.imageChanges = true;
                geoLonValueChanged = true;
                setInformationText($"Latitude reference textbox changed to : \" {LonRefTextBox.Text}\"", informationType.INFO, sender, e);
            }
        }
        private void ViewGeoDataPosButton_Click(object sender, EventArgs e)
        {
            double selectedLatValue = 0;
            string selectedLatDir = LatRefTextBox.Text;
            double selectedLonValue = 0;
            string selectedLonDir = LonRefTextBox.Text;
            string url = "https://www.google.com/maps/";
            if ((EventStreetnameNumberTextBox.Text != "") && (EventZipCodeTextBox.Text != "") && (EventAreanameTextBox.Text != "") &&
                     (EventCitynameTextBox.Text != "") && (EventCountrynameTextBox.Text != ""))
            {
                // Searching for position based on address.
                // https://www.google.com/maps/place/Villavägen+82,+137+38+Västerhaninge?entry=ttu
                string sn = EventStreetnameNumberTextBox.Text;
                int dp = sn.IndexOf(" ");
                url = url + "place/" + sn.Substring(0, dp) + "+" + sn.Substring(dp + 1, sn.Length - dp - 1) + "+"
                    + EventZipCodeTextBox.Text + "+" + EventAreanameTextBox.Text + "+" + EventCountrynameTextBox.Text + "?entry=ttu";
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    url = url.Replace("&", "^&");
                    Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    Process.Start("xdg-open", url);
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    Process.Start("open", url);
                }
            }
            else if ((selectedLatDir != "") && (selectedLonDir != ""))
            {
                string strSelLatValue = LatValueTextBox.Text;
                int dp = strSelLatValue.IndexOf(" ");
                string outSelLatValue = strSelLatValue.Substring(0, dp - 1);
                strSelLatValue = strSelLatValue.Substring(dp + 1, strSelLatValue.Length - dp - 1);
                dp = strSelLatValue.IndexOf(" ");
                outSelLatValue = outSelLatValue + "." + strSelLatValue.Substring(0, dp - 1);
                strSelLatValue = strSelLatValue.Substring(dp + 1, strSelLatValue.Length - dp - 1);
                dp = strSelLatValue.IndexOf(",");
                outSelLatValue = outSelLatValue + strSelLatValue.Substring(0, dp);
                strSelLatValue = strSelLatValue.Substring(dp + 1, strSelLatValue.Length - dp - 1);
                outSelLatValue = outSelLatValue + strSelLatValue.Substring(0, 2);
                double.TryParse(outSelLatValue, NumberStyles.Any, CultureInfo.InvariantCulture, out selectedLatValue);
                // Longitude value
                string strSelLonValue = LonValueTextBox.Text;
                // "-87° 37' 28,18\""
                dp = strSelLonValue.IndexOf(" ");
                string outSelLonValue = strSelLonValue.Substring(0, dp - 1);
                strSelLonValue = strSelLonValue.Substring(dp + 1, strSelLonValue.Length - dp - 1);
                dp = strSelLonValue.IndexOf(" ");
                outSelLonValue = outSelLonValue + "." + strSelLonValue.Substring(0, dp - 1);
                strSelLonValue = strSelLonValue.Substring(dp + 1, strSelLonValue.Length - dp - 1);
                dp = strSelLonValue.IndexOf(",");
                outSelLonValue = outSelLonValue + strSelLonValue.Substring(0, dp);
                strSelLonValue = strSelLonValue.Substring(dp + 1, strSelLonValue.Length - dp - 1);
                outSelLonValue = outSelLonValue + strSelLonValue.Substring(0, 2);
                double.TryParse(outSelLonValue, NumberStyles.Any, CultureInfo.InvariantCulture, out selectedLonValue);
                // Finding position based on Latitude-Longitude
                // https://www.google.com/maps/@59.3324449,18.074283,16z?entry=ttu
                url = url + "@" + selectedLatValue + "," + selectedLonValue;// + "?entry=ttu"; //",50z?entry=ttu";
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    url = url.Replace("&", "^&");
                    Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    Process.Start("xdg-open", url);
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    Process.Start("open", url);
                }
            }
        }
        private void SaveGeoDataChangesButton_Click(object sender, EventArgs e)
        {
            if ((geoLatValueChanged) || (geoLonValueChanged))
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
                    string sNewLonValue = LonValueTextBox.Text;
                    byte[] bteNewLonValue = Encoding.Unicode.GetBytes(sNewLonValue);
                    string newLonDirection = LonRefTextBox.Text;
                    byte[] bteNewLonDirection = Encoding.Unicode.GetBytes(newLonDirection);
                    try
                    {
                        PropertyItem currLonItm = picture.GetPropertyItem(3);
                        currLonItm.Value = bteNewLonValue;
                        currLonItm.Len = bteNewLonValue.Length;
                        picture.SetPropertyItem(currLonItm);
                    }
                    catch (Exception err)
                    {
                        setInformationText("Saving longitude values failed with: " + err.ToString(), informationType.ERROR, sender, e);
                    }
                    try
                    {
                        PropertyItem currLonDir = picture.GetPropertyItem(4);
                        currLonDir.Value = bteNewLonDirection;
                        currLonDir.Len = bteNewLonDirection.Length;
                        picture.SetPropertyItem(currLonDir);
                    }
                    catch (Exception err)
                    {
                        setInformationText("Saving longitude values failed with: " + err.ToString(), informationType.ERROR, sender, e);
                    }
                    toSave.imageChanges = true;
                }
                catch (Exception err)
                {
                    setInformationText("Saving geographical Latitude position failed: " + Environment.NewLine + err.ToString(), informationType.ERROR, sender, e);
                }
            }
        }
        private void DiscardGeoDataChangesButton_Click(object sender, EventArgs e)
        {
            getImageMetadataValues(loadedImageFileName, sender, e);
            SaveGeoDataChangesButton.Enabled = false;
            DiscardGeoDataChangesButton.Enabled = false;
        }
        #endregion
        #region Actor Information Handling
        // --- --- Actor information handling --- ---
        private void setActorGUIInfo(ActorClass incommingActor, bool editing)
        {
            ViewSelArtistDataButton.Visible = false;
            ArtistIdentityEnterTextBox.Visible = editing;
            #region UserName
            //if (editing)
            if (ActiveArtistsComboBox.SelectedText == "Add artist...")
            {
                ActiveArtistsComboBox.Enabled = false;
                ActiveArtistsComboBox.Visible = false;
                ViewSelArtistDataButton.Enabled = false;
                ViewSelArtistDataButton.Visible = false;
                ArtistIdentityEnterTextBox.Location = new System.Drawing.Point(ActiveArtistsComboBox.Location.X, ActiveArtistsComboBox.Location.Y);
                ArtistIdentityEnterTextBox.Text = "";
                ArtistIdentityEnterTextBox.Enabled = true;
                ArtistIdentityEnterTextBox.Visible = true;
            }
            else if (ActiveArtistsComboBox.SelectedText != "Select artist")
            {
                ArtistIdentityEnterTextBox.Enabled = false;
                ArtistIdentityEnterTextBox.Visible = false;
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
                ActiveArtistsComboBox.Enabled = true;
                ActiveArtistsComboBox.Visible = true;
            }
            #endregion
            #region UserContacts
            this.SelContactTypeTextBox.Width = 200;
            ActorContactTypeComboBox.Items.Clear();
            if (incommingActor.getNoOfUserContacts() > 0)
            {
                ActorContactTypeComboBox.Items.Add("Select");
                for (int i = 0; i < incommingActor.getNoOfUserContacts(); i++)
                {
                    ActorContactTypeComboBox.Items.Add(incommingActor.getUserContactType(i));
                }
                ActorContactTypeComboBox.Items.Add("Add ítem...");
                ActorContactTypeComboBox.SelectedItem = 0;
                SelContactTypeTextBox.Text = "";
            }
            else
                ActorContactTypeComboBox.Items.Add("Add item...");
            AddContactButton.Text = "Reach";
            AddContactButton.Enabled = true;
            SelContactTypeTextBox.Width = 150;
            AddContactButton.Visible = true;
            SaveActorDataChangesButton.Text = "Edit actordata";
            SaveActorDataChangesButton.Enabled = true;
            SaveActorDataChangesButton.Visible = true;
            ViewSelArtistDataButton.Visible = false;
            #endregion
            #region birthData
            if ((incommingActor.getUserBirthStreet() != "") && (incommingActor.getUserBirthStreet() != null))
            {
                BirthStreetAddrTextBox.Text = (incommingActor.getUserBirthStreet() + " " + incommingActor.getUserBirthStreetNumber() + incommingActor.getUserBirthStreetNumberAddon());
                BirthStreetAddrTextBox.Enabled = false;
            }
            else
            {
                BirthStreetAddrTextBox.Text = "";
                BirthStreetAddrTextBox.Enabled = true;
            }
            if ((incommingActor.getUserBirthZipcode() != "") && (incommingActor.getUserBirthZipcode() != null))
            {
                BirthZipCodeTextBox.Text = incommingActor.getUserBirthZipcode();
                BirthZipCodeTextBox.Enabled = false;
            }
            else
            {
                BirthZipCodeTextBox.Text = "";
                BirthZipCodeTextBox.Enabled = true;
            }
            if ((incommingActor.getUserBirthAreaname() != "") && (incommingActor.getUserBirthAreaname() != null))
            {
                BirthAreaNameTextBox.Text = incommingActor.getUserBirthAreaname();
                BirthAreaNameTextBox.Enabled = false;
            }
            else
            {
                BirthAreaNameTextBox.Text = "";
                BirthAreaNameTextBox.Enabled = true;
            }
            if ((incommingActor.getUserBirthCityname() != "") && (incommingActor.getUserBirthCityname() != null))
            {
                BirthCitynameTextBox.Text = incommingActor.getUserBirthCityname();
                BirthCitynameTextBox.Enabled = false;
            }
            else
            {
                BirthCitynameTextBox.Text = "";
                BirthCitynameTextBox.Enabled = true;
            }
            if ((incommingActor.getUserBirthCountry() != "") && (incommingActor.getUserBirthCountry() != null))
            {
                BirthCountryTextBox.Text = incommingActor.getUserBirthCountry();
                BirthCountryTextBox.Enabled = false;
            }
            else
            {
                BirthCountryTextBox.Text = "";
                BirthCountryTextBox.Enabled = true;
            }
            if ((incommingActor.getUserBirthDate() != "") && (incommingActor.getUserBirthDate() != null))
            {
                BirthDateTextBox.Text = incommingActor.getUserBirthDate();
                BirthDateTextBox.Enabled = false;
            }
            else
            {
                BirthDateTextBox.Text = "";
                BirthDateTextBox.Enabled = true;
            }
            if ((incommingActor.getUserBirthSocNo() != "") && (incommingActor.getUserBirthSocNo() != null))
            {
                SocSecNumberTextBox.Text = incommingActor.getUserBirthSocNo();
                SocSecNumberTextBox.Enabled = false;
            }
            else
            {
                SocSecNumberTextBox.Text = "";
                SocSecNumberTextBox.Enabled = true;
            }
            if ((incommingActor.getUserBirthLatitude() != "") && (incommingActor.getUserBirthLatitude() != null))
            {
                BirthLatitudeTextBox.Text = incommingActor.getUserBirthLatitude();
                ViewGeoPosButton.Enabled = true;
                BirthLatitudeTextBox.Enabled = false;
            }
            else
            {
                BirthLatitudeTextBox.Text = "";
                ViewGeoPosButton.Enabled = false;
                BirthLatitudeTextBox.Enabled = true;
            }
            if ((incommingActor.getUserBirthLongitude() != "") && (incommingActor.getUserBirthLongitude() != null))
            {
                BirthLongitudeTextBox.Text = incommingActor.getUserBirthLongitude();
                ViewGeoPosButton.Enabled = true;
                BirthLongitudeTextBox.Enabled = false;
            }
            else
            {
                BirthLongitudeTextBox.Text = "";
                ViewGeoPosButton.Enabled = false;
                BirthLongitudeTextBox.Enabled = true;
            }
            #endregion
            #region SkinTone
            SkinToneDateTimePicker.Visible = false;
            SkinToneValidDateComboBox.Items.Clear();
            SkinToneDateTxtBx.Visible = false;
            if (incommingActor.getNumberOfSkinTones() > 0)
            {
                // Actor has skintone data entries
                SkinToneTagCmbBx.Visible = false;
                SkinToneDateTimePicker.Visible = false;
                SkinToneDateTxtBx.Visible = false;
                AddSkinToneButton.Text = "";
                AddSkinToneButton.Enabled = false;
                AddSkinToneButton.Visible = false;
                SkinToneLabel.Visible = true;
                SkinToneTagTextBox.Size = new Size(((ActorData.Size.Width - AddSkinToneButton.Size.Width - SkinToneDateTxtBx.Size.Width) - 30), 20);
                SkinToneTagTextBox.Text = "";
                SkinToneTagTextBox.Enabled = false;
                SkinToneTagTextBox.Visible = true;
                SkinToneValidDateComboBox.Items.Add("Select...");
                for (int i = 0; i < incommingActor.getNumberOfSkinTones(); i++)
                    SkinToneValidDateComboBox.Items.Add(incommingActor.getUserSkinToneValidDate(i));
                SkinToneValidDateComboBox.Items.Add("Add data...");
                SkinToneValidDateComboBox.Items.Add("Add tag...");
                SkinToneValidDateComboBox.SelectedItem = 0;
                SkinToneValidDateComboBox.Enabled = true;
                SkinToneValidDateComboBox.Visible = true;
                noOfChangedGUIs = 0;
            }
            else if (linwin.getNoOfComplexions() > 0)
            {
                SkinToneTagTextBox.Size = new Size(((ActorData.Size.Width - AddSkinToneButton.Size.Width - SkinToneDateTxtBx.Size.Width) - 10), 20);
                SkinToneTagTextBox.Visible = false;
                SkinToneDateTxtBx.Visible = false;
                SkinToneValidDateComboBox.Visible = false;
                AddSkinToneButton.Text = "Add Skintone";
                AddSkinToneButton.Enabled = false;
                AddSkinToneButton.Visible = true;
                SkinToneTagCmbBx.Items.Clear();
                SkinToneTagCmbBx.Items.Add("Select...");
                for (int i = 0; i < linwin.getNoOfComplexions(); i++)
                    SkinToneTagCmbBx.Items.Add(linwin.getComplexionTag(i));
                SkinToneTagCmbBx.Items.Add("Add tag...");
                SkinToneDateTimePicker.Visible = true;
                SkinToneDateTimePicker.Enabled = true;
                SkinToneValidDateComboBox.Enabled = false;
                SkinToneValidDateComboBox.Visible = false;
                noOfChangedGUIs = 0;
            }
            else
            {
                SkinToneTagCmbBx.Visible = false;
                SkinToneDateTimePicker.Visible = false;
                SkinToneDateTxtBx.Visible = false;
                SkinToneValidDateComboBox.Visible = false;
                AddSkinToneButton.Text = "Add Tag";
                AddSkinToneButton.Enabled = false;
                AddSkinToneButton.Visible = true;
                SkinToneTagTextBox.Text = "<tag>[;|,] <descr.>[;|,] <level>";
                SkinToneTagTextBox.Size = new Size(((ActorData.Size.Width - AddSkinToneButton.Size.Width) - 5), 20);
                SkinToneTagTextBox.Enabled = true;
                SkinToneTagTextBox.Visible = true;
                noOfChangedGUIs = 0;
            }
            #endregion
            #region EyeColor
            if (incommingActor.getNumberOfEyeData() > 0)
            {
                // Actor have eyecolor data
                EyeColorValidDateComboBox.Items.Clear();
                EyeColorValidDateComboBox.Items.Add("Select...");
                for (int i = 0; i < incommingActor.getNumberOfEyeData(); i++)
                    EyeColorValidDateComboBox.Items.Add(incommingActor.getUserEyeDataValidDate(i));
                EyeColorValidDateComboBox.Items.Add("Add Item...");
                EyeColorValidDateComboBox.Items.Add("Add Tag...");
                EyeColorValidDateComboBox.SelectedIndex = 0;
                EyeColorValidDateComboBox.Enabled = true;
                EyeColorValidDateComboBox.Visible = true;
                AddEyeColorButton.Text = "";
                AddEyeColorButton.Enabled = false;
                AddEyeColorButton.Visible = false;
                EyeColorLabel.Visible = true;
                eyeColorTagsCmbBx.Enabled = false;
                eyeColorTagsCmbBx.Visible = false;
                SelEyeColorTextBox.Size = new Size(((ActorData.Size.Width - AddEyeColorButton.Size.Width - EyeColorDteTmePckr.Size.Width) - 30), 20);
                SelEyeColorTextBox.Text = "";
                SelEyeColorTextBox.Visible = true;
                SelEyeColorTextBox.Enabled = false;
            }
            else if (linwin.getNoOfEyeColors() > 0)
            {
                // Actor has no eyecolor data, user has eyecolor tags.
                SelEyeColorTextBox.Size = new Size(((ActorData.Size.Width - AddEyeColorButton.Size.Width - EyeColorDteTmePckr.Size.Width) - 10), 20);
                SelEyeColorTextBox.Text = "";
                SelEyeColorTextBox.Visible = false;
                SelEyeColorTextBox.Enabled = false;
                AddEyeColorButton.Text = "Add Color...";
                AddEyeColorButton.Enabled = false;
                AddEyeColorButton.Visible = true;
                eyeColorTagsCmbBx.Items.Clear();
                eyeColorTagsCmbBx.Items.Add("Select...");
                for (int i = 0; i < linwin.getNoOfEyeColors(); i++)
                    eyeColorTagsCmbBx.Items.Add(linwin.getEyeColorTag(i));
                eyeColorTagsCmbBx.Items.Add("Add tag...");
                EyeColorDteTmePckr.Visible = true;
                EyeColorDteTmePckr.Enabled = true;
                noOfChangedGUIs = 0;
            }
            else
            {
                eyeColorTagsCmbBx.Enabled = false;
                eyeColorTagsCmbBx.Visible = false;
                EyeColorDteTmePckr.Enabled = false;
                EyeColorDteTmePckr.Visible = false;
                EyeColorValidDateComboBox.Enabled = false;
                EyeColorValidDateComboBox.Visible = false;
                AddEyeColorButton.Text = "Add Tag";
                AddEyeColorButton.Enabled = false;
                AddEyeColorButton.Visible = true;
                SelEyeColorTextBox.Text = "<tag>[;<,] <descr.>[;|,] <level>";
                SelEyeColorTextBox.Size = new Size(((ActorData.Size.Width - AddEyeColorButton.Size.Width) - 5), 20);
                SelEyeColorTextBox.Enabled = true;
                SelEyeColorTextBox.Visible = true;
                noOfChangedGUIs = 0;
            }
            #endregion
            #region GenderInfo
            if (linwin.getUserRightsValue() >= 4)
            {
                GdrLookCmbBx.Enabled = false;
                GdrLookCmbBx.Visible = false;
                GenderTypeCmbBx.Enabled = false;
                GenderTypeCmbBx.Visible = false;
                GenderDateTimePicker.Enabled = false;
                GenderDateTimePicker.Visible = false;
                GenderBehaveCmbBx.Enabled = false;
                GenderBehaveCmbBx.Visible = false;
                GenderInfoValidDateComboBox.Items.Clear();
                GenderInfoValidDateComboBox.Items.Add("Select...");
                for (int i = 0; i < incommingActor.getNumberOfGenderData(); i++)
                    GenderInfoValidDateComboBox.Items.Add(incommingActor.getUserGenderInfoValidDate(i));
                GenderInfoValidDateComboBox.Items.Add("Add item...");
                GenderInfoValidDateComboBox.Enabled = true;
                GenderInfoValidDateComboBox.Visible = true;
                GenderInfoValidDateComboBox.SelectedIndex = 0;
                GenderTypeTextBox.Text = "";
                GenderTypeTextBox.Enabled = false;
                GenderTypeTextBox.Visible = true;
                GdrLengthTextBox.Text = "";
                GdrLengthTextBox.Enabled = false;
                GdrLengthTextBox.Visible = true;
                GdrCircumfTextBox.Text = "";
                GdrCircumfTextBox.Enabled = false;
                GdrCircumfTextBox.Visible = true;
                GdrLookTypeTextBox.Text = "";
                GdrLookTypeTextBox.Enabled = false;
                GdrLookTypeTextBox.Visible = true;
                GdrBehaveTypeTextBox.Text = "";
                GdrBehaveTypeTextBox.Enabled = false;
                GdrBehaveTypeTextBox.Visible = true;
                GdrPresentTextBox.Text = "";
                GdrPresentTextBox.Enabled = false;
                GdrPresentTextBox.Visible = true;
                AddGenderInfoButton.Text = "";
                AddGenderInfoButton.Enabled = false;
                AddGenderInfoButton.Visible = false;
            }
            #endregion
            #region LengthInfo
            LengthValidDateComboBox.Items.Clear();
            if (incommingActor.getNumberOfLengthData() > 0)
            {
                AddLengthInfoButton.Text = "";
                AddLengthInfoButton.Enabled = false;
                AddLengthInfoButton.Visible = false;
                LengthLabel.Visible = true;
                LengthValidDateComboBox.Items.Add("Select...");
                for (int i = 0; i < incommingActor.getNumberOfLengthData(); i++)
                    LengthValidDateComboBox.Items.Add(incommingActor.getUserLengthInfoValidDate(i));
                LengthValidDateComboBox.Items.Add("Add item...");
                LengthValidDateComboBox.SelectedIndex = 0;
                LengthTextBox.Text = "";
            }
            else
            {
                AddLengthInfoButton.Text = "";
                AddLengthInfoButton.Enabled = false;
                AddLengthInfoButton.Visible = false;
                LengthLabel.Visible = true;
                LengthValidDateComboBox.Items.Add("Select...");
                LengthValidDateComboBox.Items.Add("Add item...");
                LengthValidDateComboBox.SelectedIndex = 0;
                LengthTextBox.Text = "";
            }
            LengthValidDateComboBox.Enabled = true;
            #endregion
            #region WeightInformation
            WeightValidDateComboBox.Items.Clear();
            if (incommingActor.getNumberOfWeightData() > 0)
            {
                AddWeightInfoButton.Text = "";
                AddWeightInfoButton.Enabled = false;
                AddWeightInfoButton.Visible = false;
                WeightLabel.Visible = true;
                WeightValidDateComboBox.Items.Add("Select...");
                for (int i = 0; i < incommingActor.getNumberOfWeightData(); i++)
                    WeightValidDateComboBox.Items.Add(incommingActor.getUserWeightInfoValidDate(i));
                WeightValidDateComboBox.Items.Add("Add item...");
                WeightValidDateComboBox.SelectedIndex = 0;
                WeightValidDateComboBox.Enabled = true;
                WeightValidDateComboBox.Visible = true;
                WeightTextBox.Text = "";
            }
            else
            {
                AddWeightInfoButton.Text = "";
                AddWeightInfoButton.Enabled = false;
                AddWeightInfoButton.Visible = false;
                WeightLabel.Visible = true;
                WeightValidDateComboBox.Items.Add("Select...");
                WeightValidDateComboBox.Items.Add("Add item...");
                WeightValidDateComboBox.SelectedIndex = 0;
                WeightTextBox.Text = "";
            }
            #endregion
            #region ChestData
            AddChestInfoButton.Text = "";
            AddChestInfoButton.Enabled = false;
            AddChestInfoButton.Visible = false;
            ChestDateTimePicker.Enabled = false;
            ChestDateTimePicker.Visible = false;
            ChestValidDateComboBox.Items.Clear();
            ChestValidDateComboBox.Items.Add("Select...");
            if (incommingActor.getNumberOfChestData() > 0)
            {
                for (int i = 0; i < incommingActor.getNumberOfChestData(); i++)
                    ChestValidDateComboBox.Items.Add(incommingActor.getUserChestInfoValidDate(i));
            }
            ChestValidDateComboBox.Items.Add("Add item...");
            ChestValidDateComboBox.SelectedIndex = 0;
            ChestValidDateComboBox.Enabled = true;
            ChestValidDateComboBox.Visible = true;
            // BreastSizeStrings, BreastTypeStrings
            ChestTypeCmbBx.Enabled = false;
            ChestTypeCmbBx.Visible = false;
            ChestTypeTextBox.Text = "";
            ChestTypeTextBox.Enabled = false;
            ChestTypeTextBox.Visible = true;
            ChestCircTextBox.Text = "";
            ChestCircTextBox.Enabled = false;
            ChestCircTextBox.Visible = true;
            ChestSizeTypeCmbBx.Enabled = false;
            ChestSizeTypeCmbBx.Visible = false;
            ChestSizeTextBox.Text = "";
            ChestSizeTextBox.Enabled = false;
            ChestSizeTextBox.Visible = true;
            #endregion
            #region HairData
            AddHairInfoButton.Text = "";
            AddHairInfoButton.Enabled = false;
            AddHairInfoButton.Visible = false;
            HairDataLabel.Visible = true;
            hairColorCmbBx.Enabled = false;
            hairColorCmbBx.Visible = false;
            hairTextureCmbBx.Enabled = false;
            hairTextureCmbBx.Visible = false;
            hairLengthCmbBx.Enabled = false;
            hairLengthCmbBx.Visible = false;
            HairDataValidDateComboBox.Items.Clear();
            HairDataValidDateComboBox.Items.Add("Select...");
            if (incommingActor.getNumberOfHairData() > 0)
            {
                for (int i = 0; i < incommingActor.getNumberOfHairData(); i++)
                    HairDataValidDateComboBox.Items.Add(incommingActor.getUserHairValidDate(i));
            }
            HairDataValidDateComboBox.Items.Add("Add item...");
            HairDataValidDateComboBox.SelectedIndex = 0;
            HairDataValidDateComboBox.Enabled = true;
            HairDataValidDateComboBox.Visible = true;
            HairColorTextBox.Text = "";
            HairColorTextBox.Enabled = false;
            HairColorTextBox.Visible = true;
            HairTextureTypeTextBox.Text = "";
            HairTextureTypeTextBox.Enabled = false;
            HairTextureTypeTextBox.Visible = true;
            HairLengthTextBox.Text = "";
            HairLengthTextBox.Enabled = false;
            HairLengthTextBox.Visible = true;
            #endregion
            #region MarkingsData
            MarkingsLabel.Visible = true;
            AddMarkingDataButton.Text = "";
            AddMarkingDataButton.Enabled = false;
            AddMarkingDataButton.Visible = false;
            MarkingTypeTextBox.Text = "";
            MarkingTypeTextBox.Enabled = false;
            MarkingTypeTextBox.Visible = false;
            MarkingTypeComboBox.Items.Clear();
            MarkingTypeComboBox.Items.Add("Select...");
            for (int i = 0; i < actorClass.getNoOfMarkingsData(); i++)
                MarkingTypeComboBox.Items.Add(actorClass.getActorMarkingType(i));
            MarkingTypeComboBox.SelectedIndex = 0;
            MarkingTypeComboBox.Enabled = true;
            MarkingTypeComboBox.Visible = true;
            MarkingPosTextBox.Text = "";
            MarkingPosTextBox.Enabled = false;
            MarkingPosTextBox.Visible = true;
            MarkingMotifTextBox.Text = "";
            MarkingMotifTextBox.Enabled = false;
            MarkingMotifTextBox.Visible = true;
            MarkingDateTextBox.Text = "";
            MarkingDateTextBox.Enabled = false;
            MarkingDateTextBox.Visible = false;
            MarkingsDateTimePicker.Enabled = false;
            MarkingsDateTimePicker.Visible = false;
            MarkingsValidDateComboBox.Items.Clear();
            MarkingsValidDateComboBox.Items.Add("Select...");
            for (int i = 0; i < actorClass.getNoOfMarkingsData(); i++)
                MarkingsValidDateComboBox.Items.Add(actorClass.getActorMarkingValidDate(i));
            MarkingsValidDateComboBox.Items.Add("Add item...");
            MarkingsValidDateComboBox.SelectedIndex = 0;
            MarkingsValidDateComboBox.Enabled = true;
            MarkingsValidDateComboBox.Visible = true;
            #endregion
            #region OccupationData
            label4.Visible = true;
            OccupTitleTextBox.Text = "";
            OccupTitleTextBox.Enabled = false;
            OccupTitleTextBox.Visible = true;
            OccupationStartDateTimePicker.Enabled = false;
            OccupationStartDateTimePicker.Visible = false;
            occupationStartTxtBx.Text = "";
            occupationStartTxtBx.Enabled = false;
            occupationStartTxtBx.Visible = false;
            ActorOccupationStartDateComboBox.Items.Clear();
            ActorOccupationStartDateComboBox.Items.Add("Select...");
            for (int i = 0; i < actorClass.getNoOfOccupationsData(); i++)
                ActorOccupationStartDateComboBox.Items.Add(actorClass.getActorOccupationStarted(i));
            ActorOccupationStartDateComboBox.Items.Add("Add item...");
            ActorOccupationStartDateComboBox.SelectedIndex = 0;
            ActorOccupationStartDateComboBox.Enabled = true;
            ActorOccupationStartDateComboBox.Visible = true;
            ActorOccupationCompanyTextBox.Text = "";
            ActorOccupationCompanyTextBox.Enabled = false;
            ActorOccupationCompanyTextBox.Visible = true;
            AddOccupationDataButton.Text = "";
            AddOccupationDataButton.Enabled = false;
            AddOccupationDataButton.Visible = false;
            ActorEmployAddressTextBox.Text = "";
            ActorEmployAddressTextBox.Enabled = false;
            ActorEmployAddressTextBox.Visible = true;
            ActorEmployZipCodeTextBox.Text = "";
            ActorEmployZipCodeTextBox.Enabled = false;
            ActorEmployZipCodeTextBox.Visible = true;
            ActorEmpoyAreanameTextBox.Text = "";
            ActorEmpoyAreanameTextBox.Enabled = false;
            ActorEmpoyAreanameTextBox.Visible = true;
            ActorEmployCitynameTextBox.Text = "";
            ActorEmployCitynameTextBox.Enabled = false;
            ActorEmployCitynameTextBox.Visible = true;
            ActorOccupationEndDaeComboBox.Items.Clear();
            ActorOccupationEndDaeComboBox.Items.Add("Select...");
            for (int i = 0; i < actorClass.getNoOfOccupationsData(); i++)
                ActorOccupationEndDaeComboBox.Items.Add(actorClass.getActorOccupationEnded(i));
            ActorOccupationEndDaeComboBox.Items.Add("Add item...");
            ActorOccupationEndDaeComboBox.SelectedIndex = 0;
            ActorOccupationEndDaeComboBox.Enabled = true;
            ActorOccupationEndDaeComboBox.Visible = true;
            OccupationEndDateTimePicker.Enabled = false;
            OccupationEndDateTimePicker.Visible = false;
            occupationEndTxtBx.Text = "";
            occupationEndTxtBx.Enabled = false;
            occupationEndTxtBx.Visible = false;
            ActorEmployCountryTextBox.Text = "";
            ActorEmployCountryTextBox.Enabled = false;
            ActorEmployCountryTextBox.Visible = true;
            #endregion
            #region ResidenceData
            ResidenceLabel.Visible = true;
            ResidAddressTextBox.Text = "";
            ResidAddressTextBox.Enabled = false;
            ResidAddressTextBox.Visible = true;
            residenceStartTxtBx.Text = "";
            residenceStartTxtBx.Enabled = false;
            residenceStartTxtBx.Visible = false;
            ResidStartDateTimePicker.Enabled = false;
            ResidStartDateTimePicker.Visible = false;
            ResidStartDateComboBox.Items.Clear();
            ResidStartDateComboBox.Items.Add("Select...");
            for (int i = 0; i < actorClass.getNoOfResicenceData(); i++)
                ResidStartDateComboBox.Items.Add(actorClass.getActorResidBought(i));
            ResidStartDateComboBox.Items.Add("Add item...");
            ResidStartDateComboBox.SelectedIndex = 0;
            ResidStartDateComboBox.Enabled = true;
            ResidStartDateComboBox.Visible = true;
            ResidZipCodeTextBox.Text = "";
            ResidZipCodeTextBox.Enabled = false;
            ResidZipCodeTextBox.Visible = true;
            ResidAreanameTextBox.Text = "";
            ResidAreanameTextBox.Enabled = false;
            ResidAreanameTextBox.Visible = true;
            AddResidenceButton.Text = "";
            AddResidenceButton.Enabled = false;
            AddResidenceButton.Visible = false;
            ResidCitynameTextBox.Text = "";
            ResidCitynameTextBox.Enabled = false;
            ResidCitynameTextBox.Visible = true;
            residenceEndTxtBx.Text = "";
            residenceEndTxtBx.Enabled = false;
            residenceEndTxtBx.Visible = false;
            ResidEndDateTimePicker.Enabled = false;
            ResidEndDateTimePicker.Visible = false;
            ResidEndDateComboBox.Items.Clear();
            ResidEndDateComboBox.Items.Add("Select...");
            for (int i = 0; i < actorClass.getNoOfResicenceData(); i++)
                ResidEndDateComboBox.Items.Add(actorClass.getActorResidSold(i));
            ResidEndDateComboBox.Items.Add("Add item...");
            ResidEndDateComboBox.SelectedIndex = 0;
            ResidEndDateComboBox.Enabled = true;
            ResidEndDateComboBox.Visible = true;
            ResidCountryTextBox.Text = "";
            ResidCountryTextBox.Enabled = false;
            ResidCountryTextBox.Visible = true;
            #endregion
            #region EventsData
            addAttdEventBtn.Enabled = false;
            addAttdEventBtn.Visible = false;
            AttEventLabel.Visible = true;
            EventIdTextBox.Text = "";
            EventIdTextBox.Enabled = false;
            EventIdTextBox.Visible = false;
            AttEvtIDCmbBx.Items.Clear();
            AttEvtIDCmbBx.Items.Add("Select...");
            for (int i = 0; i < actorClass.getNoOfAttendedEventsData(); i++)
                AttEvtIDCmbBx.Items.Add(actorClass.getActorAttendedEventID(i));
            AttEvtIDCmbBx.Items.Add("Add item...");
            AttEvtIDCmbBx.SelectedIndex = 0;
            AttEvtIDCmbBx.Enabled = true;
            AttEvtIDCmbBx.Visible = true;
            attdEvtDateTxtBx.Text = "";
            attdEvtDateTxtBx.Enabled = false;
            attdEvtDateTxtBx.Visible = false;
            AttdEventsDateTimePicker.Enabled = false;
            AttdEventsDateTimePicker.Visible = false;
            AttdEventsDateComboBox.Items.Clear();
            AttdEventsDateComboBox.Items.Add("Select...");
            for (int i = 0; i < actorClass.getNoOfAttendedEventsData(); i++)
            {
                if (actorClass.getActorAttendedEventStarted(i) != null)
                    AttdEventsDateComboBox.Items.Add(actorClass.getActorAttendedEventStarted(i));
                else
                    AttdEventsDateComboBox.Items.Add("Undefined");
            }
            AttdEventsDateComboBox.Items.Add("Add item...");
            AttdEventsDateComboBox.SelectedIndex = 0;
            AttdEventsDateComboBox.Enabled = true;
            AttdEventsDateComboBox.Visible = true;
            EventCategTxtBx.Text = "";
            EventCategTxtBx.Enabled = false;
            EventCategTxtBx.Visible = true;
            RoleTagTxtBx.Text = "";
            RoleTagTxtBx.Enabled = false;
            RoleTagTxtBx.Visible = false;
            RoleTagCmboBx.Items.Clear();
            RoleTagCmboBx.Items.Add("Select...");
            for (int i = 0; i < actorClass.getNoOfAttendedEventsData(); i++)
            {
                if (actorClass.getActorAttendedEventRoleTag(i) != null)
                    RoleTagCmboBx.Items.Add(actorClass.getActorAttendedEventRoleTag(i));
                else
                    RoleTagCmboBx.Items.Add("Undefined");
            }
            RoleTagCmboBx.Items.Add("Add item...");
            RoleTagCmboBx.SelectedIndex = 0;
            RoleTagCmboBx.Enabled = true;
            RoleTagCmboBx.Visible = true;
            AttdEvtEndDateTxtBx.Text = "";
            AttdEvtEndDateTxtBx.Enabled = false;
            AttdEvtEndDateTxtBx.Visible = false;
            AttdEventEndDateTimePicker.Enabled = false;
            AttdEventEndDateTimePicker.Visible = false;
            AttdEventEndDateCmbBx.Items.Clear();
            AttdEventEndDateCmbBx.Items.Add("Select...");
            for (int i = 0; i < actorClass.getNoOfAttendedEventsData(); i++)
            {
                if (actorClass.getActorAttendedEventEnded(i) != null)
                    AttdEventEndDateCmbBx.Items.Add(actorClass.getActorAttendedEventEnded(i));
                else
                    AttdEventEndDateCmbBx.Items.Add("Undefined");
            }
            AttdEventEndDateCmbBx.Items.Add("Add item...");
            AttdEventEndDateCmbBx.SelectedIndex = 0;
            AttdEventEndDateCmbBx.Enabled = true;
            AttdEventEndDateCmbBx.Visible = true;
            #endregion
            #region ImagesData
            ActorRelImageComboBox.Items.Clear();
            ActorRelImageComboBox.Items.Add("Select...");
            for (int i = 0; i < incommingActor.getNoOfRelatedImages(); i++)
            {
                if (linwin.getUserRightsValue() >= incommingActor.getActorRelatedImageClassValue(i))
                {
                    string tempImPth = incommingActor.getActorRelatedImagePath(i);
                    if (tempImPth != null)
                    {
                        int dp0 = tempImPth.LastIndexOf("\\");
                        if ((dp0 > 0) && (dp0 < tempImPth.Length - 2))
                            tempImPth = tempImPth.Substring(dp0 + 1, tempImPth.Length - dp0 - 1);
                        ActorRelImageComboBox.Items.Add(tempImPth);
                    }
                }
            }
            ActorRelImageComboBox.Items.Add("Add item...");
            ActorRelImageComboBox.Items.Add("Update root");
            ActorRelImageComboBox.SelectedIndex = 0;
            ActorRleImgContextTxtBx.Text = "";
            ActorRleImgContextTxtBx.Enabled = false;
            ActorRleImgContextTxtBx.Visible = true;
            #endregion
            #region RootDirs
            if (incommingActor.getNoOfRootDirs() > 0)
            {
                cmbBoxRootDir.Items.Clear();
                cmbBoxRootDir.Items.Add("Select...");
                for (int i = 0; i < incommingActor.getNoOfRootDirs(); i++)
                {
                    if (i < incommingActor.getMaxNoOfRootDirs())
                        cmbBoxRootDir.Items.Add(incommingActor.getActorRootDir(i));
                }
                cmbBoxRootDir.Items.Add("Add item");
                cmbBoxRootDir.SelectedIndex = 0;
                cmbBoxRootDir.Enabled = true;
                cmbBoxRootDir.Visible = true;
                updRootBtn.Text = "Upd.";
                updRootBtn.Visible = true;
                updRootBtn.Enabled = false;
                btnDelRoot.Text = "Del.";
                btnDelRoot.Visible = true;
                btnDelRoot.Enabled = false;
            }
            else
            {
                cmbBoxRootDir.Items.Clear();
                cmbBoxRootDir.Items.Add("Select...");
                cmbBoxRootDir.Items.Add("Add item");
                cmbBoxRootDir.SelectedIndex = 0;
                cmbBoxRootDir.Enabled = true;
                cmbBoxRootDir.Visible = true;
                cmbBoxRootDir.Visible = true;
                cmbBoxRootDir.Enabled = false;
                btnDelRoot.Text = "Del.";
                btnDelRoot.Visible = true;
                btnDelRoot.Enabled = false;
                updRootBtn.Text = "Set";
                updRootBtn.Visible = true;
                updRootBtn.Enabled = true;
            }
            #endregion
        }
        private void loadSelArtist(string selArtist, object sender, EventArgs e)
        {
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

            string sUsrDirName = linwin.getActorStoragePath();
            if ((sUsrDirName == null) || (sUsrDirName == ""))
            {
                sUsrDirName = "C:\\Användare\\";
                if (!(System.IO.Directory.Exists(sUsrDirName)))
                    sUsrDirName = "C:\\Users\\";
                sUsrDirName = sUsrDirName + currUser + "\\" + sProgPath + "\\ActorData";
            }
            int dpp = selArtist.IndexOf(".");
            if ((dpp > 0) && (dpp < selArtist.Length))
                selArtist = selArtist.Substring(0, dpp);

            dpp = selArtist.IndexOf("_");
            if ((dpp > 0) && (dpp < selArtist.Length))
                selArtist = selArtist.Substring(dpp + 1);

//            if ((linwin.getActorStoragePath() != null) && (selArtist != "") && (System.IO.Directory.Exists(linwin.getActorStoragePath()) &&
//                (System.IO.File.Exists(linwin.getActorStoragePath() + "\\ActorData_" + selArtist + ".acf"))))
//                actorClass.loadActor((string)ActiveArtistsComboBox.SelectedItem, linwin.getActorStoragePath());
//            else 
            if ((System.IO.Directory.Exists(sUsrDirName)) && (System.IO.File.Exists(sUsrDirName + "\\ActorData_" + selArtist + ".acf")))
                actorClass.loadActor(selArtist, sUsrDirName);
            else
            {
                setInformationText("Actor data directory or file does not exist.", informationType.ERROR, sender, e);
                return;
            }
            // This is when an actor has been selected.
            setActorGUIInfo(actorClass, false);
            /* ---------------------
            AddContactButton.Text = "Reach";
            AddContactButton.Enabled = true;
            SelContactTypeTextBox.Width = 150;
            AddContactButton.Visible = true;
            SaveActorDataChangesButton.Text = "Edit actordata";
            SaveActorDataChangesButton.Enabled = true;
            SaveActorDataChangesButton.Visible = true;
            ViewSelArtistDataButton.Visible = false;
            --------------------- */
        }
        private void addNewArtist(object sender, EventArgs e)
        {
            setInformationText("Enabling to add a new artist.", informationType.INFO, sender, e);
            #region Initializations
            ViewSelArtistDataButton.Visible = false;
            ArtistIdentityEnterTextBox.Visible = false;
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
            for (int i = 0; i < linwin.noOfContactWays; i++)
                actorClass.addContactCategory(linwin.getContactWayTag(i), linwin.getContactWayDescr(i), linwin.getContactWayLevel(i));
            #endregion
            #region ArtistIdentity
            ArtistIdentityEnterTextBox.Location = new System.Drawing.Point(ActiveArtistsComboBox.Location.X, ActiveArtistsComboBox.Location.Y);
            this.ArtistIdentityEnterTextBox.Text = "";
            ActiveArtistsComboBox.Enabled = false;
            ActiveArtistsComboBox.Visible = false;
            this.ArtistIdentityEnterTextBox.Enabled = true;
            this.ArtistIdentityEnterTextBox.Visible = true;
            #endregion
            #region ArtistName(s)
            this.NameTypeComboBox.Items.Clear();
            this.NameTypeComboBox.Items.Add("Select nametype");
            this.NameTypeComboBox.Items.Add("Birth name");
            this.NameTypeComboBox.Items.Add("Taken name");
            this.NameTypeComboBox.Items.Add("Married name");
            this.NameTypeComboBox.Items.Add("Alias name");
            this.NameTypeComboBox.Items.Add("Nickname");
            this.NameTypeComboBox.SelectedIndex = 0;
            this.NameTypeComboBox.Enabled = true;
            this.NameTypeComboBox.Visible = true;
            this.SelNameTypeTextBox.Width = 150;
            this.SelNameTypeTextBox.Enabled = true;
            this.SelNameTypeTextBox.Visible = true;
            this.AddNewNameButton.Visible = true;
            this.AddNewNameButton.Enabled = true;
            #endregion
            #region ContactData
            this.ActorContactTypeComboBox.Items.Clear();
            // This below here should possibly change.
            this.ActorContactTypeComboBox.Items.Add("Select contact");
            for (int i = 0; i < actorClass.noOfContactCategories; i++)
                this.ActorContactTypeComboBox.Items.Add(actorClass.contactCategory[i].tag);
            this.ActorContactTypeComboBox.Items.Add("New type");
            //this.ActorContactTypeComboBox.SelectedIndex = 0;
            this.ActorContactTypeComboBox.Enabled = true;
            this.ActorContactTypeComboBox.Visible = true;
            this.SelContactTypeTextBox.Text = "";
            this.SelContactTypeTextBox.Enabled = true;
            this.SelContactTypeTextBox.Width = 150;
            this.SelContactTypeTextBox.Visible = true;
            this.AddContactButton.Visible = true;
            this.AddContactButton.Enabled = true;
            #endregion
            #region BirthData
            this.BirthStreetAddrTextBox.Text = "";
            this.BirthStreetAddrTextBox.Enabled = true;
            this.BirthZipCodeTextBox.Text = "";
            this.BirthZipCodeTextBox.Enabled = true;
            this.BirthAreaNameTextBox.Text = "";
            this.BirthAreaNameTextBox.Enabled = true;
            this.BirthCitynameTextBox.Text = "";
            this.BirthCitynameTextBox.Enabled = true;
            this.BirthCountryTextBox.Text = "";
            this.BirthCountryTextBox.Enabled = true;
            this.BirthDateTextBox.Text = "";
            this.BirthDateTextBox.Enabled = true;
            this.SocSecNumberTextBox.Text = "";
            this.SocSecNumberTextBox.Enabled = true;
            this.BirthLatitudeTextBox.Text = "";
            this.BirthLatitudeTextBox.Enabled = true;
            this.BirthLongitudeTextBox.Text = "";
            this.BirthLongitudeTextBox.Enabled = true;
            #endregion
            #region ComplexionData
            this.SkinToneTagTextBox.Text = "";
            this.SkinToneTagTextBox.Enabled = true;
            this.SkinToneTagTextBox.Visible = true;
            this.SkinToneValidDateComboBox.Items.Add(System.DateTime.Now.ToString("yyyy-MM-dd"));
            this.SkinToneValidDateComboBox.Items.Add("Other date");
            this.SkinToneValidDateComboBox.Enabled = true;
            this.SkinToneValidDateComboBox.Visible = true;
            this.SkinToneLabel.Visible = false;
            this.AddSkinToneButton.Visible = true;
            this.AddSkinToneButton.Enabled = true;
            #endregion
            #region EyeData
            this.EyeColorValidDateComboBox.Visible = false;
            this.SelEyeColorTextBox.Width = 201;
            this.SelEyeColorTextBox.Text = "";
            this.SelEyeColorTextBox.Enabled = true;
            this.SelEyeColorTextBox.Visible = true;
            this.AddEyeColorButton.Text = "Add eye color";
            this.AddEyeColorButton.Enabled = true;
            this.AddEyeColorButton.Visible = true;
            #endregion
            #region GenderData
            if (linwin.getUserRightsValue() > 4)
            {
                this.GenderInfoLabel.Visible = true;
                this.AddGenderInfoButton.Visible = true;
                this.AddGenderInfoButton.Enabled = true;
                this.GenderTypeTextBox.Text = "";
                this.GenderTypeTextBox.Enabled = true;
                this.GenderInfoValidDateComboBox.Items.Add(System.DateTime.Now.ToString("yyyy-MM-dd"));
                this.GenderInfoValidDateComboBox.Items.Add("Other date");
                this.GenderInfoValidDateComboBox.Enabled = true;
                this.GenderInfoValidDateComboBox.Visible = true;
                this.GdrLengthTextBox.Text = "";
                this.GdrLengthTextBox.Enabled = true;
                this.GdrLengthTextBox.Visible = true;
                this.GdrCircumfTextBox.Text = "";
                this.GdrCircumfTextBox.Enabled = true;
                this.GdrCircumfTextBox.Visible = true;
                this.GdrLookTypeTextBox.Text = "";
                this.GdrLookTypeTextBox.Enabled = true;
                this.GdrLookTypeTextBox.Visible = true;
                this.GdrBehaveTypeTextBox.Text = "";
                this.GdrBehaveTypeTextBox.Enabled = true;
                this.GdrBehaveTypeTextBox.Visible = true;
                this.GdrPresentTextBox.Text = "";
                this.GdrPresentTextBox.Enabled = true;
                this.GdrPresentTextBox.Visible = true;
            }
            else
            {
                this.GenderInfoLabel.Visible = false;
                this.AddGenderInfoButton.Visible = false;
                this.AddGenderInfoButton.Enabled = false;
                this.GenderTypeTextBox.Text = "";
                this.GenderTypeTextBox.Enabled = true;
                this.GenderInfoValidDateComboBox.Items.Clear();
                this.GenderInfoValidDateComboBox.Enabled = false;
                this.GenderInfoValidDateComboBox.Visible = false;
                this.GdrLengthTextBox.Text = "";
                this.GdrLengthTextBox.Enabled = false;
                this.GdrLengthTextBox.Visible = false;
                this.GdrCircumfTextBox.Text = "";
                this.GdrCircumfTextBox.Enabled = false;
                this.GdrCircumfTextBox.Visible = false;
                this.GdrLookTypeTextBox.Text = "";
                this.GdrLookTypeTextBox.Enabled = false;
                this.GdrLookTypeTextBox.Visible = false;
                this.GdrBehaveTypeTextBox.Text = "";
                this.GdrBehaveTypeTextBox.Enabled = false;
                this.GdrBehaveTypeTextBox.Visible = false;
                this.GdrPresentTextBox.Text = "";
                this.GdrPresentTextBox.Enabled = false;
                this.GdrPresentTextBox.Visible = false;
            }
            #endregion
            #region LengthData
            this.LengthLabel.Visible = false;
            this.AddLengthInfoButton.Visible = true;
            this.AddLengthInfoButton.Enabled = true;
            this.LengthTextBox.Text = "";
            this.LengthTextBox.Width = 116;
            this.LengthTextBox.Enabled = true;
            this.LengthTextBox.Visible = true;
            this.LengthValidDateComboBox.Items.Add(System.DateTime.Now.ToString("yyyy-MM-dd"));
            this.LengthValidDateComboBox.Items.Add("Other date");
            this.LengthValidDateComboBox.Enabled = true;
            this.LengthValidDateComboBox.Visible = true;
            #endregion
            #region WeightData
            this.WeightLabel.Visible = false;
            this.AddWeightInfoButton.Visible = true;
            this.AddWeightInfoButton.Enabled = true;
            this.WeightTextBox.Text = "";
            this.WeightTextBox.Width = 116;
            this.WeightTextBox.Enabled = true;
            this.WeightTextBox.Visible = true;
            this.WeightValidDateComboBox.Items.Add(System.DateTime.Now.ToString("yyyy-MM-dd"));
            this.WeightValidDateComboBox.Items.Add("Other date");
            this.LengthValidDateComboBox.Enabled = true;
            this.LengthValidDateComboBox.Visible = true;
            #endregion
            #region ChestData
            this.ChestDataLabel.Visible = false;
            this.AddChestInfoButton.Visible = true;
            this.AddChestInfoButton.Enabled = true;
            this.ChestTypeTextBox.Text = "";
            this.ChestTypeTextBox.Enabled = true;
            this.ChestTypeTextBox.Width = 116;
            this.ChestTypeTextBox.Visible = true;
            this.ChestCircTextBox.Text = "";
            this.ChestCircTextBox.Enabled = true;
            this.ChestCircTextBox.Visible = true;
            this.ChestValidDateComboBox.Items.Add(System.DateTime.Now.ToString("yyyy-MM-dd"));
            this.ChestValidDateComboBox.Items.Add("Other date");
            this.ChestValidDateComboBox.Enabled = true;
            this.ChestValidDateComboBox.Visible = true;
            this.ChestSizeTextBox.Text = "";
            this.ChestSizeTextBox.Enabled = true;
            this.ChestSizeTextBox.Visible = true;
            #endregion
            #region HairData
            this.HairDataLabel.Visible = false;
            this.AddHairInfoButton.Visible = true;
            this.AddHairInfoButton.Enabled = true;
            // --- Color ---
            this.HairColorTextBox.Visible = false;
            this.hairColorCmbBx.Items.Clear();
            this.hairColorCmbBx.Items.Add("Select color");
            for (int i = 0; i < actorClass.noOfHairColorCategories; i++)
                this.hairColorCmbBx.Items.Add(actorClass.hairColorCategory[i].tag);
            this.hairColorCmbBx.Items.Add("Add color...");
            this.hairColorCmbBx.Enabled = true;
            this.hairColorCmbBx.Visible = true;
            // --- Texture ---
            this.HairTextureTypeTextBox.Visible = false;
            this.hairTextureCmbBx.Items.Clear();
            this.hairTextureCmbBx.Items.Add("Select texture");
            this.hairTextureCmbBx.Items.Add("Straight");
            this.hairTextureCmbBx.Items.Add("Wavy");
            this.hairTextureCmbBx.Items.Add("Curly");
            this.hairTextureCmbBx.Items.Add("Coily");
            this.hairTextureCmbBx.Items.Add("Frizzy");
            this.hairTextureCmbBx.Enabled = true;
            this.hairTextureCmbBx.Visible = true;
            // --- Length ---
            this.HairLengthTextBox.Visible = false;
            this.hairLengthCmbBx.Items.Clear();
            this.hairLengthCmbBx.Items.Add("Select length");
            this.hairLengthCmbBx.Items.Add("Short");
            this.hairLengthCmbBx.Items.Add("Neck");
            this.hairLengthCmbBx.Items.Add("Shoulder");
            this.hairLengthCmbBx.Items.Add("MidBack");
            this.hairLengthCmbBx.Items.Add("Waist");
            this.hairLengthCmbBx.Items.Add("Ass");
            this.hairLengthCmbBx.Items.Add("Long");
            this.hairLengthCmbBx.Enabled = true;
            this.hairLengthCmbBx.Visible = true;
            this.hairValidDateTxtBx.Visible = false;
            this.HairDataValidDateComboBox.Items.Add(System.DateTime.Now.ToString("yyyy-MM-dd"));
            this.HairDataValidDateComboBox.Items.Add("Other date");
            this.HairDataValidDateComboBox.Enabled = true;
            #endregion
            #region MarkingsInfoData
            // UndefMarking, Scar, Freckles, Birthmark, Tattoo, Piercing
            this.MarkingTypeComboBox.Items.Clear();
            this.MarkingTypeComboBox.Items.Add("Select marking");
            this.MarkingTypeComboBox.Items.Add("Scar");
            this.MarkingTypeComboBox.Items.Add("Freckles");
            this.MarkingTypeComboBox.Items.Add("Birthmarks");
            this.MarkingTypeComboBox.Items.Add("Tattoo");
            this.MarkingTypeComboBox.Items.Add("Piercings");
            this.MarkingTypeComboBox.Enabled = true;
            this.MarkingTypeComboBox.Visible = true;
            this.MarkingPosTextBox.Text = "";
            this.MarkingPosTextBox.Enabled = true;
            this.MarkingPosTextBox.Visible = true;
            this.MarkingMotifTextBox.Text = "";
            this.MarkingMotifTextBox.Enabled = true;
            this.MarkingMotifTextBox.Visible = true;
            this.MarkingDateTextBox.Enabled = false;
            this.MarkingDateTextBox.Visible = false;
            this.MarkingsValidDateComboBox.Items.Clear();
            this.MarkingsValidDateComboBox.Items.Add(System.DateTime.Now.ToString("yyyy-MMM-dd"));
            this.MarkingsValidDateComboBox.Items.Add("Other date");
            this.MarkingsValidDateComboBox.Enabled = true;
            this.MarkingsValidDateComboBox.Visible = true;
            this.AddMarkingDataButton.Visible = true;
            this.AddMarkingDataButton.Enabled = true;
            #endregion
            #region OccupationData
            this.occupationStartTxtBx.Visible = false;
            this.ActorOccupationStartDateComboBox.Items.Add(System.DateTime.Now.ToString("yyyy-MM-dd"));
            this.ActorOccupationStartDateComboBox.Items.Add("Other date");
            this.ActorOccupationStartDateComboBox.Enabled = true;
            this.ActorOccupationStartDateComboBox.Visible = true;
            this.OccupTitleTextBox.Text = "";
            this.OccupTitleTextBox.Enabled = true;
            this.OccupTitleTextBox.Visible = true;
            this.ActorOccupationCompanyTextBox.Text = "";
            this.ActorOccupationCompanyTextBox.Enabled = true;
            this.ActorOccupationCompanyTextBox.Visible = true;
            this.ActorEmployAddressTextBox.Text = "";
            this.ActorEmployAddressTextBox.Enabled = true;
            this.ActorEmployAddressTextBox.Visible = true;
            this.ActorEmployZipCodeTextBox.Text = "";
            this.ActorEmployZipCodeTextBox.Enabled = true;
            this.ActorEmployZipCodeTextBox.Visible = true;
            this.ActorEmpoyAreanameTextBox.Text = "";
            this.ActorEmpoyAreanameTextBox.Enabled = true;
            this.ActorEmpoyAreanameTextBox.Visible = true;
            this.ActorEmployCitynameTextBox.Text = "";
            this.ActorEmployCitynameTextBox.Enabled = true;
            this.ActorEmployCitynameTextBox.Visible = true;
            this.ActorEmployCountryTextBox.Text = "";
            this.ActorEmployCountryTextBox.Enabled = true;
            this.ActorEmployCountryTextBox.Visible = true;
            this.occupationEndTxtBx.Visible = false;
            this.ActorOccupationEndDaeComboBox.Items.Add(System.DateTime.Now.ToString("yyyy-MM-dd"));
            this.ActorOccupationEndDaeComboBox.Items.Add("Not known");
            this.ActorOccupationEndDaeComboBox.Items.Add("Other date");
            this.ActorOccupationEndDaeComboBox.Enabled = true;
            this.ActorOccupationEndDaeComboBox.Visible = true;
            this.AddOccupationDataButton.Visible = true;
            this.AddOccupationDataButton.Enabled = true;
            #endregion
            #region ResicenceData
            this.ResidAddressTextBox.Text = "";
            this.ResidAddressTextBox.Enabled = true;
            this.ResidAddressTextBox.Visible = true;
            this.residenceStartTxtBx.Visible = false;
            this.ResidStartDateComboBox.Items.Clear();
            this.ResidStartDateComboBox.Items.Add(System.DateTime.Now.ToString("yyyy-MM-dd"));
            this.ResidStartDateComboBox.Items.Add("Other date");
            this.ResidStartDateComboBox.Enabled = true;
            this.ResidStartDateComboBox.Visible = true;
            this.ResidZipCodeTextBox.Text = "";
            this.ResidZipCodeTextBox.Enabled = true;
            this.ResidZipCodeTextBox.Visible = true;
            this.ResidAreanameTextBox.Text = "";
            this.ResidAreanameTextBox.Enabled = true;
            this.ResidAreanameTextBox.Visible = true;
            this.AddResidenceButton.Visible = true;
            this.AddResidenceButton.Enabled = true;
            this.ResidCitynameTextBox.Text = "";
            this.ResidCitynameTextBox.Enabled = true;
            this.ResidCitynameTextBox.Visible = true;
            this.residenceEndTxtBx.Visible = false;
            this.ResidEndDateComboBox.Items.Clear();
            this.ResidEndDateComboBox.Items.Add(System.DateTime.Now.ToString("yyyy-MM-dd"));
            this.ResidEndDateComboBox.Items.Add("Unknown");
            this.ResidEndDateComboBox.Items.Add("Other date");
            this.ResidEndDateComboBox.Enabled = true;
            this.ResidEndDateComboBox.Visible = true;
            this.ResidCountryTextBox.Text = "";
            this.ResidCountryTextBox.Enabled = true;
            this.ResidCountryTextBox.Visible = true;
            #endregion
            #region AttendedEventsData
            this.AttEventLabel.Visible = false;
            this.addAttdEventBtn.Enabled = true;
            this.addAttdEventBtn.Visible = true;
            this.EventIdTextBox.Enabled = false;
            this.AttEvtIDCmbBx.Items.Clear();
            string[] evtFlePths;
            evtFlePths = System.IO.Directory.GetFiles(rootPath + "\\EventData\\", "EventData_*.edf");
            this.AttEvtIDCmbBx.Items.Add("Select event");
            foreach (var efp in evtFlePths)
            {
                int dp0 = efp.LastIndexOf("\\");
                this.AttEvtIDCmbBx.Items.Add(efp.Substring(dp0 + 1, efp.Length - dp0 - 1));
            }
            this.AttEvtIDCmbBx.Items.Add("Add event...");
            this.AttEvtIDCmbBx.Enabled = true;
            this.AttEvtIDCmbBx.Visible = true;
            this.EventIdTextBox.Visible = false;
            this.AttdEventsDateComboBox.Items.Clear();
            this.AttdEventsDateComboBox.Items.Add(System.DateTime.Now.ToString("yyyyy-MM-dd"));
            this.AttdEventsDateComboBox.Items.Add("Other date");
            this.AttdEventsDateComboBox.Enabled = true;
            #endregion
            #region RelImagesData
            //this.ViewSelActorRelImageButton.Enabled = false;
            this.ViewSelActorRelImageButton.Visible = false;
            this.ActorRelImageComboBox.Width = 201;
            this.ActorRelImageComboBox.Items.Clear();
            this.ActorRelImageComboBox.Items.Add("Select image");
            this.ActorRelImageComboBox.Items.Add("Find image...");
            this.ActorRelImageComboBox.Items.Add("Update root");
            #endregion
            #region rootDir
            this.updRootBtn.Text = "Set";
            this.updRootBtn.Visible = true;
            this.updRootBtn.Enabled = true;
            this.rootDirTxtBx.Visible = false;
            #endregion
        }
        private void ActiveArtistsComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            iAmWorking = true;
            this.UseWaitCursor = true;
            if (this.ActiveArtistsComboBox.SelectedItem.ToString() == "Add artist...")
            {
                addNewArtist(sender, e);
            }
            else if ((this.ActiveArtistsComboBox.Text != "Add artist...") && (this.ActiveArtistsComboBox.Text != "Select artist"))
            {
                // Operator selected an artist from the database.
                string selArtist = this.ActiveArtistsComboBox.Text;

                if (selArtist == "") return;

                loadSelArtist(selArtist, sender, e);
            }
            this.UseWaitCursor = false;
            iAmWorking = false;
        }
        #region identityData
        private void ArtistIdentityEnterTextBox_TextChanged(object sender, EventArgs e)
        {
            if (!(iAmWorking))
            {
                if (NameTypeComboBox.SelectedText == "Add item...")
                {
                    // Expected entry format "<surname>[[ |,]<midname>][[ |,]<familyname>][ |,]<nametype>
                    //actorClass.addUserName(< Type >, < surn >, < midn >, < famn >);
                    string sEnteredData = ArtistIdentityEnterTextBox.Text;
                    string sPartOne = "";
                    string sPartTwo = "";
                    string sPartThree = "";
                    string sPartFour = "";
                    char cDelim = ' ';
                    int idp = sEnteredData.IndexOf(cDelim);
                    if ((idp < 0) || (idp > sEnteredData.Length))
                    {
                        cDelim = ',';
                        idp = sEnteredData.IndexOf(cDelim);
                    }
                    if ((idp > 0) && (idp < sEnteredData.Length - 2))
                    {
                        sPartOne = sEnteredData.Substring(0, idp);
                        // Expected entry format <midname>][[ |,]<familyname>][ |,]<nametype>
                        sEnteredData = sEnteredData.Substring(idp + 2);
                        idp = sEnteredData.IndexOf(cDelim);
                        if ((idp > 0) && (idp < sEnteredData.Length - 2))
                        {
                            sPartTwo = sEnteredData.Substring(0, idp);
                            // Expected entry format <familyname>][ |,]<nametype>
                            sEnteredData = sEnteredData.Substring(idp + 2);
                            idp = sEnteredData.IndexOf(cDelim);
                            if ((idp > 0) && (idp < sEnteredData.Length - 2))
                            {
                                sPartThree = sEnteredData.Substring(0, idp);
                                sEnteredData = sEnteredData.Substring(idp + 2);
                                idp = sEnteredData.IndexOf(cDelim);
                                if ((idp > 0) && (idp < sEnteredData.Length - 2))
                                {
                                    sPartFour = sEnteredData.Substring(0, idp);
                                    actorClass.addUserName(sPartTwo, sPartOne, sPartThree, sPartFour);
                                }
                                else
                                {
                                    actorClass.addUserName(sPartTwo, sPartOne, sPartThree, sEnteredData);
                                }
                            }
                            else
                            {
                                // Name and Type assumed.
                                actorClass.addUserName(sPartTwo, sPartOne, "", "");
                            }
                        }
                        else
                        {
                            // Alias name assumed.
                            actorClass.addUserName("Alias", sPartOne, "", "");
                        }
                    }
                    toSave.actorChanges = true;
                }
                else
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
                                        // When a actor has been typed in.
                                        setActorGUIInfo(arrayOfActors[0], false);
                                    }
                                }
                            }
                        }
                    }
                    else
                        setInformationText("Searchphrase was not found.", informationType.INFO, sender, e);
                }
                iAmWorking = false;
            }
        }
        private void ViewSelArtistDataButton_Click(object sender, EventArgs e)
        {
            if (!(iAmWorking))
            {
                ViewSelArtistDataButton.Enabled = false;
                ViewSelArtistDataButton.Visible = false;
                string selArtist = this.ActiveArtistsComboBox.Text;

                if (selArtist == "") return;

                loadSelArtist(selArtist, sender, e);

                //ViewSelArtistDataButton.Text = "Save data";
            }
        }
        private void ViewSelArtistDataButton_SelectedIndexChanged(object sender, EventArgs e)
        {
            setInformationText("Amaze and behold we are actually in here!", informationType.ERROR, sender, e);
            ViewSelArtistDataButton.Visible = true;
            ArtistIdentityEnterTextBox.Visible = false;
        }
        private void DiscardActorDataChangesButton_Click(object sender, EventArgs e)
        {
            getImageMetadataValues(loadedImageFileName, sender, e);
            SaveActorDataChangesButton.Enabled = false;
            toSave.actorChanges = false;
            DiscardActorDataChangesButton.Enabled = false;
        }
        private void SaveActorDataChangesButton_Click(object sender, EventArgs e)
        {
            if (SaveActorDataChangesButton.Text == "Edit actordata")
            {
                SaveActorDataChangesButton.Text = "Save data changes";
                ViewSelArtistDataButton.Visible = false;
                ArtistIdentityEnterTextBox.Visible = true;
                ArtistIdentityEnterTextBox.Enabled = true;
                SelContactTypeTextBox.Enabled = true;
                #region birthData
                BirthStreetAddrTextBox.Enabled = true;
                BirthZipCodeTextBox.Enabled = true;
                BirthAreaNameTextBox.Enabled = true;
                BirthCitynameTextBox.Enabled = true;
                BirthCountryTextBox.Enabled = true;
                BirthDateTextBox.Enabled = true;
                SocSecNumberTextBox.Enabled = true;
                #endregion
                #region SkinTone
                SkinToneDateTimePicker.Visible = false;
                SkinToneTagTextBox.Enabled = true;
                SkinToneTagTextBox.Visible = true;
                SkinToneValidDateComboBox.Enabled = true;
                SkinToneValidDateComboBox.Visible = true;
                #endregion
                #region EyeColor
                EyeColorValidDateComboBox.Enabled = true;
                EyeColorValidDateComboBox.Visible = true;
                SelEyeColorTextBox.Enabled = true;
                SelEyeColorTextBox.Visible = true;
                #endregion
                #region GenderInfo
                if (linwin.getUserRightsValue() >= 4)
                {
                    GenderInfoValidDateComboBox.Enabled = true;
                    GenderInfoValidDateComboBox.Visible = true;
                    GenderTypeTextBox.Enabled = true;
                    GenderTypeTextBox.Visible = true;
                    GdrLengthTextBox.Enabled = true;
                    GdrLengthTextBox.Visible = true;
                    GdrLookTypeTextBox.Enabled = true;
                    GdrLookTypeTextBox.Visible = true;
                    GdrBehaveTypeTextBox.Enabled = true;
                    GdrBehaveTypeTextBox.Visible = true;
                    GdrPresentTextBox.Enabled = true;
                    GdrPresentTextBox.Visible = true;
                }
                #endregion
                #region LengthInfo
                LengthValidDateComboBox.Enabled = true;
                LengthValidDateComboBox.Visible = true;
                LengthTextBox.Enabled = true;
                LengthTextBox.Visible = true;
                #endregion
                #region WeightInformation
                WeightValidDateComboBox.Enabled = true;
                WeightValidDateComboBox.Visible = true;
                WeightTextBox.Enabled = true;
                WeightTextBox.Visible = true;
                #endregion
                #region ChestData
                ChestValidDateComboBox.Enabled = true;
                ChestValidDateComboBox.Visible = true;
                ChestTypeTextBox.Enabled = true;
                ChestTypeTextBox.Visible = true;
                ChestCircTextBox.Enabled = true;
                ChestCircTextBox.Visible = true;
                ChestSizeTextBox.Enabled = true;
                ChestSizeTextBox.Visible = true;
                #endregion
                #region HairData
                HairDataValidDateComboBox.Enabled = true;
                HairDataValidDateComboBox.Visible = true;
                HairColorTextBox.Enabled = true;
                HairColorTextBox.Visible = true;
                HairTextureTypeTextBox.Enabled = true;
                HairTextureTypeTextBox.Visible = true;
                HairLengthTextBox.Enabled = true;
                HairLengthTextBox.Visible = true;
                #endregion
                #region MarkingsData
                MarkingTypeComboBox.Enabled = true;
                MarkingTypeComboBox.Visible = true;
                MarkingsValidDateComboBox.Enabled = true;
                MarkingsValidDateComboBox.Visible = true;
                MarkingTypeComboBox.Enabled = true;
                MarkingTypeComboBox.Visible = true;
                #endregion
                #region OccupationData
                ActorOccupationStartDateComboBox.Enabled = true;
                ActorOccupationStartDateComboBox.Visible = true;
                ActorOccupationEndDaeComboBox.Enabled = true;
                ActorOccupationEndDaeComboBox.Visible = true;
                OccupTitleTextBox.Enabled = true;
                OccupTitleTextBox.Visible = true;
                ActorOccupationCompanyTextBox.Enabled = true;
                ActorOccupationCompanyTextBox.Visible = true;
                ActorEmployAddressTextBox.Enabled = true;
                ActorEmployAddressTextBox.Visible = true;
                ActorEmployZipCodeTextBox.Enabled = true;
                ActorEmployZipCodeTextBox.Visible = true;
                ActorEmpoyAreanameTextBox.Enabled = true;
                ActorEmpoyAreanameTextBox.Visible = true;
                ActorEmployCitynameTextBox.Enabled = true;
                ActorEmployCitynameTextBox.Visible = true;
                ActorEmployCountryTextBox.Enabled = true;
                ActorEmployCountryTextBox.Visible = true;
                AddOccupationDataButton.Enabled = false;
                #endregion
                #region ResidenceData
                ResidStartDateComboBox.Enabled = true;
                ResidStartDateComboBox.Visible = true;
                ResidEndDateComboBox.Enabled = true;
                ResidEndDateComboBox.Visible = true;
                ResidAddressTextBox.Enabled = true;
                ResidZipCodeTextBox.Enabled = true;
                ResidAreanameTextBox.Enabled = true;
                ResidCitynameTextBox.Enabled = true;
                ResidCountryTextBox.Enabled = true;
                AddResidenceButton.Enabled = false;
                ResidAddressTextBox.Visible = true;
                ResidZipCodeTextBox.Visible = true;
                ResidAreanameTextBox.Visible = true;
                ResidCitynameTextBox.Visible = true;
                ResidCountryTextBox.Visible = true;
                AddResidenceButton.Visible = false;
                #endregion
                #region EventsData
                AttdEventsDateComboBox.Enabled = true;
                AttdEventsDateComboBox.Visible = true;
                activeEventComboBox.Enabled = true;
                activeEventComboBox.Visible = true;
                AttEvtIDCmbBx.Enabled = true;
                AttEvtIDCmbBx.Visible = true;
                addAttdEventBtn.Visible = false;
                #endregion
                #region ImagesData
                ActorRelImageComboBox.Enabled = true;
                ActorRelImageComboBox.Visible = true;
                //ViewSelActorRelImageButton.Enabled = true;
                #endregion
            }
            else
            {
                // TODO - Check that all actor data is handled.
                string saveActorFilePaths = "";
                string sSelItm = ActiveArtistsComboBox.SelectedItem.ToString();
                if ((linwin.getActorStoragePath() != "") && (System.IO.Directory.Exists(linwin.getActorStoragePath())))
                    saveActorFilePaths = linwin.getActorStoragePath() + "\\" + sSelItm;
                else if (System.IO.Directory.Exists(rootPath + "\\ActorData\\"))
                    saveActorFilePaths = rootPath + "\\ActorData\\" + sSelItm;

                if (actorClass.saveActorData(ActiveArtistsComboBox.SelectedItem.ToString(), saveActorFilePaths))
                {
                    setInformationText("Successfully stored actor data.", informationType.INFO, sender, e);
                    toSave.actorChanges = true;
                }
                else
                    setInformationText("Could not store actor data!", informationType.ERROR, sender, e);
            }
        }
        private void NameTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selNmeIdx = Math.Max(NameTypeComboBox.SelectedIndex, 0);
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
        private void SelNameTypeTextBox_TextChanged(object sender, EventArgs e)
        {
            AddNewNameButton.Enabled = true;
        }
        private void AddNewNameButton_Click(object sender, EventArgs e)
        {
            if (ActiveArtistsComboBox.Text == "Add artist...")
            {
                if (actorClass != null)
                {
                    string setName = this.SelNameTypeTextBox.Text;
                    // Expected format : surname[[, | ]midname][[, | ]familyname]
                    string setSurn = "";
                    string setMidn = "";
                    string setFamn = "";
                    char delim = ' ';
                    int delimlength = 1;
                    if (setName.Contains(","))
                    {
                        delim = ',';
                        delimlength = 2;
                    }
                    int dp = setName.IndexOf(delim);
                    if ((dp > 0) && (dp < setName.Length - 1))
                    {
                        setSurn = setName.Substring(0, dp);
                        setName = setName.Substring(dp + delimlength, setName.Length - dp - delimlength);
                        dp = setName.IndexOf(delim);
                        if ((dp > 0) && (dp < setName.Length - 1))
                        {
                            setMidn = setName.Substring(0, dp);
                            setName = setName.Substring(dp + delimlength, setName.Length - dp - delimlength);
                            dp = setName.IndexOf(delim);
                            while ((dp > 0) && (dp < setName.Length - 1))
                            {
                                setMidn = setMidn + " " + setName.Substring(0, dp);
                                setName = setName.Substring(dp + delimlength, setName.Length - dp - delimlength);
                                dp = setName.IndexOf(delim);
                            }
                            setFamn = setName;
                        }
                        else
                            setFamn = setName;
                    }
                    else
                        setSurn = setName;

                    if (this.NameTypeComboBox.SelectedItem.ToString() == "Birth name")
                        actorClass.addUserName("Birth", setSurn, setMidn, setFamn);
                    else if (this.NameTypeComboBox.SelectedItem.ToString() == "Taken name")
                        actorClass.addUserName("Taken", setSurn, setMidn, setFamn);
                    else if (this.NameTypeComboBox.SelectedItem.ToString() == "Married name")
                        actorClass.addUserName("Married", setSurn, setMidn, setFamn);
                    else if (this.NameTypeComboBox.SelectedItem.ToString() == "Alias name")
                        actorClass.addUserName("Alias", setSurn, setMidn, setFamn);
                    else if (this.NameTypeComboBox.SelectedItem.ToString() == "Nickname")
                        actorClass.addUserName("Nick", setSurn, setMidn, setFamn);
                    else
                        actorClass.addUserName("Undef", setSurn, setMidn, setFamn);
                    toSave.actorChanges = true;
                }
            }
            else
            {
                AddNewNameButton.Enabled = false;
            }
        }
        #endregion
        #region contactData
        private void ActorContactTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sSelCtctTpe = ActorContactTypeComboBox.SelectedText;
            if ((sSelCtctTpe.ToLower() != "Select...") && (sSelCtctTpe.ToLower() != "Add Item..."))
            {
                // Existing type selected.
                int selCtcTpe = Math.Max(ActorContactTypeComboBox.SelectedIndex, 0);
                SelContactTypeTextBox.Text = actorClass.getUserContactPath(selCtcTpe - 1);
                ActorContactTypeComboBox.SelectedIndex = selCtcTpe;
            }
        }
        private void SelContactTypeTextBox_TextChanged(object sender, EventArgs e)
        {
            AddContactButton.Enabled = true;
        }
        private void AddContactButton_Click(object sender, EventArgs e)
        {
            if (ActorContactTypeComboBox.SelectedItem.ToString() == "New type")
            {
                string setNewContactData = SelContactTypeTextBox.Text;
                // Expected format : <tag>[[, | ]<description>][[, | ]<level>]
                char delimsign = ' ';
                int delimlength = 1;
                if (setNewContactData.Contains(","))
                {
                    delimsign = ',';
                    delimlength = 2;
                }
                string setTag = "";
                string setDescr = "";
                string setLevel = "";
                int dp = setNewContactData.IndexOf(delimsign);
                if ((dp > 0) && (dp < setNewContactData.Length - 1))
                {
                    setTag = setNewContactData.Substring(0, dp);
                    setNewContactData = setNewContactData.Substring(dp + delimlength, setNewContactData.Length - dp - delimlength);
                    dp = setNewContactData.LastIndexOf(delimsign);
                    if ((dp > 0) && (dp < setNewContactData.Length - 1))
                    {
                        setDescr = setNewContactData.Substring(0, dp);
                        setLevel = setNewContactData.Substring(dp + delimlength, setNewContactData.Length - dp - delimlength);
                    }
                    else
                        setDescr = setNewContactData;
                }
                else
                    setTag = setNewContactData;
                bool foundTag = false;
                for (int i = 0; i < linwin.noOfContactWays; i++)
                {
                    if (linwin.getContactWayTag(i) == setTag)
                    {
                        setInformationText("Contact tag already exists.", informationType.INFO, sender, e);
                        foundTag = true;
                    }
                }
                if (!(foundTag))
                {
                    linwin.contactWays[linwin.noOfContactWays].tag = setTag;
                    linwin.contactWays[linwin.noOfContactWays].description = setDescr;
                    if (setLevel.ToLower() == "open")
                        linwin.contactWays[linwin.noOfContactWays++].catReqReq = LoginForm.rightsLevel.Open;
                    else if (setLevel.ToLower() == "limited")
                        linwin.contactWays[linwin.noOfContactWays++].catReqReq = LoginForm.rightsLevel.Limited;
                    else if (setLevel.ToLower() == "medium")
                        linwin.contactWays[linwin.noOfContactWays++].catReqReq = LoginForm.rightsLevel.Medium;
                    else if (setLevel.ToLower() == "relative")
                        linwin.contactWays[linwin.noOfContactWays++].catReqReq = LoginForm.rightsLevel.Relative;
                    else if (setLevel.ToLower() == "secret")
                        linwin.contactWays[linwin.noOfContactWays++].catReqReq = LoginForm.rightsLevel.Secret;
                    else if (setLevel.ToLower() == "qualifsecret")
                        linwin.contactWays[linwin.noOfContactWays++].catReqReq = LoginForm.rightsLevel.QualifSecret;
                    else
                        linwin.contactWays[linwin.noOfComplexions++].catReqReq = LoginForm.rightsLevel.Undefined;
                    actorClass.addContactCategory(setTag, setDescr, setLevel);
                }
            }
            else if (AddContactButton.Text == "Reach")
            {
                if ((ActorContactTypeComboBox.SelectedItem.ToString().ToLower().Contains("phone")) ||
                    (ActorContactTypeComboBox.SelectedItem.ToString().ToLower().Contains("mobile")) ||
                    (ActorContactTypeComboBox.SelectedItem.ToString().ToLower().Contains("tel")))
                {
                    setInformationText("Phone call handling is ot implemented.", informationType.INFO, sender, e);
                    string selectedInfo = SelContactTypeTextBox.Text;

                    // TODO - Handle phone connection
                }
                else if (ActorContactTypeComboBox.SelectedItem.ToString().ToLower().Contains("mail"))
                {
                    setInformationText("Mail handling is ot implemented.", informationType.INFO, sender, e);
                    //object mailCommand = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Clients\Mail\" + mailClient.ToString() + @"\shell\open\command", "", "none");
                    string sMailPath = SelContactTypeTextBox.Text;
                    //object mailCommand = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Clients\Mail\" + mailClient.ToString(), "", "none");
                    // TODO - Handle mail connection
                    MailMessage email = new MailMessage("from@email.com", sMailPath, "subject", "body");
                }
                else if (ActorContactTypeComboBox.SelectedItem.ToString().ToLower().Contains("twitt"))
                {
                    setInformationText("Twitter handling is ot implemented.", informationType.INFO, sender, e);
                    // TODO - Handle twitter connection
                }
                else if (ActorContactTypeComboBox.SelectedItem.ToString().ToLower().Contains("insta"))
                {
                    setInformationText("Instagram handling is ot implemented.", informationType.INFO, sender, e);
                    // TODO - Handle Instagram connection.
                }
                else if (ActorContactTypeComboBox.SelectedItem.ToString().ToLower().Contains("dir"))
                {
                    if ((SelContactTypeTextBox.Text != "") && (System.IO.Directory.Exists(SelContactTypeTextBox.Text)))
                    {
                        fixImageDisplayAndSortingButtons(SelContactTypeTextBox.Text, sender, e);
                        tabControl.SelectedTab = tabControl.TabPages["sortingTabPage"];
                    }
                    setInformationText("Should open " + SelContactTypeTextBox.Text, informationType.INFO, sender, e);
                }
                else
                {
                    int iSelIdx = Math.Max(ActorContactTypeComboBox.SelectedIndex, 0);
                    if (iSelIdx > 0)
                    {
                        string url = actorClass.getUserContactPath(iSelIdx - 1);
                        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                        {
                            url = url.Replace("&", "^&");
                            Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
                        }
                        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                        {
                            Process.Start("xdg-open", url);
                        }
                        else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                        {
                            Process.Start("open", url);
                        }
                        else
                            setInformationText("Unknown operative, can't start.", informationType.ERROR, sender, e);
                    }
                    else
                        setInformationText("Selected index, " + iSelIdx.ToString() + ", is out of range", informationType.INFO, sender, e);
                }
            }
            else
            {
                string setContactTag = ActorContactTypeComboBox.SelectedItem.ToString();
                string setContactPath = SelContactTypeTextBox.Text;
                actorClass.addUserContact(setContactTag, setContactPath);
            }
            ActorContactTypeComboBox.SelectedIndex = 0;
            SelContactTypeTextBox.Text = "";
        }
        private void AddContactButton_MouseHover(object sender, EventArgs e)
        {
            if (AddContactButton.Text == "Add")
                tt.Show("Add new contact", (Control)sender);
            else if (AddContactButton.Text == "Reach")
                tt.Show("Go to selected contact item.", (Control)sender);
            else
                tt.Show("Dunno???", (Control)sender);
        }
        #endregion
        #region birthData
        private void BirthStreetAddrTextBox_TextChanged(object sender, EventArgs e)
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
            toSave.actorChanges = true;
        }
        private void BirthZipCodeTextBox_TextChanged(object sender, EventArgs e)
        {
            actorClass.setUserBirthZipcode(BirthZipCodeTextBox.Text);
            toSave.actorChanges = true;
        }
        private void BirthAreaNameTextBox_TextChanged(object sender, EventArgs e)
        {
            actorClass.setUserBirthAreaname(BirthAreaNameTextBox.Text);
            toSave.actorChanges = true;
        }
        private void BirthCitynameTextBox_TextChanged(object sender, EventArgs e)
        {
            actorClass.setUserBirthCityname(BirthCitynameTextBox.Text);
            toSave.actorChanges = true;
        }
        private void BirthCountryTextBox_TextChanged(object sender, EventArgs e)
        {
            actorClass.setUserBirthCountry(BirthCountryTextBox.Text);
            toSave.actorChanges = true;
        }
        private void BirthDateTextBox_TextChanged(object sender, EventArgs e)
        {
            actorClass.setUserBirthDate(BirthDateTextBox.Text);
            toSave.actorChanges = true;
        }
        private void SocSecNumberTextBox_TextChanged(object sender, EventArgs e)
        {
            actorClass.setUserBirthSocNo(SocSecNumberTextBox.Text);
            toSave.actorChanges = true;
        }
        private void BirthLatitudeTextBox_TextChanged(object sender, EventArgs e)
        {
            actorClass.setUserBirthLatitude(BirthLatitudeTextBox.Text);
            toSave.actorChanges = true;
        }
        private void BirthLongitudeTextBox_TextChanged(object sender, EventArgs e)
        {
            actorClass.setUserBirthLongitude(BirthLongitudeTextBox.Text);
            toSave.actorChanges = true;
        }
        private void ViewGeoPosButton_Click(object sender, EventArgs e)
        {
            double selectedLatValue = 0;
            string sLatWorkString = BirthLatitudeTextBox.Text;
            string sHemisphere = "";
            string sLatDeg = "";
            string sLatMin = "";
            string sLatSec = "";
            string sLatSemiSec = "";
            string setLatValue = "";
            double selectedLonValue = 0;
            string sDateSide = "";
            string sLonDeg = "";
            string sLonMin = "";
            string sLonSec = "";
            string sLonSemiSec = "";
            string setLonValue = "";
            string sLonWorkString = BirthLongitudeTextBox.Text;

            string url = "https://www.google.com/maps/";

            if ((sLatWorkString != "") && (sLonWorkString != ""))
            {
                // Expected format: GG.MM.ss,zz [[N|n]/[S|s]]
                int dp = sLatWorkString.IndexOf(" ");
                if ((dp > 0) && (dp < sLatWorkString.Length))
                {
                    sHemisphere = sLatWorkString.Substring(dp + 1);
                    sLatWorkString = sLatWorkString.Substring(0, sLatWorkString.Length - dp);
                    dp = sLatWorkString.IndexOf(".");
                    if ((dp > 0) && (dp < sLatWorkString.Length))
                    {
                        sLatDeg = sLatWorkString.Substring(0, dp);
                        sLatWorkString = sLatWorkString.Substring(dp + 1);
                        dp = sLatWorkString.IndexOf(".");
                        if ((dp > 0) && (dp < sLatWorkString.Length))
                        {
                            sLatMin = sLatWorkString.Substring(0, dp);
                            sLatWorkString = sLatWorkString.Substring(dp + 1);
                            dp = sLatWorkString.IndexOf(".");
                            if ((dp > 0) && (dp < sLatWorkString.Length))
                            {
                                dp = sLatWorkString.IndexOf(",");
                                if ((dp > 0) && (dp < sLatWorkString.Length))
                                {
                                    sLatSec = sLatWorkString.Substring(0, dp);
                                    sLatSemiSec = sLatWorkString.Substring(dp + 1);
                                }
                                else
                                {
                                    sLatSec = sLatWorkString;
                                    sLatSemiSec = "0";
                                }
                            }
                            else
                            {
                                setInformationText("Erroneous format, expected format 'GG.MM.SS,ss [[N|n]/[S|s]]'.", informationType.ERROR, sender, e);
                            }
                        }
                        else
                        {
                            setInformationText("Erroneous format, expected format 'GG.MM.SS[,ss] [[N|n]/[S|s]]'.", informationType.ERROR, sender, e);
                            return;
                        }
                    }
                    else
                    {
                        setInformationText("Erroneous format, expected format 'GG.MM.SS[,ss] [[N|n]/[S|s]]'.", informationType.ERROR, sender, e);
                        return;
                    }
                }
                else
                {
                    setInformationText("Erroneous latitude format, expected format 'GG.MM.SS[,ss] [[N|n]/[S|s]]'.", informationType.ERROR, sender, e);
                    return;
                }

                dp = sLonWorkString.IndexOf(" ");
                if ((dp > 0) && (dp < sLonWorkString.Length))
                {
                    sDateSide = sLonWorkString.Substring(dp + 1);
                    sLonWorkString = sLonWorkString.Substring(0, sLonWorkString.Length - dp);
                    dp = sLonWorkString.IndexOf(".");
                    if ((dp > 0) && (dp < sLonWorkString.Length))
                    {
                        sLonDeg = sLonWorkString.Substring(0, dp);
                        sLonWorkString = sLonWorkString.Substring(dp + 1);
                        dp = sLonWorkString.IndexOf(".");
                        if ((dp > 0) && (dp < sLonWorkString.Length))
                        {
                            sLonMin = sLonWorkString.Substring(0, dp);
                            sLonWorkString = sLonWorkString.Substring(dp + 1);
                            dp = sLonWorkString.IndexOf(".");
                            if ((dp > 0) && (dp < sLonWorkString.Length))
                            {
                                dp = sLonWorkString.IndexOf(",");
                                if ((dp > 0) && (dp < sLonWorkString.Length))
                                {
                                    sLonSec = sLonWorkString.Substring(0, dp);
                                    sLonSemiSec = sLonWorkString.Substring(dp + 1);
                                }
                                else
                                {
                                    sLonSec = sLonWorkString;
                                    sLonSemiSec = "0";
                                }
                            }
                            else
                            {
                                setInformationText("Erroneous longitude format, expected format 'GGG.MM.SS[,ss] [[E|e]/[W|w]]'.", informationType.ERROR, sender, e);
                                return;
                            }
                        }
                        else
                        {
                            setInformationText("Erroneous longitude format, expected format 'GGG.MM.SS[,ss] [[E|e]/[W|w]]'.", informationType.ERROR, sender, e);
                            return;
                        }
                    }
                    else
                    {
                        setInformationText("Erroneous longitude format, expected format 'GGG.MM.SS[,ss] [[E|e]/[W|w]]'.", informationType.ERROR, sender, e);
                        return;
                    }
                }
                else
                {
                    setInformationText("Erroneous longitude format, expected format 'GGG.MM.SS[,ss] [[E|e]/[W|w]]'.", informationType.ERROR, sender, e);
                    return;
                }
            }

            if (sHemisphere.ToLower() == "n")
                setLatValue = sLatDeg + "." + sLatMin + sLatSec + sLatSemiSec;
            else
                setLatValue = "-" + sLatDeg + "." + sLatMin + sLatSec + sLatSemiSec;

            if (!(double.TryParse(setLatValue, NumberStyles.Any, CultureInfo.InvariantCulture, out selectedLatValue)))
            {
                setInformationText("Failed to generate Latitude value.", informationType.ERROR, sender, e);
                return;
            }

            if (sDateSide.ToLower() == "e")
                setLonValue = sLonDeg + "." + sLonMin + sLonSec + sLonSemiSec;
            else
                setLonValue = "-" + sLonDeg + "." + sLonMin + sLonSec + sLonSemiSec;

            if (!(double.TryParse(setLonValue, NumberStyles.Any, CultureInfo.InvariantCulture, out selectedLonValue)))
            {
                setInformationText("Failed to generate Longitude value.", informationType.ERROR, sender, e);
                return;
            }

            // https://www.google.com/maps/@59.3324449,18.074283,16z?entry=ttu
            // https://www.google.com/maps/@-44.2773181,171.3828943,9.96z?entry=ttu
            // https://www.google.com/maps/@40.4314699,-80.0629009,12z?entry=ttu
            url = url + "@" + selectedLatValue + "," + selectedLonValue;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                url = url.Replace("&", "^&");
                Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                Process.Start("xdg-open", url);
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                Process.Start("open", url);
            }
        }
        #endregion
        #region complexionInfo
        private void AddSkinToneButton_Click(object sender, EventArgs e)
        {
            string sSetButtonText = AddSkinToneButton.Text;
            if (sSetButtonText == "View Skintone")
            {
                // TODO - Show the skintone
            }
            else if (sSetButtonText == "Add Tag")
            {
                // Format: <tag>[;|,] <Description>[;|,] <Safety Class>
                char chDelim = ';';
                string sWorkTag = "";
                string sWorkDescr = "";
                string sWorkSafeClass = "";
                string sWorkString = SkinToneTagTextBox.Text;
                int dp = sWorkString.IndexOf(chDelim);
                if ((dp < 0) || (dp > sWorkString.Length))
                {
                    chDelim = ',';
                    dp = sWorkString.IndexOf(chDelim);
                }
                if ((dp > 0) && (dp < sWorkString.Length))
                {
                    // <tag>[;|,] <Description>[;|,] <Safety Class>
                    sWorkTag = sWorkString.Substring(0, dp);
                    sWorkString = sWorkString.Substring(dp + 2);
                    dp = sWorkString.IndexOf(chDelim);
                    if ((dp > 0) && (dp < sWorkString.Length))
                    {
                        // <Description>[;|,] <Safety Class>
                        sWorkDescr = sWorkString.Substring(0, dp);
                        sWorkSafeClass = sWorkString.Substring(dp + 2);
                    }
                    else
                    {
                        sWorkDescr = sWorkString;
                        sWorkSafeClass = "Undefined";
                    }
                }
                else
                {
                    sWorkTag = sWorkString;
                    sWorkDescr = "";
                    sWorkSafeClass = "Undefined";
                }
                linwin.addComplexionCategory(sWorkTag, sWorkDescr, sWorkSafeClass);
                SkinToneTagTextBox.Size = new Size(((ActorData.Size.Width - AddSkinToneButton.Size.Width - SkinToneDateTxtBx.Size.Width) - 10), 20);
                toSave.actorChanges = true;
                toSave.userChanges = true;
                saveToolStripMenuItem.Enabled = true;
                saveAsToolStripMenuItem.Enabled = true;
            }
            else if (sSetButtonText == "Add Skintone")
            {
                // Format: <Tag>[[;|,] <RChVal>[[;|,] <GChVal>[[;|,] <BChVal>]]]
                string sWorkString = SkinToneTagTextBox.Text;
                char chDelim = ';';
                string sWorkTag = "";
                string sWorkRCh = "";
                int iRCh = 0;
                string sWorkGCh = "";
                int iGCh = 0;
                string sWorkBCh = "";
                int iBCh = 0;
                string sWorkDate;
                int dp = sWorkString.IndexOf(chDelim);
                if ((dp < 0) || (dp > sWorkString.Length))
                {
                    chDelim = ',';
                    dp = sWorkString.IndexOf(chDelim);
                }
                if ((dp > 0) && (dp < sWorkString.Length))
                {
                    // <Tag>[;|,] <RChVal>[;|,] <GChVal>[;|,] <BChVal>
                    sWorkTag = sWorkString.Substring(0, dp);
                    sWorkString = sWorkString.Substring(dp + 2);
                    dp = sWorkString.IndexOf(chDelim);
                    if ((dp > 0) && (dp < sWorkString.Length))
                    {
                        // <RChVal>[;|,] <GChVal>[;|,] <BChVal>
                        sWorkRCh = sWorkString.Substring(0, dp);
                        sWorkString = sWorkString.Substring(dp + 2);
                        dp = sWorkString.IndexOf(chDelim);
                        if ((dp > 0) && (dp < sWorkString.Length))
                        {
                            // <GChVal>[;|,] <BChVal>
                            sWorkGCh = sWorkString.Substring(0, dp);
                            if (!(int.TryParse(sWorkGCh, out iGCh)))
                                iGCh = 0;
                            sWorkBCh = sWorkString.Substring(dp + 2);
                            if (!(int.TryParse(sWorkBCh, out iBCh)))
                                iBCh = 0;
                        }
                        else
                        {
                            sWorkGCh = sWorkString;
                            if (!(int.TryParse(sWorkGCh, out iGCh)))
                                iGCh = 0;
                            iBCh = 0;
                        }
                    }
                    else
                    {
                        sWorkRCh = sWorkString;
                        if (!(int.TryParse(sWorkRCh, out iRCh)))
                            iRCh = 0;
                    }
                }
                else
                {
                    sWorkTag = sWorkString;
                    iRCh = 0;
                    iGCh = 0;
                    iBCh = 0;
                }
                sWorkDate = SkinToneDateTimePicker.Value.ToString();
                actorClass.setUserSkinTone(actorClass.getNumberOfSkinTones(), sWorkTag, iRCh, iGCh, iBCh, sWorkDate);
                toSave.actorChanges = true;
                saveToolStripMenuItem.Enabled = true;
                saveAsToolStripMenuItem.Enabled = true;
            }
        }
        private void SkinToneTagTextBox_TextChanged(object sender, EventArgs e)
        {
            if ((AddSkinToneButton.Text == "Add Tag") && (noOfChangedGUIs > 0))
            {
                AddSkinToneButton.Enabled = true;
                noOfChangedGUIs = 0;
            }
            else
            {
                AddSkinToneButton.Enabled = false;
                noOfChangedGUIs++;
            }
        }
        private void SkinToneDateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            if ((AddSkinToneButton.Text == "Add Skintone") && (noOfChangedGUIs > 0))
            {
                DateTime sSdt = SkinToneDateTimePicker.Value;
                SkinToneTagTextBox.Enabled = true;
                SkinToneDateTxtBx.Text = sSdt.ToString();
                SkinToneDateTimePicker.Visible = false;
                SkinToneDateTxtBx.Visible = true;
                SkinToneDateTxtBx.Enabled = true;
//                if (SkinToneTagTextBox.Text != "")
//                    actorClass.setUserSkinTone(actorClass.getNumberOfSkinTones(), SkinToneTagTextBox.Text, 0, 0, 0, SkinToneDateTxtBx.Text);
//                else if (SkinToneTagCmbBx.SelectedItem.ToString() != "")
//                    actorClass.setUserSkinTone(actorClass.getNumberOfSkinTones(), SkinToneTagCmbBx.SelectedItem.ToString(), 0, 0, 0, SkinToneDateTxtBx.Text);
                noOfChangedGUIs++;
            }
        }
        private void SkinToneDateTxtBx_TextChanged(object sender, EventArgs e)
        {
            setInformationText("Unexpected entry into complexion date text box change!", informationType.INFO, sender, e);
        }
        private void SkinToneValidDateComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            int iSelIdx = SkinToneValidDateComboBox.SelectedIndex;
            if (iSelIdx == actorClass.getNumberOfSkinTones() + 1)
            {
                // Add Data...
                //SkinToneTagTextBox.Size = new Size(((ActorData.Size.Width - AddSkinToneButton.Size.Width - SkinToneDateTxtBx.Size.Width) - 10), 20);
                SkinToneTagTextBox.Text = "";
                SkinToneTagTextBox.Enabled = false;
                SkinToneTagTextBox.Visible = false;
                SkinToneTagCmbBx.Items.Clear();
                SkinToneTagCmbBx.Items.Add("Select");
                for (int i = 0; i < linwin.getNoOfComplexions(); i++)
                    SkinToneTagCmbBx.Items.Add(linwin.getComplexionTag(i));
                SkinToneTagCmbBx.Items.Add("Add tag...");
                SkinToneTagCmbBx.Enabled = true;
                SkinToneTagCmbBx.Visible = true;
                SkinToneValidDateComboBox.Enabled = false;
                SkinToneValidDateComboBox.Visible = false;
                SkinToneDateTxtBx.Enabled = false;
                SkinToneDateTxtBx.Visible = false;
                SkinToneDateTimePicker.Enabled = true;
                SkinToneDateTimePicker.Visible = true;
                AddSkinToneButton.Text = "Add Skintone";
                AddSkinToneButton.Enabled = true;
            }
            else if (iSelIdx == actorClass.getNumberOfSkinTones() + 2)
            {
                // Add Tag...
                SkinToneValidDateComboBox.Enabled = false;
                SkinToneValidDateComboBox.Visible = false;
                SkinToneDateTimePicker.Visible = false;
                SkinToneDateTimePicker.Enabled = false;
                SkinToneDateTxtBx.Enabled = false;
                SkinToneDateTxtBx.Visible = false;
                SkinToneTagTextBox.Size = new Size(((ActorData.Size.Width - AddSkinToneButton.Size.Width) - 30), 20);
                SkinToneTagTextBox.Text = "<tag>[[;|,] <Description>[[;|,] <Security>]]";
                SkinToneTagTextBox.Enabled = true;
                SkinToneTagTextBox.Visible = true;
                AddSkinToneButton.Text = "Add Tag";
                AddSkinToneButton.Enabled = true;
                AddSkinToneButton.Visible = true;
                noOfChangedGUIs = 0;
            }
            else if (iSelIdx >= 0)
            {
                // Selected value
                //SkinToneTagTextBox.Size = new Size(((ActorData.Size.Width - AddSkinToneButton.Size.Width - SkinToneDateTxtBx.Size.Width) - 10), 20);
                string sSkTne = "Skintone: " + actorClass.getUserSkinToneTag(iSelIdx - 1);
                SkinToneTagTextBox.Text = sSkTne;
                SkinToneTagTextBox.Enabled = false;
                SkinToneTagTextBox.Visible = true;
                AddSkinToneButton.Text = "View Skintone";
                AddSkinToneButton.Enabled = true;
                AddSkinToneButton.Visible = true;
            }
        }
        private void SkinToneTagCmbBx_SelectedIndexChanged(object sender, EventArgs e)
        {
            // TODO - Add handling when "SkinToneTagCmbBx" is set.
            // "Select..."|<complexion tag>|"Add tag..."
            int iSelTag = SkinToneTagCmbBx.SelectedIndex;
            if ((iSelTag - 1) >= linwin.getNoOfComplexions())
            {
                // "Add tag..." selected.
                SkinToneTagTextBox.Size = new Size(((ActorData.Size.Width - AddSkinToneButton.Size.Width) - 30), 20);
                SkinToneTagTextBox.Text = "<tag>[[;|,] <Description>[[;|,] <Security>]]";
                SkinToneTagTextBox.Enabled = true;
                SkinToneTagTextBox.Visible = true;
                AddSkinToneButton.Text = "Add Tag";
                AddSkinToneButton.Enabled = false;
                AddSkinToneButton.Visible = true;
                noOfChangedGUIs = 0;
            }
            else if ((iSelTag - 1) > -1)
            {
                // A complexion tag selected.
                // SkinToneTagTextBox.Size = new Size(((ActorData.Size.Width - AddSkinToneButton.Size.Width - SkinToneDateTimePicker.Size.Width) - 30), 20);
                string sSetComplexion = linwin.getComplexionTag(iSelTag - 1);
                string sSetDate = SkinToneDateTimePicker.Value.ToString();
                actorClass.setUserSkinTone(actorClass.getNumberOfSkinTones(), sSetComplexion, 0, 0, 0, sSetDate);
            }
        }
        #endregion
        #region eyeInfo
        private void EyeColorDateTxtBox_TextChanged(object sender, EventArgs e)
        {
            setInformationText("Unexpected entry into eye color date text box change!", informationType.INFO, sender, e);
        }
        private void AddEyeColorButton_Click(object sender, EventArgs e)
        {
            // "Add Tag"|"Add Item"|"View Eyecolor"
            string sSetButtonText = SelEyeColorTextBox.Text;
            if (sSetButtonText == "View Eyecolor")
            {
                // TODO - Show the eyecolor.
            }
            else if (sSetButtonText == "Add Tag")
            {
                // expected format: <tag>[[;|,] <Description>[[;|,] <Safety Class>]]
                char chDelim = ';';
                string sWorkTag = "";
                string sWorkDescr = "";
                string sWorkSafeClass = "";
                string sWorkString = SelEyeColorTextBox.Text;
                int dp = sWorkString.IndexOf(chDelim);
                if ((dp < 0) || (dp > sWorkString.Length))
                {
                    chDelim = ',';
                    dp = sWorkString.IndexOf(chDelim);
                }
                if ((dp > 0) && (dp < sWorkString.Length))
                {
                    // <tag>[[;|,] <Description>[[;|,] <Safety Class>]]
                    sWorkTag = sWorkString.Substring(0, dp);
                    sWorkString = sWorkString.Substring(dp + 2);
                    dp = sWorkString.IndexOf(chDelim);
                    if ((dp > 0) && (dp < sWorkString.Length))
                    {
                        // <Description>[;|,] <Safety Class>
                        sWorkDescr = sWorkString.Substring(0, dp);
                        sWorkSafeClass = sWorkString.Substring(dp + 2);
                    }
                    else
                    {
                        sWorkDescr = sWorkString;
                        sWorkSafeClass = "Undefined";
                    }
                }
                else
                {
                    sWorkTag = sWorkString;
                    sWorkDescr = "";
                    sWorkSafeClass = "Undefined";
                }
                linwin.addEyeColorCategory(sWorkTag, sWorkDescr, sWorkSafeClass);
                SelEyeColorTextBox.Size = new Size(((ActorData.Size.Width - AddEyeColorButton.Size.Width - EyeColorValidDateComboBox.Size.Width) - 10), 20);
                toSave.actorChanges = true;
                toSave.userChanges = true;
                saveToolStripMenuItem.Enabled = true;
                saveAsToolStripMenuItem.Enabled = true;
            }
            else if (sSetButtonText == "Add Item")
            {
                // Expected format: <color tag>[[;|,] <form tag>[[;|,] <Glasses>]]
                string sWorkString = SelEyeColorTextBox.Text;
                char chDelim = ';';
                string sWorkColor = "";
                string sWorkForm = "";
                string sWorkGlasses = "";
                string sWorkDate = "";
                bool bGlasses = false;
                int dp = sWorkString.IndexOf(chDelim);
                if ((dp < 0) || (dp > sWorkString.Length))
                {
                    chDelim = ',';
                    dp = sWorkString.IndexOf(chDelim);
                }
                if ((dp > 0) && (dp < sWorkString.Length))
                {
                    // <color tag>[[;|,] <form tag>[[;|,] <Glasses>]]
                    sWorkColor = sWorkString.Substring(0, dp);
                    sWorkString = sWorkString.Substring(dp + 2);
                    dp = sWorkString.IndexOf(chDelim);
                    if ((dp > 0) && (dp < sWorkString.Length))
                    {
                        // <form tag>[[;|,] <Glasses>]]
                        sWorkForm = sWorkString.Substring(0, dp);
                        sWorkGlasses = sWorkString.Substring(dp + 2);
                        if (sWorkGlasses.ToLower() != "no")
                            bGlasses = true;
                    }
                    else
                    {
                        // <form tag>
                        sWorkForm = sWorkString;
                        bGlasses = false;
                    }
                }
                else
                {
                    sWorkColor = sWorkString;
                    sWorkForm = "Round";
                    bGlasses = false;
                }
                sWorkDate = EyeColorDteTmePckr.Value.ToString();
                actorClass.setUserEyeData(actorClass.getNumberOfEyeData(), sWorkColor, sWorkForm, sWorkDate, bGlasses);
                toSave.actorChanges = true;
                saveToolStripMenuItem.Enabled = true;
                saveAsToolStripMenuItem.Enabled = true;
            }
        }
        private void SelEyeColorTextBox_TextChanged(object sender, EventArgs e)
        {
            if ((AddEyeColorButton.Text == "Add Tag") && (noOfChangedGUIs > 0))
            {
                AddEyeColorButton.Enabled = true;
                noOfChangedGUIs = 0;
            }
            else
            {
                AddEyeColorButton.Enabled = false;
                noOfChangedGUIs++;
            }
        }
        private void EyeColorValidDateComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            int iSelIdx = EyeColorValidDateComboBox.SelectedIndex;
            if (iSelIdx == actorClass.getNumberOfEyeData() + 1)
            {
                // "Add Item..." selected;
                //  - Button = "Add Item"
                //  - eyeColorTagsCmbBx = "Select...", <tags>, "Add tag..."
                //  - EyeColorDteTmePckr presented.
                SelEyeColorTextBox.Text = "";
                SelEyeColorTextBox.Enabled = false;
                SelEyeColorTextBox.Visible = false;
                eyeColorTagsCmbBx.Items.Clear();
                eyeColorTagsCmbBx.Items.Add("Select");
                for (int i = 0; i < linwin.getNoOfEyeColors(); i++)
                    eyeColorTagsCmbBx.Items.Add(linwin.getEyeColorTag(i));
                eyeColorTagsCmbBx.Items.Add("Add tag...");
                eyeColorTagsCmbBx.Enabled = true;
                eyeColorTagsCmbBx.Visible = true;
                EyeColorValidDateComboBox.Enabled = false;
                EyeColorValidDateComboBox.Visible = false;
                EyeColorDateTxtBox.Enabled = false;
                EyeColorDateTxtBox.Visible = false;
                EyeColorDteTmePckr.Enabled = true;
                EyeColorDteTmePckr.Visible = true;
                AddEyeColorButton.Text = "Add Item";
                AddEyeColorButton.Enabled = false;
                AddEyeColorButton.Visible = true;

            }
            else if (iSelIdx == actorClass.getNumberOfEyeData() + 2)
            {
                EyeColorValidDateComboBox.Enabled = false;
                EyeColorValidDateComboBox.Visible = false;
                EyeColorDteTmePckr.Enabled = false;
                EyeColorDteTmePckr.Visible = false;
                EyeColorDateTxtBox.Enabled = false;
                EyeColorDateTxtBox.Visible = false;
                SelEyeColorTextBox.Size = new Size(((ActorData.Size.Width - AddEyeColorButton.Size.Width) - 30), 20);
                SelEyeColorTextBox.Text = "<tag>[[;|,] <Description>[[;|,] <Securty>]]";
                SelEyeColorTextBox.Enabled = true;
                SelEyeColorTextBox.Visible = true;
                AddEyeColorButton.Text = "Add Tag";
                AddEyeColorButton.Enabled = true;
                AddEyeColorButton.Visible = true;
            }
            else if (iSelIdx >= 0)
            {
                // Selected a value
                EyeColorDteTmePckr.Enabled = false;
                EyeColorDteTmePckr.Visible = false;
                EyeColorDateTxtBox.Enabled = false;
                EyeColorDateTxtBox.Visible = false;
                EyeColorLabel.Visible = false;
                string sEyeClr = "Eye color: " + actorClass.getUserEyeColorTag(iSelIdx - 1);
                SelEyeColorTextBox.Text = sEyeClr;
                SelEyeColorTextBox.Enabled = false;
                SelEyeColorTextBox.Visible = true;
                EyeColorValidDateComboBox.Enabled = true;
                EyeColorValidDateComboBox.Visible = true;
                AddEyeColorButton.Text = "View Eyecolor";
                AddEyeColorButton.Enabled = true;
                AddEyeColorButton.Visible = true;
            }
        }
        private void EyeColorDteTmePckr_ValueChanged(object sender, EventArgs e)
        {
            if ((AddEyeColorButton.Text == "Add Item") && (noOfChangedGUIs > 0))
            {
                DateTime sSdt = EyeColorDteTmePckr.Value;
                SelEyeColorTextBox.Enabled = true;
                EyeColorDateTxtBox.Text = sSdt.ToString();
                EyeColorDteTmePckr.Visible = false;
                EyeColorDateTxtBox.Visible = true;
                EyeColorDateTxtBox.Enabled = true;
                noOfChangedGUIs = 0;
            }
        }
        private void eyeColorTagsCmbBx_SelectedIndexChanged(object sender, EventArgs e)
        {
            // TODO - Add handling when "eyeColorTagsCmbBx" is set.
            // "Select..."|<complexion tag>|"Add tag..."
            int iSelTag = eyeColorTagsCmbBx.SelectedIndex;
            if ((iSelTag - 1) >= linwin.getNoOfEyeColors())
            {
                // "Add tag..." selected.
                SelEyeColorTextBox.Size = new Size(((ActorData.Size.Width - AddEyeColorButton.Size.Width) - 30), 20);
                SelEyeColorTextBox.Text = "<tag>[[;|,] <Description>[[;|,] <Security>]]";
                SelEyeColorTextBox.Enabled = true;
                SelEyeColorTextBox.Visible = true;
                AddEyeColorButton.Text = "Add Tag";
                AddEyeColorButton.Enabled = false;
                AddEyeColorButton.Visible = true;
                noOfChangedGUIs = 0;
            }
            else if ((iSelTag - 1) > -1)
            {
                string sSetEyeColorTag = linwin.getEyeColorTag(iSelTag - 1);
                string sSetDate = EyeColorDteTmePckr.Value.ToString();
                actorClass.addUserEyeData(sSetEyeColorTag, "Round", sSetDate, false);
            }
        }
        #endregion
        #region genderInfo
        private void GenderInfoValidDateComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            int iSelDta = GenderInfoValidDateComboBox.SelectedIndex;
            if ((iSelDta - 1) >= actorClass.getNumberOfGenderData())
            {
                // --- "Add item..." selected ---
                GenderTypeTextBox.Enabled = false;
                GenderTypeTextBox.Visible = false;
                GenderTypeCmbBx.Items.Clear();
                GenderTypeCmbBx.Items.Add("Select...");
                GenderTypeCmbBx.Items.Add("Male");
                GenderTypeCmbBx.Items.Add("Female");
                //genderTypeSet = false;
                GenderTypeCmbBx.SelectedIndex = 0;
                GenderTypeCmbBx.Enabled = true;
                GenderTypeCmbBx.Visible = true;
                GenderDateTimePicker.Enabled = true;
                GenderDateTimePicker.Visible = true;
                GdrLengthTextBox.Text = "NN.nn UU";
                GdrLengthTextBox.Enabled = true;
                GdrLengthTextBox.Visible = true;
                GdrCircumfTextBox.Text = "NN.nn UU";
                GdrCircumfTextBox.Enabled = true;
                GdrCircumfTextBox.Visible = true;
                GdrLookTypeTextBox.Enabled = false;
                GdrLookTypeTextBox.Visible = false;
                GdrLookCmbBx.Items.Clear();
                GdrLookCmbBx.Items.Add("Select...");
                GdrLookCmbBx.Items.Add("Barbie");
                GdrLookCmbBx.Items.Add("Curtains");
                GdrLookCmbBx.Items.Add("Horseshoe");
                GdrLookCmbBx.Items.Add("Puffy");
                GdrLookCmbBx.Items.Add("Tulip");
                GdrLookCmbBx.Items.Add("I");
                GdrLookCmbBx.Items.Add("II");
                GdrLookCmbBx.Items.Add("III");
                GdrLookCmbBx.Items.Add("IV");
                GdrLookCmbBx.Items.Add("V");
                //genderLookSet = false;
                GdrLookCmbBx.SelectedIndex = 0;
                GdrLookCmbBx.Enabled = true;
                GdrLookCmbBx.Visible = true;
                GdrBehaveTypeTextBox.Enabled = false;
                GdrBehaveTypeTextBox.Visible = false;
                GenderBehaveCmbBx.Items.Clear();
                GenderBehaveCmbBx.Items.Add("Select...");
                GenderBehaveCmbBx.Items.Add("Sloppy");
                GenderBehaveCmbBx.Items.Add("Stretchy");
                GenderBehaveCmbBx.Items.Add("Tight");
                GenderBehaveCmbBx.Items.Add("Stiffer");
                GenderBehaveCmbBx.Items.Add("Grower");
                GenderBehaveCmbBx.SelectedIndex = 0;
                GenderBehaveCmbBx.Enabled = true;
                GenderBehaveCmbBx.Visible = true;
                GdrPresentTextBox.Text = "<Presentation>";
                GdrPresentTextBox.Enabled = true;
                GdrPresentTextBox.Visible = true;
                AddGenderInfoButton.Enabled = false;
                AddGenderInfoButton.Visible = false;
                noOfChangedGUIs = 0;
            }
            else if ((iSelDta - 1) > -1)
            {
                GdrLookCmbBx.Enabled = false;
                GdrLookCmbBx.Visible = false;
                GenderTypeCmbBx.Enabled = false;
                GenderTypeCmbBx.Visible = false;
                GenderDateTimePicker.Enabled = false;
                GenderDateTimePicker.Visible = false;
                GenderBehaveCmbBx.Enabled = false;
                GenderBehaveCmbBx.Visible = false;
                GenderTypeTextBox.Text = actorClass.getUserGenderType(iSelDta);
                GenderTypeTextBox.Enabled = false;
                GenderTypeTextBox.Visible = true;
                GdrLengthTextBox.Text = actorClass.getUserGenderLength(iSelDta);
                GdrLengthTextBox.Enabled = false;
                GdrLengthTextBox.Visible = true;
                GdrCircumfTextBox.Text = actorClass.getUserGenderCircumf(iSelDta);
                GdrCircumfTextBox.Enabled = false;
                GdrCircumfTextBox.Visible = true;
                GdrLookTypeTextBox.Text = actorClass.getUserGenderAppearance(iSelDta);
                GdrLookTypeTextBox.Enabled = false;
                GdrLookTypeTextBox.Visible = true;
                GdrBehaveTypeTextBox.Text = actorClass.getUserGenderBehaviour(iSelDta);
                GdrBehaveTypeTextBox.Enabled = false;
                GdrBehaveTypeTextBox.Visible = true;
                GdrPresentTextBox.Text = actorClass.getUserGenderPres(iSelDta);
                GdrPresentTextBox.Enabled = false;
                GdrPresentTextBox.Visible = true;
                AddGenderInfoButton.Text = "";
                AddGenderInfoButton.Enabled = false;
                AddGenderInfoButton.Visible = false;
            }
        }
        private void AddGenderInfoButton_Click(object sender, EventArgs e)
        {
            string sAddBtnTxt = AddGenderInfoButton.Text;
            if (sAddBtnTxt == "Add Info")
            {
                string sSetGenderType = "Undefined";
                if (GenderTypeCmbBx.SelectedIndex == 1)
                    sSetGenderType = "Male";
                else if (GenderTypeCmbBx.SelectedIndex == 2)
                    sSetGenderType = "Female";
                string sSetGenderDate = GenderDateTimePicker.Value.ToString();
                string sSetGenderLength = GdrLengthTextBox.Text;
                float fLength = 0;
                int dp = sSetGenderLength.IndexOf(' ');
                if ((dp > 0) && (dp < sSetGenderLength.Length))
                {
                    if (float.TryParse(sSetGenderLength.Substring(0, dp), out fLength))
                        sSetGenderLength = sSetGenderLength.Substring(dp + 1);
                    else
                        sSetGenderLength = "";
                }
                string sSetGenderCirc = GdrCircumfTextBox.Text;
                float fCirc = 0;
                dp = sSetGenderCirc.IndexOf(' ');
                if ((dp > 0) && (dp < sSetGenderCirc.Length))
                {
                    if (float.TryParse(sSetGenderCirc.Substring(0, dp), out fCirc))
                        sSetGenderCirc = sSetGenderCirc.Substring(dp + 1);
                    else
                        sSetGenderCirc = "";
                }
                string sUnit = "Undefined";
                if ((sSetGenderCirc != "") && (sSetGenderLength != "") && (sSetGenderCirc == sSetGenderLength))
                    sUnit = sSetGenderLength;
                else
                {
                    fCirc = 0;
                    fLength = 0;
                }
                string sGenderLook = "Undefined";
                if (GdrLookCmbBx.SelectedIndex > 0)
                {
                    int iSelValue = GdrLookCmbBx.SelectedIndex;
                    if (sSetGenderType.ToLower() == "male")
                    {
                        if (iSelValue == 1)
                            sGenderLook = "I";
                        else if (iSelValue == 2)
                            sGenderLook = "II";
                        else if (iSelValue == 3)
                            sGenderLook = "III";
                        else if (iSelValue == 4)
                            sGenderLook = "IV";
                        else if (iSelValue == 5)
                            sGenderLook = "V";
                    }
                    else if (sSetGenderType.ToLower() == "female")
                    {
                        if (iSelValue == 1)
                            sGenderLook = "Barbie";
                        else if (iSelValue == 2)
                            sGenderLook = "Curtains";
                        else if (iSelValue == 3)
                            sGenderLook = "Horseshoe";
                        else if (iSelValue == 4)
                            sGenderLook = "Puffy";
                        else if (iSelValue == 5)
                            sGenderLook = "Tulip";
                    }
                    else
                    {
                        if (iSelValue == 1)
                            sGenderLook = "Barbie";
                        else if (iSelValue == 2)
                            sGenderLook = "Curtains";
                        else if (iSelValue == 3)
                            sGenderLook = "Horseshoe";
                        else if (iSelValue == 4)
                            sGenderLook = "Puffy";
                        else if (iSelValue == 5)
                            sGenderLook = "Tulip";
                        else if (iSelValue == 6)
                            sGenderLook = "I";
                        else if (iSelValue == 7)
                            sGenderLook = "II";
                        else if (iSelValue == 8)
                            sGenderLook = "III";
                        else if (iSelValue == 9)
                            sGenderLook = "IV";
                        else if (iSelValue == 10)
                            sGenderLook = "V";
                    }
                }
                //                        sGenderLook = GdrLookCmbBx.SelectedText.ToString();
                string sGenderBehaviour = "Undefined";
                if (GenderBehaveCmbBx.SelectedIndex > 0)
                {
                    int iSelValue = GenderBehaveCmbBx.SelectedIndex;
                    if (iSelValue == 1)
                        sGenderBehaviour = "Sloppy";
                    else if (iSelValue == 2)
                        sGenderBehaviour = "Stretchy";
                    else if (iSelValue == 3)
                        sGenderBehaviour = "Tight";
                    else if (iSelValue == 4)
                        sGenderBehaviour = "Stiffer";
                    else if (iSelValue == 5)
                        sGenderBehaviour = "Grower";
                }
                //                        sGenderBehaviour = GenderBehaveCmbBx.SelectedText.ToString();
                string sGdrPrs = GdrPresentTextBox.Text;
                actorClass.addUserGenderData(sSetGenderType, fLength, fCirc, sUnit, sGenderLook, sGenderBehaviour, sGdrPrs, sSetGenderDate);
                AddGenderInfoButton.Text = "";
                AddGenderInfoButton.Enabled = false;
                AddGenderInfoButton.Visible = false;
                noOfChangedGUIs = 0;
                SaveActorDataChangesButton.Text = "Save actordata";
                SaveActorDataChangesButton.Enabled = true;
                DiscardActorDataChangesButton.Enabled = true;
                toSave.actorChanges = true;
                saveToolStripMenuItem.Enabled = true;
                saveAsToolStripMenuItem.Enabled = true;
                //Continue here
            }
            else
                setInformationText("Not ready to save data yet.", informationType.ERROR, sender, e);
        }
        private void GenderTypeTextBox_TextChanged(object sender, EventArgs e)
        {
            setInformationText("Unexpected gender type text entry!", informationType.ERROR, sender, e);
        }
        private void GenderTypeCmbBx_SelectedIndexChanged(object sender, EventArgs e)
        {
            int iGenTypeSel = GenderTypeCmbBx.SelectedIndex;
            //                if (!genderLookSet)
            //                {
            if (iGenTypeSel == 1)
            {
                // Male set
                GdrLookCmbBx.Items.Clear();
                GdrLookCmbBx.Items.Add("Select...");
                GdrLookCmbBx.Items.Add("I");
                GdrLookCmbBx.Items.Add("II");
                GdrLookCmbBx.Items.Add("III");
                GdrLookCmbBx.Items.Add("IV");
                GdrLookCmbBx.Items.Add("V");
            }
            else if (iGenTypeSel == 2)
            {
                // Female set
                GdrLookCmbBx.Items.Clear();
                GdrLookCmbBx.Items.Add("Select...");
                GdrLookCmbBx.Items.Add("Barbie");
                GdrLookCmbBx.Items.Add("Curtains");
                GdrLookCmbBx.Items.Add("Horseshoe");
                GdrLookCmbBx.Items.Add("Puffy");
                GdrLookCmbBx.Items.Add("Tulip");
            }
            noOfChangedGUIs++;
            if (noOfChangedGUIs > 2)
            {
                AddGenderInfoButton.Text = "Add Info";
                AddGenderInfoButton.Enabled = true;
                AddGenderInfoButton.Visible = true;
            }
            //genderTypeSet = true;
            //                }
        }
        private void GdrLengthTextBox_TextChanged(object sender, EventArgs e)
        {
            if (noOfChangedGUIs > 2)
            {
                AddGenderInfoButton.Text = "Add Info";
                AddGenderInfoButton.Enabled = true;
                AddGenderInfoButton.Visible = true;
            }
            else
            {
                noOfChangedGUIs++;
                AddGenderInfoButton.Enabled = false;
                AddGenderInfoButton.Visible = true;
            }
        }
        private void GdrCircumfTextBox_TextChanged(object sender, EventArgs e)
        {
            if (noOfChangedGUIs > 2)
            {
                AddGenderInfoButton.Text = "Add Info";
                AddGenderInfoButton.Enabled = true;
                AddGenderInfoButton.Visible = true;
            }
            else
            {
                noOfChangedGUIs++;
                AddGenderInfoButton.Enabled = false;
                AddGenderInfoButton.Visible = true;
            }
        }
        private void GdrLookTypeTextBox_TextChanged(object sender, EventArgs e)
        {
            setInformationText("Unexpected gender look text entry!", informationType.ERROR, sender, e);
        }
        private void GdrLookCmbBx_SelectedIndexChanged(object sender, EventArgs e)
        {
            // "Select..."|"Barbie"|"Curtains"|"Horseshoe"|"Puffy"|"Tulip"|"I"|"II"|"III"|"IV"|"V"
            string sSelGdrLook = GdrLookCmbBx.SelectedText;
            if ((sSelGdrLook == "Barbie") || (sSelGdrLook == "Curtains") || (sSelGdrLook == "Horseshoe") || (sSelGdrLook == "Puffy") || (sSelGdrLook == "Tulip"))
            {
                GdrLookCmbBx.BackColor = Color.FromArgb(255, 232, 232);
                //if (!genderTypeSet)
                //{
                    GenderTypeCmbBx.SelectedIndex = 2;
                    //genderTypeSet = true;
                    noOfChangedGUIs++;
                //}
            }
            else if ((sSelGdrLook == "I") || (sSelGdrLook == "II") || (sSelGdrLook == "III") || (sSelGdrLook == "IV") || (sSelGdrLook == "V"))
            {
                GdrLookCmbBx.BackColor = Color.FromArgb(128, 64, 64);
                //if (!genderTypeSet)
                //{
                    GenderTypeCmbBx.SelectedIndex = 1;
                    //genderTypeSet = true;
                    noOfChangedGUIs++;
                //}
            }
            else
                GdrLookCmbBx.BackColor = Color.FromArgb(255, 255, 255);
            noOfChangedGUIs++;
            if (noOfChangedGUIs > 2)
            {
                AddGenderInfoButton.Text = "Add Info";
                AddGenderInfoButton.Enabled = true;
                AddGenderInfoButton.Visible = true;
            }
            //genderLookSet = true;
        }
        private void GdrBehaveTypeTextBox_TextChanged(object sender, EventArgs e)
        {
            setInformationText("Unexpected gender behaviour text entry!", informationType.ERROR, sender, e);
        }
        private void GenderBehaveCmbBx_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (noOfChangedGUIs > 2)
            {
                AddGenderInfoButton.Text = "Add Info";
                AddGenderInfoButton.Enabled = true;
                AddGenderInfoButton.Visible = true;
            }
            else
            {
                noOfChangedGUIs++;
                AddGenderInfoButton.Enabled = false;
                AddGenderInfoButton.Visible = true;
            }
        }
        private void GdrPresentTextBox_TextChanged(object sender, EventArgs e)
        {
            if (noOfChangedGUIs > 2)
            {
                AddGenderInfoButton.Text = "Add Info";
                AddGenderInfoButton.Enabled = true;
                AddGenderInfoButton.Visible = true;
            }
            else
            {
                noOfChangedGUIs++;
                AddGenderInfoButton.Enabled = false;
                AddGenderInfoButton.Visible = true;
            }
        }
        private void GenderDateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            if (noOfChangedGUIs > 2)
            {
                AddGenderInfoButton.Text = "Add Info";
                AddGenderInfoButton.Enabled = true;
                AddGenderInfoButton.Visible = true;
            }
            else
            {
                noOfChangedGUIs++;
                AddGenderInfoButton.Enabled = false;
                AddGenderInfoButton.Visible = true;
            }
        }
        #endregion
        #region lengthInfo
        private void AddLengthInfoButton_Click(object sender, EventArgs e)
        {
            string setLengthInfo = LengthTextBox.Text;
            string setValueString = "";
            string setUnitString = "";
            string setDateString = LengthDateTimePicker.Value.ToString();
            int dp = setLengthInfo.IndexOf(" ");
            if ((dp > 0) && (dp < setLengthInfo.Length))
            {
                setValueString = setLengthInfo.Substring(0, dp);
                setUnitString = setLengthInfo.Substring(dp + 1);
                float fValue = 0;
                if (float.TryParse(setValueString, out fValue))
                {
                    actorClass.addUserLength(fValue, setUnitString, setDateString);
                    toSave.actorChanges = true;
                    saveToolStripMenuItem.Enabled = true;
                    saveAsToolStripMenuItem.Enabled = true;
                    // --- reset GUI ---
                    LengthLabel.Visible = true;
                    AddLengthInfoButton.Text = "";
                    AddLengthInfoButton.Enabled = false;
                    AddLengthInfoButton.Visible = false;
                    LengthTextBox.Text = "";
                    LengthDateTimePicker.Enabled = false;
                    LengthDateTimePicker.Visible = false;
                    LengthValidDateComboBox.Items.Clear();
                    LengthValidDateComboBox.Items.Add("Select...");
                    for (int i = 0; i < actorClass.getNumberOfLengthData(); i++)
                        LengthValidDateComboBox.Items.Add(actorClass.getUserLengthInfoValidDate(i));
                    LengthValidDateComboBox.Items.Add("Add item...");
                    LengthValidDateComboBox.Enabled = true;
                    LengthValidDateComboBox.Visible = true;
                    toSave.actorChanges = true;
                    saveToolStripMenuItem.Enabled = true;
                    saveAsToolStripMenuItem.Enabled = true;
                }
                else
                    setInformationText("Failed saving length data!", informationType.ERROR, sender, e);
            }
        }
        private void LengthTextBox_TextChanged(object sender, EventArgs e)
        {
            AddLengthInfoButton.Enabled = true;
        }
        private void LengthDateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            AddLengthInfoButton.Enabled = true;
        }
        private void LengthValidDateComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            int iSelItm = LengthValidDateComboBox.SelectedIndex;
            if ((iSelItm - 1) >= actorClass.getNumberOfLengthData())
            {
                // "Add item..." selected
                LengthLabel.Visible = false;
                LengthValidDateComboBox.Items.Clear();
                LengthValidDateComboBox.Enabled = false;
                LengthValidDateComboBox.Visible = false;
                LengthDateTimePicker.Enabled = true;
                LengthDateTimePicker.Visible = true;
                LengthTextBox.Text = "NN.nn UU";
                LengthTextBox.Enabled = true;
                LengthTextBox.Visible = true;
                AddLengthInfoButton.Text = "Add item";
                AddLengthInfoButton.Enabled = false;
                AddLengthInfoButton.Visible = true;
            }
            else if ((iSelItm - 1) >= 0)
            {
                // <length> data selected.
                LengthDateTimePicker.Enabled = false;
                LengthDateTimePicker.Visible = false;
                AddLengthInfoButton.Text = "";
                AddLengthInfoButton.Enabled = false;
                AddLengthInfoButton.Visible = false;
                LengthLabel.Visible = true;
                LengthTextBox.Text = actorClass.getUserLengthValue(iSelItm - 1);
                LengthTextBox.Enabled = false;
                LengthTextBox.Visible = true;
            }
        }
        #endregion
        #region weightData
        private void AddWeightInfoButton_Click(object sender, EventArgs e)
        {
            string setWeightInfo = WeightTextBox.Text;
            string setValueString = "";
            string setUnitString = "";
            string setDateString = WeightDateTimePicker.Value.ToString();
            int dp = setWeightInfo.IndexOf(" ");
            if ((dp > 0) && (dp < setWeightInfo.Length))
            {
                setValueString = setWeightInfo.Substring(0, dp);
                setUnitString = setWeightInfo.Substring(dp + 1);
                float fValue = 0;
                if (float.TryParse(setValueString, out fValue))
                {
                    actorClass.addUserWeightData(fValue, setUnitString, setDateString);
                    toSave.actorChanges = true;
                    saveToolStripMenuItem.Enabled = true;
                    saveAsToolStripMenuItem.Enabled = true;
                    // --- reset GUI ---
                    WeightLabel.Visible = true;
                    AddWeightInfoButton.Text = "";
                    AddWeightInfoButton.Enabled = false;
                    AddWeightInfoButton.Visible = false;
                    WeightTextBox.Text = "";
                    WeightDateTimePicker.Enabled = false;
                    WeightDateTimePicker.Visible = false;
                    WeightValidDateComboBox.Items.Clear();
                    WeightValidDateComboBox.Items.Add("Select...");
                    for (int i = 0; i < actorClass.getNumberOfWeightData(); i++)
                        WeightValidDateComboBox.Items.Add(actorClass.getUserWeightInfoValidDate(i));
                    WeightValidDateComboBox.Items.Add("Add item...");
                    WeightValidDateComboBox.Enabled = true;
                    WeightValidDateComboBox.Visible = true;
                    toSave.actorChanges = true;
                    saveToolStripMenuItem.Enabled = true;
                    saveAsToolStripMenuItem.Enabled = true;
                }
                else
                    setInformationText("Failed saving weight data!", informationType.ERROR, sender, e);
            }
        }
        private void WeightTextBox_TextChanged(object sender, EventArgs e)
        {
            AddWeightInfoButton.Enabled = true;
        }
        private void WeightDateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            AddWeightInfoButton.Enabled = true;
        }
        private void WeightValidDateComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            int iSelItm = WeightValidDateComboBox.SelectedIndex;
            if ((iSelItm - 1) >= actorClass.getNumberOfWeightData())
            {
                // "Add item..." selected
                WeightLabel.Visible = false;
                WeightValidDateComboBox.Items.Clear();
                WeightValidDateComboBox.Enabled = false;
                WeightValidDateComboBox.Visible = false;
                WeightDateTimePicker.Enabled = true;
                WeightDateTimePicker.Visible = true;
                WeightTextBox.Text = "NN.nn UU";
                WeightTextBox.Enabled = true;
                WeightTextBox.Visible = true;
                AddWeightInfoButton.Text = "Add item";
                AddWeightInfoButton.Enabled = false;
                AddWeightInfoButton.Visible = true;
            }
            else if ((iSelItm - 1) >= 0)
            {
                // Actual weight data selected.
                WeightDateTimePicker.Enabled = false;
                WeightDateTimePicker.Visible = false;
                AddWeightInfoButton.Text = "";
                AddWeightInfoButton.Enabled = false;
                AddWeightInfoButton.Visible = false;
                WeightLabel.Visible = true;
                WeightTextBox.Text = actorClass.getUserWeightValue(iSelItm - 1);
                WeightTextBox.Enabled = false;
                WeightTextBox.Visible = true;
            }
        }
        #endregion
        #region ChestData
        private void AddChestInfoButton_Click(object sender, EventArgs e)
        {
            string setChestType = "Undefined";
            int iSetChestType = ChestTypeCmbBx.SelectedIndex;
            if ((iSetChestType - 1) >= 0)
                setChestType = actorClass.getBreastTypeString(iSetChestType - 1);
            string setChestSizeType = "Undefined";
            int iSetChestSizeType = ChestSizeTypeCmbBx.SelectedIndex;
            if ((iSetChestSizeType - 1) >= 0)
                setChestSizeType = actorClass.getBreastSizeString(iSetChestSizeType - 1);
            string workCirc = ChestCircTextBox.Text;
            int dp = workCirc.IndexOf(" ");
            string circUnit = "";
            float circValue = 0;
            if ((dp > 0) && (dp < workCirc.Length))
            {
                circUnit = workCirc.Substring(dp + 1);
                if (!(float.TryParse(workCirc.Substring(0, dp), out circValue)))
                    circUnit = "";
            }
            string sChestDate = ChestDateTimePicker.Value.ToString();
            actorClass.addUserChestData(setChestType, circValue, circUnit, setChestSizeType, sChestDate);
            // --- Reset GUI ---
            AddChestInfoButton.Text = "";
            AddChestInfoButton.Enabled = false;
            AddChestInfoButton.Visible = false;
            ChestDataLabel.Visible = true;
            ChestTypeCmbBx.Enabled = false;
            ChestTypeCmbBx.Visible = false;
            ChestTypeTextBox.Text = "";
            ChestTypeTextBox.Enabled = false;
            ChestTypeTextBox.Visible = true;
            ChestDateTimePicker.Enabled = false;
            ChestDateTimePicker.Visible = false;
            ChestValidDateComboBox.Items.Clear();
            ChestValidDateComboBox.Items.Add("Select...");
            if (actorClass.getNumberOfChestData() > 0)
            {
                for (int i = 0; i < actorClass.getNumberOfChestData(); i++)
                    ChestValidDateComboBox.Items.Add(actorClass.getUserChestInfoValidDate(i));
            }
            ChestValidDateComboBox.Items.Add("Add item...");
            ChestValidDateComboBox.SelectedIndex = 0;
            ChestValidDateComboBox.Enabled = true;
            ChestValidDateComboBox.Visible = true;
            ChestCircTextBox.Text = "";
            ChestCircTextBox.Enabled = false;
            ChestCircTextBox.Visible = true;
            ChestSizeTypeCmbBx.Enabled = false;
            ChestSizeTypeCmbBx.Visible = false;
            ChestSizeTextBox.Text = "";
            ChestSizeTextBox.Enabled = false;
            ChestSizeTextBox.Visible = true;
            toSave.actorChanges = true;
            saveToolStripMenuItem.Enabled = true;
            saveAsToolStripMenuItem.Enabled = true;
        }
        private void ChestTypeTextBox_TextChanged(object sender, EventArgs e)
        {
            AddChestInfoButton.Visible = true;
        }
        private void ChestTypeCmbBx_SelectedIndexChanged(object sender, EventArgs e)
        {
            AddChestInfoButton.Enabled = true;
        }
        private void ChestCircTextBox_TextChanged(object sender, EventArgs e)
        {
            AddChestInfoButton.Enabled = true;
        }
        private void ChestSizeTextBox_TextChanged(object sender, EventArgs e)
        {
            AddChestInfoButton.Enabled = true;
        }
        private void ChestSizeTypeCmbBx_SelectedIndexChanged(object sender, EventArgs e)
        {
            AddChestInfoButton.Enabled = true;
        }
        private void ChestDateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            AddChestInfoButton.Enabled = true;
        }
        private void ChestValidDateComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            int iSelItm = ChestValidDateComboBox.SelectedIndex;
            if ((iSelItm - 1) >= actorClass.getNumberOfChestData())
            {
                // "Add item..." selected
                ChestDataLabel.Visible = false;
                AddChestInfoButton.Text = "Add item";
                AddChestInfoButton.Enabled = false;
                AddChestInfoButton.Visible = true;
                ChestValidDateComboBox.Enabled = false;
                ChestValidDateComboBox.Visible = false;
                ChestDateTimePicker.Enabled = true;
                ChestDateTimePicker.Visible = true;
                //ChestTypeTextBox.Text = ""; // TODO - echange for a comboBox with (Undef|Natural|Silocone|Saggy|Puffy|Nippy)
                ChestTypeTextBox.Enabled = false;
                ChestTypeTextBox.Visible = false;
                ChestTypeCmbBx.Items.Clear();
                ChestTypeCmbBx.Items.Add("Select...");
                for (int i = 0; i < actorClass.getNoOfBreastTypeStrings(); i++)
                    ChestTypeCmbBx.Items.Add(actorClass.getBreastTypeString(i));
                ChestTypeCmbBx.SelectedIndex = 0;
                ChestTypeCmbBx.Enabled = true;
                ChestTypeCmbBx.Visible = true;
                ChestCircTextBox.Text = "NN.nn UU";
                ChestCircTextBox.Enabled = true;
                ChestCircTextBox.Visible = true;
                //ChestSizeTextBox.Text = ""; // TODO - change to comboBox with (Undef, AA, A, B, C, D, E, F, Flat, Medium, Bulgy, Oversize)
                ChestSizeTextBox.Enabled = false;
                ChestSizeTextBox.Visible = false;
                ChestSizeTypeCmbBx.Items.Clear();
                ChestSizeTypeCmbBx.Items.Add("Select...");
                for (int i = 0; i < actorClass.getNoOfBreastSizeStrings(); i++)
                    ChestSizeTypeCmbBx.Items.Add(actorClass.getBreastSizeString(i));
                ChestSizeTypeCmbBx.SelectedIndex = 0;
                ChestSizeTypeCmbBx.Enabled = true;
                ChestSizeTypeCmbBx.Visible = true;
            }
            else if ((iSelItm - 1) >= 0)
            {
                // Actual Chest information selected.
                AddChestInfoButton.Text = "";
                AddChestInfoButton.Enabled = false;
                AddChestInfoButton.Visible = false;
                ChestDataLabel.Visible = true;
                ChestTypeTextBox.Text = actorClass.getUserChestType(iSelItm - 1);
                ChestTypeTextBox.Enabled = false;
                ChestTypeTextBox.Visible = true;
                ChestCircTextBox.Text = actorClass.getUserChestCircumfValue(iSelItm - 1);
                ChestCircTextBox.Enabled = false;
                ChestCircTextBox.Visible = true;
                ChestSizeTextBox.Text = actorClass.getUserChestSizeType(iSelItm - 1);
                ChestSizeTextBox.Enabled = false;
                ChestSizeTextBox.Visible = true;
            }
        }
        #endregion
        #region HairData
        private void AddHairInfoButton_Click(object sender, EventArgs e)
        {
            if (AddHairInfoButton.Text == "Add Tag")
            {
                if (!(actorClass.addHairColorCategory(HairColorTextBox.Text, HairTextureTypeTextBox.Text, actorClass.getSecrecyString(hairLengthCmbBx.SelectedIndex - 1))))
                    setInformationText("Could not add category!", informationType.ERROR, sender, e);
                // Reset GUI
                AddHairInfoButton.Text = "";
                AddHairInfoButton.Enabled = false;
                AddHairInfoButton.Visible = false;
                HairDataLabel.Visible = true;
                hairColorCmbBx.Enabled = false;
                hairColorCmbBx.Visible = false;
                HairColorTextBox.Text = "";
                HairColorTextBox.Enabled = false;
                HairColorTextBox.Visible = true;
                hairTextureCmbBx.Enabled = false;
                hairTextureCmbBx.Visible = false;
                HairTextureTypeTextBox.Text = "";
                HairTextureTypeTextBox.Enabled = false;
                HairTextureTypeTextBox.Visible = true;
                hairValidDateTxtBx.Enabled = false;
                hairValidDateTxtBx.Visible = false;
                hairDateTimePicker.Enabled = false;
                hairDateTimePicker.Visible = false;
                HairDataValidDateComboBox.Items.Clear();
                HairDataValidDateComboBox.Items.Add("Select...");
                for (int i = 0; i < actorClass.getNumberOfHairData(); i++)
                    HairDataValidDateComboBox.Items.Add(actorClass.getUserHairValidDate(i));
                HairDataValidDateComboBox.Items.Add("Add item...");
                HairDataValidDateComboBox.SelectedIndex = 0;
                HairDataValidDateComboBox.Enabled = true;
                HairDataValidDateComboBox.Visible = true;
                hairLengthCmbBx.Items.Clear();
                hairLengthCmbBx.Enabled = false;
                hairLengthCmbBx.Visible = false;
                HairLengthTextBox.Text = "";
                HairLengthTextBox.Enabled = false;
                HairLengthTextBox.Visible = true;
                toSave.actorChanges = true;
                saveToolStripMenuItem.Enabled = true;
                saveAsToolStripMenuItem.Enabled = true;
            }
            else if (AddHairInfoButton.Text == "Add Item")
            {
                int iHairColorTagNo = hairColorCmbBx.SelectedIndex;
                string sHairColorTag = "Undefined";
                if ((iHairColorTagNo - 1) >= 0)
                    sHairColorTag = actorClass.getHairColorCategoryTag(iHairColorTagNo - 1);
                int iHairTextureTagNo = hairTextureCmbBx.SelectedIndex;
                string sHairTextureTag = "Undefined";
                if ((iHairTextureTagNo - 1) >= 0)
                    sHairTextureTag = actorClass.getHairTextureString(iHairTextureTagNo - 1);
                int iHairLengthTagNo = hairLengthCmbBx.SelectedIndex;
                string sHairLengthTag = "Undefined";
                if ((iHairLengthTagNo - 1) >= 0)
                    sHairLengthTag = actorClass.getHairLengthString(iHairLengthTagNo - 1);
                actorClass.addUserHairData(sHairColorTag, sHairTextureTag, sHairLengthTag, hairDateTimePicker.Value.ToString());
                // Reset GUI
                AddHairInfoButton.Text = "";
                AddHairInfoButton.Enabled = false;
                AddHairInfoButton.Visible = false;
                HairDataLabel.Visible = true;
                hairColorCmbBx.Enabled = false;
                hairColorCmbBx.Visible = false;
                HairColorTextBox.Text = "";
                HairColorTextBox.Enabled = false;
                HairColorTextBox.Visible = true;
                hairTextureCmbBx.Enabled = false;
                hairTextureCmbBx.Visible = false;
                HairTextureTypeTextBox.Text = "";
                HairTextureTypeTextBox.Enabled = false;
                HairTextureTypeTextBox.Visible = true;
                hairValidDateTxtBx.Enabled = false;
                hairValidDateTxtBx.Visible = false;
                hairDateTimePicker.Enabled = false;
                hairDateTimePicker.Visible = false;
                HairDataValidDateComboBox.Items.Clear();
                HairDataValidDateComboBox.Items.Add("Select...");
                for (int i = 0; i < actorClass.getNumberOfHairData(); i++)
                    HairDataValidDateComboBox.Items.Add(actorClass.getUserHairValidDate(i));
                HairDataValidDateComboBox.Items.Add("Add item...");
                HairDataValidDateComboBox.SelectedIndex = 0;
                HairDataValidDateComboBox.Enabled = true;
                HairDataValidDateComboBox.Visible = true;
                hairLengthCmbBx.Items.Clear();
                hairLengthCmbBx.Enabled = false;
                hairLengthCmbBx.Visible = false;
                HairLengthTextBox.Text = "";
                HairLengthTextBox.Enabled = false;
                HairLengthTextBox.Visible = true;
                toSave.actorChanges = true;
                saveToolStripMenuItem.Enabled = true;
                saveAsToolStripMenuItem.Enabled = true;
            }
        }
        private void HairColorTextBox_TextChanged(object sender, EventArgs e)
        {
            if (AddHairInfoButton.Text == "Add Tag")
            {
                AddHairInfoButton.Enabled = true;
            }
            else
                setInformationText("Unexpected scenario!", informationType.ERROR, sender, e);
        }
        private void hairColorCmbBx_SelectedIndexChanged(object sender, EventArgs e)
        {
            int iSelItm = hairColorCmbBx.SelectedIndex;
            if ((iSelItm - 1) >= actorClass.getNoOfHairColorCategories())
            {
                // "Add Tag..." selected
                AddHairInfoButton.Text = "Add Tag";
                AddHairInfoButton.Enabled = false;
                AddHairInfoButton.Visible = true;
                hairColorCmbBx.Enabled = false;
                hairColorCmbBx.Visible = false;
                HairColorTextBox.Text = "<tag>";
                HairColorTextBox.Enabled = true;
                HairColorTextBox.Visible = true;
                hairTextureCmbBx.Enabled = false;
                hairTextureCmbBx.Visible = false;
                HairTextureTypeTextBox.Text = "<Description>";
                HairTextureTypeTextBox.Enabled = true;
                HairTextureTypeTextBox.Visible = true;
                // Secrecy level
                hairLengthCmbBx.Items.Clear();
                hairLengthCmbBx.Items.Add("Select...");
                for (int i = 0; i < actorClass.getNoOfSecrecyStrings(); i++)
                    hairLengthCmbBx.Items.Add(actorClass.getSecrecyString(i));
                hairLengthCmbBx.SelectedIndex = 0;
                hairLengthCmbBx.Enabled = true;
                hairLengthCmbBx.Visible = true;
            }
            else if ((iSelItm - 1) >= 0)
            {
                AddHairInfoButton.Enabled = true;
            }
        }
        private void HairDataValidDateComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            int iSelItm = HairDataValidDateComboBox.SelectedIndex;
            if ((iSelItm - 1) >= actorClass.getNumberOfHairData())
            {
                // "Add item..." selected.
                HairDataLabel.Visible = false;
                AddHairInfoButton.Text = "Add Item";
                AddHairInfoButton.Enabled = false;
                AddHairInfoButton.Visible = true;
                HairColorTextBox.Enabled = false;
                HairColorTextBox.Visible = false;
                hairColorCmbBx.Items.Clear();
                hairColorCmbBx.Items.Add("Select...");
                for (int i = 0; i < actorClass.getNoOfHairColorCategories(); i++)
                    hairColorCmbBx.Items.Add(actorClass.getHairColorCategoryTag(i));
                hairColorCmbBx.Items.Add("Add Tag...");
                hairColorCmbBx.SelectedIndex = 0;
                hairColorCmbBx.Enabled = true;
                hairColorCmbBx.Visible = true;
                HairTextureTypeTextBox.Enabled = false;
                HairTextureTypeTextBox.Visible = false;
                hairTextureCmbBx.Items.Clear();
                hairTextureCmbBx.Items.Add("Select...");
                for (int i = 0; i < actorClass.getNoOfHairTextureStrings(); i++)
                    hairTextureCmbBx.Items.Add(actorClass.getHairTextureString(i));
                hairTextureCmbBx.SelectedIndex = 0;
                hairTextureCmbBx.Enabled = true;
                hairTextureCmbBx.Visible = true;
                HairLengthTextBox.Enabled = false;
                HairLengthTextBox.Visible = false;
                hairLengthCmbBx.Items.Clear();
                hairLengthCmbBx.Items.Add("Select...");
                for (int i = 0; i < actorClass.getNoOfHairLengthStrings(); i++)
                    hairLengthCmbBx.Items.Add(actorClass.getHairLengthString(i));
                hairLengthCmbBx.SelectedIndex = 0;
                hairLengthCmbBx.Enabled = true;
                hairLengthCmbBx.Visible = true;
            }
            else if ((iSelItm - 1) >= 0)
            {
                // Actual hair data selected.
                AddHairInfoButton.Enabled = false;
                AddHairInfoButton.Visible = false;
                HairDataLabel.Visible = true;
                hairColorCmbBx.Enabled = false;
                hairColorCmbBx.Visible = false;
                HairColorTextBox.Text = actorClass.getHairColorCategoryTag(iSelItm - 1);
                HairColorTextBox.Enabled = false;
                HairColorTextBox.Visible = true;
                hairTextureCmbBx.Enabled = false;
                hairTextureCmbBx.Visible = false;
                HairTextureTypeTextBox.Text = actorClass.getUserHairTexture(iSelItm - 1);
                HairTextureTypeTextBox.Enabled = false;
                HairTextureTypeTextBox.Visible = true;
                hairLengthCmbBx.Enabled = false;
                hairLengthCmbBx.Visible = false;
                HairLengthTextBox.Text = actorClass.getUserHairLength(iSelItm - 1);
                HairLengthTextBox.Enabled = false;
                HairLengthTextBox.Visible = true;
            }
        }
        private void hairValidDateTxtBx_TextChanged(object sender, EventArgs e)
        {
            setInformationText("Unexpected scenario!", informationType.ERROR, sender, e);
        }
        private void hairDateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            AddHairInfoButton.Enabled = true;
        }
        private void HairTextureTypeTextBox_TextChanged(object sender, EventArgs e)
        {
            if (AddHairInfoButton.Text == "Add Tag")
                AddHairInfoButton.Enabled = true;
            else
                setInformationText("Unexpected scenario!", informationType.ERROR, sender, e);
        }
        private void hairTextureCmbBx_SelectedIndexChanged(object sender, EventArgs e)
        {
            AddHairInfoButton.Enabled = true;
        }
        private void HairLengthTextBox_TextChanged(object sender, EventArgs e)
        {
            setInformationText("Unexpected scenario!", informationType.ERROR, sender, e);
        }
        private void hairLengthCmbBx_SelectedIndexChanged(object sender, EventArgs e)
        {
            AddHairInfoButton.Enabled = true;
        }
        #endregion
        #region MarkingData
        private void MarkingTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            int iSelItm = MarkingTypeComboBox.SelectedIndex;
            if ((iSelItm - 1) >= actorClass.getNoOfMarkingsData())
            {
                // "Add item..." selected
                MarkingsLabel.Visible = false;
                AddMarkingDataButton.Text = "Add item";
                AddMarkingDataButton.Enabled = false;
                AddMarkingDataButton.Visible = true;
                MarkingTypeTextBox.Text = "";
                MarkingTypeTextBox.Enabled = false;
                MarkingTypeTextBox.Visible = false;
                MarkingTypeComboBox.Items.Clear();
                MarkingTypeComboBox.Items.Add("Select...");
                for (int i = 0; i < actorClass.getNoOfMarkingStrings(); i++)
                    MarkingTypeComboBox.Items.Add(actorClass.getMarkingString(i));
                MarkingTypeComboBox.SelectedIndex = 0;
                MarkingTypeComboBox.Enabled = true;
                MarkingTypeComboBox.Visible = true;
                MarkingPosTextBox.Text = "<Position>";
                MarkingPosTextBox.Enabled = true;
                MarkingPosTextBox.Visible = true;
                MarkingMotifTextBox.Text = "<Motif>";
                MarkingMotifTextBox.Enabled = true;
                MarkingMotifTextBox.Visible = true;
                MarkingsValidDateComboBox.Enabled = false;
                MarkingsValidDateComboBox.Visible = false;
                MarkingDateTextBox.Text = "";
                MarkingDateTextBox.Enabled = false;
                MarkingDateTextBox.Visible = false;
                MarkingsDateTimePicker.Enabled = true;
                MarkingsDateTimePicker.Visible = true;
                noOfNarrowedMarkings = 0;
            }
            else if ((iSelItm - 1) >= 0)
            {
                // Actual data item selected.
                // get the "marking type" and check if there are multiple of the same:
                //  - If only one, show that.
                //  - If multiple, update the "MarkingsValidDateComboBox" with those.
                if (noOfNarrowedMarkings > 1)
                {
                    // We have a narrowed group allready.
                    AddMarkingDataButton.Text = "";
                    AddMarkingDataButton.Enabled = false;
                    AddMarkingDataButton.Visible = false;
                    MarkingsLabel.Visible = true;
                    MarkingTypeComboBox.Enabled = false;
                    MarkingTypeComboBox.Visible = false;
                    MarkingTypeTextBox.Text = actorClass.getActorMarkingType(iSelItm - 1);
                    MarkingTypeTextBox.Enabled = false;
                    MarkingTypeTextBox.Visible = false;
                    MarkingPosTextBox.Text = actorClass.getActorMarkingPlace(iSelItm - 1);
                    MarkingPosTextBox.Enabled = false;
                    MarkingPosTextBox.Visible = true;
                    MarkingMotifTextBox.Text = actorClass.getActorMarkingMotif(iSelItm - 1);
                    MarkingMotifTextBox.Enabled = false;
                    MarkingMotifTextBox.Visible = true;
                    MarkingTypeComboBox.Enabled = false;
                    MarkingTypeComboBox.Visible = false;
                    MarkingDateTextBox.Text = actorClass.getActorMarkingValidDate(iSelItm - 1);
                    MarkingDateTextBox.Enabled = false;
                    MarkingDateTextBox.Visible = true;
                    noOfNarrowedMarkings = 0;
                }
                else
                {
                    // We are narrowing down the group.
                    string sSelMarkType = actorClass.getActorMarkingType(iSelItm - 1);
                    noOfNarrowedMarkings = 0;
                    for (int i = 0; i < actorClass.getNoOfMarkingsData(); i++)
                    {
                        if (actorClass.getActorMarkingType(i) == sSelMarkType)
                            NarrowedOriginalNumber[noOfNarrowedMarkings++] = i;
                    }
                    if (noOfNarrowedMarkings > 1)
                    {
                        MarkingsLabel.Visible = true;
                        AddMarkingDataButton.Text = "";
                        AddMarkingDataButton.Enabled = false;
                        AddMarkingDataButton.Visible = false;
                        MarkingTypeComboBox.Enabled = false;
                        MarkingTypeComboBox.Visible = false;
                        MarkingTypeTextBox.Text = sSelMarkType;
                        MarkingTypeTextBox.Enabled = false;
                        MarkingTypeTextBox.Visible = true;
                        MarkingPosTextBox.Text = "";
                        MarkingPosTextBox.Enabled = false;
                        MarkingPosTextBox.Visible = true;
                        MarkingMotifTextBox.Text = "";
                        MarkingMotifTextBox.Enabled = false;
                        MarkingMotifTextBox.Visible = true;
                        MarkingDateTextBox.Text = "";
                        MarkingDateTextBox.Enabled = false;
                        MarkingDateTextBox.Visible = false;
                        MarkingsDateTimePicker.Enabled = false;
                        MarkingsDateTimePicker.Visible = false;
                        MarkingsValidDateComboBox.Items.Clear();
                        MarkingsValidDateComboBox.Items.Add("Select...");
                        for (int i = 0; i < noOfNarrowedMarkings; i++)
                            MarkingsValidDateComboBox.Items.Add(actorClass.getActorMarkingValidDate(NarrowedOriginalNumber[i]));
                        MarkingsValidDateComboBox.SelectedIndex = 0;
                        MarkingsValidDateComboBox.Enabled = true;
                        MarkingsValidDateComboBox.Visible = true;
                    }
                    else
                    {
                        // Display the selected one.
                        MarkingsLabel.Visible = true;
                        AddMarkingDataButton.Text = "";
                        AddMarkingDataButton.Enabled = false;
                        AddMarkingDataButton.Visible = false;
                        MarkingTypeTextBox.Text = actorClass.getActorMarkingType(iSelItm - 1);
                        MarkingTypeTextBox.Enabled = false;
                        MarkingTypeTextBox.Visible = true;
                        MarkingTypeComboBox.Enabled = false;
                        MarkingTypeComboBox.Visible = false;
                        MarkingPosTextBox.Text = actorClass.getActorMarkingPlace(iSelItm - 1);
                        MarkingPosTextBox.Enabled = false;
                        MarkingPosTextBox.Visible = true;
                        MarkingMotifTextBox.Text = actorClass.getActorMarkingMotif(iSelItm - 1);
                        MarkingMotifTextBox.Enabled = false;
                        MarkingMotifTextBox.Visible = true;
                        MarkingDateTextBox.Text = "";
                        MarkingDateTextBox.Enabled = false;
                        MarkingDateTextBox.Visible = false;
                        MarkingsDateTimePicker.Enabled = false;
                        MarkingsDateTimePicker.Visible = false;
                    }
                }
            }
        }
        private void MarkingPosTextBox_TextChanged(object sender, EventArgs e)
        {
            AddMarkingDataButton.Enabled = true;
        }
        private void MarkingMotifTextBox_TextChanged(object sender, EventArgs e)
        {
            AddMarkingDataButton.Enabled = true;
        }
        private void MarkingsValidDateComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            int iSelItm = MarkingsValidDateComboBox.SelectedIndex;
            if ((iSelItm - 1) >= actorClass.getNoOfMarkingsData())
            {
                // "Add item..." selected
                MarkingsLabel.Visible = false;
                AddMarkingDataButton.Text = "Add item";
                AddMarkingDataButton.Enabled = false;
                AddMarkingDataButton.Visible = true;
                MarkingTypeTextBox.Text = "";
                MarkingTypeTextBox.Enabled = false;
                MarkingTypeComboBox.Items.Clear();
                MarkingTypeComboBox.Items.Add("Select...");
                for (int i = 0; i < actorClass.getNoOfMarkingStrings(); i++)
                    MarkingTypeComboBox.Items.Add(actorClass.getMarkingString(i));
                MarkingTypeComboBox.SelectedIndex = 0;
                MarkingTypeComboBox.Enabled = true;
                MarkingTypeComboBox.Visible = true;
                MarkingPosTextBox.Text = "<Position>";
                MarkingPosTextBox.Enabled = true;
                MarkingPosTextBox.Visible = true;
                MarkingMotifTextBox.Text = "<Motif>";
                MarkingMotifTextBox.Enabled = true;
                MarkingMotifTextBox.Visible = true;
                MarkingsValidDateComboBox.Enabled = false;
                MarkingsValidDateComboBox.Visible = false;
                MarkingDateTextBox.Text = "";
                MarkingDateTextBox.Enabled = false;
                MarkingDateTextBox.Visible = false;
                MarkingsDateTimePicker.Enabled = true;
                MarkingsDateTimePicker.Visible = true;
                noOfNarrowedMarkings = 0;
            }
            else if ((iSelItm - 1) >= 0)
            {
                // Actual data item selected.
                // Get the "Active Date" and check if there are multiple of the same:
                // - If only one, show that.
                // - If multiple, update the "MarkingsTypeComboBox" with those.
                if (noOfNarrowedMarkings > 1)
                {
                    // We have a narrowed group allready.
                    AddMarkingDataButton.Text = "";
                    AddMarkingDataButton.Enabled = false;
                    AddMarkingDataButton.Visible = false;
                    MarkingsLabel.Visible = true;
                    MarkingTypeComboBox.Enabled = false;
                    MarkingTypeComboBox.Visible = false;
                    MarkingTypeTextBox.Text = actorClass.getActorMarkingType(iSelItm - 1);
                    MarkingTypeTextBox.Enabled = false;
                    MarkingTypeTextBox.Visible = false;
                    MarkingPosTextBox.Text = actorClass.getActorMarkingPlace(iSelItm - 1);
                    MarkingPosTextBox.Enabled = false;
                    MarkingPosTextBox.Visible = true;
                    MarkingMotifTextBox.Text = actorClass.getActorMarkingMotif(iSelItm - 1);
                    MarkingMotifTextBox.Enabled = false;
                    MarkingMotifTextBox.Visible = true;
                    MarkingTypeComboBox.Enabled = false;
                    MarkingTypeComboBox.Visible = false;
                    MarkingDateTextBox.Text = actorClass.getActorMarkingValidDate(iSelItm - 1);
                    MarkingDateTextBox.Enabled = false;
                    MarkingDateTextBox.Visible = true;
                    noOfNarrowedMarkings = 0;
                }
                else
                {
                    noOfNarrowedMarkings = 0;
                    string sSelMarkDate = actorClass.getActorMarkingValidDate(iSelItm - 1);
                    for (int i = 0; i < actorClass.getNoOfMarkingsData(); i++)
                    {
                        if (actorClass.getActorMarkingValidDate(i) == sSelMarkDate)
                            NarrowedOriginalNumber[noOfNarrowedMarkings++] = i;
                    }
                    if (noOfNarrowedMarkings > 1)
                    {
                        // Multiple markings tied to the selected date.
                        MarkingsLabel.Visible = true;
                        AddMarkingDataButton.Text = "";
                        AddMarkingDataButton.Enabled = false;
                        AddMarkingDataButton.Visible = false;
                        MarkingTypeComboBox.Items.Clear();
                        MarkingTypeComboBox.Items.Add("Select...");
                        for (int i = 0; i < noOfNarrowedMarkings; i++)
                            MarkingTypeComboBox.Items.Add(actorClass.getActorMarkingType(NarrowedOriginalNumber[i]));
                        MarkingTypeComboBox.SelectedIndex = 0;
                        MarkingTypeTextBox.Enabled = true;
                        MarkingTypeTextBox.Visible = true;
                        MarkingPosTextBox.Text = "";
                        MarkingPosTextBox.Enabled = false;
                        MarkingPosTextBox.Visible = true;
                        MarkingMotifTextBox.Text = "";
                        MarkingMotifTextBox.Enabled = false;
                        MarkingMotifTextBox.Visible = true;
                        MarkingDateTextBox.Text = actorClass.getActorMarkingValidDate(iSelItm - 1);
                        MarkingDateTextBox.Enabled = false;
                        MarkingDateTextBox.Visible = true;
                        MarkingsDateTimePicker.Enabled = false;
                        MarkingsDateTimePicker.Visible = false;
                        MarkingsValidDateComboBox.Enabled = false;
                        MarkingsValidDateComboBox.Visible = false;
                    }
                    else
                    {
                        // Only one marking tied to the selected date.
                        MarkingsLabel.Visible = true;
                        AddMarkingDataButton.Text = "";
                        AddMarkingDataButton.Enabled = false;
                        AddMarkingDataButton.Visible = false;
                        MarkingTypeTextBox.Text = actorClass.getActorMarkingType(iSelItm - 1);
                        MarkingTypeTextBox.Enabled = false;
                        MarkingTypeTextBox.Visible = true;
                        MarkingTypeComboBox.Enabled = false;
                        MarkingTypeComboBox.Visible = false;
                        MarkingPosTextBox.Text = actorClass.getActorMarkingPlace(iSelItm - 1);
                        MarkingPosTextBox.Enabled = false;
                        MarkingPosTextBox.Visible = true;
                        MarkingMotifTextBox.Text = actorClass.getActorMarkingMotif(iSelItm - 1);
                        MarkingMotifTextBox.Enabled = false;
                        MarkingMotifTextBox.Visible = true;
                        MarkingDateTextBox.Text = "";
                        MarkingDateTextBox.Enabled = false;
                        MarkingDateTextBox.Visible = false;
                        MarkingsDateTimePicker.Enabled = false;
                        MarkingsDateTimePicker.Visible = false;
                    }
                }
            }
        }
        private void MarkingDateTextBox_TextChanged(object sender, EventArgs e)
        {
            AddMarkingDataButton.Enabled = true;
        }
        private void MarkingsDateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            AddMarkingDataButton.Enabled = true;
        }
        private void AddMarkingDataButton_Click(object sender, EventArgs e)
        {
            if (AddMarkingDataButton.Text == "Add item")
            {
                string sSetType = "";
                if (MarkingTypeTextBox.Text != "")
                    sSetType = MarkingTypeTextBox.Text;
                else
                    sSetType = actorClass.getMarkingString(MarkingTypeComboBox.SelectedIndex);
                string sSetPosition = MarkingPosTextBox.Text;
                string sSetMotif = MarkingMotifTextBox.Text;
                string sSetDate = MarkingsDateTimePicker.Value.ToString();
                actorClass.addActorMarkingData(sSetType, sSetPosition, sSetMotif, sSetDate);
                // Reset GUI.

            }
        }
        #endregion
        #region OccupationData
        private void OccupTitleTextBox_TextChanged(object sender, EventArgs e)
        {
            AddOccupationDataButton.Enabled = true;
            AddOccupationDataButton.Visible = true;
        }
        private void ActorOccupationStartDateComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            int iSelItm = ActorOccupationStartDateComboBox.SelectedIndex;
            if ((iSelItm - 1) >= actorClass.getNoOfOccupationsData())
            {
                // "Add item..." selected.
                label4.Visible = true;
                OccupTitleTextBox.Text = "<Title>";
                OccupTitleTextBox.Enabled = true;
                OccupTitleTextBox.Visible = true;
                occupationStartTxtBx.Text = "";
                occupationStartTxtBx.Enabled = false;
                occupationStartTxtBx.Visible = false;
                ActorOccupationStartDateComboBox.Enabled = false;
                ActorOccupationStartDateComboBox.Visible = false;
                OccupationStartDateTimePicker.Enabled = true;
                OccupationStartDateTimePicker.Visible = true;
                ActorOccupationCompanyTextBox.Text = "<Company name>";
                ActorOccupationCompanyTextBox.Enabled = true;
                ActorOccupationCompanyTextBox.Visible = true;
                ActorEmployAddressTextBox.Text = "<Streetname> NN[A]";
                ActorEmployAddressTextBox.Enabled = true;
                ActorEmployAddressTextBox.Visible = true;
                ActorEmployZipCodeTextBox.Text = "<zip-code>";
                ActorEmployZipCodeTextBox.Enabled = true;
                ActorEmployZipCodeTextBox.Visible = true;
                ActorEmpoyAreanameTextBox.Text = "<Area name>";
                ActorEmpoyAreanameTextBox.Enabled = true;
                ActorEmpoyAreanameTextBox.Visible = true;
                ActorEmployCitynameTextBox.Text = "<City name>";
                ActorEmployCitynameTextBox.Enabled = true;
                ActorEmployCitynameTextBox.Visible = true;
                occupationEndTxtBx.Text = "";
                occupationEndTxtBx.Enabled = false;
                occupationEndTxtBx.Visible = false;
                ActorOccupationEndDaeComboBox.Enabled = false;
                ActorOccupationEndDaeComboBox.Visible = false;
                OccupationEndDateTimePicker.Enabled = true;
                OccupationEndDateTimePicker.Visible = true;
                ActorEmployCountryTextBox.Text = "<Country name>";
                ActorEmployCountryTextBox.Enabled = true;
                ActorEmployCountryTextBox.Visible = true;
                AddOccupationDataButton.Text = "Add item";
                AddOccupationDataButton.Enabled = true;
                AddOccupationDataButton.Visible = true;
            }
            else if ((iSelItm - 1) >= 0)
            {
                // Actual data item selected.
                label4.Visible = true;
                OccupTitleTextBox.Text = actorClass.getActorOccupationTitle(iSelItm - 1);
                OccupTitleTextBox.Enabled = false;
                OccupTitleTextBox.Visible = true;
                occupationStartTxtBx.Text = "";
                occupationStartTxtBx.Enabled = false;
                occupationStartTxtBx.Visible = false;
                OccupationStartDateTimePicker.Enabled = false;
                OccupationStartDateTimePicker.Visible = false;
                //ActorOccupationStartDateComboBox.SelectedIndex = iSelItm;
                ActorOccupationCompanyTextBox.Text = actorClass.getActorOccupationCompany(iSelItm - 1);
                ActorOccupationCompanyTextBox.Enabled = true;
                ActorOccupationCompanyTextBox.Visible = true;
                AddOccupationDataButton.Text = "";
                AddOccupationDataButton.Enabled = false;
                AddOccupationDataButton.Visible = false;
                ActorEmployAddressTextBox.Text = actorClass.getActorOccupationStreetname(iSelItm - 1);
                ActorEmployAddressTextBox.Enabled = false;
                ActorEmployAddressTextBox.Visible = true;
                ActorEmployZipCodeTextBox.Text = actorClass.getActorOccupationZipcode(iSelItm - 1);
                ActorEmployZipCodeTextBox.Enabled = false;
                ActorEmployZipCodeTextBox.Visible = true;
                ActorEmpoyAreanameTextBox.Text = actorClass.getActorOccupationAreaname(iSelItm - 1);
                ActorEmpoyAreanameTextBox.Enabled = false;
                ActorEmpoyAreanameTextBox.Visible = true;
                ActorEmployCitynameTextBox.Text = actorClass.getActorOccupationCity(iSelItm - 1);
                ActorEmployCitynameTextBox.Enabled = false;
                ActorEmployCitynameTextBox.Visible = true;
                OccupationEndDateTimePicker.Enabled = false;
                OccupationEndDateTimePicker.Visible = false;
                ActorOccupationEndDaeComboBox.SelectedItem = iSelItm;
                occupationEndTxtBx.Text = "";
                occupationEndTxtBx.Enabled = false;
                occupationEndTxtBx.Visible = false;
                ActorEmployCountryTextBox.Text = actorClass.getActorOccupationCountry(iSelItm - 1);
                ActorEmployCountryTextBox.Enabled = false;
                ActorEmployCountryTextBox.Visible = true;
            }
        }
        private void occupationStartTxtBx_TextChanged(object sender, EventArgs e)
        {
            AddOccupationDataButton.Enabled = true;
            AddOccupationDataButton.Visible = true;
        }
        private void OccupationStartDateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            AddOccupationDataButton.Enabled = true;
        }
        private void ActorOccupationCompanyTextBox_TextChanged(object sender, EventArgs e)
        {
            AddOccupationDataButton.Enabled = true;
        }
        private void ActorEmployAddressTextBox_TextChanged(object sender, EventArgs e)
        {
            AddOccupationDataButton.Enabled = true;
        }
        private void ActorEmployZipCodeTextBox_TextChanged(object sender, EventArgs e)
        {
            AddOccupationDataButton.Enabled = true;
        }
        private void ActorEmpoyAreanameTextBox_TextChanged(object sender, EventArgs e)
        {
            AddOccupationDataButton.Enabled = true;
        }
        private void ActorEmployCitynameTextBox_TextChanged(object sender, EventArgs e)
        {
            AddOccupationDataButton.Enabled = true;
        }
        private void ActorEmployCountryTextBox_TextChanged(object sender, EventArgs e)
        {
            AddOccupationDataButton.Enabled = true;
        }
        private void AddOccupationDataButton_Click(object sender, EventArgs e)
        {
            if (AddOccupationDataButton.Text == "Add item")
            {
                string sTitle = OccupTitleTextBox.Text;
                if ((sTitle == "") || (sTitle == "<Title>"))
                    sTitle = "Unknown";
                string sStart = OccupationStartDateTimePicker.Value.ToString();
                string sCompany = ActorOccupationCompanyTextBox.Text;
                if ((sCompany == "") || (sCompany == "<Company name>"))
                    sCompany = "Unknown";
                string sStreet = ActorEmployAddressTextBox.Text;
                if ((sStreet == "") || (sStreet == "<<Streetname> NN[A]"))
                    sStreet = "Unknown";
                string sCity = ActorEmployCitynameTextBox.Text;
                if ((sCity == "") || (sCity == "<City name>"))
                    sCity = "Unknown";
                string sState = "Unknown";
                string sZipCode = ActorEmployZipCodeTextBox.Text;
                if ((sZipCode == "") || (sZipCode == "<zip-code>"))
                    sZipCode = "Unknown";
                string sArea = ActorEmpoyAreanameTextBox.Text;
                if ((sArea == "") || (sArea == "<Area name>"))
                    sArea = "Unkown";
                string sCountry = ActorEmployCountryTextBox.Text;
                if ((sCountry == "") || (sCountry == "<Country name>"))
                    sCountry = "Unknown";
                string sEnd = OccupationEndDateTimePicker.Value.ToString();
                actorClass.addActorOccupationData(sTitle, sCompany, sStreet, sCity, sState, sArea, sZipCode, sCountry, sStart, sEnd);
                // Reset GUI
                label4.Visible = true;
                OccupTitleTextBox.Text = "";
                OccupTitleTextBox.Enabled = false;
                OccupTitleTextBox.Visible = true;
                OccupationStartDateTimePicker.Enabled = false;
                OccupationStartDateTimePicker.Visible = false;
                occupationStartTxtBx.Text = "";
                occupationStartTxtBx.Enabled = false;
                occupationStartTxtBx.Visible = false;
                ActorOccupationStartDateComboBox.Items.Clear();
                ActorOccupationStartDateComboBox.Items.Add("Select...");
                for (int i = 0; i < actorClass.getNoOfOccupationsData(); i++)
                    ActorOccupationStartDateComboBox.Items.Add(actorClass.getActorOccupationStarted(i));
                ActorOccupationStartDateComboBox.Items.Add("Add item...");
                ActorOccupationStartDateComboBox.SelectedIndex = 0;
                ActorOccupationStartDateComboBox.Enabled = true;
                ActorOccupationStartDateComboBox.Visible = true;
                ActorOccupationCompanyTextBox.Text = "";
                ActorOccupationCompanyTextBox.Enabled = false;
                ActorOccupationCompanyTextBox.Visible = true;
                AddOccupationDataButton.Text = "";
                AddOccupationDataButton.Enabled = false;
                AddOccupationDataButton.Visible = false;
                ActorEmployAddressTextBox.Text = "";
                ActorEmployAddressTextBox.Enabled = false;
                ActorEmployAddressTextBox.Visible = true;
                ActorEmployZipCodeTextBox.Text = "";
                ActorEmployZipCodeTextBox.Enabled = false;
                ActorEmployZipCodeTextBox.Visible = true;
                ActorEmpoyAreanameTextBox.Text = "";
                ActorEmpoyAreanameTextBox.Enabled = false;
                ActorEmpoyAreanameTextBox.Visible = true;
                ActorEmployCitynameTextBox.Text = "";
                ActorEmployCitynameTextBox.Enabled = false;
                ActorEmployCitynameTextBox.Visible = true;
                ActorOccupationEndDaeComboBox.Items.Clear();
                ActorOccupationEndDaeComboBox.Items.Add("Select...");
                for (int i = 0; i < actorClass.getNoOfOccupationsData(); i++)
                    ActorOccupationEndDaeComboBox.Items.Add(actorClass.getActorOccupationEnded(i));
                ActorOccupationEndDaeComboBox.Items.Add("Add item...");
                ActorOccupationEndDaeComboBox.SelectedIndex = 0;
                ActorOccupationEndDaeComboBox.Enabled = true;
                ActorOccupationEndDaeComboBox.Visible = true;
                OccupationEndDateTimePicker.Enabled = false;
                OccupationEndDateTimePicker.Visible = false;
                occupationEndTxtBx.Text = "";
                occupationEndTxtBx.Enabled = false;
                occupationEndTxtBx.Visible = false;
                ActorEmployCountryTextBox.Text = "";
                ActorEmployCountryTextBox.Enabled = false;
                ActorEmployCountryTextBox.Visible = true;
            }
        }
        private void ActorOccupationEndDaeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            int iSelItm = ActorOccupationEndDaeComboBox.SelectedIndex;
            if ((iSelItm - 1) >= actorClass.getNoOfOccupationsData())
            {
                // "Add item..." selected.
                label4.Visible = true;
                OccupTitleTextBox.Text = "<Title>";
                OccupTitleTextBox.Enabled = true;
                OccupTitleTextBox.Visible = true;
                occupationStartTxtBx.Text = "";
                occupationStartTxtBx.Enabled = false;
                occupationStartTxtBx.Visible = false;
                ActorOccupationStartDateComboBox.Enabled = false;
                ActorOccupationStartDateComboBox.Visible = false;
                OccupationStartDateTimePicker.Enabled = true;
                OccupationStartDateTimePicker.Visible = true;
                ActorOccupationCompanyTextBox.Text = "<Company name>";
                ActorOccupationCompanyTextBox.Enabled = true;
                ActorOccupationCompanyTextBox.Visible = true;
                ActorEmployAddressTextBox.Text = "<Streetname> NN[A]";
                ActorEmployAddressTextBox.Enabled = true;
                ActorEmployAddressTextBox.Visible = true;
                ActorEmployZipCodeTextBox.Text = "<zip-code>";
                ActorEmployZipCodeTextBox.Enabled = true;
                ActorEmployZipCodeTextBox.Visible = true;
                ActorEmpoyAreanameTextBox.Text = "<Area name>";
                ActorEmpoyAreanameTextBox.Enabled = true;
                ActorEmpoyAreanameTextBox.Visible = true;
                ActorEmployCitynameTextBox.Text = "<City name>";
                ActorEmployCitynameTextBox.Enabled = true;
                ActorEmployCitynameTextBox.Visible = true;
                occupationEndTxtBx.Text = "";
                occupationEndTxtBx.Enabled = false;
                occupationEndTxtBx.Visible = false;
                ActorOccupationEndDaeComboBox.Enabled = false;
                ActorOccupationEndDaeComboBox.Visible = false;
                OccupationEndDateTimePicker.Enabled = true;
                OccupationEndDateTimePicker.Visible = true;
                ActorEmployCountryTextBox.Text = "<Country name>";
                ActorEmployCountryTextBox.Enabled = true;
                ActorEmployCountryTextBox.Visible = true;
                AddOccupationDataButton.Text = "Add item";
                AddOccupationDataButton.Enabled = true;
                AddOccupationDataButton.Visible = true;
            }
            else if ((iSelItm - 1) >= 0)
            {
                // Actual data information selected.
                label4.Visible = true;
                OccupTitleTextBox.Text = actorClass.getActorOccupationTitle(iSelItm - 1);
                OccupTitleTextBox.Enabled = false;
                OccupTitleTextBox.Visible = true;
                occupationStartTxtBx.Text = "";
                occupationStartTxtBx.Enabled = false;
                occupationStartTxtBx.Visible = false;
                OccupationStartDateTimePicker.Enabled = false;
                OccupationStartDateTimePicker.Visible = false;
                ActorOccupationStartDateComboBox.SelectedIndex = iSelItm;
                ActorOccupationStartDateComboBox.Enabled = true;
                ActorOccupationStartDateComboBox.Visible = true;
                ActorOccupationCompanyTextBox.Text = actorClass.getActorOccupationCompany(iSelItm - 1);
                ActorOccupationCompanyTextBox.Enabled = true;
                ActorOccupationCompanyTextBox.Visible = true;
                AddOccupationDataButton.Text = "";
                AddOccupationDataButton.Enabled = false;
                AddOccupationDataButton.Visible = false;
                ActorEmployAddressTextBox.Text = actorClass.getActorOccupationStreetname(iSelItm - 1);
                ActorEmployAddressTextBox.Enabled = false;
                ActorEmployAddressTextBox.Visible = true;
                ActorEmployZipCodeTextBox.Text = actorClass.getActorOccupationZipcode(iSelItm - 1);
                ActorEmployZipCodeTextBox.Enabled = false;
                ActorEmployZipCodeTextBox.Visible = true;
                ActorEmpoyAreanameTextBox.Text = actorClass.getActorOccupationAreaname(iSelItm - 1);
                ActorEmpoyAreanameTextBox.Enabled = false;
                ActorEmpoyAreanameTextBox.Visible = true;
                ActorEmployCitynameTextBox.Text = actorClass.getActorOccupationCity(iSelItm - 1);
                ActorEmployCitynameTextBox.Enabled = false;
                ActorEmployCitynameTextBox.Visible = true;
                OccupationEndDateTimePicker.Enabled = false;
                OccupationEndDateTimePicker.Visible = false;
                occupationEndTxtBx.Text = "";
                occupationEndTxtBx.Enabled = false;
                occupationEndTxtBx.Visible = false;
                //ActorOccupationEndDaeComboBox.SelectedIndex = iSelItm;
                //ActorOccupationEndDaeComboBox.Enabled = true;
                //ActorOccupationEndDaeComboBox.Visible = true;
                ActorEmployCountryTextBox.Text = actorClass.getActorOccupationCountry(iSelItm - 1);
                ActorEmployCountryTextBox.Enabled = false;
                ActorEmployCountryTextBox.Visible = true;
            }
        }
        private void occupationEndTxtBx_TextChanged(object sender, EventArgs e)
        {
            AddOccupationDataButton.Enabled = true;
        }
        private void OccupationEndDateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            AddOccupationDataButton.Enabled = true;
        }
        #endregion
        #region ResidenceData
        private void ResidAddressTextBox_TextChanged(object sender, EventArgs e)
        {
            AddResidenceButton.Enabled = true;
        }
        private void ResidZipCodeTextBox_TextChanged(object sender, EventArgs e)
        {
            AddResidenceButton.Enabled = true;
        }
        private void ResidAreanameTextBox_TextChanged(object sender, EventArgs e)
        {
            AddResidenceButton.Enabled = true;
        }
        private void ResidCitynameTextBox_TextChanged(object sender, EventArgs e)
        {
            AddResidenceButton.Enabled = true;
        }
        private void ResidCountryTextBox_TextChanged(object sender, EventArgs e)
        {
            AddResidenceButton.Enabled = true;
        }
        private void ResidStartDateComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            int iSelItm = ResidStartDateComboBox.SelectedIndex;
            if ((iSelItm - 1) >= actorClass.getNoOfResicenceData())
            {
                // "Add item..." selected.
                ResidenceLabel.Visible = true;
                ResidAddressTextBox.Text = "<Streetname> NN[A]";
                ResidAddressTextBox.Enabled = true;
                ResidAddressTextBox.Visible = true;
                residenceStartTxtBx.Text = "";
                residenceStartTxtBx.Enabled = false;
                residenceStartTxtBx.Visible = false;
                ResidStartDateComboBox.Enabled = false;
                ResidStartDateComboBox.Visible = false;
                ResidStartDateTimePicker.Enabled = true;
                ResidStartDateTimePicker.Visible = true;
                ResidZipCodeTextBox.Text = "<zip code>";
                ResidZipCodeTextBox.Enabled = true;
                ResidZipCodeTextBox.Visible = true;
                ResidAreanameTextBox.Text = "<Area name>";
                ResidAreanameTextBox.Enabled = true;
                ResidAreanameTextBox.Visible = true;
                AddResidenceButton.Text = "Add item";
                AddResidenceButton.Enabled = false;
                AddResidenceButton.Visible = true;
                ResidCitynameTextBox.Text = "<Cityname>";
                ResidCitynameTextBox.Enabled = true;
                ResidCitynameTextBox.Visible = true;
                residenceEndTxtBx.Text = "";
                residenceEndTxtBx.Enabled = false;
                residenceEndTxtBx.Visible = false;
                ResidEndDateComboBox.Enabled = false;
                ResidEndDateComboBox.Visible = false;
                ResidEndDateTimePicker.Enabled = true;
                ResidEndDateTimePicker.Visible = true;
                ResidCountryTextBox.Text = "<Countryname>";
                ResidCountryTextBox.Enabled = true;
                ResidCountryTextBox.Visible = true;
            }
            else if ((iSelItm - 1) >= 0)
            {
                // Actual data item selected.
                ResidenceLabel.Visible = true;
                ResidAddressTextBox.Text = actorClass.getActorResidStreetname(iSelItm - 1);
                ResidAddressTextBox.Enabled = false;
                ResidAddressTextBox.Visible = true;
                residenceStartTxtBx.Text = "";
                residenceStartTxtBx.Enabled = false;
                residenceStartTxtBx.Visible = false;
                ResidStartDateTimePicker.Enabled = false;
                ResidStartDateTimePicker.Visible = false;
                ResidZipCodeTextBox.Text = actorClass.getActorResidZipcode(iSelItm - 1);
                ResidZipCodeTextBox.Enabled = false;
                ResidZipCodeTextBox.Visible = true;
                ResidAreanameTextBox.Text = actorClass.getActorResidArea(iSelItm - 1);
                ResidAreanameTextBox.Enabled = false;
                ResidAreanameTextBox.Visible = true;
                AddResidenceButton.Text = "";
                AddResidenceButton.Enabled = false;
                AddResidenceButton.Visible = false;
                ResidCitynameTextBox.Text = actorClass.getActorResidCity(iSelItm - 1);
                ResidCitynameTextBox.Enabled = false;
                ResidCitynameTextBox.Visible = true;
                residenceEndTxtBx.Text = "";
                residenceEndTxtBx.Enabled = false;
                residenceEndTxtBx.Visible = false;
                ResidEndDateTimePicker.Enabled = false;
                ResidEndDateTimePicker.Visible = false;
                ResidEndDateComboBox.SelectedIndex = iSelItm;
                ResidCountryTextBox.Text = actorClass.getActorResidCountry(iSelItm - 1);
                ResidCountryTextBox.Enabled = false;
                ResidCountryTextBox.Visible = true;
            }
        }
        private void ResidStartDateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            AddResidenceButton.Enabled = true;
        }
        private void residenceStartTxtBx_TextChanged(object sender, EventArgs e)
        {
            AddResidenceButton.Enabled = true;
        }
        private void AddResidenceButton_Click(object sender, EventArgs e)
        {
            char[] chars = { 'a', 'A', 'b', 'B', 'c', 'C', 'd', 'D', 'e', 'E', 'f', 'F', 'g', 'G', 'h', 'H', 'i', 'I', 'j', 'J', 'k', 'K', 'l', 'L',
                                 'm', 'M', 'n', 'N', 'o', 'O', 'p', 'P', 'q', 'Q', 'r', 'R', 's', 'S', 't', 'T', 'u', 'U', 'v', 'V', 'x', 'X', 'y', 'Y',
                                 'z', 'Z', 'å', 'Å', 'ä', 'Ä', 'ö', 'Ö'};
            string sStreetname = ResidAddressTextBox.Text;
            int dp = sStreetname.IndexOf(" ");
            int iStreetNo = 0;
            string sStreetnameAddOn = "";
            if ((dp > 0) && (dp < sStreetname.Length))
            {
                sStreetnameAddOn = sStreetname.Substring(dp + 1);
                sStreetname = sStreetname.Substring(0, dp);
                dp = sStreetnameAddOn.IndexOfAny(chars);
                if ((dp > 0) && (dp < sStreetnameAddOn.Length))
                {
                    if (!(int.TryParse(sStreetnameAddOn.Substring(0, dp), out iStreetNo)))
                        setInformationText("Could not parse number!", informationType.ERROR, sender, e);
                    sStreetnameAddOn = sStreetnameAddOn.Substring(dp + 1).ToUpper();
                }
            }
            string sCityname = ResidCitynameTextBox.Text;
            string sAreaname = ResidAreanameTextBox.Text;
            string sCountry = ResidCountryTextBox.Text;
            int iZipCode = 0;
            if (!int.TryParse(ResidZipCodeTextBox.Text, out iZipCode))
                setInformationText("Parsing zip-code didn't work!", informationType.ERROR, sender, e);
            string sBoughtDate = ResidStartDateTimePicker.Value.ToString();
            string sSoldDate = ResidEndDateTimePicker.Value.ToString();
            float fBoughtSum = 0; // TODO - Add handling for bought sum.
            float fSoldSum = 0; // TODO - Add handling for sold sum.
            string sCurrencyUsed = ""; // TODO - This will be a part of the bought-, and sold-sum handling.
            actorClass.addActorResidenceData(sStreetname, iStreetNo, sStreetnameAddOn, sCityname, sAreaname, iZipCode, sCountry, sBoughtDate, sSoldDate, fBoughtSum, fSoldSum, sCurrencyUsed);
        }
        private void ResidEndDateComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            int iSelItm = ResidStartDateComboBox.SelectedIndex;
            if ((iSelItm - 1) >= actorClass.getNoOfResicenceData())
            {
                // "Add item..." selected.
                ResidenceLabel.Visible = true;
                ResidAddressTextBox.Text = "<Streetname> NN[A]";
                ResidAddressTextBox.Enabled = true;
                ResidAddressTextBox.Visible = true;
                residenceStartTxtBx.Text = "";
                residenceStartTxtBx.Enabled = false;
                residenceStartTxtBx.Visible = false;
                ResidStartDateComboBox.Enabled = false;
                ResidStartDateComboBox.Visible = false;
                ResidStartDateTimePicker.Enabled = true;
                ResidStartDateTimePicker.Visible = true;
                ResidZipCodeTextBox.Text = "<zip code>";
                ResidZipCodeTextBox.Enabled = true;
                ResidZipCodeTextBox.Visible = true;
                ResidAreanameTextBox.Text = "<Area name>";
                ResidAreanameTextBox.Enabled = true;
                ResidAreanameTextBox.Visible = true;
                AddResidenceButton.Text = "Add item";
                AddResidenceButton.Enabled = false;
                AddResidenceButton.Visible = true;
                ResidCitynameTextBox.Text = "<Cityname>";
                ResidCitynameTextBox.Enabled = true;
                ResidCitynameTextBox.Visible = true;
                residenceEndTxtBx.Text = "";
                residenceEndTxtBx.Enabled = false;
                residenceEndTxtBx.Visible = false;
                ResidEndDateComboBox.Enabled = false;
                ResidEndDateComboBox.Visible = false;
                ResidEndDateTimePicker.Enabled = true;
                ResidEndDateTimePicker.Visible = true;
                ResidCountryTextBox.Text = "<Countryname>";
                ResidCountryTextBox.Enabled = true;
                ResidCountryTextBox.Visible = true;
            }
            else if ((iSelItm - 1) >= 0)
            {
                // Actual data item selected.
                ResidenceLabel.Visible = true;
                ResidAddressTextBox.Text = actorClass.getActorResidStreetname(iSelItm - 1);
                ResidAddressTextBox.Enabled = false;
                ResidAddressTextBox.Visible = true;
                residenceStartTxtBx.Text = "";
                residenceStartTxtBx.Enabled = false;
                residenceStartTxtBx.Visible = false;
                ResidStartDateTimePicker.Enabled = false;
                ResidStartDateTimePicker.Visible = false;
                ResidStartDateComboBox.SelectedIndex = iSelItm;
                ResidZipCodeTextBox.Text = actorClass.getActorResidZipcode(iSelItm - 1);
                ResidZipCodeTextBox.Enabled = false;
                ResidZipCodeTextBox.Visible = true;
                ResidAreanameTextBox.Text = actorClass.getActorResidArea(iSelItm - 1);
                ResidAreanameTextBox.Enabled = false;
                ResidAreanameTextBox.Visible = true;
                AddResidenceButton.Text = "";
                AddResidenceButton.Enabled = false;
                AddResidenceButton.Visible = false;
                ResidCitynameTextBox.Text = actorClass.getActorResidCity(iSelItm - 1);
                ResidCitynameTextBox.Enabled = false;
                ResidCitynameTextBox.Visible = true;
                residenceEndTxtBx.Text = "";
                residenceEndTxtBx.Enabled = false;
                residenceEndTxtBx.Visible = false;
                ResidEndDateTimePicker.Enabled = false;
                ResidEndDateTimePicker.Visible = false;
                ResidCountryTextBox.Text = actorClass.getActorResidCountry(iSelItm - 1);
                ResidCountryTextBox.Enabled = false;
                ResidCountryTextBox.Visible = true;
            }
        }
        private void ResidEndDateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            AddResidenceButton.Enabled = true;
        }
        private void residenceEndTxtBx_TextChanged(object sender, EventArgs e)
        {
            AddResidenceButton.Enabled = true;
        }
        #endregion
        #region EventData
        private void EventIdTextBox_TextChanged(object sender, EventArgs e)
        {
            addAttdEventBtn.Enabled = true;
        }
        private void AttEvtIDCmbBx_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (actorClass != null)
            {
                int iSelItm = AttEvtIDCmbBx.SelectedIndex;
                if ((iSelItm - 1) >= actorClass.getNoOfAttendedEventsData())
                {
                    // "Add item..." selected.
                    AttEventLabel.Visible = false;
                    addAttdEventBtn.Text = "Add item";
                    addAttdEventBtn.Enabled = false;
                    addAttdEventBtn.Visible = true;
                    AttEvtIDCmbBx.Enabled = false;
                    AttEvtIDCmbBx.Visible = false;
                    EventIdTextBox.Text = "<Event ID>";
                    EventIdTextBox.Enabled = true;
                    EventIdTextBox.Visible = true;
                    AttdEventsDateComboBox.Enabled = false;
                    AttdEventsDateComboBox.Visible = false;
                    attdEvtDateTxtBx.Text = "";
                    attdEvtDateTxtBx.Enabled = false;
                    attdEvtDateTxtBx.Visible = false;
                    AttdEventsDateTimePicker.Enabled = true;
                    AttdEventsDateTimePicker.Visible = true;
                    EventCategTxtBx.Text = "<Event Category>";
                    EventCategTxtBx.Enabled = true;
                    EventCategTxtBx.Visible = true;
                    RoleTagTxtBx.Text = "";
                    RoleTagTxtBx.Enabled = false;
                    RoleTagTxtBx.Visible = false;
                    RoleTagCmboBx.Items.Clear();
                    RoleTagCmboBx.Items.Add("Select...");
                    for (int i = 0; i < actorClass.getNoOfRoleCategories(); i++)
                        RoleTagCmboBx.Items.Add(actorClass.getRoleCategoryTag(i));
                    RoleTagCmboBx.Items.Add("Add item...");
                    RoleTagCmboBx.SelectedIndex = 0;
                    RoleTagCmboBx.Enabled = true;
                    RoleTagCmboBx.Visible = true;
                    AttdEvtEndDateTxtBx.Text = "";
                    AttdEvtEndDateTxtBx.Enabled = false;
                    AttdEvtEndDateTxtBx.Visible = false;
                    AttdEventEndDateCmbBx.Enabled = false;
                    AttdEventEndDateCmbBx.Visible = false;
                    AttdEventEndDateTimePicker.Enabled = true;
                    AttdEventEndDateTimePicker.Visible = true;
                }
                else if ((iSelItm - 1) >= 0)
                {
                    // Actual data item selected.
                    addAttdEventBtn.Text = "";
                    addAttdEventBtn.Enabled = false;
                    addAttdEventBtn.Visible = false;
                    AttEventLabel.Visible = true;
                    EventIdTextBox.Text = "";
                    EventIdTextBox.Enabled = false;
                    EventIdTextBox.Visible = false;
                    attdEvtDateTxtBx.Text = "";
                    attdEvtDateTxtBx.Enabled = false;
                    attdEvtDateTxtBx.Visible = false;
                    AttdEventsDateTimePicker.Enabled = false;
                    AttdEventsDateTimePicker.Visible = false;
                    AttdEventsDateComboBox.SelectedIndex = iSelItm;
                    AttdEventsDateComboBox.Enabled = true;
                    AttdEventsDateComboBox.Visible = true;
                    EventCategTxtBx.Text = actorClass.getEventCategoryDescription(iSelItm - 1);
                    RoleTagTxtBx.Text = "";
                    RoleTagTxtBx.Enabled = false;
                    RoleTagTxtBx.Visible = false;
                    RoleTagCmboBx.SelectedIndex = iSelItm;
                    RoleTagCmboBx.Enabled = true;
                    RoleTagCmboBx.Visible = true;
                    AttdEvtEndDateTxtBx.Text = "";
                    AttdEvtEndDateTxtBx.Enabled = false;
                    AttdEvtEndDateTxtBx.Visible = false;
                    AttdEventEndDateTimePicker.Enabled = false;
                    AttdEventEndDateTimePicker.Visible = false;
                    AttdEventEndDateCmbBx.SelectedIndex = iSelItm;
                    AttdEventEndDateCmbBx.Enabled = true;
                    AttdEventEndDateCmbBx.Visible = true;
                }
            }
        }
        private void AttdEventsDateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            addAttdEventBtn.Enabled = true;
        }
        private void attdEvtDateTxtBx_TextChanged(object sender, EventArgs e)
        {
            addAttdEventBtn.Enabled = true;
        }
        private void AttdEventsDateComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (actorClass != null)
            {
                int iSelItm = AttEvtIDCmbBx.SelectedIndex;
                if ((iSelItm - 1) >= actorClass.getNoOfAttendedEventsData())
                {
                    // "Add item..." selected.
                    AttEventLabel.Visible = false;
                    addAttdEventBtn.Text = "Add item";
                    addAttdEventBtn.Enabled = false;
                    addAttdEventBtn.Visible = true;
                    AttEvtIDCmbBx.Enabled = false;
                    AttEvtIDCmbBx.Visible = false;
                    EventIdTextBox.Text = "<Event ID>";
                    EventIdTextBox.Enabled = true;
                    EventIdTextBox.Visible = true;
                    AttdEventsDateComboBox.Enabled = false;
                    AttdEventsDateComboBox.Visible = false;
                    attdEvtDateTxtBx.Text = "";
                    attdEvtDateTxtBx.Enabled = false;
                    attdEvtDateTxtBx.Visible = false;
                    AttdEventsDateTimePicker.Enabled = true;
                    AttdEventsDateTimePicker.Visible = true;
                    EventCategTxtBx.Text = "<Event Category>";
                    EventCategTxtBx.Enabled = true;
                    EventCategTxtBx.Visible = true;
                    RoleTagTxtBx.Text = "";
                    RoleTagTxtBx.Enabled = false;
                    RoleTagTxtBx.Visible = false;
                    RoleTagCmboBx.Items.Clear();
                    RoleTagCmboBx.Items.Add("Select...");
                    for (int i = 0; i < actorClass.getNoOfRoleCategories(); i++)
                        RoleTagCmboBx.Items.Add(actorClass.getRoleCategoryTag(i));
                    RoleTagCmboBx.Items.Add("Add item...");
                    RoleTagCmboBx.SelectedIndex = 0;
                    RoleTagCmboBx.Enabled = true;
                    RoleTagCmboBx.Visible = true;
                    AttdEvtEndDateTxtBx.Text = "";
                    AttdEvtEndDateTxtBx.Enabled = false;
                    AttdEvtEndDateTxtBx.Visible = false;
                    AttdEventEndDateCmbBx.Enabled = false;
                    AttdEventEndDateCmbBx.Visible = false;
                    AttdEventEndDateTimePicker.Enabled = true;
                    AttdEventEndDateTimePicker.Visible = true;
                }
                else if ((iSelItm - 1) >= 0)
                {
                    // Actual data item selected.
                    addAttdEventBtn.Text = "";
                    addAttdEventBtn.Enabled = false;
                    addAttdEventBtn.Visible = false;
                    AttEventLabel.Visible = true;
                    EventIdTextBox.Text = "";
                    EventIdTextBox.Enabled = false;
                    EventIdTextBox.Visible = false;
                    AttEvtIDCmbBx.SelectedIndex = iSelItm;
                    AttEvtIDCmbBx.Enabled = true;
                    AttEvtIDCmbBx.Visible = true;
                    attdEvtDateTxtBx.Text = "";
                    attdEvtDateTxtBx.Enabled = false;
                    attdEvtDateTxtBx.Visible = false;
                    AttdEventsDateTimePicker.Enabled = false;
                    AttdEventsDateTimePicker.Visible = false;
                    //AttdEventsDateComboBox.SelectedIndex = iSelItm;
                    //AttdEventsDateComboBox.Enabled = true;
                    //AttdEventsDateComboBox.Visible = true;
                    EventCategTxtBx.Text = actorClass.getEventCategoryDescription(iSelItm - 1);
                    RoleTagTxtBx.Text = "";
                    RoleTagTxtBx.Enabled = false;
                    RoleTagTxtBx.Visible = false;
                    RoleTagCmboBx.SelectedIndex = iSelItm;
                    RoleTagCmboBx.Enabled = true;
                    RoleTagCmboBx.Visible = true;
                    AttdEvtEndDateTxtBx.Text = "";
                    AttdEvtEndDateTxtBx.Enabled = false;
                    AttdEvtEndDateTxtBx.Visible = false;
                    AttdEventEndDateTimePicker.Enabled = false;
                    AttdEventEndDateTimePicker.Visible = false;
                    AttdEventEndDateCmbBx.SelectedIndex = iSelItm;
                    AttdEventEndDateCmbBx.Enabled = true;
                    AttdEventEndDateCmbBx.Visible = true;
                }
            }
        }
        private void EventCategTxtBx_TextChanged(object sender, EventArgs e)
        {
            addAttdEventBtn.Enabled = true;
        }
        private void RoleTagTxtBx_TextChanged(object sender, EventArgs e)
        {
            addAttdEventBtn.Enabled = true;
        }
        private void RoleTagCmboBx_SelectedIndexChanged(object sender, EventArgs e)
        {
            int iSelItm = AttEvtIDCmbBx.SelectedIndex;
            if ((iSelItm - 1) >= actorClass.getNoOfAttendedEventsData())
            {
                // "Add item..." selected.
                AttEventLabel.Visible = false;
                addAttdEventBtn.Text = "Add item";
                addAttdEventBtn.Enabled = false;
                addAttdEventBtn.Visible = true;
                AttEvtIDCmbBx.Enabled = false;
                AttEvtIDCmbBx.Visible = false;
                EventIdTextBox.Text = "<Event ID>";
                EventIdTextBox.Enabled = true;
                EventIdTextBox.Visible = true;
                AttdEventsDateComboBox.Enabled = false;
                AttdEventsDateComboBox.Visible = false;
                attdEvtDateTxtBx.Text = "";
                attdEvtDateTxtBx.Enabled = false;
                attdEvtDateTxtBx.Visible = false;
                AttdEventsDateTimePicker.Enabled = true;
                AttdEventsDateTimePicker.Visible = true;
                EventCategTxtBx.Text = "<Event Category>";
                EventCategTxtBx.Enabled = true;
                EventCategTxtBx.Visible = true;
                RoleTagTxtBx.Text = "";
                RoleTagTxtBx.Enabled = false;
                RoleTagTxtBx.Visible = false;
                RoleTagCmboBx.Items.Clear();
                RoleTagCmboBx.Items.Add("Select...");
                for (int i = 0; i < actorClass.getNoOfRoleCategories(); i++)
                    RoleTagCmboBx.Items.Add(actorClass.getRoleCategoryTag(i));
                RoleTagCmboBx.Items.Add("Add item...");
                RoleTagCmboBx.SelectedIndex = 0;
                RoleTagCmboBx.Enabled = true;
                RoleTagCmboBx.Visible = true;
                AttdEvtEndDateTxtBx.Text = "";
                AttdEvtEndDateTxtBx.Enabled = false;
                AttdEvtEndDateTxtBx.Visible = false;
                AttdEventEndDateCmbBx.Enabled = false;
                AttdEventEndDateCmbBx.Visible = false;
                AttdEventEndDateTimePicker.Enabled = true;
                AttdEventEndDateTimePicker.Visible = true;
            }
            else if ((iSelItm - 1) >= 0)
            {
                // Actual data item selected.
                addAttdEventBtn.Text = "";
                addAttdEventBtn.Enabled = false;
                addAttdEventBtn.Visible = false;
                AttEventLabel.Visible = true;
                EventIdTextBox.Text = "";
                EventIdTextBox.Enabled = false;
                EventIdTextBox.Visible = false;
                AttEvtIDCmbBx.SelectedIndex = iSelItm;
                AttEvtIDCmbBx.Enabled = true;
                AttEvtIDCmbBx.Visible = true;
                attdEvtDateTxtBx.Text = "";
                attdEvtDateTxtBx.Enabled = false;
                attdEvtDateTxtBx.Visible = false;
                AttdEventsDateTimePicker.Enabled = false;
                AttdEventsDateTimePicker.Visible = false;
                AttdEventsDateComboBox.SelectedIndex = iSelItm;
                AttdEventsDateComboBox.Enabled = true;
                AttdEventsDateComboBox.Visible = true;
                EventCategTxtBx.Text = actorClass.getEventCategoryDescription(iSelItm - 1);
                RoleTagTxtBx.Text = "";
                RoleTagTxtBx.Enabled = false;
                RoleTagTxtBx.Visible = false;
                //RoleTagCmboBx.SelectedIndex = iSelItm;
                //RoleTagCmboBx.Enabled = true;
                //RoleTagCmboBx.Visible = true;
                AttdEvtEndDateTxtBx.Text = "";
                AttdEvtEndDateTxtBx.Enabled = false;
                AttdEvtEndDateTxtBx.Visible = false;
                AttdEventEndDateTimePicker.Enabled = false;
                AttdEventEndDateTimePicker.Visible = false;
                AttdEventEndDateCmbBx.SelectedIndex = iSelItm;
                AttdEventEndDateCmbBx.Enabled = true;
                AttdEventEndDateCmbBx.Visible = true;
            }
        }
        private void AttdEventEndDateCmbBx_SelectedIndexChanged(object sender, EventArgs e)
        {
            addAttdEventBtn.Enabled = true;
        }
        private void AttdEventEndDateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            addAttdEventBtn.Enabled = true;
        }
        private void AttdEvtEndDateTxtBx_TextChanged(object sender, EventArgs e)
        {
            addAttdEventBtn.Enabled = true;
        }
        private void addAttdEventBtn_Click(object sender, EventArgs e)
        {
            string sEvtID = EventIdTextBox.Text;
            string sEvtType = EventCategTxtBx.Text;
            string sEvtStart = AttdEventsDateTimePicker.Value.ToString();
            string sEvtEnd = AttdEventEndDateTimePicker.Value.ToString();
            int iSelRole = RoleTagCmboBx.SelectedIndex;
            string sRole = actorClass.getActorAttendedEventRoleTag(iSelRole);
            actorClass.addActorAttendedEventData(sEvtID, sEvtType, sEvtStart, sEvtEnd, sRole);
            // Reset GUI
            addAttdEventBtn.Enabled = false;
            addAttdEventBtn.Visible = false;
            AttEventLabel.Visible = true;
            EventIdTextBox.Text = "";
            EventIdTextBox.Enabled = false;
            EventIdTextBox.Visible = false;
            AttEvtIDCmbBx.Items.Clear();
            AttEvtIDCmbBx.Items.Add("Select...");
            for (int i = 0; i < actorClass.getNoOfAttendedEventsData(); i++)
                AttEvtIDCmbBx.Items.Add(actorClass.getActorAttendedEventID(i));
            AttEvtIDCmbBx.Items.Add("Add item...");
            AttEvtIDCmbBx.SelectedIndex = 0;
            AttEvtIDCmbBx.Enabled = true;
            AttEvtIDCmbBx.Visible = true;
            attdEvtDateTxtBx.Text = "";
            attdEvtDateTxtBx.Enabled = false;
            attdEvtDateTxtBx.Visible = false;
            AttdEventsDateTimePicker.Enabled = false;
            AttdEventsDateTimePicker.Visible = false;
            AttdEventsDateComboBox.Items.Clear();
            AttdEventsDateComboBox.Items.Add("Select...");
            for (int i = 0; i < actorClass.getNoOfAttendedEventsData(); i++)
                AttdEventsDateComboBox.Items.Add(actorClass.getActorAttendedEventStarted(i));
            AttdEventsDateComboBox.Items.Add("Add item...");
            AttdEventsDateComboBox.SelectedIndex = 0;
            AttdEventsDateComboBox.Enabled = true;
            AttdEventsDateComboBox.Visible = true;
            EventCategTxtBx.Text = "";
            EventCategTxtBx.Enabled = false;
            EventCategTxtBx.Visible = true;
            RoleTagTxtBx.Text = "";
            RoleTagTxtBx.Enabled = false;
            RoleTagTxtBx.Visible = false;
            RoleTagCmboBx.Items.Clear();
            RoleTagCmboBx.Items.Add("Select...");
            for (int i = 0; i < actorClass.getNoOfAttendedEventsData(); i++)
                RoleTagCmboBx.Items.Add(actorClass.getActorAttendedEventRoleTag(i));
            RoleTagCmboBx.Items.Add("Add item...");
            RoleTagCmboBx.SelectedIndex = 0;
            RoleTagCmboBx.Enabled = true;
            RoleTagCmboBx.Visible = true;
            AttdEvtEndDateTxtBx.Text = "";
            AttdEvtEndDateTxtBx.Enabled = false;
            AttdEvtEndDateTxtBx.Visible = false;
            AttdEventEndDateTimePicker.Enabled = false;
            AttdEventEndDateTimePicker.Visible = false;
            AttdEventEndDateCmbBx.Items.Clear();
            AttdEventEndDateCmbBx.Items.Add("Select...");
            for (int i = 0; i < actorClass.getNoOfAttendedEventsData(); i++)
                AttdEventEndDateCmbBx.Items.Add(actorClass.getActorAttendedEventEnded(i));
            AttdEventEndDateCmbBx.Items.Add("Add item...");
            AttdEventEndDateCmbBx.SelectedIndex = 0;
            AttdEventEndDateCmbBx.Enabled = true;
            AttdEventEndDateCmbBx.Visible = true;
        }
        #endregion
        #region RelatedImageData
        private void ActorRelImageComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!(iAmWorking))
            {
                string tempRelIm = ActorRelImageComboBox.SelectedItem.ToString();
                if (tempRelIm == "Find image...")
                {
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
                        actorClass.addActorRelatedImage(ofd.FileName, "", "Undefined");
                        this.ActorRelImageComboBox.Items.Clear();
                        this.ActorRelImageComboBox.Items.Add("Select image");
                        for (int i = 0; i < actorClass.getNoOfRelatedImages(); i++)
                        {
                            string foundActorRelImage = actorClass.getActorRelatedImagePath(i);
                            int dp = foundActorRelImage.LastIndexOf("\\");
                            foundActorRelImage = foundActorRelImage.Substring(dp + 1, foundActorRelImage.Length - dp - 1);
                            this.ActorRelImageComboBox.Items.Add(foundActorRelImage);
                        }
                        this.ActorRelImageComboBox.Items.Add("Find image...");
                        this.ActorRelImageComboBox.Width = 116;
                        this.ViewSelActorRelImageButton.Text = "View image";
                        //this.ViewSelActorRelImageButton.Enabled = true;
                        this.ViewSelActorRelImageButton.Visible = true;
                    }
                    ActorRleImgContextTxtBx.Text = "<Context>";
                    ActorRleImgContextTxtBx.Enabled = true;
                    ActorRleImgContextTxtBx.Visible = true;
                }
                else if (tempRelIm == "Update root")
                {
                    // TODO - Var faan är knappen?
                    addRelActorImagesBtn.Text = "Root update";
                    addRelActorImagesBtn.Visible = true;
                    addRelActorImagesBtn.Enabled = true;
                }
                else
                {
                    ViewSelActorRelImageButton.Text = "View image";
                    ViewSelActorRelImageButton.Visible = true;
                    if ((ActorRelImageComboBox.SelectedIndex > 0) && (File.Exists(actorClass.getActorRelatedImagePath(Math.Max(ActorRelImageComboBox.SelectedIndex - 1, 0)))))
                        ViewSelActorRelImageButton.Enabled = true;
                    else
                        ViewSelActorRelImageButton.Enabled = false;
                    ActorRleImgContextTxtBx.Text = actorClass.getActorRelatedImageContent(ActorRelImageComboBox.SelectedIndex - 1);
                }
            }
        }
        private void ActorRleImgContextTxtBx_TextChanged(object sender, EventArgs e)
        {
            addRelActorImagesBtn.Enabled = true;
        }
        private void ViewSelActorRelImageButton_Click(object sender, EventArgs e)
        {
            if (ViewSelActorRelImageButton.Text == "View image")
            {
                loadedImageFileName = actorClass.getActorRelatedImagePath(Math.Max(this.ActorRelImageComboBox.SelectedIndex, 0));
                int dpp = loadedImageFileName.LastIndexOf("\\");
                linwin.setLastImageDirectoryValue(loadedImageFileName.Substring(0, dpp));
                picture = Image.FromFile(loadedImageFileName);
                if ((picture.Width > pictureCanvas.Width) || (picture.Height > pictureCanvas.Height))
                    pictureCanvas.SizeMode = PictureBoxSizeMode.Zoom;
                else
                    pictureCanvas.SizeMode = PictureBoxSizeMode.Normal;
                getImageMetadataValues(loadedImageFileName, sender, e);
                pictureCanvas.Image = picture;
                pictureName.Text = loadedImageFileName;
                expandedImage = true;
                ViewSelEventImageButton.Text = "Hide image";
                viewingImage = true;
            }
            else if (ViewSelEventImageButton.Text == "Hide image")
            {
                pictureCanvas.Image.Dispose();
                picture.Dispose();
                ViewSelEventImageButton.Text = "View image";
                viewingImage = false;
            }
        }
        private void addRelActorImagesBtn_Click(object sender, EventArgs e)
        {
            if (addRelActorImagesBtn.Text == "Add Rel. img.")
            {
                // TODO - add singular images related to the actor.
                setInformationText("Lacking the singular image adding functionality.", informationType.ERROR, sender, e);
            }
            else
            {
                string currActorRootDir = actorClass.getActorRootDir(Math.Max(cmbBoxRootDir.SelectedIndex -1, 0));
                if ((currActorRootDir != null) && (currActorRootDir != "") && (File.Exists(currActorRootDir)))
                {
                    string[] relDirArray = System.IO.Directory.GetDirectories(currActorRootDir);
                    string[] relFileArray = System.IO.Directory.GetFiles(currActorRootDir);
                    foreach (var exiFile in relFileArray)
                    {
                        bool foundFile = false;
                        for (int fnr = 0; fnr < actorClass.getNoOfRelatedImages(); fnr++)
                        {
                            if (exiFile.ToString() == actorClass.getActorRelatedImagePath(fnr))
                                foundFile = true;
                        }
                        if (!(foundFile))
                            actorClass.addActorRelatedImage(exiFile.ToString(), "Unknown", "Unknown");
                    }
                    foreach (var exiDir in relDirArray)
                    {
                        relFileArray = System.IO.Directory.GetFiles(exiDir.ToString());
                        foreach (var exiFile in relFileArray)
                        {
                            bool foundFileInDir = false;
                            for (int fnr = 0; fnr < actorClass.getNoOfRelatedImages(); fnr++)
                            {
                                if (exiFile.ToString() == actorClass.getActorRelatedImagePath(fnr))
                                    foundFileInDir = true;
                            }
                            if (!(foundFileInDir))
                                actorClass.addActorRelatedImage(exiFile.ToString(), "Unknown", "Unknown");
                        }
                    }
                }
            }
        }
        #endregion
        #region RootDir
        private void updRootBtn_Click(object sender, EventArgs e)
        {
            if ((updRootBtn.Text == "Add") || (updRootBtn.Text == "Set"))
            {
                int iLastMax = actorClass.getNoOfRootDirs();
                folderBrowserDialog.RootFolder = Environment.SpecialFolder.MyComputer;
                DialogResult dres = folderBrowserDialog.ShowDialog();
                if (!actorClass.addActorRootDir(folderBrowserDialog.SelectedPath.ToString(), linwin.getActorStoragePath()))
                    setInformationText("Adding actor root dir failed.", informationType.ERROR, sender, e);
                // TODO - Update the 'cmbBoxRootDir'
                cmbBoxRootDir.Items.Clear();
                cmbBoxRootDir.Items.Add("Select...");
                for (int i = 0; i < actorClass.getNoOfRootDirs(); i++)
                {
                    if (i < actorClass.getMaxNoOfRootDirs())
                        cmbBoxRootDir.Items.Add(actorClass.getActorRootDir(i));
                }
                cmbBoxRootDir.Items.Add("Add root dir");
                cmbBoxRootDir.SelectedIndex = 0;// actorClass.getNoOfRootDirs() + 1;
                cmbBoxRootDir.Enabled = true;
                //updRootBtn.Text = "Add root";
            }
            else if (updRootBtn.Text == "Upd.")
            {
                string sSelDir = actorClass.getActorRootDir(Math.Max(cmbBoxRootDir.SelectedIndex - 1, 0));
                string saveActorFilePaths = linwin.getActorStoragePath();// "";
                if ((linwin.getActorStoragePath() != "") && (System.IO.Directory.Exists(linwin.getActorStoragePath())))
                    saveActorFilePaths = linwin.getActorStoragePath() + "\\" + ActiveArtistsComboBox.SelectedItem.ToString();
                else if (System.IO.Directory.Exists(rootPath + "\\ActorData\\"))
                    saveActorFilePaths = rootPath + "\\ActorData\\" + ActiveArtistsComboBox.SelectedItem.ToString();
                actorClass.findDirsInDir(sSelDir, saveActorFilePaths);
                btnDelRoot.Text = "";
                btnDelRoot.Visible = false;
                btnDelRoot.Enabled = false;
                updRootBtn.Size = new Size(103, 20);
                updRootBtn.Text = "Add";
                ActorRelImageComboBox.Items.Clear();
                ActorRelImageComboBox.Items.Add("Select...");
                for (int i = 0; i < actorClass.getNoOfRelatedImages(); i++)
                {
                    string srim = actorClass.getActorRelatedImagePath(i);
                    int idp = srim.LastIndexOf("\\");
                    srim = srim.Substring(idp + 1, srim.Length - idp - 1);
                    ActorRelImageComboBox.Items.Add(srim);
                }
                ActorRelImageComboBox.Items.Add("Add image");
                ActorRelImageComboBox.Visible = true;
                ActorRelImageComboBox.Enabled = true;
                SaveActorDataChangesButton.Text = "Save changes";
                SaveActorDataChangesButton.Visible = true;
                SaveActorDataChangesButton.Enabled = true;
            }
        }
        private void rootDirTxtBx_TextChanged(object sender, EventArgs e)
        {
            // TODO - read path and update data.
            setInformationText("Textual adding of root dir path not functional.", informationType.ERROR, sender, e);
        }
        private void btnDelRoot_Click(object sender, EventArgs e)
        {
            if (btnDelRoot.Text == "Del.")
            {
                int iSelIdx = cmbBoxRootDir.SelectedIndex;
                if (((iSelIdx - 1) >= 0) && ((iSelIdx - 1) < actorClass.getNoOfRootDirs()))
                {
                    string sSelIdx = actorClass.getActorRootDir(iSelIdx - 1);
                    actorClass.removeActorRootDir(sSelIdx, linwin.getActorStoragePath());
                    cmbBoxRootDir.Items.Clear();
                    cmbBoxRootDir.Items.Add("Select...");
                    for (int i = 0; i < actorClass.getNoOfRootDirs(); i++)
                    {
                        if (i < actorClass.getMaxNoOfRootDirs())
                            cmbBoxRootDir.Items.Add(actorClass.getActorRootDir(i));
                    }
                    cmbBoxRootDir.Items.Add("Add root dir");
                    cmbBoxRootDir.SelectedIndex = 0;
                    cmbBoxRootDir.Enabled = true;
                }
            }
        }
        private void cmbBoxRootDir_SelectedIndexChanged(object sender, EventArgs e)
        {
            rootDirTxtBx.Visible = false;
            //string userSelectedRootDir = actorClass.getActorRootDir(Math.Max(cmbBoxRootDir.SelectedIndex - 1, 0));
            int iUserSelection = cmbBoxRootDir.SelectedIndex - 1;
            if (iUserSelection >= 0)
            {
                string sUsrSelRootDirItem = cmbBoxRootDir.SelectedText;
                string userSelectedRootDir = actorClass.getActorRootDir(iUserSelection);
                if (userSelectedRootDir == "Add root dir")
                {
                    updRootBtn.Text = "Add";
                    updRootBtn.Visible = true;
                    updRootBtn.Enabled = true;
                }
                else if ((userSelectedRootDir != "Select...") && (userSelectedRootDir != ""))
                {
                    updRootBtn.Text = "Upd.";
                    updRootBtn.Size = new Size(54, 20);
                    updRootBtn.Visible = true;
                    if (System.IO.Directory.Exists(userSelectedRootDir))
                        updRootBtn.Enabled = true;
                    else
                        updRootBtn.Enabled = false;
                    btnDelRoot.Location = new Point(updRootBtn.Location.X + 54, updRootBtn.Location.Y);
                    btnDelRoot.Text = "Del.";
                    btnDelRoot.Visible = true;
                    btnDelRoot.Enabled = true;
                }
                else
                {
                    updRootBtn.Size = new Size(54, 20);
                    updRootBtn.Text = "";
                    updRootBtn.Visible = true;
                    updRootBtn.Enabled = false;
                    btnDelRoot.Location = new Point(updRootBtn.Location.X + 54, updRootBtn.Location.Y);
                    btnDelRoot.Text = "";
                    btnDelRoot.Visible = true;
                    btnDelRoot.Enabled = false;
                }
            }
        }
        #endregion
        #endregion
        #region Event Information Handling
        // --- Event information handling ---
        private void setUpAddEvent()
        {
            //            this.pictureCanvas.Visible = false;
            eventClass = new PEEventClass();
            EventIdLabel.Visible = true;
            // ActiveEvent...
            activeEventComboBox.Visible = false;
            ActiveEventTextBox.Text = "";
            ActiveEventTextBox.Visible = true;
            ActiveEventTextBox.Enabled = true;
            // Copyright...
            CopyrightLabel.Visible = true;
            CopyrightTextBox.Text = "";
            CopyrightTextBox.Enabled = false;
            CopyrightTextBox.Visible = false;
            string sASP = linwin.getActorStoragePath();
            if ((sASP != "") && (System.IO.Directory.Exists(sASP)))
                actorFilePaths = System.IO.Directory.GetFiles(sASP + "\\", "ActorData_*.acf");
            else
                actorFilePaths = System.IO.Directory.GetFiles(rootPath + "\\ActorData\\", "ActorData_*.acf");
            CopyrightComboBox.Items.Clear();
            CopyrightComboBox.Items.Add("Select...");
            foreach (var efp in actorFilePaths)
            {
                int dp = efp.LastIndexOf("\\");
                string stef = efp.Substring(dp + 1);
                CopyrightComboBox.Items.Add(stef);
                noOfActors++;
            }
            CopyrightComboBox.Items.Add("Add owner...");
            CopyrightComboBox.SelectedIndex = 0;
            CopyrightComboBox.Enabled = true;
            CopyrightComboBox.Visible = true;
            // Secrecy level...
            EventSecrLevelLabel.Visible = true;
            EventSecrecyLevelTextBox.Text = "";
            EventSecrecyLevelTextBox.Enabled = false;
            EventSecrecyLevelTextBox.Visible = false;
            EventSecrecyLevelComboBox.Items.Clear();
            EventSecrecyLevelComboBox.Items.Add("Select...");
            for (int i = 0; i < linwin.getNoOfSecrecyStrings(); i++)
                EventSecrecyLevelComboBox.Items.Add(linwin.getSecrecyString(i));
            EventSecrecyLevelComboBox.SelectedIndex = 0;
            EventSecrecyLevelComboBox.Enabled = true;
            EventSecrecyLevelComboBox.Visible = true;
            // Start-Stop ...
            EventStartEndLabel.Visible = true;
            EventStartTextBox.Text = "";
            EventStartTextBox.Enabled = false;
            EventStartTextBox.Visible = false;
            EventStartDateTimePicker.Enabled = true;
            EventStartDateTimePicker.Visible = true;
            EventEndTextBox.Text = "";
            EventEndTextBox.Enabled = false;
            EventEndTextBox.Visible = false;
            EventEndDateTimePicker.Enabled = true;
            EventEndDateTimePicker.Visible = true;
            // Event headline...
            EventHeadlineLabel.Visible = true;
            EventHeadlineTextBox.Text = "";
            EventHeadlineTextBox.Enabled = true;
            EventHeadlineTextBox.Visible = true;
            // Event latitude, longitude ...
            EventLatitudeTextBox.Text = "";
            EventLatitudeTextBox.Enabled = true;
            EventLatitudeTextBox.Visible = true;
            EventLongitudeTextBox.Text = "";
            EventLongitudeTextBox.Enabled = true;
            EventLongitudeTextBox.Visible = true;
            // Event geographic name ...
            GeoNameLabel.Visible = true;
            EventGeographNameTextBox.Text = "";
            EventGeographNameTextBox.Enabled = true;
            EventGeographNameTextBox.Visible = true;
            // Event streetname ...
            EventAddressLabel.Visible = true;
            EventStreetnameNumberTextBox.Text = "";
            EventStreetnameNumberTextBox.Enabled = true;
            EventStreetnameNumberTextBox.Visible = true;
            EventZipCodeTextBox.Text = "";
            EventZipCodeTextBox.Enabled = true;
            EventZipCodeTextBox.Visible = true;
            EventAreanameTextBox.Text = "";
            EventAreanameTextBox.Enabled = true;
            EventAreanameTextBox.Visible = true;
            EventCitynameTextBox.Text = "";
            EventCitynameTextBox.Enabled = true;
            EventCitynameTextBox.Visible = true;
            EventCountrynameTextBox.Text = "";
            EventCountrynameTextBox.Enabled = true;
            EventCountrynameTextBox.Visible = true;
            // Attenders ...
            EventAttenderIDComboBox1.Items.Clear();
            EventAttenderIDComboBox1.Items.Add("Select...");
            foreach (var efp in actorFilePaths)
            {
                int dp = efp.LastIndexOf("\\");
                string stef = efp.Substring(dp + 1);
                EventAttenderIDComboBox1.Items.Add(stef);
            }
            EventAttenderIDComboBox1.Items.Add("Add attender...");
            EventAttenderIDComboBox1.SelectedIndex = 0;
            EventAttenderIDComboBox1.Enabled = true;
            EventAttenderIDComboBox1.Visible = true;
            EventAttenderNaneTextBox.Text = "<Level>, <Role>";
            EventAttenderNaneTextBox.Enabled = false;
            EventAttenderNaneTextBox.Visible = false;
            ViewAttenderAsActorButton.Text = "Add attender";
            ViewAttenderAsActorButton.Visible = true;
            ViewAttenderAsActorButton.Enabled = false;
            // Image names...
            EventImageNameComboBox.Items.Clear();
            EventImageNameComboBox.Items.Add("Select...");
            EventImageNameComboBox.Items.Add("Add item...");
            EventImageNameComboBox.SelectedIndex = 0;
            EventImageNameComboBox.Enabled = false;
            EventImageNameComboBox.Visible = true;
            ViewSelEventImageButton.Text = "Find image";
            ViewSelEventImageButton.Visible = true;
            ViewSelEventImageButton.Enabled = true;
            // Event root...
            EventRootLabel.Visible = true;
            EventRootTextBox.Text = "";
            EventRootTextBox.Enabled = false;
            EventRootTextBox.Visible = true;
            EventRootComboBox.Enabled = false;
            EventRootComboBox.Visible = false;
            EventRootButton.Text = "Add root";
            EventRootButton.Enabled = true;
            EventRootButton.Visible = true;
            // Event description...
            EventDescriptionTextBox.Text = "";
            EventDescriptionTextBox.Enabled = true;
            EventDescriptionTextBox.Visible = true;
            // Save, Discard data...
            SaveEventDataChangesButton.Text = "Save data";
            SaveEventDataChangesButton.Visible = true;
            SaveEventDataChangesButton.Enabled = false;
            DiscardActorDataChangesButton.Text = "Discard data";
            DiscardEventDataChangesButton.Visible = true;
            DiscardEventDataChangesButton.Enabled = false;
        }
        private void setUpSelEvent(string selEvt, object sender, EventArgs e)
        {
            // Get selected event...
            eventClass = new PEEventClass();
            // Event related categories
            for (int i = 0; i < linwin.noOfEventCategories; i++)
                eventClass.addEventCategory(linwin.getEvtCatTag(i), linwin.getEvtCatDescr(i), linwin.getEvtCatLevel(i));
            for (int i = 0; i < linwin.noOfContentCategories; i++)
                eventClass.addContentCategory(linwin.getContCatTag(i), linwin.getContCatDescr(i), linwin.getContCatLevel(i));
            for (int i = 0; i < linwin.getNoOfRoles(); i++)
                eventClass.addRoleCategory(linwin.getRoleTag(i), linwin.getRoleDescr(i), linwin.getRoleLevel(i));
            string storpa = linwin.getEventStoragePath();
            // Load selected event...
            if ((storpa != null) && (selEvt != null) && (System.IO.Directory.Exists(storpa)) && (System.IO.File.Exists(storpa + "\\" + selEvt)))
                eventClass.loadEvent(selEvt, storpa);
            else if ((storpa != null) && (selEvt != null) && (System.IO.Directory.Exists(storpa)) && (System.IO.File.Exists(storpa + "\\EventData_" + selEvt + ".edf")))
                eventClass.loadEvent(selEvt, storpa);
            else if ((System.IO.Directory.Exists("C:\\Users\\" + currUser + "\\" + sProgPath + "\\EventData")) &&
                     (System.IO.File.Exists("C:\\Users" + currUser + "\\" + sProgPath + "\\EventData\\EventData_" + selEvt + ".edf")))
                eventClass.loadEvent(selEvt, "C:\\Users\\" + currUser + "\\" + sProgPath + "\\EventData\\");
            else if ((System.IO.Directory.Exists("C:\\Användare\\" + currUser + "\\" + sProgPath + "\\EventData")) &&
                     (System.IO.File.Exists("C:\\Användare\\" + currUser + "\\" + sProgPath + "\\EventData\\EventData_" + selEvt + ".edf")))
                eventClass.loadEvent(selEvt, "C:\\Användare\\" + currUser + "\\" + sProgPath + "\\EventData\\");
            if (eventClass.getEventLevelValue() <= linwin.getUserRightsValue())
            {
                // Active event...
                EventIdLabel.Visible = true;
                activeEventComboBox.Enabled = false;
                activeEventComboBox.Visible = false;
                ActiveEventTextBox.Text = eventClass.getEventID();
                ActiveEventTextBox.Enabled = false;
                ActiveEventTextBox.Visible = true;
                // Copyright...
                CopyrightLabel.Visible = true;
                CopyrightComboBox.Enabled = false;
                CopyrightComboBox.Visible = false;
                string sEvtOwner = eventClass.getEventOwner();
                if ((sEvtOwner != null) && (sEvtOwner != ""))
                    CopyrightTextBox.Text = sEvtOwner;
                else
                    CopyrightTextBox.Text = "Unknown";
                CopyrightTextBox.Enabled = false;
                CopyrightTextBox.Visible = true;
                // Secrecy level...
                EventSecrLevelLabel.Visible = true;
                EventSecrecyLevelComboBox.Enabled = false;
                EventSecrecyLevelComboBox.Visible = false;
                EventSecrecyLevelTextBox.Text = eventClass.getEventLevel();
                EventSecrecyLevelTextBox.Enabled = false;
                EventSecrecyLevelTextBox.Visible = true;
                // Event Start-End...
                EventStartEndLabel.Visible = true;
                EventStartDateTimePicker.Enabled = false;
                EventStartDateTimePicker.Visible = false;
                EventStartTextBox.Text = eventClass.getEventStarted();
                EventStartTextBox.Enabled = false;
                EventStartTextBox.Visible = true;
                EventEndDateTimePicker.Enabled = false;
                EventEndDateTimePicker.Visible = false;
                EventEndTextBox.Text = eventClass.getEventEnded();
                EventEndTextBox.Enabled = false;
                EventEndTextBox.Visible = true;
                // Event headline...
                EventHeadlineLabel.Visible = true;
                EventHeadlineTextBox.Text = eventClass.getEventHeadline();
                EventHeadlineTextBox.Enabled = false;
                EventHeadlineTextBox.Visible = true;
                // Latitude-Longitude...
                EventGeoPosLabel.Visible = true;
                EventLatitudeTextBox.Text = eventClass.getEventLatPos();
                EventLatitudeTextBox.Enabled = false;
                EventLatitudeTextBox.Visible = true;
                EventLongitudeTextBox.Text = eventClass.getEventLonPos();
                EventLongitudeTextBox.Enabled = false;
                EventLongitudeTextBox.Visible = true;
                // Geographical name...
                GeoNameLabel.Visible = true;
                EventGeographNameTextBox.Text = eventClass.getEventGeoPosName();
                EventGeographNameTextBox.Enabled = false;
                EventGeographNameTextBox.Visible = true;
                // Event address...
                EventAddressLabel.Visible = true;
                EventStreetnameNumberTextBox.Text = eventClass.getEventStreetname();
                EventStreetnameNumberTextBox.Enabled = false;
                EventStreetnameNumberTextBox.Visible = true;
                EventZipCodeTextBox.Text = eventClass.getEventAreacode();
                EventZipCodeTextBox.Enabled = false;
                EventZipCodeTextBox.Visible = true;
                EventAreanameTextBox.Text = eventClass.getEventAreaname();
                EventAreanameTextBox.Enabled = false;
                EventAreanameTextBox.Visible = true;
                EventCitynameTextBox.Text = eventClass.getEventCityname();
                EventCitynameTextBox.Enabled = false;
                EventCitynameTextBox.Visible = true;
                EventCountrynameTextBox.Text = eventClass.getEventCountryname();
                EventCountrynameTextBox.Enabled = false;
                EventCountrynameTextBox.Visible = true;
                // Attender ID...
                EventAttenderIDComboBox1.Items.Clear();
                EventAttenderIDComboBox1.Items.Add("Select...");
                for (int i = 0; i < eventClass.getNoOfEventAttender(); i++)
                    EventAttenderIDComboBox1.Items.Add(eventClass.getEventAttenderID(i));
                EventAttenderIDComboBox1.Items.Add("Add attender...");
                EventAttenderIDComboBox1.SelectedIndex = 0;
                EventAttenderIDComboBox1.Enabled = true;
                EventAttenderIDComboBox1.Visible = true;
                EventAttenderNaneTextBox.Text = "";
                EventAttenderNaneTextBox.Enabled = false;
                EventAttenderNaneTextBox.Visible = true;
                ViewAttenderAsActorButton.Text = "View Attender as Actor";
                ViewAttenderAsActorButton.Enabled = false;
                ViewAttenderAsActorButton.Visible = true;
                // Event images...
                EventImageNameComboBox.Items.Clear();
                EventImageNameComboBox.Items.Add("Select...");
                for (int i = 0; i < eventClass.getNoOfEventImages(); i++)
                {
                    if (eventClass.getEventImageContentLevel(i) <= linwin.getUserRightsValue())
                    {
                        string sWrkString = eventClass.getEventImageName(i);
                        int iDel = sWrkString.LastIndexOf("\\");
                        if ((iDel > 0) && (iDel < sWrkString.Length))
                            sWrkString = sWrkString.Substring(iDel);
                        EventImageNameComboBox.Items.Add(sWrkString);
                    }
                }
                EventImageNameComboBox.Items.Add("Add image...");
                EventImageNameComboBox.SelectedIndex = 0;
                EventImageNameComboBox.Enabled = true;
                EventImageNameComboBox.Visible = true;
                ViewSelEventImageButton.Text = "View image";
                ViewSelEventImageButton.Enabled = false;
                ViewSelEventImageButton.Visible = true;
                // Event root...
                EventRootLabel.Visible = true;
                EventRootTextBox.Text = "";
                EventRootTextBox.Enabled = false;
                EventRootTextBox.Visible = false;
                EventRootComboBox.Items.Clear();
                EventRootComboBox.Items.Add("Select...");
                for (int i = 0; i < eventClass.getNoOfEventRoots(); i++)
                {
                    string sWrkString = eventClass.getEventRoot(i);
                    int iDel = sWrkString.LastIndexOf("\\");
                    if ((iDel > 0) && (iDel < sWrkString.Length))
                        sWrkString = sWrkString.Substring(iDel);
                    EventRootComboBox.Items.Add(sWrkString);
                }
                if (eventClass.getNoOfEventRoots() < eventClass.getMaxNoOfEventRoots())
                    EventRootComboBox.Items.Add("Add root...");
                EventRootComboBox.SelectedIndex = 0;
                EventRootComboBox.Enabled = true;
                EventRootComboBox.Visible = true;
                EventRootButton.Text = "";
                EventRootButton.Enabled = false;
                EventRootButton.Visible = true;
                // EventDescription...
                string sEventContent = "";
                for (int i = 0; i < eventClass.getNumberOfContentDescription(); i++)
                    sEventContent = sEventContent + "\n" + eventClass.getEventContentDescription(i);
                EventDescriptionTextBox.Text = sEventContent;
                EventDescriptionTextBox.Enabled = false;
                EventDescriptionTextBox.Visible = true;
                // Save-Discard buttons
                SaveEventDataChangesButton.Enabled = false;
                SaveEventDataChangesButton.Visible = true;
                DiscardEventDataChangesButton.Enabled = false;
                DiscardEventDataChangesButton.Visible = true;
            }
            else
                setInformationText("Not appropriate rights!", informationType.ERROR, sender, e);
        }
        private void activeEventComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            eventStartUpDone = false;
            int iSelItm = activeEventComboBox.SelectedIndex;
            if((iSelItm - 1) >= noOfEvents)
                setUpAddEvent();
            else if (((iSelItm - 1) >= 0))
            {
                string valtEvent = activeEventComboBox.SelectedItem.ToString();
                eventToShow = Math.Max(activeEventComboBox.SelectedIndex - 1, 0);
                setUpSelEvent(valtEvent, sender, e);
            }            
            eventStartUpDone = true;
        }
        private void ActiveEventTextBox_TextChanged(object sender, EventArgs e)
        {
            if ((eventClass != null) && (ActiveEventTextBox.Text != ""))
            {
                SaveEventDataChangesButton.Enabled = true;
                DiscardEventDataChangesButton.Enabled = true;
            }
        }
        private void CopyrightTextBox_TextChanged(object sender, EventArgs e)
        {
            if ((eventClass != null) && (CopyrightTextBox.Text != ""))
            {
                SaveEventDataChangesButton.Enabled = true;
                DiscardEventDataChangesButton.Enabled = true;
            }
        }
        private void CopyrightComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (eventClass != null)
            {
                int iSelItm = CopyrightComboBox.SelectedIndex;
                if ((iSelItm - 1) >= noOfActors)
                {
                    actorViewToolStripMenuItem_Click(sender, e);
                    addNewArtist(sender, e);
                }
                else if ((iSelItm - 1) >= 0)
                {
                    SaveEventDataChangesButton.Enabled = true;
                    DiscardEventDataChangesButton.Enabled = true;
                }
            }
        }
        private void EventSecrecyLevelTextBox_TextChanged(object sender, EventArgs e)
        {
            if ((eventClass != null) && (EventSecrecyLevelTextBox.Text != ""))
            {
                SaveEventDataChangesButton.Enabled = true;
                DiscardEventDataChangesButton.Enabled = true;
            }
        }
        private void EventSecrecyLevelComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (eventClass != null)
            {
                SaveEventDataChangesButton.Enabled = true;
                DiscardEventDataChangesButton.Enabled = true;
            }
        }
        private void EventStartTextBox_TextChanged(object sender, EventArgs e)
        {
            if ((eventClass != null) && (EventStartTextBox.Text != ""))
            {
                SaveEventDataChangesButton.Enabled = true;
                DiscardEventDataChangesButton.Enabled = true;
            }
        }
        private void EventStartDateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            if (eventClass != null)
            {
                SaveEventDataChangesButton.Enabled = true;
                DiscardEventDataChangesButton.Enabled = true;
            }
        }
        private void EventEndTextBox_TextChanged(object sender, EventArgs e)
        {
            if ((eventClass != null) && (EventEndTextBox.Text != ""))
            {
                SaveEventDataChangesButton.Enabled = true;
                DiscardEventDataChangesButton.Enabled = true;
            }
        }
        private void EventEndDateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            if (eventClass != null)
            {
                SaveEventDataChangesButton.Enabled = true;
                DiscardEventDataChangesButton.Enabled = true;
            }
        }
        private void EventHeadlineTextBox_TextChanged(object sender, EventArgs e)
        {
            if ((eventClass != null) && (EventHeadlineTextBox.Text != ""))
            {
                SaveEventDataChangesButton.Enabled = true;
                DiscardEventDataChangesButton.Enabled = true;
            }
        }
        private void EventLatitudeTextBox_TextChanged(object sender, EventArgs e)
        {
            if ((eventClass != null) && (EventLatitudeTextBox.Text != ""))
            {
                SaveEventDataChangesButton.Enabled = true;
                DiscardEventDataChangesButton.Enabled = true;
            }
        }
        private void EventLongitudeTextBox_TextChanged(object sender, EventArgs e)
        {
            if ((eventClass != null) && (EventLongitudeTextBox.Text != ""))
            {
                SaveEventDataChangesButton.Enabled = true;
                DiscardEventDataChangesButton.Enabled = true;
            }
        }
        private void EventGeographNameTextBox_TextChanged(object sender, EventArgs e)
        {
            if ((eventClass != null) && (EventGeographNameTextBox.Text != ""))
            {
                SaveEventDataChangesButton.Enabled = true;
                DiscardEventDataChangesButton.Enabled = true;
            }
        }
        private void EventStreetnameNumberTextBox_TextChanged(object sender, EventArgs e)
        {

            if ((eventClass != null) && (EventStreetnameNumberTextBox.Text != ""))
            {
                SaveEventDataChangesButton.Enabled = true;
                DiscardEventDataChangesButton.Enabled = true;
            }
        }
        private void EventZipCodeTextBox_TextChanged(object sender, EventArgs e)
        {
            if ((eventClass != null) && (EventZipCodeTextBox.Text != ""))
            {
                SaveEventDataChangesButton.Enabled = true;
                DiscardEventDataChangesButton.Enabled = true;
            }
        }
        private void EventAreanameTextBox_TextChanged(object sender, EventArgs e)
        {
            if ((eventClass != null) && (EventAreanameTextBox.Text != ""))
            {
                SaveEventDataChangesButton.Enabled = true;
                DiscardEventDataChangesButton.Enabled = true;
            }
        }
        private void EventCitynameTextBox_TextChanged(object sender, EventArgs e)
        {
            if ((eventClass != null) && (EventCitynameTextBox.Text != ""))
            {
                SaveEventDataChangesButton.Enabled = true;
                DiscardEventDataChangesButton.Enabled = true;
            }
        }
        private void EventCountrynameTextBox_TextChanged(object sender, EventArgs e)
        {
            if ((eventClass != null) && (EventCountrynameTextBox.Text != ""))
            {
                SaveEventDataChangesButton.Enabled = true;
                DiscardEventDataChangesButton.Enabled = true;
            }
        }
        private void EventAttenderNaneTextBox_TextChanged(object sender, EventArgs e)
        {
            if ((eventClass != null) && (EventAttenderNaneTextBox.Text != ""))
            {
                SaveEventDataChangesButton.Enabled = true;
                DiscardEventDataChangesButton.Enabled = true;
            }
        }
        private void EventAttenderIDComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Text in "ViewAttenderAsActorButton" can be "Add attender" or "View Attender as Actor"
            int selVal = EventAttenderIDComboBox1.SelectedIndex;
            if ((selVal - 1) >= eventClass.noOfEventAttender)
            {
                // "Add attender..." selected.
                EventAttenderIDComboBox1.Items.Add("Select...");
                foreach (var efp in actorFilePaths)
                {
                    int dp = efp.LastIndexOf("\\");
                    string stef = efp.Substring(dp + 1);
                    EventAttenderIDComboBox1.Items.Add(stef);
                }
                EventAttenderIDComboBox1.SelectedIndex = 0;
                EventAttenderIDComboBox1.Enabled = true;
                EventAttenderIDComboBox1.Visible = true;
                ViewAttenderAsActorButton.Text = "Add attender";
                ViewAttenderAsActorButton.Enabled = true;
                ViewAttenderAsActorButton.Visible = true;
            }
            else if ((selVal - 1) >= 0)
            {
                EventAttenderNaneTextBox.Text = eventClass.getEventAttenderID(selVal - 1);
                EventAttenderNaneTextBox.Enabled = false;
                EventAttenderNaneTextBox.Visible = true;
                ViewAttenderAsActorButton.Text = "View Attender as Actor";
                ViewAttenderAsActorButton.Enabled = true;
                ViewAttenderAsActorButton.Visible = true;
            }
        }
        private void ViewAttenderAsActorButton_Click(object sender, EventArgs e)
        {
            if (eventClass != null)
            {
                if (ViewAttenderAsActorButton.Text == "View Attender as Actor")
                {
                    this.currentMode = programMode.ACTORVIEW;
                    this.tabControl.SelectedTab = this.tabControl.TabPages["ActorData"];
                    /* -------------------------- */
                    Screen screen = Screen.PrimaryScreen;
                    Rectangle screenRect = screen.Bounds;
                    screenRect.Height = screenRect.Height - 50;
                    this.Size = new Size(screenRect.Width, screenRect.Height);
                    pictureName.Location = new Point(screenRect.Width - 335, 26);
                    // --- Tab handling ---
                    tabControl.Location = new Point(screenRect.Width - 335, 53);
                    tabControl.Size = new Size(330, screenRect.Height - 100);
                    int tabHeight = screenRect.Height - 100;
                    int tabWidth = Math.Max(335, (int)(screenRect.Width * 0.1677));
                    HandleGenDataTabGui(tabWidth, tabHeight);
                    HandleHWDataTabGui(tabWidth, tabHeight);
                    HandleHWAddonDataTabGui(tabWidth, tabHeight);
                    HandleGeoDataTabGui(tabWidth, tabHeight);
                    HandleActorDataTabGui(tabWidth, tabHeight);
                    HandleEventDataTabGui(tabWidth, tabHeight);
                    HandleSortingTabGui(tabWidth, tabHeight);
                    HandleRecoverTabGui(0, tabWidth, tabHeight);
                    tabControl.Refresh();
                    pictureCanvas.Size = new Size(screenRect.Width - 349, screenRect.Height - 75);
                    listView.Size = new Size(screenRect.Width - 349, screenRect.Height - 75);
                    informationTextBox.Location = new Point(8, screenRect.Height - 44);
                    informationTextBox.Size = new Size(screenRect.Width - 15, 30);
                    this.Update();
                    /* -------------------------- */
                    // loaded actor data is in actorClass
                    ActiveArtistsComboBox.Visible = false;
                    ArtistLabel.Visible = false;
                    #region ArtistIdentityEnterTextBox handling
                    string sPSN = actorClass.getUserSurName(0);
                    string sPMN = actorClass.getUserMidName(0);
                    string sPFN = actorClass.getUserFamName(0);
                    string sOutName = "";
                    if ((sPSN != null) && (sPSN != "") && (sPSN != " ") && (sOutName != ""))
                        sOutName += " " + sPSN;
                    else
                        sOutName = sPSN;
                    if ((sPMN != null) && (sPMN != "") && (sPMN != " ") && (sOutName != ""))
                        sOutName += " " + sPMN;
                    else
                        sOutName = sPMN;
                    if ((sPFN != null) && (sPFN != "") && (sPFN != " ") && (sOutName != ""))
                        sOutName += " " + sPFN;
                    else
                        sOutName = sPFN;
                    ArtistIdentityEnterTextBox.Text = sOutName;
                    ArtistIdentityEnterTextBox.Enabled = false;
                    ArtistIdentityEnterTextBox.Visible = true;
                    #endregion
                }
                else if (ViewAttenderAsActorButton.Text == "Add attender")
                {
                    // Add selected attender to Event
                    string sSelActorID = EventAttenderIDComboBox1.SelectedText;
                    int dp = sSelActorID.LastIndexOf("\\");
                    if ((dp > 0) && (dp < sSelActorID.Length))
                        sSelActorID = sSelActorID.Substring(dp + 1);
                    string sSetActortext = EventAttenderNaneTextBox.Text;
                    string sSetRoletext = "Unknown";
                    string sSetLeveltext = "Unknown";
                    dp = sSetActortext.IndexOf(",");
                    if ((dp > 0) && (dp < sSetActortext.Length))
                    {
                        sSetLeveltext = sSetActortext.Substring(0, dp);
                        sSetRoletext = sSetActortext.Substring(dp + 2);
                    }
                    eventClass.addEventAttender(sSelActorID, sSetLeveltext, sSetRoletext);
                    // Reset EventAttenderIDComboBox.
                    EventAttenderNaneTextBox.Text = "";
                    EventAttenderNaneTextBox.Enabled = false;
                    EventAttenderNaneTextBox.Visible = true;
                    EventAttenderIDComboBox1.Items.Clear();
                    EventAttenderIDComboBox1.Items.Add("Select...");
                    for (int i = 0; i < eventClass.getNoOfEventAttender(); i++)
                        EventAttenderIDComboBox1.Items.Add(eventClass.getEventAttenderID(i));
                    EventAttenderIDComboBox1.Items.Add("Add attender");
                    EventAttenderIDComboBox1.SelectedIndex = 0;
                    EventAttenderIDComboBox1.Enabled = true;
                    EventAttenderIDComboBox1.Visible = true;
                    // Reset EventAttenderAsActorButton to "" and inactive.
                    ViewAttenderAsActorButton.Text = "";
                    ViewAttenderAsActorButton.Enabled = false;
                    ViewAttenderAsActorButton.Visible = true;
                }
            }
        }
        private void EventImageLevelTextBox_TextChanged(object sender, EventArgs e)
        {
            ViewSelEventImageButton.Enabled = true;
            SaveActorDataChangesButton.Enabled = true;
            DiscardActorDataChangesButton.Enabled = true;
        }
        private void EventImageNameComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selAttdImg = EventImageNameComboBox.SelectedItem.ToString();
            if ((selAttdImg != "Select...") && (selAttdImg != "Add image..."))
            {
                ViewSelEventImageButton.Text = "View image";
                int selNr = EventImageNameComboBox.SelectedIndex;
                string selText = eventClass.getEventImagePathName(Math.Max(selNr - 1, 0)); //getEventImageName(Math.Max(selNr - 1, 0));
                if (File.Exists(selText))
                    ViewSelEventImageButton.Enabled = true;
                else
                    ViewSelEventImageButton.Enabled = false;
            }
            else if (selAttdImg == "Add image...")
            {
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
                    EventImageLevelTextBox.Text = ofd.FileName;
                }
                ofd.Dispose();
            }
        }
        private void ViewSelEventImageButton_Click(object sender, EventArgs e)
        {
            if (ViewSelEventImageButton.Text == "View image")
            {
                int selNr = Math.Max(EventImageNameComboBox.SelectedIndex, 0);
                loadedImageFileName = eventClass.getEventImagePathName(selNr - 1); //getEventImageName(selNr - 1);
                int dpp = loadedImageFileName.LastIndexOf("\\");
                linwin.setLastImageDirectoryValue(loadedImageFileName.Substring(0, dpp));
                picture = Image.FromFile(loadedImageFileName);
                if ((picture.Width > pictureCanvas.Width) || (picture.Height > pictureCanvas.Height))
                    pictureCanvas.SizeMode = PictureBoxSizeMode.Zoom;
                else
                    pictureCanvas.SizeMode = PictureBoxSizeMode.Normal;
                getImageMetadataValues(loadedImageFileName, sender, e);
                pictureCanvas.Image = picture;
                pictureName.Text = loadedImageFileName;
                expandedImage = true;
                ViewSelEventImageButton.Text = "Hide image";
                viewingImage = true;
            }
            else if (ViewSelEventImageButton.Text == "Hide image")
            {
                pictureCanvas.Image.Dispose();
                picture.Dispose();
                ViewSelEventImageButton.Text = "View image";
                viewingImage = false;
            }
            else if (ViewSelEventImageButton.Text == "Find image")
            {
                if (ViewSelEventImageButton.Text == "Find image")
                {
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
                        EventImageLevelTextBox.Text = ofd.FileName;
                    }
                    ofd.Dispose();
                }
                ViewSelEventImageButton.Text = "Add image";
            }
            else if (ViewSelEventImageButton.Text == "Add image")
            {
                // Format : <path>[, | ]<level>[, | ]<tag>[, | ]<description>
                string tempEvImPth = EventImageLevelTextBox.Text;
                char delimsign;
                int delimlength = 0;
                if (tempEvImPth.Contains(","))
                {
                    delimsign = ',';
                    delimlength = 2;
                }
                else
                {
                    delimsign = ' ';
                    delimlength = 1;
                }
                int dp = tempEvImPth.IndexOf(delimsign);
                if ((dp > 0) && (dp < tempEvImPth.Length - 1))
                {
                    string impa = tempEvImPth.Substring(0, dp);
                    tempEvImPth = tempEvImPth.Substring(dp + delimlength, tempEvImPth.Length - dp - delimlength);
                    // Format : <level>[, | ]<tag>[, | ]<description>
                    dp = tempEvImPth.IndexOf(delimsign);
                    if ((dp > 0) && (dp < tempEvImPth.Length - 1))
                    {
                        string imlvl = tempEvImPth.Substring(0, dp);
                        tempEvImPth = tempEvImPth.Substring(dp + delimlength, tempEvImPth.Length - dp - delimlength);
                        // Format : <tag>[, | ]<description>
                        dp = tempEvImPth.IndexOf(delimsign);
                        if ((dp > 0) && (dp < tempEvImPth.Length - 1))
                        {
                            string imtg = tempEvImPth.Substring(0, dp);
                            tempEvImPth = tempEvImPth.Substring(dp + delimlength, tempEvImPth.Length - dp - delimlength);
                            // Format : <description>
                            eventClass.addEventImage(impa, imlvl, imtg, tempEvImPth);
                        }
                        else
                            eventClass.addEventImage(impa, imlvl, tempEvImPth, "");
                    }
                    else
                        eventClass.addEventImage(impa, tempEvImPth, "", "");
                }
                else
                    eventClass.addEventImage(tempEvImPth, "", "", "");
                ViewSelEventImageButton.Enabled = false;
            }
        }
        private void EventDescriptionTextBox_TextChanged(object sender, EventArgs e)
        {
            SaveEventDataChangesButton.Enabled = true;
            DiscardEventDataChangesButton.Enabled = true;
        }
        private void EventRootComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            int iSelRoot = EventRootComboBox.SelectedIndex;
            if (((iSelRoot - 1) >= eventClass.getNoOfEventRoots()) &&
                (eventClass.getNoOfEventRoots() < eventClass.getMaxNoOfEventRoots()))
            {
                // "Add root..." selected.
                EventRootButton.Text = "Add root";
                EventRootButton.Enabled = true;
                EventRootButton.Visible = true;
            }
            else if ((iSelRoot - 1) >= 0)
            {
                // A actual root selected.
                // TODO - Implement that only images from this root shall be presented in the dropdown list.

                EventRootButton.Text = "Update";
                EventRootButton.Enabled = true;
                EventRootButton.Visible = true;
            }
        }
        private void EventRootButton_Click(object sender, EventArgs e)
        {
            if ((EventRootButton.Text == "Add root") && (eventClass != null))
            {
                folderBrowserDialog.RootFolder = Environment.SpecialFolder.MyComputer;
                DialogResult dres = folderBrowserDialog.ShowDialog();
                string sSelPath = folderBrowserDialog.SelectedPath.ToString();
                bool passed = eventClass.addEventRoot(eventClass.getNoOfEventRoots(), sSelPath);
                if (!passed)
                    setInformationText("Adding event root dir failed.", informationType.ERROR, sender, e);
                else
                {
                    string[] sEventRootFiles = System.IO.Directory.GetFiles(sSelPath);
                    foreach (var erf in sEventRootFiles)
                        eventClass.addEventImage(erf, eventClass.getEventLevel(), "Unknown", "Unknown");
                    EventImageNameComboBox.Items.Clear();
                    EventImageNameComboBox.Items.Add("Select...");
                    for (int i = 0; i < eventClass.getNoOfEventImages(); i++)
                        EventImageNameComboBox.Items.Add(eventClass.getEventImageName(i));
                    EventImageNameComboBox.Items.Add("Add image...");
                    EventImageNameComboBox.SelectedIndex = 0;
                    EventImageNameComboBox.Enabled = true;
                    EventImageNameComboBox.Visible = true;
                    //passed = eventClass.addEventRoot(eventClass.getNoOfEventRoots(), sSelPath);
                    EventRootComboBox.Items.Clear();
                    EventRootComboBox.Items.Add("Select...");
                    for (int i = 0; i < eventClass.getNoOfEventRoots(); i++)
                        EventRootComboBox.Items.Add(eventClass.getEventRoot(i));
                    EventRootComboBox.Items.Add("Add root");
                    EventRootComboBox.SelectedIndex = 0;
                    EventRootComboBox.Enabled = true;
                    EventRootComboBox.Visible = true;
                    EventRootButton.Text = "";
                    EventRootButton.Enabled = false;
                    EventRootButton.Visible = true;
                    SaveEventDataChangesButton.Enabled = true;
                    DiscardEventDataChangesButton.Enabled = true;
                }
            }
            else if (EventRootButton.Text == "Update")
            {
                int iSelItm = EventRootComboBox.SelectedIndex;
                string sSelRoot = eventClass.getEventRoot(iSelItm - 1);
                if (System.IO.Directory.Exists(sSelRoot))
                {
                    string[] eventRootFiles = System.IO.Directory.GetFiles(sSelRoot + "\\", "*.*");
                    foreach (var erf in eventRootFiles)
                        actorClass.addActorRelatedImage(erf, "Unknown", "Unknown");
                }
            }
        }
        private void SaveEventDataChangesButton_Click(object sender, EventArgs e)
        {
            if ((eventStartUpDone) && (eventClass != null))
            {
                if (SaveEventDataChangesButton.Text == "Save Data Changes")
                {
                    if (eventClass.setEventStarted(EventStartTextBox.Text))
                    {
                        SaveEventDataChangesButton.Enabled = true;
                        DiscardEventDataChangesButton.Enabled = true;
                    }
                    else
                        setInformationText("Faulty date format: expected format \"YY[YY][-MM[-DD]] H[H[H]]:mm [hours|minutes|seconds|time]\"", informationType.ERROR, sender, e);
                    if (eventClass.setEventEnded(EventEndTextBox.Text))
                    {
                        SaveEventDataChangesButton.Enabled = true;
                        DiscardEventDataChangesButton.Enabled = true;
                    }
                    else
                        setInformationText("Faulty date format: expected format \"YY[YY][-MM[-DD]] H[H[H]]:mm [hours|minutes|seconds|time]\"", informationType.ERROR, sender, e);
                    string evtdescr = EventDescriptionTextBox.Text;
                    eventClass.addEventContentDescription(evtdescr);
                    if (linwin.getEventStoragePath() != "")
                        eventClass.saveEvent(eventClass.getEventID(), linwin.getEventStoragePath());
                    else if (System.IO.Directory.Exists(rootPath + "\\EventData"))
                        eventClass.saveEvent(eventClass.getEventID(), rootPath + "\\EventData");
                    // Reset the presenttion
                    eventViewToolStripMenuItem_Click(sender, e);
                    SaveEventDataChangesButton.Text = "Edit data";
                    SaveEventDataChangesButton.Enabled = true;
                    SaveEventDataChangesButton.BackColor = Color.LightGray;
                }
                else if (SaveEventDataChangesButton.Text == "Edit data")
                {
                    activeEventComboBox.Visible = false;
                    ActiveEventTextBox.Visible = true;
                    ActiveEventTextBox.Enabled = true;
                    CopyrightTextBox.Enabled = true;
                    EventSecrecyLevelTextBox.Enabled = true;
                    EventStartTextBox.Enabled = true;
                    EventEndTextBox.Enabled = true;
                    EventHeadlineTextBox.Enabled = true;
                    EventLatitudeTextBox.Enabled = true;
                    EventLongitudeTextBox.Enabled = true;
                    EventGeographNameTextBox.Enabled = true;
                    EventStreetnameNumberTextBox.Enabled = true;
                    EventZipCodeTextBox.Enabled = true;
                    EventAreanameTextBox.Enabled = true;
                    EventCitynameTextBox.Enabled = true;
                    EventCountrynameTextBox.Enabled = true;
                    EventAttenderIDComboBox1.SelectedIndex = 0;
                    EventAttenderIDComboBox1.Visible = true;
                    EventAttenderIDComboBox1.Enabled = false;
                    EventAttenderNaneTextBox.Enabled = true;
                    ViewAttenderAsActorButton.Text = "Find attender";
                    ViewAttenderAsActorButton.Visible = true;
                    ViewAttenderAsActorButton.Enabled = false;
                    EventImageNameComboBox.SelectedIndex = 0;
                    EventImageNameComboBox.Visible = true;
                    EventImageNameComboBox.Enabled = false;
                    ViewSelEventImageButton.Text = "Find image";
                    ViewSelEventImageButton.Visible = true;
                    ViewSelEventImageButton.Enabled = true;
                    SaveEventDataChangesButton.Text = "Save data";
                    SaveEventDataChangesButton.Visible = true;
                    SaveEventDataChangesButton.Enabled = false;
                    DiscardEventDataChangesButton.Visible = true;
                    DiscardEventDataChangesButton.Enabled = false;
                    SaveEventDataChangesButton.BackColor = Color.LightGray;
                }
            }
        }
        private void DiscardEventDataChangesButton_Click(object sender, EventArgs e)
        {
            // TODO - Handle discarding event data changes.
        }
        #endregion
        #region Help Handling
        // --- Help handling ---
        private void folderBrowserDialog_HelpRequest(object sender, EventArgs e)
        {
            // TODO - Handle help request.
        }
        #endregion
        #region Sorting Handling
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
                        bool foundFileAsDir = false;
                        foreach (var exiDir in directoryArray)
                        {
                            if (exiFile.ToString() == exiDir.ToString())
                            {
                                foundFileAsDir = true;
                            }
                        }
                        string longHandledFile = exiFile.ToString();
                        int dp = longHandledFile.LastIndexOf("\\");
                        string shortHandledFile = longHandledFile.Substring(dp + 1, longHandledFile.Length - 1 - dp);
                        if (!(foundFileAsDir))
                        {
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
                        else
                        {
                            imageList.Images.Add(Image.FromFile(rootPath + "\\Resources\\Folder.jpg"));
                            imageList.Images.SetKeyName(nr, longHandledFile);
                        }
                    }
                }
                listView.View = View.LargeIcon;
                imageList.ImageSize = new Size(Math.Max(linwin.getSmallImageWidth(), 32), Math.Max(linwin.getSmallImageHeight(), 32));
                listView.LargeImageList = imageList;
                listView.Visible = true;
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
                    pictureCanvas.SizeMode = PictureBoxSizeMode.Normal;//.Zoom;
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
        private void pictureCanvas_MouseEnter(object sender, EventArgs e)
        {
            pictureCanvas.Focus();
        }
        private void pictureCanvas_MouseHover(object sender, EventArgs e)
        {
            tt.Show("Picture canvas where images are displayed.", (Control)sender);
//            setInformationText("Picture canvas where images are displayed.", informationType.INFO, sender, e);
        }
        private void pictureCanvas_Click(object sender, EventArgs e)
        {
            setInformationText("Clicked on the canvas area", informationType.INFO, sender, e);
            if (imageZoomed)
            {
                pictureCanvas.Image = picture;
                imageZoomed = false;
            }
            else
            {
                if (expandedImage)
                {
                    pictureCanvas.Visible = false;
                    pictureCanvas.Enabled = true;
                    listView.Visible = false;
                    RetToListBtn.Enabled = false;
                    RetToListBtn.Visible = false;
                    expandedImage = false;
                }
//                else if (this.currentMode == programMode.RESTOREVIEW)
//                {
//                    if (expandedImage)
//                    {
//                        pictureCanvas.Visible = false;
//                        listView.Visible = true;
//                        RetToListBtn.Enabled = false;
//                        RetToListBtn.Visible = false;
//                        expandedImage = false;
//                    }
//                }
                else
                {
                    pictureCanvas.Visible = true;
                    listView.Visible = false;
                    RetToListBtn.Enabled = true;
                    RetToListBtn.Visible = true;
                    expandedImage = true;
                }
            }
        }
        private void listView_SelectedIndexChanged(object sender, EventArgs e)
        {
            // TODO - fix this
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
        private bool moveFileToDir(string buttonText, object sender, EventArgs e)
        {
            setInformationText("NOT COMPLETED FUNCTION!", informationType.INFO, sender, e);
            bool retVal = false;
            string targetName;
            this.UseWaitCursor = true;
            object selViewItem = listView.FocusedItem.Clone();
            string selectedFile = selViewItem.ToString();//.Text;
            int mfDP = selectedFile.LastIndexOf("\\");
            string shortSelFile = selectedFile.Substring(mfDP);

            listView.FocusedItem.Remove();
            // TODO - Truly remove all claims on the file in question.
            if (buttonText != "..")
                targetName = selectedFile.Substring(0, mfDP + 1) + buttonText + shortSelFile;
            else
            {
                string sFilename = selectedFile.Substring(mfDP, selectedFile.Length - mfDP);
                targetName = selectedFile.Substring(0, mfDP);
                mfDP = targetName.LastIndexOf("\\");
                targetName = targetName.Substring(0, mfDP) + sFilename;
            }
            try
            {
                if (!(File.Exists(targetName)))
                    File.Move(selectedFile, targetName);
                else
                    setInformationText("Targetfile exists!", informationType.ERROR, sender, e);
            }
            catch (Exception err)
            {
                //setInformationText("Moving the file did not work.", informationType.ERROR, sender, e);
                setInformationText(err.ToString(), informationType.ERROR, sender, e);
            }
            // TODO - Regenerate listview contents.
            if (expandedImage)
            {
                pictureCanvas.Visible = false;
                listView.Visible = true;
                RetToListBtn.Enabled = false;
                RetToListBtn.Visible = false;
                expandedImage = false;
            }

            this.UseWaitCursor = false;
            fixImageDisplayAndSortingButtons(baseFolderName, sender, e);
            return retVal;
            /* -----------------------------------------
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
            --------------------------------------------------- */
        }
        private void sortingButton1_Click(object sender, EventArgs e)
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
        private void sortingButton2_Click(object sender, EventArgs e)
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
        private void sortingButton3_Click(object sender, EventArgs e)
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
        private void sortingButton4_Click(object sender, EventArgs e)
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
        private void sortingButton5_Click(object sender, EventArgs e)
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
        private void sortingButton6_Click(object sender, EventArgs e)
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
        private void sortingButton7_Click(object sender, EventArgs e)
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
        private void sortingButton8_Click(object sender, EventArgs e)
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
        private void sortingButton9_Click(object sender, EventArgs e)
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
        private void sortingButton10_Click(object sender, EventArgs e)
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
        private void sortingButton11_Click(object sender, EventArgs e)
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
        private void sortingButton12_Click(object sender, EventArgs e)
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
        #endregion
        #region User Information Handling
        // --- User information handling ---
        private void rootDirLbl_Click(object sender, EventArgs e)
        {
            // Do nothing.
        }
        private void Login_Win_Open()
        {
            // TODO - Kan göras om till att ta en boolean som markerar expanded eller inte.
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
        #endregion
    }
}
