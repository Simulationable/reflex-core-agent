#!/bin/bash
set -e

MODEL_PATH="/llama/models/openthaigpt1.5-7B-instruct-Q4KM.gguf"
GDRIVE_FILE_ID="1qpiyi-uen5cUZfjeZotKntRQUwW3yFd5"

# Create model folder if it doesn't exist
mkdir -p "$(dirname "$MODEL_PATH")"

# Install gdown if needed
if ! command -v gdown &> /dev/null; then
    echo "Installing gdown..."
    apt-get update && apt-get install -y python3-pip
    pip3 install gdown
fi

# Download model if missing
if [ ! -f "$MODEL_PATH" ]; then
    echo "Model not found. Downloading from Google Drive..."
    gdown --id "$GDRIVE_FILE_ID" -O "$MODEL_PATH"
else
    echo "Model already exists. Skipping download."
fi

# Run llama-server
echo "Starting llama-server..."
/llama/llama-server -m "$MODEL_PATH" --port 8001 --host 0.0.0.0 &

# Run ReflexCoreAgent
echo "Starting ReflexCoreAgent..."
dotnet /app/ReflexCoreAgent.dll --urls "http://0.0.0.0:5000"

wait
