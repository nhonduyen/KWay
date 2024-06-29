// See https://aka.ms/new-console-template for more information
using KWay;

Console.WriteLine("KWay algorithm");

const string inputFilePath = "input.txt";
const string outputFilePath = "output.txt";
const int NumOfNumbers = 1000000;
int chunkSize = NumOfNumbers / 10;

if (!File.Exists(inputFilePath))
{
    await FileHelper.GenerateInputFileAsync(inputFilePath, NumOfNumbers);
    Console.WriteLine($"File {inputFilePath} generated");
}

// Step 1: Split and sort chunks
Console.WriteLine("Split chunk files and sort");
List<string> sortedChunkFiles = await KWayMergeSort.SplitAndSortChunksAsync(inputFilePath, chunkSize);

// Step 2: Merge sorted chunks
Console.WriteLine($"Merge sorted chunks and write to {outputFilePath}");
KWayMergeSort.MergeSortedChunks(sortedChunkFiles, outputFilePath);

// Cleanup temporary files
foreach (string file in sortedChunkFiles)
{
    Console.WriteLine($"Delete file {file}");
    File.Delete(file);
}