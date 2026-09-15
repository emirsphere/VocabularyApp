/**
 * ProgressBar Component
 * Subtle linear progress indicator
 */

import React from 'react';
import { View, StyleSheet, ViewStyle } from 'react-native';
import { colors, spacing, borderRadius } from '@/constants/designTokens';

interface ProgressBarProps {
  percentage: number; // 0-100
  style?: ViewStyle;
  testID?: string;
}

export function ProgressBar({ percentage = 0, style, testID }: ProgressBarProps) {
  const clampedPercentage = Math.max(0, Math.min(100, percentage));

  return (
    <View style={[styles.container, style]} testID={testID}>
      <View
        style={[
          styles.fill,
          {
            width: `${clampedPercentage}%`,
          },
        ]}
      />
    </View>
  );
}

const styles = StyleSheet.create({
  container: {
    width: '100%',
    height: 4,
    backgroundColor: colors.neutral[200],
    borderRadius: borderRadius.sm,
    overflow: 'hidden',
  },
  fill: {
    height: '100%',
    backgroundColor: colors.primary[500],
    borderRadius: borderRadius.sm,
  },
});
