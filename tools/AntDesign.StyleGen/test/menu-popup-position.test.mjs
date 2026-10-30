import assert from 'node:assert/strict';
import { readFileSync } from 'node:fs';
import vm from 'node:vm';
import test from 'node:test';
import ts from 'typescript';

function fixture() {
  const source = readFileSync(new URL('../../../components/core/JsInterop/modules/components/overlay.ts', import.meta.url), 'utf8');
  const script = ts.transpileModule(source, { compilerOptions: { target: ts.ScriptTarget.ES2020, module: ts.ModuleKind.CommonJS } }).outputText;
  const exported = {};
  const window = { innerWidth: 547, innerHeight: 900, pageYOffset: 0 };
  const scroll = { x: 0, y: 0 };
  const computedStyle = { width: '260.8671875px', height: '100px' };
  vm.runInNewContext(script, { exports: exported, window, getComputedStyle: () => computedStyle, require: name => {
    if (name === '../dom/exports') return { domInfoHelper: { getMaxZIndex: () => 1000, getWindow: () => window, getScroll: () => scroll,
      getBoundingClientRect: element => element.getBoundingClientRect?.() ?? null } };
    return {};
  } });
  const { Overlay, Placement, TriggerBoundyAdjustMode } = exported;
  const overlay = Object.create(Overlay.prototype);
  Object.assign(overlay, {
    overlay: { offsetParent: {}, classList: { contains: () => false }, getBoundingClientRect: () => ({ width: parseFloat(computedStyle.width), height: parseFloat(computedStyle.height) }) }, trigger: { offsetParent: {} },
    triggerPrefixCls: 'ant-menu-submenu', isContainerBody: true,
    boundyAdjustMode: TriggerBoundyAdjustMode.InView,
    initialPlacement: Placement.BottomRight, placement: Placement.BottomRight,
    selectedHorizontalPosition: 'right', selectedVerticalPosition: 'top',
    horizontalCalculation: Overlay.setHorizontalCalculation(Placement.BottomRight, 'right'),
    verticalCalculation: Overlay.setVerticalCalculation(Placement.BottomRight, 'top'),
    triggerInfo: { absoluteLeft:338.296875,absoluteTop:100,offsetWidth:46,clientWidth:46,offsetHeight:46,clientHeight:46 },
    position: {}, triggerPosition: {}, overlayInfo: { clientWidth:260.8671875,clientHeight:100 },
    containerInfo: { scrollWidth:547,scrollHeight:900,clientWidth:547,clientHeight:900,absoluteLeft:0,absoluteTop:0,clientLeft:0,clientTop:0,scrollLeft:0,scrollTop:0 },
    overlayConstraints: { horizontalOffset:4,verticalOffset:4,arrowPointAtCenter:false },
    scrollbarSize: { verticalWidth:0,horizontalHeight:0 },
    getKeyElementDimensions() {},
  });
  return { overlay, window, scroll, computedStyle, Placement, TriggerBoundyAdjustMode, Overlay };
}

test('BottomRight menu popup stays attached to the ellipsis after repeated position calculations', () => {
  const { overlay, window } = fixture();
  const expected = window.innerWidth - Math.floor(window.innerWidth - overlay.triggerInfo.absoluteLeft - overlay.triggerInfo.offsetWidth) - overlay.overlayInfo.clientWidth;
  for (let i=0;i<6;i++) {
    const result = overlay.calculatePosition(false, i === 0);
    const left = result.left ?? window.innerWidth - result.right - overlay.overlayInfo.clientWidth;
    assert.equal(left,expected,`position calculation ${i+1}`);
    assert.equal(result.top,150);
  }
});

test('built-in InView flips BottomRight to BottomLeft at the left viewport edge', () => {
  const { overlay, window, Placement } = fixture();
  overlay.triggerInfo.absoluteLeft = 10;
  for (let i=0;i<4;i++) {
    const result = overlay.calculatePosition(false,i===0);
    const left = result.left ?? window.innerWidth - result.right - overlay.overlayInfo.clientWidth;
    assert.equal(result.placement,Placement.BottomLeft);
    assert.equal(left,10);
  }
});

test('built-in BottomRight tracks resized anchors without a menu-specific offset', () => {
  const { overlay, window } = fixture();
  for (const [width,left] of [[547,338.296875],[600,400],[440,300],[547,338.296875]]) {
    window.innerWidth = width;
    overlay.containerInfo.scrollWidth = width;
    overlay.containerInfo.clientWidth = width;
    overlay.triggerInfo.absoluteLeft = left;
    const result = overlay.calculatePosition(false,false);
    const popupLeft = result.left ?? width - result.right - overlay.overlayInfo.clientWidth;
    assert.equal(popupLeft,width-Math.floor(width-left-46)-overlay.overlayInfo.clientWidth);
  }
});

test('Select alignment floors final offsets rather than rounding a fractional anchor first', () => {
  const { overlay, window, Placement, Overlay } = fixture();
  window.innerWidth = overlay.containerInfo.scrollWidth = overlay.containerInfo.clientWidth = 1280;
  overlay.initialPlacement = overlay.placement = Placement.BottomLeft;
  overlay.selectedHorizontalPosition = 'left';
  overlay.horizontalCalculation = Overlay.setHorizontalCalculation(Placement.BottomLeft, 'left');
  overlay.triggerInfo.absoluteLeft = 339.6640625;
  overlay.triggerInfo.absoluteTop = 592.53125;
  overlay.triggerInfo.offsetHeight = 32;
  overlay.triggerInfo.offsetWidth = 120;
  const result = overlay.calculatePosition(false, true);
  assert.equal(result.left, 339);
  assert.equal(result.top, 628);
  assert.equal(overlay.position.left, 339.6640625, 'unrounded measurement survives until final alignment');
});

test('centered Popover alignment uses rendered trigger and popup bounds before pixel snapping', () => {
  const { overlay, computedStyle, Placement, Overlay } = fixture();
  overlay.initialPlacement = overlay.placement = Placement.Top;
  overlay.selectedHorizontalPosition = 'left';
  overlay.horizontalCalculation = Overlay.setHorizontalCalculation(Placement.Top, 'left');
  overlay.triggerInfo.absoluteLeft = 339.65625;
  overlay.triggerInfo.offsetWidth = 96;
  overlay.triggerInfo.clientWidth = 96;
  overlay.trigger.getBoundingClientRect = () => ({ left: 339.65625, right: 435.296875, width: 95.640625, height: 32 });
  overlay.overlayInfo.clientWidth = 200;
  overlay.overlay.getBoundingClientRect = () => ({ width: 201, height: 100 });
  computedStyle.width = '201px';

  const result = overlay.calculatePosition(false, true);
  assert.equal(result.left, 286, 'uses the 201px rendered popup box rather than the truncated 200px client width');
});

test('centered tooltip alignment uses the untransformed popup size during enter motion', () => {
  const { overlay, computedStyle, Placement, Overlay } = fixture();
  overlay.initialPlacement = overlay.placement = Placement.Top;
  overlay.selectedHorizontalPosition = 'left';
  overlay.horizontalCalculation = Overlay.setHorizontalCalculation(Placement.Top, 'left');
  overlay.triggerInfo.absoluteLeft = 339.65625;
  overlay.trigger.getBoundingClientRect = () => ({ left: 339.65625, right: 561.90625, width: 222.25, height: 22 });
  overlay.overlay.getBoundingClientRect = () => ({ width: 74.75, height: 27.2 });
  computedStyle.width = '93.4375px';
  computedStyle.height = '34px';
  computedStyle.boxSizing = 'border-box';

  const result = overlay.calculatePosition(false, true);
  assert.equal(result.left, 404.0625, 'center alignment uses the final 93.4375px layout width rather than the animated 74.75px visual width');
});

test('scaled axes retain fractional offsets like rc-trigger', () => {
  const { overlay, computedStyle } = fixture();
  computedStyle.width = '130.43359375px';
  overlay.overlay.getBoundingClientRect = () => ({ width: 260.8671875, height: 100 });
  const result = overlay.calculatePosition(false, true);
  assert.equal(result.right, 547 - 338.296875 - 46);
  assert.equal(result.top, 150);
});

test('popup motion end recalculates using the stable placement policy', () => {
  const { overlay } = fixture();
  const calls = [];
  overlay.calculatePosition = (...args) => calls.push(args);
  overlay.onMotionEnd({ target: {} });
  assert.equal(calls.length, 0);
  overlay.onMotionEnd({ target: overlay.overlay });
  assert.deepEqual(calls, [[true, false, undefined]]);
  overlay.overlay.offsetParent = null;
  overlay.onMotionEnd({ target: overlay.overlay });
  assert.equal(calls.length, 1);
});

test('position recalculation keeps the initial pixel-snap decision through scale motion', () => {
  const { overlay, computedStyle } = fixture();
  overlay.selectedHorizontalPosition = 'left';
  overlay.selectedVerticalPosition = 'top';
  overlay.position = { left: 339.65625, top: 469.828125, right: 0, bottom: 0 };
  overlay.overlay.getBoundingClientRect = () => ({ width: 200, height: 100 });
  computedStyle.width = '200px';
  computedStyle.height = '100px';

  overlay.sanitizeCalculatedPositions();
  const initial = { ...overlay.sanitizedPosition };
  assert.equal(initial.left, 339);
  assert.equal(initial.top, 469);

  // Enter motion temporarily scales the visual box. The alignment policy must
  // remain the one selected for the unanimated first layout.
  overlay.overlay.getBoundingClientRect = () => ({ width: 180, height: 90 });
  overlay.position = { left: 339.65625, top: 469.828125, right: 0, bottom: 0 };
  overlay.sanitizeCalculatedPositions();
  assert.equal(JSON.stringify(overlay.sanitizedPosition), JSON.stringify(initial));

  overlay.overlay.getBoundingClientRect = () => ({ width: 200, height: 100 });
  overlay.position = { left: 339.65625, top: 469.828125, right: 0, bottom: 0 };
  overlay.sanitizeCalculatedPositions();
  assert.equal(JSON.stringify(overlay.sanitizedPosition), JSON.stringify(initial), 'repeated calculations are idempotent after animation');
});

test('a replacement Overlay created during enter motion keeps the pre-motion pixel alignment', () => {
  const { overlay, computedStyle } = fixture();
  overlay.selectedHorizontalPosition = 'left';
  overlay.selectedVerticalPosition = 'top';
  overlay.overlay.getBoundingClientRect = () => ({ width: 180, height: 90 });
  overlay.overlay.getAnimations = () => [{ playState: 'running' }];
  computedStyle.width = '200px';
  computedStyle.height = '100px';
  overlay.position = { left: 339.65625, top: 469.828125, right: 0, bottom: 0 };

  overlay.sanitizeCalculatedPositions();
  const duringMotion = JSON.stringify(overlay.sanitizedPosition);
  assert.equal(overlay.sanitizedPosition.left, 339);
  assert.equal(overlay.sanitizedPosition.top, 469);

  overlay.overlay.getAnimations = () => [];
  overlay.overlay.getBoundingClientRect = () => ({ width: 200, height: 100 });
  overlay.sanitizeCalculatedPositions();
  assert.equal(JSON.stringify(overlay.sanitizedPosition), duringMotion, 'motion end must not move the replacement overlay');
});

test('popup resize realigns even when the initial observer delivery is being ignored', () => {
  const { overlay } = fixture();
  const calls = [];
  overlay.duringInit = true;
  overlay.calculatePosition = (...args) => calls.push(args);
  overlay.resizing([{ target: overlay.overlay }], {});
  assert.deepEqual(calls, [[true, false, undefined]]);
});

test('visible overlays realign when layout moves the trigger without resizing it', () => {
  const { overlay, window } = fixture();
  let nextFrame;
  const triggerRect = { left: 20, top: 40, width: 100, height: 24 };
  const calls = [];
  overlay.trigger.getBoundingClientRect = () => triggerRect;
  overlay.overlay.isConnected = true;
  overlay.lastTriggerClientRect = { ...triggerRect };
  overlay.calculatePosition = (...args) => calls.push(args);
  window.requestAnimationFrame = callback => {
    nextFrame = callback;
    return 1;
  };

  overlay.monitorTriggerPosition();
  triggerRect.top += 80;
  nextFrame();

  assert.deepEqual(calls, [[true, false, undefined]]);
  assert.equal(overlay.lastTriggerClientRect.left, triggerRect.left);
  assert.equal(overlay.lastTriggerClientRect.top, triggerRect.top);
  assert.equal(overlay.lastTriggerClientRect.width, triggerRect.width);
  assert.equal(overlay.lastTriggerClientRect.height, triggerRect.height);
});

test('Overlay InView contains a popup when neither horizontal direction can fully fit', () => {
  const { overlay, window, Placement, Overlay } = fixture();
  window.innerWidth=390;
  overlay.containerInfo.scrollWidth=390;
  overlay.containerInfo.clientWidth=390;
  overlay.initialPlacement=overlay.placement=Placement.RightTop;
  overlay.selectedHorizontalPosition='left';
  overlay.horizontalCalculation=Overlay.setHorizontalCalculation(Placement.RightTop,'left');
  overlay.verticalCalculation=Overlay.setVerticalCalculation(Placement.RightTop,'top');
  overlay.triggerInfo.absoluteLeft=41;
  overlay.triggerInfo.offsetWidth=overlay.triggerInfo.clientWidth=256;
  overlay.overlayInfo.clientWidth=256;
  for(let i=0;i<4;i++) {
    const result=overlay.calculatePosition(false,i===0);
    const left=result.left??390-result.right-256;
    assert.ok(left>=0&&left+256<=390);
  }
});

for (const mode of ['None', 'InScroll']) {
  test(`Overlay ${mode} preserves its existing horizontal placement policy`, () => {
    const { overlay, window, Placement, TriggerBoundyAdjustMode, Overlay } = fixture();
    window.innerWidth = overlay.containerInfo.scrollWidth = overlay.containerInfo.clientWidth = 390;
    overlay.boundyAdjustMode = TriggerBoundyAdjustMode[mode];
    overlay.initialPlacement = overlay.placement = Placement.RightTop;
    overlay.selectedHorizontalPosition = 'left';
    overlay.horizontalCalculation = Overlay.setHorizontalCalculation(Placement.RightTop, 'left');
    overlay.verticalCalculation = Overlay.setVerticalCalculation(Placement.RightTop, 'top');
    overlay.triggerInfo.absoluteLeft = 41;
    overlay.triggerInfo.offsetWidth = overlay.triggerInfo.clientWidth = 256;
    overlay.overlayInfo.clientWidth = 256;
    const result = overlay.calculatePosition(false, true);
    const left = result.left ?? 390 - result.right - 256;
    assert.ok(left < 0 || left + 256 > 390, 'non-InView modes do not shift into the viewport');
  });
}

test('Overlay InView uses the scrolled viewport rather than document origin', () => {
  const { overlay, window, scroll, Placement } = fixture();
  scroll.x = 200;
  window.innerWidth = 390;
  overlay.containerInfo.scrollWidth = 900;
  overlay.containerInfo.clientWidth = 390;
  overlay.triggerInfo.absoluteLeft = 210;
  overlay.triggerInfo.offsetWidth = overlay.triggerInfo.clientWidth = 360;
  overlay.overlayInfo.clientWidth = 300;
  for (let i = 0; i < 4; i++) {
    const result = overlay.calculatePosition(false, i === 0);
    const left = result.left ?? window.innerWidth - result.right - 300;
    assert.equal(result.placement, Placement.BottomRight);
    assert.ok(left >= scroll.x && left + 300 <= scroll.x + window.innerWidth);
  }
});

test('Overlay InView contains centered popups at either viewport edge', () => {
  for (const anchor of [0, 360]) {
    const { overlay, window, Placement, Overlay } = fixture();
    window.innerWidth = overlay.containerInfo.scrollWidth = overlay.containerInfo.clientWidth = 390;
    overlay.initialPlacement = overlay.placement = Placement.Bottom;
    overlay.selectedHorizontalPosition = 'left';
    overlay.horizontalCalculation = Overlay.setHorizontalCalculation(Placement.Bottom, 'left');
    overlay.triggerInfo.absoluteLeft = anchor;
    overlay.triggerInfo.offsetWidth = overlay.triggerInfo.clientWidth = 30;
    overlay.overlayInfo.clientWidth = 300;
    const result = overlay.calculatePosition(false, true);
    const left = result.left ?? 390 - result.right - 300;
    assert.ok(left >= 0 && left + 300 <= 390);
  }
});
