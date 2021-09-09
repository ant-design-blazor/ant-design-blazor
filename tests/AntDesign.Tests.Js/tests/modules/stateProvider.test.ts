import { state } from '../../../../components/core/JsInterop/modules/stateProvider';
import { expect } from 'chai';


describe('StateProvider', () => { 
  it('objReferenceDict behaves like a dictionary', () => {
    //arrange
    let value = "testState";
    //act
    state.objReferenceDict["test"] = value
    //assert
    expect(state.objReferenceDict["test"]).to.equal(value);    
  });
});