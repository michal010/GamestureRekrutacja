using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public struct ImageFileData
{
    //public byte[] fileBytes;
    public string filePath;
    public string fileName;
    public DateTime creationTime;
    public ImageFileData(string filePath, string fileName, DateTime creationTime)
    {
        this.filePath = filePath;
        this.fileName = fileName;
        this.creationTime = creationTime;
    }
}
//inherit interface of T type, Image loader should be T type aswell to specify the type?
public class ImageLoader
{
    public async Task<List<ImageFileData>> GetFilesAsync(string path, string fileExtension = "*.png")
    {
        List<ImageFileData> results = new List<ImageFileData>();
        var result = await Task.Run(() =>
       {
           string[] filePaths = Directory.GetFiles(path, fileExtension, SearchOption.TopDirectoryOnly);
           for (int i = 0; i < filePaths.Length; i++)
           {
               Task<DateTime> creationTimeTask = GetFileCreationTimeAsync(filePaths[i]);
               //Task<byte[]> imageBytesTask = GetImageBytesAsync(filePaths[i]);
               Task<string> fileNameTask = GetFilenameAsync(filePaths[i]);
               //Task.WaitAll(creationTimeTask, imageBytesTask, fileNameTask);
               Task.WaitAll(creationTimeTask, fileNameTask);
               results.Add(new ImageFileData(filePaths[i], fileNameTask.Result, creationTimeTask.Result));
           }
           return results;
       });
        return results;
    }
    private async Task<DateTime> GetFileCreationTimeAsync(string filePath)
    {
        var result = await Task.Run(() => 
        {
            return File.GetCreationTime(filePath); 
        });
        return result;
    }

    private async Task<byte[]> GetImageBytesAsync(string filePath)
    {
        var result = await Task.Run(() => 
        {
            return File.ReadAllBytes(filePath);
        });
        return result;
    }

    private async Task<string> GetFilenameAsync(string filePath)
    {
        var result = await Task.Run(()=>
        {
           return Path.GetFileName(filePath);
        });
        return result;
    }

}
