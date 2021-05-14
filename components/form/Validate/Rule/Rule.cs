using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AntDesign
{
    /*
        Done: defaultField	    仅在 type 为 array 类型时有效，用于指定数组元素的校验规则	                                rule
        would not support: enum(replace by oneOf)	    是否匹配枚举中的值（需要将 type 设置为 enum）	                                any[]
        Done: oneOf	        是否匹配数组中的值（type 不能为 array 类型）	                                            object[]
        Done: fields	        仅在 type 为 array 或 object 类型时有效，用于指定子元素的校验规则	                        Record<string, rule>
        Done: len	            string 类型时为字符串长度；number 类型时为确定数字； array 类型时为数组长度	                number
        Done: max	            必须设置 type：string 类型为字符串最大长度；number 类型时为最大值；array 类型时为数组最大长度	number
        Done: message	        错误信息，不设置时会通过模板自动生成	                                                    string
        Done: min	            必须设置 type：string 类型为字符串最小长度；number 类型时为最小值；array 类型时为数组最小长度	number
        Done: pattern	        正则表达式匹配	                                                                        RegExp
        Done: required	        是否为必选字段	                                                                        boolean
        Done: transform	        将字段值转换成目标值后进行校验	                                                            (value) => any
        Done: type	            类型，常见有 string |number |boolean |url | email。更多请参考此处	                        string
        TODO: validateTrigger	设置触发验证时机，必须是 Form.Item 的 validateTrigger 的子集	                            string | string[]
        Done: validator	        自定义校验，接收 Promise 作为返回值。示例参考	                                            (rule, value) => Promise
        would not support: whitespace	    如果字段仅包含空格则校验不通过	                                                            boolean
     */
    public class Rule
    {
        public decimal? Len { get; set; }
        public decimal? Min { get; set; }
        public decimal? Max { get; set; }
        public bool? Required { get; set; }
        public string Pattern { get; set; }
        public string Message { get; set; }
        public Rule DefaultField { get; set; }
        public object[] OneOf { get; set; }
        public Dictionary<object, Rule> Fields { get; set; }
        public Func<RuleValidationContext, ValidationResult> Validator { get; set; }
        public Func<object, object> Transform { get; set; } = (value) => value;
        public RuleFieldType Type { get; set; } = RuleFieldType.String;
    }
}
