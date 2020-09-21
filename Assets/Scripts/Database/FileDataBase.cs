using System.Collections;
using System.Collections.Generic;
using System.IO;

public class FileDataBase {
    private string path;
    public FileDataBase(string url)
    {
        this.path = url;
    }

    public IEnumerable<string> ReadLine()
    {
        return File.ReadLines(this.path);
    }
}
