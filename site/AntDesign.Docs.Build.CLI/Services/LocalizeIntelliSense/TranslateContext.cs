using System;
using System.Globalization;


// This code is modified from the soures of https://github.com/stratosblue/IntelliSenseLocalizer


namespace IntelliSenseLocalizer.ThirdParty;

public class TranslateContext
{
    #region Public 属性

    public string FilePath { get; set; }

    public bool IsPatch { get; }

    public string OutputPath { get; }

    public int ParallelCount { get; set; } = 2;

    public CultureInfo SourceCultureInfo { get; }

    public CultureInfo TargetCultureInfo { get; }

    #endregion Public 属性

    #region Public 构造函数

    public TranslateContext(string filePath, string outputPath, CultureInfo sourceCultureInfo, CultureInfo targetCultureInfo, bool isPatch)
    {
        if (string.IsNullOrWhiteSpace(filePath))
        {
            throw new ArgumentException($"“{nameof(filePath)}”不能为 null 或空白。", nameof(filePath));
        }

        if (string.IsNullOrWhiteSpace(outputPath))
        {
            throw new ArgumentException($"“{nameof(outputPath)}”不能为 null 或空白。", nameof(outputPath));
        }

        FilePath = filePath;
        OutputPath = outputPath;
        SourceCultureInfo = sourceCultureInfo ?? throw new ArgumentNullException(nameof(sourceCultureInfo));
        TargetCultureInfo = targetCultureInfo ?? throw new ArgumentNullException(nameof(targetCultureInfo));
        IsPatch = isPatch;
    }

    #endregion Public 构造函数
}
