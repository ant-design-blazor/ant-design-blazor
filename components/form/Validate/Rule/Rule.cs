using System;
using System.Collections.Generic;
using System.Text;

namespace AntDesign
{
    /*
        TODO: defaultField	    仅在 type 为 array 类型时有效，用于指定数组元素的校验规则	                                rule
        TODO: enum	            是否匹配枚举中的值（需要将 type 设置为 enum）	    any[]
        TODO: fields	        仅在 type 为 array 或 object 类型时有效，用于指定子元素的校验规则	                        Record<string, rule>
        Done: len	            string 类型时为字符串长度；number 类型时为确定数字； array 类型时为数组长度	                number
        TODO: max	            必须设置 type：string 类型为字符串最大长度；number 类型时为最大值；array 类型时为数组最大长度	number
        TODO: message	        错误信息，不设置时会通过模板自动生成	                                                    string
        TODO: min	            必须设置 type：string 类型为字符串最小长度；number 类型时为最小值；array 类型时为数组最小长度	number
        TODO: pattern	        正则表达式匹配	                                                                        RegExp
        Done: required	        是否为必选字段	                                                                        boolean
        TODO: transform	        将字段值转换成目标值后进行校验	                                                            (value) => any
        Done: type	            类型，常见有 string |number |boolean |url | email。更多请参考此处	                        string
        TODO: validateTrigger	设置触发验证时机，必须是 Form.Item 的 validateTrigger 的子集	                            string | string[]
        TODO: validator	        自定义校验，接收 Promise 作为返回值。示例参考	                                            (rule, value) => Promise
        TODO: whitespace	    如果字段仅包含空格则校验不通过	                                                            boolean
     */
    public class Rule
    {
        public decimal? Len { get; set; }
        public decimal? Min { get; set; }
        public decimal? Max { get; set; }
        public bool? Required { get; set; }
        public RuleFieldType Type { get; set; } = RuleFieldType.String;
    }
}
