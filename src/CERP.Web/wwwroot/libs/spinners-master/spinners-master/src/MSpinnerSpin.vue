<template>
    <i class="m-spinner--spin"></i>
</template>

<script>
export default {
    name: 'm-spinner-spin'
}
</script>

<style lang="scss">
.m-spinner--spin {
    --spinner-color: var(--color, black);
    --spinner-color-secondary: var(--color-secondary ,currentColor);
    --spinner-duration: 6s;
    --spinner-size: 64px;

    display: inline-block;
    color: var(--spinner-color);
    font-size: var(--spinner-size);
    position: relative;
    width: 1em;
    height: 1em;
    max-width: 1em;
    max-height: 1em;
    border-radius: 50%;
    background-image: radial-gradient(circle at center, transparent 52%, currentColor 54%, currentColor 65%, transparent 67%);
    background-repeat: no-repeat;

    animation-name: m-spinner-spin;
    animation-timing-function: linear;
    animation-duration: var(--spinner-duration);
    animation-delay: -500ms;
    animation-iteration-count: infinite;
}

@function pow($number, $exponent) {
    $value: 1;

    @if $exponent > 0 {
        @for $i from 1 through $exponent {
            $value: $value * $number;
        }
    }

    @return $value;
}

@keyframes m-spinner-spin {
    // Cubic-bezier: http://cubic-bezier.com/#.35,0,.25,1
    // https://docs.google.com/spreadsheets/d/1xbluxRx6d6FM6atu8ot-txJ6Rbef3p1sd-zargRsfU8/edit#gid=0
    $a: 0;
    $b: 0.35;
    $c: 0.25;
    $d: 1;
    
    // Polygon transitions
    $polygon: (
        0: polygon(50% 50%, 83% 12%, 100% 43%, 94% 78%, 68% 100%, 32% 100%, 6% 78%, 0% 43%, 17% 12%, 33.5% 6%),
        1: polygon(50% 50%, 91.5% 27.5%, 100% 43%, 94% 78%, 68% 100%, 32% 100%, 6% 78%, 0% 43%, 17% 12%, 33.5% 6%),
        2: polygon(50% 50%, 100% 43%, 100% 43%, 94% 78%, 68% 100%, 32% 100%, 6% 78%, 0% 43%, 17% 12%, 33.5% 6%),
        3: polygon(50% 50%, 97% 60.5%, 97% 60.5%, 94% 78%, 68% 100%, 32% 100%, 6% 78%, 0% 43%, 17% 12%, 33.5% 6%),
        4: polygon(50% 50%, 94% 78%, 94% 78%, 94% 78%, 68% 100%, 32% 100%, 6% 78%, 0% 43%, 17% 12%, 33.5% 6%),
        5: polygon(50% 50%, 81% 89%, 81% 89%, 81% 89%, 68% 100%, 32% 100%, 6% 78%, 0% 43%, 17% 12%, 33.5% 6%),
        6: polygon(50% 50%, 68% 100%, 68% 100%, 68% 100%, 68% 100%, 32% 100%, 6% 78%, 0% 43%, 17% 12%, 33.5% 6%),
        7: polygon(50% 50%, 50% 100%, 50% 100%, 50% 100%, 50% 100%, 32% 100%, 6% 78%, 0% 43%, 17% 12%, 33.5% 6%),
        8: polygon(50% 50%, 32% 100%, 32% 100%, 32% 100%, 32% 100%, 32% 100%, 6% 78%, 0% 43%, 17% 12%, 33.5% 6%),
        9: polygon(50% 50%, 19% 89%, 19% 89%, 19% 89%, 19% 89%, 19% 89%, 6% 78%, 0% 43%, 17% 12%, 33.5% 6%),
        10: polygon(50% 50%, 6% 78%, 6% 78%, 6% 78%, 6% 78%, 6% 78%, 6% 78%, 0% 43%, 17% 12%, 33.5% 6%),
        11: polygon(50% 50%, 3% 60.5%, 3% 60.5%, 3% 60.5%, 3% 60.5%, 3% 60.5%, 3% 60.5%, 0% 43%, 17% 12%, 33.5% 6%),
        12: polygon(50% 50%, 0% 43%, 0% 43%, 0% 43%, 0% 43%, 0% 43%, 0% 43%, 0% 43%, 17% 12%, 33.5% 6%),
        13: polygon(50% 50%, 8.5% 27.5%, 8.5% 27.5%, 8.5% 27.5%, 8.5% 27.5%, 8.5% 27.5%, 8.5% 27.5%, 8.5% 27.5%, 17% 12%, 33.5% 6%),
        14: polygon(50% 50%, 17% 12%, 17% 12%, 17% 12%, 17% 12%, 17% 12%, 17% 12%, 17% 12%, 17% 12%, 33.5% 6%),
        15: polygon(50% 50%, 8.5% 27.5%, 8.5% 27.5%, 8.5% 27.5%, 8.5% 27.5%, 8.5% 27.5%, 8.5% 27.5%, 8.5% 27.5%, 17% 12%, 33.5% 6%),
        16: polygon(50% 50%, 0% 43%, 0% 43%, 0% 43%, 0% 43%, 0% 43%, 0% 43%, 0% 43%, 17% 12%, 33.5% 6%),
        17: polygon(50% 50%, 3% 60.5%, 3% 60.5%, 3% 60.5%, 3% 60.5%, 3% 60.5%, 3% 60.5%, 0% 43%, 17% 12%, 33.5% 6%),
        18: polygon(50% 50%, 6% 78%, 6% 78%, 6% 78%, 6% 78%, 6% 78%, 6% 78%, 0% 43%, 17% 12%, 33.5% 6%),
        19: polygon(50% 50%, 19% 89%, 19% 89%, 19% 89%, 19% 89%, 19% 89%, 6% 78%, 0% 43%, 17% 12%, 33.5% 6%),
        20: polygon(50% 50%, 32% 100%, 32% 100%, 32% 100%, 32% 100%, 32% 100%, 6% 78%, 0% 43%, 17% 12%, 33.5% 6%),
        21: polygon(50% 50%, 50% 100%, 50% 100%, 50% 100%, 50% 100%, 32% 100%, 6% 78%, 0% 43%, 17% 12%, 33.5% 6%),
        22: polygon(50% 50%, 68% 100%, 68% 100%, 68% 100%, 68% 100%, 32% 100%, 6% 78%, 0% 43%, 17% 12%, 33.5% 6%),
        23: polygon(50% 50%, 81% 89%, 81% 89%, 81% 89%, 68% 100%, 32% 100%, 6% 78%, 0% 43%, 17% 12%, 33.5% 6%),
        24: polygon(50% 50%, 94% 78%, 94% 78%, 94% 78%, 68% 100%, 32% 100%, 6% 78%, 0% 43%, 17% 12%, 33.5% 6%),
        25: polygon(50% 50%, 97% 60.5%, 97% 60.5%, 94% 78%, 68% 100%, 32% 100%, 6% 78%, 0% 43%, 17% 12%, 33.5% 6%),
        26: polygon(50% 50%, 100% 43%, 100% 43%, 94% 78%, 68% 100%, 32% 100%, 6% 78%, 0% 43%, 17% 12%, 33.5% 6%),
        27: polygon(50% 50%, 91.5% 27.5%, 100% 43%, 94% 78%, 68% 100%, 32% 100%, 6% 78%, 0% 43%, 17% 12%, 33.5% 6%),
        28: polygon(50% 50%, 83% 12%, 100% 43%, 94% 78%, 68% 100%, 32% 100%, 6% 78%, 0% 43%, 17% 12%, 33.5% 6%)
    );

    $t: 4;
    $offsetDeg: 630;
    $offsetRotations: 2.5;
    @for $i from 0 through ($t - 1) { 
        @for $p from 0 through 28 /* through 28 polygon transitions */ {
            $x: ((100/28) * if($p <= 14, $p, $p - 14)) * 0.02;
            $px: ((pow(1-$x, 3) * $a) + (3 * $x * pow(1-$x, 2) * $b) + (3 * pow($x, 2) * (1-$x) * $c) + (pow($x, 3) * $d)) * 50;
            $key: if($p <= 14, $px, 50 + $px);

            #{$key/$t + ($i * 100/$t) * 1%} {
                clip-path: #{map-get($polygon, $p)};
                transform: rotate(#{if($p <= 14, ($i * $offsetDeg) + (360/28) * $p, ($i * $offsetDeg) + ((360/28)*14) + (((360/28) * ($p - 14)) * $offsetRotations)) * 1deg});
            }
        }
    }
}
</style>
