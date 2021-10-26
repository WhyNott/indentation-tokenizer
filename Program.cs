using System;
using System.Collections.Generic;
using tokens;
namespace indentation_tokenizer {

    public class InconsistentIndentException : Exception{
        public InconsistentIndentException() {
        }
        
        public InconsistentIndentException(string message)
            : base(message) {
        }
        
        public InconsistentIndentException(string message, Exception inner)
            : base(message, inner){
        }
    }



    
    class Program {
        static void Main(string[] args) {
            var physical_lines = System.IO.File.ReadAllLines("example.yaml");
            List<Token> output_tokens = new List<Token>();
            Stack<int> indentation_stack = new Stack<int>();
            indentation_stack.Push(0);
            int linenum = 1;
            foreach (string line in physical_lines) {
                bool in_indentation = true;
                int indent_counter = 0;
                string sentence_so_far = "";
                void FlushSentence() {
                    if (sentence_so_far != "") {
                        output_tokens.Add(new SentenceToken(sentence_so_far.Trim()));
                    }
                    sentence_so_far = "";
                }

                foreach (char ch in line.ToCharArray()) {
                    if (ch == '#') {
                        break;
                    }
                    if (in_indentation) {
                        if (ch == ' ') {
                            indent_counter++;
                            continue;
                        }
                        if (indent_counter > indentation_stack.Peek()) {
                            output_tokens.Add(new IndentToken());
                            indentation_stack.Push(indent_counter);
                        } else if (indent_counter < indentation_stack.Peek()) {
                            while (indentation_stack.Count > 0) {
                                output_tokens.Add(new DedentToken());
                                int indentation = indentation_stack.Pop();
                                if (indentation == indent_counter) {
                                    break;
                                }
                            }

                            if (indentation_stack.Count == 0) {
                                if (indent_counter == 0){
                                    indentation_stack.Push(0);
                                } else {
                                    throw new InconsistentIndentException(
                                        "Indentation error at line " + linenum);
                                }
                            }

                        }
                        in_indentation = false;
                    }
                    if (Enum.IsDefined(typeof(DelimiterKind), (int)ch)) {
                        FlushSentence();
                        output_tokens.Add(new DelimiterToken(ch));
                    } else {
                        sentence_so_far += ch;
                    }
                }
                FlushSentence();
                output_tokens.Add(new NewlineToken());
                linenum++;
            }

            //Output all remaining dedentation tokens
            while (indentation_stack.Count > 0) {
                output_tokens.Add(new DedentToken());
                indentation_stack.Pop();
            }
            
            foreach (Token t in output_tokens){
                Console.WriteLine(t.ToString());
            }
        }
    }
}
