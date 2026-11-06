import { infoHelper } from '../../../../../components/core/JsInterop/modules/dom/infoHelper';
import { expect } from 'chai';
import * as domInit from '../../domInit'


describe('domInfoHelper', () => { 
  it('getWindow should return proper object', () => {
    //act
    let info = infoHelper.getWindow();
    //assert
    expect(info).to.haveOwnProperty("innerWidth");
    expect(info).to.haveOwnProperty("innerHeight");
  });

  it('getElementAbsoulutePos should return domTypes.position', () => {
    //arrange
    const testedElement = domInit.addElementToBody('<div id="underTest">Test element</div>');
    //act
    let pos = infoHelper.getElementAbsolutePos(testedElement);
    //assert
    expect(pos).to.haveOwnProperty("x");
    expect(pos).to.haveOwnProperty("y");
  });

  it('getInfo should handle elements with auto margins without crashing', () => {
    //arrange
    const testedElement = domInit.addElementToBody('<div id="autoMarginTest" style="margin: auto; width: 100px;">Test element</div>');
    
    //act
    let info = infoHelper.getInfo(testedElement);
    
    //assert
    expect(info).to.haveOwnProperty("marginTop");
    expect(info).to.haveOwnProperty("marginBottom");
    expect(info).to.haveOwnProperty("marginLeft");
    expect(info).to.haveOwnProperty("marginRight");
    
    // All margin values should be numbers (not NaN) when margin is auto
    // Now they might be actual calculated values instead of just 0
    expect(typeof info.marginTop).to.equal('number');
    expect(typeof info.marginBottom).to.equal('number');
    expect(typeof info.marginLeft).to.equal('number');
    expect(typeof info.marginRight).to.equal('number');
    
    expect(Number.isNaN(info.marginTop)).to.be.false;
    expect(Number.isNaN(info.marginBottom)).to.be.false;
    expect(Number.isNaN(info.marginLeft)).to.be.false;
    expect(Number.isNaN(info.marginRight)).to.be.false;
    
    // Values should be >= 0 (could be 0 or actual calculated margins)
    expect(info.marginTop).to.be.at.least(0);
    expect(info.marginBottom).to.be.at.least(0);
    expect(info.marginLeft).to.be.at.least(0);
    expect(info.marginRight).to.be.at.least(0);
  });

  it('getInfo should handle elements with inherit margins without crashing', () => {
    //arrange
    const testedElement = domInit.addElementToBody('<div id="inheritMarginTest" style="margin: inherit;">Test element</div>');
    
    //act
    let info = infoHelper.getInfo(testedElement);
    
    //assert
    // Values should be numbers (not NaN) and >= 0
    expect(typeof info.marginTop).to.equal('number');
    expect(typeof info.marginBottom).to.equal('number');
    expect(typeof info.marginLeft).to.equal('number');
    expect(typeof info.marginRight).to.equal('number');
    
    expect(Number.isNaN(info.marginTop)).to.be.false;
    expect(Number.isNaN(info.marginBottom)).to.be.false;
    expect(Number.isNaN(info.marginLeft)).to.be.false;
    expect(Number.isNaN(info.marginRight)).to.be.false;
    
    expect(info.marginTop).to.be.at.least(0);
    expect(info.marginBottom).to.be.at.least(0);
    expect(info.marginLeft).to.be.at.least(0);
    expect(info.marginRight).to.be.at.least(0);
  });

  it('getInfo should handle elements with initial margins correctly', () => {
    //arrange
    const testedElement = domInit.addElementToBody('<div id="initialMarginTest" style="margin: initial;">Test element</div>');
    
    //act
    let info = infoHelper.getInfo(testedElement);
    
    //assert
    // Initial margin values should be 0
    expect(info.marginTop).to.equal(0);
    expect(info.marginBottom).to.equal(0);
    expect(info.marginLeft).to.equal(0);
    expect(info.marginRight).to.equal(0);
  });
});
