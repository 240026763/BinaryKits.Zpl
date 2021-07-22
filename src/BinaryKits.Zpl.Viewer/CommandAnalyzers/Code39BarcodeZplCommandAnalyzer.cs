﻿using BinaryKits.Zpl.Label.Elements;

namespace BinaryKits.Zpl.Viewer.CommandAnalyzers
{
    public class Code39BarcodeZplCommandAnalyzer : ZplCommandAnalyzerBase
    {
        public Code39BarcodeZplCommandAnalyzer(VirtualPrinter virtualPrinter) : base("^B3", virtualPrinter)
        { }

        public override ZplElementBase Analyze(ZplCommandStructure zplCommandStructure)
        {
            var x = this.VirtualPrinter.NextElementPosition.X;
            var y = this.VirtualPrinter.NextElementPosition.Y;

            this.VirtualPrinter.ClearNextElementPosition();

            var zplCommandData = zplCommandStructure.CurrentCommand.Substring(this.PrinterCommandPrefix.Length);

            var zplDataParts = zplCommandData.Split(',');

            var mod43CheckDigit = false;
            var height = this.VirtualPrinter.BarcodeInfo.Height;
            var printInterpretationLine = true;
            var printInterpretationLineAboveCode = false;

            var fieldOrientation = this.ConvertFieldOrientation(zplDataParts[0]);
            if (zplDataParts.Length > 1)
            {
                mod43CheckDigit = this.ConvertBoolean(zplDataParts[1]);
            }
            if (zplDataParts.Length > 2)
            {
                _ = int.TryParse(zplDataParts[2], out height);
            }
            if (zplDataParts.Length > 3)
            {
                printInterpretationLine = !this.ConvertBoolean(zplDataParts[3]);
            }
            if (zplDataParts.Length > 4)
            {
                printInterpretationLineAboveCode = this.ConvertBoolean(zplDataParts[4]);
            }

            return new ZplBarcode39("123456", x, y, height, fieldOrientation, printInterpretationLine, printInterpretationLineAboveCode, mod43CheckDigit);
        }
    }
}
