/**
 * Provides checker for handlers that do not affect availablilty for selection
 */
export class AlwaysTrueAvailabilityChecker {
    static checker = (rows: any[]): boolean => true;
}
