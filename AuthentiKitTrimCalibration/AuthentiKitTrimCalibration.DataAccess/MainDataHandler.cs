﻿using MappingManager.Common.DataProvider;
using MappingManager.Common.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Xml;
using static MappingManager.Common.Model.OutputAxis;

namespace AuthentiKitTrimCalibration.DataAccess
{
    public class MainDataHandler : IMainDataHandler
    {
        private readonly string FILENAME = "settings.xml";
        private readonly string CONFIG = "CONFIG";
        private readonly string GROUP = "GROUP";
        private readonly string MAPPING = "MAPPING";
        private readonly string NAME = "NAME";
        private readonly string TYPE_ID = "TYPE_ID";
        private readonly string ACTIVE = "ACTIVE";
        private readonly string INPUT_CHANNEL_A_HASH = "INPUT_CHANNEL_A_HASH";
        private readonly string INPUT_CHANNEL_B_HASH = "INPUT_CHANNEL_B_HASH";
        private readonly string OUTPUT_CHANNEL_HASH = "OUTPUT_CHANNEL_HASH";
        private readonly string AXIS_SENSITIVITY = "AXIS_SENSITIVITY";
        private readonly string ENCODER_PPR = "ENCODER_PPR";
        private readonly string REVS_IN_PER_REVS_OUT = "REVS_IN_PER_REVS_OUT";
        private readonly string BUTTON_MULTIPLIER = "BUTTON_MULTIPLIER";
        private readonly string RESET_COMMAND = "RESET_COMMAND";

        public MappingDTO GetBlankMapping()
        {
            return new MappingDTO { Name = "New Mapping" };
        }

        public IEnumerable<MappingDTO> LoadMappings(ObservableCollection<InputChannel> inputChannelsA, ObservableCollection<InputChannel> inputChannelsB, ObservableCollection<OutputChannel> outputAxes, ObservableCollection<OutputChannel> outputButtons)
        {
            // Open the XML Document
            XmlDocument settingsFile = new();
            settingsFile.Load(FILENAME);

            // Parse values from XML
            XmlNode config = settingsFile.SelectSingleNode(CONFIG);
            List<MappingDTO> mappings = new();
            try
            {
                // Loop through each group
                foreach (XmlNode groupNode in config.SelectNodes(GROUP))
                {
                    var groupName = groupNode.Attributes[NAME];
                    if (groupName != null)
                    {
                        Debug.WriteLine("Reading Group: " + groupName.Value);
                    }

                    // Loop through each mapping
                    foreach (XmlNode mappingNode in groupNode.SelectNodes(MAPPING))
                    {
                        // Parse values from XML
                        string name = mappingNode.SelectSingleNode(NAME).InnerText;
                        int typeId = int.Parse(mappingNode.SelectSingleNode(TYPE_ID).InnerText);
                        bool active = bool.Parse(mappingNode.SelectSingleNode(ACTIVE).InnerText);
                        int inputChannelAHash = int.Parse(mappingNode.SelectSingleNode(INPUT_CHANNEL_A_HASH).InnerText);
                        int inputChannelBHash = int.Parse(mappingNode.SelectSingleNode(INPUT_CHANNEL_B_HASH).InnerText);
                        int outputChannelHash = int.Parse(mappingNode.SelectSingleNode(OUTPUT_CHANNEL_HASH).InnerText);
                        int axisSensitivity = int.Parse(mappingNode.SelectSingleNode(AXIS_SENSITIVITY).InnerText);
                        int encoderPPR = int.Parse(mappingNode.SelectSingleNode(ENCODER_PPR).InnerText);
                        float revsInPerRevsOut = float.Parse(mappingNode.SelectSingleNode(REVS_IN_PER_REVS_OUT).InnerText);
                        int buttonMultiplier = int.Parse(mappingNode.SelectSingleNode(BUTTON_MULTIPLIER).InnerText);
                        string resetCommand = mappingNode.SelectSingleNode(RESET_COMMAND).InnerText;

                        // Create Mapping from values and add to mappings list
                        mappings.Add(new MappingDTO
                        {
                            Name = name,
                            TypeId = typeId,
                            Active = active,
                            InputChannelA = GetInputChannel(inputChannelAHash, inputChannelsA),
                            InputChannelB = GetInputChannel(inputChannelBHash, inputChannelsB),
                            OutputChannel = GetOutputChannel(outputChannelHash, outputAxes, outputButtons),
                            AxisSensitivity = axisSensitivity,
                            EncoderPPR = encoderPPR,
                            RevsInPerRevsOut = revsInPerRevsOut,
                            ButtonMultiplier = buttonMultiplier,
                            ResetCommand = resetCommand
                        });
                    }
                }
            }
            catch
            {
                throw new Exception("Error finding or reading settings.xml");
            }
            return mappings;
        }

        public void SaveMappings(IEnumerable<MappingDTO> mappings)
        {
            XmlDocument doc = new();
            XmlElement configNode = doc.CreateElement(CONFIG);
            XmlElement groupNode = doc.CreateElement(GROUP);

            foreach (var mapping in mappings)
            {
                XmlElement mappingNode = doc.CreateElement(MAPPING);

                // Name
                XmlElement nameNode = doc.CreateElement(NAME);
                nameNode.InnerText = mapping.Name;
                mappingNode.AppendChild(nameNode);

                // TypeId
                XmlElement typeIdNode = doc.CreateElement(TYPE_ID);
                typeIdNode.InnerText = String.Format("{0}", mapping.TypeId);
                mappingNode.AppendChild(typeIdNode);

                // Active
                XmlElement activeNode = doc.CreateElement(ACTIVE);
                activeNode.InnerText = String.Format("{0}", mapping.Active);
                mappingNode.AppendChild(activeNode);

                // InputChannelA
                XmlElement inputChannelANode = doc.CreateElement(INPUT_CHANNEL_A_HASH);
                inputChannelANode.InnerText = String.Format("{0}", mapping.InputChannelA.Hash);
                mappingNode.AppendChild(inputChannelANode);

                // InputChannelB
                XmlElement inputChannelBNode = doc.CreateElement(INPUT_CHANNEL_B_HASH);
                inputChannelBNode.InnerText = String.Format("{0}", mapping.InputChannelB.Hash);
                mappingNode.AppendChild(inputChannelBNode);

                // OutputChannel
                XmlElement outputChannelNode = doc.CreateElement(OUTPUT_CHANNEL_HASH);
                outputChannelNode.InnerText = String.Format("{0}", mapping.OutputChannel.Hash);
                mappingNode.AppendChild(outputChannelNode);

                // AxisSensitivity
                XmlElement axisSensitivityNode = doc.CreateElement(AXIS_SENSITIVITY);
                axisSensitivityNode.InnerText = String.Format("{0}", mapping.AxisSensitivity);
                mappingNode.AppendChild(axisSensitivityNode);

                // encoderPPRNode
                XmlElement encoderPPRNode = doc.CreateElement(ENCODER_PPR);
                encoderPPRNode.InnerText = String.Format("{0}", mapping.EncoderPPR);
                mappingNode.AppendChild(encoderPPRNode);

                // revsInPerRevsOut
                XmlElement revsInPerRevsOut = doc.CreateElement(REVS_IN_PER_REVS_OUT);
                revsInPerRevsOut.InnerText = String.Format("{0}", mapping.RevsInPerRevsOut);
                mappingNode.AppendChild(revsInPerRevsOut);

                // ButtonMultiplier
                XmlElement buttoMultiplierNode = doc.CreateElement(BUTTON_MULTIPLIER);
                buttoMultiplierNode.InnerText = String.Format("{0}", mapping.ButtonMultiplier);
                mappingNode.AppendChild(buttoMultiplierNode);

                // ResetCommand
                XmlElement resetCommandNode = doc.CreateElement(RESET_COMMAND);
                resetCommandNode.InnerText = mapping.ResetCommand;
                mappingNode.AppendChild(resetCommandNode);

                // Add to group
                groupNode.AppendChild(mappingNode);
            }

            configNode.AppendChild(groupNode);
            doc.AppendChild(configNode);
            doc.Save(FILENAME);
        }
        private static InputChannel GetInputChannel(int hash, ObservableCollection<InputChannel> inputChannels)
        {
            foreach (var channel in inputChannels)
            {
                if (channel.Hash == hash)
                {
                    return channel;
                }
            }
            return new InputChannel();
        }
        private static OutputChannel GetOutputChannel(int hash, ObservableCollection<OutputChannel> outputAxes, ObservableCollection<OutputChannel> outputButtons)
        {
            foreach (var channel in outputAxes)
            {
                if (channel.Hash == hash)
                {
                    return channel;
                }
            }
            foreach (var channel in outputButtons)
            {
                if (channel.Hash == hash)
                {
                    return channel;
                }
            }
            return new OutputChannel();
        }

        /// Returns an AuthentiKit or BU0836 input channel for the specified button, if available
        private InputChannel getAuthentiKitInputChannel(ObservableCollection<InputChannel> channels, int button)
        {
            foreach (var channel in channels)
                if ((channel.Button == button) && (channel.Device.Contains("AuthentiKit")))
                    return channel;

            foreach (var channel in channels)
                if ((channel.Button == button) && (channel.Device.Contains("BU0836")))
                    return channel;

            return new InputChannel();
        }

        /// Returns the specified vJoy output if available
        private OutputChannel getOutputChannel(ObservableCollection<OutputChannel> channels, uint vJoyItem)
        {
            foreach (var channel in channels)
                if (channel.VJoyItem == vJoyItem)
                    return channel;
            return new OutputAxis();
        }
        public IEnumerable<MappingDTO> GetDefaultMappings(Aircraft aircraft, ObservableCollection<InputChannel> inputChannelsA, ObservableCollection<InputChannel> inputChannelsB, ObservableCollection<OutputChannel> outputAxes, ObservableCollection<OutputChannel> outputButtons)
        {
            ObservableCollection<MappingDTO> mappings = new();
            if (aircraft == Aircraft.SPITFIRE_MKIX)
            {
                // Elevator Trim Axis
                mappings.Add(new MappingDTO
                {
                    Name = "Elevator Trim",
                    TypeId = MappingType.AXIS,
                    InputChannelA = getAuthentiKitInputChannel(inputChannelsA, 10),
                    InputChannelB = getAuthentiKitInputChannel(inputChannelsB, 11),
                    OutputChannel = getOutputChannel(outputAxes, (uint)AxisId.X),
                    AxisSensitivity = 347,
                });

                // Rudder Trim Button Left
                mappings.Add(new MappingDTO
                {
                    Name = "Rudder Trim Left",
                    TypeId = MappingType.BUTTON,
                    InputChannelA = getAuthentiKitInputChannel(inputChannelsA, 9),
                    OutputChannel = getOutputChannel(outputButtons, 1),
                    ButtonMultiplier = 7
                });

                // Rudder Trim Button Right
                mappings.Add(new MappingDTO
                {
                    Name = "Rudder Trim Right",
                    TypeId = MappingType.BUTTON,
                    InputChannelA = getAuthentiKitInputChannel(inputChannelsA, 8),
                    OutputChannel = getOutputChannel(outputButtons, 2),
                    ButtonMultiplier = 7
                });

                /* // Experimental Encoder based Elevator Trim
                 mappings.Add(new MappingDTO
                 {
                     Name = "EXPERIMENTAL: Elevator Trim (Axis)",
                     TypeId = MappingType.ENCODER_AXIS,
                     InputChannelA = getAuthentiKitInputChannel(inputChannelsA, 10),
                     InputChannelB = getAuthentiKitInputChannel(inputChannelsB, 11),
                     OutputChannel = getOutputChannel(outputAxes, (uint)AxisId.X),
                     EncoderPPR = 24,
                     RevsInPerRevsOut = 4
                 });*/
            }
            return mappings;
        }
    }
}
