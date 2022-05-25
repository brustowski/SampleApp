import { AlwaysTrueAvailabilityChecker } from './always-true-availability-checker';

export class GridDataHandler<T> {
    constructor(public name: string,
        private func: (data: T) => void,
        private checkFunc: (data: T[]) => boolean = AlwaysTrueAvailabilityChecker.checker) {
    }
    exec(data: T): void {
        try {
            this.func(data);
        } catch (error) {
            console.error(`An error occurred when handler was working. Handler name: ${this.name}.`, data, error);
        }
    }

    check(data: T[]): boolean {
        return this.checkFunc(data);
    }
}

export class GridDataHandlerCollection<T> {
    private handlers: GridDataHandler<T>[] = [];

    get Count(): number {
        return this.handlers.length;
    }

    clear(): void {
        this.handlers = [];
    }

    add(handler: GridDataHandler<T>): boolean {
        if (!this.handlers.some(h => h.name.toLowerCase() === handler.name.toLowerCase())) {
            this.handlers.push(handler);
            return true;
        }
        return false;
    }

    delete(name: string): boolean {
        const indx = this.handlers.findIndex(h => h.name.toLowerCase() === name.toLowerCase());
        if (indx > -1) {
            this.handlers.splice(indx, 1);
            return true;
        }
        return false;
    }

    contains(name: string): boolean {
        return (this.handlers.findIndex(h => h.name.toLowerCase() === name.toLowerCase()) > -1);
    }

    handle(data: any): void {
        this.handlers.forEach(handler => handler.exec(data));
    }

    handleMultiple(rows: any[]): void {
        if (this.Count) {
            rows.forEach(row => {
                this.handle(row);
            });
        }
    }

    checkMultiple(rows: any[]): boolean {
        let allIsOk = true;
        let i = 0;
        while (allIsOk && i < this.Count) {
            const handler = this.handlers[i++];
            const result = handler.check(rows);
            if (!result) {
                allIsOk = false;
            }
        }
        return allIsOk;
    }
}
