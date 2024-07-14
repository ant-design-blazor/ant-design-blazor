import { Overlay, Placement, TriggerBoundyAdjustMode, overlayConstraints } from '../../../../../components/core/JsInterop/modules/components/overlay';
import { infoHelper } from '../../../../../components/core/JsInterop/modules/dom/infoHelper';
import * as domInit from '../../domInit'
import { resizeObserver } from '../../../../../components/core/JsInterop/ObservableApi/resizeObserver'
import { mutationObserver } from '../../../../../components/core/JsInterop/ObservableApi/mutationObserver'

//test imports
const itParam = require('mocha-param').itParam;
import { expect } from 'chai';
import * as sinon from 'sinon';
import * as rewire from 'rewire'
import { domInfo } from '../../../../../components/core/JsInterop/modules/dom/types';
import exp = require('constants');
import { domInfoHelper, domTypes } from '../../../../../components/core/JsInterop/modules/dom/exports';

const domInfoDefaults: domInfo = {
  offsetTop: 0,
  offsetLeft: 0,
  offsetWidth: 0,
  offsetHeight: 0,
  scrollHeight: 0,
  scrollWidth: 0,
  scrollLeft: 0,
  scrollTop: 0,
  clientTop: 0,
  clientLeft: 0,
  clientHeight: 0,
  clientWidth: 0,
  selectionStart: 0,
  absoluteTop: 0,
  absoluteLeft: 0,
  marginTop: 0,
  marginBottom: 0,
  marginLeft: 0,
  marginRight: 0,
}

type cooridnates = {
  top: number,
  bottom: number, 
  left: number, 
  right: number 
}

describe ('Overlay maps test', () => {
  const rewModule = rewire('../../../../../components/core/JsInterop/modules/components/overlay');
  const rewClass = rewModule.__get__('Overlay');

  it ('appliedStylePositionMap correct', function () {  
    var placement = Placement.Bottom;
    var actual = rewClass.appliedStylePositionMap.get(placement);
    expect(actual.horizontal).to.equal("left");
    expect(actual.vertical).to.equal("top");
  });

  it ('reverseVerticalPlacementMap correct (position ignored)', function () {  
    var placement = Placement.Bottom;
    var actual = rewClass.reverseVerticalPlacementMap.get(placement)("top");
    expect(actual).to.equal(Placement.Top);    
  });  

  it ('reverseVerticalPlacementMap correct (position evaluated)', function () {  
    var placement = Placement.Left;
    var actual = rewClass.reverseVerticalPlacementMap.get(placement)("top");
    expect(actual).to.equal(Placement.LeftBottom);    
  });    

  it ('reverseHorizontalPlacementMap correct (position ignored)', function () {  
    var placement = Placement.LeftTop;
    var actual = rewClass.reverseHorizontalPlacementMap.get(placement)("top");
    expect(actual).to.equal(Placement.RightTop);    
  });  

  it ('reverseHorizontalPlacementMap correct (position evaluated)', function () {  
    var placement = Placement.Bottom;
    var actual = rewClass.reverseHorizontalPlacementMap.get(placement)("right");
    expect(actual).to.equal(Placement.BottomLeft);    
  });      
});

type positionCalculationTheory = {  
  coversPlacementScenarios: string,
  placement: Placement,
  trigger: { 
    startingPosition?: number,     
    height?: number, 
    width?: number, 
    domInfoResult: domInfo 
  },
  overlay: { height?: number, width?: number },
  position: "top" | "bottom" | "left" | "right",
  container: domInfo,
  overlayConstraints: overlayConstraints,
  expected: { position1: number, position2: number }  
};

const overlayConstraintsDefaults: overlayConstraints = {
  arrowPointAtCenter: false,
  verticalOffset: 4,
  horizontalOffset: 4
};

const theoryVerticalCalculationData: Array<positionCalculationTheory> = [
    { 
      coversPlacementScenarios: "LeftTop & RightTop",
      placement: Placement.LeftTop,      
      position: "top",
      trigger: { startingPosition: 22, height: 19,
        domInfoResult: {
            ...domInfoDefaults,
            absoluteTop: 22,
            clientHeight: 0,
            offsetHeight: 19          
        }
      },
      overlayConstraints: overlayConstraintsDefaults,
      container: domInfoDefaults,
      overlay: { height: 100 },
      expected: { position1: 22, position2: -122 }
    },
    { 
      coversPlacementScenarios: "All Bottom",
      placement: Placement.Bottom,      
      position: "top",
      trigger: { startingPosition: 22, height: 19,
        domInfoResult: {
            ...domInfoDefaults,
            absoluteTop: 22,
            clientHeight: 0,
            offsetHeight: 19          
        }
      },
      overlayConstraints: overlayConstraintsDefaults,
      container: domInfoDefaults,
      overlay: { height: 100 },
      expected: { position1: 45, position2: -145 }
    },
    { 
      coversPlacementScenarios: "Left & Right",
      placement: Placement.Left,      
      position: "top",
      trigger: { startingPosition: 22, height: 19,
        domInfoResult: {
            ...domInfoDefaults,
            absoluteTop: 22,
            clientHeight: 0,
            offsetHeight: 19          
        }
      },
      overlayConstraints: overlayConstraintsDefaults,
      container: domInfoDefaults,
      overlay: { height: 100 },
      expected: { position1: -18.5, position2: -81.5 }
    },
    { 
      coversPlacementScenarios: "All Top",
      placement: Placement.Top,      
      position: "bottom",
      trigger: { startingPosition: -41, height: 19,
        domInfoResult: {
            ...domInfoDefaults,
            absoluteTop: 22,
            clientHeight: 0,
            offsetHeight: 19          
        }
      },
      overlayConstraints: overlayConstraintsDefaults,
      container: domInfoDefaults,
      overlay: { height: 100 },
      expected: { position1: -82, position2: -18 }
    },
    { 
      coversPlacementScenarios: "LeftBottom & RightBottom",
      placement: Placement.LeftBottom,      
      position: "bottom",
      trigger: { startingPosition: -41, height: 19,
        domInfoResult: {
            ...domInfoDefaults,
            absoluteTop: 22,
            clientHeight: 0,
            offsetHeight: 19          
        }
      },
      overlayConstraints: overlayConstraintsDefaults,
      container: domInfoDefaults,
      overlay: { height: 100 },
      expected: { position1: -59, position2: -41 }
    }    
];

const theoryHorizontalCalculationData: Array<positionCalculationTheory> = [
    { 
      coversPlacementScenarios: "TopLeft & BottomLeft",
      placement: Placement.TopLeft,      
      position: "left",
      trigger: { startingPosition: 790, width: 157,
        domInfoResult: {
            ...domInfoDefaults,
            absoluteLeft: 790,
            clientWidth: 0,
            offsetWidth: 157
        }
      },
      overlayConstraints: overlayConstraintsDefaults,
      container: domInfoDefaults,
      overlay: { width: 100 },
      expected: { position1: 790, position2: -890 }
    },
    { 
      coversPlacementScenarios: "All Right",
      placement: Placement.Right,      
      position: "left",
      trigger: { startingPosition: 790, width: 157,
        domInfoResult: {
            ...domInfoDefaults,
            absoluteLeft: 790,
            clientWidth: 0,
            offsetWidth: 157
        }
      },
      overlayConstraints: overlayConstraintsDefaults,
      container: domInfoDefaults,
      overlay: { width: 100 },
      expected: { position1: 951, position2: -1051 }
    },
    { 
      coversPlacementScenarios: "Top & Top & Bottom & Bottom",
      placement: Placement.Top,      
      position: "left",
      trigger: { startingPosition: 790, width: 157,
        domInfoResult: {
            ...domInfoDefaults,
            absoluteLeft: 790,
            clientWidth: 0,
            offsetWidth: 157
        }
      },
      overlayConstraints: overlayConstraintsDefaults,
      container: domInfoDefaults,
      overlay: { width: 100 },
      expected: { position1: 818.5, position2: -918.5 }
    },
    { 
      coversPlacementScenarios: "TopRight & BottomRight",
      placement: Placement.TopRight,      
      position: "right",
      trigger: { startingPosition: -947, width: 157,
        domInfoResult: {
            ...domInfoDefaults,
            absoluteLeft: 790,
            clientWidth: 0,
            offsetWidth: 157
        }
      },
      overlayConstraints: overlayConstraintsDefaults,
      container: domInfoDefaults,
      overlay: { width: 100 },
      expected: { position1: 847, position2: -947 }
    },
    { 
      coversPlacementScenarios: "Left & LeftTop & LeftBottom ",
      placement: Placement.Left,      
      position: "right",
      trigger: { startingPosition: -947, width: 157,
        domInfoResult: {
            ...domInfoDefaults,
            absoluteLeft: 790,
            clientWidth: 0,
            offsetWidth: 157
        }
      },
      overlayConstraints: overlayConstraintsDefaults,
      container: domInfoDefaults,
      overlay: { width: 100 },
      expected: { position1: 686, position2: -786 }
    }    
];

describe ('Overlay calculation functions', () => {
  const rewModule = rewire('../../../../../components/core/JsInterop/modules/components/overlay');
  const rewClass = rewModule.__get__('Overlay');  

  itParam("setVerticalCalculation for position: '${value.position}', placement: ${value.coversPlacementScenarios}", theoryVerticalCalculationData, (testData: positionCalculationTheory) => {
    var fn = rewClass.setVerticalCalculation(testData.placement, testData.position);
    const actual = fn(testData.trigger.startingPosition, testData.trigger.height, testData.container, testData.trigger.domInfoResult, testData.overlay.height, testData.overlayConstraints);
    expect(actual.top).to.equal(testData.expected.position1);
    expect(actual.bottom).to.equal(testData.expected.position2);
  });

  itParam("setHorizontalCalculation for position: '${value.position}', placement: ${value.coversPlacementScenarios}", theoryHorizontalCalculationData, (testData: positionCalculationTheory) => {
    var fn = rewClass.setHorizontalCalculation(testData.placement, testData.position);
    const actual = fn(testData.trigger.startingPosition, testData.trigger.width, testData.container, testData.trigger.domInfoResult, testData.overlay.width, testData.overlayConstraints);
    expect(actual.left).to.equal(testData.expected.position1);
    expect(actual.right).to.equal(testData.expected.position2);
  });   
});

type positionTheory = {
  theoryName: string,
  trigger: { triggerBoundyAdjustMode: TriggerBoundyAdjustMode },
  overlay: { placement: Placement },
  window: { innerHeight: number, innerWidth: number, pageXOffset: number, pageYOffset: number},  
  documentElement: { clientHeight: number, clientWidth: number },
  getInfo: { [elementId: string]: { 
    absoluteTop?: number, 
    absoluteLeft?: number, 
    clientLeft?: number, 
    clientTop?: number    
    clientHeight?: number,
    clientWidth?: number, 
    offsetHeight?: number, 
    offsetWidth?: number,
    scrollLeft?: number, 
    scrollTop?: number, 
    scrollHeight?: number, 
    scrollWidth?: number}},    
  containerParentElement?: {
    clientHeight?: number,
    clientWidth?: number,
    scrollLeft?: number,
    scrollTop?: number,
    zIndex?: number
  },    
  expected: cooridnates
  expectedCorrected: cooridnates & { placement?: Placement }
};

const theoryPositionData: Array<positionTheory> = [
    { 
      theoryName: "scenario 1",
      trigger: { triggerBoundyAdjustMode: TriggerBoundyAdjustMode.InView },
      overlay: { placement: Placement.BottomLeft },
      window: { innerWidth: 1920, innerHeight: 955, pageXOffset: 0, pageYOffset: 0},
      documentElement: { clientHeight: 955, clientWidth: 1920 },
      getInfo: {
        "body": { absoluteTop: 0, absoluteLeft: 0, clientLeft: 0, clientTop: 0, scrollLeft: 0, scrollTop: 0, scrollHeight: 955, scrollWidth: 1920},
        "trigger": { absoluteTop: 834, absoluteLeft: 33, clientHeight: 0, clientWidth: 0, offsetHeight: 19, offsetWidth: 157},
        "overlay": { clientHeight: 808, clientWidth: 372 },
      },       
       expected: { top: 857, bottom: -710, left: 33, right: 1515 },
       expectedCorrected: { top: null, bottom: 125, left: 33, right: null, placement: Placement.TopLeft }
    },
    { 
      theoryName: "scenario 2",
      trigger: { triggerBoundyAdjustMode: TriggerBoundyAdjustMode.InView },
      overlay: { 
        placement: Placement.BottomLeft },
      window: { innerWidth: 1920, innerHeight: 955, pageXOffset: 0, pageYOffset: 0},
      documentElement: { clientHeight: 955, clientWidth: 1920 },
      getInfo: {
        "body": { absoluteTop: 0, absoluteLeft: 0, clientLeft: 0, clientTop: 0, scrollLeft: 0, scrollTop: 0, scrollHeight: 955, scrollWidth: 1920},
        "trigger": { absoluteTop: 42, absoluteLeft: 1738, clientHeight: 0, clientWidth: 0, offsetHeight: 19, offsetWidth: 157},
        "overlay": { clientHeight: 808, clientWidth: 372 },
      },       
       expected: { top: 65, bottom: 82, left: 1738, right: -190 },
       expectedCorrected: { top: 65, bottom: null, left: null, right: 25, placement: Placement.BottomRight }
    },
    { 
      theoryName: "scenario 3",
      trigger: { triggerBoundyAdjustMode: TriggerBoundyAdjustMode.InView },
      overlay: { 
        placement: Placement.BottomLeft },
      window: { innerWidth: 1920, innerHeight: 955, pageXOffset: 0, pageYOffset: 0},
      documentElement: { clientHeight: 955, clientWidth: 1920 },
      getInfo: {
        "body": { absoluteTop: 0, absoluteLeft: 0, clientLeft: 0, clientTop: 0, scrollLeft: 0, scrollTop: 0, scrollHeight: 955, scrollWidth: 1920},
        "trigger": { absoluteTop: 438, absoluteLeft: 1738, clientHeight: 0, clientWidth: 0, offsetHeight: 19, offsetWidth: 157},
        "overlay": { clientHeight: 808, clientWidth: 372 },
      },       
       expected: { top: 461, bottom: -314, left: 1738, right: -190 },
       expectedCorrected: { top: 461, bottom: null, left: null, right: 25, placement: Placement.BottomRight }
    },
    { 
      theoryName: "scenario 4",
      trigger: { triggerBoundyAdjustMode: TriggerBoundyAdjustMode.InView },
      overlay: { 
        placement: Placement.BottomLeft },
      window: { innerWidth: 1920, innerHeight: 955, pageXOffset: 1920, pageYOffset: 955},
      documentElement: { clientHeight: 0, clientWidth: 0 },
      getInfo: {
        "body": { absoluteTop: 0, absoluteLeft: 0, clientLeft: 0, clientTop: 0, scrollLeft: 0, scrollTop: 0, scrollHeight: 2883, scrollWidth: 5816},
        "trigger": { absoluteTop: 1386, absoluteLeft: 2785, clientHeight: 0, clientWidth: 0, offsetHeight: 22, offsetWidth: 74},
        "overlay": { clientHeight: 744, clientWidth: 372 },
      },       
       expected: { top: 1412, bottom: 727, left: 2785, right: 2659 },
       expectedCorrected: { top: 1412, bottom: null, left: 2785, right: null, placement: Placement.BottomLeft }
    },
    { 
      theoryName: "scenario 5",
      trigger: { triggerBoundyAdjustMode: TriggerBoundyAdjustMode.InView },
      overlay: { 
        placement: Placement.BottomLeft },
      window: { innerWidth: 1920, innerHeight: 955, pageXOffset: 64, pageYOffset: 41},
      documentElement: { clientHeight: 938, clientWidth: 1903 },
      getInfo: {
        "body": { absoluteTop: 0, absoluteLeft: 0, clientLeft: 0, clientTop: 0, clientHeight: 2865, clientWidth: 5760, offsetHeight: 2865, offsetWidth: 5760, scrollLeft: 0, scrollTop: 0, scrollHeight: 2883, scrollWidth: 5816},
        "trigger": { absoluteTop: 966, absoluteLeft: 1940, clientHeight: 0, clientWidth: 0, offsetHeight: 22, offsetWidth: 74},
        "overlay": { clientHeight: 744, clientWidth: 372 },
      },       
       expected: { top: 992, bottom: 1147, left: 1940, right: 3504 },
       expectedCorrected: { top: null, bottom: -24, left: null, right: -111, placement: Placement.TopRight }
    },
    { 
      theoryName: "scenario 6",
      trigger: { triggerBoundyAdjustMode: TriggerBoundyAdjustMode.InView },
      overlay: { 
        placement: Placement.Left },
      window: { innerWidth: 1920, innerHeight: 955, pageXOffset: 0, pageYOffset: 0},
      documentElement: { clientHeight: 955, clientWidth: 1920 },
      getInfo: {
        "body": { absoluteTop: 0, absoluteLeft: 0, clientLeft: 0, clientTop: 0, clientHeight: 955, clientWidth: 1920, offsetHeight: 955, offsetWidth: 1920, scrollLeft: 0, scrollTop: 0, scrollHeight: 955, scrollWidth: 1920},
        "trigger": { absoluteTop: 810, absoluteLeft: 885, clientHeight: 0, clientWidth: 0, offsetHeight: 22, offsetWidth: 74},
        "overlay": { clientHeight: 744, clientWidth: 372 },
      },       
       expected: { top: 449, bottom: -238, left: 509, right: 1039 },
       expectedCorrected: { top: null, bottom: 123, left: null, right: 1039, placement: Placement.LeftBottom }
    },
    { 
      theoryName: "scenario 7 tooltip left",
      trigger: { triggerBoundyAdjustMode: TriggerBoundyAdjustMode.InView },
      overlay: { 
        placement: Placement.Left },
      window: { innerWidth: 1920, innerHeight: 955, pageXOffset: 0, pageYOffset: 0},
      documentElement: { clientHeight: 955, clientWidth: 1920 },
      getInfo: {
        "body": { absoluteTop: 0, absoluteLeft: 0, clientLeft: 0, clientTop: 0, clientHeight: 955, clientWidth: 1920, offsetHeight: 955, offsetWidth: 1920, scrollLeft: 0, scrollTop: 0, scrollHeight: 955, scrollWidth: 1920},
        "trigger": { absoluteTop: 138, absoluteLeft: 282, clientHeight: 32, clientWidth: 55, offsetHeight: 32, offsetWidth: 55},
        "overlay": { clientHeight: 34, clientWidth: 97 },
      },       
       expected: { top: 137, bottom: 784, left: 181, right: 1642 },
       expectedCorrected: { top: 137, bottom: null, left: null, right: 1642, placement: Placement.Left }
    },
    { 
      theoryName: "scenario 8 dropDown button BottomRight",
      trigger: { triggerBoundyAdjustMode: TriggerBoundyAdjustMode.InView },
      overlay: { 
        placement: Placement.BottomRight },
      window: { innerWidth: 1903, innerHeight: 955, pageXOffset: 0, pageYOffset: 0},
      documentElement: { clientHeight: 955, clientWidth: 1903 },
      getInfo: {
        "body": { absoluteTop: 0, absoluteLeft: 0, clientLeft: 0, clientTop: 0, clientHeight: 955, clientWidth: 1903, offsetHeight: 955, offsetWidth: 1903, scrollLeft: 0, scrollTop: 0, scrollHeight: 5559, scrollWidth: 1903},
        "trigger": { absoluteTop: 705, absoluteLeft: 1318, clientHeight: 30, clientWidth: 108, offsetHeight: 32, offsetWidth: 110},
        "overlay": { clientHeight: 104, clientWidth: 118 },
      },       
       expected: { top: 741, bottom: 4714, left: 1310, right: 475 },
       expectedCorrected: { top: 741, bottom: null, left: null, right: 475, placement: Placement.BottomRight }
    },
    { 
      theoryName: "scenario 9 container topRight",
      trigger: { triggerBoundyAdjustMode: TriggerBoundyAdjustMode.InView },
      overlay: { 
        placement: Placement.TopRight },
      window: { innerWidth: 1920, innerHeight: 955, pageXOffset: 0, pageYOffset: 1836},
      documentElement: { clientHeight: 955, clientWidth: 1903 },
      getInfo: {
        "container": { absoluteTop: 2018, absoluteLeft: 416, clientLeft: 0, clientTop: 0, clientHeight: 1000, clientWidth: 581, offsetHeight: 1000, offsetWidth: 581, scrollLeft: 0, scrollTop: 0, scrollHeight: 1000, scrollWidth: 581},
        "trigger": { absoluteTop: 2150, absoluteLeft: 696, clientHeight: 30, clientWidth: 84, offsetHeight: 32, offsetWidth: 86},
        "overlay": { clientHeight: 104, clientWidth: 118 },
      },       
      containerParentElement: {
        clientWidth: 581,
        clientHeight: 183,
        scrollLeft: 0,
        scrollTop: 114
      },
       expected: { top: 24, bottom: 872, left: 248, right: 215 },
       expectedCorrected: { top: 168, bottom: null, left: null, right: 215, placement: Placement.BottomRight }
    },
    { 
      theoryName: "popup in container (based on autocomplete)",
      trigger: { triggerBoundyAdjustMode: TriggerBoundyAdjustMode.InView },
      overlay: { 
        placement: Placement.BottomLeft },
      window: { innerWidth: 1920, innerHeight: 955, pageXOffset: 0, pageYOffset: 1137.5999755859375},
      documentElement: { clientHeight: 955, clientWidth: 1903 },
      getInfo: {
        "container": { absoluteTop: 1701, absoluteLeft: 416, clientLeft: 0, clientTop: 0, clientHeight: 1000, clientWidth: 581, offsetHeight: 1000, offsetWidth: 581, scrollLeft: 0, scrollTop: 0, scrollHeight: 1000, scrollWidth: 581},
        "trigger": { absoluteTop: 1801, absoluteLeft: 516, clientHeight: 30, clientWidth: 379, offsetHeight: 32, offsetWidth: 381},
        "overlay": { clientHeight: 200, clientWidth: 379 },
      },       
      containerParentElement: {
        clientWidth: 581,
        clientHeight: 183,
        scrollLeft: 0,
        scrollTop: 0
      },
       expected: { top: 136, bottom: 664, left: 100, right: 102 },
       expectedCorrected: { top: null, bottom: 904, left: 100, right: null, placement: Placement.TopLeft }
    },    
    { 
      theoryName: "scenario 11 trigger not in viewport (tooltip)",
      trigger: { triggerBoundyAdjustMode: TriggerBoundyAdjustMode.InView },
      overlay: { 
        placement: Placement.Top },
      window: { innerWidth: 1920, innerHeight: 955, pageXOffset: 0, pageYOffset: 0},
      documentElement: { clientHeight: 955, clientWidth: 1903 },
      getInfo: {
        "body": { absoluteTop: 0, absoluteLeft: 0, clientLeft: 0, clientTop: 0, scrollLeft: 0, scrollTop: 0, scrollHeight: 3983, scrollWidth: 1903},
        "trigger": { absoluteTop: 1588, absoluteLeft: 559, clientHeight: 10, clientWidth: 10, offsetHeight: 14, offsetWidth: 14},
        "overlay": { clientHeight: 42, clientWidth: 32 },
      },       
       expected: { top: 1542, bottom: 2399, left: 550, right: 1321 },
       expectedCorrected: { top: null, bottom: -629, left: 550, right: null, placement: Placement.Top }
    },
    { 
      theoryName: "trigger scrolled parttially  out of screen",
      trigger: { triggerBoundyAdjustMode: TriggerBoundyAdjustMode.InView },
      overlay: { 
        placement: Placement.BottomLeft },
      window: { innerWidth: 1920, innerHeight: 955, pageXOffset: 0, pageYOffset: 934},
      documentElement: { clientHeight: 955, clientWidth: 1903 },
      getInfo: {
        "body": { absoluteTop: 0, absoluteLeft: 0, clientLeft: 0, clientTop: 0, scrollLeft: 0, scrollTop: 0, scrollHeight: 9505, scrollWidth: 1903},
        "trigger": { absoluteTop: 869, absoluteLeft: 406, clientHeight: 30, clientWidth: 136, offsetHeight: 32, offsetWidth: 138},
        "overlay": { clientHeight: 309, clientWidth: 280 },
      },       
       expected: { top: 905, bottom: 8291, left: 406, right: 1217 },
       expectedCorrected: { top: 905, bottom: null, left: 406, right: null, placement: Placement.BottomLeft }
    }
  ];  

type scrollTheory = positionTheory & {
  scrollSteps: Array<{x?: number, y?: number, expectedAfterScroll: cooridnates & { placement?: Placement } }>,    
}

const theoryScrollData: Array<scrollTheory> = [
    { 
      theoryName: "trigger scrolled parttially  out of screen",
      trigger: { triggerBoundyAdjustMode: TriggerBoundyAdjustMode.InView },
      overlay: { 
        placement: Placement.BottomLeft },
      window: { innerWidth: 1920, innerHeight: 955, pageXOffset: 0, pageYOffset: 0},
      documentElement: { clientHeight: 955, clientWidth: 1903 },
      getInfo: {
        "body": { absoluteTop: 0, absoluteLeft: 0, clientLeft: 0, clientTop: 0, scrollLeft: 0, scrollTop: 0, scrollHeight: 9505, scrollWidth: 1903},
        "trigger": { absoluteTop: 439, absoluteLeft: 406, clientHeight: 30, clientWidth: 136, offsetHeight: 32, offsetWidth: 138},
        "overlay": { clientHeight: 309, clientWidth: 280 },
      },       
      expected: { top: 475, bottom: 8721, left: 406, right: 1217 },
      expectedCorrected: { top: 475, bottom: null, left: 406, right: null, placement: Placement.BottomLeft },
      scrollSteps: [ 
        {
          y: 476,
          expectedAfterScroll: { top: 475, bottom: null, left: 406, right: null, placement: Placement.BottomLeft }
        }, 
        {
          y: 477,
          expectedAfterScroll: { top: 475, bottom: null, left: 406, right: null, placement: Placement.BottomLeft }           
        }, 
        {
          y:0,
          expectedAfterScroll: { top: 475, bottom: null, left: 406, right: null, placement: Placement.BottomLeft }
        } 
        ]
    }
]

const theoryModalPositionData: Array<positionTheory> = [
    { 
      theoryName: "scenario 1 container is modal",
      trigger: { triggerBoundyAdjustMode: TriggerBoundyAdjustMode.InView },
      overlay: { 
        placement: Placement.BottomLeft },
      window: { innerWidth: 1920, innerHeight: 955, pageXOffset: 0, pageYOffset: 2346},
      documentElement: { clientHeight: 955, clientWidth: 1920 },
      getInfo: {
        "container": { absoluteTop: 2346, absoluteLeft: 0, clientLeft: 0, clientTop: 0, clientHeight: 955, clientWidth: 1920, offsetHeight: 955, offsetWidth: 1920, scrollLeft: 0, scrollTop: 0, scrollHeight: 955, scrollWidth: 1920},
        "trigger": { absoluteTop: 2667, absoluteLeft: 724, clientHeight: 0, clientWidth: 0, offsetHeight: 19, offsetWidth: 74},
        "overlay": { clientHeight: 40, clientWidth: 113 },
      },       
      containerParentElement: {
        clientWidth: 1903,
        clientHeight: 0,
        scrollLeft: 0,
        scrollTop: 0,
        zIndex: 1000
      },
       expected: { top: 344, bottom: 571, left: 724, right: 1083 },
       expectedCorrected: { top: 344, bottom: null, left: 724, right: null, placement: Placement.BottomLeft }
    },
    { 
      theoryName: "modal on page top with overlay fitting better abover trigger but not fitting in body",
      trigger: { triggerBoundyAdjustMode: TriggerBoundyAdjustMode.InView },
      overlay: { 
        placement: Placement.BottomLeft },
      window: { innerWidth: 1920, innerHeight: 955, pageXOffset: 0, pageYOffset: 0},
      documentElement: { clientHeight: 955, clientWidth: 1920 },
      getInfo: {
        "container": { absoluteTop: 100, absoluteLeft: 700, clientLeft: 0, clientTop: 0, clientHeight: 279, clientWidth: 520, offsetHeight: 279, offsetWidth: 520, scrollLeft: 0, scrollTop: 0, scrollHeight: 279, scrollWidth: 520},
        "trigger": { absoluteTop: 250, absoluteLeft: 803, clientHeight: 22, clientWidth: 128, offsetHeight: 24, offsetWidth: 130},
        "overlay": { clientHeight: 311, clientWidth: 280 },
      },       
      containerParentElement: {
        clientWidth: 520,
        clientHeight: 303,
        scrollLeft: 0,
        scrollTop: 0,
        zIndex: 1000
      },
       expected: { top: 178, bottom: -210, left: 103, right: 137 },
       expectedCorrected: { top: 178, bottom: null, left: 103, right: null, placement: Placement.BottomLeft }
    },    
];

describe('Overlay position functions', () => {        
  const rewModule = rewire('../../../../../components/core/JsInterop/modules/components/overlay');
  const rewClass = rewModule.__get__('Overlay');  

  let sandbox;
  before(() => sandbox = sinon.createSandbox());
  afterEach(() => sandbox.restore());  

  itParam("getVerticalPosition & getHorizontalPosition: ${value.theoryName}", theoryPositionData, (testData: positionTheory) => {
    //arrange
    let container;
    let containerParent;
    let triggerElement;
    const triggerElementHtml = '<div id="trigger">TriggerElement</div>';
    if (testData.getInfo.hasOwnProperty("body")) {
      container = document.body;    
      triggerElement = domInit.addElementToBody(triggerElementHtml);                  
    } else {            
      let containerHtml: string = '<div id="container"></div>'
      if (testData.containerParentElement) {
        containerHtml = `<div id="containerParent">${containerHtml}</div>`
        containerParent = domInit.addElementToBody(containerHtml)
        container = containerParent.firstChild;
        const c1 = sandbox.stub(container, 'parentElement').get(() => testData.containerParentElement);
      } else {
        container = domInit.addElementToBody(containerHtml)
      }
      container.insertAdjacentHTML("afterbegin", triggerElementHtml);
      triggerElement = container.firstChild;
    }

    const overlayElement = domInit.addElementToBody('<div id="overlay" style="position:absolute"></div>') as HTMLDivElement;  

    //discard variable names    
    const w1 = sandbox.stub(window, 'innerHeight').get(() => testData.window.innerHeight);
    const w2 = sandbox.stub(window, 'innerWidth').get(() => testData.window.innerWidth);
    const w3 = sandbox.stub(window, 'pageXOffset').get(() => testData.window.pageXOffset);
    const w4 = sandbox.stub(window, 'pageYOffset').get(() => testData.window.pageYOffset);      
    const d1 = sandbox.stub(document.documentElement, 'clientHeight').get(() => testData.documentElement.clientHeight);
    const d2 = sandbox.stub(document.documentElement, 'clientWidth').get(() => testData.documentElement.clientWidth);
    const o1 = sandbox.stub(resizeObserver, 'create').callsFake((key, invoker, isDotNetInvoker: boolean) => null);
    const o2 = sandbox.stub(mutationObserver, 'create').callsFake((key, invoker, isDotNetInvoker: boolean) => null);    

    const s = sandbox.stub(infoHelper, "getInfo").callsFake((element: HTMLElement) => {      
      let elementId = element.getAttribute("id");
      if (!elementId && element === document.body) {
        elementId = "body";
      }
      return testData.getInfo[elementId];
    }); 

    //act
    let overlay = new rewClass("testId", overlayElement, container, triggerElement, testData.overlay.placement, testData.trigger.triggerBoundyAdjustMode, false, '', overlayConstraintsDefaults);
    overlay.getKeyElementDimensions();
    let verticalPosition = overlay.getVerticalPosition();
    let horizontalPosition = overlay.getHorizontalPosition();
    //assert
    expect(verticalPosition.top).to.eq(testData.expected.top);    
    expect(verticalPosition.bottom).to.eq(testData.expected.bottom);
    expect(horizontalPosition.left).to.eq(testData.expected.left);
    expect(horizontalPosition.right).to.eq(testData.expected.right);
  });
});

describe('Overlay calculation', () => {        

  let sandbox;
  before(() => sandbox = sinon.createSandbox());
  afterEach(() => sandbox.restore());  

  itParam("should contain calculated values: ${value.theoryName}", theoryPositionData, (testData: positionTheory) => {
    //arrange
    let container;
    let containerParent;
    let triggerElement;
    const triggerElementHtml = '<div id="trigger">TriggerElement</div>';
    if (testData.getInfo.hasOwnProperty("body")) {
      container = document.body;    
      triggerElement = domInit.addElementToBody(triggerElementHtml);                  
    } else {            
      let containerHtml: string = '<div id="container"></div>'
      if (testData.containerParentElement) {        
        containerHtml = `<div id="containerParent">${containerHtml}</div>`
        containerParent = domInit.addElementToBody(containerHtml)
        container = containerParent.firstChild;

        const c1 = sandbox.stub(container.parentElement, 'clientHeight').get(() => testData.containerParentElement.clientHeight);
        const c2 = sandbox.stub(container.parentElement, 'clientWidth').get(() => testData.containerParentElement.clientWidth);
        const c3 = sandbox.stub(container.parentElement, 'scrollLeft').get(() => testData.containerParentElement.scrollLeft);
        const c4 = sandbox.stub(container.parentElement, 'scrollTop').get(() => testData.containerParentElement.scrollTop);   
        const c5 = sandbox.stub(infoHelper, 'findAncestorWithZIndex').callsFake((element: HTMLElement) => testData.containerParentElement.zIndex);
      } else {
        container = domInit.addElementToBody(containerHtml)
      }
      container.insertAdjacentHTML("afterbegin", triggerElementHtml);
      triggerElement = container.firstChild;
    }

    const overlayElement = domInit.addElementToBody('<div id="overlay" style="position:absolute"></div>') as HTMLDivElement;

    //discard variable names    
    const w1 = sandbox.stub(window, 'innerHeight').get(() => testData.window.innerHeight);
    const w2 = sandbox.stub(window, 'innerWidth').get(() => testData.window.innerWidth);
    const w3 = sandbox.stub(window, 'pageXOffset').get(() => testData.window.pageXOffset);
    const w4 = sandbox.stub(window, 'pageYOffset').get(() => testData.window.pageYOffset);      
    const w6 = sandbox.stub(triggerElement, 'offsetParent').get(() => 1);  
    const d1 = sandbox.stub(document.documentElement, 'clientHeight').get(() => testData.documentElement.clientHeight);
    const d2 = sandbox.stub(document.documentElement, 'clientWidth').get(() => testData.documentElement.clientWidth);

    const o1 = sandbox.stub(resizeObserver, 'create').callsFake((key, invoker, isDotNetInvoker: boolean) => null);
    const o2 = sandbox.stub(mutationObserver, 'create').callsFake((key, invoker, isDotNetInvoker: boolean) => null);    

    const s7 = sandbox.stub(infoHelper, "getInfo").callsFake((element: HTMLElement) => {      
      let elementId = element.getAttribute("id");
      if (!elementId && element === document.body) {
        elementId = "body";
      }
      return testData.getInfo[elementId];
    }); 

    //act
    let overlay = new Overlay("testId", overlayElement, container, triggerElement, testData.overlay.placement, testData.trigger.triggerBoundyAdjustMode, false, '', overlayConstraintsDefaults);
    overlay.calculatePosition(false, true);    
    //assert
    if (testData.expectedCorrected.top !== null) {
      expect(overlay.sanitizedPosition.top).to.eq(testData.expectedCorrected.top);    
      expect(overlay.sanitizedPosition.bottom).to.eq(null);    
    }
    else {
      expect(overlay.sanitizedPosition.bottom).to.eq(testData.expectedCorrected.bottom);    
      expect(overlay.sanitizedPosition.top).to.eq(null);    
    }
    if (testData.expectedCorrected.left !== null) {
      expect(overlay.sanitizedPosition.left).to.eq(testData.expectedCorrected.left);
      expect(overlay.sanitizedPosition.right).to.eq(null);
    }
    else {
      expect(overlay.sanitizedPosition.right).to.eq(testData.expectedCorrected.right);
      expect(overlay.sanitizedPosition.left).to.eq(null);
    }
    expect(overlay.sanitizedPosition.placement).to.eq(testData.expectedCorrected.placement);
  });

  itParam("(modal) should contain calculated values: ${value.theoryName}", theoryModalPositionData, (testData: positionTheory) => {
    //arrange
    let container;
    let containerParent;
    let triggerElement;
    const triggerElementHtml = '<div id="trigger">TriggerElement</div>';
    let containerHtml: string = '<div id="container" class="ant-modal-wrap"></div>'
    containerHtml = `<div id="containerParent" class="ant-modal-root">${containerHtml}</div>`
    containerParent = domInit.addElementToBody(containerHtml)
    container = containerParent.firstChild;
    const c1 = sandbox.stub(container.parentElement, 'clientHeight').get(() => testData.containerParentElement.clientHeight);
    const c2 = sandbox.stub(container.parentElement, 'clientWidth').get(() => testData.containerParentElement.clientWidth);
    const c3 = sandbox.stub(container.parentElement, 'scrollLeft').get(() => testData.containerParentElement.scrollLeft);
    const c4 = sandbox.stub(container.parentElement, 'scrollTop').get(() => testData.containerParentElement.scrollTop);        
    const c5 = sandbox.stub(infoHelper, 'findAncestorWithZIndex').callsFake((element: HTMLElement) => testData.containerParentElement.zIndex);    

    container.insertAdjacentHTML("afterbegin", triggerElementHtml);
    triggerElement = container.firstChild;

    const overlayElement = domInit.addElementToBody('<div id="overlay" style="position:absolute"></div>') as HTMLDivElement;

    //discard variable names    
    const w1 = sandbox.stub(window, 'innerHeight').get(() => testData.window.innerHeight);
    const w2 = sandbox.stub(window, 'innerWidth').get(() => testData.window.innerWidth);
    const w3 = sandbox.stub(window, 'pageXOffset').get(() => testData.window.pageXOffset);
    const w4 = sandbox.stub(window, 'pageYOffset').get(() => testData.window.pageYOffset);
    const w6 = sandbox.stub(triggerElement, 'offsetParent').get(() => 1);
    const d1 = sandbox.stub(document.documentElement, 'clientHeight').get(() => testData.documentElement.clientHeight);
    const d2 = sandbox.stub(document.documentElement, 'clientWidth').get(() => testData.documentElement.clientWidth);

    const o1 = sandbox.stub(resizeObserver, 'create').callsFake((key, invoker, isDotNetInvoker: boolean) => null);
    const o2 = sandbox.stub(mutationObserver, 'create').callsFake((key, invoker, isDotNetInvoker: boolean) => null);  
    
    const s7 = sandbox.stub(infoHelper, "getInfo").callsFake((element: HTMLElement) => {      
      let elementId = element.getAttribute("id");
      if (!elementId && element === document.body) {
        elementId = "body";
      }
      return testData.getInfo[elementId];
    }); 

    //act
    let overlay = new Overlay("testId", overlayElement, container, triggerElement, testData.overlay.placement, testData.trigger.triggerBoundyAdjustMode, false, '', overlayConstraintsDefaults);
    overlay.calculatePosition(false, true);    
    //assert
    if (testData.expectedCorrected.top !== null) {
      expect(overlay.sanitizedPosition.top).to.eq(testData.expectedCorrected.top);    
      expect(overlay.sanitizedPosition.bottom).to.eq(null);    
    }
    else {
      expect(overlay.sanitizedPosition.bottom).to.eq(testData.expectedCorrected.bottom);    
      expect(overlay.sanitizedPosition.top).to.eq(null);    
    }
    if (testData.expectedCorrected.left !== null) {
      expect(overlay.sanitizedPosition.left).to.eq(testData.expectedCorrected.left);
      expect(overlay.sanitizedPosition.right).to.eq(null);
    }
    else {
      expect(overlay.sanitizedPosition.right).to.eq(testData.expectedCorrected.right);
      expect(overlay.sanitizedPosition.left).to.eq(null);
    }
    expect(overlay.sanitizedPosition.placement).to.eq(testData.expectedCorrected.placement);
  })  

  itParam("scrollScenarios: ${value.theoryName}", theoryScrollData, (testData: scrollTheory) => {
    //arrange
    let container;
    let containerParent;
    let triggerElement;
    const triggerElementHtml = '<div id="trigger">TriggerElement</div>';
    if (testData.getInfo.hasOwnProperty("body")) {
      container = document.body;    
      triggerElement = domInit.addElementToBody(triggerElementHtml);                  
    } else {            
      let containerHtml: string = '<div id="container"></div>'
      if (testData.containerParentElement) {
        containerHtml = `<div id="containerParent">${containerHtml}</div>`
        containerParent = domInit.addElementToBody(containerHtml)
        container = containerParent.firstChild;

        const c1 = sandbox.stub(container.parentElement, 'clientHeight').get(() => testData.containerParentElement.clientHeight);
        const c2 = sandbox.stub(container.parentElement, 'clientWidth').get(() => testData.containerParentElement.clientWidth);
        const c3 = sandbox.stub(container.parentElement, 'scrollLeft').get(() => testData.containerParentElement.scrollLeft);
        const c4 = sandbox.stub(container.parentElement, 'scrollTop').get(() => testData.containerParentElement.scrollTop);   
      } else {
        container = domInit.addElementToBody(containerHtml)
      }
      container.insertAdjacentHTML("afterbegin", triggerElementHtml);
      triggerElement = container.firstChild;
    }

    const overlayElement = domInit.addElementToBody('<div id="overlay" style="position:absolute"></div>') as HTMLDivElement;
    let pageXOffset = testData.window.pageXOffset;
    let pageYOffset = testData.window.pageYOffset;
    //discard variable names    
    const w1 = sandbox.stub(window, 'innerHeight').get(() => testData.window.innerHeight);
    const w2 = sandbox.stub(window, 'innerWidth').get(() => testData.window.innerWidth);
    const w3 = sandbox.stub(window, 'pageXOffset').get(() => pageXOffset);
    const w4 = sandbox.stub(window, 'pageYOffset').get(() => pageYOffset);      
    const w5 = sandbox.stub(overlayElement, 'offsetParent').get(() => 1); //not null and > 0 is needed   
    const w6 = sandbox.stub(triggerElement, 'offsetParent').get(() => 1);  
    const d1 = sandbox.stub(document.documentElement, 'clientHeight').get(() => testData.documentElement.clientHeight);
    const d2 = sandbox.stub(document.documentElement, 'clientWidth').get(() => testData.documentElement.clientWidth);

    const o1 = sandbox.stub(resizeObserver, 'create').callsFake((key, invoker, isDotNetInvoker: boolean) => null);
    const o2 = sandbox.stub(mutationObserver, 'create').callsFake((key, invoker, isDotNetInvoker: boolean) => null);    

    const s7 = sandbox.stub(infoHelper, "getInfo").callsFake((element: HTMLElement) => {      
      let elementId = element.getAttribute("id");
      if (!elementId && element === document.body) {
        elementId = "body";
      }
      return testData.getInfo[elementId];
    }); 

    //act
    let overlay = new Overlay("testId", overlayElement, container, triggerElement, testData.overlay.placement, testData.trigger.triggerBoundyAdjustMode, false, '', overlayConstraintsDefaults);
    overlay.calculatePosition(false, true);

    //assert
    if (testData.expectedCorrected.top !== null) {
      expect(overlay.sanitizedPosition.top).to.eq(testData.expectedCorrected.top);    
      expect(overlay.sanitizedPosition.bottom).to.eq(null);    
    }
    else {
      expect(overlay.sanitizedPosition.bottom).to.eq(testData.expectedCorrected.bottom);    
      expect(overlay.sanitizedPosition.top).to.eq(null);    
    }
    if (testData.expectedCorrected.left !== null) {
      expect(overlay.sanitizedPosition.left).to.eq(testData.expectedCorrected.left);
      expect(overlay.sanitizedPosition.right).to.eq(null);
    }
    else {
      expect(overlay.sanitizedPosition.right).to.eq(testData.expectedCorrected.right);
      expect(overlay.sanitizedPosition.left).to.eq(null);
    }
    expect(overlay.sanitizedPosition.placement).to.eq(testData.expectedCorrected.placement);
    
    for(let i = 0; i < testData.scrollSteps.length; i++) {
      let scrollStep = testData.scrollSteps[i];
      pageXOffset = scrollStep.x ?? pageXOffset;
      pageYOffset = scrollStep.y ?? pageYOffset;
      overlay.calculatePosition(true, false, null);

      if (scrollStep.expectedAfterScroll.top !== null) {
        expect(overlay.sanitizedPosition.top).to.eq(scrollStep.expectedAfterScroll.top);    
        expect(overlay.sanitizedPosition.bottom).to.eq(null);    
      }
      else {
        expect(overlay.sanitizedPosition.bottom).to.eq(scrollStep.expectedAfterScroll.bottom);    
        expect(overlay.sanitizedPosition.top).to.eq(null);    
      }
      if (scrollStep.expectedAfterScroll.left !== null) {
        expect(overlay.sanitizedPosition.left).to.eq(scrollStep.expectedAfterScroll.left);
        expect(overlay.sanitizedPosition.right).to.eq(null);
      }
      else {
        expect(overlay.sanitizedPosition.right).to.eq(scrollStep.expectedAfterScroll.right);
        expect(overlay.sanitizedPosition.left).to.eq(null);
      }
      expect(overlay.sanitizedPosition.placement).to.eq(scrollStep.expectedAfterScroll.placement);

    }
  });  
});

type resizeTheory = positionTheory & {
  contentRect: { height: number, width: number }
}

const theoryResizeData: Array<resizeTheory> = [
    { 
      theoryName: "scenario 1",
      trigger: { triggerBoundyAdjustMode: TriggerBoundyAdjustMode.InView },
      overlay: { 
        placement: Placement.BottomLeft },
      window: { innerWidth: 1920, innerHeight: 955, pageXOffset: 0, pageYOffset: 0},
      documentElement: { clientHeight: 900, clientWidth: 1000 },
      getInfo: {
        "body": { absoluteTop: 0, absoluteLeft: 0, clientLeft: 0, clientTop: 0, scrollLeft: 0, scrollTop: 0, scrollHeight: 955, scrollWidth: 1920},
        "trigger": { absoluteTop: 22, absoluteLeft: 790, clientHeight: 0, clientWidth: 0, offsetHeight: 19, offsetWidth: 157},
        "overlay": { clientHeight: 808, clientWidth: 372 },
      },       
      contentRect: {height: 900, width: 1000},
      expected: { top: null, left: null, right: null, bottom: null },
      expectedCorrected: { top: 45, left: null, right: 53, bottom: null, placement: Placement.BottomRight }
    }
];

describe('Overlay recalculation on container resize', () => {
  let sandbox;
  before(() => sandbox = sinon.createSandbox());
  afterEach(() => sandbox.restore());  

  itParam("resize observer should calculate new position", theoryResizeData, (testData: resizeTheory) => {
    //arrange
    const triggerElement = domInit.addElementToBody('<div id="trigger">TriggerElement</div>');            
    const overlayElement = domInit.addElementToBody('<div id="overlay" style="position:absolute"></div>') as HTMLDivElement;
    const container = document.body;    

    //discard variable names    
    let inResize = false;
    const w1 = sandbox.stub(window, 'innerHeight').get(() => testData.window.innerHeight);
    const w2 = sandbox.stub(window, 'innerWidth').get(() => testData.window.innerWidth);
    const w3 = sandbox.stub(window, 'pageXOffset').get(() => testData.window.pageXOffset);
    const w4 = sandbox.stub(window, 'pageYOffset').get(() => testData.window.pageYOffset);
    const w5 = sandbox.stub(overlayElement, 'offsetParent').get(() => 1); //not null and > 0 is needed   
    const w6 = sandbox.stub(triggerElement, 'offsetParent').get(() => 1);  
    const d1 = sandbox.stub(document.documentElement, 'clientHeight').get(() => testData.documentElement.clientHeight);
    const d2 = sandbox.stub(document.documentElement, 'clientWidth').get(() => testData.documentElement.clientWidth);
    let resizeCallback;
    let mutationCallback;
    const o1 = sandbox.stub(resizeObserver, 'create').callsFake((key, invoker, isDotNetInvoker: boolean) => {
      resizeCallback = invoker;
      return;
    });
    const o2 = sandbox.stub(mutationObserver, 'create').callsFake((key, invoker, isDotNetInvoker: boolean) => {
      mutationCallback = invoker;
      return;
    });      


    const s7 = sandbox.stub(infoHelper, "getInfo").callsFake((element: HTMLElement) => {      
      let elementId = element.getAttribute("id");
      if (!elementId && element === document.body) {
        elementId = "body";
      }

      return testData.getInfo[elementId];
    }); 

    //act
    let overlay = new Overlay("testId", overlayElement, container, triggerElement, testData.overlay.placement, testData.trigger.triggerBoundyAdjustMode, false, '', overlayConstraintsDefaults);
    overlay.calculatePosition(false, true);
    resizeCallback(null, null); //reset Overlay.duringInit;

    //change test data for stubs
    testData.window.innerHeight = testData.contentRect.height;
    testData.window.innerWidth = testData.contentRect.width;
    testData.getInfo["body"].scrollHeight = testData.contentRect.height;
    testData.getInfo["body"].scrollWidth = testData.contentRect.width;

    resizeCallback(testData.contentRect, null);
    //assert    
    if (testData.expectedCorrected.top !== null) {
      expect(overlay.sanitizedPosition.top).to.eq(testData.expectedCorrected.top);    
      expect(overlay.sanitizedPosition.bottom).to.eq(null);    
    }
    else {
      expect(overlay.sanitizedPosition.bottom).to.eq(testData.expectedCorrected.bottom);    
      expect(overlay.sanitizedPosition.top).to.eq(null);    
    }
    if (testData.expectedCorrected.left !== null) {
      expect(overlay.sanitizedPosition.left).to.eq(testData.expectedCorrected.left);
      expect(overlay.sanitizedPosition.right).to.eq(null);
    }
    else {
      expect(overlay.sanitizedPosition.right).to.eq(testData.expectedCorrected.right);
      expect(overlay.sanitizedPosition.left).to.eq(null);
    }
    expect(overlay.sanitizedPosition.placement).to.eq(testData.expectedCorrected.placement);
  });

  it("should not reposition when submenu's parent is hidden", () => {
    //arrange
    const triggerElement = domInit.addElementToBody('<div id="trigger">TriggerElement</div>');            
    const overlayElement = domInit.addElementToBody('<div id="overlay" style="position:absolute"></div>') as HTMLDivElement;
    const container = document.body;    

    //discard variable names    
    let offsetParent = 1;
    let inResize = false;
    const w1 = sandbox.stub(window, 'innerHeight').get(() => 955);
    const w2 = sandbox.stub(window, 'innerWidth').get(() => 1920);
    const w3 = sandbox.stub(window, 'pageXOffset').get(() => 0);
    const w4 = sandbox.stub(window, 'pageYOffset').get(() => 1020);
    const w5 = sandbox.stub(overlayElement, 'offsetParent').get(() => 1); //not null and > 0 is needed   
    const w6 = sandbox.stub(triggerElement, 'offsetParent').get(() => offsetParent);      
    const d1 = sandbox.stub(document.documentElement, 'clientHeight').get(() => 955);
    const d2 = sandbox.stub(document.documentElement, 'clientWidth').get(() => 1903);
    let resizeCallback;
    let mutationCallback;
    const o1 = sandbox.stub(resizeObserver, 'create').callsFake((key, invoker, isDotNetInvoker: boolean) => {
      resizeCallback = invoker;
      return;
    });
    const o2 = sandbox.stub(mutationObserver, 'create').callsFake((key, invoker, isDotNetInvoker: boolean) => {
      mutationCallback = invoker;
      return;
    });      
    let triggerInfo: domInfo = {
      ...domInfoDefaults,
      absoluteTop: 1577, absoluteLeft: 406, clientHeight: 32, clientWidth: 175, offsetHeight: 32, offsetWidth: 175      
    }

    const s7 = sandbox.stub(infoHelper, "getInfo").callsFake((element: HTMLElement) => {      
      let elementId = element.getAttribute("id");
      if (!elementId && element === document.body) {
        elementId = "body";
      }
      let domInfo = domInfoDefaults;
      switch (elementId) {
        case "body": 
          domInfo = {
            ...domInfoDefaults,
            absoluteTop: 0, absoluteLeft: 0, clientLeft: 0, clientTop: 0, scrollLeft: 0, scrollTop: 0, scrollHeight: 5560, scrollWidth: 1903
          }
          break;
        case "trigger":
          domInfo = triggerInfo;          
          break;
        case "overlay":
          domInfo = {
            ...domInfoDefaults,
            clientHeight: 80, clientWidth: 123
          }                
          break;
      }      
      return domInfo;
    }); 

    //act
    let overlay = new Overlay("testId", overlayElement, container, triggerElement, Placement.RightTop, TriggerBoundyAdjustMode.None, false, '', overlayConstraintsDefaults);
    overlay.calculatePosition(false, true);
    resizeCallback(null, null); //reset Overlay.duringInit;

    //change test data for stubs
    offsetParent = null;
    triggerInfo = {
      ...domInfoDefaults,
      absoluteTop: 1020
    };
    resizeCallback({height: 900, width: 1000}, null);
    //assert    
    expect(overlay.sanitizedPosition.top).to.eq(1577);        
    expect(overlay.sanitizedPosition.left).to.eq(585);    
    expect(overlay.sanitizedPosition.right).to.eq(null);    
    expect(overlay.sanitizedPosition.bottom).to.eq(null);    
    expect(overlay.sanitizedPosition.placement).to.eq(Placement.RightTop);    
  });  
});