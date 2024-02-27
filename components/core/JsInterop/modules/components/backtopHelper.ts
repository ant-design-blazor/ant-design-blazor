﻿import { domInfoHelper, domManipulationHelper } from '../dom/exports'

export class backtopHelper {
  static backTop(target: string) {
    const dom = domInfoHelper.get(target);
    if (dom) {
      domManipulationHelper.slideTo(dom.scrollTop);
    } else {
      domManipulationHelper.slideTo(0);
    }
  }
}