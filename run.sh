# Check if any AVDs are available
echo "Checking if any AVDs are available..."
if [[ -z "$(ls -A $HOME/.android/avd)" ]]; then
    echo "No AVDs found. Create a new AVD throw Android Studio"
else    
    # Store the list of AVDs, excluding any lines containing "INFO"
    echo "Storing the list of AVDs..."
    avds=$($HOME/AppData/Local/Android/Sdk/emulator/emulator -list-avds | grep -v "INFO")

    # Select the first AVD from the list
    echo "Selecting the first AVD from the list..."
    emulator=$(echo "$avds" | head -n 1)

    # Check if the selected AVD is already running
    echo "Checking if the selected AVD is already running..."
    running_avd=$($HOME/AppData/Local/Android/Sdk/platform-tools/adb devices | grep "$emulator")
    if [[ -n "$running_avd" ]]; then
        echo "The selected AVD is already running."
    else
        # Start the selected AVD
        echo "Starting the selected AVD..."
        $HOME/AppData/Local/Android/Sdk/emulator/emulator -avd "$emulator" &

        # Wait for the Android device to be ready
        echo "Waiting for the Android device to be ready..."
        $HOME/AppData/Local/Android/Sdk/platform-tools/adb wait-for-device    
    fi

    # Build and run the .NET project for Android 8.0
    echo "Building and running the .NET project for Android 8.0..."
    dotnet build -t:Run -f net8.0-android
fi