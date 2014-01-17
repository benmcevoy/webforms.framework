using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace Webforms.Framework.Validation
{
    public class RangeFieldClientValidator : ClientValidator
    {
        public RangeFieldClientValidator(DataAnnotationValidator parentValidator, ValidationAttribute validationAttribute, string errorMessage)
            : base(parentValidator, validationAttribute, errorMessage)
        {
        }

        public override void AddValidatorAttributes()
        {
            var rangeAttribute = _validationAttribute as RangeAttribute;

            Type type = rangeAttribute.OperandType;

            RenderBaseCompareFieldAttributes(type);

            AddAttributesToRender("evaluationfunction", "RangeValidatorEvaluateIsValid");

            var maximumValue = ConvertToString(rangeAttribute.Maximum, _parentValidator.PropertyDescriptor);
            var minimumValue = ConvertToString(rangeAttribute.Minimum, _parentValidator.PropertyDescriptor);

            AddAttributesToRender("maximumvalue", maximumValue);
            AddAttributesToRender("minimumvalue", minimumValue);
        }

        private void RenderBaseCompareFieldAttributes(Type type)
        {
            if (type != typeof(string))
            {
                var namedType = GetTypeAsString(type);

                AddAttributesToRender("type", GetTypeAsString(type));

                NumberFormatInfo currentInfo = NumberFormatInfo.CurrentInfo;
                switch (namedType)
                {
                    case "Double":
                        {
                            string numberDecimalSeparator = currentInfo.NumberDecimalSeparator;
                            AddAttributesToRender("decimalchar", numberDecimalSeparator);
                            break;
                        }
                    case "Currency":
                        {
                            string currencyDecimalSeparator = currentInfo.CurrencyDecimalSeparator;
                            AddAttributesToRender("decimalchar", currencyDecimalSeparator);
                            string currencyGroupSeparator = currentInfo.CurrencyGroupSeparator;
                            if (currencyGroupSeparator[0] == '\x00a0')
                            {
                                currencyGroupSeparator = " ";
                            }
                            AddAttributesToRender("groupchar", currencyGroupSeparator);
                            AddAttributesToRender("digits", currentInfo.CurrencyDecimalDigits.ToString(NumberFormatInfo.InvariantInfo));
                            int currencyGroupSize = GetCurrencyGroupSize(currentInfo);
                            if (currencyGroupSize > 0)
                            {
                                AddAttributesToRender("groupsize", currencyGroupSize.ToString(NumberFormatInfo.InvariantInfo));
                                break;
                            }
                            break;
                        }
                    case "Date":
                        {
                            AddAttributesToRender("dateorder", GetDateElementOrder());
                            AddAttributesToRender("cutoffyear", CutoffYear.ToString(NumberFormatInfo.InvariantInfo));
                            int year = DateTime.Today.Year;
                            AddAttributesToRender("century", (year - (year % 100)).ToString(NumberFormatInfo.InvariantInfo));
                            break;
                        }
                }
            }
        }

        private string GetTypeAsString(Type type)
        {
            if (type == typeof(Int16) || type == typeof(Int32) || type == typeof(Int64) || type == typeof(UInt16) || type == typeof(UInt32) || type == typeof(UInt64))
            {
                return "Integer";
            }

            if (type == typeof(DateTime))
            {
                return "Date";
            }

            if (type == typeof(double) || type == typeof(decimal) || type == typeof(float))
            {
                return "Double";
            }

            return "String";
        }

        private string GetDateElementOrder()
        {
            string shortDatePattern = DateTimeFormatInfo.CurrentInfo.ShortDatePattern;
            if (shortDatePattern.IndexOf('y') < shortDatePattern.IndexOf('M'))
            {
                return "ymd";
            }
            if (shortDatePattern.IndexOf('M') < shortDatePattern.IndexOf('d'))
            {
                return "mdy";
            }
            return "dmy";
        }

        private int GetCurrencyGroupSize(NumberFormatInfo info)
        {
            int[] currencyGroupSizes = info.CurrencyGroupSizes;
            if ((currencyGroupSizes != null) && (currencyGroupSizes.Length == 1))
            {
                return currencyGroupSizes[0];
            }
            return -1;
        }

        private int CutoffYear
        {
            get
            {
                return DateTimeFormatInfo.CurrentInfo.Calendar.TwoDigitYearMax;
            }
        }

        private string ConvertToString(object value, PropertyDescriptor property)
        {
            if (property.PropertyType == typeof(DateTime))
            {
                return ((DateTime)value).ToShortDateString();
            }
            return Convert.ToString(value, CultureInfo.CurrentCulture);
        }
    }
}
