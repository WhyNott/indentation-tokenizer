namespace tokens {

    
    public abstract class Token {
        
    }

    public class IndentToken : Token {
        public override string ToString() {
            return "[INDENT]";
        }
    }

    public class DedentToken : Token {
        public override string ToString() {
            return "[DEDENT]";
        }
    }

    public class NewlineToken : Token {
        public override string ToString() {
            return "[NEWLINE]";
        }
    }

    public class SentenceToken : Token {
        public string sentence_content;
        public SentenceToken(string s) {
            this.sentence_content = s;
        }

        public override string ToString() {
            return "[SENTENCE: '"+ this.sentence_content +"']";
        }
    }

    public enum DelimiterKind {
        Coma = ',',
        Colon = ':',
        Dot = '.',
        Semicolon = ';',
        At = '@',
        Equals = '=',
        Ampersand = '&',
        Dash = '-',
        Star = '*'
    }

    public class DelimiterToken : Token {
        public DelimiterKind delimiter_content;

        public DelimiterToken(char ch) {
            this.delimiter_content = (DelimiterKind)ch;
        }

        public override string ToString() {
            return "[DELIMITER: '"+ this.delimiter_content.ToString() +"']";
        }
        
    }

    
    
}
