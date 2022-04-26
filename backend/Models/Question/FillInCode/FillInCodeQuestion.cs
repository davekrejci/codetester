using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Codetester.Models
{
    public class FillInCodeQuestion : Question
    {
        [Required]
        public string CodeDescription { get; set; }

        [Required]
        public string Code { get; set; }

        [Required]
        public int FillCount { get; set; }

        [Required]
        public ICollection<FillInCodeBlock> FillInCodeBlocks { get; set; }


        public FillInCodeQuestion()
        {
            this.QuestionType = Models.QuestionType.FILL_IN_CODE;
        }

        public override QuestionInstance CreateInstance()
        {
            FillInCodeQuestionInstance instance = new FillInCodeQuestionInstance();
            var rand = new Random();
            
            // 1. Populate code description & max score
            instance.CodeDescription = this.CodeDescription;
            instance.MaxScore = this.FillCount;

            // 2. Populate code with '{widget...}' value in place of blocks
            var transformedCode = this.Code;
            
                // pick n random blocks based on selected fillCount number
                var blocks = this.FillInCodeBlocks.OrderBy(x => rand.Next()).Take(this.FillCount);

                // sort randomly selected blocks by position in content so that the strings can be properly replaced
                blocks = blocks.OrderBy(x => x.StartPosition);

                List<FillInCodeBlockInstance> blockInstances = new List<FillInCodeBlockInstance>();

                // replace each block with shortcode and create block instances
                int charDiff = 0;
                foreach (FillInCodeBlock block in blocks)
                {
                    FillInCodeBlockInstance blockInstance = new FillInCodeBlockInstance(block);
                    string widgetId = generateWidgetId();
                    blockInstance.WidgetId = widgetId;
                    blockInstances.Add(blockInstance);
                    var fromIndex = blockInstance.StartPosition;
                    var toIndex = blockInstance.EndPosition;
                    var length = toIndex - fromIndex;
                    var widgetString = $"{{\"widget_id\":\"{blockInstance.WidgetId}\", \"length\":{length}}}";
                    transformedCode = transformedCode.Remove(fromIndex + charDiff, length).Insert(fromIndex + charDiff, widgetString);
                    charDiff += widgetString.Length - length;
                }

                instance.FillInCodeBlocks = blockInstances;

                foreach(var block in instance.FillInCodeBlocks)
                {
                    System.Console.WriteLine("Id: " + block.Id);
                }

                System.Console.WriteLine("The transformed code: " + transformedCode);
                instance.Code = transformedCode;

            return instance;


            string generateWidgetId()
            {
                return Guid.NewGuid().ToString("N");
            }
        }
    }
}