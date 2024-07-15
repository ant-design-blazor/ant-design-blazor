export type domInfo = {
  offsetTop: number,
  offsetLeft: number,
  offsetWidth: number,
  offsetHeight: number,
  scrollHeight: number,
  scrollWidth: number,
  scrollLeft: number,
  scrollTop: number,
  clientTop: number,
  clientLeft: number,
  clientHeight: number,
  clientWidth: number,
  selectionStart: number,
  absoluteTop: number,
  absoluteLeft: number,
  marginTop: number,
  marginBottom: number,
  marginLeft: number,
  marginRight: number,
}

export type position = {
  x: number,
  y: number
}

export type domRect = {
  width?: number,
  height?: number,
  top?: number,
  right?: number,
  bottom?: number,
  left?: number,
  x?: number,
  y?: number
}

export type boxSize = {
  blockSize?: number
  inlineSize?: number
}

export type eventCallback = (Event) => void;