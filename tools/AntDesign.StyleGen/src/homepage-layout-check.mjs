import { readFileSync } from 'node:fs';
import { resolve } from 'node:path';
import { pathToFileURL } from 'node:url';

/** Layout invariants for browser-collected homepage snapshots, not a DOM/CSS parity claim. */
export function checkHomepageLayout(observations, tolerance = 1) {
  if (!Array.isArray(observations) || observations.length === 0)
    throw new Error('At least one browser observation is required.');
  if (!Number.isFinite(tolerance) || tolerance < 0) throw new Error('Invalid layout tolerance.');
  const checks = observations.map(observation => {
    const errors = [];
    const number = (value, name) => {
      if (!Number.isFinite(value)) { errors.push(`Missing/invalid ${name}`); return false; }
      return true;
    };
    const { width, requestedWidth, clientWidth, scrollWidth, header, sections, actions, crowded } = observation;
    if (number(requestedWidth, 'requestedWidth') && number(width, 'width') && width !== requestedWidth)
      errors.push(`Wrong viewport: requested ${requestedWidth}, observed ${width}`);
    if (number(clientWidth, 'clientWidth') && clientWidth <= 0) errors.push('Empty document viewport');
    if (number(scrollWidth, 'scrollWidth') && scrollWidth > clientWidth + tolerance)
      errors.push(`Document overflows by ${scrollWidth - clientWidth}px`);
    if (number(header?.clientWidth, 'header.clientWidth') && number(header?.scrollWidth, 'header.scrollWidth')
      && header.scrollWidth > header.clientWidth + tolerance)
      errors.push(`Header overflows by ${header.scrollWidth - header.clientWidth}px`);
    if (observation.navigationVisible !== true) errors.push('No visible header navigation');
    if (observation.navigationClipped === true) errors.push('Header navigation items are clipped');
    if (!Array.isArray(sections) || sections.length !== 3) errors.push('Expected all three full-width homepage sections');
    for (const section of sections ?? []) {
      if (number(section.x, 'section.x') && number(section.right, 'section.right')
        && (section.x < -tolerance || section.right > clientWidth + tolerance))
        errors.push(`Section escapes document viewport: ${section.name}`);
    }
    if (!Array.isArray(actions) || actions.length !== 2) errors.push('Expected both hero action buttons');
    for (const action of actions ?? []) {
      if (number(action.x, 'action.x') && number(action.right, 'action.right')
        && (action.x < -tolerance || action.right > clientWidth + tolerance))
        errors.push('Hero action is clipped outside the document viewport');
    }
    if (crowded && number(crowded.top, 'crowded.top') && number(crowded.navTop, 'crowded.navTop')
      && Math.abs(crowded.top - crowded.navTop) > tolerance)
      errors.push('Additional navigation wrapped onto a second row');
    return { width, locale: observation.locale, passed: errors.length === 0, errors };
  });
  return { scope: 'homepage-layout-only', passed: checks.every(check => check.passed), checks };
}

if (process.argv[1] && import.meta.url === pathToFileURL(resolve(process.argv[1])).href) {
  if (!process.argv[2]) throw new Error('Usage: node homepage-layout-check.mjs <browser-observations.json>');
  const input = JSON.parse(readFileSync(process.argv[2], 'utf8'));
  const report = checkHomepageLayout(Array.isArray(input) ? input : input.observations);
  console.log(JSON.stringify(report, null, 2));
  if (!report.passed) process.exitCode = 1;
}
