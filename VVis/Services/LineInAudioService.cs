using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Media.Audio;
using Windows.Media.Capture;
using Windows.Media.Devices;
using Windows.Media.MediaProperties;
using Windows.Media.Render;
using Windows.Media.Transcoding;
using ReactiveUI;
using Windows.Storage;

namespace VVis.Services
{
    /// <summary>
    /// https://github.com/Microsoft/Windows-universal-samples/blob/master/Samples/AudioCreation/cs/AudioCreation/Scenario2_DeviceCapture.xaml.cs
    /// </summary>
    public class LineInAudioService : ReactiveObject
    {
        private AudioGraph graph;
        private AudioFileOutputNode fileOutputNode;
        private AudioDeviceOutputNode deviceOutputNode;
        private AudioDeviceInputNode deviceInputNode;
        private DeviceInformationCollection outputDevices;

        public ReactiveList<DeviceInformation> Devices = new ReactiveList<DeviceInformation>();

        private DeviceInformation _selectedDevice;
        public DeviceInformation SelectedDevice
        {
            get { return _selectedDevice; }
            set { this.RaiseAndSetIfChanged(ref _selectedDevice, value); }
        }

        private async Task PopulateDeviceList()
        {
            Devices.Clear();
            outputDevices = await DeviceInformation.FindAllAsync(MediaDevice.GetAudioRenderSelector());
            foreach (var device in outputDevices)
            {
                Devices.Add(device);
            }
        }

        private async Task CreateAudioGraph()
        {
            AudioGraphSettings settings = new AudioGraphSettings(AudioRenderCategory.Media);
            settings.QuantumSizeSelectionMode = QuantumSizeSelectionMode.LowestLatency;
            settings.PrimaryRenderDevice = SelectedDevice;

            CreateAudioGraphResult result = await AudioGraph.CreateAsync(settings);

            if (result.Status != AudioGraphCreationStatus.Success)
            {
                // Cannot create graph
                //rootPage.NotifyUser(String.Format("AudioGraph Creation Error because {0}", result.Status.ToString()), NotifyType.ErrorMessage);
                return;
            }

            graph = result.Graph;
            //rootPage.NotifyUser("Graph successfully created!", NotifyType.StatusMessage);

            // Create a device output node
            CreateAudioDeviceOutputNodeResult deviceOutputNodeResult = await graph.CreateDeviceOutputNodeAsync();
            if (deviceOutputNodeResult.Status != AudioDeviceNodeCreationStatus.Success)
            {
                // Cannot create device output node
                //rootPage.NotifyUser(String.Format("Audio Device Output unavailable because {0}", deviceOutputNodeResult.Status.ToString()), NotifyType.ErrorMessage);
                //outputDeviceContainer.Background = new SolidColorBrush(Colors.Red);
                return;
            }

            deviceOutputNode = deviceOutputNodeResult.DeviceOutputNode;
            //rootPage.NotifyUser("Device Output connection successfully created", NotifyType.StatusMessage);
            //outputDeviceContainer.Background = new SolidColorBrush(Colors.Green);

            // Create a device input node using the default audio input device
            CreateAudioDeviceInputNodeResult deviceInputNodeResult = await graph.CreateDeviceInputNodeAsync(MediaCategory.Other);

            if (deviceInputNodeResult.Status != AudioDeviceNodeCreationStatus.Success)
            {
                // Cannot create device input node
                //rootPage.NotifyUser(String.Format("Audio Device Input unavailable because {0}", deviceInputNodeResult.Status.ToString()), NotifyType.ErrorMessage);
                //inputDeviceContainer.Background = new SolidColorBrush(Colors.Red);
                return;
            }

            deviceInputNode = deviceInputNodeResult.DeviceInputNode;
            //rootPage.NotifyUser("Device Input connection successfully created", NotifyType.StatusMessage);
            //inputDeviceContainer.Background = new SolidColorBrush(Colors.Green);

            // Since graph is successfully created, enable the button to select a file output
            //fileButton.IsEnabled = true;

            // Disable the graph button to prevent accidental click
            //createGraphButton.IsEnabled = false;

            // Because we are using lowest latency setting, we need to handle device disconnection errors
            graph.UnrecoverableErrorOccurred += Graph_UnrecoverableErrorOccurred;
        }

        private async void Graph_UnrecoverableErrorOccurred(AudioGraph sender, AudioGraphUnrecoverableErrorOccurredEventArgs args)
        {
            sender.Dispose();

            // Re-query for devices
            await PopulateDeviceList();
        }

        private void StartRecording()
        {
            graph.Start();
        }

        private async Task StopRecording()
        {
            graph.Stop();

            var finalizeResult = await fileOutputNode.FinalizeAsync();
            if (finalizeResult != TranscodeFailureReason.None)
            {
                // Finalization of file failed. Check result code to see why
                //rootPage.NotifyUser(String.Format("Finalization of file failed because {0}", finalizeResult.ToString()), NotifyType.ErrorMessage);
                //fileButton.Background = new SolidColorBrush(Colors.Red);
                return;
            }

            //It worked!
            StorageFile file = null;// await saveFilePicker.PickSaveFileAsync();
            var fileProfile = MediaEncodingProfile.CreateMp3(AudioEncodingQuality.High);
            var fileOutputNodeResult = await graph.CreateFileOutputNodeAsync(file, fileProfile);

            if (fileOutputNodeResult.Status != AudioFileNodeCreationStatus.Success)
            {
                // FileOutputNode creation failed
                //rootPage.NotifyUser(String.Format("Cannot create output file because {0}", fileOutputNodeResult.Status.ToString()), NotifyType.ErrorMessage);
                //fileButton.Background = new SolidColorBrush(Colors.Red);
                return;
            }

            fileOutputNode = fileOutputNodeResult.FileOutputNode;

            // Connect the input node to both output nodes
            deviceInputNode.AddOutgoingConnection(fileOutputNode);
            deviceInputNode.AddOutgoingConnection(deviceOutputNode);
            //recordStopButton.IsEnabled = true;
        }
    }
}
