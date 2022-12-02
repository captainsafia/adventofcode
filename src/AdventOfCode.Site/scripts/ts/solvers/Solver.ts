// Copyright (c) Martin Costello, 2015. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

import { ProblemDetails } from '../models/ProblemDetails';
import { Solution } from '../models/Solution';

export interface Solver {
    solve(inputs: string[], resource: string): Promise<ProblemDetails | Solution>;
}