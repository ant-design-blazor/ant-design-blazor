export class borderBeamHelper {
  static update(beam: HTMLElement): void {
    const host = beam?.parentElement;
    if (!host) return;
    const style = getComputedStyle(host);
    const inset = [style.borderTopWidth, style.borderRightWidth, style.borderBottomWidth, style.borderLeftWidth]
      .map(value => `${-(parseFloat(value) || 0)}px`).join(' ');
    // No DOM reparenting: the host owns the decoration in Blazor's render tree.
    Array.from(host.children).forEach(child => {
      if (child instanceof HTMLElement && child.classList.contains('ant-border-beam'))
        child.style.setProperty('--ant-border-beam-inset-offset', inset);
    });
  }
}
