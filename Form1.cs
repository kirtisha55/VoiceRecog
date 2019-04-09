using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Speech.Synthesis;
using System.Speech.Recognition;
using System.Threading;

namespace VoiceRecog
{
    public partial class Form1 : Form
    {
        SpeechRecognitionEngine _recognizer = new SpeechRecognitionEngine();
        SpeechSynthesizer _speak = new SpeechSynthesizer();

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string str = textBox1.Text;
            _speak.Speak(str);
           
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            txtspeed.Text = "1";
            txtvolume.Text = "100";
            _speak.Rate = Convert.ToInt32(txtspeed.Text);
            _speak.Volume = Convert.ToInt32(txtvolume.Text);

            Choices commands = new Choices();
            commands.Add(new string[] { "say Hello", "print my name"});
            GrammarBuilder gbuilder = new GrammarBuilder();
            gbuilder.Append(commands);
            Grammar grammar = new Grammar(gbuilder);

            _recognizer.LoadGrammarAsync(grammar);

            //_recognizer = LoadDictationGrammars();

            _recognizer.SetInputToDefaultAudioDevice();
            _recognizer.SpeechRecognized += _recognizer_SpeechRecognized;


        }

        private void btnEnable_Click(object sender, EventArgs e)
        {
            _recognizer.RecognizeAsync(RecognizeMode.Multiple); // recognize speech 
            btnDisable.Enabled = true;
        }

        void _recognizer_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            switch(e.Result.Text)
            {
                case "say hello":
                    _speak.Speak("Hello!");
                    break;
                case "print my name":
                    textBox1.Text = "Name";
                    break;
                default:
                    textBox1.Text = e.Result.Text;
                    break;
            }
        }

        private SpeechRecognitionEngine LoadDictationGrammars()
        {

            // Create a default dictation grammar.  
            DictationGrammar defaultDictationGrammar = new DictationGrammar();
            defaultDictationGrammar.Name = "default dictation";
            defaultDictationGrammar.Enabled = true;

            // Create the spelling dictation grammar.  
            DictationGrammar spellingDictationGrammar =
              new DictationGrammar("grammar:dictation#spelling");
            spellingDictationGrammar.Name = "spelling dictation";
            spellingDictationGrammar.Enabled = true;

            // Create the question dictation grammar.  
            DictationGrammar customDictationGrammar =
              new DictationGrammar("grammar:dictation");
            customDictationGrammar.Name = "question dictation";
            customDictationGrammar.Enabled = true;

            // Create a SpeechRecognitionEngine object and add the grammars to it.  
            SpeechRecognitionEngine recoEngine = new SpeechRecognitionEngine();
            recoEngine.LoadGrammar(defaultDictationGrammar);
            recoEngine.LoadGrammar(spellingDictationGrammar);
            recoEngine.LoadGrammar(customDictationGrammar);

            // Add a context to customDictationGrammar.  
            customDictationGrammar.SetDictationContext("How do you", null);

            return recoEngine;
        }

        private void btnDisable_Click(object sender, EventArgs e)
        {
            _recognizer.RecognizeAsyncStop();
            btnDisable.Enabled = false;
        }
    }
}
