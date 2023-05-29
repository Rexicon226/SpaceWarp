using UnityEngine.UIElements;
using UnityEngine;
using System;

namespace SpaceWarpElements.Console
{ 
    public class LogEntry : BindableElement
    {
        public static readonly string ussClassName = "spacewarp-logEntry";
        public static readonly string ussLogEntryElementClassName = ussClassName + "-element";
        public static readonly string ussHeaderGrouperClassName = ussClassName + "__headerContent";
        public static readonly string ussTimeDateClassName = ussClassName + "__timeDate";
        public static readonly string ussLogLevelClassName = ussClassName + "__logLevel";
        public static readonly string ussLogSourceClassName = ussClassName + "__logSource";
        public static readonly string ussLogMessageHeaderClassName = ussClassName + "__logMessage";

        public static readonly string ussMessageGrouperClassName = ussClassName + "__messageContent";
        public static readonly string ussMessageClassName = ussClassName + "__messageLabel";

        public VisualElement HeaderGrouper;
        public Label TimeDateLabel;
        public string TimeDate { get; set; }

        public Label LogLevelLabel;
        public string logLevel
        {
            get => _logLevel;
            set
            {
                if (value != _logLevel)
                {
                    _logLevel = value;
                    LogLevelLabel.text = Colorize($"[{_logLevel}");
                }
            }
        }
        private string _logLevel = "None";

        public Label LogSourceLabel;
        public string LogSource
        {
            get => _logSource;
            set
            {
                if (value != _logSource)
                {
                    _logSource = value;
                    LogSourceLabel.text = Colorize($":{_logSource}]");
                }
            }
        }
        private string _logSource;

        public Label LogMessageHeaderLabel;
        public string LogMessageHeader
        {
            get
            {
                if (_logMessage.Contains(@"\n"))
                {
                    return _logMessage.Substring(0, _logMessage.IndexOf(@"\n")) + (!Expanded ? " (...)" : string.Empty);
                }
                else
                    return _logMessage;
                
            }
        }
        private string _logMessage;

        public VisualElement MessageGrouper;
        public Label LogMessageLabel;
        public string LogMessage
        {
            get => _logMessage;
            set
            {
                if (value != _logMessage)
                {
                    _logMessage = value;
                    LogMessageLabel.text = Colorize($" {_logMessage}");
                    LogMessageHeaderLabel.text = Colorize(LogMessageHeader);
                }
            }
        }
        public bool Expanded
        {
            get => _expanded;
            set
            {
                if(value != _expanded)
                {
                    _expanded = value;
                    if (value)
                    {
                        LogMessageLabel.style.display = DisplayStyle.Flex;
                    }
                    else
                    {
                        LogMessageLabel.style.display = DisplayStyle.None;
                    }
                    LogMessageHeaderLabel.text = Colorize(LogMessageHeader);
                }
            }
        }
        private bool _expanded;
        public bool Show
        {
            get => _show;
            set
            {
                if(value != _show)
                {
                    _show = value;
                    if (value)
                    {
                        this.style.display = DisplayStyle.Flex;
                    }
                    else
                    {
                        this.style.display = DisplayStyle.None;
                    }
                }
            }
        }
        private bool _show;

        public new class UxmlFactory : UxmlFactory<LogEntry, UxmlTraits> { }

        public new class UxmlTraits : BindableElement.UxmlTraits
        {
            UxmlStringAttributeDescription timeDate = new UxmlStringAttributeDescription { name = "timeDate", defaultValue = "00:00:00.000" };
            //UxmlEnumAttributeDescription<LogLevel> logLevel = new UxmlEnumAttributeDescription<LogLevel> { name = "logLevel", defaultValue = LogLevel.Info };
            UxmlStringAttributeDescription logSource = new UxmlStringAttributeDescription { name = "logSource", defaultValue = "spacewarp" };
            UxmlColorAttributeDescription textColor = new UxmlColorAttributeDescription { name = "textColor", defaultValue = Color.white };
            UxmlStringAttributeDescription logMessage = new UxmlStringAttributeDescription { name = "logMessage", defaultValue = "There should be an log here" };
            UxmlBoolAttributeDescription expanded = new UxmlBoolAttributeDescription { name = "expanded", defaultValue =  false };


            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);

                if (ve is LogEntry _logEntry)
                {
                    _logEntry.TimeDate = timeDate.GetValueFromBag(bag, cc);
                    //_logEntry.logLevel = logLevel.GetValueFromBag(bag, cc).ToString();
                    _logEntry.LogSource = logSource.GetValueFromBag(bag, cc);
                    _logEntry.LogMessage = logMessage.GetValueFromBag(bag, cc);
                    _logEntry.TextColor = textColor.GetValueFromBag(bag, cc);
                    _logEntry.Expanded = expanded.GetValueFromBag(bag, cc);
                }
            }
        }
        public Color TextColor
        {
            get => _textColor;
            set
            {
                if (_textColor != value)
                {
                    _textColor = value;
                    UpdateLabels();
                }
            }
        }

        private void UpdateLabels()
        {
            TimeDateLabel.text = Colorize(TimeDate);
            LogLevelLabel.text = Colorize($"[{_logLevel}");
            LogSourceLabel.text = Colorize($":{_logSource}]");
            LogMessageHeaderLabel.text = Colorize(LogMessageHeader);
            LogMessageLabel.text = Colorize($" {_logMessage}");
        }

        private Color _textColor = Color.white;
        public string Colorize(string toColorize) => $"<color=#{ColorUtility.ToHtmlStringRGB(TextColor)}>{toColorize}</color>";

        public LogEntry() : this("00:00:00.000[ERROR : Source] some error here\n and there") { }
        public LogEntry(string toParse) : this(toParse, Color.white) { }

        public LogEntry(string toParse, Color textColor, bool startCollapsed = true)
        {
            string timeStamp = toParse.Substring(0, 12);
            string levelAndSource = toParse.Split('[')[1];
            string logLevel = levelAndSource.Split(':')[0];
            string source = levelAndSource.Split(':')[1].Split(']')[0];
            string data = levelAndSource.Split(']')[1];
            TextColor = textColor;


            AddToClassList(ussClassName);

            HeaderGrouper = new VisualElement()
            {
                name = "console-LogEntry-headerGroup"
            };
            HeaderGrouper.AddToClassList(ussHeaderGrouperClassName);
            HeaderGrouper.AddToClassList(ussLogEntryElementClassName);

            hierarchy.Add(HeaderGrouper);

            TimeDateLabel = new Label()
            {
                name = "console-LogEntry-timeDate",
                text = Colorize($"{timeStamp}")
            };

            TimeDateLabel.AddToClassList(ussTimeDateClassName);
            TimeDateLabel.AddToClassList(ussLogEntryElementClassName);
            LogLevelLabel = new Label()
            {
                name = "console-LogEntry-logLevel",
                text = Colorize($"[{logLevel}")
            };

            LogLevelLabel.AddToClassList(ussLogLevelClassName);
            LogLevelLabel.AddToClassList(ussLogEntryElementClassName);

            LogSourceLabel = new Label()
            {
                name = "console-LogEntry-logSource",
                text = Colorize($":{source}]")
            };

            LogSourceLabel.AddToClassList(ussLogSourceClassName);
            LogSourceLabel.AddToClassList(ussLogEntryElementClassName);

            LogMessageHeaderLabel = new Label()
            {
                name = "console-LogEntry-logMessageHeader",
                //text = $"{LogMessageHeader}"
            };

            LogMessageHeaderLabel.AddToClassList(ussLogMessageHeaderClassName);
            LogMessageHeaderLabel.AddToClassList(ussLogEntryElementClassName);

            MessageGrouper = new VisualElement()
            {
                name = "console-LogEntry-messageGroup"
            };
            MessageGrouper.AddToClassList(ussMessageGrouperClassName);
            MessageGrouper.AddToClassList(ussLogEntryElementClassName);
            hierarchy.Add(MessageGrouper);

            LogMessageLabel = new Label()
            {
                name = "console-LogEntry-logMessage",
                //text = $"{data.Substring(0, _logMessage.IndexOf("\n"))[0]}"
            };

            LogMessageLabel.AddToClassList(ussMessageClassName);
            LogMessageLabel.AddToClassList(ussLogEntryElementClassName);

            HeaderGrouper.hierarchy.Add(TimeDateLabel);
            HeaderGrouper.hierarchy.Add(LogLevelLabel);
            HeaderGrouper.hierarchy.Add(LogSourceLabel);
            HeaderGrouper.hierarchy.Add(LogMessageHeaderLabel);

            MessageGrouper.hierarchy.Add(LogMessageLabel);

            TimeDate = timeStamp;
            this.LogSource = source;
            this.logLevel = logLevel;
            this.LogMessage = data;
            this.Expanded = !startCollapsed;
        }
    }
}