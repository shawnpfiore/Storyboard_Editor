#region File Header
//------------------------------------------------------------------------------------------
//
//  File Name:         	ParseStoryboardSchema.cs
//  Author:            	Shawn Fiore
//  Creation Date:  	January 17, 2019
//  Last Update:    	January 17, 2019
//                    	Shawn Fiore
//
//  Copyrights:        	Copyright 2018
//                     	Engineering and Computer Simulations, Inc.
//                     	All Rights Reserved.
//
//------------------------------------------------------------------------------------------
#endregion
using ECS.Utilites.Logging;
using StoryboardConfig;
using System;
using System.Collections.ObjectModel;
using System.Xml;
using System.Xml.Schema;

namespace StoryboardUtils
{
    public static class ParseStoryboardSchema
    {
        #region Fields/Variables
        /// <summary>
        /// Stores the storyboard configuration file
        /// </summary>
        private static StoryboardConfiguration storyboardConfig;

        /// <summary>
        /// Stores the xmltextreader for the .xsd file
        /// </summary>
        private static XmlTextReader xtr = null;

        /// <summary>
        /// Stores the storyboard xmlschema
        /// </summary>
        private static XmlSchema schema = null;

        /// <summary>
        /// Stores the schema set
        /// </summary>
        private static XmlSchemaSet schemaSet = null;

        /// <summary>
        /// Stores the compiled schema
        /// </summary>
        private static XmlSchema compiledSchema = null;
        #endregion

        #region Methods
        #region Public Methods
        /// <summary>
        /// Gets the Available game cues from the storyboard schema file
        /// </summary>
        /// <returns>The storyboard cue names</returns>
        public static ObservableCollection<string> GetAvailableCues()
        {
            var availableCues = new ObservableCollection<string>();

            if (LoadStoryboardConfigs.StoryboardConfigurationFile != null)
            {
                storyboardConfig = LoadStoryboardConfigs.StoryboardConfigurationFile;
            }

            // check the config first for the location of the master schema
            var masterSchemaFile = storyboardConfig.MasterSchemaFile; 

            if(string.IsNullOrEmpty(masterSchemaFile))
            {
                masterSchemaFile = @".\data\xsd\StoryBoardContainer.xsd";
            }

            try
            {
                xtr = new XmlTextReader(masterSchemaFile);
                schema = XmlSchema.Read(xtr, new ValidationEventHandler(ValidationCallbackOne));
                schemaSet = new XmlSchemaSet();

                schemaSet.ValidationEventHandler += new ValidationEventHandler(ValidationCallbackOne);
                schemaSet.Add(schema);
                schemaSet.Compile();

                foreach (XmlSchema schema1 in schemaSet.Schemas())
                {
                    compiledSchema = schema1;
                }

                foreach (XmlSchemaObject schemaObject in compiledSchema?.Items)
                {
                    if (schemaObject?.GetType() == typeof(XmlSchemaComplexType))
                    {
                        XmlSchemaComplexType complexType = schemaObject as XmlSchemaComplexType;

                        if (complexType?.ContentModel?.GetType() == typeof(XmlSchemaComplexContent))
                        {
                            XmlSchemaComplexContent complextContent = complexType.ContentModel as XmlSchemaComplexContent;

                            if (complextContent?.Content?.GetType() == typeof(XmlSchemaComplexContentExtension))
                            {
                                XmlSchemaComplexContentExtension complexExtension = complextContent?.Content as XmlSchemaComplexContentExtension;
                                if (complexExtension?.BaseTypeName?.Name == "CueBase")
                                    availableCues?.Add(complexType?.Name);
                            }
                        }
                    }
                }
            }
            catch(Exception e)
            {
                ECSLogger.Instance.LogFatal($"{e.Message}");
            }

            xtr.Close();

            return availableCues;
        }
        #endregion

        #region Non Public Methods
        /// <summary>
        /// XmlSchema reader call back 
        /// </summary>
        /// <param name="sender">The sending object</param>
        /// <param name="args">The event args</param>
        private static void ValidationCallbackOne(object sender, ValidationEventArgs args)
        {
            if (args.Severity == XmlSeverityType.Warning)
                ECSLogger.Instance.LogWarn("WARNING: ");
            else if (args.Severity == XmlSeverityType.Error)
                ECSLogger.Instance.LogError("ERROR: ");

            ECSLogger.Instance.LogInfo(args.Message);
        }
        #endregion
        #endregion
    }
}
