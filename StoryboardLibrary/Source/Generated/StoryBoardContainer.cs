//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by xsd, Version=4.6.1055.0.
// 
namespace StoryboardContainer
{
    using System.Xml.Serialization;


    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class Task
    {

        private string titleField;

        private string shortNameField;

        private string dateField;

        private string storyboardToolVersionField;

        private string cinematicLevelField;

        private CommonData taskDataField;

        private int timeOfDayField;

        private bool timeOfDayFieldSpecified;

        private Transition[] transitionsField;

        private CueBase[] gameCuesField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Title
        {
            get
            {
                return this.titleField;
            }
            set
            {
                this.titleField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string ShortName
        {
            get
            {
                return this.shortNameField;
            }
            set
            {
                this.shortNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Date
        {
            get
            {
                return this.dateField;
            }
            set
            {
                this.dateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string StoryboardToolVersion
        {
            get
            {
                return this.storyboardToolVersionField;
            }
            set
            {
                this.storyboardToolVersionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string CinematicLevel
        {
            get
            {
                return this.cinematicLevelField;
            }
            set
            {
                this.cinematicLevelField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public CommonData TaskData
        {
            get
            {
                return this.taskDataField;
            }
            set
            {
                this.taskDataField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public int TimeOfDay
        {
            get
            {
                return this.timeOfDayField;
            }
            set
            {
                this.timeOfDayField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool TimeOfDaySpecified
        {
            get
            {
                return this.timeOfDayFieldSpecified;
            }
            set
            {
                this.timeOfDayFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlArrayItemAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = false)]
        public Transition[] Transitions
        {
            get
            {
                return this.transitionsField;
            }
            set
            {
                this.transitionsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [System.Xml.Serialization.XmlArrayItemAttribute("GameCue", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = false)]
        public CueBase[] GameCues
        {
            get
            {
                return this.gameCuesField;
            }
            set
            {
                this.gameCuesField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class CommonData
    {

        private string textField;

        private Step[] stepField;

        private string objectNameField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Text
        {
            get
            {
                return this.textField;
            }
            set
            {
                this.textField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Step", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public Step[] Step
        {
            get
            {
                return this.stepField;
            }
            set
            {
                this.stepField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string ObjectName
        {
            get
            {
                return this.objectNameField;
            }
            set
            {
                this.objectNameField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class Step
    {

        private CommonData stepDataField;

        private string idField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public CommonData StepData
        {
            get
            {
                return this.stepDataField;
            }
            set
            {
                this.stepDataField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "ID")]
        public string Id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(HighlightObjectsCue))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DelayCue))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(SnapToCameraCue))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(PlayAudioCue))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(SmartObjectCue))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(UnknownCue))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class CueBase
    {

        private string idField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "ID")]
        public string Id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class HighlightObjectsCue : CueBase
    {

        private string[] gameObjectsField;

        private string[] highlightField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("GameObjects", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string[] GameObjects
        {
            get
            {
                return this.gameObjectsField;
            }
            set
            {
                this.gameObjectsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Highlight", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string[] Highlight
        {
            get
            {
                return this.highlightField;
            }
            set
            {
                this.highlightField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class DelayCue : CueBase
    {

        private float delayField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public float Delay
        {
            get
            {
                return this.delayField;
            }
            set
            {
                this.delayField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class SnapToCameraCue : CueBase
    {

        private string[] cameraNameField;

        private float[] delayField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("CameraName", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string[] CameraName
        {
            get
            {
                return this.cameraNameField;
            }
            set
            {
                this.cameraNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public float[] Delay
        {
            get
            {
                return this.delayField;
            }
            set
            {
                this.delayField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class PlayAudioCue : CueBase
    {

        private string audioClipField;

        private string captionField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string AudioClip
        {
            get
            {
                return this.audioClipField;
            }
            set
            {
                this.audioClipField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Caption
        {
            get
            {
                return this.captionField;
            }
            set
            {
                this.captionField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class SmartObjectCue : CueBase
    {

        private string objectNameField;

        private string highlightField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string ObjectName
        {
            get
            {
                return this.objectNameField;
            }
            set
            {
                this.objectNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Highlight
        {
            get
            {
                return this.highlightField;
            }
            set
            {
                this.highlightField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class UnknownCue : CueBase
    {

        private string descriptionField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.6.1055.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class Transition
    {

        private string fromStepField;

        private string toStepField;

        private bool conditionField;

        private bool conditionFieldSpecified;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, DataType = "IDREF")]
        public string FromStep
        {
            get
            {
                return this.fromStepField;
            }
            set
            {
                this.fromStepField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, DataType = "IDREF")]
        public string ToStep
        {
            get
            {
                return this.toStepField;
            }
            set
            {
                this.toStepField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public bool Condition
        {
            get
            {
                return this.conditionField;
            }
            set
            {
                this.conditionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ConditionSpecified
        {
            get
            {
                return this.conditionFieldSpecified;
            }
            set
            {
                this.conditionFieldSpecified = value;
            }
        }
    }
}
