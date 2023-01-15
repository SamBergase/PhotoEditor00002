using System;
using System.IO;

namespace ns_actorData
{
    #region declaration section two
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
        public struct ActorBirthValues
        {
            DateTime BirthDate;
            uint securityNumber;
            addressData birthPlace;
            genderTypes birthGender;
        }
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
        public struct ActorContactValues
        {
            contactType actorContactType;
            string actorContactPath;
        }
        public struct ActorLengthValues
        {
            float actorLength;
            distanceUnits usedUnits;
            DateTime validDate;
        }
        public struct ActorWeightValues
        {
            float actorWeight;
            weightUnits usedUnits;
            DateTime validDate;
        }
        public struct ActorChestValues
        {
            float actorChestCircumference;
            distanceUnits usedUnits;
            breastSize actorBreastSize;
            breastType actorBreastType;
            DateTime validDate;
        }
        public struct ActorWaistValues
        {
            float actorWaistCircumference;
            distanceUnits usedUnits;
            DateTime validDate;
        }
        public struct ActorHipValues
        {
            float actorHipCircumference;
            distanceUnits usedUnits;
            DateTime validDate;
        }
        public struct ActorComplexionValues
        {
            etnicityType actorEtnicity;
            uint red;
            uint green;
            uint blue;
            DateTime validDate;
        }
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
        public struct ActorResicenceValues
        {
            addressData actorResidenceAddress;
            float actorResidenceBuyValue;
            float actorResidenceSaleValue;
            currencyUnits usedUnits;
            DateTime actorResidenceMovedIn;
            DateTime actorResidenceMovedOut;
        }
        public struct ActorHairValues
        {
            hairColorType actorHairColor;
            hairTextureType actorHairTexture;
            hairLengthType actorHairLength;
            DateTime validDate;
        }
        public struct ActorMarkingValues
        {
            markingType actorMarkingType;
            string actorMarkingPosition;
            string actorMarkingMotif;
            DateTime validDate;
        }
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
        public struct ActorRelatedEventValues
        {
            string actorRelatedEventId;
        }
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
        public struct ActorRelatedImagesValues
        {
            string actorRelatedImagesPathName;
            imageType actorRelatedImageType;                            // Collect from image?
            string actorRelatedImageDescription;                        // Collect from image?
            classificationLevel actorRelatedImageClassificationLevel;   // Collect from image?
            DateTime actorRelatedImageExposureDateTime;                 // Collect from image?
        }
        public struct ActorBodyTypeValues
        {
            bodyType actorBodyType;
            DateTime validDate;
        }
    #endregion
    public class actorName
    {
        public nameType nTyp { get; set; }
        public string surn { get; set; }
        string middn { get; set; }
        string famn { get; set; }

        public actorName()//nameType type, string sn, string mn, string fn)
        {
            nTyp = nameType.UndefNameType;
            surn = "";
            middn = "";
            famn = "";
        }
        public actorName(nameType ntp, string snm, string mnm, string fnm)
        {
            nTyp = ntp;
            surn = snm;
            middn = mnm;
            famn = fnm;
        }
    }
    #region actorDeclarations
        public struct ActorDataValues
        {
            public string uId;

            public ActorBirthValues actorDataBirthValues;
            public relationType arrActorDataCurrentRelation;

            public string actorDataDescription;
            public string arrActorDataNationality;

            public eyeColorType actorDataEyeColor;

            public uint actorDataNoOfBodyTypes;
            public bodyType[] arrActorDataBodyTypeArr;

            public uint actorDataNoOfGenderInfo;
            public ActorGenderValues[] arrActorDataGenderInfo;

            public uint actorDataNoOfContactInfo;
            public ActorContactValues[] arrActorDataContactInfo;

            public uint actorDataNoOfLengthInfo;
            public ActorLengthValues[] arrActorDataLengthInfo;

            public uint actorDataNoOfWeightInfo;
            public ActorWeightValues[] arrActorDataWeightInfo;

            public uint actorDataNoOfChestInfo;
            public ActorChestValues[] arrActorDataChestInfo;

            public uint actorDataNoOfWaistInfo;
            public ActorWaistValues[] arrActorDataWaistInfo;

            public uint actorDataNoOfHipInfo;
            public ActorHipValues[] arrActorDataHipInfo;

            public uint actorDataNoOfComplexionInfo;
            public ActorComplexionValues[] arrActorDataComplexionInfo;

            public uint actorDataNoOfFaceFormInfo;
            public ActorFaceformValues[] arrActorDataFaceFormInfo;

            public uint actorDataNoOfResidenceInfo;
            public ActorResicenceValues[] arrActorDataResidenceInfo;

            public uint actorDataNoOfHairInfo;
            public ActorHairValues[] arrActorDataHairInfo;

            public uint actorDataNoOfMarkingInfo;
            public ActorMarkingValues[] arrActorDataMarkingInfo;

            public uint actorDataNoOfOccupationInfo;
            public ActorOccupationValues[] arrActorDataOccupationInfo;

            public uint actorDataNoOfRelatedEventsInfo;
            public ActorRelatedEventValues[] arrActorDataRelatedEventsInfo;

            public uint actorDataNoOfRelatedImagesInfo;
            public ActorRelatedImagesValues[] arrActorDataRelatedImagesInfo;

            public uint actorDataNoOfRelationsInfo;
            public ActorRelationValues[] arrActorDataRelationsInfo;

            public bool actorDataSet;
        }
    #endregion
    public class actor_data
    {
        ActorDataValues data = new ActorDataValues();

        public actor_data()
        {
            data.uId = "";
//            data.arrActorDataName[0].
        }
    }
}