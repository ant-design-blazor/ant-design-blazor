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
    expect(document.body.querySelector("#" + id)).to.equal(null);
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

});
