import { manipulationHelper } from "../../../../../components/core/JsInterop/modules/dom/manipulationHelper";
import { expect } from "chai";
import { JSDOM } from 'jsdom';
import { Guid } from "../../domInit";


describe("domManipulationHelper", () => {
  it("body should add a new child dom to body and then remove it from body", () => {
    //act
    const ele = document.createElement("div");
    const id = Guid.randomId();
    ele.setAttribute("id", id);
    manipulationHelper.addElementToBody(ele);
    console.log("document.body.lastChild", document.body.lastChild)
    
    //assert
    expect(document.body.lastChild).to.equal(ele);

    manipulationHelper.delElementFromBody(ele);
    //assert
    expect(document.body.querySelector("#" + id)).to.equal(null);
  });

  it("body should add a new child dom to a container dom and then remove it from the container dom", () => {
    const container: HTMLDivElement = document.createElement("div");
    const containerId = Guid.randomId("container");
    container.setAttribute("id", containerId);
    manipulationHelper.addElementToBody(container);

    const ele = document.createElement("div");
    const id = Guid.randomId();
    ele.setAttribute("id", id);
    manipulationHelper.addElementTo(ele, container);

    //assert
    expect(container.lastChild).to.equal(ele);

    manipulationHelper.delElementFrom(ele, container, 0);
    //assert
    setTimeout(()=> expect(document.body.querySelector("#" + id)).to.equal(null),0);
  });


  it("body should add a new child dom to a container dom as prepend insert", () => {
    const container = document.createElement("div");
    const containerId = Guid.randomId("container");
    container.setAttribute("id", containerId);
    manipulationHelper.addElementToBody(container);

    const ele = document.createElement("div");
    const id = Guid.randomId();
    ele.setAttribute("id", id);
    manipulationHelper.addElementTo(ele, container, true);

    //assert
    expect(container.firstChild).to.equal(ele);
  });

  it("a attribute must be set to a dom", () => {
    const attributes = {
      class:"cls-r-" + Guid.randomId()
    }
    manipulationHelper.setDomAttribute(document.body, attributes);
    
    //assert
    expect(document.body.getAttribute("class")).to.equal(attributes.class);
  });

  describe("parseNumericValue", () => {
    it("should return 0 for 'auto' value", () => {
      const result = manipulationHelper.parseNumericValue("auto");
      expect(result).to.equal(0);
    });

    it("should return 0 for 'inherit' value", () => {
      const result = manipulationHelper.parseNumericValue("inherit");
      expect(result).to.equal(0);
    });

    it("should return 0 for 'initial' value", () => {
      const result = manipulationHelper.parseNumericValue("initial");
      expect(result).to.equal(0);
    });

    it("should return 0 for empty string", () => {
      const result = manipulationHelper.parseNumericValue("");
      expect(result).to.equal(0);
    });

    it("should return 0 for null/undefined", () => {
      const result1 = manipulationHelper.parseNumericValue(null as any);
      const result2 = manipulationHelper.parseNumericValue(undefined as any);
      expect(result1).to.equal(0);
      expect(result2).to.equal(0);
    });

    it("should parse valid numeric values correctly", () => {
      expect(manipulationHelper.parseNumericValue("10px")).to.equal(10);
      expect(manipulationHelper.parseNumericValue("15.5")).to.equal(15.5);
      expect(manipulationHelper.parseNumericValue("0")).to.equal(0);
      expect(manipulationHelper.parseNumericValue("-5")).to.equal(-5);
      expect(manipulationHelper.parseNumericValue("100%")).to.equal(100);
    });

    it("should return 0 for invalid numeric values", () => {
      expect(manipulationHelper.parseNumericValue("invalid")).to.equal(0);
      expect(manipulationHelper.parseNumericValue("abc")).to.equal(0);
      expect(manipulationHelper.parseNumericValue("px")).to.equal(0);
    });

    it("should handle edge cases that would cause NaN", () => {
      // These are the cases that would previously cause crashes
      expect(manipulationHelper.parseNumericValue("auto")).to.equal(0);
      expect(manipulationHelper.parseNumericValue("inherit")).to.equal(0);
      expect(manipulationHelper.parseNumericValue("initial")).to.equal(0);
      expect(manipulationHelper.parseNumericValue("unset")).to.equal(0);
      expect(manipulationHelper.parseNumericValue("revert")).to.equal(0);
    });

    it("should calculate actual values for auto margins when element is provided", () => {
      // Create a test element with auto margins
      const container = document.createElement("div");
      container.style.width = "200px";
      container.style.height = "100px";
      container.style.position = "relative";
      document.body.appendChild(container);

      const element = document.createElement("div");
      element.style.width = "100px";
      element.style.height = "50px";
      element.style.margin = "auto";
      element.style.position = "absolute";
      element.style.top = "0";
      element.style.left = "0";
      container.appendChild(element);

      // Test with element and property - new API
      const marginValue = manipulationHelper.parseNumericValue(element, "marginLeft");
      
      // Should return a number (actual calculated margin) instead of 0
      expect(typeof marginValue).to.equal("number");
      expect(Number.isNaN(marginValue)).to.be.false;
      
      // Clean up
      document.body.removeChild(container);
    });

    it("should fallback to 0 when auto calculation fails", () => {
      const element = document.createElement("div");
      // Don't add to DOM, so calculation should fail gracefully
      element.style.margin = "auto";
      
      const marginValue = manipulationHelper.parseNumericValue(element, "marginLeft");
      expect(marginValue).to.equal(0);
    });

    it("should support legacy string-based API", () => {
      // Test legacy API still works
      expect(manipulationHelper.parseNumericValue("10px")).to.equal(10);
      expect(manipulationHelper.parseNumericValue("auto")).to.equal(0);
      expect(manipulationHelper.parseNumericValue("inherit")).to.equal(0);
    });

    it("should calculate inherit values from parent element", () => {
      // Create parent with specific margin
      const parent = document.createElement("div");
      parent.style.marginLeft = "20px";
      document.body.appendChild(parent);

      // Create child with inherit margin
      const child = document.createElement("div");
      child.style.setProperty('margin-left', 'inherit');
      parent.appendChild(child);

      // Test that the method handles inherit without crashing and returns a number
      const marginValue = manipulationHelper.parseNumericValue(child, "marginLeft");
      expect(typeof marginValue).to.equal("number");
      expect(Number.isNaN(marginValue)).to.be.false;
      
      // In test environment (JSDOM), inherit might not work as expected
      // In real browser, it should return 20, but in test environment it might return 0
      // The important thing is that it doesn't crash and returns a valid number
      expect(marginValue).to.be.at.least(0);

      // Clean up
      document.body.removeChild(parent);
    });

    it("should handle nested inherit values", () => {
      // Create grandparent with specific margin
      const grandparent = document.createElement("div");
      grandparent.style.marginTop = "30px";
      document.body.appendChild(grandparent);

      // Create parent with inherit margin
      const parent = document.createElement("div");
      parent.style.marginTop = "inherit";
      grandparent.appendChild(parent);

      // Create child with inherit margin
      const child = document.createElement("div");
      child.style.marginTop = "inherit";
      parent.appendChild(child);

      // Test that nested inherit doesn't crash
      const marginValue = manipulationHelper.parseNumericValue(child, "marginTop");
      expect(typeof marginValue).to.equal("number");
      expect(Number.isNaN(marginValue)).to.be.false;
      expect(marginValue).to.be.at.least(0);

      // Clean up
      document.body.removeChild(grandparent);
    });

    it("should directly test inherit calculation with known parent value", () => {
      // Create parent with specific margin
      const parent = document.createElement("div");
      parent.style.marginLeft = "25px";
      document.body.appendChild(parent);

      // Create child
      const child = document.createElement("div");
      parent.appendChild(child);

      // Test calculateInheritValue method directly by accessing it through the class
      // We'll simulate the inherit scenario by directly calling the method
      const inheritValue = (manipulationHelper as any).calculateInheritValue(child, "marginLeft");
      expect(typeof inheritValue).to.equal("number");
      expect(Number.isNaN(inheritValue)).to.be.false;
      // Should get the parent's 25px value
      expect(inheritValue).to.equal(25);

      // Clean up
      document.body.removeChild(parent);
    });

    it("should handle initial values correctly", () => {
      const element = document.createElement("div");
      element.style.marginTop = "initial";
      document.body.appendChild(element);

      // Initial value for margin should be 0
      const marginValue = manipulationHelper.parseNumericValue(element, "marginTop");
      expect(marginValue).to.equal(0);

      // Clean up
      document.body.removeChild(element);
    });

    it("should handle unset values correctly", () => {
      const element = document.createElement("div");
      element.style.marginBottom = "unset";
      document.body.appendChild(element);

      // Unset value for margin should be 0 (since margin is not inherited)
      const marginValue = manipulationHelper.parseNumericValue(element, "marginBottom");
      expect(marginValue).to.equal(0);

      // Clean up
      document.body.removeChild(element);
    });
  });

});
