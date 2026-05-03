using System.Collections.Generic;
using System.Linq;
using JeopardyNesTextTool.ViewModel;

namespace JeopardyNesTextTool.Model
{
    public static class GroupSizeCalculator
    {
        public static void Recalculate(ApplicationViewModel app)
        {
            if (app.ModelBlocks == null || app.ModelBlocks.Count == 0)
            {
                ResetAll(app);
                return;
            }

            var questionsTree = BuildTree(QuestionsAndTopicsText(app.ModelBlocks));
            var answersTree = BuildTree(AnswersText(app.ModelBlocks));

            var blockIndex = 0;
            foreach (var vmGroup in app.ViewModelGroups)
            {
                uint groupActual = 0;
                foreach (var vmBlock in vmGroup.ViewModelBlocks)
                {
                    if (blockIndex >= app.ModelBlocks.Count) break;
                    var plainBlock = app.ModelBlocks[blockIndex].GetPlainBlock();
                    plainBlock.Encode(questionsTree, answersTree);
                    var bytes = (uint)plainBlock.GetEncodedBinaryBlock().Length;
                    vmBlock.ActualBytes = bytes;
                    groupActual += bytes;
                    blockIndex++;
                }
                vmGroup.ActualBytes = groupActual;
            }
        }

        private static InternalNode BuildTree(string text)
        {
            var tree = new InternalNode(text);
            tree.SetTreeCharsPaths();
            return tree;
        }

        private static string QuestionsAndTopicsText(IEnumerable<StructuredTextBlock> blocks)
        {
            var plain = blocks.Select(b => b.GetPlainBlock()).ToList();
            var questions = plain.SelectMany(p => p.Questions);
            var topics = plain.SelectMany(p => p.Topics);
            return string.Concat(questions.Concat(topics));
        }

        private static string AnswersText(IEnumerable<StructuredTextBlock> blocks)
        {
            return string.Concat(blocks.SelectMany(b => b.GetPlainBlock().Answers));
        }

        private static void ResetAll(ApplicationViewModel app)
        {
            foreach (var vmGroup in app.ViewModelGroups)
            {
                vmGroup.ActualBytes = 0;
                foreach (var vmBlock in vmGroup.ViewModelBlocks)
                {
                    vmBlock.ActualBytes = 0;
                }
            }
        }
    }
}
